using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChildWatchApi.Data;

namespace RunTestCases
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            btnShowMembers.Click += ShowMembersClick;
            btnInterval.Click += ShowIntervalClick;
        }

        public void ShowMembersClick(object o, EventArgs e)
        {
            try
            {
                ReportManager manager = new ReportManager(new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["database"].ToString()));

                grid.DataSource = manager.GetMemberReport();

            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.ToString());
            }

        }

        public void ShowIntervalClick(object o, EventArgs e)
        {
            try
            {
                ReportManager manager = new ReportManager(new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["database"].ToString()));

                grid.DataSource = manager.GetIntervalReport(30, new DateTime(2018,4,11,5,0,0), new DateTime(2018, 4, 11, 22, 0, 0), 1 );

            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.ToString());
            }
        }
    }
}
