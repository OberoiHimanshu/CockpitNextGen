using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cockpit_NextGenMVC.BAL;
using Cockpit_NextGenMVC.BAL_User_Mgmt;
using Cockpit_NextGenMVC.Models;

namespace Cockpit_NextGenMVC.Controllers
{
    [SessionExpire]
    public class HomeController : Controller
    {
        static BAL.Service1Client service = new BAL.Service1Client();
        static BAL_User_Mgmt.Service1Client UM_service = new BAL_User_Mgmt.Service1Client();

        Tbl_Backlog_Summary Summary;
        Tbl_Daily_Control_Reports_Summary[] BCRSummary, OverallBCRSummary;
        List<SelectListItem> lstRolebasedCockpit;
        List<BCRstats> lstBCRStats;
        List<Backlogstats> lstBacklogstats;
        Tbl_Followup_Summary[] oMyFollowups;
        Tbl_Followups[] oSystemGeneratedFollowups;
        //VW_Catagory_Followups[] oCatagoryFollowups;
        Backlogstats oFollowupStats, oDBStats, oSNIStats;
        BCRstats oBCRFollowupStats;

        List<VW_USERS> oSessionUserProfiles;
        List<SelectListItem> lstRoles;
        VW_USERS oSessionUser;
        List<TBL_ROLE> lst_Roles;
        List<TBL_TEAM_STRUCTURE> lst_Teams;

        Tbl_Unmapped_Users[] unmappedSummery;

        DashboardModal oDashboardModal;

        string CockpitUI = string.Empty;
        string currentMainReport, FollowupCatagory;

