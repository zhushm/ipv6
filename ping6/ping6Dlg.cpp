//zhu:����������Թ���Ա�������
//Full version

// ping6Dlg.cpp : ʵ���ļ�
//

#include "stdafx.h"
#include "ping6.h"
#include "ping6Dlg.h"
#include "afxdialogex.h"


#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//zhu:�������
//static const int DEFDATALEN = 56;//8�ֽ�ͷ��+16�ֽ�����+16�ֽ�Դ��ַ+16�ֽ�Ŀ���ַ
//static const int MAXIPLEN = 60;
//static const int MAXICMPLEN = 76;

//΢�����
static const int DEFDATALEN = 32008;//���8�ֽ�ͷ��+32000�ֽ�����
static const int MEASURELEN = 10;//�����������
//static const int MAXIPLEN = DEFDATALEN + 40;//����+8�ֽ�ͷ��+16�ֽ�Դ��ַ+16�ֽ�Ŀ���ַ
//static const int MAXICMPLEN = DEFDATALEN +48;//����+8�ֽ�ͷ��+16�ֽ�Դ��ַ+16�ֽ�Ŀ���ַ+8�ֽ�

//zhu:��̬ȫ�ֱ���
static int myid;// , options;
static struct sockaddr_in6 pingaddr;//Ŀ���ַ
static CString strDestIP, myRTT,RTT;//�ַ���ʽĿ���ַ,˫��ʱ��

static SOCKET  rawSocket;
//static int roundtriptime,totalrtt,minrtt,maxrtt;//zhu:˫��ʱ�ӣ�����
static float roundtriptime, totalrtt, minrtt, maxrtt;//zhu:˫��ʱ�ӣ�΢��
static short nTransmitted,nReceived,nPacketSize;//���͸��������ܸ���������С
//΢���ʱ
double run_time; //����ʱ��
LARGE_INTEGER time_start, time_end;  //��ʼʱ�䣬����ʱ��  
double dqFreq;  //��ʱ��Ƶ��
LARGE_INTEGER f;      //��ʱ��Ƶ��  
//����С����Сʱ������
static int Size[MEASURELEN];
static float MinTime[MEASURELEN];





// ����Ӧ�ó��򡰹��ڡ��˵���� CAboutDlg �Ի���

class CAboutDlg : public CDialogEx
{
public:
	CAboutDlg();

// �Ի�������
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_ABOUTBOX };
#endif

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV ֧��

// ʵ��
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


// Cping6Dlg �Ի���



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

	DDX_Text(pDX, IDC_EDIT_IPaddr, m_pingaddr);//Ŀ��ipv6 address
	DDX_Text(pDX, IDC_EDIT_PingResult, m_pingresult);//ping���
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


// Cping6Dlg ��Ϣ�������

BOOL Cping6Dlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// ��������...���˵�����ӵ�ϵͳ�˵��С�

	// IDM_ABOUTBOX ������ϵͳ���Χ�ڡ�
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

	// ���ô˶Ի����ͼ�ꡣ  ��Ӧ�ó��������ڲ��ǶԻ���ʱ����ܽ��Զ�
	//  ִ�д˲���
	SetIcon(m_hIcon, TRUE);			// ���ô�ͼ��
	SetIcon(m_hIcon, FALSE);		// ����Сͼ��

	// TODO: �ڴ���Ӷ���ĳ�ʼ������

	m_pingaddr = "2001:da8:e000:92::29"; // ipv6.zju.edu.cn(���// "2001:470:0:76::2"; //he.net
	m_Times = "5";
	m_Size = "100";
	UpdateData(FALSE); //�ӱ��������ؼ�

	return TRUE;  // ���ǽ��������õ��ؼ������򷵻� TRUE
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

// �����Ի��������С����ť������Ҫ����Ĵ���
//  �����Ƹ�ͼ�ꡣ  ����ʹ���ĵ�/��ͼģ�͵� MFC Ӧ�ó���
//  �⽫�ɿ���Զ���ɡ�

void Cping6Dlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // ���ڻ��Ƶ��豸������

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// ʹͼ���ڹ����������о���
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// ����ͼ��
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialogEx::OnPaint();
	}
}

