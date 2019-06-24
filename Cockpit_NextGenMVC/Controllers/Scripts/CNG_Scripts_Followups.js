function validate_FollowupForm() {
    var x = document.forms["FrmIndex"]["Followup_SalesOrder"].value;
    if (x == null || x == "") {
        alert("Sales order must be filled out");
        document.forms["FrmIndex"]["Followup_SalesOrder"].focus();
        return false;
    }
    if (isNaN(x) || x.indexOf(" ") != -1) {
        alert("Sales order must be numeric");
        document.forms["FrmIndex"]["Followup_SalesOrder"].focus();
        return false;
    }

    var x = document.forms["FrmIndex"]["Followup_Customername"].value;
    if (x == null || x == "") {
        alert("Customer name must be filled out");
        document.forms["FrmIndex"]["Followup_Customername"].focus();
        return false;
    }
    var x = document.forms["FrmIndex"]["Followup_Description"].value;
    if (x == null || x == "") {
        alert("Description must be filled out");
        document.forms["FrmIndex"]["Followup_Description"].focus();
        return false;
    }

    var SalesOrder = document.forms["FrmIndex"]["Followup_SalesOrder"].value;
    var CustomerName = document.forms["FrmIndex"]["Followup_Customername"].value;
    var Description = document.forms["FrmIndex"]["Followup_Description"].value;
    var OwnerName = document.forms["FrmIndex"]["Followup_owner"].value;
    var DueDate = document.forms["FrmIndex"]["dt_DueDate"].value;
    var Priority = document.forms["FrmIndex"]["drpPriority"].value;
    var Comments = document.forms["FrmIndex"]["Followup_Comments"].value;

    showProgress();

    $.ajax({
        type: 'POST',
        url: '../Generic/NewFollowup_AJAX/',
        dataType: 'json',
        data: {
            SalesOrder: SalesOrder,
            Owner: OwnerName,
            Status: "Open",
            Customer: CustomerName,
            Description: Description,
            DueDate: DueDate,
            Priority: Priority,
            Comments: Comments
        },
        success: function (Result) {
            hideProgress();

            if (Result) {
                alert("Followup Created for Sales Order :" + SalesOrder);

                var ms = $("#grdOrderOpenFollowups").data('kendoGrid');

                //Check if followup has been created from order buckets, if not skip refreshing open folowups list for order bucket.
                if (ms != null) {
                    ReadOrderOpenFollowHistory(SalesOrder);
                }
                else {
                    Refresh_FollowupList_AfterCreate();
                }

                //Clear Commenting Space to perform Next order Comment
                document.forms["FrmIndex"]["Followup_Description"].value = "";
                document.forms["FrmIndex"]["dt_DueDate"].value = "";
                document.forms["FrmIndex"]["Followup_Comments"].value = "";

                $("#window").data("kendoWindow").close();
            }

        },
        error: function (ex) {
            alert('Failed to retrieve states.' + ex);
        }
    });

}


function checkAllFollowups(ele) {
    var state = $(ele).is(':checked');
    var grid = $('#grdMyFollowups').data().kendoGrid;

    $("#grdMyFollowups input").each(function () {

        if (state == true) {
            $(this).prop('checked', true);
            selectFollowup(this);
        }
        else {
            $(this).prop('checked', false);
            selectFollowup(this);
        }
    });
}


function selectFollowup(ele) {
    var checked = ele.checked,
        row = $(ele).closest("tr"),
        grid = $("#grdMyFollowups").data("kendoGrid"),
        dataItem = grid.dataItem(row);

    checkedIds[dataItem.Followupid] = checked;
    if (checked) {
        //-select the row
        row.addClass("k-state-selected");
    } else {
        //-remove selection
        row.removeClass("k-state-selected");
    }
}


// New Followup 

$(document).ready(function () {

    resizeGrid(); //This will set the grid Height as per user Device height

    var window = $("#window"),
            btnNewFollowup = $("#btnNewFollowup");

    btnNewFollowup.bind("click", function () {
        window.data("kendoWindow").open();
        btnNewFollowup.hide();
    });

    if (!window.data("kendoWindow")) {
        window.kendoWindow({
            width: "700px",
            actions: ["Close"],
            title: "Create New Follow-up",
            close: function () {
                btnNewFollowup.show();
            }
        });
    }
});


