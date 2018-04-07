using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChildWatchApi.Data.Report;
using ChildWatchApi.Data;
using System.Data;

public partial class Reporting : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

        if (IsPostBack)
        {

            argumentDisplay();
        }
        if (!IsPostBack)
        {
            
            string conn =  ConfigurationManager.ConnectionStrings["childwatchdb"].ToString();
            ReportManager reports = new ReportManager(new SqlConnection(conn));
            OrganizationManager organization = new OrganizationManager(new SqlConnection(conn));
            Location[] locations = organization.GetLocations(1);



            ddlLocation.DataSource = locations;
            ddlLocation.DataBind();

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


    //Display appropriate Divs for report arguments
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
        string conn = ConfigurationManager.ConnectionStrings["childwatchdb"].ToString();
        ReportManager reports = new ReportManager(new SqlConnection(conn));

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
                        GridView1.DataSource = reports.GetMemberReport(memStat);
                        GridView1.DataBind();
                    }
                    else if (ddlMemStatus.SelectedValue.Equals("false"))
                    {
                        memStat = false;
                        
                            try
                            {
                                DataSet ds = new DataSet();
                                //var report = reports.GetMemberReport(memStat);
                                //ds = reports.GetMemberReport(memStat);
                                //GridView1.DataSource = report;
                                //GridView1.DataBind();
                            }
                            catch {

                                }
                    }
                        else
                    {
                        GridView1.DataSource = reports.GetMemberReport();
                        GridView1.DataBind();
                    }
                //GetMemberReport();
                paramsPassed.Text = Session["report"] + ", " + Session["memberActiveStatus"] + ddlMemStatus.SelectedValue;
                break;

            case 2:
                //GetDailyReport();
                paramsPassed.Text = Session["daysStartFrom"] + ", " + Session["daysNumber"];

                break;
            default:

                break;
        }
        
    }

    // Updating session variables when report arguments are changed
    protected void ddlInterval_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (ddlInterval.SelectedIndex){
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
                Session["memberActiveStatus"] = 1;
                break;
            case 1:
                Session["memberActiveStatus"] = 0;
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