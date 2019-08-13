
//---------------- CNG Project Scripts -----------------------------//

var AllPieRows = [];
var AllMaterials = [];
var UniqueMaterials = [];
var LatestComment;
var checkedIds = {};
var checked = [];
var currentReportSelected = $("#hid_CurrentReport").val();


function checkAll(ele) {
    var state = $(ele).is(':checked');
    var grid = $('#grdOrders').data().kendoGrid;

    $("#grdOrders input").each(function () {

        if (state == true) {
            $(this).prop('checked', true);
            selectRow(this);
        }
        else {
            $(this).prop('checked', false);
            selectRow(this);
        }
    });
}



//on click of the checkbox:
function selectRow(ele) {
    var checked = ele.checked,
        row = $(ele).closest("tr"),
        grid = $("#grdOrders").data("kendoGrid"),
        dataItem = grid.dataItem(row);

    if (checkedIds != null) {
        checkedIds[dataItem.SALES_ORDERNO] = checked;

        if (checked) {
            //-select the row
            row.addClass("k-state-selected");
        } else {
            //-remove selection
            row.removeClass("k-state-selected");
        }

        var selectedOrders = 0;

        for (var i in checkedIds) {
            if (checkedIds[i]) {
                selectedOrders += 1;
            }
        }

        if (selectedOrders > 0) {

            $('#cmt_lblSalesOrder').val("Multiple Commenting Mode.");
            $('#cmt_lblorderowner').val("Current Order Owner.");

            document.getElementById('chkAllSelectedordersComment').checked = true;
        }
    }
}


//on dataBound event restore previous selected rows:
function onDataBound(e) {
    var view = this.dataSource.view();
    for (var i = 0; i < view.length; i++) {
        if (checkedIds[view[i].id]) {
            this.tbody.find("tr[data-uid='" + view[i].uid + "']")
                .addClass("k-state-selected")
                .find(".checkbox")
                .attr("checked", "checked");
        }
    }
}

function GetUnique(inputArray) {
    for (var i = 0; i < inputArray.length; i++) {
        if (UniqueMaterials.indexOf(inputArray[i].Material) == -1) {
            if (inputArray[i].Material != "")
                UniqueMaterials.push(inputArray[i].Material);
        }
    }
    return UniqueMaterials;
}

// Ready for Sign Off
function showSelectedOrders() {
    checked.length = 0;

    for (var i in checkedIds) {
        if (checkedIds[i]) {
            checked.push(i);
        }
    }

    var result = confirm("Are you sure to Approve comments for # " + checked.length + " Selected Orders ?");
    //alert("User Selected : " + result);

    if (result == true) {
        ReadySignOff();
    }
}

function ReadySignOff() {
    currentReportSelected = $("#hid_CurrentReport").val();

    $.ajax(
        {
            type: 'POST',
            url: '../BCR/SignOffOrderComments/',
            dataType: 'json',
            data: { Report: currentReportSelected, Owner: "", Area: "SignOff", Orders: checked.toString() },
            success: function (result) {
                $("#grdOrders").data("kendoGrid").dataSource.data(result);
                showSignOffSuccess(checked.length.toString());

                //alert("Sign-off approved for " + checked.length + " orders. Pending Summary will be updated automatically.");
            }
        });

}



// Selection Changed report definition

function onReportSelected(e) {
    var dataItem = this.dataItem(e.item.index());
    showProgress();
    $.ajax(
    {
        type: 'POST',
        url: '../Generic/SelectedReportData/',
        dataType: 'json',
        data: { ReportName: dataItem.Text, Area: $("#hid_Area").val() },
        success: function (result) {
            hideProgress();
            $("#SelectedReportName").val(dataItem.Text);
            if (result.length > 0) {
                $("#grdOrders").data("kendoGrid").dataSource.data(result[4].RawData);
            }

            currentReportSelected = dataItem.Text;
            GetchangedReportDefinition(dataItem.Text);
        }
    });

}

function onReportSelectedforProcessDetails(e) {
    var dataItem = this.dataItem(e.item.index());
    showProgress();
    $.ajax(
    {
        type: 'POST',
        url: '../BCR/SelectedReportData/',
        dataType: 'json',
        data: { Report: dataItem.Text, Area: $("#hid_Area").val() },
        success: function (result) {

            hideProgress();

            if (result.length > 0) {

                for (var i = 0; i < result.length; i++) {
                    result[i].REVIEW_DATE = formatJSONDateFromNumericdate(result[i].REVIEW_DATE);
                    result[i].COMMENTED_DATE = formatJSONDateFromNumericdate(result[i].COMMENTED_DATE);
                    result[i].EXPECTED_RELEASE_DATE = formatJSONDateFromNumericdate(result[i].EXPECTED_RELEASE_DATE);
                }

                $("#SelectedReportName").val(dataItem.Text);
                $("#grdOrders").data("kendoGrid").dataSource.data(result);
                currentReportSelected = dataItem.Text;
                GetchangedReportDefinition(dataItem.Text);
            }
            else
                alert("No Pending Orders for this report.");

        }
    });

}



function GetchangedReportDefinition(reportName) {

    $.ajax(
        {
            type: 'POST',
            url: '../BCR/SelectedReportDefinition/',
            dataType: 'json',
            data: { Report: reportName },
            success: function (result) {

                var res = result.split("^");

                $("#Help_ReportName").val(res[0]);
                $("#Help_Description2").val(res[1]);
                $("#Help_Description1").val(res[2]);


                if (res[1] != "Item Level commenting") {
                    $('#cmb_Materials').prop("disabled", false);
                }

            }
        });
}

// Process Details Grid Row Selection

function OnOSBRChange(e) {
    var grid = $("#grdOrders").data("kendoGrid");

    //Getting selected Item(s)
    var selectedItems = grid.dataItem(grid.select());

    ReadOrderHeaderDetails(selectedItems.SALES_DOC);
    ReadOrderBlockDetails(selectedItems.SALES_DOC);
    ReadOrderLineItem(selectedItems.SALES_DOC);
    ReadOrderDeliveryDetails(selectedItems.SALES_DOC);
    ReadOrderPartnerDetails(selectedItems.SALES_DOC);
    ReadOrderCommentsHistory(selectedItems.SALES_DOC);

    ReadOrderOpenFollowHistory(selectedItems.SALES_DOC);
    ReadOrderCloseFollowHistory(selectedItems.SALES_DOC);

}