function UpdateMultipleSelecedFollowups() {
    checked.length = 0;

    for (var i in checkedIds) {
        if (checkedIds[i]) {
            checked.push(i);
        }
    }

    var TotalFollowupsselected = checked.length;
    var NewOwnerNtLogin = $("#cmb_NewOwner").val();

    var result = confirm("Are you sure to Change Ownership for  " + TotalFollowupsselected + " # of Followups to " + NewOwnerNtLogin + "?");
    if (result == true) {

        $.ajax({
            cache: false,
            type: "POST",
            async: false,
            url: '../Followup/AssignMultipleFollowups',
            data: { selectedFollowupIds: checked.toString(), NewOwnerName: NewOwnerNtLogin },
            datatype: "json",
            success: function (data) {
                window.location.reload();
            },
            statusCode: {
                404: function (content) { alert('cannot find resource'); },
                500: function (content) { alert('internal server error'); }
            },
            error: function (req, status, errorObj) {
                // handle status === "timeout"
                // handle other errors
            }
        });
    }
}

function CloseMultipleSelecedFollowups() {

    checked.length = 0;

    for (var i in checkedIds) {
        if (checkedIds[i]) {
            checked.push(i);
        }
    }

    var TotalFollowupsselected = checked.length;

    var result = confirm("Are you sure to close " + TotalFollowupsselected + " # of Followups?");

    if (result == true) {

        $.ajax({
            cache: false,
            type: "POST",
            async: false,
            url: '../Followup/CloseMultipleFollowups',
            data: { selectedFollowupIds: checked.toString() },
            datatype: "json",
            success: function (data) {
                window.location.reload();
            },
            statusCode: {
                404: function (content) { alert('cannot find resource'); },
                500: function (content) { alert('internal server error'); }
            },
            error: function (req, status, errorObj) {
                // handle status === "timeout"
                // handle other errors
            }
        });


    }

}

function OnFollowupChange(e) {
    var grid = $("#grdMyFollowups").data("kendoGrid");
    //Getting selected Item(s)
    var selectedItems = grid.dataItem(grid.select());
    AddIndiactor_DueDate();

    var sessionemail = $("#hid_UserEmail").val();

    $("#emailsubject").val("Sales Order Follow-up: " + selectedItems.Sales_Order);
    $("#emailCC").val(sessionemail);

    var followupBody = "Follow-up Sales Order : " + selectedItems.Sales_Order + "</br>";
    followupBody = followupBody + "Customer Name             : " + selectedItems.CustomerName + "</br>";
    followupBody = followupBody + "Description               : " + selectedItems.Description + "</br>";
    followupBody = followupBody + "Comment                   : " + selectedItems.Comment + "</br>";
    followupBody = followupBody + "Priority                  : " + selectedItems.Priority + "</br><hr></br>";

    var editor = $("#Body").data("kendoEditor");
    // set value
    editor.value(followupBody);

    $.ajax({
        cache: false,
        type: "POST",
        async: false,
        url: '../Followup/ReadFollowupHistory',
        data: { Followup_ID: selectedItems.Followupid },
        datatype: "json",
        success: function (data) {

            //alert(data);

            var target = $("#FollowupHistory");
            target.empty();
            for (var i = 0; i < data.length; i++) {
                var item = data[i];

                var html = "<tr><td>" + item.Sales_Order + "</td>";
                html += "<td>" + item.CustomerName + "</td>";
                html += "<td>" + item.Description + "</td>";
                html += "<td>" + item.Owner + "</td>";
                html += "<td>" + formatJSONDateFromNumericdate(item.DueDate) + "</td>";
                html += "<td>" + item.Comment + "</td>";
                html += "<td>" + item.Status + "</td>";
                html += "<td>" + item.Priority + "</td>";
                html += "<td>" + item.Modified_By + "</td>";
                html += "<td>" + formatJSONDateFromNumericdate(item.Modified_On) + "</td></tr>";

                target.append(html);
            }
        },
        statusCode: {
            404: function (content) { alert('cannot find resource'); },
            500: function (content) { alert('internal server error'); }
        },
        error: function (req, status, errorObj) {
            // handle status === "timeout"
            // handle other errors
        }
    });

}

