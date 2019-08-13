//---------------- CNG Project Scripts -----------------------------//

var LocalFilters = [];

// Data Bound Grid Events -------------------------

function Clear_HiddenFilters() {
    $("#hid_SalesOrder").val("");
    $("#hid_CustPONum").val("");
    $("#hid_Sorg").val("");
    $("#hid_OrderOwner").val("");
    $("#hid_Aging").val("");
    $("#hid_AgingBucket").val("");
    $("#hid_SNIAging").val("");
    $("#hid_SNIAgingBucket").val("");
    $("#hid_DBClosureStatus").val("");
    $("#hid_SNIClosureStatus").val("");
    $("#hid_SoldToAccount").val("");
    $("#hid_ZUAccount").val("");
    $("#hid_SalesRep").val("");
    $("#hid_SalesForce").val(""),
    $("#hid_InstallationStatus").val(""),
    $("#hid_PaymentTerm").val("");
    $("#hid_billingblock").val("");
    $("#hid_DeliveryBlock").val("");
    $("#hid_NLHD").val("");
    $("#hid_LoadDate").val("");
    $("#hid_TrioLoadDate").val("");
    $("#hid_DeltaLoaddateBucket").val("");
    $("#hid_CRDD").val("");
    $("#hid_ExpReleaseDate").val("");
    $("#hid_ReasonCode").val("");
}

