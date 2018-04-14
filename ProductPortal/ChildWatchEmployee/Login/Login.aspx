<%@ Page Title="" Language="C#" MasterPageFile="~/Login/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ChildWatchEmployee.Login" %>

<asp:Content ContentPlaceHolderID="HEAD" ID="Content2" runat="server">
    <script src="../Scripts/login.js"></script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div id="login">
    <fieldset>
        <legend>Sign In</legend>
        <div class="row form-group">
            <label class="col-md-4 control-label">Barcode</label>
            <div class="col-md-8">
                <input id="barcode" class="form-control" type="text" />
            </div>
        </div>
        <div class="row form-group">
            <label class="col-md-4 control-label">Pin</label><br />
            <div class="col-md-8">
                <input id="pin" class="form-control" type="text" /><br />
            </div>
        </div>
        <div class="form-group">
            <button type="button" class="btn btn-primary btn-block form-control" onclick="validate()">Sign In</button>
        </div>
    </fieldset>
    </div>

    <div id="childList" style="display:none;">
        <table id="children">

        </table>
        <button type="button" class="btn btn-primary btn-block" onclick="signinFamily()">Get Band #</button>
    </div>

    <div id="divBand" style="display:none;" class="jumbotron">
        <h1 id="band">Band# </h1>
        <p>Please write your given band number on a braclet for you and your children.  This will be 
            checked by a YMCA staff member before your leave.
        </p>
        <p id="warning" class="alert">
            Time Remaining: 30 seconds
        </p>
        <p>
            We will remove all personal data and your band number when the timer expires.  If you need additional assisstance after the allotted time,
            please talk to one of our member representatives.
        </p>
    </div>
</asp:Content>