        void BuildDashboadModel(String ReportArea, string currentMainReport, string UIType)
        {
            oDashboardModal = new DashboardModal();

            oDashboardModal.UI_Type = UIType;

            if (UIType == "CSR Cockpit")
            {

                oMyFollowups = service.GetMyFollowupsSummary(oSessionUser.NTLOGIN, "");
                oDashboardModal.FollowupSummary = oMyFollowups;

                oDashboardModal.TotalFollowups = Convert.ToInt32(oMyFollowups[0].TotalFollowups);
                oDashboardModal.FollowupSummary = oMyFollowups;

                oSystemGeneratedFollowups = service.GetMyFollowupsSystemGeneratedList(oSessionUser.NTLOGIN, "");
                oDashboardModal.SystemGeneratedFollowups = oSystemGeneratedFollowups;

                BCRSummary = service.GetMyControlReportsSummary(oSessionUser.FULLNAME, oSessionUser.SUPERREGION, oSessionUser.ROLE_DESC, "");
                Summary = service.GetMasterFocusSummary(oSessionUser.FULLNAME, UIType);

                OverallBCRSummary = (from tbl in BCRSummary
                                     group tbl by new { tbl.ReportName } into g
                                     select new Tbl_Daily_Control_Reports_Summary
                                     {
                                         ReportName = g.Key.ReportName,
                                         Pending_Comments = g.Sum(s => s.Pending_Comments),
                                         Pending_Review = g.Sum(s => s.Pending_Review),
                                         Pending_Sign_Off = g.Sum(s => s.Pending_Sign_Off)
                                     }).ToArray();


                oDashboardModal.My_BCRSummary = OverallBCRSummary;
            }
            else
            {

                oMyFollowups = service.GetTeamFollowupsSummary(oSessionUser.NTLOGIN, oSessionUser.TEAM_NAME);
                oDashboardModal.FollowupSummary = oMyFollowups;

                oDashboardModal.TotalFollowups = Convert.ToInt32(oMyFollowups[0].TotalFollowups);
                oDashboardModal.FollowupSummary = oMyFollowups;

                oSystemGeneratedFollowups = service.GetTeamFollowupsSystemGeneratedList("", oSessionUser.TEAM_NAME);
                oDashboardModal.SystemGeneratedFollowups = oSystemGeneratedFollowups;

                BCRSummary = service.GetTeamControlReportsSummary(oSessionUser.FULLNAME, oSessionUser.SUPERREGION, oSessionUser.ROLE_DESC, oSessionUser.TEAM_NAME);
                Summary = service.GetMasterFocusSummary(oSessionUser.FULLNAME, UIType);

                OverallBCRSummary = (from tbl in BCRSummary
                                     group tbl by new { tbl.ReportName } into g
                                     select new Tbl_Daily_Control_Reports_Summary
                                     {
                                         ReportName = g.Key.ReportName,
                                         Pending_Comments = g.Sum(s => s.Pending_Comments),
                                         Pending_Review = g.Sum(s => s.Pending_Review),
                                         Pending_Sign_Off = g.Sum(s => s.Pending_Sign_Off)
                                     }).ToArray();


                oDashboardModal.My_BCRSummary = OverallBCRSummary;
            }

            oDashboardModal.BacklogSummary = new Tbl_Backlog_Summary();
            oDashboardModal.BCRSummary = new Models.BCRSummary();

            lstBacklogstats = new List<Backlogstats>();
            lstBCRStats = new List<BCRstats>();

            //Bind Level 1 Summary Buckets

            if (Summary != null)
            {
                SetBacklogDashboardValues();
                SetBCRDashboardValues();
            }
            //End Binding

            // Bind Level 2 Summary Buckets
            switch (currentMainReport)
            {
                case "ControlReports":
                    DashboardModal tmpModel = oDashboardModal.BuildControlReportsSummaryforUI(oDashboardModal, BCRSummary, oSessionUser);
                    oDashboardModal.Pie_Model = tmpModel.Pie_Model;
                    oDashboardModal.Level2_Summary = tmpModel.Level2_Summary;
                    oDashboardModal.BCR_Level1_RawData = tmpModel.BCR_Level1_RawData;
                    break;

                #region Follow-ups
                case "Followups":

                    Tbl_Followup_Summary myfollowUP = oMyFollowups.FirstOrDefault();

                    oFollowupStats = new Backlogstats();
                    oFollowupStats.ReportName = "All Open Follow-ups";
                    oFollowupStats.Totalorders = Convert.ToDouble(myfollowUP.TotalFollowups);
                    oFollowupStats.Backcolor = "green";
                    oFollowupStats.Unit = " count";
                    oFollowupStats.Area = "Followup";

                    if (oSessionUser.ROLE_DESC == "CSR")
                        oFollowupStats.ReportPath = "MyFollowupsList";
                    else
                        oFollowupStats.ReportPath = "TeamFollowupsList";

                    lstBacklogstats.Add(oFollowupStats);

                    myfollowUP = oMyFollowups.FirstOrDefault();

                    oFollowupStats = new Backlogstats();
                    oFollowupStats.ReportName = "Passed due date";
                    oFollowupStats.Totalorders = Convert.ToDouble(myfollowUP.PassedDuedate);
                    oFollowupStats.Backcolor = "green";
                    oFollowupStats.Unit = " count";
                    oFollowupStats.Area = "Followup";

                    if (oSessionUser.ROLE_DESC == "CSR")
                        oFollowupStats.ReportPath = "MyPasseddueDate";
                    else
                        oFollowupStats.ReportPath = "TeamPasseddueDate";

                    lstBacklogstats.Add(oFollowupStats);

                    oFollowupStats = new Backlogstats();
                    oFollowupStats.ReportName = "Due Today";
                    oFollowupStats.Totalorders = Convert.ToDouble(myfollowUP.DueToday);
                    oFollowupStats.Backcolor = "green";
                    oFollowupStats.Unit = " count";
                    oFollowupStats.Area = "Followup";

                    if (oSessionUser.ROLE_DESC == "CSR")
                        oFollowupStats.ReportPath = "MyDueToday";
                    else
                        oFollowupStats.ReportPath = "TeamDueToday";

                    lstBacklogstats.Add(oFollowupStats);

                    oFollowupStats = new Backlogstats();
                    oFollowupStats.ReportName = "System Generated Follow-ups";
                    oFollowupStats.Totalorders = Convert.ToDouble(myfollowUP.SystemGenerated);
                    oFollowupStats.Backcolor = "green";
                    oFollowupStats.Unit = " count";
                    oFollowupStats.Area = "Followup";
                    oFollowupStats.ReportPath = "MysystemGenerated";
                    lstBacklogstats.Add(oFollowupStats);

                    oFollowupStats = new Backlogstats();
                    oFollowupStats.ReportName = "Re-Assigned Follow-ups";
                    oFollowupStats.Totalorders = Convert.ToDouble(myfollowUP.ReassignedtoYou);
                    oFollowupStats.Backcolor = "green";
                    oFollowupStats.Unit = " count";
                    oFollowupStats.Area = "Followup";
                    oFollowupStats.ReportPath = "MyReassigned";

                    lstBacklogstats.Add(oFollowupStats);

                    break;

                #endregion

                #region DB Orders
                case "DBOrders":

                    oDBStats = new Backlogstats();
                    oDBStats.ReportName = "All DB Orders";
                    oDBStats.Percentorders = Convert.ToDouble(((Summary.Total_DB / 1000000) / oDashboardModal.BacklogSummary
                        .Total_DB) * 100);
                    oDBStats.TotalDB_Cnt = Convert.ToInt32(Summary.Total_DB_Count);
                    oDBStats.OrdersWorth = Math.Round(Convert.ToDouble(Summary.Total_DB / 1000000), 2);
                    oDBStats.Unit = " (M$)";
                    oDBStats.Area = "DB";

                    if (oSessionUser.ROLE_DESC == "WW Lead")
                        oDBStats.ReportPath = "DB_Summary";
                    else
                        oDBStats.ReportPath = "DB_AllOrders";

                    lstBacklogstats.Add(oDBStats);

                    oDBStats = new Backlogstats();
                    oDBStats.ReportName = "Orders aged > 90 days";
                    oDBStats.Percentorders = Convert.ToDouble(((Summary.Total_DB_greater_90_Days / 1000000) / oDashboardModal.BacklogSummary
                        .Total_DB) * 100);
                    oDBStats.TotalDB_Cnt = Convert.ToInt32(Summary.Total_DB_greater_90_Days_Count);
                    oDBStats.OrdersWorth = Math.Round(Convert.ToDouble(Summary.Total_DB_greater_90_Days / 1000000), 2);
                    oDBStats.Unit = " (M$)";
                    oDBStats.Area = "DB";
                    oDBStats.ReportPath = "DB_AgedGreater90Days";

                    lstBacklogstats.Add(oDBStats);

                    oDBStats = new Backlogstats();
                    oDBStats.ReportName = "Passed Exp. Release";
                    oDBStats.Percentorders = Convert.ToDouble(((Summary.Total_DB_Expected_Released_Passed / 1000000) / oDashboardModal.BacklogSummary
                        .Total_DB) * 100);
                    oDBStats.TotalDB_Cnt = Convert.ToInt32(Summary.Total_DB_Expected_Released_Passed_Count);
                    oDBStats.OrdersWorth = Math.Round(Convert.ToDouble(Summary.Total_DB_Expected_Released_Passed / 1000000), 2);
                    oDBStats.Unit = " (M$)";
                    oDBStats.Area = "DB";
                    oDBStats.ReportPath = "DB_expectedreleasedatepassed";

                    lstBacklogstats.Add(oDBStats);

                    oDBStats = new Backlogstats();
                    oDBStats.ReportName = "DB Overdue";
                    oDBStats.Percentorders = Convert.ToDouble(((Summary.Total_DB_Overdue / 1000000) / oDashboardModal.BacklogSummary.Total_DB) * 100);
                    oDBStats.TotalDB_Cnt = Convert.ToInt32(Summary.Total_DB_Overdue_Count);
                    oDBStats.OrdersWorth = Math.Round(Convert.ToDouble(Summary.Total_DB_Overdue / 1000000), 2);
                    oDBStats.Unit = " (M$)";
                    oDBStats.Area = "DB";
                    oDBStats.ReportPath = "DB_OverDueOrders";

                    lstBacklogstats.Add(oDBStats);

                    oDBStats = new Backlogstats();
                    oDBStats.ReportName = "Become overdue within 14 days";
                    oDBStats.Percentorders = Convert.ToDouble(((Summary.Total_DB_Overdue_14_Days / 1000000) / oDashboardModal.BacklogSummary.Total_DB) * 100);
                    oDBStats.TotalDB_Cnt = Convert.ToInt32(Summary.Total_DB_Overdue_14_Days_Count);
                    oDBStats.OrdersWorth = Math.Round(Convert.ToDouble(Summary.Total_DB_Overdue_14_Days / 1000000), 2);
                    oDBStats.Unit = " (M$)";
                    oDBStats.Area = "DB";
                    oDBStats.ReportPath = "DB_becomeoverduewithin14days";

                    lstBacklogstats.Add(oDBStats);

                    oDBStats = new Backlogstats();
                    oDBStats.ReportName = "DB Speed to Revenue";
                    oDBStats.Percentorders = Convert.ToDouble(((Summary.Total_DB_SppedtoRevenue / 1000000) / oDashboardModal.BacklogSummary.Total_DB) * 100);
                    oDBStats.TotalDB_Cnt = Convert.ToInt32(Summary.Total_DB_SppedtoRevenue_Count);
                    oDBStats.OrdersWorth = Math.Round(Convert.ToDouble(Summary.Total_DB_SppedtoRevenue / 1000000), 2);
                    oDBStats.Unit = " (M$)";
                    oDBStats.Area = "DB";

                    if (oSessionUser.ROLE_DESC == "WW Lead")
                        oDBStats.ReportPath = "DBSpeedtoRevenueSummary";
                    else
                        oDBStats.ReportPath = "DBSpeedtoRevenue";

                    lstBacklogstats.Add(oDBStats);


                    break;
                #endregion

                #region SNI Orders
                case "SNIOrders":

                    oSNIStats = new Backlogstats();
                    oSNIStats.ReportName = "All SNI Orders";
                    oSNIStats.Percentorders = Convert.ToDouble(((Summary.Total_SNI / 1000000) / oDashboardModal.BacklogSummary.Total_SNI) * 100);
                    oSNIStats.TotalSNI_Cnt = Convert.ToInt32(oDashboardModal.BacklogSummary.Total_SNI_Count);
                    oSNIStats.OrdersWorth = Convert.ToDouble(oDashboardModal.BacklogSummary.Total_SNI);
                    oSNIStats.Unit = " (M$)";
                    oSNIStats.Area = "SNI";
                    if (oSessionUser.ROLE_DESC == "WW Lead")
                        oSNIStats.ReportPath = "SNI_Summary";
                    else
                        oSNIStats.ReportPath = "SNI_AllOrders";


                    lstBacklogstats.Add(oSNIStats);

                    oSNIStats = new Backlogstats();
                    oSNIStats.ReportName = "Aged SNI Orders";
                    oSNIStats.Percentorders = Convert.ToDouble(((Summary.Total_SNI_Aged / 1000000) / oDashboardModal.BacklogSummary.Total_SNI) * 100);
                    oSNIStats.TotalSNI_Cnt = Convert.ToInt32(oDashboardModal.BacklogSummary.Total_SNI_Aged_Count);
                    oSNIStats.OrdersWorth = Convert.ToDouble(oDashboardModal.BacklogSummary.Total_SNI_Aged);
                    oSNIStats.Unit = " (M$)";
                    oSNIStats.Area = "SNI";
                    oSNIStats.ReportPath = "SNI_AgedSNI";

                    lstBacklogstats.Add(oSNIStats);

                    oSNIStats = new Backlogstats();
                    oSNIStats.ReportName = "Due Today Exp. Release date ";
                    oSNIStats.Percentorders = Convert.ToDouble(((Summary.Total_SNI_Expected_Release_Today / 1000000) / oDashboardModal.BacklogSummary.Total_SNI) * 100);
                    oSNIStats.OrdersWorth = Convert.ToDouble(oDashboardModal.BacklogSummary.Total_SNI_Expected_Release_Today);
                    oSNIStats.TotalSNI_Cnt = Convert.ToInt32(oDashboardModal.BacklogSummary.Total_SNI_Expected_Release_Today_Count);
                    oSNIStats.Unit = " (M$)";
                    oSNIStats.Area = "SNI";
                    oSNIStats.ReportPath = "SNI_ReleaseDateDueTodayOrders";

                    lstBacklogstats.Add(oSNIStats);

                    oSNIStats = new Backlogstats();
                    oSNIStats.ReportName = "Passed Exp. Release date";
                    oSNIStats.Percentorders = Convert.ToDouble(((Summary.Total_SNI_Expected_Released_Passed / 1000000) / oDashboardModal.BacklogSummary.Total_SNI) * 100);
                    oSNIStats.OrdersWorth = Convert.ToDouble(oDashboardModal.BacklogSummary.Total_SNI_Expected_Released_Passed);
                    oSNIStats.TotalSNI_Cnt = Convert.ToInt32(oDashboardModal.BacklogSummary.Total_SNI_Expected_Released_Passed_Count);
                    oSNIStats.Unit = " (M$)";
                    oSNIStats.Area = "SNI";
                    oSNIStats.ReportPath = "SNI_ReleaseDatePassedOrders";

                    lstBacklogstats.Add(oSNIStats);

                    oSNIStats = new Backlogstats();
                    oSNIStats.ReportName = "No expected release dates";
                    oSNIStats.Percentorders = Convert.ToDouble(((Summary.Total_SNI_No_Expected_Release_Date / 1000000) / oDashboardModal.BacklogSummary.Total_SNI) * 100);
                    oSNIStats.OrdersWorth = Convert.ToDouble(oDashboardModal.BacklogSummary.Total_SNI_No_Expected_Release_Date);
                    oSNIStats.TotalSNI_Cnt = Convert.ToInt32(oDashboardModal.BacklogSummary.Total_SNI_No_Expected_Release_Date_Count);
                    oSNIStats.Unit = " (M$)";
                    oSNIStats.Area = "SNI";
                    oSNIStats.ReportPath = "SNI_NoReleaseDateOrders";

                    lstBacklogstats.Add(oSNIStats);

                    oSNIStats = new Backlogstats();
                    oSNIStats.ReportName = "Invoicing Errors";
                    oSNIStats.Percentorders = Convert.ToDouble(((Summary.Total_SNI_MF_Invoicing_Errors / 1000000) / oDashboardModal.BacklogSummary.Total_SNI) * 100);
                    oSNIStats.OrdersWorth = Convert.ToDouble(oDashboardModal.BacklogSummary.Total_SNI_MF_Invoicing_Errors);
                    oSNIStats.TotalSNI_Cnt = Convert.ToInt32(oDashboardModal.BacklogSummary.Total_SNI_MF_Invoicing_Errors_Count);
                    oSNIStats.Unit = " (M$)";
                    oSNIStats.Area = "SNI";
                    oSNIStats.ReportPath = "SNI_InvoicingErrorsOrders";

                    lstBacklogstats.Add(oSNIStats);
                    break;
                #endregion

                #region Unblocked Orders
                case "UnblockedOrders":

                    Backlogstats oUBStats = new Backlogstats();
                    oUBStats.ReportName = "All Unblocked";
                    oUBStats.Percentorders = Convert.ToDouble(((Summary.Total_Unblocked_Orders / 1000000) / oDashboardModal.BacklogSummary.Total_Unblocked_Orders) * 100);
                    oUBStats.CurrentTotalorders = 3000;
                    oUBStats.TotalUnblocked_Cnt = Convert.ToInt32(Summary.Total_Unblocked_Orders_Count);
                    oUBStats.OrdersWorth = Convert.ToDouble(oDashboardModal.BacklogSummary.Total_Unblocked_Orders);
                    oUBStats.Backcolor = "blue";
                    oUBStats.Unit = " (M$)";
                    oUBStats.Area = "UnBlocked";

                    if (oSessionUser.ROLE_DESC == "WW Lead")
                        oUBStats.ReportPath = "UB_Summary";
                    else
                        oUBStats.ReportPath = "AllUnblocked";


                    lstBacklogstats.Add(oUBStats);

                    oUBStats = new Backlogstats();
                    oUBStats.ReportName = "Unblocked Overdue";
                    oUBStats.Percentorders = Convert.ToDouble(((Summary.Total_Unblocked_Overdue / 1000000) / oDashboardModal.BacklogSummary.Total_Unblocked_Orders) * 100);
                    oUBStats.CurrentTotalorders = 200;
                    oUBStats.TotalUnblocked_Cnt = Convert.ToInt32(Summary.Total_Unblocked_Overdue_Count);
                    oUBStats.OrdersWorth = Convert.ToDouble(oDashboardModal.BacklogSummary.Total_Unblocked_Overdue);
                    oUBStats.Backcolor = "blue";
                    oUBStats.Unit = " (M$)";
                    oUBStats.Area = "UnBlocked";
                    oUBStats.ReportPath = "AllUnblockedOverdue";


                    lstBacklogstats.Add(oUBStats);

                    break;
                #endregion

                #region OSBR Notifications
                case "OSBRReports":

                    Backlogstats oOSBRStats = new Backlogstats();
                    oOSBRStats.ReportName = "All OSBR Notifications";
                    oOSBRStats.CurrentTotalorders = 5;
                    oOSBRStats.Totalorders = 3000;
                    oOSBRStats.Percentorders = Convert.ToDouble(((Summary.Total_OSBR_Notification / 1000000) / oDashboardModal.BacklogSummary.Total_OSBR_Notification) * 100);
                    oOSBRStats.TotalOSBR_Cnt = Convert.ToInt32(Summary.Total_OSBR_Notification_Count);
                    oOSBRStats.OrdersWorth = Convert.ToDouble(oDashboardModal.BacklogSummary.Total_OSBR_Notification);
                    oOSBRStats.Backcolor = "pink";
                    oOSBRStats.Unit = " (M$)";
                    oOSBRStats.Area = "OSBR";
                    oOSBRStats.ReportPath = "Index";

                    lstBacklogstats.Add(oOSBRStats);

                    break;
                #endregion
            }

            // Forcing code to Refresh control reprots summary on each Dashboard refresh along with individual bucket refresh requested.
            if (currentMainReport != "ControlReports")
            {
                DashboardModal tmpModel = oDashboardModal.BuildControlReportsSummaryforUI(oDashboardModal, BCRSummary, oSessionUser);
                oDashboardModal.Pie_Model = tmpModel.Pie_Model;
                oDashboardModal.Level2_Summary = tmpModel.Level2_Summary;
                oDashboardModal.BCR_Level1_RawData = tmpModel.BCR_Level1_RawData;
            }
            
            //End Level 2 Binding

            oDashboardModal.BacklogStats = lstBacklogstats;

            unmappedSummery = UM_service.GetUnMappedUsersByRegion(oSessionUser.SUPERREGION);
            //Session["unmappedSummery"] = unmappedSummery;
            var UnMappedOrderCount = unmappedSummery.Where(w => w.USERGROUP == "COPC CSR").Select(p => p.Order_Count).Sum();
            Session["UnMappedOrderCount"] = UnMappedOrderCount;
            var UnMappedNetValue = unmappedSummery.Where(w => w.USERGROUP == "COPC CSR").Select(p => p.Total_NV).Sum();
            Session["UnMappedNetValue"] = UnMappedNetValue;
            var UnMappedUserCount = unmappedSummery.Where(w => w.USERGROUP == "COPC CSR").Select(p => p.TotalUser).Sum(); //
            Session["UnMappedUserCount"] = UnMappedUserCount;

            Session["lstControlReports"] = oDashboardModal.BacklogStats;
        }

