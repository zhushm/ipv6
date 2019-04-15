using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//文件读写添加
using System.IO;
//网络通信
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace Multipath6
{
    public partial class FileTransfer : Form
    {
        public FileTransfer()
        {
            InitializeComponent();
            //解决线程间操作无效问题
            CheckForIllegalCrossThreadCalls = false;
        }
       
        public int SendLength = 32768; //发送块长度：一次发32KB(32*1024=32768字节) ，实际发送长度为：SendLength+4 个字节
        public int ReciveSize = 1024*1024;  //至多1,000,000个数据块,文件最大32GB
        public int BufferSize = 1000000000;//发送和接收缓冲区大小：1G

        public string strConnection;//连接提示信息
        public DateTime dt=new DateTime();//用于计算时间戳
        public int recvNum1, recvNum2;//接收的数据块数量
        public string flag1, flag2; //接收结束标志#
        //
        public class Msg : IComparable
        {
            public int Num { get; set; }
            public string Flag { get; set; }
            public byte[] Content { get; set; }
            /// <summary>
            /// 实现IComparable接口，用Num做比较
            /// </summary>
            /// <param name="obj">比较对象</param>
            /// <returns>比较结果</returns>
            public int CompareTo(object obj)
            {
                if (obj is Msg)
                {
                    return Num.CompareTo(((Msg)obj).Num);
                }
               return 1;
            }
        }
        public Msg[] totalMessages, Messages1, Messages2;//消息数组
          

        private void FileTransfer_Load(object sender, EventArgs e)
        {
            //窗体启动时间
            dt = DateTime.Now;
            //两种send-receive方法的时间差为：(t2-t1)-(t4-t3)=(t3-t1）-(t4-t2),若时间差大于0，则两条路径优于一条路径
        }

        //开始监听（单地址）
        private void btnStart_Click(object sender, EventArgs e)
        {
            string myIP = txtRecvIP.Text.Trim(); //目标IPv6地址
            //Socket mySocket; 
            Socket mySocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);//ipv6
            int myPort = int.Parse(txtRecvPort.Text.Trim());//端口，假设为11110
            if ( myIP == "" || myIP == null || myPort==0)
                return;
            //初始化Socket参数
            try
            {
                IPAddress IPAddr6 = IPAddress.Parse(myIP); //装换IPv6地址
                IPEndPoint ep = new IPEndPoint(IPAddr6, myPort); //端口，假设为11110

                //mySocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);//ipv6
                //设置发送缓冲区
                mySocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, BufferSize);

                mySocket.Bind(ep);
                mySocket.Listen(10000);//指定侦听队列容量10000个
                //Accept（）：接收连接并返回一个新的Socket; Receive():从Socket中读取数据
                Thread thread = new Thread(AcceptInfo);
                thread.IsBackground = true;
                thread.Start(mySocket);


                //


            }
            catch (System.Exception err)
            {
                MessageBox.Show(err.ToString(), "socket exception!");
                return;
            }//结束socket初始化
            rState.Text = "正在监听...";
            rState.ForeColor = Color.Red;
        }

        //zhu:记录通信用的Socket
        Dictionary<string, Socket> dic = new Dictionary<string, Socket>();//Dictionary字典里面的每一个元素都是一个键值对(由二个元素组成：键和值) 
        void AcceptInfo(object o)
        {
            Socket socket = o as Socket;
            while (true)
            {
                try
                {
                    Socket tSocket = socket.Accept();//创建通信用的Socket

                    string point = tSocket.RemoteEndPoint.ToString();
                    //MessageBox.Show(point, "连接成功！");
                    rtbConnection.AppendText(point+ "连接成功！\n");
                    dic.Add(point, tSocket);
                    //接收消息
                    Thread th = new Thread(ReceiveMsg);
                    th.IsBackground = true;
                    th.Start(tSocket);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "AcceptInfo error！");break;
                }
            }

          }

        //接收消息
        void ReceiveMsg(object o)
        {
            Socket client = o as Socket;
            int i = 0;//数据块数
            int blockNum;
            string flag;
            string fileName = txtRecvFileName.Text.Trim();
            Msg[] Messages = new Msg[ReciveSize];//至多1024*1024个数据块,文件最大32GB
            if (fileName != "" && fileName != null)
            {
                try
                {
                    FileStream fs = new FileStream(fileName, FileMode.Create);
                    while (true)
                    {
                        //接收客户端发送过来的数据
                        try
                        {
                            //定义byte数组存放从客户端接收过来的数据
                            //int SendLength = 32768; //发送数据块块长度：一次发32KB(32*1024字节) 
                            byte[] buffer = new byte[SendLength + 5];//前面4个字节存放块号n ，第5个字节存放块结束标志0xFF
                                                                     //将接收过来的数据放到buffer中，并返回实际接受数据的长度
                            byte[] intbuffer = new byte[4];//数据块数组
                            int n = client.Receive(buffer);//接收字节长度
                                                           //
                            if (n > 5)
                            {
                                //提取首4个字节：块号
                                Buffer.BlockCopy(buffer, 0, intbuffer, 0, 4);
                                blockNum = BitConverter.ToInt32(intbuffer, 0);
                                //提取第5位：标志位
                                flag = buffer[4].ToString("X2");  //字节转16进制字符

                                //写入文件，从第6位开始
                                //fs.Write(buffer, 5, n-5);
                                //写入messages
                                Messages[i] = new Msg();
                                Messages[i].Num = blockNum;
                                Messages[i].Flag = flag;
                                Messages[i].Content = new byte[n - 5];
                                Buffer.BlockCopy(buffer, 5, Messages[i].Content, 0, n - 5);
                                i++;
                                Thread.Sleep(50);//暂停50ms
                            }
                            else //如果返回一个0，即读取完了 
                            {

                                Array.Sort(Messages); //按数据块序号排序
                                                      //写入文件
                                foreach (Msg m in Messages)
                                {
                                    if (m != null)
                                    {
                                        //MessageBox.Show(m.Num.ToString(), m.Flag);
                                        fs.Write(m.Content, 0, m.Content.Length);//写入文件
                                        if (m.Flag == "FF") //最后一个块
                                            break;
                                    }
                                }
                                //清空缓冲区、关闭流
                                fs.Flush();
                                fs.Close();
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString(), "ReceiveMsg error！"); break;
                        }

                    }
                    TimeSpan ts = DateTime.Now - dt;
                    t3.Text = Convert.ToInt64(ts.TotalMilliseconds).ToString(); //（毫秒） 
                                                                                //MessageBox.Show(i.ToString(),"接收的数据块数量");
                    rtbConnection.AppendText("接收了" + i.ToString() + "个数据块。\n");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "文件目录不存在！");
                }
            }
        }


        //清空接收框
        private void btnClear_Click(object sender, EventArgs e)
        {
            rtbConnection.Text = "";
            sState.Text = "等待发送...";
            sState.ForeColor = Color.Black;
            //???
            //Student[] students = new Student[]{
            //    new Student(){Age = 10,Name="张三",Score=70},
            //    new Student(){Age = 4,Name="李四",Score=97},
            //    new Student(){Age = 11,Name="王五",Score=80},
            //    new Student(){Age = 9,Name="赵六",Score=66},
            //    new Student(){Age = 12,Name="司马",Score=90},
            //};
            //Array.Sort(students);
            //Array.ForEach<Student>(students, (s) => Console.WriteLine(string.Format("{0}{1,2}岁了，他的分数是{2,3}", s.Name, s.Age, s.Score)));
            //???

        }
        
        //切换到Routing6
        private void btnRouting_Click(object sender, EventArgs e)
        {
            Hide();
            //Routing6 form1 = new Routing6();
            //form1.ShowDialog();
            this.Close();
        }

        //选择一个文件
        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "所有文件(*.*)|*.*";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                txtSendFileName.Text = fileDialog.FileName;
            }
        }

        //分块单地址发送
        private void btnSendFile_Click(object sender, EventArgs e)
        {

            sState.Text = "正在发送...";
            string fileName = txtSendFileName.Text.Trim(); //文件名
            string myIP = txtSendIP.Text.Trim(); //目标IPv6地址
            int myPort = int.Parse(txtSendPort.Text.Trim());//端口，假设为11110
            Socket mySocket; //套接字
            if (fileName == "" || fileName == null || myIP == "" || myIP == null || myPort==0 )
                return;
            //初始化Socket参数
            try
            {
                IPAddress IPAddr6 = IPAddress.Parse(myIP); //装换IPv6地址
                IPEndPoint ep = new IPEndPoint(IPAddr6, myPort); 
                mySocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);//ipv6
                //设置发送缓冲区
                mySocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, BufferSize);
                //发起连接
                mySocket.Connect(ep);
            }
            catch (System.Exception err)
            {
                MessageBox.Show(err.ToString(), "socket exception!");
                return;
            }//结束socket初始化

            //打开文件
            try
            {
                FileStream sFile = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                long streamLen = sFile.Length; //以字节数计算的长度
                //计算发送块的大小
                long blockNumber =streamLen / SendLength; //发送块的数量
                int m=(int) (streamLen %  SendLength); //取其余数
                if (m > 0)
                    blockNumber += 1;
                //开始发送
                int i;
                //因为文件可能会比较大，通过一个循环分块去读取,每次读取SendLength大小，
                //最后4个字节存放块号n,最后1个字节放结束标志0xFF  
                byte[] buffer = new byte[SendLength + 5];
                byte[] intBuffer = new byte[4];
                for (i = 1; i <= blockNumber; i++)
                {
                    //***
                    if (i == blockNumber && m == 0) //最后一个块,且是整块
                    {
                        //块计数+1，并存储到首4个字节
                        intBuffer = BitConverter.GetBytes(i);     // 将 int 转换成字节数组
                        Buffer.BlockCopy(intBuffer, 0, buffer, 0, 4);
                        //第5位为标志位
                        buffer[4] = (byte)(0xFF); //255
                        //返回本次读取实际读取到的字节数,存储到第6位开始的位置  
                        int r = sFile.Read(buffer, 5, SendLength);

                        //发送
                        try
                        {
                            int iRet = mySocket.Send(buffer, SendLength + 5, 0);
                            if (iRet <= 0) return;//小于0表示出错,等于0表示断开;
                        }
                        catch (System.Exception err)
                        {
                            MessageBox.Show(err.ToString(), "send exception!");
                            return;
                        }
                        Array.Clear(buffer, 0, SendLength + 5);
                    }
                    else if (i == blockNumber && m > 0) //最后一个非整块
                    {
                        //块计数+1，并存储到首4个字节
                        intBuffer = BitConverter.GetBytes(i);     // 将 int 转换成字节数组
                        Buffer.BlockCopy(intBuffer, 0, buffer, 0, 4);
                        //第5位为标志位
                        buffer[4] = (byte)(0xFF); //255
                        //返回本次读取实际读取到的字节数,存储到第6位开始的位置  
                        int r = sFile.Read(buffer, 5, m);

                        //发送
                        try
                        {
                            int iRet = mySocket.Send(buffer, m + 5, 0);
                            if (iRet <= 0) return;//小于0表示出错,等于0表示断开;
                        }
                        catch (System.Exception err)
                        {
                            MessageBox.Show(err.ToString(), "send exception!");
                            return;
                        }
                        Array.Clear(buffer, 0, SendLength + 5);

                    }
                    else
                    {
                        //块计数+1，并存储到首4个字节
                        intBuffer = BitConverter.GetBytes(i);     // 将 int 转换成字节数组
                        Buffer.BlockCopy(intBuffer, 0, buffer, 0, 4);
                        //第5位为标志位
                        buffer[4] = (byte)(0x00); //0
                        //返回本次读取实际读取到的字节数,存储到第6位开始的位置  
                        int r = sFile.Read(buffer, 5, SendLength);
                        
                        //发送
                        try
                        {
                            int iRet = mySocket.Send(buffer, SendLength + 5, 0);
                            if (iRet <= 0) return;//小于0表示出错,等于0表示断开;

                        }
                        catch (System.Exception err)
                        {
                            MessageBox.Show(err.ToString(), "send exception!");
                            return;
                        }
                        Array.Clear(buffer, 0, SendLength + 5);
                        sState.Text = "已发送成功" + i.ToString() + "/" + blockNumber.ToString() + "个数据块...";

                    }
                    //***
                } //end for循环

                //取当前系统时间戳
                TimeSpan ts = DateTime.Now - dt;
                t1.Text= Convert.ToInt64(ts.TotalMilliseconds).ToString(); //（毫秒） 

                //关闭套接字
                mySocket.Close();
                //关闭文件
                sFile.Close();
                sState.Text = "发送成功" + blockNumber.ToString() + "个数据块。";
                sState.ForeColor = Color.Red;
            }
            catch (IOException err)
            {
                MessageBox.Show(err.ToString(), "IO exception!");
                return;
            }//结束文件操作
        }

        //按比例发送
        private void btnSendinProportion_Click(object sender, EventArgs e)
        {
            sState.Text = "正在按比例发送...";
            string fileName = txtSendFileName.Text.Trim(); //文件名
            //Path1
            string myIP1 = txtSendIP1.Text.Trim(); //目标IPv6地址
            int myPort1 = int.Parse(txtSendPort1.Text.Trim());//端口，假设为11111
            Socket mySocket1; //套接字
            //Path2
            string myIP2 = txtSendIP2.Text.Trim(); //目标IPv6地址
            int myPort2 = int.Parse(txtSendPort2.Text.Trim());//端口，假设为11112
            Socket mySocket2; //套接字
            Random rd = new Random();//随机发送
            int p;
            if (fileName == "" || fileName == null || myIP1 == "" || myIP1 == null || myPort1 == 0 || myIP2 == "" || myIP2 == null || myPort2 == 0)
                return;
            //初始化Socket参数
            try
            {
                //path1
                IPAddress IPAddr1 = IPAddress.Parse(myIP1); //装换IPv6地址
                IPEndPoint ep1 = new IPEndPoint(IPAddr1, myPort1);
                mySocket1 = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);//ipv6
                //设置发送缓冲区
                mySocket1.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, BufferSize);
                //发起连接
                mySocket1.Connect(ep1);
                //path2
                IPAddress IPAddr2 = IPAddress.Parse(myIP2); //装换IPv6地址
                IPEndPoint ep2 = new IPEndPoint(IPAddr2, myPort2);
                mySocket2 = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);//ipv6
                //设置发送缓冲区
                mySocket2.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, BufferSize);
                //发起连接
                mySocket2.Connect(ep2);
            }
            catch (System.Exception err)
            {
                MessageBox.Show(err.ToString(), "socket exception!");
                return;
            }//结束socket初始化

            //打开文件
            try
            {
                FileStream sFile = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                long streamLen = sFile.Length; //以字节数计算的长度
                //计算发送块的大小
                long blockNumber = streamLen / SendLength; //发送块的数量
                //计算分块余数
                int m = (int)(streamLen % SendLength); //取其余数
                if (m > 0)
                    blockNumber += 1;
                //设定两条链路的带宽
                float B1 = float.Parse(txtBandwidth1.Text);
                float B2 = float.Parse(txtBandwidth2.Text);
                //计算比例系数
                int p1, p2;
                /*
                p1 = Convert.ToInt32(blockNumber * B1 / (B1 + B2));//5舍6入
                p2 = Convert.ToInt32(blockNumber - p1);
                */
                if (ckbRandom.Checked)//随机发送
                {   //p1和p2为0~blockNumber之间的一个数
                    p1 = Convert.ToInt32(blockNumber * B1 / (B1 + B2));//5舍6入
                    p2 = Convert.ToInt32(blockNumber - p1);
                }
                else //根据块个位数的值（1~10）按顺序发送
                {   //p1和p2为0~10之间的一个数
                    p1 = Convert.ToInt32(10 * B1 / (B1 + B2));//5舍6入
                    p2 = Convert.ToInt32(10 - p1);
                }

                int iRet =0;
                //开始发送
                int i;
                //因为文件可能会比较大，通过一个循环分块去读取,每次读取SendLength大小，
                //最后4个字节存放块号n,最后1个字节放结束标志0xFF  
                byte[] buffer = new byte[SendLength + 5];
                byte[] intBuffer = new byte[4];
                for (i = 1; i <= blockNumber; i++)
                {
                    if (i == blockNumber && m == 0) //最后一个块,且是整块
                    {
                        //块计数+1，并存储到首4个字节
                        intBuffer = BitConverter.GetBytes(i);     // 将 int 转换成字节数组
                        Buffer.BlockCopy(intBuffer, 0, buffer, 0, 4);
                        //第5位为标志位
                        buffer[4] = (byte)(0xFF); //255
                        //返回本次读取实际读取到的字节数,存储到第6位开始的位置  
                        int r = sFile.Read(buffer, 5, SendLength);

                        //发送
                        try
                        {
                            if (ckbRandom.Checked)//随机发送
                            {
                                p = rd.Next(1, (int)blockNumber);
                                if (p <= p1) //通过path1发送
                                    iRet = mySocket1.Send(buffer, SendLength + 5, 0);
                                else //通过path2发送
                                    iRet = mySocket2.Send(buffer, SendLength + 5, 0);
                            }
                            /*
                            else //顺序发送
                            {
                                if (i <= p1) //通过path1发送
                                    iRet = mySocket1.Send(buffer, SendLength + 5, 0);
                                else //通过path2发送
                                    iRet = mySocket2.Send(buffer, SendLength + 5, 0);
                            }
                            */
                            else //根据块个位数的值（1~10）按顺序发送j
                            {
                                int j = i % 10;//取个位数
                                if (j == 0)
                                    j = 10;//将0换成10
                                if (j <= p1) //前面p1个通过path1发送
                                    iRet = mySocket1.Send(buffer, SendLength + 5, 0);
                                else //通过path2发送
                                    iRet = mySocket2.Send(buffer, SendLength + 5, 0);
                            }

                            if (iRet <= 0) return;//小于0表示出错,等于0表示断开;
                        }
                        catch (System.Exception err)
                        {
                            MessageBox.Show(err.ToString(), "send exception!");
                            return;
                        }
                        Array.Clear(buffer, 0, SendLength + 5);
                    }
                    else if (i == blockNumber && m > 0) //最后一个非整块
                    {
                        //块计数+1，并存储到首4个字节
                        intBuffer = BitConverter.GetBytes(i);     // 将 int 转换成字节数组
                        Buffer.BlockCopy(intBuffer, 0, buffer, 0, 4);
                        //第5位为标志位
                        buffer[4] = (byte)(0xFF); //255
                        //返回本次读取实际读取到的字节数,存储到第6位开始的位置  
                        int r = sFile.Read(buffer, 5, m);

                        //发送
                        try
                        {
                            //if (i <= p1) //通过path1发送
                            //    iRet = mySocket1.Send(buffer, m + 5, 0);
                            //else //通过path2发送
                            //    iRet = mySocket2.Send(buffer, m + 5, 0);
                            //if (iRet <= 0) return;//小于0表示出错,等于0表示断开;
                            if (ckbRandom.Checked)//随机发送
                            {
                                p = rd.Next(1, (int)blockNumber);
                                if (p <= p1) //通过path1发送
                                    iRet = mySocket1.Send(buffer, m + 5, 0);
                                else //通过path2发送
                                    iRet = mySocket2.Send(buffer, m + 5, 0);
                            }
                            /*
                            else //顺序发送
                            {
                                if (i <= p1) //通过path1发送
                                    iRet = mySocket1.Send(buffer, m + 5, 0);
                                else //通过path2发送
                                    iRet = mySocket2.Send(buffer, m + 5, 0);
                            }
                            */
                            else //根据块个位数的值（1~10）按顺序发送j
                            {
                                int j = i % 10;//取个位数
                                if (j == 0)
                                    j = 10;//将0换成10
                                if (j <= p1) //前面p1个通过path1发送
                                    iRet = mySocket1.Send(buffer, m + 5, 0);
                                else //通过path2发送
                                    iRet = mySocket2.Send(buffer, m + 5, 0);
                            }
                        }
                        catch (System.Exception err)
                        {
                            MessageBox.Show(err.ToString(), "send exception!");
                            return;
                        }
                        Array.Clear(buffer, 0, SendLength + 5);

                    }
                    else
                    {
                        //块计数+1，并存储到首4个字节
                        intBuffer = BitConverter.GetBytes(i);     // 将 int 转换成字节数组
                        Buffer.BlockCopy(intBuffer, 0, buffer, 0, 4);
                        //第5位为标志位
                        buffer[4] = (byte)(0x00); //0
                        //返回本次读取实际读取到的字节数,存储到第6位开始的位置  
                        int r = sFile.Read(buffer, 5, SendLength);

                        //发送
                        try
                        {
                            //if (i <= p1) //通过path1发送
                            //    iRet = mySocket1.Send(buffer, SendLength + 5, 0);
                            //else //通过path2发送
                            //    iRet = mySocket2.Send(buffer, SendLength + 5, 0);
                            //if (iRet <= 0) return;//小于0表示出错,等于0表示断开;
                            if (ckbRandom.Checked)//随机发送
                            {
                                p = rd.Next(1, (int)blockNumber);
                                if (p <= p1) //通过path1发送
                                    iRet = mySocket1.Send(buffer, SendLength + 5, 0);
                                else //通过path2发送
                                    iRet = mySocket2.Send(buffer, SendLength + 5, 0);
                            }
                            /*
                            else //顺序发送
                            {
                                if (i <= p1) //通过path1发送
                                    iRet = mySocket1.Send(buffer, SendLength + 5, 0);
                                else //通过path2发送
                                    iRet = mySocket2.Send(buffer, SendLength + 5, 0);
                            }
                            */
                            else //根据块个位数的值（1~10）按顺序发送j
                            {
                                int j = i % 10;//取个位数
                                if (j == 0)
                                    j = 10;//将0换成10
                                if (j <= p1) //前面p1个通过path1发送
                                    iRet = mySocket1.Send(buffer, SendLength + 5, 0);
                                else //通过path2发送
                                    iRet = mySocket2.Send(buffer, SendLength + 5, 0);
                            }

                        }
                        catch (System.Exception err)
                        {
                            MessageBox.Show(err.ToString(), "send exception!");
                            return;
                        }
                        Array.Clear(buffer, 0, SendLength + 5);
                        sState.Text = "已按比例发送成功" + i.ToString() + "/" + blockNumber.ToString() + "个数据块...";

                    }
                }

                //取当前系统时间戳
                TimeSpan ts = DateTime.Now - dt;
                t2.Text = Convert.ToInt64(ts.TotalMilliseconds).ToString(); //（毫秒） 
                t2_t1.Text = (int.Parse(t2.Text) - int.Parse(t1.Text)).ToString(); //发送时间差： t2-t1
                //关闭套接字
                mySocket1.Close();
                mySocket2.Close();
                //关闭文件
                sFile.Close();
                sState.Text = "按比例发送成功" + blockNumber.ToString() + "个数据块，其中path1："+ Convert.ToInt32(blockNumber * B1 / (B1 + B2)).ToString()+"个，path2："+ Convert.ToInt32(blockNumber - Convert.ToInt32(blockNumber * B1 / (B1 + B2))).ToString()+"个。";
                sState.ForeColor = Color.Red;
            }
            catch (IOException err)
            {
                MessageBox.Show(err.ToString(), "IO exception!");
                return;
            }//结束文件操作
        }

        //开始监听（双地址）
        private void btnStartinProportion_Click(object sender, EventArgs e)
        {
            //recvNum1 = 0;//接收的数据块数
            strConnection = "";
            string myIP1 = txtRecvIP1.Text.Trim(); //目标IPv6地址
            string myIP2 = txtRecvIP2.Text.Trim(); //目标IPv6地址2
            //Socket mySocket, mySocket2; //套接字
            Socket mySocket1 = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);//ipv6
            Socket mySocket2 = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);//ipv6
            int myPort1 = int.Parse(txtRecvPort1.Text.Trim());//端口，假设为11111
            int myPort2 = int.Parse(txtRecvPort2.Text.Trim());//端口，假设为11112
            if (myIP1 == "" || myIP1 == null || myPort1 == 0 || myIP2 == "" || myIP2 == null || myPort2 == 0)
                return;
            //初始化Socket参数
            try
            {
                IPAddress IPAddr1 = IPAddress.Parse(myIP1); //装换IPv6地址
                IPEndPoint ep1 = new IPEndPoint(IPAddr1, myPort1); //端口，假设为11111
                IPAddress IPAddr2 = IPAddress.Parse(myIP2); //装换IPv6地址
                IPEndPoint ep2 = new IPEndPoint(IPAddr2, myPort2); //端口，假设为11112
                //设置发送缓冲区
                mySocket1.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, BufferSize);
                mySocket2.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, BufferSize);

                mySocket1.Bind(ep1);
                mySocket2.Bind(ep2);
                mySocket1.Listen(10000);//指定侦听队列容量10000个
                mySocket2.Listen(10000);//指定侦听队列容量10000个

                //Accept（）：接收连接并返回一个新的Socket; Receive():从Socket中读取数据
                Thread thread1 = new Thread(AcceptInfo1);
                thread1.IsBackground = true;
                thread1.Start(mySocket1);

                Thread thread2 = new Thread(AcceptInfo2);
                thread2.IsBackground = true;
                thread2.Start(mySocket2);

                //


            }
            catch (System.Exception err)
            {
                MessageBox.Show(err.ToString(), "socket exception!");
                return;
            }//结束socket初始化
            rState.Text = "正在监听（双地址）...";
            rState.ForeColor = Color.Red;
        }
        //开始接收
        void AcceptInfo1(object o) //path1
        {
            Socket socket1 = o as Socket;
            while (true)
            {
                try
                {
                    Socket tSocket1 = socket1.Accept();//创建通信用的Socket
                    string point1 = tSocket1.RemoteEndPoint.ToString();
                    strConnection += point1 + " 连接成功(Path1)！\n"; ;
                    dic.Add(point1, tSocket1);
                    ////接收消息
                    Thread th1 = new Thread(ReceiveMsg1);
                    th1.IsBackground = true;
                    th1.Start(tSocket1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "AcceptInfo1 error！"); break;
                }
            }

        }
        void AcceptInfo2(object o) //path2
        {
            Socket socket2 = o as Socket;
            while (true)
            {
                try
                {
                    Socket tSocket2 = socket2.Accept();//创建通信用的Socket
                    string point2 = tSocket2.RemoteEndPoint.ToString();
                    strConnection += point2+" 连接成功(Path2)！\n";
                    dic.Add(point2, tSocket2);
                    ////接收消息
                    Thread th2 = new Thread(ReceiveMsg2);
                    th2.IsBackground = true;
                    th2.Start(tSocket2);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "AcceptInfo2 error！"); break;
                }
            }

        }
        //接收消息（path2）
        void ReceiveMsg1(object o)
        {
            rtbConnection.AppendText(strConnection + "\n");
            Socket client = o as Socket;
            int blockNum;//数据块的序号
            recvNum1 = 0;
            flag1 = "";
            string flag;
            Messages1 = new Msg[ReciveSize];//至多1024*1024个数据块,文件最大32GB
            while (true)
            {
                //接收客户端发送过来的数据
                try
                {
                    //定义byte数组存放从客户端接收过来的数据
                    byte[] buffer = new byte[SendLength + 5];//前面4个字节存放块号n ，第5个字节存放块结束标志0xFF
                                                             //将接收过来的数据放到buffer中，并返回实际接受数据的长度
                    byte[] intbuffer = new byte[4];//数据块数组
                    int n = client.Receive(buffer);//接收字节长度
                                                   //

                    if (n > 5)
                    {

                        //提取首4个字节：块号
                        Buffer.BlockCopy(buffer, 0, intbuffer, 0, 4);
                        blockNum = BitConverter.ToInt32(intbuffer, 0);
                        //提取第5位：标志位
                        flag = buffer[4].ToString("X2");  //字节转16进制字符
                        Messages1[recvNum1] = new Msg();
                        Messages1[recvNum1].Num = blockNum;
                        Messages1[recvNum1].Flag = flag;
                        Messages1[recvNum1].Content = new byte[n - 5];
                        Buffer.BlockCopy(buffer, 5, Messages1[recvNum1].Content, 0, n - 5);
                        recvNum1++; //接收的数据块+1
                        Thread.Sleep(50);//暂停50ms
                    }
                    else //如果返回一个0，即读取完了 
                    {
                        //path1接收完毕
                        flag1 = "#";
                        break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "ReceiveMsg1 error！"); break;
                }
            }//end while
            if (flag1 == "#" && flag2 == "#") //path1和path2都结束了
            {
                ReceiveFile();
            }
        }
        //接收消息（path2）
        void ReceiveMsg2(object o)
        {
            rtbConnection.AppendText(strConnection + "\n");
            Socket client = o as Socket;
            int blockNum;//数据块的序号
            recvNum2 = 0;
            flag2 = "";
            string flag;
            Messages2 = new Msg[ReciveSize];//至多1024*1024个数据块,文件最大32GB
            while (true)
                {
                    //接收客户端发送过来的数据
                    try
                    {
                        //定义byte数组存放从客户端接收过来的数据
                        byte[] buffer = new byte[SendLength + 5];//前面4个字节存放块号n ，第5个字节存放块结束标志0xFF
                                                                 //将接收过来的数据放到buffer中，并返回实际接受数据的长度
                        byte[] intbuffer = new byte[4];//数据块数组
                        int n = client.Receive(buffer);//接收字节长度
                        //

                        if (n > 5)
                        {
                            
                            //提取首4个字节：块号
                            Buffer.BlockCopy(buffer, 0, intbuffer, 0, 4);
                            blockNum = BitConverter.ToInt32(intbuffer, 0);
                            //提取第5位：标志位
                            flag = buffer[4].ToString ("X2");  //字节转16进制字符
                            Messages2[recvNum2] = new Msg();
                            Messages2[recvNum2].Num = blockNum;
                            Messages2[recvNum2].Flag=flag;
                            Messages2[recvNum2].Content = new byte[n-5];
                            Buffer.BlockCopy(buffer, 5, Messages2[recvNum2].Content, 0, n - 5);
                            recvNum2++; //接收的数据块+1
                        Thread.Sleep(50);//暂停50ms
                    }
                        else //如果返回一个0，即读取完了 
                        {
                            //path2接收完毕
                            flag2="#";
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "ReceiveMsg2 error！"); break;
                    }
                }//end while
            if (flag1=="#" && flag2=="#") //path1和path2都结束了
            {
                ReceiveFile();
            }
        }

        private void ReceiveFile()
        {
            int recvNum = recvNum1 + recvNum2;
            
            int i, j;
            string fileName = txtRecvFileName.Text.Trim();
            if (recvNum > 0 && fileName != "" && fileName != null)
            {
                FileStream fs = new FileStream(fileName, FileMode.Create);
                //设置文件共享模式为允许随后写入，即多线程同事写入文件
                //FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);
                //
                Msg[] totalMessages = new Msg[ReciveSize];//至多1024*1024个数据块,文件最大32GB
                try
                {   if (recvNum1 > 0)
                        for (i = 0; i < recvNum1; i++)
                        {
                            totalMessages[i]=Messages1[i];
                        }
                    if (recvNum2 > 0)
                        for (j = 0; j < recvNum2; j++)
                        {
                            totalMessages[j+ recvNum1] = Messages2[j];
                        }

                    Array.Sort(totalMessages); //按数据块序号排序
                    foreach (Msg m in totalMessages)
                    {
                        if (m != null)
                        {
                            //MessageBox.Show(m.Num.ToString(), m.Flag);
                            fs.Write(m.Content, 0, m.Content.Length);//写入文件
                            if (m.Flag == "FF") //最后一个块
                                break;
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "writeFile error！");
                }

                //清空缓冲区、关闭流
                fs.Flush();
                fs.Close();
                TimeSpan ts = DateTime.Now - dt;
                t4.Text = Convert.ToInt64(ts.TotalMilliseconds).ToString(); //（毫秒）   
                t4_t3.Text = (int.Parse(t4.Text) - int.Parse(t3.Text)).ToString(); //发送时间差： t4-t3
                rtbConnection.AppendText("接收了" + (recvNum).ToString() + "个数据块。\n");

            }
        }
    }

}
