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
    public partial class FileServer : Form
    {
        public FileServer()
        {
            InitializeComponent();
        }

        public int SendLength = 32768; //发送块长度：一次发32KB(32*1024=32768字节) ，实际发送长度为：SendLength+4 个字节
        public int ReciveSize = 1024 * 1024;  //至多1,000,000个数据块,文件最大32GB
        public int BufferSize = 1000000000;//发送和接收缓冲区大小：1G

        public string strConnection;//连接提示信息
        public DateTime dt = new DateTime();//用于计算时间戳
        public int recvNum1, recvNum2;//接收的数据块数量
        public string flag1, flag2; //接收结束标志#
        public string MfileName="";   //多路径接收文件名
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
        private void FileServer_Load(object sender, EventArgs e)
        {
            //窗体启动时间
            dt = DateTime.Now;
            //两种send-receive方法的时间差为：(t2-t1)-(t4-t3)=(t3-t1）-(t4-t2),若时间差大于0，则两条路径优于一条路径
        }
        private void FileServer_Shown(object sender, EventArgs e)
        {
            //显示文件
            showFile();
        }
        //开始监听
        private void btnStart_Click(object sender, EventArgs e)
        {
            //单路径启动
            string myIP = txtRecvIP.Text.Trim(); //目标IPv6地址
            //Socket mySocket; 
            Socket mySocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);//ipv6
            int myPort = int.Parse(txtRecvPort.Text.Trim());//端口，假设为11110
            if (myIP == "" || myIP == null || myPort == 0)
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
            }
            catch (System.Exception err)
            {
                MessageBox.Show(err.ToString(), "socket exception!");
                return;
            }//结束socket初始化
            sState.Text = "正在监听...";
            sState.ForeColor = Color.Red;
            //多路径启动
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
            }
            catch (System.Exception err)
            {
                MessageBox.Show(err.ToString(), "socket exception!");
                return;
            }//结束socket初始化
            sState.Text = "正在监听（双地址）...";
            sState.ForeColor = Color.Red;
        }

        //清空接收框
        private void btnClear_Click(object sender, EventArgs e)
        {
            rtbConnection.Text = "";
            sState.Text = "等待...";
            sState.ForeColor = Color.Black;
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
                    //rtbConnection.AppendText(point + "连接成功！\n");
                    //线程间操作无效: 从不是创建控件的线程访问它,修改此bug如下：创建一个delegate,并通过Invoke() 来调用它.
                    Action<string> action = (data) =>
                    {
                        this.rtbConnection.AppendText(data + "连接成功！\n");
                    };
                    Invoke(action, point);
                    //
                    dic.Add(point, tSocket);
                    //接收消息
                    Thread th = new Thread(ReceiveMsg);
                    th.IsBackground = true;
                    th.Start(tSocket);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "AcceptInfo error！"); break;
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
            string fileName = "";
            string pathName = txtRecvFileName.Text.Trim();
            Msg[] Messages = new Msg[ReciveSize];//至多1024*1024个数据块,文件最大32GB
            if (pathName != "" && pathName != null)
            {
                //FileStream fs = new FileStream(fileName, FileMode.Create);
                FileStream fs;
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
                            
                            if (blockNum==0 )//第0个块，提取文件名
                            {
                                byte[] byteArray=new byte[n-5];
                                Buffer.BlockCopy(buffer, 5, byteArray, 0, n - 5);
                                fileName = System.Text.Encoding.Default.GetString(byteArray);
                                Thread.Sleep(50);//暂停50ms
                            }
                            else
                            {
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
                        }
                        else   //如果返回一个0，即读取完了 
                        {
                            if (fileName != "")
                            {   //打开文件
                                fileName = pathName + fileName;
                                //MessageBox.Show("接收文件为：" + fileName, "单路径");
                                fs = new FileStream(fileName, FileMode.Create);
                             
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
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "ReceiveMsg error！"); break;
                    }

                }
                TimeSpan ts = DateTime.Now - dt;
                //t3.Text = Convert.ToInt64(ts.TotalMilliseconds).ToString(); //（毫秒） 
                //线程间操作无效: 从不是创建控件的线程访问它,修改此bug如下：创建一个delegate,并通过Invoke() 来调用它.
                Action<TimeSpan> action = (data) =>
                {
                    this.t3.Text = Convert.ToInt64(data.TotalMilliseconds).ToString(); //（毫秒）
                };
                Invoke(action, ts);
                //
                MessageBox.Show("接收文件为：" + fileName, "单路径");
                //rtbConnection.AppendText("接收了" + i.ToString() + "个数据块。\n");
                //线程间操作无效: 从不是创建控件的线程访问它,修改此bug如下：创建一个delegate,并通过Invoke() 来调用它.
                Action<int> action1 = (data1) =>
                {
                    this.rtbConnection.AppendText("接收了" + data1.ToString() + "个数据块。\n");
                };
                Invoke(action1, i);
                //显示文件
                showFile();
            }
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
                    strConnection += point2 + " 连接成功(Path2)！\n";
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

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtRecvFileName.Text = folderBrowserDialog1.SelectedPath;
                //加上斜杠
                string a = txtRecvFileName.Text;
                string b = a.Remove(0, a.Length - 1);
                if (b != "\\")
                    txtRecvFileName.Text += "\\";
                //显示文件
                showFile();
            }

        }

        //接收消息（path2）
        void ReceiveMsg1(object o)
        {
            //rtbConnection.AppendText(strConnection + "\n");
            //线程间操作无效: 从不是创建控件的线程访问它,修改此bug如下：创建一个delegate,并通过Invoke() 来调用它.
            Action<string> action = (data) =>
            {
                this.rtbConnection.AppendText(data + "\n");
            };
            Invoke(action, strConnection);
            //
            Socket client = o as Socket;
            int blockNum;//数据块的序号
            recvNum1 = 0;
            flag1 = "";
            string flag;
            MfileName = "";
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

                        if (blockNum == 0)//第0个块，提取文件名
                        {
                            byte[] byteArray = new byte[n - 5];
                            Buffer.BlockCopy(buffer, 5, byteArray, 0, n - 5);
                            MfileName = System.Text.Encoding.Default.GetString(byteArray);
                            Thread.Sleep(50);//暂停50ms
                            //MessageBox.Show(MfileName, MfileName.Length.ToString());
                        }
                        else
                        {
                            Messages1[recvNum1] = new Msg();
                            Messages1[recvNum1].Num = blockNum;
                            Messages1[recvNum1].Flag = flag;
                            Messages1[recvNum1].Content = new byte[n - 5];
                            Buffer.BlockCopy(buffer, 5, Messages1[recvNum1].Content, 0, n - 5);
                            recvNum1++; //接收的数据块+1
                            Thread.Sleep(50);//暂停50ms
                        }

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
            if (flag1 == "#" && flag2 == "#" && MfileName!="") //path1和path2都结束了
            {
                ReceiveFile();
            }
        }

        

        //接收消息（path2）
        void ReceiveMsg2(object o)
        {
            //rtbConnection.AppendText(strConnection + "\n");
            //线程间操作无效: 从不是创建控件的线程访问它,修改此bug如下：创建一个delegate,并通过Invoke() 来调用它.
            Action<string> action = (data) =>
            {
                this.rtbConnection.AppendText(data + "\n");
            };
            Invoke(action, strConnection);
            //
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
                        flag = buffer[4].ToString("X2");  //字节转16进制字符
                        Messages2[recvNum2] = new Msg();
                        Messages2[recvNum2].Num = blockNum;
                        Messages2[recvNum2].Flag = flag;
                        Messages2[recvNum2].Content = new byte[n - 5];
                        Buffer.BlockCopy(buffer, 5, Messages2[recvNum2].Content, 0, n - 5);
                        recvNum2++; //接收的数据块+1
                        Thread.Sleep(50);//暂停50ms
                    }
                    else //如果返回一个0，即读取完了 
                    {
                        //path2接收完毕
                        flag2 = "#";
                        break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "ReceiveMsg2 error！"); break;
                }
            }//end while
            if (flag1 == "#" && flag2 == "#" && MfileName != "") //path1和path2都结束了
            {
                ReceiveFile();
            }
        }

        //组装文件
        private void ReceiveFile()
        {
            int recvNum = recvNum1 + recvNum2;

            int i, j;
            string pathName = txtRecvFileName.Text.Trim();
            if (recvNum > 0 && pathName != "" && pathName != null)
            {   //打开文件
                string fullName = pathName + MfileName;
                //MessageBox.Show("接收文件为：" + fullName, "多路径");

                FileStream fs = new FileStream(fullName, FileMode.Create);
                //设置文件共享模式为允许随后写入，即多线程同事写入文件
                //FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);
                //
                Msg[] totalMessages = new Msg[ReciveSize];//至多1024*1024个数据块,文件最大32GB
                try
                {
                    if (recvNum1 > 0)
                        for (i = 0; i < recvNum1; i++)
                        {
                            totalMessages[i] = Messages1[i];
                        }
                    if (recvNum2 > 0)
                        for (j = 0; j < recvNum2; j++)
                        {
                            totalMessages[j + recvNum1] = Messages2[j];
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
                //t4.Text = Convert.ToInt64(ts.TotalMilliseconds).ToString(); //（毫秒）   
                //线程间操作无效: 从不是创建控件的线程访问它,修改此bug如下：创建一个delegate,并通过Invoke() 来调用它.
                Action<TimeSpan> action = (data) =>
                {
                    this.t4.Text = Convert.ToInt64(data.TotalMilliseconds).ToString(); //（毫秒）
                };
                Invoke(action, ts);
                //
                //t4_t3.Text = (Convert.ToInt64(t4.Text) - Convert.ToInt64(t3.Text)).ToString(); //发送时间差： t4-t3
                //线程间操作无效: 从不是创建控件的线程访问它,修改此bug如下：创建一个delegate,并通过Invoke() 来调用它.
                string s= (Convert.ToInt64(t4.Text) - Convert.ToInt64(t3.Text)).ToString(); //发送时间差： t4-t3
                Action<string> action1 = (data1) =>
                {
                    t4_t3.Text = data1; //发送时间差： t4-t3
                };
                Invoke(action1, s);
                MessageBox.Show("接收文件为：" + fullName, "多路径");
                //rtbConnection.AppendText("接收了" + (recvNum).ToString() + "个数据块。\n");
                //线程间操作无效: 从不是创建控件的线程访问它,修改此bug如下：创建一个delegate,并通过Invoke() 来调用它.
                Action<int> action2 = (data2) =>
                {
                    rtbConnection.AppendText("接收了" + (data2).ToString() + "个数据块。\n");
                };
                Invoke(action2, recvNum);
                //显示文件
                showFile();

            }
        }
        private void showFile()
        {

            //线程间操作无效: 从不是创建控件的线程访问它,修改此bug如下：创建一个delegate,并通过Invoke() 来调用它.
            DirectoryInfo folder = new DirectoryInfo(txtRecvFileName.Text);
            Action<DirectoryInfo> action = (data) =>
            {
                    this.dataGridViewFile.Rows.Clear();
               try
               {
               
                    foreach (FileInfo file in data.GetFiles("*.*"))
                    {
                        int i = this.dataGridViewFile.Rows.Add();
                        this.dataGridViewFile.Rows[i].Cells[0].Value = file.Name;
                        this.dataGridViewFile.Rows[i].Cells[1].Value = file.LastWriteTime;
                        this.dataGridViewFile.Rows[i].Cells[2].Value = file.Extension;
                        this.dataGridViewFile.Rows[i].Cells[3].Value = file.Length / 1024;
                    }
               
                }
               catch(Exception ex)
               {
                MessageBox.Show(ex.ToString(), "目录不存在！");
               }
            };
            Invoke(action, folder);
            /*
            //清空
            this.dataGridViewFile.Rows.Clear();
            //显示文件
            DirectoryInfo folder = new DirectoryInfo(txtRecvFileName.Text);
            foreach (FileInfo file in folder.GetFiles("*.*"))
            {
                int i = this.dataGridViewFile.Rows.Add();
                this.dataGridViewFile.Rows[i].Cells[0].Value = file.Name;
                this.dataGridViewFile.Rows[i].Cells[1].Value = file.LastWriteTime;
                this.dataGridViewFile.Rows[i].Cells[2].Value = file.Extension;
                this.dataGridViewFile.Rows[i].Cells[3].Value = file.Length / 1024;
            }
            */
        }
        //切换到Client
        private void btnClient_Click(object sender, EventArgs e)
        {
            Hide();
            //FileClient form1 = new FileClient();
            //form1.ShowDialog();
            this.Close();

        }
    }
}