        public ActionResult SearchOrder(string SalesOrder)
        {
            ViewBag.SalesOrder = SalesOrder;
            List<Tbl_Order_Search> result = new List<Tbl_Order_Search>();

            try
            {
                result = service.Search_Global_Order(Convert.ToDouble(SalesOrder)).ToList();
            }
            catch (Exception ex)
            {

            }

            ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
            return View(result);
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Next Generation Cockpit - Home.";


            if (Session["UserProfile"] != null)
            {
                oSessionUser = (VW_USERS)Session["UserProfile"];
                oMyFollowups = (Tbl_Followup_Summary[])Session["MyFollowupSummary"];
                //oCatagoryFollowups = (VW_Catagory_Followups[])Session["CatagoryFollowups"];
                lstRolebasedCockpit = (List<SelectListItem>)Session["lstRolebasedCockpit"];
                oDashboardModal = (DashboardModal)Session["oDashboardModal"];

                if (Session["currentMainReport"] != null)
                {
                    currentMainReport = Session["currentMainReport"].ToString();
                    ViewBag.currentMainReport = currentMainReport;
                }
            }

            //Set Default Cockpit UI
            if (Session["CockpitUI"] == null) Session["CockpitUI"] = "CSR Cockpit";

            if (oSessionUser.ROLE_DESC == "CSR")
            {
                ViewBag.CurrentDashboard = Session["CockpitUI"].ToString();
                ViewBag.Dashboardlist = lstRolebasedCockpit;
            }

            if (oSessionUser.ROLE_DESC == "CSR" || oSessionUser.ROLE_DESC == "Supervisor" || oSessionUser.ROLE_DESC == "BPA")
            {
                ViewBag.ReportGroup = "BCRReport";
            }
            else
                ViewBag.ReportGroup = "BacklogReport";

            BuildDashboadModel(ViewBag.ReportGroup, currentMainReport, oDashboardModal.UI_Type);

            ViewBag.UserRole = oSessionUser.ROLE_DESC;
            ViewData["oDashboardModal"] = oDashboardModal;

            return View(oDashboardModal);
        }