//���û��϶���С������ʱϵͳ���ô˺���ȡ�ù��
//��ʾ��
HCURSOR Cping6Dlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}



void Cping6Dlg::OnBnClickedOk()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	//CDialogEx::OnOK();
	//
	// �����ؼ��е����ݱ��浽��Ӧ�ı���   
	UpdateData(TRUE); //�ӿؼ���������
	
	

	//zhu
	//const char * myHost;//Ŀ��������ַ
	CString myIP;
	char *myHost;
	int myNum= _ttoi(m_Times); //���Դ���

	myIP = m_pingaddr;
	//CStringת����char*
	_bstr_t bstr;
	bstr = myIP;
	myHost = bstr;
	//myHost = "2001:470:0:76::2"; //he.net
	//myHost = "2001:12ff:0:4::6"; //nic.br
	myid = (USHORT)GetCurrentThreadId() ;//��ǰ���̺�,16λ
	nTransmitted = 0;
	nReceived = 0;
	totalrtt = 0;
	minrtt = 0;
	maxrtt = 0;
	RTT = "";
	
	//΢���ʱ
	QueryPerformanceFrequency(&f);
	dqFreq = (double)f.QuadPart;//�����ʱ��Ƶ��



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
			RTT.Append(_T("packet size��"));
			//myRTT.Format(_T("%d"),nPacketSize);
			myRTT.Format(_T("%d"), nPacketSize-8);
			RTT.Append(myRTT + "�ֽ�\r\n");

			RTT.Append(_T("ƽ��ʱ�ӣ�"));
			//myRTT.Format(_T("%d"), totalrtt/nReceived);
			myRTT.Format(_T("%5.2f"), totalrtt / nReceived);
			RTT.Append(myRTT + "ms�� ");
			RTT.Append(_T("���ʱ�ӣ�"));
			//myRTT.Format(_T("%d"), maxrtt);
			myRTT.Format(_T("%5.2f"), maxrtt);
			RTT.Append(myRTT + "ms�� ");
			RTT.Append(_T("��Сʱ�ӣ�"));
			//myRTT.Format(_T("%d"), minrtt);
			myRTT.Format(_T("%5.2f"), minrtt);
			RTT.Append(myRTT + "ms\r\n");

			RTT.Append(_T("����ʧ�ʣ�"));
			myRTT.Format(_T("%0.2f"), (float)(nTransmitted-nReceived) / (float)nTransmitted *100);
			RTT.Append(myRTT+"%�� ");
			myRTT.Format(_T("���ͣ�%d�� "), nTransmitted);
			RTT.Append(myRTT);
			myRTT.Format(_T("���գ�%d"), nReceived);
			RTT.Append(myRTT + "\r\n");
			
		}
		else
		{
			RTT.Append(_T("\r\n"));
			RTT.Append(_T("ƽ��˫��ʱ�ӣ���ʱ   "));			
			RTT.Append(_T("����ʧ�ʣ�100%\r\n"));			
			
		}


		m_pingresult = RTT;
	}
	
	// ���ݸ�������ֵ������Ӧ�Ŀؼ����͵ı༭�����ʾm_editSum��ֵ   
	UpdateData(FALSE); //�ӱ��������ؼ�
	//��0��Ϊ�´β�����׼��
	

}