function Set_Hidden_FilterValue(FilterName, FilterValue, Condition) {
    switch (FilterName) {
        case "SALES_ORDERNO":
            var Current_Val = $("#hid_SalesOrder").val();
            Current_Val += "," + FilterValue;
            $("#hid_SalesOrder").val(Current_Val);
            break;
        case "CUSTOMER_PO_NO":
            var Current_Val = $("#hid_CustPONum").val();
            Current_Val += "," + FilterValue;
            $("#hid_CustPONum").val(Current_Val);
            break;
        case "ORDER_OWNER":
            var Current_Val = $("#hid_OrderOwner").val();
            Current_Val += "," + FilterValue;
            $("#hid_OrderOwner").val(Current_Val);
            break;
        case "FE_FE_DESC":
            var Current_Val = $("#hid_SalesRep").val();
            Current_Val += "," + FilterValue;
            $("#hid_SalesRep").val(Current_Val);
            break;
        case "QUOTA_SF":
            var Current_Val = $("#hid_SalesForce").val();
            Current_Val += "," + FilterValue;
            $("#hid_SalesForce").val(Current_Val);
            break;
        case "NLHD_STATUS":
            var Current_Val = $("#hid_NLHD").val();
            Current_Val += "," + FilterValue;
            $("#hid_NLHD").val(Current_Val);
            break;
        case "COMMIT_DATE":
            var Current_Val = $("#hid_LoadDate").val();

            if (Condition == "eq") {
                Current_Val += "='" + formatJSONDate(FilterValue) + "'";
            }
            else if (Condition == "neq") {
                Current_Val += " <> '" + formatJSONDate(FilterValue) + "'";
            }
            else if (Condition == "gt") {
                Current_Val += " > '" + formatJSONDate(FilterValue) + "'";
            }
            else if (Condition == "gte") {
                Current_Val += " >= '" + formatJSONDate(FilterValue) + "'";
            }
            else if (Condition == "lt") {
                Current_Val += " AND COMMIT_DATE < '" + formatJSONDate(FilterValue) + "'";
            }
            else if (Condition == "lte") {
                Current_Val += " AND COMMIT_DATE <= '" + formatJSONDate(FilterValue) + "'";
            }

            $("#hid_LoadDate").val(Current_Val);
            break;
        case "TRIO_LOAD_DATE":
            var Current_Val = $("#hid_TrioLoadDate").val();

            if (Condition == "eq") {
                Current_Val += "='" + formatJSONDate(FilterValue) + "'";
            }
            else if (Condition == "neq") {
                Current_Val += " <> '" + formatJSONDate(FilterValue) + "'";
            }
            else if (Condition == "gt") {
                Current_Val += " > '" + formatJSONDate(FilterValue) + "'";
            }
            else if (Condition == "gte") {
                Current_Val += " >= '" + formatJSONDate(FilterValue) + "'";
            }
            else if (Condition == "lt") {
                Current_Val += " AND TRIO_LOAD_DATE < '" + formatJSONDate(FilterValue) + "'";
            }
            else if (Condition == "lte") {
                Current_Val += " AND TRIO_LOAD_DATE <= '" + formatJSONDate(FilterValue) + "'";
            }

            $("#hid_TrioLoadDate").val(Current_Val);
            break;
        case "CUSTOMER_REQ_GI_DATE":
            var Current_Val = $("#hid_CRDD").val();

            if (Condition == "eq") {
                Current_Val += "='" + formatJSONDate(FilterValue) + "'";
            }
            else if (Condition == "neq") {
                Current_Val += " <> '" + formatJSONDate(FilterValue) + "'";
            }
            else if (Condition == "gt") {
                Current_Val += " > '" + formatJSONDate(FilterValue) + "'";
            }
            else if (Condition == "gte") {
                Current_Val += " >= '" + formatJSONDate(FilterValue) + "'";
            }
            else if (Condition == "lt") {
                Current_Val += " AND CUSTOMER_REQ_GI_DATE < '" + formatJSONDate(FilterValue) + "'";
            }
            else if (Condition == "lte") {
                Current_Val += " AND CUSTOMER_REQ_GI_DATE <= '" + formatJSONDate(FilterValue) + "'";
            }

            $("#hid_CRDD").val(Current_Val);
            break;
        case "EXPECTED_RELEASE_DATE":
            var Current_Val = $("#hid_ExpReleaseDate").val();

            if (Condition == "eq") {
                Current_Val += "='" + formatJSONDate(FilterValue) + "'";
            }
            else if (Condition == "neq") {
                Current_Val += " <> '" + formatJSONDate(FilterValue) + "'";
            }
            else if (Condition == "gt") {
                Current_Val += " > '" + formatJSONDate(FilterValue) + "'";
            }
            else if (Condition == "gte") {
                Current_Val += " >= '" + formatJSONDate(FilterValue) + "'";
            }
            else if (Condition == "lt") {
                Current_Val += " AND EXPECTED_RELEASE_DATE < '" + formatJSONDate(FilterValue) + "'";
            }
            else if (Condition == "lte") {
                Current_Val += " AND EXPECTED_RELEASE_DATE <= '" + formatJSONDate(FilterValue) + "'";
            }

            $("#hid_ExpReleaseDate").val(Current_Val);
            break;
        case "REASON_CODE":
            var Current_Val = $("#hid_ReasonCode").val();
            Current_Val += "," + FilterValue;
            $("#hid_ReasonCode").val(Current_Val);
            break;
        case "SALES_ORG":
            var Current_Val = $("#hid_Sorg").val();
            Current_Val += "," + FilterValue;
            $("#hid_Sorg").val(Current_Val);
            break;
        case "DB_AGING_BUCKET":
            var Current_Val = $("#hid_AgingBucket").val();
            Current_Val += "," + FilterValue;
            $("#hid_AgingBucket").val(Current_Val);
            break;
        case "DB_AGING_BUCKET":
            var Current_Val = $("#hid_DBAging").val();
            Current_Val += "," + FilterValue;
            $("#hid_DBAging").val(Current_Val);
            break;
        case "ORDER_AGE":
            var Current_Val = $("#hid_Aging").val();
            Current_Val += "," + FilterValue;
            $("#hid_Aging").val(Current_Val);
            break;
        case "OVERALL_INSTALLATION_STATUS":
            var Current_Val = $("#hid_InstallationStatus").val();
            Current_Val += "," + FilterValue;
            $("#hid_InstallationStatus").val(Current_Val);
            break;
        case "SNI_AGE":
            var Current_Val = $("#hid_SNIAging").val();
            Current_Val += "," + FilterValue;
            $("#hid_SNIAging").val(Current_Val);
            break;
        case "SNI_AGING_BUCKET":
            var Current_Val = $("#hid_SNIAgingBucket").val();
            Current_Val += "," + FilterValue;
            $("#hid_SNIAgingBucket").val(Current_Val);
            break;
        case "SNI_CLOSURE_STATUS":
            var Current_Val = $("#hid_SNIClosureStatus").val();
            Current_Val += "," + FilterValue;
            $("#hid_SNIClosureStatus").val(Current_Val);
            break;
        case "SOLD_TO_PARTY_NAME":
            var Current_Val = $("#hid_SoldToAccount").val();
            Current_Val += "," + FilterValue;
            $("#hid_SoldToAccount").val(Current_Val);
            break;
        case "ZU_ACCOUNT_NAME":
            var Current_Val = $("#hid_ZUAccount").val();
            Current_Val += "," + FilterValue;
            $("#hid_ZUAccount").val(Current_Val);
            break;
        case "PAYMENT_TERMS":
            var Current_Val = $("#hid_PaymentTerm").val();
            Current_Val += "," + FilterValue;
            $("#hid_PaymentTerm").val(Current_Val);
            break;
        case "BILLING_BLOCK_CD":
            var Current_Val = $("#hid_billingblock").val();
            Current_Val += "," + FilterValue;
            $("#hid_billingblock").val(Current_Val);
            break;
        case "DELIVERY_BLK_HDR_CD":
            var Current_Val = $("#hid_DeliveryBlock").val();
            Current_Val += "," + FilterValue;
            $("#hid_DeliveryBlock").val(Current_Val);
            break;
        case "DELTA_LOAD_DATE_BUCKET":
            var Current_Val = $("#hid_DeltaLoaddateBucket").val();
            Current_Val += "," + FilterValue;
            $("#hid_DeltaLoaddateBucket").val(Current_Val);
            break;
    }
}


function DisplaySessionFilters() {

    var FiltersApplied = "";

//    if ($("#lblFilterCriteria")[0].innerHTML != "") {
//        FiltersApplied += $("#lblFilterCriteria")[0].innerHTML;
//    }

    for (var i = 0; i < LocalFilters.length; i++) {
        var filterChosen = LocalFilters[i];

        if (filterChosen.logic != undefined) { // Multiple filters chosen
            FiltersApplied += filterChosen.filters[0].field + " = ";

            for (var j = 0; j < filterChosen.filters.length; j++) {
                FiltersApplied += filterChosen.filters[j].value + ", ";
            }
        }
        else { //Single Filter chosen
            if (FiltersApplied.indexOf(filterChosen.field) == -1) {
                FiltersApplied += filterChosen.field + " = " + filterChosen.value;
            }
            else {
                FiltersApplied += ", " + filterChosen.value;
            }
        }

    }

    if (FiltersApplied != "") {
        $("#lblFilterCriteria").text(FiltersApplied);
        $("#lblFilterCriteria2").text(FiltersApplied);
    }
}