        void Set_SessionUserProfile(string NtLogin)
        {
            oSessionUserProfiles = UM_service.GetUserDetails(NtLogin).ToList();

            if (oSessionUserProfiles.Count == 0)
            {
                Response.Redirect("~/USer/UserLogin");
            }
            else
            {
                oSessionUser = oSessionUserProfiles.FirstOrDefault();

                var RolesList = oSessionUserProfiles.Select(p => p.ROLE_DESC).Distinct();


                lstRoles = new List<SelectListItem>();
                foreach (string Role in RolesList)
                {
                    lstRoles.Add(new SelectListItem() { Text = Role, Value = Role, Selected = false });
                }


                ViewBag.RolesList = lstRoles;
                Session.Clear();

                var TeamProfile = UM_service.GetUserTeamDetails(oSessionUser.TEAM_NAME);
                var UniqueMembers = (from tbl in TeamProfile group tbl by new { tbl.FULLNAME } into g select new Model_Pie { category = g.Key.FULLNAME }).ToList();



                Session["TeamProfile"] = UniqueMembers;

                Session["lstRoles"] = lstRoles;
                Session["UserProfile"] = oSessionUser;
                Session["ntlogin"] = User.Identity.Name.Replace("SPL\\", "");
            }
        }

        [HttpPost]
        public ActionResult Index(FormCollection objCollection)
        {

            oSessionUser = (VW_USERS)Session["UserProfile"];

            #region Role Access Check & Default UI Assignment

            if (objCollection["ntlogin"] != null && (oSessionUser.ROLE_DESC != objCollection["Role"] || oSessionUser.NTLOGIN != objCollection["ntlogin"]))
            {
                string ntlogin = objCollection["ntlogin"];
                Set_SessionUserProfile(ntlogin);

                oSessionUser = (VW_USERS)Session["UserProfile"];

                var TeamProfile = UM_service.GetUserTeamDetails(oSessionUser.TEAM_NAME);
                var UniqueMembers = (from tbl in TeamProfile group tbl by new { tbl.FULLNAME, tbl.NTLOGIN } into g select new Model_Pie { category = g.Key.FULLNAME, Color = g.Key.NTLOGIN }).ToList();

                var UniqueCSRs = (from tbl in TeamProfile where tbl.ROLE_DESC == "CSR" group tbl by new { tbl.FULLNAME, tbl.NTLOGIN } into g select new Model_Pie { category = g.Key.FULLNAME, Color = g.Key.NTLOGIN }).ToList();

                Session["TeamProfile"] = UniqueMembers;
                Session["UniqueCSRs"] = UniqueCSRs;
            }
            else
            {
                if (Session["CockpitUI"] != null)
                    CockpitUI = Session["CockpitUI"].ToString();
                else
                {
                    //Set Default Cockpit UI
                    if (oSessionUser.ROLE_DESC == "CSR")
                        CockpitUI = "CSR Cockpit";
                    else
                        CockpitUI = "Team Cockpit";

                    Session["CockpitUI"] = CockpitUI;
                }
            }

            if (oSessionUser.ROLE_DESC == "CSR" || oSessionUser.ROLE_DESC == "Supervisor" || oSessionUser.ROLE_DESC == "BPA")
            {
                ViewBag.ReportGroup = "BCRReport";
            }
            else
                ViewBag.ReportGroup = "BacklogReport";

            if (!objCollection.AllKeys.Contains("hidReportName") && ViewBag.ReportGroup == "BCRReport")
            {
                if (oSessionUser.ROLE_DESC == "Supervisor" || oSessionUser.ROLE_DESC == "BPA")
                {
                    //  Default Supervisor / BPA Load of Report;
                    currentMainReport = "ControlReports";
                    CockpitUI = "Team Cockpit";
                    ViewBag.CurrentDashboard = CockpitUI;
                }
                else
                {
                    //  Default CSR Load of Report;
                    currentMainReport = "Followups";
                    CockpitUI = "CSR Cockpit";
                    ViewBag.CurrentDashboard = CockpitUI;
                }
            }
            else if (!objCollection.AllKeys.Contains("hidReportName") && ViewBag.ReportGroup == "BacklogReport")
            {
                // Default Load of Report;
                currentMainReport = "DBOrders";
            }
            else if (objCollection.AllKeys.Contains("hidReportName"))
            {
                if (CockpitUI != objCollection[0].ToString())
                {
                    // VW_USERS Selected Report
                    CockpitUI = objCollection[0].ToString();

                    //reset session Variable
                    Session["CockpitUI"] = CockpitUI;
                }

                currentMainReport = objCollection["hidReportName"].ToString();
                //currentMainReport = "ControlReports";
                ViewBag.CurrentDashboard = CockpitUI;
            }
            else
            {
                currentMainReport = objCollection[1].ToString(); //Followup Key
                ViewBag.CurrentDashboard = CockpitUI;
            }



            #endregion


            if (ViewBag.ReportGroup == "BacklogReport")
            {
                BuildDashboadModel("BacklogReport", currentMainReport, CockpitUI);
            }
            else if (ViewBag.ReportGroup == "BCRReport")
            {
                BuildDashboadModel("BCRReport", currentMainReport, CockpitUI);
            }


            ViewBag.Message = "Next Generation Cockpit - Home.";
            ViewBag.LastDataRefresh = oDashboardModal.LastRefreshDate;

            ViewBag.currentMainReport = currentMainReport;
            Session["currentMainReport"] = currentMainReport;
            Session["oDashboardModal"] = oDashboardModal;

            if (currentMainReport == "ControlReports")
            {
                ViewData["PendingComments"] = oDashboardModal.Level2_Summary.PendingComments;
                ViewData["PendingReview"] = oDashboardModal.Level2_Summary.PendingReview;
                ViewData["PendingSignOff"] = oDashboardModal.Level2_Summary.PendingSignOff;
                ViewData["UniqueReports"] = oDashboardModal.Level2_Summary.UniqueReports;
                ViewData["RawData"] = oDashboardModal.BCR_Level1_RawData;

                ViewData["PendingComments2"] = oDashboardModal.Level2_Summary.PendingComments2;
                ViewData["PendingReview2"] = oDashboardModal.Level2_Summary.PendingReview2;
                ViewData["PendingSignOff2"] = oDashboardModal.Level2_Summary.PendingSignOff2;
                ViewData["UniqueReports2"] = oDashboardModal.Level2_Summary.UniqueReports2;
                ViewData["RawData"] = oDashboardModal.BCR_Level2_RawData;

                ViewBag.DefaultReport = oDashboardModal.Level2_Default_Report;

                ViewData["lstReportsMaster"] = oDashboardModal.lstReportsMaster;
            }

            ViewData["oDashboardModal"] = oDashboardModal;
            return View(oDashboardModal);
        }

