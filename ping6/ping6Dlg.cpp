//zhu:本程序必须以管理员身份运行
//Full version

// ping6Dlg.cpp : 实现文件
//

#include "stdafx.h"
#include "ping6.h"
#include "ping6Dlg.h"
#include "afxdialogex.h"


#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//zhu:毫秒测试
//static const int DEFDATALEN = 56;//8字节头部+16字节数据+16字节源地址+16字节目标地址
//static const int MAXIPLEN = 60;
//static const int MAXICMPLEN = 76;

//微秒测试
static const int DEFDATALEN = 32008;//最大：8字节头部+32000字节数据
static const int MEASURELEN = 10;//带宽测量次数
//static const int MAXIPLEN = DEFDATALEN + 40;//数据+8字节头部+16字节源地址+16字节目标地址
//static const int MAXICMPLEN = DEFDATALEN +48;//数据+8字节头部+16字节源地址+16字节目标地址+8字节

//zhu:静态全局变量
static int myid;// , options;
static struct sockaddr_in6 pingaddr;//目标地址
static CString strDestIP, myRTT,RTT;//字符形式目标地址,双程时延

static SOCKET  rawSocket;
//static int roundtriptime,totalrtt,minrtt,maxrtt;//zhu:双程时延，毫秒
static float roundtriptime, totalrtt, minrtt, maxrtt;//zhu:双程时延，微秒
static short nTransmitted,nReceived,nPacketSize;//发送个数，接受个数，包大小
//微秒计时
double run_time; //运行时间
LARGE_INTEGER time_start, time_end;  //开始时间，结束时间  
double dqFreq;  //计时器频率
LARGE_INTEGER f;      //计时器频率  
//包大小和最小时延数组
static int Size[MEASURELEN];
static float MinTime[MEASURELEN];





// 用于应用程序“关于”菜单项的 CAboutDlg 对话框

class CAboutDlg : public CDialogEx
{
public:
	CAboutDlg();

// 对话框数据
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_ABOUTBOX };
#endif

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

// 实现
protected:
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialogEx(IDD_ABOUTBOX)
{
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialogEx)
END_MESSAGE_MAP()


// Cping6Dlg 对话框



Cping6Dlg::Cping6Dlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(IDD_PING6_DIALOG, pParent)
	, m_pingaddr(_T(""))
	, m_pingresult(_T(""))
	, m_Times(_T(""))
	, m_Size(_T(""))
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void Cping6Dlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);

	DDX_Text(pDX, IDC_EDIT_IPaddr, m_pingaddr);//目标ipv6 address
	DDX_Text(pDX, IDC_EDIT_PingResult, m_pingresult);//ping结果
	DDX_Text(pDX, IDC_EDIT_times, m_Times);
	DDX_Text(pDX, IDC_EDIT_size, m_Size);
}

BEGIN_MESSAGE_MAP(Cping6Dlg, CDialogEx)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDOK, &Cping6Dlg::OnBnClickedOk)
	ON_BN_CLICKED(IDBandWidth, &Cping6Dlg::OnBnClickedBandwidth)
END_MESSAGE_MAP()


// Cping6Dlg 消息处理程序

BOOL Cping6Dlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// 将“关于...”菜单项添加到系统菜单中。

	// IDM_ABOUTBOX 必须在系统命令范围内。
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL)
	{
		BOOL bNameValid;
		CString strAboutMenu;
		bNameValid = strAboutMenu.LoadString(IDS_ABOUTBOX);
		ASSERT(bNameValid);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// 设置此对话框的图标。  当应用程序主窗口不是对话框时，框架将自动
	//  执行此操作
	SetIcon(m_hIcon, TRUE);			// 设置大图标
	SetIcon(m_hIcon, FALSE);		// 设置小图标

	// TODO: 在此添加额外的初始化代码

	m_pingaddr = "2001:da8:e000:92::29"; // ipv6.zju.edu.cn(浙大）// "2001:470:0:76::2"; //he.net
	m_Times = "5";
	m_Size = "100";
	UpdateData(FALSE); //从变量传给控件

	return TRUE;  // 除非将焦点设置到控件，否则返回 TRUE
}

