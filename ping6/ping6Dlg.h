
// ping6Dlg.h : ͷ�ļ�
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


// Cping6Dlg �Ի���
class Cping6Dlg : public CDialogEx
{
// ����
public:
	Cping6Dlg(CWnd* pParent = NULL);	// ��׼���캯��

// �Ի�������
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_PING6_DIALOG };
#endif

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV ֧��


// ʵ��
protected:
	HICON m_hIcon;

	// ���ɵ���Ϣӳ�亯��
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedOk();
	afx_msg void OnBnClickedBandwidth();
	//zhu
	BOOL  Ping(const char * pHost,int nNum);//ping������
	int  WaitForEchoReply(SOCKET  s);//�ȴ���Ӧ
	void SendPacket(void);//�����԰�
	//�������
	BOOL  Bandwidth(const char * pHost, int nNum, int nSize);//������Ժ���
	//int  WaitForEchoReplyB(SOCKET  s);//�ȴ���Ӧ
	void SendPacketB(int nBSize);//���ɱ���԰�

	//��Ա����


	CString m_pingaddr;
	CString m_pingresult;
	CString m_Times;
	CString m_Size;
	
};
static const int DATALEN = 4000;
//  ICMP6  Header  -  RFC  4443  
typedef  struct  icmp6_hdr
{   //ͷ��8���ֽ�
	u_char            icmp6_type;                          //  Type  ����1�ֽ�
	u_char            icmp6_code;                         //  Code  ����1�ֽ�
	u_short           icmp6_cksum;                        //  Checksum  У���2�ֽ�
	u_short           icmp6_id;                          //  Identification
	u_short           icmp6_seq;                         //  Sequence  
    //Data  :�洢����ʱ��
	//u_int             icmp6_data[4];  //���뼶��32λ��������4*4=16�ֽ�
	LONGLONG             icmp6_data[DATALEN]; //΢�뼶,64λ��������8*4000=32000�ֽ�
} ICMP6_HDR,*PICMP6_HDR;



