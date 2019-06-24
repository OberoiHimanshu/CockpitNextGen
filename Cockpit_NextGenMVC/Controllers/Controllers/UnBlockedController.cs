using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Mvc;
using Cockpit_NextGenMVC.BAL;
using Cockpit_NextGenMVC.Models;

using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace Cockpit_NextGenMVC.Controllers
{
    [SessionExpire]
    public class UnBlockedController : Controller
    {
        static BAL.Service1Client service = new BAL.Service1Client();
        VW_USERS oSessionUser;
        string CockpitUI;
        public List<VW_Orders_Info> lst_UBOrderd;
        public List<Tbl_Review_Reports> lstReportsMaster;
        public Tbl_Review_Reports oCurrentReport;
        public List<Tbl_WW_Blocked_Orders_Summary> lst_Summary_AgingBucket, lst_Summary_DollarBucket;
        public DashboardModal oDashboardModel;

        public ActionResult UB_Summary()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            oDashboardModel = (DashboardModal)Session["oDashboardModal"];
            CockpitUI = oDashboardModel.UI_Type;

            ViewBag.UserRole = oSessionUser.ROLE_DESC;
            ViewBag.ThisReport = "All Unblocked Orders";

            lst_Summary_AgingBucket = service.WW_Process_Summary("Unblocked", "DollarBucket").ToList();

            ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);

            ViewBag.BucketArea = "Unblocked";
            ViewBag.BucketName = "DollarBucket";

            return View(lst_Summary_AgingBucket);
        }

        public ActionResult AllUnblocked()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            oDashboardModel = (DashboardModal)Session["oDashboardModal"];
            CockpitUI = oDashboardModel.UI_Type;
            ViewBag.UserRole = oSessionUser.ROLE_DESC;
            ViewBag.ThisReport = "All Unblocked Orders";

            if (oSessionUser.ROLE_DESC != "WW Lead")
            {
                oDashboardModel.oSessionFilters.Region = oSessionUser.SUPERREGION;
                oDashboardModel.oSessionFilters.OrderOwner = oSessionUser.FULLNAME;
                oDashboardModel.oSessionFilters.CockpitUI = oDashboardModel.UI_Type;
            }
            else
            {
                oDashboardModel.oSessionFilters.Region = "";
                oDashboardModel.oSessionFilters.OrderOwner = oSessionUser.FULLNAME;
                oDashboardModel.oSessionFilters.CockpitUI = oDashboardModel.UI_Type;
            }

            lst_UBOrderd = service.Get_Unblocked_All(oDashboardModel.oSessionFilters.Business, oDashboardModel.oSessionFilters.Division, oDashboardModel.oSessionFilters.PrimaryProduct, oDashboardModel.oSessionFilters.Region, oDashboardModel.oSessionFilters.Sorg, "", oSessionUser.FULLNAME,
                                                          oDashboardModel.oSessionFilters.SNIClosureStatus, oDashboardModel.oSessionFilters.DBClosureStatus, oDashboardModel.oSessionFilters.Aging,
                                                          oDashboardModel.oSessionFilters.SNIAging, oDashboardModel.oSessionFilters.AgingBucket, oDashboardModel.oSessionFilters.SNIAgingBucket,
                                                          oDashboardModel.oSessionFilters.DeltaLoaddate, oDashboardModel.oSessionFilters.DeltaLoaddateBucket, oDashboardModel.oSessionFilters.DollarBucket,
                                                          oDashboardModel.oSessionFilters.BacklogStatus, oDashboardModel.oSessionFilters.CustomerPONumber, oDashboardModel.oSessionFilters.SoldToAccountID,
                                                          oDashboardModel.oSessionFilters.ShipToAccountID, oDashboardModel.oSessionFilters.SoldToAccount, oDashboardModel.oSessionFilters.ShipToAccount,
                                                          oDashboardModel.oSessionFilters.ZUAccount, oDashboardModel.oSessionFilters.PL, oDashboardModel.oSessionFilters.SoldToCountry, oDashboardModel.oSessionFilters.ShipToCountry,
                                                          oDashboardModel.oSessionFilters.ZUAccountID, oDashboardModel.oSessionFilters.ZUCountry, oDashboardModel.oSessionFilters.BillingBlock_Code, oDashboardModel.oSessionFilters.DeliveryBlock_Code,
                                                          oDashboardModel.oSessionFilters.BillingBlock_HeaderText, oDashboardModel.oSessionFilters.DeliveryBlock_HeaderText, oDashboardModel.oSessionFilters.BillingBlock_ItemText, oDashboardModel.oSessionFilters.DeliveryBlock_ItemText,
                                                          oDashboardModel.oSessionFilters.PaymentTerm, oDashboardModel.oSessionFilters.SalesRep, oDashboardModel.oSessionFilters.BTM, oDashboardModel.oSessionFilters.BTM_Manager,
                                                          oDashboardModel.oSessionFilters.ClosuredaysDeltaFrom, oDashboardModel.oSessionFilters.ClosuredaysDeltaTo,
                                                          "", CockpitUI).ToList();

            lstReportsMaster = service.Get_Reports_Master().ToList();
            oCurrentReport = lstReportsMaster.Where(p => p.ReportName == "Unblocked Orders").FirstOrDefault();


            Session["oCurrentReport"] = "Unblocked All";
            ViewBag.oCurrentReport = oCurrentReport;


            ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
            return View(lst_UBOrderd);

        }

        public ActionResult AllUnblocked_via_Summary(string Region, string Bucket, string Bucket_Type)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            oDashboardModel = (DashboardModal)Session["oDashboardModal"];

            CockpitUI = oDashboardModel.UI_Type;
            ViewBag.UserRole = oSessionUser.ROLE_DESC;
            ViewBag.ThisReport = "All Unblocked Orders";

            lstReportsMaster = service.Get_Reports_Master().ToList();
            oCurrentReport = lstReportsMaster.Where(p => p.ReportName == "Unblocked Orders").FirstOrDefault();

            Session["oCurrentReport"] = "Unblocked All";
            ViewBag.oCurrentReport = oCurrentReport;

            if (Region == "WW") Region = "";
            oDashboardModel.oSessionFilters.Region = Region;
            oDashboardModel.oSessionFilters.OrderOwner = oSessionUser.FULLNAME;
            oDashboardModel.oSessionFilters.CockpitUI = oDashboardModel.UI_Type;

            VW_Orders_Info[] lst_UBOrderd;

            if (Bucket_Type == "AgingBucket")
            {
                lst_UBOrderd = service.Get_Unblocked_All(oDashboardModel.oSessionFilters.Business, oDashboardModel.oSessionFilters.Division, oDashboardModel.oSessionFilters.PrimaryProduct, oDashboardModel.oSessionFilters.Region, oDashboardModel.oSessionFilters.Sorg, "", oSessionUser.FULLNAME,
                                                          oDashboardModel.oSessionFilters.SNIClosureStatus, oDashboardModel.oSessionFilters.DBClosureStatus, oDashboardModel.oSessionFilters.Aging,
                                                          oDashboardModel.oSessionFilters.SNIAging, Bucket, oDashboardModel.oSessionFilters.SNIAgingBucket,
                                                          oDashboardModel.oSessionFilters.DeltaLoaddate, oDashboardModel.oSessionFilters.DeltaLoaddateBucket, oDashboardModel.oSessionFilters.DollarBucket,
                                                          oDashboardModel.oSessionFilters.BacklogStatus, oDashboardModel.oSessionFilters.CustomerPONumber, oDashboardModel.oSessionFilters.SoldToAccountID,
                                                          oDashboardModel.oSessionFilters.ShipToAccountID, oDashboardModel.oSessionFilters.SoldToAccount, oDashboardModel.oSessionFilters.ShipToAccount,
                                                          oDashboardModel.oSessionFilters.ZUAccount, oDashboardModel.oSessionFilters.PL, oDashboardModel.oSessionFilters.SoldToCountry, oDashboardModel.oSessionFilters.ShipToCountry,
                                                          oDashboardModel.oSessionFilters.ZUAccountID, oDashboardModel.oSessionFilters.ZUCountry, oDashboardModel.oSessionFilters.BillingBlock_Code, oDashboardModel.oSessionFilters.DeliveryBlock_Code,
                                                          oDashboardModel.oSessionFilters.BillingBlock_HeaderText, oDashboardModel.oSessionFilters.DeliveryBlock_HeaderText, oDashboardModel.oSessionFilters.BillingBlock_ItemText, oDashboardModel.oSessionFilters.DeliveryBlock_ItemText,
                                                          oDashboardModel.oSessionFilters.PaymentTerm, oDashboardModel.oSessionFilters.SalesRep, oDashboardModel.oSessionFilters.BTM, oDashboardModel.oSessionFilters.BTM_Manager,
                                                          oDashboardModel.oSessionFilters.ClosuredaysDeltaFrom, oDashboardModel.oSessionFilters.ClosuredaysDeltaTo,
                                                          "", CockpitUI);
                ViewData.Add("lst_UBOrderd", lst_UBOrderd);
                ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
                ViewBag.CurrentSelectedRegion = Region;

                return View(lst_UBOrderd);
            }
            else if (Bucket_Type == "DollarBucket")
            {
                lst_UBOrderd = service.Get_Unblocked_All(oDashboardModel.oSessionFilters.Business, oDashboardModel.oSessionFilters.Division, oDashboardModel.oSessionFilters.PrimaryProduct, oDashboardModel.oSessionFilters.Region, oDashboardModel.oSessionFilters.Sorg, "", oSessionUser.FULLNAME,
                                                          oDashboardModel.oSessionFilters.SNIClosureStatus, oDashboardModel.oSessionFilters.DBClosureStatus, oDashboardModel.oSessionFilters.Aging,
                                                          oDashboardModel.oSessionFilters.SNIAging, oDashboardModel.oSessionFilters.AgingBucket, oDashboardModel.oSessionFilters.SNIAgingBucket,
                                                          oDashboardModel.oSessionFilters.DeltaLoaddate, oDashboardModel.oSessionFilters.DeltaLoaddateBucket, Bucket,
                                                          oDashboardModel.oSessionFilters.BacklogStatus, oDashboardModel.oSessionFilters.CustomerPONumber, oDashboardModel.oSessionFilters.SoldToAccountID,
                                                          oDashboardModel.oSessionFilters.ShipToAccountID, oDashboardModel.oSessionFilters.SoldToAccount, oDashboardModel.oSessionFilters.ShipToAccount,
                                                          oDashboardModel.oSessionFilters.ZUAccount, oDashboardModel.oSessionFilters.PL, oDashboardModel.oSessionFilters.SoldToCountry, oDashboardModel.oSessionFilters.ShipToCountry,
                                                          oDashboardModel.oSessionFilters.ZUAccountID, oDashboardModel.oSessionFilters.ZUCountry, oDashboardModel.oSessionFilters.BillingBlock_Code, oDashboardModel.oSessionFilters.DeliveryBlock_Code,
                                                          oDashboardModel.oSessionFilters.BillingBlock_HeaderText, oDashboardModel.oSessionFilters.DeliveryBlock_HeaderText, oDashboardModel.oSessionFilters.BillingBlock_ItemText, oDashboardModel.oSessionFilters.DeliveryBlock_ItemText,
                                                          oDashboardModel.oSessionFilters.PaymentTerm, oDashboardModel.oSessionFilters.SalesRep, oDashboardModel.oSessionFilters.BTM, oDashboardModel.oSessionFilters.BTM_Manager,
                                                          oDashboardModel.oSessionFilters.ClosuredaysDeltaFrom, oDashboardModel.oSessionFilters.ClosuredaysDeltaTo,
                                                          "SNI_MF", CockpitUI);
                ViewData.Add("lst_UBOrderd", lst_UBOrderd);
                ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
                ViewBag.CurrentSelectedRegion = Region;

                return View(lst_UBOrderd);
            }
            else
            {
                lst_UBOrderd = service.Get_Unblocked_All(oDashboardModel.oSessionFilters.Business, oDashboardModel.oSessionFilters.Division, oDashboardModel.oSessionFilters.PrimaryProduct, oDashboardModel.oSessionFilters.Region, oDashboardModel.oSessionFilters.Sorg, "", oSessionUser.FULLNAME,
                                                          oDashboardModel.oSessionFilters.SNIClosureStatus, oDashboardModel.oSessionFilters.DBClosureStatus, oDashboardModel.oSessionFilters.Aging,
                                                          oDashboardModel.oSessionFilters.SNIAging, oDashboardModel.oSessionFilters.AgingBucket, oDashboardModel.oSessionFilters.SNIAgingBucket,
                                                          oDashboardModel.oSessionFilters.DeltaLoaddate, oDashboardModel.oSessionFilters.DeltaLoaddateBucket, oDashboardModel.oSessionFilters.DollarBucket,
                                                          oDashboardModel.oSessionFilters.BacklogStatus, oDashboardModel.oSessionFilters.CustomerPONumber, oDashboardModel.oSessionFilters.SoldToAccountID,
                                                          oDashboardModel.oSessionFilters.ShipToAccountID, oDashboardModel.oSessionFilters.SoldToAccount, oDashboardModel.oSessionFilters.ShipToAccount,
                                                          oDashboardModel.oSessionFilters.ZUAccount, oDashboardModel.oSessionFilters.PL, oDashboardModel.oSessionFilters.SoldToCountry, oDashboardModel.oSessionFilters.ShipToCountry,
                                                          oDashboardModel.oSessionFilters.ZUAccountID, oDashboardModel.oSessionFilters.ZUCountry, oDashboardModel.oSessionFilters.BillingBlock_Code, oDashboardModel.oSessionFilters.DeliveryBlock_Code,
                                                          oDashboardModel.oSessionFilters.BillingBlock_HeaderText, oDashboardModel.oSessionFilters.DeliveryBlock_HeaderText, oDashboardModel.oSessionFilters.BillingBlock_ItemText, oDashboardModel.oSessionFilters.DeliveryBlock_ItemText,
                                                          oDashboardModel.oSessionFilters.PaymentTerm, oDashboardModel.oSessionFilters.SalesRep, oDashboardModel.oSessionFilters.BTM, oDashboardModel.oSessionFilters.BTM_Manager,
                                                          oDashboardModel.oSessionFilters.ClosuredaysDeltaFrom, oDashboardModel.oSessionFilters.ClosuredaysDeltaTo,
                                                          "SNI_MF", CockpitUI);
                ViewData.Add("lst_UBOrderd", lst_UBOrderd);
                ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
                ViewBag.CurrentSelectedRegion = Region;

                return View(lst_UBOrderd);
            }

        }

        public ActionResult AllUnblockedOverdue()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];

            oDashboardModel = (DashboardModal)Session["oDashboardModal"];
            CockpitUI = oDashboardModel.UI_Type;

            ViewBag.UserRole = oSessionUser.ROLE_DESC;
            ViewBag.ThisReport = "Unblocked Overdue Orders";

            if (oSessionUser.ROLE_DESC != "WW Lead")
            {
                oDashboardModel.oSessionFilters.Region = oSessionUser.SUPERREGION;
                oDashboardModel.oSessionFilters.OrderOwner = oSessionUser.FULLNAME;
                oDashboardModel.oSessionFilters.CockpitUI = oDashboardModel.UI_Type;
            }
            else
            {
                oDashboardModel.oSessionFilters.Region = "";
                oDashboardModel.oSessionFilters.OrderOwner = oSessionUser.FULLNAME;
                oDashboardModel.oSessionFilters.CockpitUI = oDashboardModel.UI_Type;
            }


            lst_UBOrderd = service.GetUnblocked_Overdue(oDashboardModel.oSessionFilters.Business, oDashboardModel.oSessionFilters.Division, oDashboardModel.oSessionFilters.PrimaryProduct, oDashboardModel.oSessionFilters.Region, oDashboardModel.oSessionFilters.Sorg, "", oSessionUser.FULLNAME,
                                                          oDashboardModel.oSessionFilters.SNIClosureStatus, oDashboardModel.oSessionFilters.DBClosureStatus, oDashboardModel.oSessionFilters.Aging,
                                                          oDashboardModel.oSessionFilters.SNIAging, oDashboardModel.oSessionFilters.AgingBucket, oDashboardModel.oSessionFilters.SNIAgingBucket,
                                                          oDashboardModel.oSessionFilters.DeltaLoaddate, oDashboardModel.oSessionFilters.DeltaLoaddateBucket, oDashboardModel.oSessionFilters.DollarBucket,
                                                          oDashboardModel.oSessionFilters.BacklogStatus, oDashboardModel.oSessionFilters.CustomerPONumber, oDashboardModel.oSessionFilters.SoldToAccountID,
                                                          oDashboardModel.oSessionFilters.ShipToAccountID, oDashboardModel.oSessionFilters.SoldToAccount, oDashboardModel.oSessionFilters.ShipToAccount,
                                                          oDashboardModel.oSessionFilters.ZUAccount, oDashboardModel.oSessionFilters.PL, oDashboardModel.oSessionFilters.SoldToCountry, oDashboardModel.oSessionFilters.ShipToCountry,
                                                          oDashboardModel.oSessionFilters.ZUAccountID, oDashboardModel.oSessionFilters.ZUCountry, oDashboardModel.oSessionFilters.BillingBlock_Code, oDashboardModel.oSessionFilters.DeliveryBlock_Code,
                                                          oDashboardModel.oSessionFilters.BillingBlock_HeaderText, oDashboardModel.oSessionFilters.DeliveryBlock_HeaderText, oDashboardModel.oSessionFilters.BillingBlock_ItemText, oDashboardModel.oSessionFilters.DeliveryBlock_ItemText,
                                                          oDashboardModel.oSessionFilters.PaymentTerm, oDashboardModel.oSessionFilters.SalesRep, oDashboardModel.oSessionFilters.BTM, oDashboardModel.oSessionFilters.BTM_Manager,
                                                          oDashboardModel.oSessionFilters.ClosuredaysDeltaFrom, oDashboardModel.oSessionFilters.ClosuredaysDeltaTo,
                                                          "", CockpitUI).ToList();
            lstReportsMaster = service.Get_Reports_Master().ToList();
            oCurrentReport = lstReportsMaster.Where(p => p.ReportName == "Unblocked Orders").FirstOrDefault();

            Session["oCurrentReport"] = "Unblocked Overdue";
            ViewBag.oCurrentReport = oCurrentReport;


            ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
            return View(lst_UBOrderd);


        }

        public ActionResult Download(string ReportPath)
        {
            string filename = ReportPath;
            string filepath = Path.Combine(Server.MapPath("~/MECExtracts"), ReportPath + ".xlsx");

            return File(filepath, "application/vnd.ms-excel", ReportPath + ".xlsx");
        }

        public ActionResult ExcelExport(string Report, string Region)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            oDashboardModel = (DashboardModal)Session["oDashboardModal"];
            Session_Filters oFil = oDashboardModel.oSessionFilters;

            string CockpitUI = oDashboardModel.UI_Type;

            if (Region == "WW") Region = "";

            List<SNI_Excel_View> lst_SNIResult = new List<SNI_Excel_View>();

            switch (Report)
            {
                case "All Unblocked Orders":
                    lst_SNIResult = service.Get_UB_CatagoryOrdersExport(oFil.Region, oFil.Division, oFil.SalesOrder, oFil.CustomerPONumber, oFil.Sorg, oFil.Business, oFil.PL, oFil.PrimaryProduct,
                                    oFil.CreatedBy, oFil.OrderOwner, oFil.BacklogStatus, oFil.DollarBucket, oFil.Aging, oFil.AgingBucket, oFil.SNIAging, oFil.SNIAgingBucket, oFil.SNIClosureStatus,
                                    oFil.DBClosureStatus, oFil.BacklogAmt, oFil.SoldToAccountID, oFil.ShipToAccountID, oFil.SoldToAccount, oFil.ShipToAccount, oFil.SoldToCountry, oFil.ShipToCountry,
                                    oFil.ZUAccountID, oFil.ZUAccount, oFil.ZUCountry, oFil.SalesRep, oFil.BTM, oFil.BTM_Manager, oFil.PaymentTerm, oFil.BillingBlock_Code, oFil.DeliveryBlock_Code, oFil.BillingBlock_HeaderText, oFil.DeliveryBlock_HeaderText,
                                    oFil.BillingBlock_ItemText, oFil.DeliveryBlock_ItemText, oFil.DeltaLoaddate, oFil.DeltaLoaddateBucket, oFil.ClosuredaysDeltaFrom, oFil.ClosuredaysDeltaTo, oFil.NLHD,
                                    oFil.LoaDDate, oFil.TrioLoaDDate, oFil.CRDD, oFil.ExpReleaseDate, oFil.ReasonCode,
                        "All_UB", CockpitUI).ToList();
                    break;
                case "Unblocked Overdue Orders":
                    lst_SNIResult = service.Get_UB_CatagoryOrdersExport(oFil.Region, oFil.Division, oFil.SalesOrder, oFil.CustomerPONumber, oFil.Sorg, oFil.Business, oFil.PL, oFil.PrimaryProduct,
                                    oFil.CreatedBy, oFil.OrderOwner, oFil.BacklogStatus, oFil.DollarBucket, oFil.Aging, oFil.AgingBucket, oFil.SNIAging, oFil.SNIAgingBucket, oFil.SNIClosureStatus,
                                    oFil.DBClosureStatus, oFil.BacklogAmt, oFil.SoldToAccountID, oFil.ShipToAccountID, oFil.SoldToAccount, oFil.ShipToAccount, oFil.SoldToCountry, oFil.ShipToCountry,
                                    oFil.ZUAccountID, oFil.ZUAccount, oFil.ZUCountry, oFil.SalesRep, oFil.BTM, oFil.BTM_Manager, oFil.PaymentTerm, oFil.BillingBlock_Code, oFil.DeliveryBlock_Code, oFil.BillingBlock_HeaderText, oFil.DeliveryBlock_HeaderText,
                                    oFil.BillingBlock_ItemText, oFil.DeliveryBlock_ItemText, oFil.DeltaLoaddate, oFil.DeltaLoaddateBucket, oFil.ClosuredaysDeltaFrom, oFil.ClosuredaysDeltaTo, oFil.NLHD,
                                    oFil.LoaDDate, oFil.TrioLoaDDate, oFil.CRDD, oFil.ExpReleaseDate, oFil.ReasonCode,
                        "UB_OverDue", CockpitUI).ToList();
                    break;
            }

            GridView grid = new GridView();
            grid.AutoGenerateColumns = false;


            #region Data Column Binding
            BoundField column = new BoundField();
            column.HeaderText = "REGION";
            column.DataField = "REGION";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "SALES ORDER NO";
            column.DataField = "SALES_ORDERNO";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "CUSTOMER_PO_NO";
            column.DataField = "CUSTOMER_PO_NO";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "SALES_ORG";
            column.DataField = "SALES_ORG";
            grid.Columns.Add(column);


            column = new BoundField();
            column.HeaderText = "BUSINESS_GROUP";
            column.DataField = "BUSINESS_GROUP";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "PRIMARY_PRODUCT";
            column.DataField = "PRIMARY_PRODUCT";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "ORDER_CREATED_BY";
            column.DataField = "ORDER_CREATED_BY";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "ORDER_OWNER";
            column.DataField = "ORDER_OWNER";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "BACKLOG_STATUS";
            column.DataField = "BACKLOG_STATUS";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "DELV_BLK_TYPE";
            column.DataField = "DELV_BLK_TYPE";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "ORDER_AGE";
            column.DataField = "ORDER_AGE";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "Aging_Bucket";
            column.DataField = "Aging_Bucket";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "SNI_AGE";
            column.DataField = "SNI_AGE";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "SNI_AGING_BUCKET";
            column.DataField = "SNI_AGING_BUCKET";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "SNI_CLOSURE_STATUS";
            column.DataField = "SNI_CLOSURE_STATUS";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "DB_CLOSURE_STATUS";
            column.DataField = "DB_CLOSURE_STATUS";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "BACKLOG_AMT";
            column.DataField = "BACKLOG_AMT";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "COMPLETE_DELIVERY_HEADER";
            column.DataField = "COMPLETE_DELIVERY_HEADER";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "SOLD_TO_PARTY";
            column.DataField = "SOLD_TO_PARTY";
            grid.Columns.Add(column);


            column = new BoundField();
            column.HeaderText = "SOLD_TO_PARTY_NAME";
            column.DataField = "SOLD_TO_PARTY_NAME";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "SHIP_TO_PARTY_NAME";
            column.DataField = "SHIP_TO_PARTY_NAME";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "ZU_ACCOUNT_ID";
            column.DataField = "ZU_ACCOUNT_ID";
            grid.Columns.Add(column);


            column = new BoundField();
            column.HeaderText = "ZU_ACCOUNT_NAME";
            column.DataField = "ZU_ACCOUNT_NAME";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "Sales Force";
            column.DataField = "QUOTA_SF";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "FE_FE_DESC";
            column.DataField = "FE_FE_DESC";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "SRTATTR_AREA_MGR_NME";
            column.DataField = "SRTATTR_AREA_MGR_NME";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "SRTATTR_DISTRICT_MGR_NME";
            column.DataField = "SRTATTR_DISTRICT_MGR_NME";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "PAYMENT_TERMS";
            column.DataField = "PAYMENT_TERMS";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "PAYMENT_TYPE";
            column.DataField = "PAYMENT_TYPE";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "SHIPPING_POINT";
            column.DataField = "SHIPPING_POINT";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "DELIVERY_BLK_HDR_CD";
            column.DataField = "DELIVERY_BLK_HDR_CD";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "DELIVERY_BLK_HDR_DESC";
            column.DataField = "DELIVERY_BLK_HDR_DESC";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "BILLING_BLOCK_CD";
            column.DataField = "BILLING_BLOCK_CD";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "BILLING_BLOCK_DESC";
            column.DataField = "BILLING_BLOCK_DESC";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "NLHD_STATUS";
            column.DataField = "NLHD_STATUS";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "COMMIT_DATE";
            column.DataField = "COMMIT_DATE";
            column.DataFormatString = "{0:MM/dd/yy}";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "DELTA_LOAD_DATE_BUCKET";
            column.DataField = "DELTA_LOAD_DATE_BUCKET";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "SNI_CLOSURE_DAYS_DELTA";
            column.DataField = "SNI_CLOSURE_DAYS_DELTA";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "DB_CLOSURE_DAYS_DELTA";
            column.DataField = "DB_CLOSURE_DAYS_DELTA";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "ORDER_DT";
            column.DataField = "ORDER_DT";
            column.DataFormatString = "{0:MM/dd/yy}";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "CUSTOMER_REQ_GI_DATE";
            column.DataField = "CUSTOMER_REQ_GI_DATE";
            column.DataFormatString = "{0:MM/dd/yy}";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "EARLY_DEL_ACCEPTABLE";
            column.DataField = "EARLY_DEL_ACCEPTABLE";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "DELIVERY_BLOCK_CUT_OFF_DATE";
            column.DataField = "DELIVERY_BLOCK_CUT_OFF_DATE";
            column.DataFormatString = "{0:MM/dd/yy}";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "EXPECTED_RELEASE_DATE";
            column.DataField = "EXPECTED_RELEASE_DATE";
            column.DataFormatString = "{0:MM/dd/yy}";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "REASON_CODE";
            column.DataField = "REASON_CODE";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "SALES_ORG";
            column.DataField = "SALES_ORG";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "SERVICE_ORDER";
            column.DataField = "SERVICE_ORDER";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "CRM_INSTALL_END_DATE";
            column.DataField = "CRM_INSTALL_END_DATE";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "OVERALL_INSTALLATION_STATUS";
            column.DataField = "OVERALL_INSTALLATION_STATUS";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "LATEST_COMMENT";
            column.DataField = "LATEST_COMMENT";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "LATEST_COMMENT_BY";
            column.DataField = "LATEST_COMMENT_BY";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "LATEST_COMMENT_DATE_TIME";
            column.DataField = "LATEST_COMMENT_DATE_TIME";
            grid.Columns.Add(column);
            #endregion

            grid.DataSource = lst_SNIResult;
            grid.DataBind();

            foreach (GridViewRow row in grid.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    if (row.Cells[32].Text == "01/01/00") row.Cells[32].Text = "";
                    if (row.Cells[36].Text == "01/01/00") row.Cells[36].Text = "";
                    if (row.Cells[37].Text == "01/01/00") row.Cells[37].Text = "";
                    if (row.Cells[39].Text == "01/01/00") row.Cells[39].Text = "";
                }
            }

            Response.ClearContent();
            Response.Buffer = true;
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            Response.AddHeader("content-disposition", "attachment; filename=Result.xls");



            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("SNI_AgedSNI");

        }

    }
}