void Cping6Dlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialogEx::OnSysCommand(nID, lParam);
	}
}

// 如果向对话框添加最小化按钮，则需要下面的代码
//  来绘制该图标。  对于使用文档/视图模型的 MFC 应用程序，
//  这将由框架自动完成。

void Cping6Dlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // 用于绘制的设备上下文

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// 使图标在工作区矩形中居中
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// 绘制图标
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialogEx::OnPaint();
	}
}

//当用户拖动最小化窗口时系统调用此函数取得光标
//显示。
HCURSOR Cping6Dlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}



void Cping6Dlg::OnBnClickedOk()
{
	// TODO: 在此添加控件通知处理程序代码
	//CDialogEx::OnOK();
	//
	// 将各控件中的数据保存到相应的变量   
	UpdateData(TRUE); //从控件传给变量
	
	

	//zhu
	//const char * myHost;//目标主机地址
	CString myIP;
	char *myHost;
	int myNum= _ttoi(m_Times); //测试次数

	myIP = m_pingaddr;
	//CString转换成char*
	_bstr_t bstr;
	bstr = myIP;
	myHost = bstr;
	//myHost = "2001:470:0:76::2"; //he.net
	//myHost = "2001:12ff:0:4::6"; //nic.br
	myid = (USHORT)GetCurrentThreadId() ;//当前进程号,16位
	nTransmitted = 0;
	nReceived = 0;
	totalrtt = 0;
	minrtt = 0;
	maxrtt = 0;
	RTT = "";
	
	//微秒计时
	QueryPerformanceFrequency(&f);
	dqFreq = (double)f.QuadPart;//计算计时器频率



	if (!Ping(myHost, myNum))
		MessageBox(myIP, TEXT("Ping is failed!"), MB_OK);
	else
	{
		/*myRTT.Format(_T("%d"), roundtriptime);
		myRTT+=("ms\r\n");*/
		//MessageBox(myIP + " RTT is " + myRTT, TEXT("Ping is successful!"), MB_OK);
		if (nReceived > 0)
		{
			RTT.Append(_T("\r\n"));
			RTT.Append(_T("packet size："));
			//myRTT.Format(_T("%d"),nPacketSize);
			myRTT.Format(_T("%d"), nPacketSize-8);
			RTT.Append(myRTT + "字节\r\n");

			RTT.Append(_T("平均时延："));
			//myRTT.Format(_T("%d"), totalrtt/nReceived);
			myRTT.Format(_T("%5.2f"), totalrtt / nReceived);
			RTT.Append(myRTT + "ms， ");
			RTT.Append(_T("最大时延："));
			//myRTT.Format(_T("%d"), maxrtt);
			myRTT.Format(_T("%5.2f"), maxrtt);
			RTT.Append(myRTT + "ms， ");
			RTT.Append(_T("最小时延："));
			//myRTT.Format(_T("%d"), minrtt);
			myRTT.Format(_T("%5.2f"), minrtt);
			RTT.Append(myRTT + "ms\r\n");

			RTT.Append(_T("包丢失率："));
			myRTT.Format(_T("%0.2f"), (float)(nTransmitted-nReceived) / (float)nTransmitted *100);
			RTT.Append(myRTT+"%， ");
			myRTT.Format(_T("发送：%d， "), nTransmitted);
			RTT.Append(myRTT);
			myRTT.Format(_T("接收：%d"), nReceived);
			RTT.Append(myRTT + "\r\n");
			
		}
		else
		{
			RTT.Append(_T("\r\n"));
			RTT.Append(_T("平均双程时延：超时   "));			
			RTT.Append(_T("包丢失率：100%\r\n"));			
			
		}


		m_pingresult = RTT;
	}
	
	// 根据各变量的值更新相应的控件。和的编辑框会显示m_editSum的值   
	UpdateData(FALSE); //从变量传给控件
	//清0，为下次测试做准备
	

}

