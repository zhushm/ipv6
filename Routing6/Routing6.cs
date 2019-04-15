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


namespace Routing6
{
   public  partial class Routing6 : Form //若要使用指针，则需要采用unsafe不安全的上下文（routing6 项目的属性：“生成”也应配置成“允许不安全代码”）
    {
        public Routing6()
        {
            InitializeComponent();
            

        }

        
        int INFINITY = 65535;           //a number larger than every maximum path
        int MAX_NODES=10;             //maximum number of nodes of Multipath,default is 10，实际节点为1~9
        int MAX_MULTIPATHS=6;         //maximum number of Multipaths, 实际最大寻路数是5
        int MAX_HOPS=9;               //maximum hops,, 实际最大跳数是8

        //node state of QoS routing，满足最大时延和最小带宽的条件下，选择剩余带宽最宽的路径
        struct QState
        {
            public string[,] path; // path from source to this node
            public int[,] length; //length from source to this node: d[v,h+1,p]=d[u,h,p]+d[u,v] 
            public int[,] width; //bandwidth from source to this node: b[v,h+1,p]=min{b[u,h,p], b[u,v]}
            public string[] label; //label state for each hop, P:pemanet, T:tentative
            public int[] ingress; //number of ingress paths  for each hop at this node
        };
        //node state of Dijkstra (Shortest routing)
        struct DState
        {
            public int predecessor; // previous node
            public int length;      //length from Destination to this node
            public string label;    //label state, P:pemanet, T:tentative
        };
       



        private void btnShowNetwork_Click(object sender, EventArgs e)
        {
            //画出节点N1~N9之间的连线
            ShowNetwork();
            
           
        }