        private void SetBacklogDashboardValues()
        {
            oDashboardModal.LastRefreshDate = Summary.SnapshotDate.ToString();

            oDashboardModal.BacklogSummary.Total_SNI = Math.Round((Convert.ToDouble(Summary.Total_SNI) / 1000000), 2);
            oDashboardModal.BacklogSummary.Total_SNI_Count = Convert.ToInt32(Summary.Total_SNI_Count);

            oDashboardModal.BacklogSummary.Total_SNI_Aged = Math.Round((Convert.ToDouble(Summary.Total_SNI_Aged) / 1000000), 2);
            oDashboardModal.BacklogSummary.Total_SNI_Aged_Count = Convert.ToInt32(Summary.Total_SNI_Aged_Count);

            oDashboardModal.BacklogSummary.Total_SNI_Expected_Release_Today = Math.Round((Convert.ToDouble(Summary.Total_SNI_Expected_Release_Today) / 1000000), 2);
            oDashboardModal.BacklogSummary.Total_SNI_Expected_Release_Today_Count = Convert.ToInt32(Summary.Total_SNI_Expected_Release_Today_Count);

            oDashboardModal.BacklogSummary.Total_SNI_Expected_Released_Passed = Math.Round((Convert.ToDouble(Summary.Total_SNI_Expected_Released_Passed) / 1000000), 2);
            oDashboardModal.BacklogSummary.Total_SNI_Expected_Released_Passed_Count = Convert.ToInt32(Summary.Total_SNI_Expected_Released_Passed_Count);

            oDashboardModal.BacklogSummary.Total_SNI_MF_Invoicing_Errors = Math.Round((Convert.ToDouble(Summary.Total_SNI_MF_Invoicing_Errors) / 1000000), 2);
            oDashboardModal.BacklogSummary.Total_SNI_MF_Invoicing_Errors_Count = Convert.ToInt32(Summary.Total_SNI_MF_Invoicing_Errors_Count);

            oDashboardModal.BacklogSummary.Total_SNI_No_Expected_Release_Date = Math.Round((Convert.ToDouble(Summary.Total_SNI_No_Expected_Release_Date) / 1000000), 2);
            oDashboardModal.BacklogSummary.Total_SNI_No_Expected_Release_Date_Count = Convert.ToInt32(Summary.Total_SNI_No_Expected_Release_Date_Count);

            oDashboardModal.BacklogSummary.Total_DB = Math.Round((Convert.ToDouble(Summary.Total_DB) / 1000000), 2);
            oDashboardModal.BacklogSummary.Total_DB_Count = Convert.ToInt32(Summary.Total_DB_Count);

            oDashboardModal.BacklogSummary.Total_DB_Overdue = Math.Round(Convert.ToDouble((Summary.Total_DB_Overdue) / 1000000), 2);
            oDashboardModal.BacklogSummary.Total_DB_Overdue_Count = Convert.ToInt32(Summary.Total_DB_Overdue_Count);

            oDashboardModal.BacklogSummary.Total_DB_Expected_Released_Passed = Math.Round(Convert.ToDouble((Summary.Total_DB_Expected_Released_Passed) / 1000000), 2);
            oDashboardModal.BacklogSummary.Total_DB_Expected_Released_Passed_Count = Convert.ToInt32(Summary.Total_DB_Expected_Released_Passed_Count);

            oDashboardModal.BacklogSummary.Total_DB_greater_90_Days = Math.Round(Convert.ToDouble((Summary.Total_DB_greater_90_Days) / 1000000), 2);
            oDashboardModal.BacklogSummary.Total_DB_greater_90_Days_Count = Convert.ToInt32(Summary.Total_DB_greater_90_Days_Count);

            oDashboardModal.BacklogSummary.Total_DB_Overdue_14_Days = Math.Round(Convert.ToDouble((Summary.Total_DB_Overdue_14_Days) / 1000000), 2);
            oDashboardModal.BacklogSummary.Total_DB_Overdue_14_Days_Count = Convert.ToInt32(Summary.Total_DB_Overdue_14_Days_Count);

            oDashboardModal.BacklogSummary.Total_DB_SppedtoRevenue = Math.Round(Convert.ToDouble((Summary.Total_DB_SppedtoRevenue) / 1000000), 2);
            oDashboardModal.BacklogSummary.Total_DB_SppedtoRevenue_Count = Convert.ToInt32(Summary.Total_DB_SppedtoRevenue_Count);

            oDashboardModal.BacklogSummary.Total_Unblocked_Orders = Math.Round(Convert.ToDouble((Summary.Total_Unblocked_Orders) / 1000000), 2);
            oDashboardModal.BacklogSummary.Total_Unblocked_Orders_Count = Convert.ToInt32(Summary.Total_Unblocked_Orders_Count);

            oDashboardModal.BacklogSummary.Total_Unblocked_Overdue = Math.Round(Convert.ToDouble((Summary.Total_Unblocked_Overdue) / 1000000), 2);
            oDashboardModal.BacklogSummary.Total_Unblocked_Overdue_Count = Convert.ToInt32(Summary.Total_Unblocked_Overdue_Count);

            oDashboardModal.BacklogSummary.Total_OSBR_Notification = Math.Round((Convert.ToDouble(Summary.Total_OSBR_Notification) / 1000000), 2);
            oDashboardModal.BacklogSummary.Total_OSBR_Notification_Count = Convert.ToInt32(Summary.Total_OSBR_Notification_Count);

            ViewBag.UserRole = oSessionUser.ROLE_DESC;
        }

        private void SetBCRDashboardValues()
        {
            oDashboardModal.BCRSummary.TotalPendingOrders = Convert.ToInt32(BCRSummary.Select(p => p.Pending_Comments).Sum());
            oDashboardModal.BCRSummary.TotalPendingSignOffs = Convert.ToInt32(BCRSummary.Select(p => p.Pending_Sign_Off).Sum());
            oDashboardModal.BCRSummary.TotalPendingReviews = Convert.ToInt32(BCRSummary.Select(p => p.Pending_Review).Sum());

            ViewBag.UserRole = oSessionUser.ROLE_DESC;
        }

    }
}