function dataBound_STR(e) {
    AddIndiactor_Aging_STR();
}

function dataBound_DB(e) {
    //Get Currently Applied filters from Grid.
    var filter = this.dataSource.filter();

    //get current set of filters, which is supposed to be array.
    //if the object obtained is null, set this to empty array.
    var currentFilters = filter ? filter.filters : [];

    //Fill LocalFilters Global Array
    if (LocalFilters != undefined) {
        LocalFilters.length = 0;
        Clear_HiddenFilters();
    }

    for (var i = 0; i < currentFilters.length; i++) {
        LocalFilters.push(currentFilters[i]);
        Set_Hidden_FilterValue(currentFilters[i].field, currentFilters[i].value, currentFilters[i].operator);
    }

    if ($("#hid_CurrentChart").val() == "") {
        _LoadWaterfallChart();

        var newWidth = window.innerWidth * .98 // 80% of screen width
        var chart = $("#WaterfallChart").data("kendoChart"); // get chart widget
        chart.options.chartArea.width = newWidth; // set new width
        chart.redraw(); // redraw the chart
    }
    else if ($("#hid_CurrentChart").val() == "Piechart") {
        _LoadPieChart();

        var newWidth = window.innerWidth * .8 // 80% of screen width
        var chart = $("#PieChart").data("kendoChart"); // get chart widget
        chart.options.chartArea.width = newWidth; // set new width
        chart.redraw(); // redraw the chart
    }
    else {
        _LoadWaterfallChart();

        var newWidth = window.innerWidth * .8 // 80% of screen width
        var chart = $("#WaterfallChart").data("kendoChart"); // get chart widget
        chart.options.chartArea.width = newWidth; // set new width
        chart.redraw(); // redraw the chart
    }

    DisplaySessionFilters();

    var AppliedFiltersstate = {
        SalesOrder: $("#hid_SalesOrder").val(),
        CustomerPONumber: $("#hid_CustPONum").val(),
        Sorg: $("#hid_Sorg").val(),
        OwnerName: $("#hid_OrderOwner").val(),
        Aging: $("#hid_Aging").val(),
        AgingBucket: $("#hid_AgingBucket").val(),
        SNIAging: $("#hid_SNIAging").val(),
        SNIAgingBucket: $("#hid_SNIAgingBucket").val(),
        DBClosureStatus: $("#hid_DBClosureStatus").val(),
        SNIClosureStatus: $("#hid_SNIClosureStatus").val(),
        SoldToAccount: $("#hid_SoldToAccount").val(),
        ZUAccount: $("#hid_ZUAccount").val(),
        SalesRep: $("#hid_SalesRep").val(),
        SalesForce: $("#hid_SalesForce").val(),
        InstallationStatus: $("#hid_InstallationStatus").val(),
        PaymentTerm: $("#hid_PaymentTerm").val(),
        BillingBlock: $("#hid_billingblock").val(),
        DeliveryBlock: $("#hid_DeliveryBlock").val(),
        NLHD: $("#hid_NLHD").val(),
        LoaDDate: $("#hid_LoadDate").val(),
        TrioLoaDDate: $("#hid_TrioLoadDate").val(),
        DeltaLoaddateBucket: $("#hid_DeltaLoaddateBucket").val(),
        CRDD: $("#hid_CRDD").val(),
        ExpReleaseDate: $("#hid_ExpReleaseDate").val(),
        ReasonCode: $("#hid_ReasonCode").val()
    };

    $.ajax({
        url: "../Generic/SaveLocalFilters/",
        type: 'POST',
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(AppliedFiltersstate),
        dataType: "json",
        success: function (data) {
            //alert('yay');
        }

    });

    AddIndiactor_Aging();
}

