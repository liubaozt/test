using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestWebService
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TestWebService.ServiceReference1.DataServiceSoapClient ss = new TestWebService.ServiceReference1.DataServiceSoapClient();
            DataTable dt=new DataTable();
            ss.fun2(out dt, "");
            string dd = "";
        }
    }
}
