﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Web.Mvc;
using Cockpit_NextGenMVC.BAL;
using Cockpit_NextGenMVC.Models;

using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace Cockpit_NextGenMVC.Controllers
{
    [SessionExpire]
    public class DBController : Controller
    {
        static BAL.Service1Client service = new BAL.Service1Client();
        VW_USERS oSessionUser;
        string CockpitUI;
        public List<VW_Orders_Info> lst_DBMasterFocus;
        public List<Tbl_WW_Blocked_Orders_Summary> lst_Summary_AgingBucket, lst_Summary_DollarBucket;

        public List<Tbl_Review_Reports> lstReportsMaster;
        public Tbl_Review_Reports oCurrentReport;
        public DashboardModal oDashboardModel;

        public ActionResult DB_Summary()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            oDashboardModel = (DashboardModal)Session["oDashboardModal"];
            CockpitUI = oDashboardModel.UI_Type;

            ViewBag.UserRole = oSessionUser.ROLE_DESC;
            Session["Report"] = "Delivery block";

            lst_Summary_AgingBucket = service.WW_Process_Summary("Delivery block", "DollarBucket").ToList();

            ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);

            ViewBag.BucketArea = "Delivery block";
            ViewBag.BucketName = "DollarBucket";

            return View(lst_Summary_AgingBucket);
        }

        public ActionResult DB_AllOrders([DataSourceRequest] DataSourceRequest request)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            oDashboardModel = (DashboardModal)Session["oDashboardModal"];
            CockpitUI = oDashboardModel.UI_Type;

            ViewBag.UserRole = oSessionUser.ROLE_DESC;
            ViewBag.ThisReport = "DB All Orders";

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

            lst_DBMasterFocus = service.Get_DB_RawOrders(oFilters, "", CockpitUI).ToList();
            lstReportsMaster = service.Get_Reports_Master().ToList();
            oCurrentReport = lstReportsMaster.Where(p => p.ReportName == "Delivery block").FirstOrDefault();

            Session["oCurrentReport"] = oCurrentReport;
            ViewBag.oCurrentReport = oCurrentReport;
            ViewData.Add("lst_DBMasterFocus", lst_DBMasterFocus);

            ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);

            return View(lst_DBMasterFocus);
        }

        public ActionResult DB_AllOrders_via_Summary(string Region, string Bucket, string Bucket_Type)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            oDashboardModel = (DashboardModal)Session["oDashboardModal"];
            CockpitUI = oDashboardModel.UI_Type;

            ViewBag.UserRole = oSessionUser.ROLE_DESC;
            ViewBag.ThisReport = "DB All Orders";

            lstReportsMaster = service.Get_Reports_Master().ToList();
            oCurrentReport = lstReportsMaster.Where(p => p.ReportName == "Delivery block").FirstOrDefault();

            Session["oCurrentReport"] = oCurrentReport;
            ViewBag.oCurrentReport = oCurrentReport;
            ViewData.Add("lst_DBMasterFocus", lst_DBMasterFocus);

            if (Region == "WW") Region = "";
            oDashboardModel.oSessionFilters.Region = Region;
            oDashboardModel.oSessionFilters.OrderOwner = oSessionUser.FULLNAME;
            oDashboardModel.oSessionFilters.CockpitUI = oDashboardModel.UI_Type;

            BAL.Session_Filters oFilters = oDashboardModel.oSessionFilters.Set_ServiceFilters();

            VW_Orders_Info[] lst_UBOrderd;

            if (Bucket_Type == "AgingBucket")
            {
                lst_DBMasterFocus = service.Get_DB_RawOrders(oFilters, "", CockpitUI).ToList();

                ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
                ViewBag.CurrentSelectedRegion = Region;

                return View(lst_DBMasterFocus);
            }
            else if (Bucket_Type == "DollarBucket")
            {
                lst_DBMasterFocus = service.Get_DB_RawOrders(oFilters, "", CockpitUI).ToList();
                ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
                ViewBag.CurrentSelectedRegion = Region;

                return View(lst_DBMasterFocus);
            }
            else
            {
                lst_DBMasterFocus = service.Get_DB_RawOrders(oFilters, "", CockpitUI).ToList();
                ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
                ViewBag.CurrentSelectedRegion = Region;

                return View(lst_DBMasterFocus);
            }
        }

        public ActionResult DB_OverDueOrders()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            oDashboardModel = (DashboardModal)Session["oDashboardModal"];
            CockpitUI = oDashboardModel.UI_Type;

            ViewBag.UserRole = oSessionUser.ROLE_DESC;
            ViewBag.ThisReport = "DB Overdue Orders";

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

            var lst_DBMasterFocus = service.Get_DB_Overdue(oFilters,"", CockpitUI);
            ViewData.Add("lst_DBOverdue", lst_DBMasterFocus);


            lstReportsMaster = service.Get_Reports_Master().ToList();
            oCurrentReport = lstReportsMaster.Where(p => p.ReportName == "Delivery block").FirstOrDefault();

            Session["oCurrentReport"] = oCurrentReport;
            ViewBag.oCurrentReport = oCurrentReport;

            ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);

            return View(lst_DBMasterFocus);
        }

        public ActionResult DB_AgedGreater90Days()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            oDashboardModel = (DashboardModal)Session["oDashboardModal"];
            CockpitUI = oDashboardModel.UI_Type;

            ViewBag.UserRole = oSessionUser.ROLE_DESC;
            ViewBag.ThisReport = "DB Aged > 90 Orders";

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

            var lst_DBMasterFocus = service.Get_DB_Greater_90_days(oFilters, "", CockpitUI);
            ViewData.Add("lst_DBMasterFocus", lst_DBMasterFocus);

            lstReportsMaster = service.Get_Reports_Master().ToList();
            oCurrentReport = lstReportsMaster.Where(p => p.ReportName == "Delivery block").FirstOrDefault();

            Session["oCurrentReport"] = oCurrentReport;
            ViewBag.oCurrentReport = oCurrentReport;

            ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
            return View(lst_DBMasterFocus);
        }

        public ActionResult DB_expectedreleasedatepassed()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            oDashboardModel = (DashboardModal)Session["oDashboardModal"];
            CockpitUI = oDashboardModel.UI_Type;
            ViewBag.UserRole = oSessionUser.ROLE_DESC;
            ViewBag.ThisReport = "DB ReleasePassed Orders";

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


            var lst_DBMasterFocus = service.Get_DB_Expected_Release_Passed(oFilters, "", CockpitUI);

            lstReportsMaster = service.Get_Reports_Master().ToList();
            oCurrentReport = lstReportsMaster.Where(p => p.ReportName == "Delivery block").FirstOrDefault();

            Session["oCurrentReport"] = oCurrentReport;
            ViewBag.oCurrentReport = oCurrentReport;

            oSessionUser = (VW_USERS)Session["UserProfile"];
            ViewBag.UserRole = oSessionUser.ROLE_DESC;


            ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
            return View(lst_DBMasterFocus);
        }

        public ActionResult DB_becomeoverduewithin14days()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            oDashboardModel = (DashboardModal)Session["oDashboardModal"];
            CockpitUI = oDashboardModel.UI_Type;

            ViewBag.UserRole = oSessionUser.ROLE_DESC;
            ViewBag.ThisReport = "DB Overdue in 14 Days Orders";

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


            var lst_DBMasterFocus = service.Get_DB_Overdue_14_Days(oFilters,
                                                          "", CockpitUI);

            lstReportsMaster = service.Get_Reports_Master().ToList();
            oCurrentReport = lstReportsMaster.Where(p => p.ReportName == "Delivery block").FirstOrDefault();

            Session["oCurrentReport"] = oCurrentReport;
            ViewBag.oCurrentReport = oCurrentReport;

            oSessionUser = (VW_USERS)Session["UserProfile"];
            ViewBag.UserRole = oSessionUser.ROLE_DESC;


            ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
            return View(lst_DBMasterFocus);
        }

        public ActionResult DBSpeedtoRevenueSummary()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];

            oDashboardModel = (DashboardModal)Session["oDashboardModal"];
            CockpitUI = oDashboardModel.UI_Type;

            ViewBag.UserRole = oSessionUser.ROLE_DESC;
            ViewBag.ThisReport = "DB Speed to Revenue Summary";

            var lst_DBMasterFocus = service.GetSTR_Summary(System.DateTime.Today.AddDays(-1), System.DateTime.Today);

            ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
            return View(lst_DBMasterFocus);
        }

        public ActionResult DBSpeedtoRevenue(string Region)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];

            if (Region == null)
                Region = oSessionUser.SUPERREGION;

            ViewBag.UserRole = oSessionUser.ROLE_DESC;

            ViewBag.ThisReport = "DB Speed to Revenue";

            var lst_DBMasterFocus = service.Get_DB_SpeedToRevenue(oSessionUser.FULLNAME, "", "", "", Region, "", "", "", "", "", "", "", "", "Blocked");
            lstReportsMaster = service.Get_Reports_Master().ToList();
            oCurrentReport = lstReportsMaster.Where(p => p.ReportName == "Delivery block").FirstOrDefault();

            Session["oCurrentReport"] = oCurrentReport;
            ViewBag.oCurrentReport = oCurrentReport;


            ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
            return View(lst_DBMasterFocus);
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
            BAL.Session_Filters oFil = oDashboardModel.oSessionFilters.Set_ServiceFilters();

            string CockpitUI = oDashboardModel.UI_Type;

            if (Region == "WW") Region = "";

            List<SNI_Excel_View> lst_SNIResult = new List<SNI_Excel_View>();


            switch (Report)
            {
                case "DB All Orders":
                    lst_SNIResult = service.Get_DB_CatagoryOrdersExport(oFil,
                        "All_DB", CockpitUI).ToList();
                    break;
                case "DB Overdue Orders":
                    lst_SNIResult = service.Get_DB_CatagoryOrdersExport(oFil,
                        "DB_OverDue", CockpitUI).ToList();
                    break;
                case "DB Aged > 90 Orders":
                    lst_SNIResult = service.Get_DB_CatagoryOrdersExport(oFil,
                        "DB_Aged_90_Days", CockpitUI).ToList();
                    break;
                case "DB ReleasePassed Orders":
                    lst_SNIResult = service.Get_DB_CatagoryOrdersExport(oFil,
                        "DB_PassedDueDate", CockpitUI).ToList();
                    break;
                case "DB Overdue in 14 Days Orders":
                    lst_SNIResult = service.Get_DB_CatagoryOrdersExport(oFil,
                        "DB_OverDue_14_Days", CockpitUI).ToList();
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

            return RedirectToAction("DB_AgedGreater90Days");
        }

    }
}