//zhu
//PING函数返回值1:成功,0:失败
BOOL Cping6Dlg::Ping(const char * pHost,int nNum)
{
	struct icmp6_hdr *pkt;//?
	int c;
	//char packet[DEFDATALEN + MAXIPLEN + MAXICMPLEN];
	char packet[DEFDATALEN];
	CString err;
	strDestIP = pHost;
	WSADATA  wsaData;
	//创建一个Raw套节字 
	if (WSAStartup(0x202, &wsaData)) { //返回值为0则启动异步套接字成功
		MessageBox(strDestIP, TEXT("WSAStartup Error"), MB_OK);
		return 0;
	}
	rawSocket = socket(AF_INET6, SOCK_RAW, IPPROTO_ICMPV6);//ipv6
	if (rawSocket == SOCKET_ERROR)
	{
		err.Format(_T("%d"), WSAGetLastError());
		MessageBox(err, TEXT("Socket error"), MB_OK);
		closesocket(rawSocket);
		::WSACleanup();
		return  0;
	}
	memset(&pingaddr, 0, sizeof(struct sockaddr_in6));
	pingaddr.sin6_family = AF_INET6;
	if (inet_pton(AF_INET6, pHost, (void *)&pingaddr.sin6_addr) != 1) //地址字符串转成数值，成功则返回值为1
	{
		err.Format(_T("%d"), WSAGetLastError());
		MessageBox(err, TEXT("Source IP error"), MB_OK);
		closesocket(rawSocket);
		::WSACleanup();
		return  0;
	}

	//发送测试包
	int i, nRet;
	for (i = 0; i < nNum; i++)
	{SendPacket();
	//等待回应 
	nRet = WaitForEchoReply(rawSocket);


	
	

	if (nRet == SOCKET_ERROR) //nRet=-1
	{
		err.Format(_T("%d"), WSAGetLastError());
		MessageBox(err, TEXT("WaitForEchoReply error"), MB_OK);
		nRet = closesocket(rawSocket);
		::WSACleanup();
		return  0;
	}
	if (!nRet) //nRet=0
	{
		/*MessageBox(strDestIP, TEXT("WaitForEchoReply return error"), MB_OK);
		nRet = closesocket(rawSocket);
		::WSACleanup();
		return  0;*/
		RTT.Append(_T("time out\r\n"));
	}
	else
	{
	 while (1) {
		struct sockaddr_in6 from;
		int fromlen = sizeof(struct  sockaddr_in6);

		if ((c = recvfrom(rawSocket, packet, sizeof(packet), 0,(struct sockaddr *) &from, &fromlen)) < 0) 
		{
			if (errno == EINTR)
				continue;
			err.Format(_T("%d"), GetLastError());
			MessageBox(err, TEXT(" recvfrom error"), MB_OK);
			continue;
		}
		if (c >= 8) {			/* icmp6_hdr */
			pkt = (struct icmp6_hdr *) packet;
			char IPdotdec[200];
			CString sTemp;
			inet_ntop(AF_INET6, &from.sin6_addr, IPdotdec, sizeof(IPdotdec));//ipv6
			sTemp = IPdotdec;	
			if (pkt->icmp6_type == ICMP6_ECHO_REPLY  && sTemp == strDestIP && pkt->icmp6_id == myid)
			{   //双程时延
				//roundtriptime = GetTickCount();//毫秒
				//roundtriptime -= pkt->icmp6_data[0];
				//微秒时延
				QueryPerformanceCounter(&time_end);   //计时开始
				run_time = 1000000 * (time_end.QuadPart- pkt->icmp6_data[0]) / dqFreq; //乘以1000000把单位由秒化为微秒，精度为1000 000/（cpu主频）微秒
				roundtriptime =(float) run_time/1000.;

				//myRTT.Format(_T("%d"), roundtriptime);
				myRTT.Format(_T("%5.2f"), roundtriptime);
				RTT.Append(myRTT+"ms\r\n");
				//累计双程时延和收包个数
				nReceived++;
				totalrtt += roundtriptime;
				//最大和最小时延
				if (roundtriptime > maxrtt)
					maxrtt = roundtriptime;
				if (roundtriptime < minrtt || minrtt==0)
					minrtt = roundtriptime;

			}
			else
				roundtriptime = -1;
				break;
		}
	}
	}
	

	}
	closesocket(rawSocket);
	::WSACleanup();
	return 1;
}
//等待回应  
int  Cping6Dlg::WaitForEchoReply(SOCKET  s)
{
	struct  timeval  Timeout;
	fd_set  readfds;
	FD_ZERO(&readfds);
	FD_SET(s, &readfds);
	//            readfds.fd_count  =  1;              
	//            readfds.fd_array[0]  =  s;  
	Timeout.tv_sec = 5;
	Timeout.tv_usec = 0;
	int  i = select(1, &readfds, NULL, NULL, &Timeout);
	return(i);
}