function dataBound_SNI(e) {
    //Get Currently Applied filters from Grid.
    var filter = this.dataSource.filter();

    //get current set of filters, which is supposed to be array.
    //if the object obtained is null, set this to empty array.
    var currentFilters = filter ? filter.filters : [];

    //Fill LocalFilters Global Array
    if (LocalFilters != undefined) {
        LocalFilters.length = 0;
        Clear_HiddenFilters();
    }

    for (var i = 0; i < currentFilters.length; i++) {
        LocalFilters.push(currentFilters[i]);
        Set_Hidden_FilterValue(currentFilters[i].field, currentFilters[i].value, currentFilters[i].operator);
    }

    if ($("#hid_CurrentChart").val() == "") {
        _LoadWaterfallChart();

        var newWidth = window.innerWidth * .8 // 80% of screen width
        var chart = $("#WaterfallChart").data("kendoChart"); // get chart widget
        chart.options.chartArea.width = newWidth; // set new width
        chart.redraw(); // redraw the chart
    }
    else if ($("#hid_CurrentChart").val() == "Piechart") {
        _LoadPieChart();

        var newWidth = window.innerWidth * .8 // 80% of screen width
        var chart = $("#PieChart").data("kendoChart"); // get chart widget
        chart.options.chartArea.width = newWidth; // set new width
        chart.redraw(); // redraw the chart
    }
    else {
        _LoadWaterfallChart();

        var newWidth = window.innerWidth * .8 // 80% of screen width
        var chart = $("#WaterfallChart").data("kendoChart"); // get chart widget
        chart.options.chartArea.width = newWidth; // set new width
        chart.redraw(); // redraw the chart
    }

    DisplaySessionFilters();

    var AppliedFiltersstate = {
        SalesOrder: $("#hid_SalesOrder").val(),
        CustomerPONumber: $("#hid_CustPONum").val(),
        Sorg: $("#hid_Sorg").val(),
        OrderOwner: $("#hid_OrderOwner").val(),
        Aging: $("#hid_Aging").val(),
        AgingBucket: $("#hid_AgingBucket").val(),
        SNIAgingBucket: $("#hid_SNIAgingBucket").val(),
        DBClosureStatus: $("#hid_DBClosureStatus").val(),
        SNIClosureStatus: $("#hid_SNIClosureStatus").val(),
        SoldToAccount: $("#hid_SoldToAccount").val(),
        ZUAccount: $("#hid_ZUAccount").val(),
        SalesRep: $("#hid_SalesRep").val(),
        SalesForce: $("#hid_SalesForce").val(),
        InstallationStatus: $("#hid_InstallationStatus").val(),
        PaymentTerm: $("#hid_PaymentTerm").val(),
        BillingBlock: $("#hid_billingblock").val(),
        DeliveryBlock: $("#hid_DeliveryBlock").val(),
        NLHD: $("#hid_NLHD").val(),
        LoaDDate: $("#hid_LoadDate").val(),
        TrioLoaDDate: $("#hid_TrioLoadDate").val(),
        DeltaLoaddateBucket: $("#hid_DeltaLoaddateBucket").val(),
        CRDD: $("#hid_CRDD").val(),
        ExpReleaseDate: $("#hid_ExpReleaseDate").val(),
        ReasonCode: $("#hid_ReasonCode").val()
    };

    $.ajax({
        url: "../Generic/SaveLocalFilters/",
        type: 'POST',
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(AppliedFiltersstate),
        dataType: "json",
        success: function (data) {
            //alert('Local Filters Set');
        }

    });

    AddIndiactor_SNI_ReleaseDate();
}

function dataBound_Unblocked(e) {
    //Get Currently Applied filters from Grid.
    var filter = this.dataSource.filter();

    //get current set of filters, which is supposed to be array.
    //if the object obtained is null, set this to empty array.
    var currentFilters = filter ? filter.filters : [];

    //Fill LocalFilters Global Array
    if (LocalFilters != undefined) {
        LocalFilters.length = 0;
        Clear_HiddenFilters();
    }

    for (var i = 0; i < currentFilters.length; i++) {
        LocalFilters.push(currentFilters[i]);
        Set_Hidden_FilterValue(currentFilters[i].field, currentFilters[i].value, currentFilters[i].operator);
    }

    var AppliedFiltersstate = {
        SalesOrder: $("#hid_SalesOrder").val(),
        CustomerPONumber: $("#hid_CustPONum").val(),
        Sorg: $("#hid_Sorg").val(),
        OrderOwner: $("#hid_OrderOwner").val(),
        Aging: $("#hid_Aging").val(),
        AgingBucket: $("#hid_AgingBucket").val(),
        SNIAgingBucket: $("#hid_SNIAgingBucket").val(),
        DBClosureStatus: $("#hid_DBClosureStatus").val(),
        SNIClosureStatus: $("#hid_SNIClosureStatus").val(),
        SoldToAccount: $("#hid_SoldToAccount").val(),
        ZUAccount: $("#hid_ZUAccount").val(),
        SalesRep: $("#hid_SalesRep").val(),
        SalesForce: $("#hid_SalesForce").val(),
        InstallationStatus: $("#hid_InstallationStatus").val(),
        PaymentTerm: $("#hid_PaymentTerm").val(),
        BillingBlock: $("#hid_billingblock").val(),
        DeliveryBlock: $("#hid_DeliveryBlock").val(),
        NLHD: $("#hid_NLHD").val(),
        LoaDDate: $("#hid_LoadDate").val(),
        TrioLoaDDate: $("#hid_TrioLoadDate").val(),
        DeltaLoaddateBucket: $("#hid_DeltaLoaddateBucket").val(),
        CRDD: $("#hid_CRDD").val(),
        ExpReleaseDate: $("#hid_ExpReleaseDate").val(),
        ReasonCode: $("#hid_ReasonCode").val()
    };

    $.ajax({
        url: "../Generic/SaveLocalFilters/",
        type: 'POST',
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(AppliedFiltersstate),
        dataType: "json",
        success: function (data) {
            //alert('Local Filters Set');
        }

    });

}

// ---------------------------------------------------------

// Color Coding Scheme

function AddIndiactor_ClosureStatus() {
    var bulletred = "../Content/img/bullet-red.png";
    var bulletYellow = "../Content/img/bullet-yellow.png";
    var grid = $("#grdOrders").data("kendoGrid");
    var data = grid.dataSource.data();
    $.each(data, function (i, row) {
        var DBAGINGBUCKET = row.SNI_CLOSURE_STATUS;
        if (DBAGINGBUCKET == "Overdue" || DBAGINGBUCKET == "Late") {

            $('tr[data-uid="' + row.uid + '"] td:nth-child(16)').append('<img class="Banti" name="imgIndiacot" style="float:right !important; height:20px !important;" src="' + bulletred + '"/>');

        }
        if (DBAGINGBUCKET == "InProcess") {

            $('tr[data-uid="' + row.uid + '"] td:nth-child(16)').append('<img name="imgIndiacot" style="float:right !important; height:20px !important;" src="' + bulletYellow + '"/>');


        }
    });
}

