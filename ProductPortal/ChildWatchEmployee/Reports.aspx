<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="ChildWatchEmployee.Reports" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>YMCA ChildWatch Reporting Portal</title>
    <script src="Scripts/jquery-3.3.1.min.js"></script>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css"/>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/bistyle.css" rel="stylesheet" />   
</head>
<body id="bootstrap-overrides">
    <form id="form1" runat="server">
        <div class="topdiv">
        <div id="reportAside" class="aside" runat="server">
            <h2><strong>Reports</strong></h2>
             <div id="repList" class="list-group">
                  <a id="rep1" href="#" class="list-group-item" onclick="return showHide('rep1');">Interval Headcount</a>
                  <a id="rep2" href="#" class="list-group-item" onclick="return showHide('rep2');">Member Information</a>
                  <a id="rep3" href="#" class="list-group-item" onclick="return showHide('rep3');">Daily Totals</a>
                 <asp:HiddenField runat="server" ID="reportSelected"/>
            </div> 
        </div>
        <div id="repArgsMain" class="repArgsMain">
    <div id="reportArguments" class ="accordion">           
            <div id="interval" class="interval" runat="server">
                  <div class="accordion-section">
                        <a class="accordion-section-title" href="#accordion-1"><strong>Selected: Interval Report</strong></a>
                        <div id="accordion-1" class="accordion-section-content">
                            <p>
                                    Displays headcount for each location broken down by time interval.
                            </p>   
                            <ul id="intervalList" class="list">
                                <li>Time Interval: Determines the reports grouping for the headcount <em>-Default 30min</em></li>                
                                <li>Start Time: When to begin counting <em>-Default 5 am</em></li>
                                <li>End Time: When to stop counting <em>-Default 10 pm</em></li>
                                <li>Date: Select the date for which the report should return information <em>-Default Today</em></li>
                                <li>Location: Area to view information for</li>
                            </ul>                                
                         </div><!--end .accordion-section-content-->
                    </div><!--end .accordion-section-->
                    <div id="centerDiv" class="centerDiv">
                        <div class="input-group">
                            <span class="input-group-addon">Time Interval: </span>
                            <asp:DropDownList ID="ddlInterval" cssclass="form-control" runat="server" >
                                <asp:ListItem Selected="True" Value="30">30 min</asp:ListItem>
                                <asp:ListItem value="60">1 hour </asp:ListItem>
                            </asp:DropDownList>
                        </div>                                          
                        <div class="input-group">
                            <span class="input-group-addon">Start Time: </span>
                            <select id="selectStart" runat="server" class="form-control" onchange="updateTimes()"></select>
                        </div>                            
                        <div class="input-group">
                            <span class="input-group-addon">End Time: </span>
                            <select id="selectStop" runat="server" class="form-control"></select>
                        </div>                            
                        <div class="input-group">
                            <span class="input-group-addon">Date: </span>
                            <asp:TextBox ID="txtDate" runat="server" cssclass="form-control"></asp:TextBox>
                        </div>                                
                        <div class="input-group">
                            <span class="input-group-addon">Location: </span>
                            <asp:DropDownList ID="ddlLocInterval" runat="server" cssclass="form-control"></asp:DropDownList>
                        </div>                   
                    </div> 
                 </div>        
             <div id="member" class="member" style="display:none;" runat="server">
                <div class="accordion-section">
                    <a class="accordion-section-title" href="#accordion-2"><strong>Selected: Member Information Report</strong></a>
                        <div id="accordion-2" class="accordion-section-content">
                            <p>
                                 Displays information about YMCA members registered in the application system.
                            </p>
                            <ul>
                                <li>Member Status: Select to view information for active YMCA membership status, inactive, or both. <em>-Default Active</em></li>
                            </ul>                            
                        </div><!--end .accordion-section-content-->
                    </div><!--end .accordion-section-->
                <div id="centerDiv2" class="centerDiv">
                    <div class="input-group">
                        <span class="input-group-addon">Member Status: </span>
                        <asp:DropDownList ID="ddlMemStatus" cssclass="form-control" runat="server" >
                            <asp:ListItem Value="true" Selected="True">Active</asp:ListItem>
                            <asp:ListItem Value="false">Inactive</asp:ListItem>
                            <asp:ListItem Value="">Both</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>               
             </div>        
        <div id="dayTotals" class="dayTotals" style="display:none;" runat="server">
                    <div class="accordion-section">
                        <a class="accordion-section-title" href="#accordion-3"><strong>Selected: Daily Report</strong></a>
                            <div id="accordion-3" class="accordion-section-content">
                                <p>
                                    Displays totals for each day over a specified time frame for a location.
                                </p>
                                <ul>
                                    <li>Start Date: Enter the day to begin search (mm/dd/yyyy) <em>-Default 1 week ago</em></li>
                                    <li>End Date: Day to stop search (mm/dd/yyyy) <em>-Default Today</em></li>
                                    <li>Location:  Area to view information for</li>
                                </ul>
                            </div><!--end .accordion-section-content-->
                        </div><!--end .accordion-section-->             
            <div id="centerDiv3" class="centerDiv">
                <div class="input-group">
                    <span class="input-group-addon">Start Date: </span>
                     <asp:TextBox ID="txtDateFrom" runat="server"  cssclass="form-control"></asp:TextBox>
                </div>
                <div class="input-group">
                    <span class="input-group-addon">End Date: </span>
                    <asp:TextBox ID="txtDateTo" runat="server"  cssclass="form-control"></asp:TextBox>
                </div>
                <div class="input-group">
                    <span class="input-group-addon">Location: </span>
                    <asp:DropDownList ID="ddlLocation" runat="server" cssclass="form-control"></asp:DropDownList>
                </div>         
            </div>
         </div>
        <div id="centerDiv4" class="centerBtnDiv">
            <asp:Button ID="btnRunReport" runat="server" Text="Run Report" cssclass="btn" OnClick="btnRunReport_Click" BackColor="#F47920" BorderColor="#009784" Font-Bold="True" ForeColor="White" />
        </div>
    </div>
    </div>
    </div>
    <hr />
    <div id="reportDisplay" class="reportDisplay">
        
        <asp:DataGrid ID="ReportGrid" runat="server" CssClass="table-striped" BorderStyle="None" GridLines="None" AllowPaging="True" OnPageIndexChanged="ReportGrid_PageIndexChanged" PageSize="40">
            <AlternatingItemStyle BackColor="#E4E4E4" />
            <HeaderStyle BackColor="#EF9E0A" BorderColor="#F47A21" BorderStyle="Solid" BorderWidth="1px" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" VerticalAlign="Middle" />
            <PagerStyle HorizontalAlign="Left" BackColor="White" BorderStyle="None" Font-Bold="True" PageButtonCount="100" Position="TopAndBottom" />
        </asp:DataGrid>
    </div>
    </form>
    <script  type="text/javascript"  lang="javascript">  
        //Apply Datepicker functionality to inputs
        $(function () {
             $('#<%=txtDateFrom.ClientID %>').datepicker(
                 {
                     dateFormat: 'mm/dd/yy',
                     changeMonth: true,
                     changeYear: true,
                     yearRange: '2018:2100',
                     constrainInput: true
                });
            $('#<%=txtDateTo.ClientID %>').datepicker(
                {
                    dateFormat: 'mm/dd/yy',
                    changeMonth: true,
                    changeYear: true,
                    yearRange: '2018:2100',
                    constrainInput: true
                });
            $('#<%=txtDate.ClientID %>').datepicker(
                {
                    dateFormat: 'mm/dd/yy',
                    changeMonth: true,
                    changeYear: true,
                    yearRange: '2018:2100',
                    constrainInput: true
                });
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
        //Handle accordion sections for report descriptions
        function close_accordion_section() {
            $('.accordion .accordion-section-title').removeClass('active');
            $('.accordion .accordion-section-content').slideUp(300).removeClass('open');
        }

        //Update what report description and inputs are displayed based on new report list selection
         var h = document.getElementById('reportSelected');
         function showHide(e) {
             if ('rep1' === e) {                 
                 $("#interval").show();
                 $("#member").hide();
                 $("#dayTotals").hide();
                 document.getElementById("reportSelected").value = "1";
             } else if ('rep2' === e) {
                 $("#member").show();
                 $("#interval").hide();
                 $("#dayTotals").hide();
                 document.getElementById("reportSelected").value = "2";
             } else if ('rep3' === e) {
                 $("#dayTotals").show();
                 $("#member").hide();
                 $("#interval").hide();
                 document.getElementById("reportSelected").value = "3";
             }
         }
        //limit options in selectStop list based on the selected start time for the interval report
         function updateTimes() {
             // Empty stop list
             $('#selectStop').empty();
             // Get new selected time.
            var start = parseInt(getSelectedTime());
            
            for (var x = start + 1; x <= 22; x++) {
                var text = "";

                if (x < 12) {
                    text = x.toString() + " AM";
                }
                else if (x == 12) {
                    text = "12 PM";
                }
                else {
                    text = (x - 12).toString() + " PM";
                }

                $("#selectStop").append($('<option />', { text: text, value: x}));
            }
             /*
             //reload based on start selected value
             var time = sel + 5;
             for (x = time + 1; x <= 22; x++) {
                 var text = "";
                 var val = x;
                 if (x > 12) {
                     val = x - 12;
                     text = val + " PM";
                 } else {
                     if (x == 12) {
                         text = val + " AM";
                     } else {
                         text = val + " AM";
                     }
                 }
                 selectStop.options[selectStop.options.length] = new Option(text, x, false, false);
             }
             if (origStop > time) {
                 $("#selectStop").val(origStop).change();
             }
             */
         }

         function getSelectedTime() {
             var ddl = document.getElementById("selectStart");
             var list = ddl.children;
             for (var i = 0; i < list.length; i++) {
                 if (list[i].selected) {
                     return list[i].value;
                 }
             }
             return null;
         }
</script>
</body>
</html>

