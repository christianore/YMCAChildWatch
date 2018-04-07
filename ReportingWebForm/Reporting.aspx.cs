using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reporting : System.Web.UI.Page
{
    List<String> times = new List<String>(new string[] { "5:00am", "6:00am", "7:00am", "8:00am", "9:00am", "10:00am", "11:00am", "12:00pm"
                                                          ,"1:00pm","2:00pm","3:00pm","4:00pm","5:00pm","6:00pm","7:00pm","8:00pm","9:00pm","10:00pm"});
    protected void Page_Load(object sender, EventArgs e)
    {

        if (IsPostBack)
        {
            argumentDisplay();
        }
        if (!IsPostBack)
        {
            //initialize the drop down list data source
            ddlStartTime.DataSource = times;
            ddlStartTime.DataBind();
            ddlStartTime.SelectedIndex = 0;

            ddlStopTime.DataSource = times;
            ddlStopTime.DataBind();
            ddlStopTime.SelectedIndex = 17;

            //Start with the default session variables

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

        switch (lbxReports.SelectedIndex)
        {
            case 0:
                
                //GetIntervalReport();
                paramsPassed.Text = Session["report"] + ", " + Session["interval"] + ", " + Session["intervalStartTime"] + ", " + Session["intervalEndTime"] + ", " + Session["intervalDate"];
                break;
            case 1:
                paramsPassed.Text = Session["report"] + ", " + Session["memberActiveStatus"];
                break;

            case 2:
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
        Session["memberActiveStatus"] = ddlMemStatus.SelectedIndex;
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