function AddIndiactor_SNI_ReleaseDate() {
    var bulletred = "../Content/img/bullet-red.png";
    var bulletYellow = "../Content/img/bullet-yellow.png";
    var bulletGreen = "../Content/img/bullet-green.png";
    var grid = $("#grdOrders").data("kendoGrid");
    var data = grid.dataSource.data();
    $.each(data, function (i, row) {
        var OVERALL_INSTALLATION_STATUS = row.OVERALL_INSTALLATION_STATUS;
        var DBAGINGBUCKET = row.SNI_CLOSURE_STATUS;
        var ReleaseDate = convert(row.EXPECTED_RELEASE_DATE);
        var CurrentDate = new Date();
        CurrentDate = convert(CurrentDate);
        var date = new Date();
        date.setDate(date.getDate() + 7);
        var pastWeekDate = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate();

        //Format Dates to Server IST Date

        row.CUSTOMER_REQ_GI_DATE = convertToServerTimeZone(row.CUSTOMER_REQ_GI_DATE);
        row.COMMIT_DATE = convertToServerTimeZone(row.COMMIT_DATE);
        row.TRIO_LOAD_DATE = convertToServerTimeZone(row.TRIO_LOAD_DATE);

        // End formating

        if (OVERALL_INSTALLATION_STATUS == "Fully Installed") {

            $('tr[data-uid="' + row.uid + '"] td:nth-child(11)').append('<img name="imgIndiacot" style="float:right !important; height:20px !important;" src="' + bulletGreen + '"/>');
        }
        if (DBAGINGBUCKET == "Overdue" || DBAGINGBUCKET == "Late") {

            $('tr[data-uid="' + row.uid + '"] td:nth-child(24)').append('<img class="Banti" name="imgIndiacot" style="float:right !important; height:20px !important;" src="' + bulletred + '"/>');

        }
        if (DBAGINGBUCKET == "InProcess") {

            $('tr[data-uid="' + row.uid + '"] td:nth-child(24)').append('<img name="imgIndiacot" style="float:right !important; height:20px !important;" src="' + bulletYellow + '"/>');
        }
        if (ReleaseDate != '1970-01-01') {
            if (CurrentDate > ReleaseDate) {
                $('tr[data-uid="' + row.uid + '"] td:nth-child(25)').append('<img class="Banti" name="imgIndiacot" style="float:right !important; height:20px !important;" src="' + bulletred + '"/>');
            }

            if (ReleaseDate >= pastWeekDate || ReleaseDate == CurrentDate) {
                $('tr[data-uid="' + row.uid + '"] td:nth-child(25)').append('<img name="imgIndiacot" style="float:right !important; height:20px !important;" src="' + bulletYellow + '"/>');
            }
        }

    });
}

function AddIndiactor_ReleaseDate() {
    var bulletred = "../Content/img/bullet-red.png";
    var bulletYellow = "../Content/img/bullet-yellow.png";
    var grid = $("#grdOrders").data("kendoGrid");
    var data = grid.dataSource.data();
    $.each(data, function (i, row) {

        row.CUSTOMER_REQ_GI_DATE = convertToServerTimeZone(row.CUSTOMER_REQ_GI_DATE);
        row.COMMIT_DATE = convertToServerTimeZone(row.COMMIT_DATE);
        row.TRIO_LOAD_DATE = convertToServerTimeZone(row.TRIO_LOAD_DATE);


        var DBAGINGBUCKET = row.SNI_CLOSURE_STATUS;
        var ReleaseDate = convert(row.EXPECTED_RELEASE_DATE);
        var CurrentDate = new Date();
        CurrentDate = convert(CurrentDate);
        var date = new Date();
        date.setDate(date.getDate() + 7);
        var pastWeekDate = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate();
        if (DBAGINGBUCKET == "Overdue" || DBAGINGBUCKET == "Late") {

            $('tr[data-uid="' + row.uid + '"] td:nth-child(18)').append('<img class="Banti" name="imgIndiacot" style="float:right !important; height:20px !important;" src="' + bulletred + '"/>');

        }
        if (DBAGINGBUCKET == "InProcess") {

            $('tr[data-uid="' + row.uid + '"] td:nth-child(18)').append('<img name="imgIndiacot" style="float:right !important; height:20px !important;" src="' + bulletYellow + '"/>');
        }
        if (ReleaseDate != '1970-01-01') {
            if (CurrentDate > ReleaseDate) {
                $('tr[data-uid="' + row.uid + '"] td:nth-child(19)').append('<img class="Banti" name="imgIndiacot" style="float:right !important; height:20px !important;" src="' + bulletred + '"/>');
            }

            if (ReleaseDate >= pastWeekDate || ReleaseDate == CurrentDate) {
                $('tr[data-uid="' + row.uid + '"] td:nth-child(19)').append('<img name="imgIndiacot" style="float:right !important; height:20px !important;" src="' + bulletYellow + '"/>');
            }
        }

    });
}



