using ChildWatchApi.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChildWatchEmployee
{
    public partial class Reports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (IsPostBack)
            {

                // argumentDisplay();
            }
            if (!IsPostBack)
            {
                for(int x = 5; x <= 22; x++)
                {
                    string text = "";
                    int val = x;

                    if(x > 12)
                    {
                        val = x - 12;
                        text = val.ToString() + " PM";
                    }
                    else
                    {
                        if (x == 12)
                            text = val.ToString() + " AM";
                        else
                            text = val.ToString() + " AM";
                    }
                    
                    ddlStartTime.Items.Add(new ListItem(text, x.ToString()));
                    ddlStopTime.Items.Add(new ListItem(text, x.ToString()));
                }
                string conn = ConfigurationManager.ConnectionStrings["database"].ToString();
                ReportManager reports = new ReportManager(new SqlConnection(conn));
                OrganizationManager organization = new OrganizationManager(new SqlConnection(conn));
                //Location[] locations = organization.GetLocations(1);


                //ddlLocation.DataSource = locations;
                //ddlLocation.DataBind();

                //Start with the default session variables
                var app = ConfigurationManager.AppSettings;
                Session["interval"] = 30;   //Interval report, 30 minute bucket
                Session["intervalStartTime"] = "05:00"; //Default start time 5am
                Session["intervalEndTime"] = "22:00"; //Default end time 10pm
                Session["intervalDate"] = DateTime.Now.Date; //Default of current day
                Session["memberActiveStatus"] = "True"; // Member report active only
                Session["daysStartFrom"] = DateTime.Now.Date.AddDays(-7); //Default a week ago
                Session["daysNumber"] = "7"; //default a week
            }

        }


        //Display appropriate Divs for report arguments ONLY NEEDED FOR LIST BOX
        protected void argumentDisplay()
        {
            switch (lbxReports.SelectedIndex)
            {
                case 0:
                    interval.Attributes.Remove("hidden");
                    member.Attributes["hidden"] = "hidden";
                    dayTotals.Attributes["hidden"] = "hidden";
                    break;
                case 1:
                    member.Attributes.Remove("hidden");
                    interval.Attributes["hidden"] = "hidden";
                    dayTotals.Attributes["hidden"] = "hidden";
                    break;

                case 2:
                    dayTotals.Attributes.Remove("hidden");
                    member.Attributes["hidden"] = "hidden";
                    interval.Attributes["hidden"] = "hidden";
                    break;

                default:
                    //should never hit

                    break;
            }
        }

        //take the selected arguments and request for selected report
        protected void btnRunReport_Click(object sender, EventArgs e)
        {
            string conn = ConfigurationManager.ConnectionStrings["database"].ToString();
            ReportManager reports = new ReportManager(new SqlConnection(conn));
            SqlConnection c = new SqlConnection(conn);

            int report = int.Parse(reportSelected.Value);
            Session["SelectedReport"] = report;
            switch (report)
            {
                case 1:
                    int inter = int.Parse(ddlInterval.SelectedValue);

                    DateTime.TryParse(txtDate.Text, out DateTime userDate);

                    DateTime start = new DateTime(userDate.Year, userDate.Month, userDate.Day, int.Parse(ddlStartTime.SelectedValue), 0, 0); //DateTime.Parse(ddlStartTime.SelectedValue + txtDate.Text);
                    DateTime end = new DateTime(userDate.Year, userDate.Month, userDate.Day, int.Parse(ddlStopTime.SelectedValue), 0, 0);
                    // Dates are valid
                    if(start < end)
                    {
                        try
                        {
                            using(SqlCommand runReport = new SqlCommand("r_interval",c ))
                            {
                                runReport.CommandType = CommandType.StoredProcedure;
                                runReport.Parameters.AddRange(new SqlParameter[]
                                {
                                    new SqlParameter("start", start),
                                    new SqlParameter("end", end),
                                    new SqlParameter("interval", inter),
                                    new SqlParameter("location", 1)
                                });

                                c.Open();
                                DataTable table = new DataTable();
                                table.Load(runReport.ExecuteReader());

                                ReportGrid.DataSource = table;
                                ReportGrid.DataBind();
                            }
                        }
                        catch(Exception ex)
                        {

                        }
                        finally
                        {
                            if (c.State == System.Data.ConnectionState.Open)
                                c.Close();
                            c.Dispose();
                        }
                    }
                    
                    interval.Attributes["display"] = "block";
                    member.Attributes["display"] = "none";
                    dayTotals.Attributes["display"] = "none";

                    break;
                case 2:
                // Run a member report
                    string select = ddlMemStatus.SelectedValue;
                    
                    try
                    {
                       using (SqlCommand command = new SqlCommand("r_members", c))
                       {
                            SqlParameter parameter = null;

                            if (!string.IsNullOrEmpty(select))
                            {
                                bool.TryParse(select, out bool selection);
                                parameter = new SqlParameter("active", selection);
                            }
                        
                            command.CommandType = System.Data.CommandType.StoredProcedure;

                            if (parameter != null)
                                command.Parameters.Add(parameter);

                            c.Open();
                            DataTable t = new DataTable();
                            t.Load(command.ExecuteReader());

                            if(t.Rows.Count > 0)
                            {
                                ReportGrid.DataSource = t;
                                ReportGrid.DataBind();
                            }
                       }
                       
                    }
                    catch(Exception ex)
                    {
                        
                    }
                    finally
                    {
                        if (c.State == System.Data.ConnectionState.Open)
                            c.Close();
                        c.Dispose();
                    }

                    break;
                case 3:



                    break;
            }



            /* Reworking for html list this is old listbox code
             * 
            switch (lbxReports.SelectedIndex)
            {
                case 0:

                    //GetIntervalReport();
                    paramsPassed.Text = Session["report"] + ", " + Session["interval"] + ", " + Session["intervalStartTime"] + ", " + Session["intervalEndTime"] + ", " + Session["intervalDate"];
                    break;
                case 1:
                    bool memStat;
                    if (ddlMemStatus.SelectedValue.Equals("true"))
                        {
                        memStat = true;
                            var report = reports.GetMemberReport(memStat);
                            GridView1.DataSource = reports.GetMemberReport(memStat);
                            GridView1.DataBind();
                        }
                        else if (ddlMemStatus.SelectedValue.Equals("false"))
                        {
                            memStat = false;                        
                                try
                                {
                                    var report = reports.GetMemberReport(memStat);
                                    GridView1.DataSource = report;
                                    GridView1.DataBind();
                                }
                                catch {
                                    }
                        }
                            else
                        {
                            GridView1.DataSource = reports.GetMemberReport();
                            GridView1.DataBind();
                        }
                    //GridView1.DataSource = reports.GetMemberReport(Session["memberActiveStatus"]);
                    //GetMemberReport();
                    paramsPassed.Text = Session["report"] + ", " + Session["memberActiveStatus"] + ddlMemStatus.SelectedValue;
                    break;
                case 2:
                //GridView1.DataSource = reports.GetDailyReport();
                    //GetDailyReport();
                    paramsPassed.Text = Session["daysStartFrom"] + ", " + Session["daysNumber"];
                    break;
                default:
                    break;
            }   */
        }



        // Updating session variables when report arguments are changed
        protected void ddlInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlInterval.SelectedIndex)
            {
                case 0:
                    Session["interval"] = 30;
                    break;
                case 1:
                    Session["interval"] = 60;
                    break;
            }
        }

        protected void lbxReports_SelectedIndexChanged(object sender, EventArgs e)
        {
            argumentDisplay();
            Session["report"] = lbxReports.SelectedIndex;
        }
        protected void ddlStartTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["intervalStartTime"] = ddlStartTime.SelectedValue;
        }

        protected void ddlStopTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["intervalEndTime"] = ddlStopTime.SelectedValue;
        }

        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            Session["intervalDate"] = txtDate.Text;
        }

        protected void ddlMemStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlMemStatus.SelectedIndex)
            {
                case 0:
                    Session["memberActiveStatus"] = true;
                    break;
                case 1:
                    Session["memberActiveStatus"] = false;
                    break;
                default:
                    Session["memberActiveStatus"] = null;
                    break;
            }
        }

        protected void txtDateFrom_TextChanged(object sender, EventArgs e)
        {
            Session["daysStartFrom"] = txtDateFrom.Text;
        }

        protected void txtDays_TextChanged(object sender, EventArgs e)
        {
            Session["daysNumber"] = txtDays.Text;
        }
    }
}