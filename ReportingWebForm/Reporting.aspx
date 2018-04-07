﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Reporting.aspx.cs" Inherits="Reporting" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>YMCA ChildWatch Reporting Portal</title>

    <link rel="stylesheet" href="style.css" type="text/css"/>
    <script src="Scripts/jquery-1.10.2.min.js"></script>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css"/>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />


    <script>  
        $(function ()  
        {  
            $('#<%=txtDate.ClientID %>').datepicker(  
            {  
                dateFormat: 'mm/dd/yy',  
                changeMonth: true,  
                changeYear: true,  
                yearRange: '2018:2100',
            });  
        })
        $(function ()  
        {  
            $('#<%=txtDateFrom.ClientID %>').datepicker(  
            {  
                dateFormat: 'mm/dd/yy',  
                changeMonth: true,  
                changeYear: true,  
                yearRange: '2018:2100'  
            });  
        })

        $(document).ready(function () {
            function close_accordion_section() {
                $('.accordion .accordion-section-title').removeClass('active');
                $('.accordion .accordion-section-content').slideUp(300).removeClass('open');
            }
            $('.accordion-section-title').click(function (e) {
                // Grab current anchor value
                var currentAttrValue = $(this).attr('href');
                if ($(e.target).is('.active')) {
                    close_accordion_section();
                } else {
                    close_accordion_section();
                    // Add active class to section title
                    $(this).addClass('active');
                    // Open up the hidden content panel
                    $('.accordion ' + currentAttrValue).slideDown(300).addClass('open');
                }
                e.preventDefault();
            });
        });

</script>