function AddIndiactor_Aging_STR() {
    var bulletred = "../Content/img/bullet-red.png";
    var bulletYellow = "../Content/img/bullet-yellow.png";
    var grid = $("#grdOrders").data("kendoGrid");
    var data = grid.dataSource.data();
    $.each(data, function (i, row) {

        //        row.ReqDlyDate = convertToServerTimeZone(row.ReqDlyDate);
        //        row.DELIVERY_BLOCK_CUT_OFF_DATE = convertToServerTimeZone(row.DELIVERY_BLOCK_CUT_OFF_DATE);
        //        row.SHIPMENT_CUT_OFF_DATE = convertToServerTimeZone(row.SHIPMENT_CUT_OFF_DATE);

        var DBAGINGBUCKET = row.DB_AGING_BUCKET;
        var ReleaseDate = convert(row.EXPECTED_RELEASE_DATE);
        var CurrentDate = new Date();
        CurrentDate = convert(CurrentDate);
        var date = new Date();
        date.setDate(date.getDate() + 7);
        var pastWeekDate = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate();
        if (DBAGINGBUCKET == "> 90 days") {
            if ($('tr[data-uid="' + row.uid + '"] td:nth-child(7)').find('img').length > 0) {
            }
            else {
                $('tr[data-uid="' + row.uid + '"] td:nth-child(9)').append('<img class="Banti" name="imgIndiacot" style="float:right !important; height:20px !important;" src="' + bulletred + '"/>');
            }
        }
        if (DBAGINGBUCKET == "60 - 90 Days") {
            if ($('tr[data-uid="' + row.uid + '"] td:nth-child(9)').find('img').length > 0) {
            }
            else {
                $('tr[data-uid="' + row.uid + '"] td:nth-child(9)').append('<img name="imgIndiacot" style="float:right !important; height:20px !important;" src="' + bulletYellow + '"/>');
            }

        }

        if (ReleaseDate != '1970-01-01') {
            if (CurrentDate > ReleaseDate) {
                $('tr[data-uid="' + row.uid + '"] td:nth-child(27)').append('<img class="Banti" name="imgIndiacot" style="float:right !important; height:20px !important;" src="' + bulletred + '"/>');
            }

            if (ReleaseDate >= pastWeekDate || ReleaseDate == CurrentDate) {
                $('tr[data-uid="' + row.uid + '"] td:nth-child(27)').append('<img name="imgIndiacot" style="float:right !important; height:20px !important;" src="' + bulletYellow + '"/>');
            }
        }

    });
}


function AddIndiactor_Aging() {
    var bulletred = "../Content/img/bullet-red.png";
    var bulletYellow = "../Content/img/bullet-yellow.png";
    var grid = $("#grdOrders").data("kendoGrid");
    var data = grid.dataSource.data();
    $.each(data, function (i, row) {
        var DBAGINGBUCKET = row.DB_AGING_BUCKET;
        var ReleaseDate = convert(row.EXPECTED_RELEASE_DATE);
        var CurrentDate = new Date();
        CurrentDate = convert(CurrentDate);
        var date = new Date();
        date.setDate(date.getDate() + 7);
        var pastWeekDate = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate();

        //Format Dates to Server IST Date

        row.CUSTOMER_REQ_GI_DATE = convertToServerTimeZone(row.CUSTOMER_REQ_GI_DATE);
        row.COMMIT_DATE = convertToServerTimeZone(row.COMMIT_DATE);
        row.TRIO_LOAD_DATE = convertToServerTimeZone(row.TRIO_LOAD_DATE);

        // End formating


        if (DBAGINGBUCKET == "> 90 days") {
            if ($('tr[data-uid="' + row.uid + '"] td:nth-child(7)').find('img').length > 0) {
            }
            else {
                $('tr[data-uid="' + row.uid + '"] td:nth-child(7)').append('<img class="Banti" name="imgIndiacot" style="float:right !important; height:20px !important;" src="' + bulletred + '"/>');
            }
        }
        if (DBAGINGBUCKET == "60 - 90 Days") {
            if ($('tr[data-uid="' + row.uid + '"] td:nth-child(7)').find('img').length > 0) {
            }
            else {
                $('tr[data-uid="' + row.uid + '"] td:nth-child(7)').append('<img name="imgIndiacot" style="float:right !important; height:20px !important;" src="' + bulletYellow + '"/>');
            }

        }

        if (ReleaseDate != '1970-01-01') {
            if (CurrentDate > ReleaseDate) {
                $('tr[data-uid="' + row.uid + '"] td:nth-child(25)').append('<img class="Banti" name="imgIndiacot" style="float:right !important; height:20px !important;" src="' + bulletred + '"/>');
            }

            if (ReleaseDate >= pastWeekDate || ReleaseDate == CurrentDate) {
                $('tr[data-uid="' + row.uid + '"] td:nth-child(25)').append('<img name="imgIndiacot" style="float:right !important; height:20px !important;" src="' + bulletYellow + '"/>');
            }
        }



    });
}

function AddIndiactor_DueDate() {
    var bulletred = "../Content/img/bullet-red.png";
    var bulletYellow = "../Content/img/bullet-yellow.png";
    var grid = $("#grdMyFollowups").data("kendoGrid");
    var data = grid.dataSource.data();
    var CurrentDate = new Date();
    CurrentDate = convert(CurrentDate);
    $.each(data, function (i, row) {

        //Format Dates to Server IST Date

        row.CUSTOMER_REQ_GI_DATE = convertToServerTimeZone(row.CUSTOMER_REQ_GI_DATE);
        row.COMMIT_DATE = convertToServerTimeZone(row.COMMIT_DATE);
        row.TRIO_LOAD_DATE = convertToServerTimeZone(row.TRIO_LOAD_DATE);

        // End formating

        var DueDate = convert(row.DueDate);
        if (CurrentDate > DueDate) {
            if ($('tr[data-uid="' + row.uid + '"] td:nth-child(8)').find('img').length > 0) {
            }
            else {
                $('tr[data-uid="' + row.uid + '"] td:nth-child(8)').append('<img class="Banti" name="imgIndiacot" style="float:right !important; height:20px !important;" src="' + bulletred + '"/>');
            }
        }
        if (CurrentDate == DueDate) {
            if ($('tr[data-uid="' + row.uid + '"] td:nth-child(8)').find('img').length > 0) {
            }
            else {
                $('tr[data-uid="' + row.uid + '"] td:nth-child(8)').append('<img name="imgIndiacot" style="float:right !important; height:20px !important;" src="' + bulletYellow + '"/>');
            }
        }
    });
}