//发测试包
void Cping6Dlg::SendPacket()
{
	struct icmp6_hdr *pkt;
	int i;
	CString err;
	nPacketSize = _ttoi(m_Size)+8; //测试包大小 
	//pkt =(struct icmp6_hdr *)malloc(sizeof(int)*nPacketSize);
	char packet[DEFDATALEN];
	
	pkt = (struct icmp6_hdr *) packet;
	pkt->icmp6_type = ICMP6_ECHO_REQUEST;
	pkt->icmp6_code = 0;
	pkt->icmp6_cksum = 0;
	pkt->icmp6_seq = nTransmitted++;
	pkt->icmp6_id = myid;
	//pkt->icmp6_data[0] = GetTickCount(); // 用于获取自windows启动以来经历的时间长度（毫秒）
	//微秒计时
	QueryPerformanceCounter(&time_start);   //计时开始
	time_start.QuadPart; //（cpu主频）微秒
	pkt->icmp6_data[0] = time_start.QuadPart;
	
	//nPacketSize = sizeof(packet);
	i = sendto(rawSocket, packet, nPacketSize, 0,
		(struct sockaddr *) &pingaddr, sizeof(struct sockaddr_in6));
	if (i < 0 || i != nPacketSize)//发送成功，则返回值c为packet的长度数192；否则为发送失败
	{
		err.Format(_T("%d"), GetLastError());
		MessageBox(err, TEXT("sendto error"), MB_OK);
		closesocket(rawSocket);
		::WSACleanup();
		
	}
	
}

