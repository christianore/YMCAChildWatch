<%@ Page Title="" Language="C#" MasterPageFile="~/Login/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ChildWatchEmployee.Login" %>
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

    <div id="childList">
        <table id="children">

        </table>
        <button type="button" class="btn btn-primary btn-block" onclick="signinFamily()">Get Band #</button>
    </div>

    <div id="divBand" class="jumbotron">
        <h1 id="band">Band# </h1>
        <p>Please write your given band number on a braclet for you and your children.  This will be 
            checked by a YMCA staff member before your leave.
        </p>
        <p class="alert">
            You have 60 seconds before the screen is updated.
        </p>
    </div>

    <script>
        var curr_member_id = 0;

        function validate() {
            var json = JSON.stringify(
                {
                    token: {
                        barcode: document.getElementById("barcode").value.toString(),
                        pin: document.getElementById("pin").value.toString()
                    }
                }
            );

            $.ajax({
                url: "http://localhost:50920/Login/Login.aspx/ValidateMember",
                data: json,
                type: "POST",
                dataType: "json",
                contentType: 'application/json',
                success: validateSuccess,
                error: function (x, status, error) { alert(error) }
            });
        }     
        function locationChanged(caller, row) {
            var cells = document.getElementById(row).children;
            for (var i = 2; i < cells.length; i++) {
                cells[i].children[0].checked = false;
            }
           caller.checked = true;
        }
        function selectChanged(caller, row) {
            
            var cells = document.getElementById(row).children;
            for (var i = 2; i < cells.length; i++) {
                if (caller.checked) {
                    $(cells[i].children[0]).show();
                }
                else {
                    $(cells[i].children[0]).hide();
                    cells[i].children[0].checked = false;
                }
                
            }
        }
        function validateSuccess(data) {
            if (data != null && data.d.IsSuccess) {
                var children = data.d.Family.Children;
                var member = data.d.Family.Guardian;
                var locations = data.d.Locations;
                var table = $('#children');
                table.empty();

                curr_member_id = member.MemberId;

                if (children.length > 0) {
                    // Build header row
                    var header = $('<tr/>');
                    header.append($('<th/>').text('Select'));
                    header.append($('<th/>').text('Name'));

                    for (var y = 0; y < locations.length; y++) {
                        header.append($('<th/>').text(locations[y].Name));
                    }

                    table.append(header);
                    var id = 1;
                    for (var x = 0; x < children.length; x++) {
                        var row_id = "row_" + id; 
                        row = $('<tr/>', { id:row_id});
                        id++;
                        row.append($('<td/>').append($('<input/>', { value: children[x].Id, onclick: 'selectChanged(this,"'+row_id+'")', type: "checkbox" })));
                        row.append($('<td/>').text(children[x].FirstName + " " + children[x].LastName));

                        
                        for (var y = 0; y < locations.length; y++) {
                            row.append($('<td/>').append($('<input/>', { onclick:'locationChanged(this,"'+ row_id+'")' ,type: "checkbox", value: locations[y].Id }).hide()));
                        }

                        table.append(row);
                    }
                }
                else {
                    // They have no children so they don't need to be here.
                }
            }
        }
        function signinSuccess(data) {
            var band = data.d;
            if (band > 0) {
                $('#band').text("Band# " + data.d);
                setTimeout(function () {
                    curr_member_id = 0;
                    $('#children').empty();
                    $('#band').text("Band#");
                }, 30000);
            }
        }
        function signinFamily() {
            var rows = document.getElementById('children').children;
            var selection = [];

            for (var i = 1; i < rows.length; i++) {
                var cells = rows[i].cells;
                // child selected
                if (cells[0].children[0].checked) {
                    var child_id = cells[0].children[0].value;
                    var zone = -1;
                    for (var x = 2; x < cells.length; x++) {
                        if (cells[x].children[0].checked) {
                            zone = cells[x].children[0].value;
                            break;
                        }
                    }
                    selection[selection.length] = { Child: child_id, Location: zone };
                }
            }

            var json = JSON.stringify(
                {
                    Assignments: selection,
                    MemberId: curr_member_id  
                }
            );

            $.ajax({
                url: "http://localhost:50920/Default.aspx/SigninMembers",
                data: JSON.stringify({ data: json })  ,
                type: "POST",
                dataType: "json",
                contentType: 'application/json',
                success: signinSuccess,
                error: function (x, status, error) { alert(error) }
            });
        }

    </script>

</asp:Content>
