using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cockpit_NextGenMVC.BAL;
using Cockpit_NextGenMVC.BAL_User_Mgmt;
using Cockpit_NextGenMVC.Models;
using System.Web.Mvc;
using System.IO;
using System.Web.UI;

namespace Cockpit_NextGenMVC.Controllers
{
    [SessionExpire]
    public class BCRController : Controller
    {
        static BAL.Service1Client service = new BAL.Service1Client();
        VW_USERS oSessionUser;
        string CockpitUI, currentAction;
        public IEnumerable<Backlogstats> oBacklogStats;
        public List<TBL_ORDER_COMMENT_VIEW> lstresults;

        public List<Tbl_Review_Reports> lstReportsMaster;

        public Tbl_Review_Reports oCurrentReport;
        public VW_BCR_SignOff_Summary[] Closed_BCRSummary, Closed_BCRSummary_Result;
        public Tbl_Daily_Control_Reports_Summary[] BCRSummary;
        public List<Tbl_History_Comments> Closed_Details_Result;

        public List<Tbl_Daily_Control_Reports_Summary> lstSummary;
        DashboardModal oDashboardModal;

        public ActionResult Summary()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            oDashboardModal = (DashboardModal)Session["oDashboardModal"];

            if (oDashboardModal.UI_Type == "CSR Cockpit")
            {
                BCRSummary = service.GetMyControlReportsSummary(oSessionUser.FULLNAME, oSessionUser.SUPERREGION, oSessionUser.ROLE_DESC, "");
            }
            else
            {
                BCRSummary = service.GetTeamControlReportsSummary(oSessionUser.FULLNAME, oSessionUser.SUPERREGION, oSessionUser.ROLE_DESC, oSessionUser.TEAM_NAME);
            }

            Session["BCRSummary"] = BCRSummary;

            DashboardModal tmpModel = oDashboardModal.BuildControlReportsSummaryforUI(oDashboardModal, BCRSummary, oSessionUser);
            oDashboardModal.Pie_Model = tmpModel.Pie_Model;
            oDashboardModal.Level2_Summary = tmpModel.Level2_Summary;
            oDashboardModal.BCR_Level1_RawData = tmpModel.BCR_Level1_RawData;

            Session["oDashboardModal"] = oDashboardModal;
            ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);