//带宽测试
void Cping6Dlg::OnBnClickedBandwidth()
{
	// TODO: 在此添加控件通知处理程序代码
	// 将各控件中的数据保存到相应的变量   
	UpdateData(TRUE); //从控件传给变量

	CString myIP;
	char *myHost;
	int myNum = _ttoi(m_Times); //测试次数
	int mySize = _ttoi(m_Size); //带宽测量增幅大小

	myIP = m_pingaddr;
	//CString转换成char*
	_bstr_t bstr;
	bstr = myIP;
	myHost = bstr;
	//myHost = "2001:470:0:76::2"; //he.net
	//myHost = "2001:12ff:0:4::6"; //nic.br
	myid = (USHORT)GetCurrentThreadId();//当前进程号,16位
	nTransmitted = 0;
	nReceived = 0;
	totalrtt = 0;
	minrtt = 0;
	maxrtt = 0;
	RTT = "Data(bytes)\tMin-time(ms)\tLossRate\r\n";
	//微秒计时
	QueryPerformanceFrequency(&f);
	dqFreq = (double)f.QuadPart;//计算计时器频率



	if (!Bandwidth(myHost, myNum, mySize))
		MessageBox(myIP, TEXT("Ping is failed!"), MB_OK);
	else
	{
		int i, j;
		float minTime, diffTime;
		i = 0;
		minTime = 0;
		diffTime = 0;
		for (j = 0; j < MEASURELEN; j++)
		{
			
			if (MinTime[j] < minTime || minTime == 0)//最小的时延时间
			{
				minTime = MinTime[j];
				i = j;//当前的数据包序号
			}
			else
			{
				if (j == i + 1)//最小时延包的下一个包
					diffTime = MinTime[j] - minTime;
			}
			

		}
		if (diffTime == 0)
		{
			RTT.Append(_T("\r\nBandwidth testing failed,try again.\r\n"));
		}
		else
		{
			myRTT.Format(_T("\r\nBandwidth is: %8.2f"), mySize*8*2 /diffTime/1000); //输出单位为Mbps（时间单位为ms，双程时延的一半是单程时延）
			RTT.Append(myRTT + "Mbps\r\n");
		}
		m_pingresult = RTT;
		
	}

	// 根据各变量的值更新相应的控件。和的编辑框会显示m_editSum的值   
	UpdateData(FALSE); //从变量传给控件
	//清0，为下次测试做准备

}

