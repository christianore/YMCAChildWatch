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
            
            if (IsPostBack){
                
            }
            if (!IsPostBack)
            {
                string conn = ConfigurationManager.ConnectionStrings["database"].ToString();
                ReportManager reports = new ReportManager(new SqlConnection(conn));
                // Set up location selections
                OrganizationManager organization = new OrganizationManager(new SqlConnection(conn));
                Location[] locations = organization.GetLocations();
                ddlLocation.Items.Add(new ListItem("All", "4000"));
                ddlLocInterval.Items.Add(new ListItem("All", "4000"));
                foreach (Location loc in locations)
                {
                    int id = loc.Id;
                    String locaName = loc.Name;
                    ddlLocation.Items.Add(new ListItem(locaName, id.ToString()));
                    ddlLocInterval.Items.Add(new ListItem(locaName, id.ToString()));
                }
                ddlLocation.SelectedValue = "4000";
                ddlLocInterval.SelectedValue = "4000";
                //Start with the default session variables
                reportSelected.Value = "1";
                // datepicker default values
                txtDate.Text = System.DateTime.Today.ToString("MM/dd/yy");
                txtDateTo.Text = System.DateTime.Today.ToString("MM/dd/yy");
                txtDateFrom.Text = System.DateTime.Now.AddDays(-7).ToString("MM/dd/yy");
                //initial load of time drop downs - interval report 5am to 10pm
                for (int x = 5; x <= 22; x++)
                {
                    string text = "";
                    int val = x;
                    if (x > 12)
                    {
                        val = x - 12;
                        text = val.ToString() + " PM";
                    }
                    else
                    {
                        if (x == 12)
                            text = val.ToString() + " PM";
                        else
                            text = val.ToString() + " AM";
                    }
                    selectStart.Items.Add(new ListItem(text, x.ToString()));
                }
                for (int x = 5 + 1; x <= 22; x++)
                {
                    string text = "";
                    int val = x;

                    if (x > 12)
                    {
                        val = x - 12;
                        text = val.ToString() + " PM";
                    }
                    else
                    {
                        if (x == 12)
                            text = val.ToString() + " PM";
                        else
                            text = val.ToString() + " AM";
                    }
                    selectStop.Items.Add(new ListItem(text, x.ToString()));
                }
                ListItem li = selectStop.Items.FindByValue("22");
                li.Selected = true;
            }
        }
        /* Triggered by the Run Report button. Method for passing validated user input
         * into a report request and displaying the output starting at PageIndex of 0
         */
        protected void btnRunReport_Click(object sender, EventArgs e)
        {
            ReportGrid.CurrentPageIndex = 0;
            getReport();
        }
        /* Display alert to user if a date is found to be invalid
         * Return focus to the input field that caused the exception
         */
        public void invalidDateMessage(int dateElement)
        {
            string message = "Entered Date is invalid.";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append("window.onload=function(){");
            sb.Append("alert('");
            sb.Append(message);
            sb.Append("')};");
            sb.Append("</script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
            switch (dateElement)
            {
                case 1:
                    txtDate.Focus();
                    break;
                case 2:
                    txtDateFrom.Focus();
                    break;
                case 3:
                    txtDateTo.Focus();
                    break;
            }
        }
        /* Display alert to user if an error occurs calling the report with valid arguments
         * Denotees issue in database connection
         */
        public void reportError()
        {
            string message = "Error Running Report on Database";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append("window.onload=function(){");
            sb.Append("alert('");
            sb.Append(message);
            sb.Append("')};");
            sb.Append("</script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
        }
        /* For report sets that have multiple pages update data displayed based on 
         * the page selection*/
        protected void ReportGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            ReportGrid.CurrentPageIndex = e.NewPageIndex;
            Session["pagedIndex"] = ReportGrid.CurrentPageIndex;
            ReportGrid.DataBind();
            getReport();
        }
        /*Call the ReportManager and get data back from database based on user input.
         * Handles based on current report selection and calling from paged gridview*/
        protected void getReport()
        {
            ReportManager manager = new ReportManager(new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["database"].ToString()));
            int report = int.Parse(reportSelected.Value);
            Session["SelectedReport"] = report;
            switch (report)
            {
                case 1:
                    //run the interval report
                    int inter = int.Parse(ddlInterval.SelectedValue);
                    int loca = int.Parse(ddlLocInterval.SelectedValue);

                        DateTime userDate;
                    if (DateTime.TryParse(txtDate.Text, out userDate))
                    {
                        DateTime start = new DateTime(userDate.Year, userDate.Month, userDate.Day, int.Parse(selectStart.Value), 0, 0);
                        DateTime end = new DateTime(userDate.Year, userDate.Month, userDate.Day, int.Parse(selectStop.Value), 0, 0);
                        if (start < end)
                        {
                            try
                            {
                                // section to handle returning all locations
                                if (loca == 4000)
                                {
                                    DataTable allInterval = manager.GetIntervalReport(inter, start, end, 0);
                                    allInterval.Columns.Remove("amount");
                                    foreach (ListItem li in ddlLocInterval.Items)
                                    {
                                        if (li.Value != "4000")
                                        {
                                            int loc;
                                            int.TryParse(li.Value, out loc);
                                            DataTable appendee = manager.GetIntervalReport(inter, start, end, loc);
                                            //get the count column from the table
                                            DataColumn appendCol = new DataColumn(li.Text, typeof(int));
                                            allInterval.Columns.Add(appendCol);
                                            int indexNewColumn = allInterval.Columns.IndexOf(li.Text);
                                            appendee.Columns.RemoveAt(0);
                                            int row = 0; // track row number
                                            foreach (DataRow sourcerow in appendee.Rows)
                                            {
                                                allInterval.Rows[row][indexNewColumn] = sourcerow[0];
                                                row = row + 1;
                                            }
                                        }
                                    }
                                    ReportGrid.DataSource = allInterval;
                                    ReportGrid.DataBind();
                                }
                                else
                                {
                                    ReportGrid.DataSource = manager.GetIntervalReport(inter, start, end, loca);
                                    ReportGrid.DataBind();
                                }
                            }
                            catch
                            {
                                reportError();
                            }
                        }
                        else
                        {
                            string message = "Stop Time must be after Start Time";
                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            sb.Append("<script type = 'text/javascript'>");
                            sb.Append("window.onload=function(){");
                            sb.Append("alert('");
                            sb.Append(message);
                            sb.Append("')};");
                            sb.Append("</script>");
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                        }
                    }
                    else
                    {
                        invalidDateMessage(1);
                    }

                        interval.Style.Add("display", "block");
                        member.Style.Add("display", "none");
                        dayTotals.Style.Add("display", "none");
                    break;
                case 2:
                    // Run a member report
                    string select = ddlMemStatus.SelectedValue;
                    try
                    {
                        if (!string.IsNullOrEmpty(select))
                        {
                            bool selection;
                            bool.TryParse(select, out selection);
                            ReportGrid.DataSource = manager.GetMemberReport(selection);
                            ReportGrid.DataBind();
                        }
                        else
                        {
                            ReportGrid.DataSource = manager.GetMemberReport(null);
                            ReportGrid.DataBind();
                        }
                    }
                    catch
                    {
                        reportError();
                    }
                    finally
                    {
                        interval.Style.Add("display", "none");
                        member.Style.Add("display", "block");
                        dayTotals.Style.Add("display", "none");
                    }
                    break;
                case 3:
                    int loc2 = int.Parse(ddlLocation.SelectedValue);
                    DateTime dateFrom;
                    if (DateTime.TryParse(txtDateFrom.Text, out dateFrom))
                    {
                        DateTime dateTo;
                        if(DateTime.TryParse(txtDateTo.Text, out dateTo)) {
                            if (dateFrom < dateTo)
                            {
                                try
                                {                           
                                    // section to handle returning all locations
                                    if (loc2 == 4000)
                                    {
                                        DataTable allDaily = manager.GetDailyReport(dateFrom, dateTo, 0);
                                        allDaily.Columns.Remove("count");
                                        foreach (ListItem li in ddlLocation.Items)
                                        {
                                            if (li.Value != "4000")
                                            {
                                                int loc;
                                                int.TryParse(li.Value, out loc);
                                                DataTable appendee = manager.GetDailyReport(dateFrom, dateTo, loc);
                                                //get the count column from the table
                                                DataColumn appendCol = new DataColumn(li.Text, typeof(int));
                                                allDaily.Columns.Add(appendCol);
                                                int indexNewColumn = allDaily.Columns.IndexOf(li.Text);
                                                //works to here
                                                appendee.Columns.RemoveAt(0);
                                                int row = 0; // track row number
                                                foreach (DataRow sourcerow in appendee.Rows)
                                                {
                                                    allDaily.Rows[row][indexNewColumn] = sourcerow[0];
                                                    row = row + 1;
                                                }
                                            }
                                        }
                                        ReportGrid.DataSource = allDaily;
                                        ReportGrid.DataBind();
                                    }
                                    else
                                    {
                                        ReportGrid.DataSource = manager.GetDailyReport(dateFrom, dateTo, loc2);
                                        ReportGrid.DataBind();
                                    }

                                }catch
                                {
                                    reportError();
                                }
                            }
                            else
                            {
                                invalidDateMessage(3);
                            }
                        }
                        else
                        {
                            invalidDateMessage(3);
                        }
                    }
                    else
                    {
                        invalidDateMessage(2);
                    }
                        interval.Style.Add("display", "none");
                        member.Style.Add("display", "none");
                        dayTotals.Style.Add("display", "block");
                    break;
            }            
            }
        }
    }