        private void btnRunQoSrouting_Click(object sender, EventArgs e)
        {
            //计算满足最大时延和最小带宽的条件下，选择剩余带宽最宽的的两条路径（且瓶颈链路不相交）并输出
            //清空输出结果文本框clear results richtextbox:
            rtbResult.Text = "";
            ShowNetwork(); //清空网络连线
            //Run QoS routng algorithm;
            int s, t;                   //s: Source node   t: Destination(Target node)
            int dt, bt;                 //dt:Max-delay(delay threshold), bt:Min-bandwidth(bandwidth threshold)
            int mp ;                    //output multipaths of an application, mp要小于等于MAX_MULTIPATHS
            //int N, P0, H0;              //number of nodes , number of multipaths and number of hops
            int i, j, m, h, k, q;    // i,j:node in a  network ,m:path in a  network
            int[,,] d, b;                  //distance and bandwidth from u to t at hop h by path p
            d = new int[MAX_NODES, MAX_HOPS, MAX_MULTIPATHS]; //d[u,h,p]:distance from u to t at hop h by path p 
            b = new int[MAX_NODES, MAX_HOPS, MAX_MULTIPATHS]; //b[u,h,p]:bandwidth from u to t at hop h by path p
            int[,] band, delay;         //bandwidth and delay
            band = new int[MAX_NODES, MAX_NODES];   //band[i,j] is the bandwidth of path from i to j 
            delay = new int[MAX_NODES, MAX_NODES];  //delay[i,j] is the delay of path from i to j 
            QState[] state;
            state = new QState[MAX_NODES];     //state of a node after each computation
            int min, max;
            int[] hop, path;   //hop and path of each path
            hop = new int[MAX_MULTIPATHS];
            path = new int[MAX_MULTIPATHS];
            string paths; //results


            //初始化
            //initiate delay[] and band[]
            for (i = 1; i < MAX_NODES; i++)
            {
                for (j = 1; j < MAX_NODES; j++)
                {
                    if (i == j)
                    {
                        delay[i, j] = 0;
                        band[i, j] = INFINITY;
                    }
                    else
                    {
                        delay[i, j] = INFINITY;
                        band[i, j] = 0;
                    }
                }
            }
            //setup delay and Bandwidth in Network Graph,假设其双向时延和带宽都是一样的
            delay[1, 2] = int.Parse(txtDelay1_2.Text);      delay[2, 1] = int.Parse(txtDelay1_2.Text);
            band[1, 2] = int.Parse(txtBandwidth1_2.Text);   band[2, 1] = int.Parse(txtBandwidth1_2.Text);
            delay[1, 3] = int.Parse(txtDelay1_3.Text);      delay[3, 1] = int.Parse(txtDelay1_3.Text);
            band[1, 3] = int.Parse(txtBandwidth1_3.Text);   band[3, 1] = int.Parse(txtBandwidth1_3.Text);

            delay[2, 4] = int.Parse(txtDelay2_4.Text);      delay[4, 2] = int.Parse(txtDelay2_4.Text);
            band[2, 4] = int.Parse(txtBandwidth2_4.Text);   band[4, 2] = int.Parse(txtBandwidth2_4.Text);
            delay[2, 5] = int.Parse(txtDelay2_5.Text);      delay[5, 2] = int.Parse(txtDelay2_5.Text);
            band[2, 5] = int.Parse(txtBandwidth2_5.Text);   band[5, 2] = int.Parse(txtBandwidth2_5.Text);
            delay[2, 7] = int.Parse(txtDelay2_7.Text);      delay[7, 2] = int.Parse(txtDelay2_7.Text);
            band[2, 7] = int.Parse(txtBandwidth2_7.Text);   band[7, 2] = int.Parse(txtBandwidth2_7.Text);

            delay[3, 4] = int.Parse(txtDelay3_4.Text);      delay[4, 3] = int.Parse(txtDelay3_4.Text);
            band[3, 4] = int.Parse(txtBandwidth3_4.Text);   band[4, 3] = int.Parse(txtBandwidth3_4.Text);
            delay[3, 8] = int.Parse(txtDelay3_8.Text);      delay[8, 3] = int.Parse(txtDelay3_8.Text);
            band[3, 8] = int.Parse(txtBandwidth3_8.Text);   band[8, 3] = int.Parse(txtBandwidth3_8.Text);

            delay[4, 6] = int.Parse(txtDelay4_6.Text);      delay[6, 4] = int.Parse(txtDelay4_6.Text);
            band[4, 6] = int.Parse(txtBandwidth4_6.Text);   band[6, 4] = int.Parse(txtBandwidth4_6.Text);
            delay[5, 7] = int.Parse(txtDelay5_7.Text);      delay[7, 5] = int.Parse(txtDelay5_7.Text);
            band[5, 7] = int.Parse(txtBandwidth5_7.Text);   band[7, 5] = int.Parse(txtBandwidth5_7.Text);

            delay[6, 7] = int.Parse(txtDelay6_7.Text);      delay[7, 6] = int.Parse(txtDelay6_7.Text);
            band[6, 7] = int.Parse(txtBandwidth6_7.Text);   band[7, 6] = int.Parse(txtBandwidth6_7.Text);
            delay[6, 8] = int.Parse(txtDelay6_8.Text);      delay[8, 6] = int.Parse(txtDelay6_8.Text);
            band[6, 8] = int.Parse(txtBandwidth6_8.Text);   band[8, 6] = int.Parse(txtBandwidth6_8.Text);

            delay[7, 9] = int.Parse(txtDelay7_9.Text);      delay[9, 7] = int.Parse(txtDelay7_9.Text);
            band[7, 9] = int.Parse(txtBandwidth7_9.Text);   band[9, 7] = int.Parse(txtBandwidth7_9.Text);
            delay[8, 9] = int.Parse(txtDelay8_9.Text);      delay[9, 8] = int.Parse(txtDelay8_9.Text);
            band[8, 9] = int.Parse(txtBandwidth8_9.Text);   band[9, 8] = int.Parse(txtBandwidth8_9.Text);

            //setup application parameters
            s = int.Parse(txtSource.Text);       // source  node
            t = int.Parse(txtDestination.Text); // destination node
            dt = int.Parse(txtMaxDelay.Text);      //delay threshold
            bt = int.Parse(txtMinBandwidth.Text);  //'bandwidth threshold
            mp = 2;        //要输出的路径数量
            int N = MAX_NODES - 1; //实际节点为1~9  
            if (s > N || t > N || dt <= 0 || bt < 0 || mp >= MAX_MULTIPATHS || mp <= 0 || s <= 0 || t <= 0)
                MessageBox.Show("Please input correct parameters", "Prompt:");

            else
            {
                //Begin :From destination to compute widest path with distance QoS

                for (j = 1; j <= mp; j++) //each path
                {
                    //'initiate state of each node	
                    for (i = 1; i <= N; i++)
                    {
                        //initiate the members of each node
                        state[i].path = new string[MAX_HOPS, MAX_MULTIPATHS];
                        state[i].length = new int[MAX_HOPS, MAX_MULTIPATHS];
                        state[i].width = new int[MAX_HOPS, MAX_MULTIPATHS];
                        state[i].label = new string[MAX_HOPS];
                        state[i].ingress = new int[MAX_HOPS];

                        for (h = 0; h < MAX_HOPS; h++) //  
                        {
                            for (m = 1; m < MAX_MULTIPATHS; m++)    
                            {
                                if (i == s)
                                {
                                    state[s].path[h, m] = " " + s.ToString() + " ";
                                    state[s].length[h, m] = 0;
                                    state[s].width[h, m] = INFINITY;
                                    state[s].ingress[h] = 1;
                                    b[s, h, m] = INFINITY;
                                    d[s, h, m] = 0;
                                }
                                else
                                {
                                    state[i].path[h, m] = " ";
                                    state[i].length[h, m] = INFINITY;
                                    state[i].width[h, m] = 0;
                                    state[i].label[h] = "";
                                    state[i].ingress[h] = 0;
                                    b[i, h, m] = 0;
                                    d[i, h, m] = 0;
                                }

                            }
                        }
                    }
                    state[s].label[0] = "T";    // s is the initial working node 
                    for (h = 0; h < MAX_HOPS; h++) //  each hop
                    {
                        for (k = 1; k <= N; k++) //'this is a working node u at this hop
                        {
                            if (state[k].label[h] == "T" && k != t)
                            {
                                for (m = 1; m <= state[k].ingress[h]; m++)
                                {
                                    //select the widest path with distance<D between u and i
                                    for (i = 1; i <= N; i++)   //this is a neighbor v 
                                    {
                                        q = state[i].ingress[h + 1] + 1;
                                        if (delay[k, i] > 0 && d[k, h, m] + delay[k, i] <= dt && band[k, i] >= bt && state[k].path[h, m].IndexOf(i.ToString()) <= 0)
                                            {

                                            if (b[k, h, m] > band[k, i])
                                                b[i, h + 1, q] = band[k, i];
                                            else
                                                b[i, h + 1, q] = b[k, h, m];

                                            d[i, h + 1, q] = d[k, h, m] + delay[k, i];
                                            //update the status of node i
                                            state[i].ingress[h + 1] = q;
                                            state[i].path[h + 1, q] = state[k].path[h, m] + i.ToString() + " ";
                                            state[i].length[h + 1, q] = d[i, h + 1, q];
                                            state[i].width[h + 1, q] = b[i, h + 1, q];
                                            state[i].label[h + 1] = "T";
                                        }
                                    } //end of each i
                                } //end of each m
                                state[k].label[h] = "P";
                            }  //end if
                        } //end of each k
                    } //end of each hop h
                    //select the shortest and wedest path from s to t
                    min = INFINITY;
                    max = 0;
                    for (h = 0; h < MAX_HOPS; h++)
                    {
                        for (m = 1; m < MAX_MULTIPATHS; m++)
                        {
                            if (state[t].length[h, m] <= dt && state[t].width[h, m] > max) //' select the widest one;
                            {
                                max = state[t].width[h, m];
                                hop[j] = h;
                                path[j] = m;
                            }
                            else if (state[t].length[h, m] <= dt && state[t].width[h, m] == max && state[t].width[h, m] > 0) //the same widest, select the shortest one
                                if (state[t].length[h, m] < state[t].length[hop[j], path[j]])
                                {
                                    max = state[t].width[h, m];
                                    hop[j] = h;
                                    path[j] = m;
                                }

                        } // end of m
                    } //end of h
                    //modify the residual bandwith of each link
                    paths = state[t].path[hop[j], path[j]];
                    k = 0;
                    if (paths == "" || paths == null)
                    {
                        paths = "path " + j.ToString() + ":" + "-";
                    }
                    else //非空
                    {

                        while (paths.Length > 3)
                        {
                            i = paths.IndexOf(" ");
                            h = int.Parse(paths.Substring(i + 1, 2));
                            m = int.Parse(paths.Substring(i + 3, 2));
                            band[h, m] -= max;
                            band[m, h] -= max;
                            paths = paths.Substring(i + 2, paths.Length - 2);
                            k += 1;
                        }

                        //output multipaths
                        ShowPath(j, state[t].path[hop[j], path[j]]);
                        paths = "path " + j.ToString() + ":" + state[t].path[hop[j], path[j]] + ",   bandwidth:" + state[t].width[hop[j], path[j]] + "M, delay:" + state[t].length[hop[j], path[j]] + "ms, hops:" + k.ToString();
                    }
                    rtbResult.AppendText(paths + "\n");
                } // end of each path
            } // end if
        }

