using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Multipath6
{
    public partial class MultipathForm : Form
    {
        public MultipathForm()
        {
            InitializeComponent();
        }

        private void routingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Routing6 form1 = new Routing6();
            form1.ShowDialog();
        }

        private void multipathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileTransfer form1 = new FileTransfer();
            form1.ShowDialog();
        }
        //Server
        private void serverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FileServer form1 = new FileServer();
            //form1.ShowDialog();
            FileServer form1 = new FileServer();
            form1.Show();

            //FileServer childForm = new FileServer();//子窗体
            //childForm.MdiParent = this;
            //childForm.WindowState = FormWindowState.Maximized;//初始为最大
            //childForm.Show();
        }
        //Client
        private void clientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FileClient form1 = new FileClient();
            //form1.ShowDialog();
            FileClient form1 = new FileClient();
            form1.Show();
            //FileClient childForm = new FileClient();//子窗体
            //childForm.MdiParent = this;
            //childForm.WindowState = FormWindowState.Maximized;//初始为最大
            //childForm.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
