
// ping6Dlg.h : 头文件
//

//zhu
#include <winsock2.h>
#include <ws2tcpip.h>
#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <ws2def.h>
#include <ws2ipdef.h>

#pragma once


// Cping6Dlg 对话框
class Cping6Dlg : public CDialogEx
{
// 构造
public:
	Cping6Dlg(CWnd* pParent = NULL);	// 标准构造函数

// 对话框数据
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_PING6_DIALOG };
#endif

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV 支持


// 实现
protected:
	HICON m_hIcon;

	// 生成的消息映射函数
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedOk();
	afx_msg void OnBnClickedBandwidth();
	//zhu
	BOOL  Ping(const char * pHost,int nNum);//ping主函数
	int  WaitForEchoReply(SOCKET  s);//等待回应
	void SendPacket(void);//发测试包
	//带宽测试
	BOOL  Bandwidth(const char * pHost, int nNum, int nSize);//带宽测试函数
	//int  WaitForEchoReplyB(SOCKET  s);//等待回应
	void SendPacketB(int nBSize);//发可变测试包

	//成员变量


	CString m_pingaddr;
	CString m_pingresult;
	CString m_Times;
	CString m_Size;
	
};
static const int DATALEN = 4000;
//  ICMP6  Header  -  RFC  4443  
typedef  struct  icmp6_hdr
{   //头部8个字节
	u_char            icmp6_type;                          //  Type  类型1字节
	u_char            icmp6_code;                         //  Code  代码1字节
	u_short           icmp6_cksum;                        //  Checksum  校验和2字节
	u_short           icmp6_id;                          //  Identification
	u_short           icmp6_seq;                         //  Sequence  
    //Data  :存储发送时间
	//u_int             icmp6_data[4];  //毫秒级，32位整数，共4*4=16字节
	LONGLONG             icmp6_data[DATALEN]; //微秒级,64位整数，共8*4000=32000字节
} ICMP6_HDR,*PICMP6_HDR;