        private void btnDijkstra_Click(object sender, EventArgs e)
        {

            txtShortest.Text = "";//清空输出结果文本框
            ShowNetwork(); //清空网络连线
            int n;  //number of nodes 
            int i, k;   // node in a  network
            int[,] dist;
            dist = new int[MAX_NODES, MAX_NODES];   //dist[i,j] is the distance from i to k 
            DState[] state; //state of each node
            state = new DState[MAX_NODES];
            int s, t;   // s: Source node   t: Destination node
            int min;
            int[] path;
            path = new int[MAX_NODES];
            string paths="";
            n = MAX_NODES - 1;

            //initiate dist[]
            for (i = 0; i < MAX_NODES; i++)
            {
                for (k = 0; k < MAX_NODES; k++)
                {
                    if (i == k)
                        dist[i, k] = 0;
                    else
                        dist[i, k] = INFINITY;
                }
            }
            //setup dist[] in Graph，距离是双向的
            
            dist[1, 2] = int.Parse(txtDelay1_2.Text); dist[2, 1] = int.Parse(txtDelay1_2.Text);
            dist[1, 3] = int.Parse(txtDelay1_3.Text); dist[3, 1] = int.Parse(txtDelay1_3.Text);

            dist[2, 4] = int.Parse(txtDelay2_4.Text); dist[4, 2] = int.Parse(txtDelay2_4.Text);
            dist[2, 5] = int.Parse(txtDelay2_5.Text); dist[5, 2] = int.Parse(txtDelay2_5.Text);
            dist[2, 7] = int.Parse(txtDelay2_7.Text); dist[7, 2] = int.Parse(txtDelay2_7.Text);

            dist[3, 4] = int.Parse(txtDelay3_4.Text); dist[4, 3] = int.Parse(txtDelay3_4.Text);
            dist[3, 8] = int.Parse(txtDelay3_8.Text); dist[8, 3] = int.Parse(txtDelay3_8.Text);

            dist[4, 6] = int.Parse(txtDelay4_6.Text); dist[6, 4] = int.Parse(txtDelay4_6.Text);
            dist[5, 7] = int.Parse(txtDelay5_7.Text); dist[7, 5] = int.Parse(txtDelay5_7.Text);

            dist[6, 7] = int.Parse(txtDelay6_7.Text); dist[7, 6] = int.Parse(txtDelay6_7.Text);
            dist[6, 8] = int.Parse(txtDelay6_8.Text); dist[8, 6] = int.Parse(txtDelay6_8.Text);

            dist[7, 9] = int.Parse(txtDelay7_9.Text); dist[9, 7] = int.Parse(txtDelay7_9.Text);
            dist[8, 9] = int.Parse(txtDelay8_9.Text); dist[9, 8] = int.Parse(txtDelay8_9.Text);
            //initiate state of each node			
            for (i = 1; i <= n; i++)
            {
                state[i].predecessor = -1;
                state[i].length = INFINITY;
                state[i].label = "T";
            }
            //setup source and destination node
            s = int.Parse(txtSource.Text);
            t = int.Parse(txtDestination.Text);
            //run Dijkstra algorithm
            //From destination to compute shortest path
            if (s != t)
            {
                state[t].length = 0;
                state[t].label = "P";
                k = t;  // k is the initial working node 
                do
                {
                    //Is there a better path from k ? 
                    for (i = 1; i <= n; i++) //this graph has n nodes
                    {
                        if (dist[k, i] != 0 && state[i].label == "T")
                            if (state[k].length + dist[k, i] < state[i].length)
                            {
                                state[i].predecessor = k;
                                state[i].length = state[k].length + dist[k, i];
                            }

                    }

                    //Find the tentatively labeled node with the smallest distance. 
                    k = 1;
                    min = INFINITY;
                    for (i = 1; i <= n; i++)
                    {
                        if (state[i].label == "T" && state[i].length < min)
                        {
                            min = state[i].length;
                            k = i;
                        }
                    }
                    state[k].label = "P";
                }
                while (k != s);
                //Copy the path into the output array.
                i = 1;
                k = s;
                do //output
                {
                    paths += " " + k.ToString();
                    path[i] = k;
                    k = state[k].predecessor;
                    i += 1;
                }
                while (k >= 0);
                //
                ShowPath(0, paths);
                txtShortest.Text = "Shortest path:" + paths + "   delay:" + state[s].length + " hops:" + (i - 2).ToString();
            }
            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtShortest.Text = "";
            rtbResult.Text = "";

        }