function clear_form_elementsKendo(ele) {
    $("[data-role='multiselect']").each(function (e) {
        var multi = $(this).data("kendoMultiSelect");
        multi.value("");
        multi.input.blur();
    });

    $("#fltr_CreatedBy").val("");
    $("#fltr_DeltaLoadDateFrom").val("");
    $("#fltr_DeltaLoadDateTo").val("");
    $("#fltr_ClosureDaysDeltaFrom").val("");
    $("#fltr_ClosureDaysDeltaTo").val("");
    $("#fltr_SoldtoAccountID").val("");
    $("#fltr_ShiptoAccountID").val("");
    $("#fltr_ZUAccountID").val("");
    $("#fltr_ShiptoAccount").val("");
}

function ResetAdvancedFilters() {
    showProgress();

    var CurrentReport = $("#hid_CurrentBOReport").val();
    var Regions = $("#fltr_Region").val();

    clear_form_elementsKendo(this);

    var Regionmulti = $("#fltr_Region").data("kendoMultiSelect");
    Regionmulti.value(Regions);

    $.ajax({
        cache: false,
        type: "POST",
        traditional: true,
        async: false,
        url: '../Generic/AdvancedFilter/',
        data: {
            Report: CurrentReport,
            Region: Regions,
            Business: "",
            Division: "",
            PrimaryProduct: "",
            PL: "",
            BacklogStatus: "",
            CreatedBy: "",
            DollarBucket: "",
            SoldToAccountID: "",
            shipToAccountID: "",
            ZUAccountID: "",
            SoldToCountry: "",
            ShipToAccount: "",
            ShipToCountry: "",
            BTM: "",
            BTM_Manager: "",
            PaymentTerm: "",
            BillingBlock_Header: "",
            DeliveryBlock_Header: "",
            BillingBlock_Item: "",
            DeliveryBlock_Item: "",
            DeltaLoaddate: 0,
            ClosureDaysDeltaFrom: 0,
            ClosureDaysDeltaTo: 0
        },
        datatype: "json",
        success: function (data) {
            hideProgress();

            if (data.length == 0) {
                alert("No data found for this criteria.");
            }
            else {
                var ms = $("#grdOrders").data('kendoGrid');

                for (var i = 0; i < data.length; i++) {
                    var item = data[i];

                    item.COMMIT_DATE = formatJSONDateFromNumericdate(item.COMMIT_DATE);
                    item.TRIO_LOAD_DATE = formatJSONDateFromNumericdate(item.TRIO_LOAD_DATE);
                    item.CUSTOMER_REQ_GI_DATE = formatJSONDateFromNumericdate(item.CUSTOMER_REQ_GI_DATE);

                    item.EXPECTED_RELEASE_DATE = formatJSONDateFromNumericdate(item.EXPECTED_RELEASE_DATE);

                }

                ms.dataSource.data(data);
            }

            var AdvancedsearchWindow = $("#AdvancedsearchWindow");
            AdvancedsearchWindow.data("kendoWindow").close();

        },
        statusCode: {
            404: function (content) { alert('cannot find resource'); hideProgress(); },
            500: function (content) { alert('internal server error'); hideProgress(); }
        },
        error: function (req, status, errorObj) {
            // handle status === "timeout"
            // handle other errors
        }
    });

}


