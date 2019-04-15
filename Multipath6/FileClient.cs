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
    public partial class FileClient : Form
    {
        public int SendLength = 32768; //发送块长度：一次发32KB(32*1024=32768字节) ，实际发送长度为：SendLength+4 个字节
        //public int ReciveSize = 1024 * 1024;  //至多1,000,000个数据块,文件最大32GB
        public int BufferSize = 1000000000;//发送和接收缓冲区大小：1G

        //public string strConnection;//连接提示信息
        public DateTime dt = new DateTime();//用于计算时间戳
        //public int recvNum1, recvNum2;//接收的数据块数量
        //public string flag1, flag2; //接收结束标志#


        public FileClient()
        {
            InitializeComponent();
        }
        private void FileClient_Load(object sender, EventArgs e)
        {
            //窗体启动时间
            dt = DateTime.Now;
            //两种send-receive方法的时间差为：(t2-t1)-(t4-t3)=(t3-t1）-(t4-t2),若时间差大于0，则两条路径优于一条路径
        }
        private void FileClient_Shown(object sender, EventArgs e)
        {
            //显示文件
            showFile();
        }

        private void btnSendinProportion_Click(object sender, EventArgs e)
        {

        }

        //切换到Server
        private void btnServer_Click(object sender, EventArgs e)
        {
            Hide();
            //FileServer form1 = new FileServer();
            //form1.ShowDialog();
            this.Close();

        }
        //清空
        private void btnClear_Click(object sender, EventArgs e)
        {
            //rtbConnection.Text = "";
            sState.Text = "等待发送...";
            sState.ForeColor = Color.Black;

        }
        //发送文件
        private void btnSendFile_Click(object sender, EventArgs e)
        {
            
            //发送文件
            if (ckbMultiPath.Checked)//多路径发送
            {
                SendFileMultiPathWithName();
            }
            else //单路径发送
            {
                SendFileWithName();
            }
        }
        //单路径发送（带文件名）
        private void SendFileWithName()
        {
            sState.Text = "正在发送...";
            //string fileName = txtSendFileName.Text.Trim(); //文件名
            string fileName = "";//文件名
            fileName =dataGridViewFile.CurrentRow.Cells[0].Value.ToString();
            string fullName = txtSendFileName.Text + fileName;
            string myIP = txtSendIP.Text.Trim(); //目标IPv6地址
            int myPort = int.Parse(txtSendPort.Text.Trim());//端口，假设为11110
            Socket mySocket; //套接字
            if (fileName == "" || fileName == null || myIP == "" || myIP == null || myPort == 0)
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
                FileStream sFile = new FileStream(fullName, FileMode.Open, FileAccess.Read, FileShare.Read);
                long streamLen = sFile.Length; //以字节数计算的长度
                //计算发送块的大小
                long blockNumber = streamLen / SendLength; //发送块的数量
                int m = (int)(streamLen % SendLength); //取其余数
                if (m > 0)
                    blockNumber += 1;
                //开始发送
                int i;
                //因为文件可能会比较大，通过一个循环分块去读取,每次读取SendLength大小，
                //最后4个字节存放块号n,最后1个字节放结束标志0xFF  
                byte[] buffer = new byte[SendLength + 5];
                byte[] intBuffer = new byte[4];

                //先发送文件名 
                intBuffer = BitConverter.GetBytes(0);     // 将 int 转换成字节数组，第0个块
                Buffer.BlockCopy(intBuffer, 0, buffer, 0, 4);
                //第5位为标志位
                buffer[4] = (byte)(0x0F); //F
                //将文件名转化为字节数,存储到第6位开始的位置  
                byte[] byteArray = System.Text.Encoding.Default.GetBytes(fileName);
                Buffer.BlockCopy(byteArray, 0, buffer, 5, byteArray.GetLength(0));
                //sState.Text = "发送文件为：" + fileName;
                MessageBox.Show("发送文件为：" + fileName, "单路径");
                //发送fileName
                try
                {
                    int iRet = mySocket.Send(buffer, byteArray.GetLength(0) + 5, 0);
                    if (iRet <= 0) return;//小于0表示出错,等于0表示断开;

                }
                catch (System.Exception err)
                {
                    MessageBox.Show(err.ToString(), "send exception!");
                    return;
                }
                Array.Clear(buffer, 0, SendLength + 5);
                //发送文件名结束

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
                t1.Text = Convert.ToInt64(ts.TotalMilliseconds).ToString(); //（毫秒） 

                //关闭套接字
                mySocket.Close();
                //关闭文件
                sFile.Close();
                //MessageBox.Show("发送文件为：" + fileName, "单路径");
                sState.Text = "发送成功" + blockNumber.ToString() + "个数据块。";
                sState.ForeColor = Color.Red;
            }
            catch (IOException err)
            {
                MessageBox.Show(err.ToString(), "IO exception!");
                return;
            }//结束文件操作
        }

        //多路径发送（带文件名）
        private void SendFileMultiPathWithName()
        {
            sState.Text = "正在按比例发送...";
            //string fileName = txtSendFileName.Text.Trim(); //文件名
            string fileName = "";//文件名
            fileName = dataGridViewFile.CurrentRow.Cells[0].Value.ToString();
            string fullName = txtSendFileName.Text + fileName;
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
                FileStream sFile = new FileStream(fullName, FileMode.Open, FileAccess.Read, FileShare.Read);
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
                int p1, p2;//p1和p2为0~blockNumber之间的一个数
                p1 = Convert.ToInt32(blockNumber * B1 / (B1 + B2));//5舍6入
                p2 = Convert.ToInt32(blockNumber - p1);
                int iRet = 0;
                //开始发送
                int i;
                //因为文件可能会比较大，通过一个循环分块去读取,每次读取SendLength大小，
                //最后4个字节存放块号n,最后1个字节放结束标志0xFF  
                byte[] buffer = new byte[SendLength + 5];
                byte[] intBuffer = new byte[4];

                //先发送文件名 
                intBuffer = BitConverter.GetBytes(0);     // 将 int 转换成字节数组，第0个块
                Buffer.BlockCopy(intBuffer, 0, buffer, 0, 4);
                //第5位为标志位
                buffer[4] = (byte)(0x0F); //F
                //将文件名转化为字节数,存储到第6位开始的位置  
                byte[] byteArray = System.Text.Encoding.Default.GetBytes(fileName);
                Buffer.BlockCopy(byteArray, 0, buffer, 5, byteArray.GetLength(0));
                //sState.Text = "发送文件为：" + fileName;
                MessageBox.Show("发送文件为：" + fileName, "多路径");
                //发送fileName
                try
                {
                    iRet = mySocket1.Send(buffer, byteArray.GetLength(0) + 5, 0);
                    if (iRet <= 0) return;//小于0表示出错,等于0表示断开;

                }
                catch (System.Exception err)
                {
                    MessageBox.Show(err.ToString(), "send exception!");
                    return;
                }
                Array.Clear(buffer, 0, SendLength + 5);
                //发送文件名结束

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
                            p = rd.Next(1, (int)blockNumber);
                            if (p <= p1) //通过path1发送
                                iRet = mySocket1.Send(buffer, SendLength + 5, 0);
                            else //通过path2发送
                                iRet = mySocket2.Send(buffer, SendLength + 5, 0);
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
                            p = rd.Next(1, (int)blockNumber);
                            if (p <= p1) //通过path1发送
                                iRet = mySocket1.Send(buffer, m + 5, 0);
                            else //通过path2发送
                                iRet = mySocket2.Send(buffer, m + 5, 0);
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
                            p = rd.Next(1, (int)blockNumber);
                            if (p <= p1) //通过path1发送
                                iRet = mySocket1.Send(buffer, SendLength + 5, 0);
                            else //通过path2发送
                                iRet = mySocket2.Send(buffer, SendLength + 5, 0);
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
                t2_t1.Text = (Convert.ToInt64(t2.Text) - Convert.ToInt64(t1.Text)).ToString(); //发送时间差： t2-t1
                //关闭套接字
                mySocket1.Close();
                mySocket2.Close();
                //关闭文件
                sFile.Close();
                //MessageBox.Show("发送文件为：" + fileName, "多路径");
                sState.Text = "按比例发送成功" + blockNumber.ToString() + "个数据块，其中path1：" + Convert.ToInt32(blockNumber * B1 / (B1 + B2)).ToString() + "个，path2：" + Convert.ToInt32(blockNumber - Convert.ToInt32(blockNumber * B1 / (B1 + B2))).ToString() + "个。";
                sState.ForeColor = Color.Red;
            }
            catch (IOException err)
            {
                MessageBox.Show(err.ToString(), "IO exception!");
                return;
            }//结束文件操作

        }
        
        /***
        //单路径发送
        private void SendFile()
        {
            sState.Text = "正在发送...";
            //string fileName = txtSendFileName.Text.Trim(); //文件名
            string fileName="";//文件名
            fileName = txtSendFileName.Text + dataGridViewFile.CurrentRow.Cells[0].Value.ToString();

            string myIP = txtSendIP.Text.Trim(); //目标IPv6地址
            int myPort = int.Parse(txtSendPort.Text.Trim());//端口，假设为11110
            Socket mySocket; //套接字
            if (fileName == "" || fileName == null || myIP == "" || myIP == null || myPort == 0)
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
                long blockNumber = streamLen / SendLength; //发送块的数量
                int m = (int)(streamLen % SendLength); //取其余数
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
                t1.Text = Convert.ToInt64(ts.TotalMilliseconds).ToString(); //（毫秒） 

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
        

        //多路径发送
        private void SendFileMultiPath()
        {
            sState.Text = "正在按比例发送...";
            //string fileName = txtSendFileName.Text.Trim(); //文件名
            string fileName="";//文件名
            fileName =txtSendFileName.Text+ dataGridViewFile.CurrentRow.Cells[0].Value.ToString();
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
                int p1, p2;//p1和p2为0~blockNumber之间的一个数
                p1 = Convert.ToInt32(blockNumber * B1 / (B1 + B2));//5舍6入
                p2 = Convert.ToInt32(blockNumber - p1);
                int iRet = 0;
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
                            p = rd.Next(1, (int)blockNumber);
                            if (p <= p1) //通过path1发送
                                    iRet = mySocket1.Send(buffer, SendLength + 5, 0);
                            else //通过path2发送
                                    iRet = mySocket2.Send(buffer, SendLength + 5, 0);
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
                                p = rd.Next(1, (int)blockNumber);
                                if (p <= p1) //通过path1发送
                                    iRet = mySocket1.Send(buffer, m + 5, 0);
                                else //通过path2发送
                                    iRet = mySocket2.Send(buffer, m + 5, 0);
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
                                p = rd.Next(1, (int)blockNumber);
                                if (p <= p1) //通过path1发送
                                    iRet = mySocket1.Send(buffer, SendLength + 5, 0);
                                else //通过path2发送
                                    iRet = mySocket2.Send(buffer, SendLength + 5, 0);
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
                t2_t1.Text = (Convert.ToInt64(t2.Text) - Convert.ToInt64(t1.Text)).ToString(); //发送时间差： t2-t1
                //关闭套接字
                mySocket1.Close();
                mySocket2.Close();
                //关闭文件
                sFile.Close();
                sState.Text = "按比例发送成功" + blockNumber.ToString() + "个数据块，其中path1：" + Convert.ToInt32(blockNumber * B1 / (B1 + B2)).ToString() + "个，path2：" + Convert.ToInt32(blockNumber - Convert.ToInt32(blockNumber * B1 / (B1 + B2))).ToString() + "个。";
                sState.ForeColor = Color.Red;
            }
            catch (IOException err)
            {
                MessageBox.Show(err.ToString(), "IO exception!");
                return;
            }//结束文件操作

        }
        ***/
        
            //选择文件
        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            /*OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "所有文件(*.*)|*.*";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                txtSendFileName.Text = fileDialog.FileName;
            }*/
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtSendFileName.Text = folderBrowserDialog1.SelectedPath;
                //加上斜杠
                string a = txtSendFileName.Text;
                string b = a.Remove(0, a.Length - 1);
                if (b != "\\")
                    txtSendFileName.Text += "\\";

                showFile();
            }
        }

        private void showFile()
        {
            //清空
            this.dataGridViewFile.Rows.Clear(); 
            //显示文件
            DirectoryInfo folder = new DirectoryInfo(txtSendFileName.Text);
            try
            {
                foreach (FileInfo file in folder.GetFiles("*.*"))
                {
                    int i = this.dataGridViewFile.Rows.Add();
                    this.dataGridViewFile.Rows[i].Cells[0].Value = file.Name;
                    this.dataGridViewFile.Rows[i].Cells[1].Value = file.LastWriteTime;
                    this.dataGridViewFile.Rows[i].Cells[2].Value = file.Extension;
                    this.dataGridViewFile.Rows[i].Cells[3].Value = file.Length / 1024;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "目录不存在！");
            }
            
        }
    }
}