</head>
<body>
    <form id="form1" runat="server">
        <header>
            <img src="YMCALogo.JPG" width="75" height="75"/>
        </header>
        <div id="reportAside" class="aside" runat="server">
            <p><strong>Reports</strong></p>
            <asp:ListBox ID="lbxReports" CssClass="lbxReports" runat="server" Font-Size="Large" Width="193px" OnSelectedIndexChanged="lbxReports_SelectedIndexChanged" AutoPostBack="True">
                <asp:ListItem Selected="True">Interval Report</asp:ListItem>
                <asp:ListItem>Registered Members</asp:ListItem>
                <asp:ListItem>Daily Report</asp:ListItem>
            </asp:ListBox>
        </div>
    <div id="reportArguments" class ="accordion">
        
           
            <div id="interval" class="interval" runat="server">



                    <div class="accordion-section">
                        <a class="accordion-section-title" href="#accordion-1"><strong>Selected: Interval Report</strong></a>
                            <div id="accordion-1" class="accordion-section-content">
                                <p>
                                     Displays headcount for each location broken down by time interval.<br/>

                                </p>   
                                <ul id="intervalList" class="list">
                                    <li>Time Interval: Determines the reports grouping for the headcount</li>                
                                    <li>Start Time: When to begin counting</li>
                                    <li>End Time: When to stop counting</li>
                                    <li>Date: Select the date for which the report should return information</li>
                                </ul>                
                
                            </div><!--end .accordion-section-content-->
                        </div><!--end .accordion-section-->

                 
                <div id="centerDiv" class="centerDiv">
                    <asp:Label ID="Label1" runat="server" Text="Time Interval: " Width="150px"></asp:Label><asp:DropDownList ID="ddlInterval" runat="server" OnSelectedIndexChanged="ddlInterval_SelectedIndexChanged">
                        <asp:ListItem Selected="True">30 min</asp:ListItem>
                        <asp:ListItem>1 hour</asp:ListItem>
                    </asp:DropDownList>               
                         <br />
                    <asp:Label ID="Label3" runat="server" Text="Start Time:" Width="150px"></asp:Label><asp:DropDownList ID="ddlStartTime" runat="server" OnSelectedIndexChanged="ddlStartTime_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="05:00">5 am</asp:ListItem>
                        <asp:ListItem Value="06:00">6 am</asp:ListItem>
                        <asp:ListItem Value="07:00">7 am</asp:ListItem>
                        <asp:ListItem Value="08:00">8 am</asp:ListItem>
                        <asp:ListItem Value="09:00">9 am</asp:ListItem>
                        <asp:ListItem Value="10:00">10 am</asp:ListItem>
                        <asp:ListItem Value="11:00">11 am</asp:ListItem>
                        <asp:ListItem Value="12:00">12 pm</asp:ListItem>
                        <asp:ListItem Value="13:00">1 pm</asp:ListItem>
                        <asp:ListItem Value="14:00">2 pm</asp:ListItem>
                        <asp:ListItem Value="15:00">3 pm</asp:ListItem>
                        <asp:ListItem Value="16:00">4 pm</asp:ListItem>
                        <asp:ListItem Value="17:00">5 pm</asp:ListItem>
                        <asp:ListItem Value="18:00">6 pm</asp:ListItem>
                        <asp:ListItem Value="19:00">7 pm</asp:ListItem>
                        <asp:ListItem Value="20:00">8 pm</asp:ListItem>
                        <asp:ListItem Value="21:00">9 pm</asp:ListItem>
                        <asp:ListItem Value="22:00">10 pm</asp:ListItem>
                    </asp:DropDownList>
                        <br />
                    <asp:Label ID="Label4" runat="server" Text="End Time:" Width="150px"></asp:Label><asp:DropDownList ID="ddlStopTime" runat="server" OnSelectedIndexChanged="ddlStopTime_SelectedIndexChanged">
                        <asp:ListItem Value="05:00">5 am</asp:ListItem>
                        <asp:ListItem Value="06:00">6 am</asp:ListItem>
                        <asp:ListItem Value="07:00">7 am</asp:ListItem>
                        <asp:ListItem Value="08:00">8 am</asp:ListItem>
                        <asp:ListItem Value="09:00">9 am</asp:ListItem>
                        <asp:ListItem Value="10:00">10 am</asp:ListItem>
                        <asp:ListItem Value="11:00">11 am</asp:ListItem>
                        <asp:ListItem Value="12:00">12 pm</asp:ListItem>
                        <asp:ListItem Value="13:00">1 pm</asp:ListItem>
                        <asp:ListItem Value="14:00">2 pm</asp:ListItem>
                        <asp:ListItem Value="15:00">3 pm</asp:ListItem>
                        <asp:ListItem Value="16:00">4 pm</asp:ListItem>
                        <asp:ListItem Value="17:00">5 pm</asp:ListItem>
                        <asp:ListItem Value="18:00">6 pm</asp:ListItem>
                        <asp:ListItem Value="19:00">7 pm</asp:ListItem>
                        <asp:ListItem Value="20:00">8 pm</asp:ListItem>
                        <asp:ListItem Value="21:00">9 pm</asp:ListItem>
                        <asp:ListItem Selected="True" Value="22:00">10 pm</asp:ListItem>
                    </asp:DropDownList>
                                   <br />
                    <asp:Label ID="Label2"  runat="server" Text="Date:" Width="150px"></asp:Label><asp:TextBox ID="txtDate" cssclass="datePick" runat="server" OnTextChanged="txtDate_TextChanged"></asp:TextBox>
               </div> 
            </div>
        
        <div id="member" class="member" hidden="hidden" runat="server">

                <div class="accordion-section">
                    <a class="accordion-section-title" href="#accordion-2"><strong>Selected: Member Information Report</strong></a>
                        <div id="accordion-2" class="accordion-section-content">
                            <p>
                                 Displays information about YMCA members registered in the applicatio system.<br />
                                Member Status: Select to view information for active YMCA membership status, inactive, or both.
                            </p>
                        </div><!--end .accordion-section-content-->
                    </div><!--end .accordion-section-->

            
                <div id="centerDiv2" class="centerDiv">
                    <asp:Label ID="Label6" runat="server" Text="Member Status" Width="150px"></asp:Label><asp:DropDownList ID="ddlMemStatus" runat="server" OnSelectedIndexChanged="ddlMemStatus_SelectedIndexChanged">
                        <asp:ListItem Value="true" Selected="True">Active</asp:ListItem>
                        <asp:ListItem Value="false">Inactive</asp:ListItem>
                        <asp:ListItem Value="">Both</asp:ListItem>
                    </asp:DropDownList>
                </div>               
        </div>
        
        <div id="dayTotals" class="dayTotals" hidden="hidden" runat="server">

                    <div class="accordion-section">
                        <a class="accordion-section-title" href="#accordion-3"><strong>Selected: Daily Report</strong></a>
                            <div id="accordion-3" class="accordion-section-content">
                                <p>
                                    Displays totals for each day over a given time frame<br />
                                    Start Date: Enter the day to begin search<br />
                                    Days: The number of days past selected start date that should be included.
                                </p>
                            </div><!--end .accordion-section-content-->
                        </div><!--end .accordion-section-->
              
            <div id="centerDiv3" class="centerDiv">
                <asp:Label ID="Label7" runat="server" Text="Start Date:" Width="150px"></asp:Label><asp:TextBox ID="txtDateFrom" runat="server" OnTextChanged="txtDateFrom_TextChanged" cssclass="form-control"></asp:TextBox>
                    <br />
                <asp:Label ID="Label5" runat="server" Text="Days:" Width="150px"></asp:Label><asp:TextBox ID="txtDays" runat="server" OnTextChanged="txtDays_TextChanged" cssclass="form-control"></asp:TextBox>
                <asp:Label ID="Label8" runat="server" Text="Location:"></asp:Label><asp:DropDownList ID="ddlLocation" runat="server"></asp:DropDownList>
            
            </div>
                
        </div>
        <div id="centerDiv4" class="centerDiv">
            <asp:Button ID="btnRunReport" runat="server" Text="Run Report" BackColor="#FF6600" BorderColor="#FF6600" BorderStyle="Solid" Font-Bold="True" Font-Size="Medium" ForeColor="White" OnClick="btnRunReport_Click" />
        </div>
    </div>
    <div id="reportDisplay" class="reportDisplay">
        <p>
        <asp:label ID="rs" runat="server"></asp:label>  
              Show the Result Set Here!
        <asp:Label ID="paramsPassed" runat="server" Text="Label"></asp:Label>
        </p>

        <asp:GridView ID="GridView1" runat="server"></asp:GridView>
    </div>

        <asp:SqlDataSource ID="SqlDataSource1" runat="server" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:childwatchdb %>"></asp:SqlDataSource>

    </form>
</body>
</html>