//zhu
//PING��������ֵ1:�ɹ�,0:ʧ��
BOOL Cping6Dlg::Ping(const char * pHost,int nNum)
{
	struct icmp6_hdr *pkt;//?
	int c;
	//char packet[DEFDATALEN + MAXIPLEN + MAXICMPLEN];
	char packet[DEFDATALEN];
	CString err;
	strDestIP = pHost;
	WSADATA  wsaData;
	//����һ��Raw�׽��� 
	if (WSAStartup(0x202, &wsaData)) { //����ֵΪ0�������첽�׽��ֳɹ�
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
	if (inet_pton(AF_INET6, pHost, (void *)&pingaddr.sin6_addr) != 1) //��ַ�ַ���ת����ֵ���ɹ��򷵻�ֵΪ1
	{
		err.Format(_T("%d"), WSAGetLastError());
		MessageBox(err, TEXT("Source IP error"), MB_OK);
		closesocket(rawSocket);
		::WSACleanup();
		return  0;
	}

	//���Ͳ��԰�
	int i, nRet;
	for (i = 0; i < nNum; i++)
	{SendPacket();
	//�ȴ���Ӧ 
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
			{   //˫��ʱ��
				//roundtriptime = GetTickCount();//����
				//roundtriptime -= pkt->icmp6_data[0];
				//΢��ʱ��
				QueryPerformanceCounter(&time_end);   //��ʱ��ʼ
				run_time = 1000000 * (time_end.QuadPart- pkt->icmp6_data[0]) / dqFreq; //����1000000�ѵ�λ���뻯Ϊ΢�룬����Ϊ1000 000/��cpu��Ƶ��΢��
				roundtriptime =(float) run_time/1000.;

				//myRTT.Format(_T("%d"), roundtriptime);
				myRTT.Format(_T("%5.2f"), roundtriptime);
				RTT.Append(myRTT+"ms\r\n");
				//�ۼ�˫��ʱ�Ӻ��հ�����
				nReceived++;
				totalrtt += roundtriptime;
				//������Сʱ��
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
//�ȴ���Ӧ  
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

//�����԰�
void Cping6Dlg::SendPacket()
{
	struct icmp6_hdr *pkt;
	int i;
	CString err;
	nPacketSize = _ttoi(m_Size)+8; //���԰���С 
	//pkt =(struct icmp6_hdr *)malloc(sizeof(int)*nPacketSize);
	char packet[DEFDATALEN];
	
	pkt = (struct icmp6_hdr *) packet;
	pkt->icmp6_type = ICMP6_ECHO_REQUEST;
	pkt->icmp6_code = 0;
	pkt->icmp6_cksum = 0;
	pkt->icmp6_seq = nTransmitted++;
	pkt->icmp6_id = myid;
	//pkt->icmp6_data[0] = GetTickCount(); // ���ڻ�ȡ��windows��������������ʱ�䳤�ȣ����룩
	//΢���ʱ
	QueryPerformanceCounter(&time_start);   //��ʱ��ʼ
	time_start.QuadPart; //��cpu��Ƶ��΢��
	pkt->icmp6_data[0] = time_start.QuadPart;
	
	//nPacketSize = sizeof(packet);
	i = sendto(rawSocket, packet, nPacketSize, 0,
		(struct sockaddr *) &pingaddr, sizeof(struct sockaddr_in6));
	if (i < 0 || i != nPacketSize)//���ͳɹ����򷵻�ֵcΪpacket�ĳ�����192������Ϊ����ʧ��
	{
		err.Format(_T("%d"), GetLastError());
		MessageBox(err, TEXT("sendto error"), MB_OK);
		closesocket(rawSocket);
		::WSACleanup();
		
	}
	
}

//�������
void Cping6Dlg::OnBnClickedBandwidth()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	// �����ؼ��е����ݱ��浽��Ӧ�ı���   
	UpdateData(TRUE); //�ӿؼ���������

	CString myIP;
	char *myHost;
	int myNum = _ttoi(m_Times); //���Դ���
	int mySize = _ttoi(m_Size); //�������������С

	myIP = m_pingaddr;
	//CStringת����char*
	_bstr_t bstr;
	bstr = myIP;
	myHost = bstr;
	//myHost = "2001:470:0:76::2"; //he.net
	//myHost = "2001:12ff:0:4::6"; //nic.br
	myid = (USHORT)GetCurrentThreadId();//��ǰ���̺�,16λ
	nTransmitted = 0;
	nReceived = 0;
	totalrtt = 0;
	minrtt = 0;
	maxrtt = 0;
	RTT = "Data(bytes)\tMin-time(ms)\tLossRate\r\n";
	//΢���ʱ
	QueryPerformanceFrequency(&f);
	dqFreq = (double)f.QuadPart;//�����ʱ��Ƶ��



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
			
			if (MinTime[j] < minTime || minTime == 0)//��С��ʱ��ʱ��
			{
				minTime = MinTime[j];
				i = j;//��ǰ�����ݰ����
			}
			else
			{
				if (j == i + 1)//��Сʱ�Ӱ�����һ����
					diffTime = MinTime[j] - minTime;
			}
			

		}
		if (diffTime == 0)
		{
			RTT.Append(_T("\r\nBandwidth testing failed,try again.\r\n"));
		}
		else
		{
			myRTT.Format(_T("\r\nBandwidth is: %8.2f"), mySize*8*2 /diffTime/1000); //�����λΪMbps��ʱ�䵥λΪms��˫��ʱ�ӵ�һ���ǵ���ʱ�ӣ�
			RTT.Append(myRTT + "Mbps\r\n");
		}
		m_pingresult = RTT;
		
	}

	// ���ݸ�������ֵ������Ӧ�Ŀؼ����͵ı༭�����ʾm_editSum��ֵ   
	UpdateData(FALSE); //�ӱ��������ؼ�
	//��0��Ϊ�´β�����׼��

}

//zhu
//Bandwidth��������ֵ1:�ɹ�,0:ʧ��
BOOL Cping6Dlg::Bandwidth(const char * pHost, int nNum,int nSize)
{
	struct icmp6_hdr *pkt;//?
	int c;
	int nBSize;//���Ͱ�data��С
	char packet[DEFDATALEN];
	CString err;
	strDestIP = pHost;
	WSADATA  wsaData;
	//����һ��Raw�׽��� 
	if (WSAStartup(0x202, &wsaData)) { //����ֵΪ0�������첽�׽��ֳɹ�
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
	if (inet_pton(AF_INET6, pHost, (void *)&pingaddr.sin6_addr) != 1) //��ַ�ַ���ת����ֵ���ɹ��򷵻�ֵΪ1
	{
		err.Format(_T("%d"), WSAGetLastError());
		MessageBox(err, TEXT("Source IP error"), MB_OK);
		closesocket(rawSocket);
		::WSACleanup();
		return  0;
	}

	//���Ͳ��԰�
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
			//�ȴ���Ӧ 
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
				while (1) {//���հ�
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
						{   //˫��ʱ��
							//roundtriptime = GetTickCount();//����
							//roundtriptime -= pkt->icmp6_data[0];
							//΢��ʱ��
							QueryPerformanceCounter(&time_end);   //��ʱ��ʼ
							run_time = 1000000 * (time_end.QuadPart - pkt->icmp6_data[0]) / dqFreq; //����1000000�ѵ�λ���뻯Ϊ΢�룬����Ϊ1000 000/��cpu��Ƶ��΢��
							roundtriptime = (float)run_time / 1000.;
							//�ۼ�˫��ʱ�Ӻ��հ�����
							nReceived++;
							totalrtt += roundtriptime;
							//������Сʱ��
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
//���ɱ䳤�Ȳ��԰�
void Cping6Dlg::SendPacketB(int nBSize)
{
	struct icmp6_hdr *pkt;
	int i;
	CString err;
	nPacketSize = nBSize + 8; //���԰���С 
									 //pkt =(struct icmp6_hdr *)malloc(sizeof(int)*nPacketSize);
	char packet[DEFDATALEN];

	pkt = (struct icmp6_hdr *) packet;
	pkt->icmp6_type = ICMP6_ECHO_REQUEST;
	pkt->icmp6_code = 0;
	pkt->icmp6_cksum = 0;
	pkt->icmp6_seq = nTransmitted++;
	pkt->icmp6_id = myid;
	//pkt->icmp6_data[0] = GetTickCount(); // ���ڻ�ȡ��windows��������������ʱ�䳤�ȣ����룩
	//΢���ʱ
	QueryPerformanceCounter(&time_start);   //��ʱ��ʼ
	time_start.QuadPart; //��cpu��Ƶ��΢��
	pkt->icmp6_data[0] = time_start.QuadPart;

	//nPacketSize = sizeof(packet);
	i = sendto(rawSocket, packet, nPacketSize, 0,
		(struct sockaddr *) &pingaddr, sizeof(struct sockaddr_in6));
	if (i < 0 || i != nPacketSize)//���ͳɹ����򷵻�ֵcΪpacket�ĳ�����192������Ϊ����ʧ��
	{
		err.Format(_T("%d"), GetLastError());
		MessageBox(err, TEXT("sendto error"), MB_OK);
		closesocket(rawSocket);
		::WSACleanup();

	}

}