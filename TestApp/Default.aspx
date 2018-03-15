<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TestApp._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <input id="barcode" value=""/>
    <input id="pin" value="" />
    <button type="button" id="btn" onclick="postToWeb()">Post</button>
    <button type="button" id="timertest" onclick="timer()">Timer</button>
    <button type="button" id="registerchild" onclick="RegisterChild()">RegisterChild</button>
    <div id="testDiv">

    </div>
    <script>

        function timer() {
            setTimeout(function () {
                $('#testDiv').append($('<p/>').text("Timer hit"));
            }, 3000);
        }
        function postToWeb() {

            var obj = {
                barcode: document.getElementById("barcode").value.toString(),
                pin: document.getElementById("pin").value.toString()
            };
            console.warn("Sending " + obj.barcode + "  " + obj.pin);
            $.ajax({
                url: '<%= ResolveUrl("Default.aspx/ValidateMember") %>',
                data: JSON.stringify(obj),
                type: "POST",
                dataType: "json",
                contentType: 'application/json; charset=utf-8',
                success: function (response) { alert(response.d.Error.toString()) },
                error: function (x, status, error) { alert(status) }
            });
        }
        function RegisterChild() {
            var obj = {
                c: {
                    FirstName: "Coleman",
                    LastName: "Wilson",
                    BirthDate: "5/5/1999",
                    Id: -1
                },
                member_id : "10394749"
            }
            var url = '<%= ResolveUrl("Default.aspx/RegisterChild") %>';
            postToServer(obj, url, function (r) { alert("Success") });
        }
        function postToServer(obj, url, fn_callback) {
            $.ajax({
                url: url,
                data: JSON.stringify(obj),
                type: "POST",
                dataType: "json",
                contentType: 'application/json; charset=utf-8',
                success: function (response) { fn_callback(response) },
                error: function (x, status, error) { alert(status) }
            });
        }
    </script>
</asp:Content>
