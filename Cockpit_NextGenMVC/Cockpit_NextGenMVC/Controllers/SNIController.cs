﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Mvc;
using Cockpit_NextGenMVC.BAL;
using Cockpit_NextGenMVC.BAL_User_Mgmt;
using Cockpit_NextGenMVC.Models;

using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace Cockpit_NextGenMVC.Controllers
{
    [SessionExpire]
    public class SNIController : Controller
    {
        //
        // GET: /SNI/

        static BAL.Service1Client service = new BAL.Service1Client();
        static readonly BAL_User_Mgmt.Service1Client Users_service = new BAL_User_Mgmt.Service1Client();

        VW_USERS oSessionUser;
        string CockpitUI;
        public List<VW_Orders_Info> lst_SNIResult;
        public List<Tbl_Review_Reports> lstReportsMaster;
        public Tbl_Review_Reports oCurrentReport;
        DashboardModal oDashboardModel;
        public List<Tbl_WW_Blocked_Orders_Summary> lst_Summary_AgingBucket, lst_Summary_DollarBucket;

        public ActionResult SNI_Summary()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            oDashboardModel = (DashboardModal)Session["oDashboardModal"];
            CockpitUI = oDashboardModel.UI_Type;

            ViewBag.UserRole = oSessionUser.ROLE_DESC;
            Session["Report"] = "Delivery block";

            lst_Summary_AgingBucket = service.WW_Process_Summary("SNI", "DollarBucket").ToList();

            ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);

            ViewBag.BucketArea = "SNI";
            ViewBag.BucketName = "DollarBucket";

            return View(lst_Summary_AgingBucket);
        }

        public ActionResult SNI_AgedSNI()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            oDashboardModel = (DashboardModal)Session["oDashboardModal"];
            CockpitUI = oDashboardModel.UI_Type;
            ViewBag.UserRole = oSessionUser.ROLE_DESC;
            ViewBag.ThisReport = "Aged SNI Orders";

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

            BAL.Session_Filters oFilters = oDashboardModel.oSessionFilters.Set_ServiceFilters();
            lst_SNIResult = service.Get_SNICatagoryOrders(oFilters, 
                                                          "SNI_MF", CockpitUI).ToList();
            ViewData.Add("lst_SNIResult", lst_SNIResult);

            lstReportsMaster = service.Get_Reports_Master().ToList();
            oCurrentReport = lstReportsMaster.Where(p => p.ReportName == "Billing Block").FirstOrDefault();

            Session["oCurrentReport"] = oCurrentReport;
            ViewBag.oCurrentReport = oCurrentReport;

            var TeamProfile = Users_service.GetUserTeamDetails(oSessionUser.TEAM_NAME);
            var UniqueMembers = (from tbl in TeamProfile group tbl by new { tbl.FULLNAME } into g select new Model_Pie { category = g.Key.FULLNAME }).ToList();

            Session["TeamProfile"] = UniqueMembers;
            ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);

            return View(lst_SNIResult);
        }

        public ActionResult SNI_AllOrders()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            oDashboardModel = (DashboardModal)Session["oDashboardModal"];
            CockpitUI = oDashboardModel.UI_Type;

            ViewBag.UserRole = oSessionUser.ROLE_DESC;
            ViewBag.ThisReport = "SNI All Orders";

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
            BAL.Session_Filters oFilters = oDashboardModel.oSessionFilters.Set_ServiceFilters();


            var lst_SNIResult = service.Get_SNIAllOrders(oFilters, 
                                                          "SNI_MF", CockpitUI);
            lstReportsMaster = service.Get_Reports_Master().ToList();
            oCurrentReport = lstReportsMaster.Where(p => p.ReportName == "Billing Block").FirstOrDefault();

            Session["oCurrentReport"] = oCurrentReport;
            ViewBag.oCurrentReport = oCurrentReport;
            ViewData.Add("lst_SNIResult", lst_SNIResult);

            ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
            return View(lst_SNIResult);
        }

        public ActionResult SNI_AllOrders_via_Summary(string Region, string Bucket, string Bucket_Type)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];

            oDashboardModel = (DashboardModal)Session["oDashboardModal"];
            CockpitUI = oDashboardModel.UI_Type;

            ViewBag.UserRole = oSessionUser.ROLE_DESC;
            ViewBag.ThisReport = "SNI All Orders";

            lstReportsMaster = service.Get_Reports_Master().ToList();
            oCurrentReport = lstReportsMaster.Where(p => p.ReportName == "Billing Block").FirstOrDefault();

            Session["oCurrentReport"] = oCurrentReport;
            ViewBag.oCurrentReport = oCurrentReport;
            ViewBag.BucketName = Bucket_Type;

            if (Region == "WW") Region = "";
            oDashboardModel.oSessionFilters.Region = Region;
            oDashboardModel.oSessionFilters.OrderOwner = oSessionUser.FULLNAME;
            oDashboardModel.oSessionFilters.CockpitUI = oDashboardModel.UI_Type;
            BAL.Session_Filters oFilters = oDashboardModel.oSessionFilters.Set_ServiceFilters();


            VW_Orders_Info[] lst_SNIResult;

            if (Bucket_Type == "AgingBucket")
            {
                lst_SNIResult = service.Get_SNIAllOrders(oFilters, 
                                                          "SNI_MF", CockpitUI);
                ViewData.Add("lst_SNIResult", lst_SNIResult);

                ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
                ViewBag.CurrentSelectedRegion = Region;

                return View(lst_SNIResult);
            }
            else if (Bucket_Type == "DollarBucket")
            {
                lst_SNIResult = service.Get_SNIAllOrders(oFilters,
                                                          "SNI_MF", CockpitUI);
                ViewData.Add("lst_SNIResult", lst_SNIResult);

                ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
                ViewBag.CurrentSelectedRegion = Region;

                return View(lst_SNIResult);
            }
            else
            {
                lst_SNIResult = service.Get_SNIAllOrders(oFilters, 
                                                          "SNI_MF", CockpitUI);
                ViewData.Add("lst_SNIResult", lst_SNIResult);

                ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
                ViewBag.CurrentSelectedRegion = Region;

                return View(lst_SNIResult);
            }
        }

        public ActionResult SNI_InvoicingErrorsOrders()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            oDashboardModel = (DashboardModal)Session["oDashboardModal"];
            CockpitUI = oDashboardModel.UI_Type;

            ViewBag.UserRole = oSessionUser.ROLE_DESC;
            ViewBag.ThisReport = "SNI Invoicing Errors";

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

            BAL.Session_Filters oFilters = oDashboardModel.oSessionFilters.Set_ServiceFilters();


            var lst_SNIResult = service.Get_SNICatagoryOrders(oFilters, 
                                                          "SNI_InvoicingError", CockpitUI);
            ViewData.Add("lst_SNIResult", lst_SNIResult);

            lstReportsMaster = service.Get_Reports_Master().ToList();
            oCurrentReport = lstReportsMaster.Where(p => p.ReportName == "Billing Block").FirstOrDefault();

            Session["oCurrentReport"] = oCurrentReport;
            ViewBag.oCurrentReport = oCurrentReport;

            ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
            return View(lst_SNIResult);
        }

        public ActionResult SNI_ReleaseDatePassedOrders()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];

            oDashboardModel = (DashboardModal)Session["oDashboardModal"];
            CockpitUI = oDashboardModel.UI_Type;

            ViewBag.UserRole = oSessionUser.ROLE_DESC;
            ViewBag.ThisReport = "SNI Release Passed";

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

            BAL.Session_Filters oFilters = oDashboardModel.oSessionFilters.Set_ServiceFilters();

            var lst_SNIResult = service.Get_SNICatagoryOrders(oFilters, 
                                                          "SNI_ExpectedReleasePassed", CockpitUI);
            ViewData.Add("lst_SNIResult", lst_SNIResult);

            lstReportsMaster = service.Get_Reports_Master().ToList();
            oCurrentReport = lstReportsMaster.Where(p => p.ReportName == "Billing Block").FirstOrDefault();

            Session["oCurrentReport"] = oCurrentReport;
            ViewBag.oCurrentReport = oCurrentReport;

            ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
            return View(lst_SNIResult);
        }

        public ActionResult SNI_ReleaseDateDueTodayOrders()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];

            oDashboardModel = (DashboardModal)Session["oDashboardModal"];
            CockpitUI = oDashboardModel.UI_Type;

            ViewBag.UserRole = oSessionUser.ROLE_DESC;
            ViewBag.ThisReport = "SNI Release Due Today";

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

            BAL.Session_Filters oFilters = oDashboardModel.oSessionFilters.Set_ServiceFilters();

            var lst_SNIResult = service.Get_SNICatagoryOrders(oFilters, 
                                                          "SNI_ExpectedReleaseToday", CockpitUI);
            ViewData.Add("lst_SNIResult", lst_SNIResult);

            lstReportsMaster = service.Get_Reports_Master().ToList();
            oCurrentReport = lstReportsMaster.Where(p => p.ReportName == "Billing Block").FirstOrDefault();

            Session["oCurrentReport"] = oCurrentReport;
            ViewBag.oCurrentReport = oCurrentReport;

            ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);

            return View(lst_SNIResult);
        }

        public ActionResult SNI_NoReleaseDateOrders()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];

            oDashboardModel = (DashboardModal)Session["oDashboardModal"];
            CockpitUI = oDashboardModel.UI_Type;

            ViewBag.UserRole = oSessionUser.ROLE_DESC;
            ViewBag.ThisReport = "SNI Release No Release Date";

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
            BAL.Session_Filters oFilters = oDashboardModel.oSessionFilters.Set_ServiceFilters();

            var lst_SNIResult = service.Get_SNICatagoryOrders(oFilters, 
                                                          "SNI_No_ExpectedRelease", CockpitUI);
            ViewData.Add("lst_SNIResult", lst_SNIResult);

            lstReportsMaster = service.Get_Reports_Master().ToList();
            oCurrentReport = lstReportsMaster.Where(p => p.ReportName == "Billing Block").FirstOrDefault();

            Session["oCurrentReport"] = oCurrentReport;
            ViewBag.oCurrentReport = oCurrentReport;

            ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
            return View(lst_SNIResult);
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

            string CockpitUI = oDashboardModel.UI_Type;

            if (Region == "WW") Region = "";

            

            List<SNI_Excel_View> lst_SNIResult = new List<SNI_Excel_View>();

            BAL.Session_Filters oFilters = oDashboardModel.oSessionFilters.Set_ServiceFilters();

            switch (Report)
            {
                case "SNI All Orders":
                    lst_SNIResult = service.Get_SNICatagoryOrdersExport(oFilters,
                                    "SNI_All", CockpitUI).ToList();
                    break;
                case "Aged SNI Orders":
                    lst_SNIResult = service.Get_SNICatagoryOrdersExport(oFilters,
                        "SNI_MF", CockpitUI).ToList();

                    break;
                case "SNI Invoicing Errors":
                    lst_SNIResult = service.Get_SNICatagoryOrdersExport(oFilters,
                        "SNI_InvoicingError", CockpitUI).ToList();

                    break;
                case "SNI Release Passed":
                    lst_SNIResult = service.Get_SNICatagoryOrdersExport(oFilters,
                        "SNI_ExpectedReleasePassed", CockpitUI).ToList();

                    break;
                case "SNI Release Due Today":
                    lst_SNIResult = service.Get_SNICatagoryOrdersExport(oFilters,
                        "SNI_ExpectedReleaseToday", CockpitUI).ToList();

                    break;
                case "SNI Release No Release Date":
                    lst_SNIResult = service.Get_SNICatagoryOrdersExport(oFilters,
                        "SNI_No_ExpectedRelease", CockpitUI).ToList();

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
            column.HeaderText = "LATEST_COMMENT";
            column.DataField = "LATEST_COMMENT";
            grid.Columns.Add(column);

            column = new BoundField();
            column.HeaderText = "SALES_ORG";
            column.DataField = "SALES_ORG";
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