//zhu
//Bandwidth函数返回值1:成功,0:失败
BOOL Cping6Dlg::Bandwidth(const char * pHost, int nNum,int nSize)
{
	struct icmp6_hdr *pkt;//?
	int c;
	int nBSize;//发送包data大小
	char packet[DEFDATALEN];
	CString err;
	strDestIP = pHost;
	WSADATA  wsaData;
	//创建一个Raw套节字 
	if (WSAStartup(0x202, &wsaData)) { //返回值为0则启动异步套接字成功
		MessageBox(strDestIP, TEXT("WSAStartup Error"), MB_OK);
		return 0;
	}
	rawSocket = socket(AF_INET6, SOCK_RAW, IPPROTO_ICMPV6);//ipv6
	if (rawSocket == SOCKET_ERROR)
	{
		err.Format(_T("%d"), WSAGetLastError());
		MessageBox(err, TEXT("Socket error"), MB_OK);
		closesocket(rawSocket);
		::WSACleanup();
		return  0;
	}
	memset(&pingaddr, 0, sizeof(struct sockaddr_in6));
	pingaddr.sin6_family = AF_INET6;
	if (inet_pton(AF_INET6, pHost, (void *)&pingaddr.sin6_addr) != 1) //地址字符串转成数值，成功则返回值为1
	{
		err.Format(_T("%d"), WSAGetLastError());
		MessageBox(err, TEXT("Source IP error"), MB_OK);
		closesocket(rawSocket);
		::WSACleanup();
		return  0;
	}

	//发送测试包
	int i, j,nRet;
	for (j = 0; j < MEASURELEN; j++)
	{
		nBSize =32+ j * nSize;
		Size[j] = nBSize;
		nReceived = 0;
		totalrtt = 0;
		maxrtt = 0;
		minrtt = 0;
		nTransmitted = 0;
		for (i = 0; i < nNum; i++)
		{
			SendPacketB(nBSize);
			//等待回应 
			nRet = WaitForEchoReply(rawSocket);





			if (nRet == SOCKET_ERROR) //nRet=-1
			{
				err.Format(_T("%d"), WSAGetLastError());
				MessageBox(err, TEXT("WaitForEchoReply error"), MB_OK);
				nRet = closesocket(rawSocket);
				::WSACleanup();
				return  0;
			}
			if (!nRet) //nRet=0
			{
				
				//RTT.Append(_T("time out\r\n"));
			}
			else
			{
				while (1) {//接收包
					struct sockaddr_in6 from;
					int fromlen = sizeof(struct  sockaddr_in6);

					if ((c = recvfrom(rawSocket, packet, sizeof(packet), 0, (struct sockaddr *) &from, &fromlen)) < 0)
					{
						if (errno == EINTR)
							continue;
						err.Format(_T("%d"), GetLastError());
						MessageBox(err, TEXT(" recvfrom error"), MB_OK);
						continue;
					}
					if (c >= 8) {			/* icmp6_hdr */
						pkt = (struct icmp6_hdr *) packet;
						char IPdotdec[200];
						CString sTemp;
						inet_ntop(AF_INET6, &from.sin6_addr, IPdotdec, sizeof(IPdotdec));//ipv6
						sTemp = IPdotdec;
						if (pkt->icmp6_type == ICMP6_ECHO_REPLY  && sTemp == strDestIP && pkt->icmp6_id == myid)
						{   //双程时延
							//roundtriptime = GetTickCount();//毫秒
							//roundtriptime -= pkt->icmp6_data[0];
							//微秒时延
							QueryPerformanceCounter(&time_end);   //计时开始
							run_time = 1000000 * (time_end.QuadPart - pkt->icmp6_data[0]) / dqFreq; //乘以1000000把单位由秒化为微秒，精度为1000 000/（cpu主频）微秒
							roundtriptime = (float)run_time / 1000.;
							//累计双程时延和收包个数
							nReceived++;
							totalrtt += roundtriptime;
							//最大和最小时延
							if (roundtriptime > maxrtt)
								maxrtt = roundtriptime;
							if (roundtriptime < minrtt || minrtt == 0)
							{
								minrtt = roundtriptime;
								MinTime[j] = minrtt;
							}
						}
						else
							roundtriptime = -1;
						break;
					}
				}
			}


		}//end i
		if (nReceived > 0)
		{
			
			myRTT.Format(_T("%d"), nBSize);
			RTT.Append(myRTT + "\t\t");
	
			myRTT.Format(_T("%5.2f"), minrtt);
			RTT.Append(myRTT + "\t\t");

			myRTT.Format(_T("%0.2f"), (float)(nTransmitted - nReceived) / (float)nTransmitted * 100);
			RTT.Append(myRTT + "%\r\n");
			

		}
		
	}//end j
	closesocket(rawSocket);
	::WSACleanup();
	return 1;
}
//发可变长度测试包
void Cping6Dlg::SendPacketB(int nBSize)
{
	struct icmp6_hdr *pkt;
	int i;
	CString err;
	nPacketSize = nBSize + 8; //测试包大小 
									 //pkt =(struct icmp6_hdr *)malloc(sizeof(int)*nPacketSize);
	char packet[DEFDATALEN];

	pkt = (struct icmp6_hdr *) packet;
	pkt->icmp6_type = ICMP6_ECHO_REQUEST;
	pkt->icmp6_code = 0;
	pkt->icmp6_cksum = 0;
	pkt->icmp6_seq = nTransmitted++;
	pkt->icmp6_id = myid;
	//pkt->icmp6_data[0] = GetTickCount(); // 用于获取自windows启动以来经历的时间长度（毫秒）
	//微秒计时
	QueryPerformanceCounter(&time_start);   //计时开始
	time_start.QuadPart; //（cpu主频）微秒
	pkt->icmp6_data[0] = time_start.QuadPart;

	//nPacketSize = sizeof(packet);
	i = sendto(rawSocket, packet, nPacketSize, 0,
		(struct sockaddr *) &pingaddr, sizeof(struct sockaddr_in6));
	if (i < 0 || i != nPacketSize)//发送成功，则返回值c为packet的长度数192；否则为发送失败
	{
		err.Format(_T("%d"), GetLastError());
		MessageBox(err, TEXT("sendto error"), MB_OK);
		closesocket(rawSocket);
		::WSACleanup();

	}

}