function OnChange(e) {
    var grid = $("#grdOrders").data("kendoGrid");

    showProgress();

    //Getting selected Item(s)
    var selectedItems = grid.dataItem(grid.select());

    // Set Commenting fields
    $("#cmt_lblSalesOrder").val(selectedItems.SALES_ORDERNO);
    $("#cmt_lblMaterial").val("");
    $("#cmb_Materials").empty();
    $("#cmt_lblorderowner").val(selectedItems.ORDER_OWNER);

    $("#cmt_lblorderNetValue").val(selectedItems.BACKLOG_AMT.toFixed(2).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"));
    $("#cmt_lblSalesOrg").val(selectedItems.SALES_ORG);
    $("#cmt_Sold_To_Customer").val(selectedItems.SOLD_TO_PARTY_NAME);

    var ReasonCodeList = $("#cmt_ReasonCode").data("kendoDropDownList");

    if (ReasonCodeList != null) {
        ReasonCodeList.select(function (dataItem) {
            return dataItem.Value === selectedItems.REASON_CODE;
        });
    }

    $("#cmt_ActionOwner").val(selectedItems.NEXT_ACTION_OWNER);
    $("#cmt_ClearDate").val(formatJSONDate(selectedItems.EXPECTED_RELEASE_DATE));
    $("#cmt_ApprovedToDate").val(formatJSONDate(selectedItems.APPROVED_TO_DATE));
    $("#cmt_Comments").val(selectedItems.LATEST_COMMENT);

    // Set New Followup Form Fields
    $("#Followup_SalesOrder").val(selectedItems.SALES_ORDERNO);
    $("#Followup_owner").val($("#hid_SessionUSer_NTLOGIN").val());
    $("#Followup_Customername").val(selectedItems.SOLD_TO_PARTY_NAME);
    $("#Followup_Comments").val('Net Value:  $' + selectedItems.BACKLOG_AMT.toFixed(2).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"));

    $("#Followup_Customername").prop('readonly', true);

    ReadOrderTabsInfo(selectedItems.SALES_ORDERNO);

    hideProgress();
}


function Set_DeliveryInfo(selectedItems) {
    // Set Order Delivery Info Fields
    $("#Delivery_SalesOrder").val(selectedItems.SALES_ORDERNO);
    $("#Delivery_CRDD").val(formatJSONDateFromNumericdate(selectedItems.CUSTOMER_REQ_GI_DATE));
    $("#Delivery_AGIDate").val(formatJSONDateFromNumericdate(selectedItems.ACTUAL_GI_DATE));
    $("#Delivery_CompleteDeliveryRequired").val(selectedItems.COMPLETE_DELIVERY_HEADER);
    $("#Delivery_Priority").val(selectedItems.DELV_PRIO_CD_HDR);
    $("#Delivery_Prio_Text").val(selectedItems.DELV_PRIO_DESC_HDR);
    $("#Delivery_EDA").val(selectedItems.EARLY_DEL_ACCEPTABLE);
    $("#Delivery_Shipment_Point").val(selectedItems.SHIPPING_POINT);
    $("#Delivery_Shipment_CutOffDate").val(formatJSONDateFromNumericdate(selectedItems.SHIPMENT_CUT_OFF_DATE));
    $("#Delivery_Shipping_Condition").val(selectedItems.SHIP_CONDTN_DESC);
    $("#Delivery_Shipping_Date_Change_Reason").val(selectedItems.SHIP_DT_CHANGE_REASON);
    $("#Delivery_Date_Change_Reason").val(selectedItems.DLVY_DT_CHANGE_REASON);
}

function Set_PartnerInfo(selectedItems) {
    // Set Order Partner Info Fields
    $("#Partner_soldtoAccount").val(selectedItems.SOLD_TO_PARTY);
    $("#Partner_soldtoCustomer").val(selectedItems.SOLD_TO_PARTY_NAME);
    $("#Partner_soldtoCountry").val(selectedItems.SOLD_TO_COUNTRY);
    $("#Partner_shiptoAccountNumber").val(selectedItems.SHIP_TO_PARTY);
    $("#Partner_shiptoAccount").val(selectedItems.SHIP_TO_PARTY_NAME);
    $("#Partner_shiptoCountry").val(selectedItems.SHIP_TO_COUNTRY);
    $("#Partner_salesForce").val(selectedItems.QUOTA_SF);
    $("#Partner_salesRep").val(selectedItems.FE_FE_DESC);
    $("#Partner_BTM").val(selectedItems.SRTATTR_AREA_MGR_NME);
    $("#Partner_BTMManager").val(selectedItems.SRTATTR_DISTRICT_MGR_NME);
    $("#Partner_ZUAccountID").val(selectedItems.ZU_ACCOUNT_ID);
    $("#Partner_ZUAccount").val(selectedItems.ZU_ACCOUNT_NAME);
    $("#Partner_ZUCountry").val(selectedItems.ZU_COUNTRY);
    $("#Partner_ENAccount").val(selectedItems.EN_ACCOUNT_NAME);
    $("#Partner_ENCountry").val(selectedItems.EN_COUNTRY);
}

function Set_BlockedDetails(selectedItems) {
    // Set Order Block Info Fields
    $("#Block_SalesOrder").val(selectedItems.SALES_ORDERNO);
    $("#Block_Backlogstatus").val(selectedItems.BACKLOG_STATUS);
    $("#Block_NetValue").val('$' + selectedItems.BACKLOG_AMT.toFixed(2).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"));
    $("#Block_CompleteDelivery").val(selectedItems.COMPLETE_DELIVERY_HEADER);
    $("#Block_HeaderDeliveryBlock").val(selectedItems.DELIVERY_BLK_HDR_DESC);
    $("#Block_HeaderBillingBlock").val(selectedItems.BILLING_BLOCK_DESC);
    $("#Block_NLHD").val(selectedItems.NLHD_STATUS);
    $("#Block_DBCutOffDate").val(formatJSONDateFromNumericdate(selectedItems.DELIVERY_BLOCK_CUT_OFF_DATE));
    $("#Block_TotalBlocks").val(selectedItems.DELV_BLK_COUNT);
    $("#Block_TotalDaysBlocked").val(selectedItems.Total_Days_Blocked);
    $("#Block_LastDeliveryBlock").val(selectedItems.SAP_DELV_BLK_DAYS);
    $("#Block_DtLastDeliveryBlockApplied").val(formatJSONDateFromNumericdate(selectedItems.DELV_BLK_LAST_APPLD_DT));
    $("#Block_DtLastDeliveryBlockRemoved").val(formatJSONDateFromNumericdate(selectedItems.DELV_BLK_REL_DT));
    $("#Block_DeliveryDtChangeReason").val(selectedItems.DLVY_DT_CHANGE_REASON);
    $("#Block_Type").val(selectedItems.DELV_BLK_TYPE);
    $("#Block_DeliveryBlockCode").val(selectedItems.DELIVERY_BLK_HDR_CD);
    $("#Block_BillingBlockCode").val(selectedItems.BILLING_BLOCK_CD);
}

function Set_OrderHeaderDetails(selectedItems) {
    // Set Order Header Fields

    $("#header_SalesOrder").val(selectedItems.SALES_ORDERNO);
    $("#header_PO").val(selectedItems.CUSTOMER_PO_NO);
    $("#header_CreatedBy").val(selectedItems.ORDER_CREATED_BY);
    $("#header_OrderOwner").val(selectedItems.ORDER_OWNER);
    $("#header_Backlogstatus").val(selectedItems.BACKLOG_STATUS);
    $("#header_InstallationStatus").val(selectedItems.OVERALL_INSTALLATION_STATUS);
    $("#header_Backlog").val('$' + selectedItems.BACKLOG_AMT.toFixed(2).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"));
    $("#header_OrderDate").val(formatJSONDateFromNumericdate(selectedItems.ORDER_DT));
    $("#cmt_lblorderDate").val(formatJSONDateFromNumericdate(selectedItems.ORDER_DT));
    $("#header_OrderRegion").val(selectedItems.REGION);
    $("#header_CompleteDelivery").val(selectedItems.COMPLETE_DELIVERY_HEADER);
    $("#header_PrimaryProduct").val(selectedItems.PRIMARY_PRODUCT);
    $("#header_Aging").val(selectedItems.ORDER_AGE);
    $("#header_Aging_Bucket").val(selectedItems.Aging_Bucket);
    $("#header_SNI_Aging").val(selectedItems.SNI_AGE);
    $("#header_SNI_Aging_Bucket").val(selectedItems.SNI_AGING_BUCKET);
    $("#header_PaymentTerm").val(selectedItems.PAYMENT_TERMS);
    $("#header_PaymentTermDescr").val(selectedItems.PAYMENT_TERMS_DESC);
    $("#header_PaymentType").val(selectedItems.PAYMENT_TYPE);
    $("#header_SNIClosureDaysDelta").val(selectedItems.SNI_CLOSURE_DAYS_DELTA);
    $("#header_DBClosureDaysDelta").val(selectedItems.DB_CLOSURE_DAYS_DELTA);
    $("#header_DeltaLoadDateBucket").val(selectedItems.DELTA_LOAD_DATE_BUCKET);
}

// for Open Followup on order basis
function ReadOrderOpenFollowHistory(SALES_ORDERNO) {
    $.ajax({
        cache: false,
        type: "POST",
        async: false,
        url: '../Generic/GetFollowUpSummaryBySalesOrderOpen/',
        data: { Sales_Order: SALES_ORDERNO },
        datatype: "json",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                var item = data[i];

                item.DueDate = formatJSONDateFromNumericdate(item.DueDate);
                item.Created_On = formatJSONDateFromNumericdate(item.Created_On);
                item.Modified_On = formatJSONDateFromNumericdate(item.Modified_On);

            }

            var ms = $("#grdOrderOpenFollowups").data('kendoGrid');
            ms.dataSource.data(data);

            $("#lblOpenFollowups").text(data.length.toString());

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

// for Close Followup on order basis
function ReadOrderCloseFollowHistory(SALES_ORDERNO) {
    $.ajax({
        cache: false,
        type: "POST",
        async: false,
        url: '../Generic/GetFollowUpSummaryBySalesOrderClose/',
        data: { Sales_Order: SALES_ORDERNO },
        datatype: "json",
        success: function (data) {
            //alert(data);
            for (var i = 0; i < data.length; i++) {
                var item = data[i];

                item.DueDate = formatJSONDateFromNumericdate(item.DueDate);
                item.Created_On = formatJSONDateFromNumericdate(item.Created_On);
                item.Modified_On = formatJSONDateFromNumericdate(item.Modified_On);

            }

            var ms = $("#grdOrderClosedFollowups").data('kendoGrid');
            ms.dataSource.data(data);

            $("#lblClosedFollowups").text(data.length.toString());

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

//end here

function ReadOrderHeaderDetails(SALES_ORDERNO) {

    $.ajax({
        cache: false,
        type: "POST",
        async: false,
        url: '../Generic/ReadOrderHeaderInfo/',
        data: { Sales_Order: SALES_ORDERNO },
        datatype: "json",
        success: function (data) {
            if (data.length > 0)
                Set_OrderHeaderDetails(data[0]);
            else
                alert("User Session Expired. Please Signout & Signin again in CNG.");

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

function search_ReadOrderHeaderDetails(SALES_ORDERNO) {

    $.ajax({
        cache: false,
        type: "POST",
        async: false,
        url: '../../Generic/ReadOrderHeaderInfo/',
        data: { Sales_Order: SALES_ORDERNO },
        datatype: "json",
        success: function (data) {
            if (data.length > 0)
                Set_OrderHeaderDetails(data[0]);
            else
                alert("No Header Information is available for this order.This order may be Invoiced/Closed.");

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

function ReadOrderLineItem(SALES_ORDERNO) {

    $.ajax({
        cache: false,
        type: "POST",
        async: false,
        url: '../Generic/ReadOrderLineItems/',
        data: { Sales_Order: SALES_ORDERNO },
        datatype: "json",
        success: function (data) {

            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    var item = data[i];

                    item.CUSTOMER_REQ_GI_DATE = formatJSONDateFromNumericdate(item.CUSTOMER_REQ_GI_DATE);
                    item.TRIO_LOAD_DATE = formatJSONDateFromNumericdate(item.TRIO_LOAD_DATE);
                    item.COMMIT_DATE = formatJSONDateFromNumericdate(item.COMMIT_DATE);

                }

                var ms = $("#grdOrderLineItems").data('kendoGrid');
                ms.dataSource.data(data);
            }
            else
                alert("No Item Information is available for this order.This order may be Invoiced/Closed.");

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


function search_ReadOrderLineItem(SALES_ORDERNO) {

    $.ajax({
        cache: false,
        type: "POST",
        async: false,
        url: '../../Generic/ReadOrderLineItems/',
        data: { Sales_Order: SALES_ORDERNO },
        datatype: "json",
        success: function (data) {

            if (data.length > 0) {

                for (var i = 0; i < data.length; i++) {
                    var item = data[i];

                    item.CUSTOMER_REQ_GI_DATE = formatJSONDateFromNumericdate(item.CUSTOMER_REQ_GI_DATE);
                    item.TRIO_LOAD_DATE = formatJSONDateFromNumericdate(item.TRIO_LOAD_DATE);
                    item.COMMIT_DATE = formatJSONDateFromNumericdate(item.COMMIT_DATE);

                }

                var ms = $("#grdOrderLineItems").data('kendoGrid');
                ms.dataSource.data(data);
            }
            else
                alert("No Item Information is available for this order.This order may be Invoiced/Closed.");

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


function ReadOrderBlockDetails(SALES_ORDERNO) {

    $.ajax({
        cache: false,
        type: "POST",
        async: false,
        url: '../Generic/ReadOrderBlockInfo/',
        data: { Sales_Order: SALES_ORDERNO },
        datatype: "json",
        success: function (data) {
            if (data.length > 0)
                Set_BlockedDetails(data[0]);
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


function search_ReadOrderBlockDetails(SALES_ORDERNO) {

    $.ajax({
        cache: false,
        type: "POST",
        async: false,
        url: '../../Generic/ReadOrderBlockInfo/',
        data: { Sales_Order: SALES_ORDERNO },
        datatype: "json",
        success: function (data) {
            if (data.length > 0)
                Set_BlockedDetails(data[0]);
            else
                alert("No Block Information is available for this order.This order may be Invoiced/Closed.");
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


function ReadOrderDeliveryDetails(SALES_ORDERNO) {

    $.ajax({
        cache: false,
        type: "POST",
        async: false,
        url: '../Generic/ReadOrderDeliveryInfo/',
        data: { Sales_Order: SALES_ORDERNO },
        datatype: "json",
        success: function (data) {
            if (data.length > 0)
                Set_DeliveryInfo(data[0]);
            else
                alert("No Delivery Details found for this order. Check whether this order is Invoiced/Closed.");
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

function search_ReadOrderDeliveryDetails(SALES_ORDERNO) {

    $.ajax({
        cache: false,
        type: "POST",
        async: false,
        url: '../../Generic/ReadOrderDeliveryInfo/',
        data: { Sales_Order: SALES_ORDERNO },
        datatype: "json",
        success: function (data) {
            if (data.length > 0)
                Set_DeliveryInfo(data[0]);
            else
                alert("No Delivery Details found for this order. Check whether this order is Invoiced/Closed.");
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


function ReadOrderPartnerDetails(SALES_ORDERNO) {

    $.ajax({
        cache: false,
        type: "POST",
        async: false,
        url: '../Generic/ReadOrderPartnerInfo/',
        data: { Sales_Order: SALES_ORDERNO },
        datatype: "json",
        success: function (data) {
            if (data.length > 0)
                Set_PartnerInfo(data[0]);
            else
                alert("No Partner Information is available for this order.This order may be Invoiced/Closed.");
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


function search_ReadOrderPartnerDetails(SALES_ORDERNO) {

    $.ajax({
        cache: false,
        type: "POST",
        async: false,
        url: '../../Generic/ReadOrderPartnerInfo/',
        data: { Sales_Order: SALES_ORDERNO },
        datatype: "json",
        success: function (data) {
            if (data.length > 0)
                Set_PartnerInfo(data[0]);
            else
                alert("No Partner Information is available for this order.This order may be Invoiced/Closed.");
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



function ReadOrderCommentsHistory(SALES_ORDERNO) {
    $.ajax({
        cache: false,
        type: "POST",
        async: false,
        url: '../Generic/ReadSelectedOrder/',
        data: { Sales_Order: SALES_ORDERNO },
        datatype: "json",
        success: function (data) {

            if (data.length > 0) {

                var target = $("#commentsHistory");
                target.empty();
                AllMaterials.length = 0;
                LatestComment = null;

                for (var i = 0; i < data.length; i++) {
                    var item = data[i];

                    item.Reviewdate = formatJSONDateFromNumericdate(item.Reviewdate);
                    item.Cleardate = formatJSONDateFromNumericdate(item.Cleardate);
                    item.Comment_Date = formatJSONDateFromNumericdate(item.Comment_Date);

                    AllMaterials.push({
                        Material: item.Material
                    });
                }

                var ms = $("#grdOrderHistory").data('kendoGrid');
                ms.dataSource.data(data);

                //Calculate Unique Materials of Order
                UniqueMaterials.length = 0;
                GetUnique(AllMaterials);

                //Update pre-selected Material list for enabling new commenting
                $("#cmb_Materials").find("option").remove();

                $.each(UniqueMaterials, function (key, value) {
                    $('#cmb_Materials').append($('<option selected=selected>', { value: value }).text(value));
                });

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


function search_ReadOrderCommentsHistory(SALES_ORDERNO) {
    $.ajax({
        cache: false,
        type: "POST",
        async: false,
        url: '../../Generic/ReadSelectedOrder/',
        data: { Sales_Order: SALES_ORDERNO },
        datatype: "json",
        success: function (data) {

            if (data.length > 0) {

                var target = $("#commentsHistory");
                target.empty();
                AllMaterials.length = 0;
                LatestComment = null;

                for (var i = 0; i < data.length; i++) {
                    var item = data[i];

                    item.Reviewdate = formatJSONDateFromNumericdate(item.Reviewdate);
                    item.Cleardate = formatJSONDateFromNumericdate(item.Cleardate);
                    item.Comment_Date = formatJSONDateFromNumericdate(item.Comment_Date);

                    AllMaterials.push({
                        Material: item.Material
                    });
                }

                var ms = $("#grdOrderHistory").data('kendoGrid');
                ms.dataSource.data(data);
                //Calculate Unique Materials of Order
                UniqueMaterials.length = 0;
                GetUnique(AllMaterials);

                //Update pre-selected Material list for enabling new commenting
                $("#cmb_Materials").find("option").remove();

                $.each(UniqueMaterials, function (key, value) {
                    $('#cmb_Materials').append($('<option selected=selected>', { value: value }).text(value));
                });

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


function formatJSONDate(jsonDate) {
    if (jsonDate != null) {
        try {
            var month = jsonDate.getMonth() + 1;
            var day = jsonDate.getDate();
            var year = jsonDate.getFullYear();
            var newDate = month + "/" + day + "/" + year;

            if (newDate == "1/1/1900") newDate = "";

            //return convertToServerTimeZone(newDate);
            return newDate
        }
        catch (exception) {
            formatJSONDateFromNumericdate(jsonDate);
        }
    }
    else {
        //Do Nothing
    }
}

function convertToServerTimeZone(dt) {
    //IST Time Offset from UTC
    var offset = 5.5;
    var clientDate = new Date(dt);

    // Calculcate UTC Time from local date
    utc = clientDate.getTime() + (clientDate.getTimezoneOffset() * 60000);

    //Bring Local UTC Time to IST time which is equal to server IST time
    serverDate = new Date(utc + (3600000 * offset));

    return serverDate;
}

function formatJSONDateFromNumericdate(jsonDate) {
    if (jsonDate != null) {
        try {
            var dateString = jsonDate.substr(6);
            var currentTimeString = new Date(parseInt(dateString));

            var currentTime = convertToServerTimeZone(currentTimeString);

            var month = currentTime.getMonth() + 1;
            var day = currentTime.getDate();
            var year = currentTime.getFullYear();
            var newDate = month + "/" + day + "/" + year;

            if (newDate == "1/1/1900") newDate = "";

            return newDate;
        }
        catch (exception) {
            formatJSONDate(jsonDate)
        }
    }
    else {
        //Do Nothing
    }
}



function convert(str) {
    var date = new Date(str),
        mnth = ("0" + (date.getMonth() + 1)).slice(-2),
        day = ("0" + date.getDate()).slice(-2);
    return [date.getFullYear(), mnth, day].join("-");
}

// New Comment Validation

function Validate_Comment_and_Save() {

    var MultipleCommenting = document.getElementById('chkAllSelectedordersComment').checked;

    var SalesOrderNo = $("#cmt_lblSalesOrder").val();
    var OrderOwner = $("#cmt_lblorderowner").val();
    var Materials = $("#cmb_Materials").val();

    var Sales_Org = $("#cmt_lblSalesOrg").val();
    var Sold_To_Customer = $("#cmt_Sold_To_Customer").val();
    var Order_Date = $("#cmt_lblorderDate").val();
    var Net_Value = $("#cmt_lblorderNetValue").val();

    var ReasonCode = $("#cmt_ReasonCode").val();
    var ReviewDate = $("#cmt_ReviewDate").val();
    var ApprovedToDate = $("#cmt_ApprovedToDate").val();
    var ClearDate = $("#cmt_ClearDate").val();
    var UserComments = $("#cmt_Comments").val();
    var ReportName = $("#Help_ReportName").val();

    if (Materials == undefined) Materials = "";
    if (ReviewDate == undefined) ReviewDate = "";


    if (SalesOrderNo == "" && MultipleCommenting == false) {
        alert("Sales Order Number is Mandatory to choose from above list of orders.");
        return false;
    }
    else if (UserComments == "") {
        alert("Comment is Mandatory to fill for Report - " + ReportName);
        return false;
    }
    else if (UserComments == undefined) {
        alert("Comment is Mandatory to fill for Report - " + ReportName);
        return false;
    }
    else if (UserComments == "Pending Comment") {
        alert("Comment is Mandatory to fill for Report - " + ReportName);
        return false;
    }
    else if (ReportName.substring(0, ReportName.indexOf("-") - 1) == "Free Of Charge Report" && (ReasonCode == "" || ReasonCode == null)) {
        alert("Reason Code is Mandatory to fill for Report - " + ReportName);
        return false;
    }
    else if (ReportName.indexOf("Key Control") >= 0 && ReportName.substring(0, ReportName.indexOf("-") - 1) != "Incomplete Orders") {
        if (ClearDate == null || ClearDate == "") {
            alert("Expected Release Date is Mandatory to fill for Report - " + ReportName);
            return false;
        }
        else if (ReasonCode == "" || ReasonCode == null) {
            alert("Reason Code is Mandatory to fill for Report - " + ReportName);
            return false;
        }
        else if (MultipleCommenting == true) {

            checked.length = 0;

            for (var i in checkedIds) {
                if (checkedIds[i]) {
                    checked.push(i);
                }
            }

            var confirmation = confirm("Are you sure to apply same comment to (" + checked.length + ") Multiple orders ?");

            if (confirmation == true) {
                SalesOrderNo = "";

                for (var i = 0; i < checked.length; i++) {
                    SalesOrderNo += checked[i].toString() + ",";
                }


                SalesOrderNo = SalesOrderNo.substring(0, SalesOrderNo.length - 1);
            }

        }
    }
    else if (MultipleCommenting == true) {

        checked.length = 0;

        for (var i in checkedIds) {
            if (checkedIds[i]) {
                checked.push(i);
            }
        }

        var confirmation = confirm("Are you sure to apply same comment to (" + checked.length + ") Multiple orders ?");

        if (confirmation == true) {
            SalesOrderNo = "";

            for (var i = 0; i < checked.length; i++) {
                SalesOrderNo += checked[i].toString() + ",";
            }


            SalesOrderNo = SalesOrderNo.substring(0, SalesOrderNo.length - 1);
        }

    }

    showProgress();

    ReportName = ReportName.substring(0, ReportName.indexOf("-") - 1);

    if (SalesOrderNo == "") {
        alert("Atleast One Sales Order Must be selected for commenting!");
    }
    else {
        $.ajax({
            type: 'POST',
            url: '../Generic/AddComment_Ajax/',
            dataType: 'json',
            data: {
                SalesOrder: SalesOrderNo,
                Materials: Materials.toString(),
                Sales_Org: Sales_Org,
                Sold_To_Customer: Sold_To_Customer,
                Order_Date: Order_Date,
                Net_Value: Net_Value,
                OrderOwner: OrderOwner,
                ReasonCode: ReasonCode,
                ReviewDate: ReviewDate,
                ApprovedToDate: ApprovedToDate,
                ClearDate: ClearDate,
                Comments: UserComments,
                ReportHelpDescr: ReportName
            },
            success: function (Result) {
                hideProgress();

                if (Result != "Success") {
                    alert("Error while saving Comment :" + Result);
                }
                else {
                    //alert("Comment Saved Successfully");
                    showCommentSuccess();
                    ReadOrderCommentsHistory(SalesOrderNo);

                    //Clear Commenting Space to perform Next order Comment
                    $("#cmb_Materials").val("");

                    if ($("#cmt_ReasonCode").data("kendoDropDownList") != null) {
                        $("#cmt_ReasonCode").data("kendoDropDownList").select(0);
                    }
                    $("#cmt_ReviewDate").val("");
                    $("#cmt_ClearDate").val("");
                    $("#cmt_ApprovedToDate").val("");


                    $("#cmt_Comments").val("");

                    var grid = $('#grdOrders').data().kendoGrid;

                    $("#grdOrders input").each(function () {
                        $(this).prop('checked', false);
                        selectRow(this);
                    });

                    // Clear All Pre-Selected Orders for Multiple Commenting
                    checkedIds = [];
                    document.getElementById('chkAllSelectedordersComment').checked = false;
                }

            },
            error: function (ex) {
                alert('Failed to retrieve states.' + ex);
            }
        });
    }

}

function RefreshOrdersData() {
    window.location.reload();
}

function Validate_Comment() {

    var ReportName = $("#Help_ReportName").val();
    var ClearDate = $("#cmt_ClearDate").val();
    var Comment = $("#cmt_Comments").val();
    var ReasonCode = $("#cmt_ReasonCode").val();


    if (Comment == "") {
        alert("Comment is Mandatory to fill for Report - " + ReportName);
        return false;
    }
    else if (ReportName.indexOf("Key Control") >= 0) {

        if (ClearDate == null || ClearDate == "") {
            alert("Clear Date is Mandatory to fill for Report - " + ReportName);
            return false;
        }
        else if (ReasonCode == "Not Applicable") {
            alert("Reason Code is Mandatory to fill for Report - " + ReportName);
            return false;
        }
        else
            return true;


    }
}


// Change Ownership Scripts

$("#rad_SelectedOrdersOwnership_").change(function () {
    var SelectedOrdersOwnership = $("#rad_SelectedOrdersOwnership_")[0].checked;
    var ExistingOwners = $("#cmb_ExistingOwner").data("kendoComboBox");

    if (SelectedOrdersOwnership == true) {
        ExistingOwners.enable(false);
        $("#lbl_OrdersSelected").removeAttr("disabled");
    }
});

function All_OrdersSelection() {
    var SelectedOrdersOwnership = $("#frmOwnership input[type='radio']:checked")[0].checked;
    var ExistingOwners = $("#cmb_ExistingOwner").data("kendoDropDownList");

    if (SelectedOrdersOwnership == true) {
        ExistingOwners.enable(true);
        $("#lbl_OrdersSelected").attr("disabled", "disabled");
    }
}

function Validate() {
    var ExistingOwner = $("#cmb_ExistingOwner").data("kendoDropDownList");
    var NewOwner = $("#cmb_NewOwner").data("kendoDropDownList");

    var TempOwnership = document.forms["frmOwnership"].Ownership_Type[0].checked;
    var PermanentOwnership = document.forms["frmOwnership"].Ownership_Type[1].checked;

    if (TempOwnership == false && PermanentOwnership == false) {
        alert("Either Temporary or Permanent ownership is needed.");
        return false;
    }
    else if (ExistingOwner._old == NewOwner._old) {
        alert("Exiting & New Owner can not be same. please change either New or Existing Owner name.");
        return false;
    }
    return true;
}

function ChangeOwnership() {

    checked.length = 0;

    for (var i in checkedIds) {
        if (checkedIds[i]) {
            checked.push(i);
        }
    }

    var result = confirm("Are you sure to Change Action Ownership for # " + checked.length + " Selected Orders ?");
    //alert("User Selected : " + result);

    if (result == true) {
        ReadyOwnershipChange();
    }

}

function ResetFiltersApplied() {
    $.ajax({
        type: 'POST',
        url: '../Generic/ResetSessionFiltersApplied/', // we are calling json method
        dataType: 'json',
        success: function (Manager) {
            RefreshOrdersData();
        },
        error: function (ex) {
            alert('Failed to retrieve states.' + ex);
        }
    });

}

$(document).ready(function () {

    var ChangeFollowupOwnershipWindow = $("#ChangeFollwupOwnership"),
        ChangeOwnershipWindow = $("#ChangeOwnershipWindow"),
        SessionFiltersWindow = $("#div_SessionFilters"),
            AssignOwnerships = $("#AssignOwnerships"),
            btnChangeOwnership = $("#btnChangeOwnership"),
            btn_SessionFilters = $("#btn_SessionFilters");

    AssignOwnerships.bind("click", function () {

        checked.length = 0;

        for (var i in checkedIds) {
            if (checkedIds[i]) {
                checked.push(i);
            }
        }

        $("#lbl_FollowupsSelected").val(checked.toString());
        ChangeFollowupOwnershipWindow.data("kendoWindow").open();
    });

    btnChangeOwnership.bind("click", function () {
        checked.length = 0;

        for (var i in checkedIds) {
            if (checkedIds[i]) {
                checked.push(i);
            }
        }

        $("#lbl_OrdersSelected").val(checked.toString());
        ChangeOwnershipWindow.data("kendoWindow").open();
    });

    btn_SessionFilters.bind("click", function () {
        SessionFiltersWindow.data("kendoWindow").open();
    });



    if (!ChangeOwnershipWindow.data("kendoWindow")) {
        ChangeOwnershipWindow.kendoWindow({
            width: "700px",
            actions: ["Close"],
            title: "Change Order Ownership",
            close: function () {
                // alert("Closed filters.");
            }
        });
    }

    if (!SessionFiltersWindow.data("kendoWindow")) {
        ChangeOwnershipWindow.kendoWindow({
            width: "700px",
            actions: ["Close"],
            title: "Change Order Ownership",
            close: function () {
                // alert("Closed filters.");
            }
        });
    }


    // begin grid binding


    $("#grdOrders").kendoGrid({
        dataSource: {
            type: "odata",
            transport: {
                read: {
                    url: "../DB/DB_AllOrders"
                }
            },
            schema: {
                model: {
                    fields: {
                        SALES_ORDERNO: { type: "string" },
                        CUSTOMER_PO_NO: { type: "string" },
                        SALES_ORG: { type: "string" },
                        ORDER_OWNER: { type: "string" },
                        ORDER_AGE: { type: "number" },
                        DB_AGING_BUCKET: { type: "string" },
                        BACKLOG_AMT: { type: "number" },
                        SOLD_TO_PARTY_NAME: { type: "string" },
                        FE_FE_DESC: { type: "string" },
                        ZU_ACCOUNT_NAME: { type: "string" },
                        ZU_COUNTRY: { type: "string" },
                        PAYMENT_TERMS: { type: "string" },
                        BILLING_BLOCK_CD: { type: "string" },
                        DELIVERY_BLK_HDR_CD: { type: "string" },
                        NLHD_STATUS: { type: "string" },
                        COMMIT_DATE: { type: "date" },
                        TRIO_LOAD_DATE: { type: "date" },
                        CUSTOMER_REQ_GI_DATE: { type: "date" },
                        INCOTERMS: { type: "string" },
                        DB_CLOSURE_STATUS: { type: "string" },
                        DELTA_LOAD_DATE_BUCKET: { type: "string" },
                        EXPECTED_RELEASE_DATE: { type: "date" },
                        REASON_CODE: { type: "string" },
                        LATEST_COMMENT: { type: "string" }
                    }
                }
            },
            pageSize: 20,
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true
        }
    });


    // end 




});



$(document).ready(function () {
    $("#tabstrip").kendoTabStrip({
        animation: {
            open: {
                effects: "fadeIn"
            }
        }
    });
});

// Error Handlers

function error_handler(e) {
    if (e.errors) {
        var message = "Errors:\n";
        $.each(e.errors, function (key, value) {
            if ('errors' in value) {
                $.each(value.errors, function () {
                    message += this + "\n";
                });
            }
        });
        alert(message);
    }
}

function onError(e, status) {
    if (e.errors) {
        var message = "Error:\n";

        var grid = $('#grdTeamUsers').data('kendoGrid');
        var gridElement = grid.editable.element;

        var validationMessageTemplate = kendo.template(
                "<div id='#=field#_validationMessage' " +
                    "class='k-widget k-tooltip k-tooltip-validation " +
                        "k-invalid-msg field-validation-error' " +
                    "style='margin: 0.5em;' data-for='#=field#' " +
                    "data-val-msg-for='#=field#' role='alert'>" +
                    "<span class='k-icon k-warning'></span>" +
                    "#=message#" +
                    "<div class='k-callout k-callout-n'></div>" +
                "</div>");

        $.each(e.errors, function (key, value) {
            if (value.errors) {
                gridElement.find("[data-valmsg-for=" + key + "],[data-val-msg-for=" + key + "]")
                        .replaceWith(validationMessageTemplate({ field: key, message: value.errors[0] }));
                gridElement.find("input[name=" + key + "]").focus();
            }
        });
        grid.one("dataBinding", function (e) {
            e.preventDefault();   // cancel grid rebind
        });
    }
}



// User Registration related

$(document).ready(function () {
    //Dropdownlist Selectedchange event
    $("#ddlTeam").change(function () {
        var aa = $("#ddlTeam").val();
        $.ajax({
            type: 'POST',
            url: '../Generic/GetmanagerName/', // we are calling json method
            dataType: 'json',
            data: { Team: aa },
            success: function (Manager) {
                var aa = Manager;
                var datalength = aa.length;
                if (datalength > 0) {
                    var item = aa[0];
                    $("#txtManager").val(item.MANAGER);
                    $("#txtmngemail").val(item.EMAIL);
                }
            },
            error: function (ex) {
                alert('Failed to retrieve states.' + ex);
            }
        });
        return false;
    })
});

$(document).ready(function () {
    //Dropdownlist Selectedchange event
    $("#ddlRegion").change(function () {
        $("#ddlCountry").empty();
        var RegionName = $("#ddlRegion").val();
        $.ajax({
            type: 'POST',
            url: '../Generic/GetCountryByRegion/', // we are calling json method
            dataType: 'json',
            data: { CountryName: RegionName },
            success: function (Countrys) {
                var aa = Countrys;
                var datalength = aa.length;
                for (i = 0; i < datalength; i++) {
                    var item = aa[i];

                    $("#ddlCountry").append('<option value="' + item + '">' + item + '</option>');
                }

            },
            error: function (ex) {
                alert('Failed to retrieve states.' + ex);
            }
        });
        return false;
    })
});

function validate_RegistrationForm() {
    var x = document.forms["frmRegistration"]["txtUsername"].value;
    if (x == null || x == "") {
        alert("user Name must be filled out");
        return false;
    }
    var x = document.forms["frmRegistration"]["txtFullname"].value;
    if (x == null || x == "") {
        alert("Fullname must be filled out");
        return false;
    }
    var x = document.forms["frmRegistration"]["txtEmail"].value;
    if (x == null || x == "") {
        alert("Email must be filled out");
        return false;
    }
    var x = document.forms["frmRegistration"]["txtManager"].value;
    if (x == null || x == "") {
        alert("manager name must be filled out");
        return false;
    }
    var x = document.forms["frmRegistration"]["ddlTeam"].value;
    if (x == "0") {
        alert("Please Select Team");
        return false;
    }
    var x = document.forms["frmRegistration"]["ddlRegion"].value;
    if (x == "0") {
        alert("Please Select Region");
        return false;

    }
    var x = document.forms["frmRegistration"]["ddlCountry"].value;
    if (x == "0") {
        alert("Please select country");
        return false;
    }

    var email = document.forms["frmRegistration"]["txtEmail"].value;
    var regex = /^\w+@@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/;

    if (!regex.test(email)) {
        alert("Enter a valid email");
        document.forms["frmRegistration"]["txtEmail"].focus();
        return false;
    }

}

function GetUserDeatailsByNTLogin(NTLogin) {

    $.ajax({
        cache: false,
        type: "POST",
        async: false,
        url: '../User/GetUserDetailsByNTLogin/',
        data: { NtLogin: NTLogin },
        datatype: "json",
        success: function (data) {
            var item = data;

            $("#txtFullName").val(item.FullName);
            $("#txtEmail").val(item.Email);

            $("#txtmanager").val(item.ManagerName);
        },
        statusCode: {
            404: function (content) { alert('cannot find resource'); },
            500: function (content) { alert('internal server error'); }
        },
        error: function (req, status, errorObj) {
        }
    });
}

function getCountryTeam(RegionName) {
    SelectTeam(RegionName);
}

function getManagerTeam(TeamName) {
    var ddlTeam = $('#ddlTeam').data("kendoDropDownList").text();
    GetManagerByTeam(ddlTeam);
}


function SelectTeam(RegionName) {
    $("#Team").empty();
    var ddl = $('#Team').data("kendoDropDownList");
    $.ajax({
        type: 'POST',
        url: '../Generic/GetTeamsByRegion/', // we are calling json method
        dataType: 'json',
        data: { RegionName: RegionName },
        success: function (Team) {
            ddl.setDataSource(Team);
        },
        error: function (ex) {
            alert('Failed to retrieve states.' + ex);
        }
    });
    return false;
}

function GetManagerByTeam(TeamName) {
    $.ajax({
        type: 'POST',
        url: '../User/GetmanagerName/', // we are calling json method
        dataType: 'json',
        data: { Team: TeamName },
        success: function (Managers) {
            if (Managers.length > 0) {
                $('#lblmanager')[0].innerText = Managers[0].MANAGER.toString();
            }
        },
        error: function (ex) {
            alert('Failed to retrieve states.' + ex);
        }
    });
    return false;
}

function GetManagerName() {
    var Team = $("#Team").data("kendoDropDownList").text();

    $.ajax({
        type: 'POST',
        url: '../User/GetmanagerName/', // we are calling json method
        dataType: 'json',
        data: { Team: Team },
        success: function (Managers) {
            if (Managers.length > 0) {

                var ddl = $('#Manager').data("kendoDropDownList");
                ddl.setDataSource(Managers);
            }
        },
        error: function (ex) {
            alert('Failed to retrieve states.' + ex);
        }
    });
    return false;
}

function SeachUserDetailsByNTLogin(NTLogin) {
    if (NTLogin == "") {
        alert("Please enter NTLogin");
        document.getElementById("nttlogin").focus();
        return false;
    }
    else {
        GetUserDeatailsByNTLogin(NTLogin);
    }

}

function MapUserRegistration() {
    if (document.getElementById('txtFullName').value == "") {
        alert("Full name can't be blank.");
        document.getElementById('txtFullName').focus();
        return false;
    }
    if (document.getElementById('nttlogin').value == "") {
        alert("NT Login can't be blank.");
        document.getElementById('nttlogin').focus();
        return false;
    }
    if (document.getElementById('txtEmail').value == "") {
        alert("Email can't be blank.");
        document.getElementById('txtEmail').focus();
        return false;
    }
    if (document.getElementById('ddlRole').value == "") {
        alert("Please select role.");
        return false;
    }
    if (document.getElementById('ddlRegion').value == "") {
        alert("Please select region.");
        return false;
    }
    if (document.getElementById('txtUsername').value == "") {
        alert("SAP name can't be blank.");
        return false;
    }
    if (document.getElementById('Team').value == "") {
        alert("Please select team.");
        return false;
    }

    var UserDetails =
        {
            "EMAIL": document.getElementById('txtEmail').value,
            "FULLNAME": document.getElementById('txtFullName').value,
            "NTLOGIN": document.getElementById('nttlogin').value,
            "ROLE_ID": document.getElementById('ddlRole').value,
            "SUPERREGION": document.getElementById('ddlRegion').value,
            "USERNAME": document.getElementById('txtUsername').value,
            "TEAM_ID": document.getElementById('Team').value,
            "COUNTRY": $("#Team").data("kendoDropDownList").text(),
            "PROFILE_PIC": "User_Profiles/default-profile-big.png"
        };
    $.ajax({
        url: '../User/SaveUserRegistrationNew/',
        type: 'POST',
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(UserDetails),
        dataType: "json",
        success: function (data) {
            alert("Data Saved Successfully ! Please refresh the page to get the updated data.");
            $("#Details").data("kendoWindow").close();

        }
    });
    return false;
}

function EditUser() {
    if (document.getElementById('txtFullName').value == "") {
        alert("Full name can't be blank.");
        document.getElementById('txtFullName').focus();
        return false;
    }
    if (document.getElementById('nttlogin').value == "") {
        alert("NT Login can't be blank.");
        document.getElementById('nttlogin').focus();
        return false;
    }
    if (document.getElementById('txtEmail').value == "") {
        alert("Email can't be blank.");
        document.getElementById('txtEmail').focus();
        return false;
    }
    if (document.getElementById('ddlRole').value == "") {
        alert("Please select role.");
        return false;
    }
    if (document.getElementById('ddlRegion').value == "") {
        alert("Please select region.");
        return false;
    }
    var roleid;
    var role = document.getElementById('ddlRole').value;
    if (role == "WW Lead") {
        roleid = "8";
    }
    if (role == "Regional User") {
        roleid = "9";
    }
    if (role == "Regional Lead") {
        roleid = "7";
    }
    if (role == "Supervisor") {
        roleid = "1";
    }
    if (role == "CSR") {
        roleid = "2";
    }
    if (role == "BPA") {
        roleid = "3";
    }
    if (role == "System") {
        roleid = "4";
    }
    if (role == "Admin") {
        roleid = "5";
    }



    var UserDetails =
        {
            "USER_ID": document.getElementById('hdnID').value,
            "EMAIL": document.getElementById('txtEmail').value,
            "FULLNAME": document.getElementById('txtFullName').value,
            "NTLOGIN": document.getElementById('nttlogin').value,
            "ROLE_ID": roleid,
            "SUPERREGION": document.getElementById('ddlRegion').value,
            "USERNAME": document.getElementById('txtSAPUSERNAME').value,
            "TEAM_ID": document.getElementById('ddlTeam').value,
            "PROFILE_PIC": document.getElementById('txtPROFILEPIC').value
        };
    $.ajax({
        url: '../User/SaveUserDetails/',
        type: 'POST',
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(UserDetails),
        dataType: "json",
        success: function (data) {

            $("#Details").data("kendoWindow").close();
            showUserUpdateSuccess();
            window.location.reload();
        }
    });
    return false;
}


function EditPersonalUser() {
    if (document.getElementById('Fullname').value == "") {
        alert("Full name can't be blank.");
        document.getElementById('Fullname').focus();
        return false;
    }

    if (document.getElementById('SAP_User_Name').value == "") {
        alert("SAP name can't be blank.");
        document.getElementById('SAP_User_Name').focus();
        return false;
    }

    if (document.getElementById('ntlogin').value == "") {
        alert("NT Login can't be blank.");
        document.getElementById('ntlogin').focus();
        return false;
    }
    if (document.getElementById('Email').value == "") {
        alert("Email can't be blank.");
        document.getElementById('Email').focus();
        return false;
    }
    if (document.getElementById('Role').value == "") {
        alert("Please select role.");
        return false;
    }

    if (document.getElementById('Team').value == "") {
        alert("Please select team.");
        return false;
    }
    var team = document.getElementById('Team').value;
    var role = document.getElementById('Role').value;
    var userid = document.getElementById('UserID').value;

    var UserDetails =
        {
            "USER_ID": userid,
            "EMAIL": document.getElementById('Email').value,
            "FULLNAME": document.getElementById('Fullname').value,
            "NTLOGIN": document.getElementById('ntlogin').value,
            "ROLE_ID": role,
            "SUPERREGION": "",
            "USERNAME": document.getElementById('SAP_User_Name').value,
            "TEAM_ID": document.getElementById('Team').value,
            "PROFILE_PIC": document.getElementById('User_Pic').value
        };
    $.ajax({
        url: '../User/SaveUserDetails/',
        type: 'POST',
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(UserDetails),
        dataType: "json",
        success: function (data) {
            alert("Data Saved Successfully ! Please refresh the page to get the updated data.");
            $("#Details").data("kendoWindow").close();

        }
    });
    return false;
}


// Level 1 & 2 Summary Scripts

LoadBCRViews = function () {
    $('#hidReportName').val("ControlReports");
    //alert($('#hidReportName').val().toString());
    $('#hidReportName').parents('form').submit();
    return false;
}


LoadFollowupViews = function () {
    $('#hidReportName').val("Followups");
    //alert($('#hidReportName').val().toString());
    $('#hidReportName').parents('form').submit();
    return false;
}

LoadDBViews = function () {
    $('#hidReportName').val("DBOrders");
    //alert($('#hidReportName').val().toString());
    $('#hidReportName').parents('form').submit();
    return false;
}

LoadBCRViews = function () {
    $('#hidReportName').val("ControlReports");
    //alert($('#hidReportName').val().toString());
    $('#hidReportName').parents('form').submit();
    return false;
}

LoadOSBRViews = function () {
    $('#hidReportName').val("OSBRReports");
    //alert($('#hidReportName').val().toString());
    $('#hidReportName').parents('form').submit();
    return false;
}

LoadSNIViews = function () {
    $('#hidReportName').val("SNIOrders");
    //alert($('#hidReportName').val().toString());
    $('#hidReportName').parents('form').submit();
    return false;
}

LoadUBViews = function () {
    $('#hidReportName').val("UnblockedOrders");
    //alert($('#hidReportName').val().toString());
    $('#hidReportName').parents('form').submit();
    return false;
}

LoadFollowupViews = function () {
    $('#hidReportName').val("Followups");
    //alert($('#hidReportName').val().toString());
    $('#hidReportName').parents('form').submit();
    return false;
}

function onMyCockpitClick(e) {
    $('#hidCockpitUI').val("CSR Cockpit");
    $('#hidReportName').val("ControlReports");
    //alert($('#hidReportName').val().toString());
    $('#hidReportName').parents('form').submit();
}

function onTeamCockpitClick(e) {
    $('#hidCockpitUI').val("Team Cockpit");
    $('#hidReportName').val("ControlReports");
    //alert($('#hidReportName').val().toString());
    $('#hidReportName').parents('form').submit();
}


// Search Order Methods

function SeachOrderDetails(SALES_ORDERNO) {
    if (isNaN(SALES_ORDERNO) || SALES_ORDERNO.indexOf(" ") != -1) {
        alert("Sales order must be numeric");
        return false;
    }
    else {
        GetCustomerNameBySalesOrderNumber(SALES_ORDERNO);
        GetOrderAmountBySalesOrderNumber(SALES_ORDERNO);
    }
}

function GetCustomerNameBySalesOrderNumber(SALES_ORDERNO) {

    $.ajax({
        cache: false,
        type: "POST",
        async: false,
        url: '../Generic/ReadOrderPartnerInfo/',
        data: { Sales_Order: SALES_ORDERNO },
        datatype: "json",
        success: function (data) {
            var target = $("#Followup_Customername");
            target.empty();
            target.val(null);
            target.prop('readonly', false);
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    var item = data[i];
                    if (item.SOLD_TO_PARTY_NAME != "") {
                        target.val(item.SOLD_TO_PARTY_NAME);
                        target.prop('readonly', true);
                    }
                    else {
                        target.val(null);
                        target.prop('readonly', false);
                    }
                }
            }
            else {
                alert("Sales order number not found!");
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

function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

function GetOrderAmountBySalesOrderNumber(SALES_ORDERNO) {
    $.ajax({
        cache: false,
        type: "POST",
        async: false,
        url: '../Generic/ReadOrderHeaderInfo/',
        data: { Sales_Order: SALES_ORDERNO },
        datatype: "json",
        success: function (data) {
            var editor1 = $("#Followup_Comments");
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                var backlogamt = item.BACKLOG_AMT;
                var backlogamtBody = "Net value (USD) : " + numberWithCommas(Math.round(item.BACKLOG_AMT));
                editor1.val(backlogamtBody);
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

function onTopSearchOrderClick() {
    var SO_Num = $('#Top_Search_Order').val();
    SearchOrderInDatabase(SO_Num);
}

function onSearchOrderClick() {
    var param_Sales_Order = $('#Search_Order').val();
    SearchOrderInDatabase(param_Sales_Order);
}

function SearchOrderInDatabase(OrderNumber) {
    if (OrderNumber == "") {
        alert("Sales Order Number is Needed for Search !");
    }
    else if (isNaN(OrderNumber) || OrderNumber.indexOf(" ") != -1) {
        alert("Sales order must be numeric");
    }
    else {
        window.location.href = '../Home/SearchOrder/?SalesOrder=' + OrderNumber;
    }
}

function SearchFullDetails() {
    var param_Sales_Order = $('#hid_OrderToSearch').val();



    search_ReadOrderHeaderDetails(param_Sales_Order);
    search_ReadOrderBlockDetails(param_Sales_Order);
    search_ReadOrderLineItem(param_Sales_Order);
    search_ReadOrderDeliveryDetails(param_Sales_Order);
    search_ReadOrderPartnerDetails(param_Sales_Order);
    search_ReadOrderCommentsHistory(param_Sales_Order);
}

function Search_Order_Global(param_Sales_Order) {
    $.ajax({
        type: 'POST',
        url: '../Generic/SearchGlobalOrder/', // we are calling json method
        dataType: 'json',
        data: { Sales_Order: param_Sales_Order },
        success: function (data) {
            var result = data;
            var datalength = result.length;

            $("#Search_Results").css("display", "block");
            $("#lblSearch_Order").val(param_Sales_Order);


            var target = $("#Order_Search_Result");
            target.empty();

            for (i = 0; i < datalength; i++) {
                var item = result[i];

                var html = "<tr><td>" + item.Sales_Order + "</td>";
                html += "<td>" + item.Found_in_Area + "</td>";
                html += "<td>" + item.Bucket_Security_On + "</td>";
                html += "<td>" + item.Current_Owner + "</td></tr>";

                target.append(html);
            }

        },
        error: function (ex) {
            alert('Failed to retrieve states.' + ex);
        }
    });
}

function ShowCommentDetails(Comments) {

    var grid = $("#grdOrders").data("kendoGrid");

    //Getting selected Item(s)
    var selectedItems = grid.dataItem(grid.select());
    alert(selectedItems.LATEST_COMMENT);
}

// Spinner Animation

function showProgress() {
    $("div#spinner").show("fast");
    $("div#spinner").css("visibility", "visible");
    $("div#spinner").css("z-index", "99999");
}

function hideProgress() {
    $("div#spinner").hide("fast");
    $("div#spinner").css("visibility", "hidden");
}

function showCommentSuccess() {
    $("div#div_Comment_Success").show("fast");
    $("div#div_Comment_Success").css("visibility", "visible");
    $("div#div_Comment_Success").css("z-index", "99999");

    $("div#div_Comment_Success").hide('blind', {}, 5000);
}

function hideCommentSuccess() {
    $("div#div_Comment_Success").show("fast");
    $("div#div_Comment_Success").css("visibility", "hidden");
}

function showSignOffSuccess(count) {
    $("div#div_signOff_Success").show("fast");
    $("div#div_signOff_Success").css("visibility", "visible");
    $("div#div_signOff_Success").css("z-index", "99999");

    $("#lbl_SignOff_Confirmation").val(count + " Order(s) Comments has been signed off Successfully.");
    $("div#div_signOff_Success").hide('blind', {}, 5000);
}

function showUserUpdateSuccess(count) {
    $("div#div_UserUpdate_Success").show("fast");
    $("div#div_UserUpdate_Success").css("visibility", "visible");
    $("div#div_UserUpdate_Success").css("z-index", "99999");

    $("div#div_UserUpdate_Success").hide('blind', {}, 5000);
}


function Set_OrderCommentHistory(data) {
    if (data.length > 0) {

        var target = $("#commentsHistory");
        target.empty();
        AllMaterials.length = 0;
        LatestComment = null;

        for (var i = 0; i < data.length; i++) {
            var item = data[i];

            item.Reviewdate = formatJSONDateFromNumericdate(item.Reviewdate);
            item.Cleardate = formatJSONDateFromNumericdate(item.Cleardate);
            item.Comment_Date = formatJSONDateFromNumericdate(item.Comment_Date);

            AllMaterials.push({
                Material: item.Material
            });
        }

        var ms = $("#grdOrderHistory").data('kendoGrid');
        ms.dataSource.data(data);

        //Calculate Unique Materials of Order
        UniqueMaterials.length = 0;
        GetUnique(AllMaterials);

        //Update pre-selected Material list for enabling new commenting
        $("#cmb_Materials").find("option").remove();

        $.each(UniqueMaterials, function (key, value) {
            $('#cmb_Materials').append($('<option selected=selected>', { value: value }).text(value));
        });

    }
}

function Set_OrderItemDetails(data) {
    if (data.length > 0) {

        for (var i = 0; i < data.length; i++) {
            var item = data[i];

            item.CUSTOMER_REQ_GI_DATE = formatJSONDateFromNumericdate(item.CUSTOMER_REQ_GI_DATE);
            item.TRIO_LOAD_DATE = formatJSONDateFromNumericdate(item.TRIO_LOAD_DATE);
            item.COMMIT_DATE = formatJSONDateFromNumericdate(item.COMMIT_DATE);

        }

        var ms = $("#grdOrderLineItems").data('kendoGrid');
        ms.dataSource.data(data);

    }
}

function ReadOrderTabsInfo(SALES_ORDERNO) {

    $.ajax({
        cache: false,
        type: "POST",
        async: false,
        url: '../Generic/ReadOrderTabsInfo/',
        data: { Sales_Order: SALES_ORDERNO },
        datatype: "json",
        success: function (data) {
            if (data.Header != null) {
                Set_OrderHeaderDetails(data.Header[0]);
                Set_OrderItemDetails(data.Items)
                Set_DeliveryInfo(data.Delivery[0]);
                Set_BlockedDetails(data.Block[0]);
                Set_PartnerInfo(data.Partner[0]);
                Set_OrderCommentHistory(data.Comments);
                Set_Order_OpenFollowup(data.OpenFollowup);
                Set_Order_CloseFollowup(data.ClosedFollowup);
            }
            else
                alert("No Header Information is available for this order.This order may be Invoiced/Closed.");

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

var Followup;

function Set_Order_OpenFollowup(data) {

    $("#lblOpenFollowups").text(data.length.toString());

    for (var i = 0; i < data.length; i++) {
        var item = data[i];

        if (item.Modified_By == null)
            item.Modified_By = "";

        if (item.Modified_On == undefined)
            item.Modified_On = "";

        item.DueDate = formatJSONDateFromNumericdate(item.DueDate);
        item.Created_On = formatJSONDateFromNumericdate(item.Created_On);
        item.Modified_On = formatJSONDateFromNumericdate(item.Modified_On);

    }

    var ms = $("#grdOrderOpenFollowups").data('kendoGrid');
    ms.dataSource.data(data);

}

function Set_Order_CloseFollowup(data) {
    var target = $("#ClosedFollowup");
    target.empty();
    AllMaterials.length = 0;
    LatestComment = null;

    $("#lblClosedFollowups").text(data.length.toString());

    for (var i = 0; i < data.length; i++) {
        var item = data[i];

        item.DueDate = formatJSONDateFromNumericdate(item.DueDate);
        item.Created_On = formatJSONDateFromNumericdate(item.Created_On);
        item.Modified_On = formatJSONDateFromNumericdate(item.Modified_On);

    }

    var ms = $("#grdOrderClosedFollowups").data('kendoGrid');
    ms.dataSource.data(data);
}

function ShowFilters() {
    var SessionFiltersWindow = $("#div_SessionFilters");
    SessionFiltersWindow.data("kendoWindow").open();
}

// Advanced Filters
$(document).ready(function () {
    var AdvancedsearchWindow = $("#AdvancedsearchWindow"),
            btnAdvancedSearch = $("#btnAdvancedSearch");

    var PieChartWindow = $("#PieChartWindow"),
            btnPieChart = $("#btnPieChart");

    var WaterfallChartWindow = $("#WaterfallChartWindow"),
            btnWaterfallChart = $("#btnWaterfallChart");


    btnAdvancedSearch.bind("click", function () {
        AdvancedsearchWindow.data("kendoWindow").open();
        _LoadAdvancedFilters();
    });

    btnPieChart.bind("click", function () {
        $("#hid_CurrentChart").val("Piechart");

        _LoadPieChart();

    });

    btnWaterfallChart.bind("click", function () {
        $("#hid_CurrentChart").val("Waterfallchart");
        _LoadWaterfallChart();

    });


    if (!AdvancedsearchWindow.data("kendoWindow")) {
        AdvancedsearchWindow.kendoWindow({
            width: "500px",
            actions: ["Close"],
            title: "Advanced Filters",
            close: function () {
                // alert("Closed filters.");
            }
        });
    }

});

function RegionFilterChanged(e) {
    var Region = e.sender._old;
    var UserSecuredRegion = $("#hid_UserRegion").val();

    if (Region == "WW") Region = "";

    showProgress();

    GetAdvancedFilterValues(Region);
}

function _LoadWaterfallChart() {

    showProgress();

    var GridOrders = $("#grdOrders").data("kendoGrid").dataSource;

    if (GridOrders._total != GridOrders.data().length) {
        var data = GridOrders._view;
    }
    else {
        var data = GridOrders.data();
    }
    
    gridDataSource = GridOrders;

    if (data.length > 0) {
        var totalRows = data.length;

        var TotalBacklog = 0;
        var ReleaseCurrentMonth = 0;
        var ToBeCommented = 0;
        var ReleasePriorCurrentMonth = 0;
        var NoExpectedReleaseDate = 0;
        var FutureRelease = 0;
        var FutureMonth1 = 0;
        var FutureMonth2 = 0;
        var FutureMonth3 = 0;
        var FutureMonth_Far = 0;

        var LastDayPrevMonth = new Date(); // current date
        LastDayPrevMonth.setDate(1); // going to 1st of the month
        LastDayPrevMonth.setHours(-1); // going to last hour before this date even started.

        var FirstDayCurrMonth = new Date();
        FirstDayCurrMonth.setDate(1);

        var date = new Date();
        var FirstDatNextMonth = date.setMonth(date.getMonth() + 1, 1);

        for (var i = 0; i < totalRows; i++) {
            var currentDataItem = data[i];
            // Chart Data modeling

            TotalBacklog += currentDataItem.BACKLOG_AMT;

            var TodaysDate = new Date();
            if (currentDataItem.EXPECTED_RELEASE_DATE != null && currentDataItem.EXPECTED_RELEASE_DATE.getYear() > 0) {
                var ReleaseDate = new Date(currentDataItem.EXPECTED_RELEASE_DATE);

                //Release Prior CurrentMonth
                if (ReleaseDate < LastDayPrevMonth) {
                    ReleasePriorCurrentMonth += currentDataItem.BACKLOG_AMT;
                    //alert("Sales Order : " + currentDataItem.SALES_ORDERNO + ' : Prev. Month ' + ReleaseDate.toString());

                } //Release CurrentMonth
                else if (ReleaseDate >= FirstDayCurrMonth && ReleaseDate < FirstDatNextMonth) {
                    ReleaseCurrentMonth += currentDataItem.BACKLOG_AMT;
                    //alert("Sales Order : " + currentDataItem.SALES_ORDERNO + ' : Current Month ' + ReleaseDate.toString());
                } //Future Release
                else if (ReleaseDate >= FirstDayCurrMonth) {

                    if (ReleaseDate.getMonth() == (TodaysDate.getMonth() + 1)) {
                        FutureMonth1 += currentDataItem.BACKLOG_AMT;
                        //alert("Sales Order : " + currentDataItem.SALES_ORDERNO + ' : Future 1 ');
                    }
                    else if (ReleaseDate.getMonth() == (TodaysDate.getMonth() + 2)) {
                        FutureMonth2 += currentDataItem.BACKLOG_AMT;
                        //alert("Sales Order : " + currentDataItem.SALES_ORDERNO + ' : Future 2 ');
                    }
                    else if (ReleaseDate.getMonth() == (TodaysDate.getMonth() + 3)) {
                        FutureMonth3 += currentDataItem.BACKLOG_AMT;
                        //alert("Sales Order : " + currentDataItem.SALES_ORDERNO + ' : Future 3 ');
                    }
                    else {
                        FutureMonth_Far += currentDataItem.BACKLOG_AMT;
                        //alert("Sales Order : " + currentDataItem.SALES_ORDERNO + ' : Future Far ');
                    }
                }
            }
            else {
                //Release TBC
                ToBeCommented += currentDataItem.BACKLOG_AMT;
                //alert("Sales Order : " + currentDataItem.SALES_ORDERNO + ' : TBC ');
            }
        }

        ToBeCommented = ToBeCommented + ReleasePriorCurrentMonth;

        FutureRelease = FutureMonth1 + FutureMonth2 + FutureMonth3 + FutureMonth_Far;
        NoExpectedReleaseDate = ToBeCommented - ReleasePriorCurrentMonth;

        var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
        var d = new Date();
        var month = d.getMonth();

        var WaterfallData = [
        {
            period: "Backlog",
            amount: TotalBacklog
        }, {
            period: "Current Month",
            amount: ReleaseCurrentMonth * -1
        }, {
            period: "TBC",
            amount: ToBeCommented * -1
        }, {
            period: "Past Months",
            amount: ReleasePriorCurrentMonth
        }, {
            period: "No Exp. Rel.",
            amount: NoExpectedReleaseDate
        }, {
            period: "Future",
            amount: FutureRelease *-1
        }, {
            period: monthNames[d.getMonth() + 1],
            amount: FutureMonth1
        }, {
            period: monthNames[d.getMonth() + 2],
            amount: FutureMonth2
        }, {
            period: monthNames[d.getMonth() + 3],
            amount: FutureMonth3
        }];

        $("#WaterfallChart").kendoChart({
            title: {
                text: "Expected Release Date Projection"
            },
            legend: {
                visible: false
            },
            Tooltip: {
                visible: true,
                format: "$ ##,#",
                template: "#= FormatLongNumber(value) #"
            },
            dataSource: {
                data: WaterfallData
            },
            valueAxis: [{
                minorGridLines: {
                    visible: true,
                    dashType: "longDash"
                },
                labels: {
                    template: "#= FormatLongNumber(value) #"
                }
            }],
            series: [{
                type: "waterfall",
                field: "amount",
                categoryField: "period",
                summaryField: "summary",
                color: function (point) {
                    if (point.category == "Backlog") {
                        return "blue";
                    } else if (point.category == "Past Months") {
                        return "gray";
                    }
                    else if (point.category == "TBC") {
                        return "orange";
                    }
                    else if (point.category == "Future") {
                        return "blue";
                    }
                    else if (point.category == "Current Month") {
                        return "green";
                    }
                    else // Future Month
                    {
                        return "gray";
                    }
                },
                width: 2,
                labels: {
                    visible: true,
                    format: "C0",
                    position: "insideEnd",
                    template: "#= FormatLongNumber(value) #"
                }
            }]
        });
    }



    document.getElementById("PieChart").style.visibility = "hidden";
    document.getElementById("PieChart").style.height = "0px";


    document.getElementById("WaterfallChart").style.visibility = "visible";
    document.getElementById("WaterfallChart").style.height = "400px";

    $("#WaterfallChart").data("kendoChart").resize(true);

    hideProgress();

}

function _LoadPieChart() {

    showProgress();

    var GridOrders = $("#grdOrders").data("kendoGrid").dataSource;

    if (GridOrders._total != GridOrders.data().length) {
        var data = GridOrders._view;
    }
    else {
        var data = GridOrders.data();
    }

    gridDataSource = GridOrders;

    if (gridDataSource._total > 0) {

        var totalRows = data.length;

        AllPieRows.length = 0;

        var Reason = '';

        for (var i = 0; i < totalRows; i++) {
            var currentDataItem = data[i];

            if (currentDataItem.REASON_CODE == null)
            { Reason = '(Blank)'; }
            else
            { Reason = currentDataItem.REASON_CODE; }

            AllPieRows.push({
                REASON_CODE: Reason,
                BACKLOG_AMT: currentDataItem.BACKLOG_AMT
            });
        }


        var AggregatedData = new kendo.data.DataSource({
            data: AllPieRows,
            schema: {
                model: {
                    fields: {
                        REASON_CODE: { type: "string" },
                        BACKLOG_AMT: { type: "number" }
                    }
                }
            },
            group: {
                field: "REASON_CODE",
                aggregates: [{ field: "BACKLOG_AMT", aggregate: "sum"}]
            }
        });

        AggregatedData.fetch();

        //gather into arrays (named)
        var dataObjs = AggregatedData.view(),
                        recCount = dataObjs.length,
                        dataObj,
                        myData = [];

        for (var i = 0; i < recCount; i++) {
            dataObj = dataObjs[i];
            myData.push({
                value: dataObj.aggregates.BACKLOG_AMT.sum / 1000,
                category: dataObj.value
            });
        };


        $("#PieChart").kendoChart({
            title: {
                text: "Reasons Distribution"
            },
            dataSource: {
                data: myData
            },
            seriesDefaults: {
                labels: {
                    template: "#= category # - K#= kendo.format('{0:C0}', value)#",
                    position: "outsideEnd",
                    visible: true,
                    background: "transparent"
                }
            },
            series: [{
                type: "pie",
                field: "value",
                categoryField: "category"
            }],
            seriesColors: ["#03a9f4", "#ff9800", "#fad84a", "#4caf50", "#33a9f4", "#bb9800", "#ccd84a", "#5daf50", "#BC79FF", "#A80054"],
            tooltip: {
                visible: true,
                template: "#= category # - #= kendo.format('{0:p}', percentage)#"
            }
        });


    }


    if (document.getElementById("PieChart").style.visibility == "hidden") {
        document.getElementById("PieChart").style.visibility = "visible";
        document.getElementById("PieChart").style.height = "400px";
    }

    document.getElementById("WaterfallChart").style.visibility = "hidden";
    document.getElementById("WaterfallChart").style.height = "0px";

    $("#PieChart").data("kendoChart").resize(true);

    hideProgress();
}


function _LoadAdvancedFilters() {
    var Region = $("#fltr_Region").val();

    if (Region == "WW") Region = "";

    showProgress();

    GetAdvancedFilterValues(Region);
}

function GetAdvancedFilterValues(Region) {

    var Regions = "";
    for (var i = 0; i < Region.length; i++) {
        Regions += Region[i] + ",";
    }

    $.ajax({
        cache: false,
        type: "POST",
        async: false,
        url: '../Generic/GetFilterValues/',
        data: { RegionFilter: Regions },
        datatype: "json",
        success: function (data) {
            hideProgress();

            if (data[0].length > 1) {
                var ms = $("#fltr_BTM").data('kendoMultiSelect');
                ms.setDataSource(data[0]);
            }

            if (data[1].length > 1) {
                var ms = $("#fltr_BTMManager").data('kendoMultiSelect');
                ms.setDataSource(data[1]);
            }

            if (data[2].length > 1) {
                var ms = $("#fltr_ShiptoCountry").data('kendoMultiSelect');
                ms.setDataSource(data[2]);
            }

            if (data[3].length > 1) {
                var ms = $("#fltr_SoldToCountry").data('kendoMultiSelect');
                ms.setDataSource(data[3]);
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


function resizeGrid() {
    var ModifiedHeight;

    var gridElement = $("#grdOrders"),
        dataArea = gridElement.find(".k-grid-content"),
        gridHeight = gridElement.innerHeight(),
        ModifiedHeight = gridHeight * .7;
    otherElements = gridElement.children().not(".k-grid-content"),
        otherElementsHeight = 0;
    otherElements.each(function () {
        otherElementsHeight += $(this).outerHeight();
    });
    dataArea.height(ModifiedHeight);
}


// Summary Bucket Change
function onSummaryBucketSelection(e) {
    var dataItem = e.sender._old;

    var Bucketname;

    if (dataItem == "Summary by Aging Bucket")
        Bucketname = "AgingBucket";
    else
        Bucketname = "DollarBucket";

    showProgress();
    $.ajax(
    {
        type: 'POST',
        url: '../Generic/GetSummaryBucket/',
        dataType: 'json',
        data: { Area: $("#hid_BucketArea").val(), BucketName: Bucketname },
        success: function (result) {
            hideProgress();
            if (result.length > 0) {
                $("#grdSummaryOrders").data("kendoGrid").dataSource.data(result);
            }
        }
    });
}


// UTC Time Zone settings

var offsetMiliseconds = new Date().getTimezoneOffset() * 60000;

function onRequestEnd(e) {
    if (e.response != undefined && (e.response.Data && e.response.Data.length)) {
        var SalesOrders = e.response.Data;
        addOffset(SalesOrders);
    }
}


function addOffset(SalesOrders) {
    for (var i = 0; i < SalesOrders.length; i++) {

        SalesOrders[i].DELIVERY_BLOCK_CUT_OFF_DATE = SalesOrders[i].DELIVERY_BLOCK_CUT_OFF_DATE.replace(/\d+/,
            function (n) { return parseInt(n) + offsetMiliseconds }
        );

        SalesOrders[i].SHIPMENT_CUT_OFF_DATE = SalesOrders[i].SHIPMENT_CUT_OFF_DATE.replace(/\d+/,
            function (n) { return parseInt(n) + offsetMiliseconds }
        );

        SalesOrders[i].ReqDlyDate = SalesOrders[i].ReqDlyDate.replace(/\d+/,
            function (n) { return parseInt(n) + offsetMiliseconds }
        );

        SalesOrders[i].EXPECTED_RELEASE_DATE = SalesOrders[i].EXPECTED_RELEASE_DATE.replace(/\d+/,
            function (n) { return parseInt(n) + offsetMiliseconds }
        );
    }
}

function ActivateDeactivateUser(userid, email, fullname) {
    $.ajax({
        url: "../User/ActivateUser",
        type: "POST",
        data: JSON.stringify({ 'UserID': userid, 'email': email, 'fullname': fullname }),
        dataType: "json",
        traditional: true,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("#grdTeamUsers").data("kendoGrid").dataSource.data(data);
        },
        error: function () {
            alert("An error has occured!!!");
        }
    });
}

function FormatLongNumber(value) {
    if (value == 0) {
        return 0;
    }
    else {

        value = Math.abs(value);

        // hundreds
        if (value <= 999) {
            return Math.floor(value);
        }

        // thousands
        //else if (value >= 1000 && value <= 999999) {
        else if (value >= 1000) {
            return 'K$ ' + Math.floor((value / 1000)).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        }
    }
}

// ---------------- End CNG Scripts ------------------------------//