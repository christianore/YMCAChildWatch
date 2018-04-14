// Member number of the connected person
var curr_member_id = 0;
// Connection to hub to update the signin roster.
var proxy = $.connection.childWatchHub;
$.connection.hub.start();

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
        url: url.validate,//"/SignIn/ValidateMember",
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
// Invoked when information is returned from the server for a validation
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
                row = $('<tr/>', { id: row_id });
                id++;
                row.append($('<td/>').append($('<input/>',
                    {
                        value: children[x].Id,
                        onclick: 'selectChanged(this,"' + row_id + '")',
                        type: "checkbox"
                    })));
                row.append($('<td/>').text(children[x].FirstName + " " + children[x].LastName));


                for (var y = 0; y < locations.length; y++) {
                    row.append($('<td/>').append($('<input/>', {
                        onclick: 'locationChanged(this,"' + row_id + '")',
                        type: "checkbox", value: locations[y].Id
                    }).hide()));
                }

                table.append(row);
            }
            $('#barcode').val("");
            $('#pin').val("");
            $('#login').hide();
            $('#childList').show();
        }
        else {
            // They have no children so they don't need to be here.
        }
    }
}
function signinSuccess(data) {
    var band = data.d;
    if (band > 0) {
        $('#childList').hide();
        $('#divBand').show();        
        $('#band').text("Band# " + data.d);
        proxy.server.sendRefreshNotification();
        
        var time = 30;
        var timer = setInterval(function () {
            time = time - 1;
            $('#warning').text("Time Remaining: " + time.toString() + " seconds.");
            
            if (time == 0) {
                window.clearInterval(timer);
                curr_member_id = 0;
                $('#children').empty();
                $('#band').text("Band#");
                $('#divBand').hide();
                $('#login').show();
                $('#warning').text("Time Remaining: 30 seconds");
            }
        }, 1000);
        
    }
    else {
        // Failed to get a band
    }
}
function startTimer(time) {
 
    if (time == 0) {

    }
    else {
      
        
        setTimeout(startTimer(time - 1), 10000);
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
        url: url.signin,//"/SignIn/SigninMembers",
        data: JSON.stringify({ data: json }),
        type: "POST",
        dataType: "json",
        contentType: 'application/json',
        success: signinSuccess,
        error: function (x, status, error) { alert(error) }
    });
}