function Open_Followup_ToEdit(id, SalesOrder, Customer, Comment, Owner, Priority, status) {

    $("#Followup_SalesOrder").val(Followup.Sales_Order);
    $("#Followup_owner").val(Owner);
    $("#Followup_Customername").val(Customer);

    var window = $("#window");
    window.kendoWindow({
        width: "700px",
        actions: ["Close"],
        title: "Edit selected follow-up",
        close: function () {
            alert("Followup Edit Complete");
        }
    });

    window.data("kendoWindow").open();

}

function Refresh_ClosedFollowupList(e) {
    if (e.type == "update") {

        var Sales_Order = $("#cmt_lblSalesOrder").val();

        $.ajax(
        {
            type: 'POST',
            url: '../Generic/GetFollowUpSummaryBySalesOrderOpen/',
            data: { Sales_Order: Sales_Order },
            dataType: 'json',
            success: function (result) {
                if (result.length > 0) {
                    var grid = $("#grdOrderClosedFollowups").data("kendoGrid");

                    var model = result;

                    for (var i = 0; i < model.length; i++) {
                        model[i].DueDate = kendo.parseDate(model[i].DueDate);
                        model[i].Modified_On = kendo.parseDate(model[i].Modified_On);
                        model[i].Created_On = kendo.parseDate(model[i].Created_On);
                    }

                    grid.dataSource.data(model);

                    $("#lblClosedFollowups").text(model.length.toString());

                    ReadOrderOpenFollowHistory(Sales_Order);
                }
                else {
                    var model = result;

                    var grid = $("#grdOrderOpenFollowups").data("kendoGrid");
                    grid.dataSource.data(model);

                    $("#lblClosedFollowups").text(model.length.toString());

                    ReadOrderOpenFollowHistory(Sales_Order);
                }
            }
        });
    }
}


function Refresh_OpenFollowupList(e) {
    if (e.type == "update") {

        var Sales_Order = $("#cmt_lblSalesOrder").val();

        $.ajax(
        {
            type: 'POST',
            url: '../Generic/GetFollowUpSummaryBySalesOrderOpen/',
            data: { Sales_Order: Sales_Order },
            dataType: 'json',
            success: function (result) {
                if (result.length > 0) {
                    var grid = $("#grdOrderOpenFollowups").data("kendoGrid");

                    var model = result;

                    for (var i = 0; i < model.length; i++) {
                        model[i].DueDate = kendo.parseDate(model[i].DueDate);
                        model[i].Modified_On = kendo.parseDate(model[i].Modified_On);
                        model[i].Created_On = kendo.parseDate(model[i].Created_On);
                    }

                    grid.dataSource.data(model);

                    $("#lblOpenFollowups").text(model.length.toString());

                    ReadOrderCloseFollowHistory(Sales_Order);
                }
                else {
                    var model = result;

                    var grid = $("#grdOrderOpenFollowups").data("kendoGrid");
                    grid.dataSource.data(model);

                    $("#lblOpenFollowups").text(model.length.toString());

                    ReadOrderCloseFollowHistory(Sales_Order);
                }
            }
        });
    }
}

function Refresh_FollowupList(e) {
    if (e.type == "update") {
        $.ajax(
        {
            type: 'POST',
            url: '../Followup/MyFollowupsListJson/',
            dataType: 'json',
            //data: { ReportName: ''},
            success: function (result) {
                if (result.length > 0) {
                    var grid = $("#grdMyFollowups").data("kendoGrid");

                    var model = result;

                    for (var i = 0; i < model.length; i++) {
                        model[i].DueDate = kendo.parseDate(model[i].DueDate);
                        model[i].Modified_On = kendo.parseDate(model[i].Modified_On);
                        model[i].Created_On = kendo.parseDate(model[i].Created_On);
                    }

                    grid.dataSource.data(model);
                }
            }
        });
    }
}

function Refresh_FollowupList_AfterCreate() {

    $.ajax(
        {
            type: 'POST',
            url: '../Followup/MyFollowupsListJson/',
            dataType: 'json',
            //data: { ReportName: ''},
            success: function (result) {
                if (result.length > 0) {
                    var grid = $("#grdMyFollowups").data("kendoGrid");

                    var model = result;

                    for (var i = 0; i < model.length; i++) {
                        model[i].DueDate = kendo.parseDate(model[i].DueDate);
                        model[i].Modified_On = kendo.parseDate(model[i].Modified_On);
                        model[i].Created_On = kendo.parseDate(model[i].Created_On);
                    }

                    grid.dataSource.data(model);
                }
            }
        });
}