        //画出节点N1~N9之间的连线
        private void ShowNetwork()
        {
            System.Drawing.Pen myPen;

            // 画笔颜色  

            myPen = new System.Drawing.Pen(System.Drawing.Color.Black, 2);
            System.Drawing.Graphics formGraphics = this.CreateGraphics();

            // 画线  

            formGraphics.DrawLine(myPen, 50, 150, 150, 80);     //N1~N2
            formGraphics.DrawLine(myPen, 50, 150, 150, 220);    //N1~N3
            formGraphics.DrawLine(myPen, 150, 80, 250, 150);    //N2~N4
            formGraphics.DrawLine(myPen, 150, 80, 350, 10);     //N2~N5
            formGraphics.DrawLine(myPen, 150, 80, 550, 80);     //N2~N7
            formGraphics.DrawLine(myPen, 150, 220, 250, 150);   //N3~N4
            formGraphics.DrawLine(myPen, 150, 220, 550, 220);   //N3~N8

            formGraphics.DrawLine(myPen, 250, 150, 450, 150);   //N4~N6
            formGraphics.DrawLine(myPen, 350, 10, 550, 80);     //N5~N7
            formGraphics.DrawLine(myPen, 450, 150, 550, 80);    //N6~N7
            formGraphics.DrawLine(myPen, 450, 150, 550, 220);   //N6~N8
            formGraphics.DrawLine(myPen, 550, 80, 650, 150);    //N7~N9
            formGraphics.DrawLine(myPen, 550, 220, 650, 150);   //N8~N9

            //给各链路和带宽赋值
            L1_2.Text = txtDelay1_2.Text + "ms, " + txtBandwidth1_2.Text + "M";
            L1_3.Text = txtDelay1_3.Text + "ms, " + txtBandwidth1_3.Text + "M";
            L2_4.Text = txtDelay2_4.Text + "ms, " + txtBandwidth2_4.Text + "M";
            L2_5.Text = txtDelay2_5.Text + "ms, " + txtBandwidth2_5.Text + "M";
            L2_7.Text = txtDelay2_7.Text + "ms, " + txtBandwidth2_7.Text + "M";
            L3_4.Text = txtDelay3_4.Text + "ms, " + txtBandwidth3_4.Text + "M";
            L3_8.Text = txtDelay3_8.Text + "ms, " + txtBandwidth3_8.Text + "M";
            L4_6.Text = txtDelay4_6.Text + "ms, " + txtBandwidth4_6.Text + "M";

            L5_7.Text = txtDelay5_7.Text + "ms, " + txtBandwidth5_7.Text + "M";
            L6_7.Text = txtDelay6_7.Text + "ms, " + txtBandwidth6_7.Text + "M";
            L6_8.Text = txtDelay6_8.Text + "ms, " + txtBandwidth6_8.Text + "M";
            L7_9.Text = txtDelay7_9.Text + "ms, " + txtBandwidth7_9.Text + "M";
            L8_9.Text = txtDelay8_9.Text + "ms, " + txtBandwidth8_9.Text + "M";

            //释放画笔和画布资源  

            myPen.Dispose();

            formGraphics.Dispose();
        }
        //画出路径path
        private void ShowPath(int j,string path)
        {
            
            System.Drawing.Pen myPen;
            path = path.Trim();
            // 画笔颜色  
            switch (j)
            {
                case 0: //Dijkstra_Shortest path
                    myPen = new System.Drawing.Pen(System.Drawing.Color.Green, 2);
                    break;
                case 1: //QoS routing path1
                    myPen = new System.Drawing.Pen(System.Drawing.Color.Red, 2);
                    break;
                case 2: //QoS routing path2
                    myPen = new System.Drawing.Pen(System.Drawing.Color.Blue, 2);
                    break;
                default:
                    myPen = new System.Drawing.Pen(System.Drawing.Color.Black, 2);
                    break;
            }
            System.Drawing.Graphics formGraphics = this.CreateGraphics();
            int i, k,len; //i,k:each node;
            int x1, x2, y1,y2; //坐标;
            len = path.Length;
            if (len>=3)
            {
                do
                { i = int.Parse(path.Substring(0, 1));
                    k = int.Parse(path.Substring(2, 1));
                    path = path.Substring(2, len - 2);
                    len = len - 2;
                    //Draw line from i to k
                    switch (i) //x1和y1坐标
                    {
                        case 1:
                            x1 = 50; y1 = 150;
                            break;
                        case 2:
                            x1 = 150; y1 = 80;
                            break;
                        case 3:
                            x1 = 150; y1 = 220;
                            break;
                        case 4:
                            x1 = 250; y1 = 150;
                            break;
                        case 5:
                            x1 = 350; y1 = 10;
                            break;
                        case 6:
                            x1 = 450; y1 = 150;
                            break;
                        case 7:
                            x1 = 550; y1 = 80;
                            break;
                        case 8:
                            x1 = 550; y1 = 220;
                            break;
                        case 9:
                            x1 = 650; y1 = 150;
                            break;
                        default:
                            x1 = 0; y1 = 0;
                            break;
                    }
                    switch (k) //x2和y2坐标
                    {
                        case 1:
                            x2 = 50; y2 = 150;
                            break;
                        case 2:
                            x2 = 150; y2 = 80;
                            break;
                        case 3:
                            x2 = 150; y2 = 220;
                            break;
                        case 4:
                            x2 = 250; y2 = 150;
                            break;
                        case 5:
                            x2 = 350; y2 = 10;
                            break;
                        case 6:
                            x2 = 450; y2 = 150;
                            break;
                        case 7:
                            x2 = 550; y2 = 80;
                            break;
                        case 8:
                            x2 = 550; y2 = 220;
                            break;
                        case 9:
                            x2 = 650; y2 = 150;
                            break;
                        default:
                            x2 = 0; y2 = 0;
                            break;
                    }
                    //画线
                    formGraphics.DrawLine(myPen, x1, y1, x2, y2);     //Node i~Node k


                }
                while (len >= 3);
            }
            //释放画笔和画布资源  
            myPen.Dispose();
           formGraphics.Dispose();
        }
        
        private void Routing6_Load(object sender, EventArgs e)
        {
            //MessageBox.Show("aaa");
           
        }
        private void Routing6_Shown(object sender, EventArgs e)
        { ShowNetwork(); }

        //切换到FileTransfer
        private void btnFileTransfer_Click(object sender, EventArgs e)
        {
            Hide();
            //FileTransfer form1 = new FileTransfer();
            //form1.ShowDialog();
            this.Close();
        }

        private void lblResult_Click(object sender, EventArgs e)
        {

        }
    }
}