            return View(oDashboardModal);
        }

        public ActionResult Closed_Summary()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            oDashboardModal = (DashboardModal)Session["oDashboardModal"];

            DateTime DtFrom = System.DateTime.Now.AddMonths(-3);
            DateTime DtTo = System.DateTime.Now;

            if(Session["FromDate"] != null)
            {
                DtFrom = Convert.ToDateTime(Session["FromDate"]);
                DtTo = Convert.ToDateTime(Session["ToDate"]);
            }

            if (oSessionUser.ROLE_DESC == "WW Lead")
                Closed_BCRSummary = service.GetBCR_ClosedOrdersSummary("", DtFrom, DtTo);
            else
                Closed_BCRSummary = service.GetBCR_ClosedOrdersSummary(oSessionUser.SUPERREGION, DtFrom, DtTo);

            ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);

            ViewData.Add("FromDate", DtFrom.ToShortDateString());
            ViewData.Add("ToDate", DtTo.ToShortDateString());

            Session["FromDate"] = DtFrom.ToShortDateString();
            Session["ToDate"] = DtTo.ToShortDateString();

            return View(Closed_BCRSummary);
        }

        [HttpPost]
        public ActionResult Closed_Summary(FormCollection objCollection)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            oDashboardModal = (DashboardModal)Session["oDashboardModal"];

            DateTime DtFrom = Convert.ToDateTime(objCollection["Archive_Range_From"].ToString());
            DateTime DtTo = Convert.ToDateTime(objCollection["Archive_Range_To"].ToString());


            if (oSessionUser.ROLE_DESC == "WW Lead")
                Closed_BCRSummary = service.GetBCR_ClosedOrdersSummary("", DtFrom, DtTo);
            else
                Closed_BCRSummary = service.GetBCR_ClosedOrdersSummary(oSessionUser.SUPERREGION, DtFrom, DtTo);

            ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);

            ViewData.Add("FromDate", DtFrom);
            ViewData.Add("ToDate", DtTo);

            Session["FromDate"] =  DtFrom.ToShortDateString();
            Session["ToDate"] =  DtTo.ToShortDateString();

            return View(Closed_BCRSummary);
        }


        public ActionResult Closed_Details(string Report, string FromDate, string ToDate)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            oDashboardModal = (DashboardModal)Session["oDashboardModal"];

            if (FromDate != null)
            {
                ViewData.Add("FromDate", FromDate);
                ViewData.Add("ToDate", ToDate);
            }
            else if (Session["FromDate"] != null)
            {
                FromDate = Session["FromDate"].ToString();
                ToDate = Session["ToDate"].ToString();
            }
            else
            {
                FromDate = System.DateTime.Now.AddMonths(-3).ToShortDateString();
                ToDate = System.DateTime.Now.ToShortDateString();
            }

            if (oSessionUser.ROLE_DESC == "WW Lead")
                Closed_Details_Result = service.GetBCR_ClosedOrdersDetails("", Report, "0", "", "", "", "", "", "", "", FromDate, ToDate).ToList();
            else
                Closed_Details_Result = service.GetBCR_ClosedOrdersDetails(oSessionUser.SUPERREGION, Report, "0", "", "", "", "", "", "", "", FromDate, ToDate).ToList();


            ViewData.Add("FromDate", FromDate);
            ViewData.Add("ToDate", ToDate);

            ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
            ViewBag.currentBCR = Report;

            return View(Closed_Details_Result);
        }

        [HttpPost]
        public ActionResult Closed_Details(FormCollection objCollection, string Report)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            oDashboardModal = (DashboardModal)Session["oDashboardModal"];

            DateTime DtFrom = Convert.ToDateTime(objCollection["Archive_Range_From"].ToString());
            DateTime DtTo = Convert.ToDateTime(objCollection["Archive_Range_To"].ToString());

            if (Report == null) Report = objCollection["hid_ReportName"].ToString();

            ViewData.Add("FromDate", DtFrom);
            ViewData.Add("ToDate", DtTo);

            Session["FromDate"] = DtFrom.ToShortDateString();
            Session["ToDate"] = DtTo.ToShortDateString();

            if (oSessionUser.ROLE_DESC == "WW Lead")
                Closed_Details_Result = service.GetBCR_ClosedOrdersDetails("", Report, "0", "", "", "", "", "", "", "", DtFrom.ToShortDateString(), DtTo.ToShortDateString()).ToList();
            else
                Closed_Details_Result = service.GetBCR_ClosedOrdersDetails(oSessionUser.SUPERREGION, Report, "0", "", "", "", "", "", "", "", DtFrom.ToShortDateString(), DtTo.ToShortDateString()).ToList();

            ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
            ViewBag.currentBCR = Report;

            return View(Closed_Details_Result);
        }

        public JsonResult SignOffOrderComments(string Report, string Owner, string Area, string Orders)
        {
            Tbl_Order_comments oComment = new Tbl_Order_comments();
            oSessionUser = (VW_USERS)Session["UserProfile"];

            if (Report != "" && Orders != "") // VW_USERS Selected Orders for Reprot to sign-off
            {
                string[] MultipleOrders = Orders.Split(",".ToCharArray());
                foreach (string strOrder in MultipleOrders)
                {
                    try
                    {
                        oComment.Report = Report;
                        oComment.Sales_Ord = Convert.ToDouble(strOrder);
                        oComment.Material = "";
                        oComment.OrderOwner = Owner;
                        oComment.Reason_Code = "";
                        oComment.NextAction = "";
                        oComment.Reviewdate = Convert.ToDateTime("1/1/1900");
                        oComment.Cleardate = Convert.ToDateTime("1/1/1900");
                        oComment.Comment = "";
                        oComment.Comment_Date = Convert.ToDateTime("1/1/1900");
                        oComment.SignOff = "Yes";
                        oComment.SignOff_By = oSessionUser.NTLOGIN;
                        oComment.SignOff_Date = System.DateTime.Now;
                        oComment.snapshotdate = Convert.ToDateTime("1/1/1900");

                        service.Add_OrderSignOff(oComment, oSessionUser.NTLOGIN);
                    }
                    catch (Exception ex)
                    { }
                }
            }

            BindDefaultData(Report, Owner, Session["currentAction"].ToString());
            return Json(lstresults);
        }

        public ActionResult CSRReport(string Report, string Owner, string Area)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            oDashboardModal = (DashboardModal)Session["oDashboardModal"];

            BindDefaultData(Report, Owner, Area);

            currentAction = Area;
            Session["currentAction"] = currentAction;

            oCurrentReport = (Tbl_Review_Reports)Session["oCurrentReport"];
            ViewData["lstReportsMaster"] = new SelectList((List<Tbl_Review_Reports>)Session["lstReportsMaster"], "ReportName", "ReportName", oCurrentReport.ReportName);

            ViewBag.oCurrentReport = oCurrentReport;
            ViewBag.oCurrentOwner = Owner;
            ViewBag.currentBCR = Report;
            ViewBag.oArea = Area;


            ViewData.Add("oDashboardModal", oDashboardModal);
            return View(lstresults);
        }

        public void BindDefaultData(string Report, string Owner, string Area)
        {
            if (Session["UserProfile"] != null)
            {
                oSessionUser = (VW_USERS)Session["UserProfile"];
                oDashboardModal = (DashboardModal)Session["oDashboardModal"];

                ViewBag.UserRole = oSessionUser.ROLE_DESC;

                if (Report == null & Session["Report"] != null) Report = Session["Report"].ToString();
                if (Area == null & Session["Area"] != null) Area = Session["Area"].ToString();
                if (Owner == null & Session["Owner"] != null) Owner = Session["Owner"].ToString();

                try
                {
                    if (oDashboardModal.UI_Type == "CSR Cockpit")
                        lstresults = service.GetControlReportsDetails(oSessionUser.FULLNAME, oSessionUser.SUPERREGION, oSessionUser.ROLE_DESC, "", "", "", "", Report, oDashboardModal.UI_Type, Area).ToList();
                    else
                        lstresults = service.GetControlReportsDetails(oSessionUser.FULLNAME, oSessionUser.SUPERREGION, oSessionUser.ROLE_DESC, "", "", "", "", Report, oDashboardModal.UI_Type, Area).ToList();
                }
                catch (Exception ex)
                {

                }

                lstReportsMaster = service.Get_Reports_Master().ToList();

                oCurrentReport = lstReportsMaster.Where(p => p.ReportName == Report).FirstOrDefault();

                oBacklogStats = (IEnumerable<Backlogstats>)Session["lstControlReports"];

                Session["lstresults"] = lstresults;
                Session["lstReportsDetails"] = lstReportsMaster;
                Session["lstReportsMaster"] = lstReportsMaster;
                Session["oCurrentReport"] = oCurrentReport;

                Session["Report"] = Report;
                Session["Area"] = Area;
                Session["Owner"] = Owner;
            }
        }

        public JsonResult SelectedReportDefinition(string Report)
        {
            if (Session["lstReportsDetails"] != null)
                lstReportsMaster = (List<Tbl_Review_Reports>)Session["lstReportsDetails"];
            else
                lstReportsMaster = service.Get_Reports_Master().ToList();

            var chosenreport = lstReportsMaster.Where(p => p.ReportName == Report).FirstOrDefault();

            string result = chosenreport.ReportName + " - " + chosenreport.CustomFilter + "^" + chosenreport.Description2 + "^" + chosenreport.Description1;
            return Json(result);
        }

        public JsonResult SelectedReportData(string Report, string Area)
        {
            if (Session["UserProfile"] != null)
            {
                oSessionUser = (VW_USERS)Session["UserProfile"];
                ViewBag.UserRole = oSessionUser.ROLE_DESC;

                oBacklogStats = (IEnumerable<Backlogstats>)Session["lstControlReports"];
                ViewData["lstReportsMaster"] = new SelectList(oBacklogStats, "ReportName", "ReportName");
            }

            oDashboardModal = (DashboardModal)Session["oDashboardModal"];

            if (oDashboardModal.UI_Type == "CSR Cockpit")
                lstresults = service.GetControlReportsDetails(oSessionUser.FULLNAME, oSessionUser.SUPERREGION, oSessionUser.ROLE_DESC, "", "", "", "", Report, oDashboardModal.UI_Type, Area).ToList();
            else
                lstresults = service.GetControlReportsDetails(oSessionUser.FULLNAME, oSessionUser.SUPERREGION, "Supervisor", "", "", "", "", Report, oDashboardModal.UI_Type, Area).ToList();

            Session["lstresults"] = lstresults;
            Session["lstReportsMaster"] = ViewData["lstReportsMaster"];
            Session["Report"] = Report;
            Session["Area"] = Area;


            ViewBag.currentBCR = Report;
            return Json(lstresults);
        }

        public JsonResult GetUniqueMaterials(string order)
        {
            lstresults = (List<TBL_ORDER_COMMENT_VIEW>)Session["lstresults"];

            List<Tbl_Order_comments> oComments = (List<Tbl_Order_comments>)Session["CommentsHistory"];

            string[] uniqueMaterials = oComments.Select(p => p.Material).Distinct().ToArray();

            return Json(uniqueMaterials);
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        public ActionResult ExcelExport(string Region)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            DashboardModal oDashboardModel = (DashboardModal)Session["oDashboardModal"];

            if (Region == "WW") Region = "";

            List<Tbl_Order_comments> lst_BCRResult = new List<Tbl_Order_comments>();

            if (oSessionUser.ROLE_DESC == "CSR" && oDashboardModel.UI_Type == "CSR Cockpit")
            {
                lst_BCRResult = service.GetControlReprotsExcel("", "", "", "", oSessionUser.FULLNAME, "", "", Region, "", "", "", "", "", "").ToList();
            }
            else if (oSessionUser.ROLE_DESC == "CSR" && oDashboardModel.UI_Type != "CSR Cockpit")
            {
                lst_BCRResult = service.GetControlReprotsExcel("", "", "", "", "", oSessionUser.TEAM_NAME, "", Region, "", "", "", "", "", "").ToList();
            }
            else if (oSessionUser.ROLE_DESC == "Supervisor")
            {
                lst_BCRResult = service.GetControlReprotsExcel("", "", "", "", "", oSessionUser.TEAM_NAME, "", Region, "", "", "", "", "", "").ToList();
            }
            else if (oSessionUser.ROLE_DESC == "BPA" || oSessionUser.ROLE_DESC == "Regional Lead")
            {
                lst_BCRResult = service.GetControlReprotsExcel("", "", "", "", "", "", "", Region, "", "", "", "", "", "").ToList();
            }
            else if (oSessionUser.ROLE_DESC == "WW Lead")
            {
                lst_BCRResult = service.GetControlReprotsExcel("", "", "", "", "", "", "", "", "", "", "", "", "", "").ToList();
            }

            System.Web.UI.WebControls.GridView grid = new System.Web.UI.WebControls.GridView();
            grid.DataSource = lst_BCRResult;
            grid.DataBind();

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