function ApplyAdvancedFilters() {
    var Regions = $("#fltr_Region").val();
    var Businesss = $("#fltr_Business").val();
    var Divs = $("#fltr_Div").val();
    var PrimaryProducts = $("#fltr_Primary_Product").val();
    var PLs = $("#fltr_PL").val();
    var BacklogStatuss = $("#fltr_Backlog_Status").val();
    var DollarBuckets = $("#fltr_Dollar_Bucket").val();
    var SoldToCountrys = $("#fltr_SoldToCountry").val();
    var ShipToAccounts = $("#fltr_ShiptoAccount").val();
    var ShipToCountrys = $("#fltr_ShiptoCountry").val();
    var ZUCountrys = $("#fltr_ZUCountry").val();
    var BTMs = $("#fltr_BTM").val();
    var BTMManagers = $("#fltr_BTMManager").val();
    var PaymentTermss = $("#fltr_Payment_Terms").val();
    var CreatedBy = $("#fltr_CreatedBy").val();
    var SoldToAccountIDs = $("#fltr_SoldtoAccountID").val();
    var ShipToAccountIDs = $("#fltr_ShiptoAccountID").val();
    var ZUAccountIDs = $("#fltr_ZUAccountID").val();

    var BB_Header = $("#fltr_BB_Header").val();
    var DB_Header = $("#fltr_DB_Header").val();
    var BB_Item = $("#fltr_BB_Item").val();
    var DB_Item = $("#fltr_DB_Item").val();

    var DeltaLoadDat = $("#fltr_DeltaLoadDateFrom").val() + " and " + $("#fltr_DeltaLoadDateTo").val();
    if ($("#fltr_DeltaLoadDateFrom").val() == "") { DeltaLoadDat = ""; }

    var ClosureDaysDeltasFrom = $("#fltr_ClosureDaysDeltaFrom").val();
    var ClosureDaysDeltasTo = $("#fltr_ClosureDaysDeltaTo").val();

    var CurrentReport = $("#hid_CurrentBOReport").val();

    if (Businesss == null) Businesss = "";
    if (Divs == null) Divs = "";
    if (PrimaryProducts == null) PrimaryProducts = "";
    if (PLs == null) PLs = "";
    if (BacklogStatuss == null) BacklogStatuss = "";
    if (DollarBuckets == null) DollarBuckets = "";
    if (SoldToCountrys == null) SoldToCountrys = "";
    if (ShipToAccounts == null) ShipToAccounts = "";
    if (ShipToCountrys == null) ShipToCountrys = "";
    if (ZUCountrys == null) ZUCountrys = "";
    if (BTMs == null) BTMs = "";
    if (BTMManagers == null) BTMManagers = "";
    if (PaymentTermss == null) PaymentTermss = "";
    if (CreatedBy == null) CreatedBy = "";

    if (ShipToAccountIDs == null) ShipToAccountIDs = "";
    if (ShipToAccountIDs == null) ShipToAccountIDs = "";
    if (ZUAccountIDs == null) ZUAccountIDs = "";
    if (BB_Header == null) BB_Header = "";
    if (DB_Header == null) DB_Header = "";
    if (BB_Item == null) BB_Item = "";
    if (DB_Item == null) DB_Item = "";
    if (DeltaLoadDat == null) DeltaLoadDat = "";
    if (ClosureDaysDeltasFrom == null) ClosureDaysDeltasFrom = "";
    if (ClosureDaysDeltasTo == null) ClosureDaysDeltasTo = "";

    showProgress();
    $.ajax({
        cache: false,
        type: "POST",
        traditional: true,
        async: false,
        url: '../Generic/AdvancedFilter/',
        data: {
            Report: CurrentReport,
            Region: Regions.toString(),
            Business: Businesss.toString(),
            Division: Divs.toString(),
            PrimaryProduct: PrimaryProducts.toString(),
            PL: PLs.toString(),
            BacklogStatus: BacklogStatuss.toString(),
            CreatedBy: CreatedBy.toString(),
            DollarBucket: DollarBuckets.toString(),
            SoldToAccountID: SoldToAccountIDs,
            shipToAccountID: ShipToAccountIDs,
            ZUAccountID: ZUAccountIDs,
            SoldToCountry: SoldToCountrys.toString(),
            ShipToAccount: ShipToAccounts.toString(),
            ShipToCountry: ShipToCountrys.toString(),
            BTM: BTMs.toString(),
            BTM_Manager: BTMManagers.toString(),
            PaymentTerm: PaymentTermss.toString(),
            BillingBlock_Header: BB_Header.toString(),
            DeliveryBlock_Header: DB_Header.toString(),
            BillingBlock_Item: BB_Item.toString(),
            DeliveryBlock_Item: DB_Item.toString(),
            DeltaLoaddate: DeltaLoadDat,
            ClosureDaysDeltaFrom: ClosureDaysDeltasFrom,
            ClosureDaysDeltaTo: ClosureDaysDeltasTo
        },
        datatype: "json",
        success: function (data) {
            hideProgress();

            if (data.length == 0) {
                alert("No data found for this criteria.");
            }
            else {
                var ms = $("#grdOrders").data('kendoGrid');

                for (var i = 0; i < data.length; i++) {
                    var item = data[i];

                    item.COMMIT_DATE = formatJSONDateFromNumericdate(item.COMMIT_DATE);
                    item.TRIO_LOAD_DATE = formatJSONDateFromNumericdate(item.TRIO_LOAD_DATE);
                    item.CUSTOMER_REQ_GI_DATE = formatJSONDateFromNumericdate(item.CUSTOMER_REQ_GI_DATE);

                    item.EXPECTED_RELEASE_DATE = formatJSONDateFromNumericdate(item.EXPECTED_RELEASE_DATE);

                }

                ms.dataSource.data(data);
            }

            var AdvancedsearchWindow = $("#AdvancedsearchWindow");
            AdvancedsearchWindow.data("kendoWindow").close();

        },
        statusCode: {
            404: function (content) { alert('cannot find resource'); hideProgress(); },
            500: function (content) { alert('internal server error'); hideProgress(); }
        },
        error: function (req, status, errorObj) {
            // handle status === "timeout"
            // handle other errors
        }
    });

}


// Filter Menu Events

function onFilterMenuInit(e) {

    if (e.field === "SALES_ORG" || e.field === "ORDER_OWNER" || e.field === "ZU_COUNTRY" || e.field === "PAYMENT_TERMS" || e.field === "BILLING_BLOCK_CD" || e.field === "INCOTERMS" || e.field === "REASON_CODE") {

        var filterMultiCheck = this.thead.find("[data-field=" + e.field + "]").data("kendoFilterMultiCheck");
        filterMultiCheck.container.empty();
        filterMultiCheck.checkSource.sort({ field: e.field, dir: "asc" });
        var view = filterMultiCheck.checkSource.view().toJSON();
        filterMultiCheck.checkSource.data(view);
        filterMultiCheck.createCheckBoxes();
    }
}