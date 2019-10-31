using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using B_Web_App.Models;

namespace B_Web_App.Controllers
{
    public class AnalyticsController : Controller
    {
        private CNG_Azure service = new CNG_Azure();

        VW_USERS oSessionUser;
        string CockpitUI;

        public ActionResult DB_WaterFall()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            ViewBag.UserRole = oSessionUser.ROLE_DESC;
            List<Release_Summary> listStats = new List<Release_Summary>();
            List<Tbl_SNI_Release_Projection> lst_GraphData_Raw = new List<Tbl_SNI_Release_Projection>();

            string FromDate = System.DateTime.Now.AddMonths(-2).ToShortDateString();
            string ToDate = System.DateTime.Now.AddMonths(2).ToShortDateString();

            if (!ViewData.Keys.Contains("lst_GraphData_Raw"))
            {
                if (oSessionUser.ROLE_DESC == "BPA" || oSessionUser.ROLE_DESC == "Regional Lead")
                {
                    #region BPA Chart

                    //lst_GraphData_Raw = service.DB_Release_Target_Projection(FromDate, ToDate, oSessionUser.SUPERREGION, "").ToList();
                    List<Release_Summary> lst_GraphData_Main_Summary = (from tbl in lst_GraphData_Raw
                                                                        group tbl by new { tbl.Feedback.Value, tbl.REGION, tbl.Feedback_Calc_Est_Invoice_dt_Month } into g
                                                                        select new Release_Summary
                                                                        {
                                                                            Date = g.Key.Value,
                                                                            Year = (Convert.ToDateTime(g.Key.Value).Year < System.DateTime.Now.Year || Convert.ToDateTime(g.Key.Value).Year == 1900) ? "TBC" : Convert.ToDateTime(g.Key.Value).Year.ToString(),
                                                                            Month = g.Key.Feedback_Calc_Est_Invoice_dt_Month,
                                                                            Region = g.Key.REGION,
                                                                            value = g.Sum(s => s.BAcklog)
                                                                        }).ToList();

                    List<Release_Summary> lst_GraphData_Consolidation = (from tbl in lst_GraphData_Main_Summary
                                                                         orderby tbl.Date
                                                                         group tbl by new { tbl.Month, tbl.Year, tbl.Region } into g
                                                                         select new Release_Summary
                                                                         {
                                                                             Date = (g.Key.Year == "TBC" || g.Key.Month == "TBC") ? Convert.ToDateTime("1/1/1900") : Convert.ToDateTime("1-" + g.Key.Month + "-" + g.Key.Year),
                                                                             Month = g.Key.Year == "TBC" ? "TBC" : g.Key.Month + "-" + g.Key.Year,
                                                                             Region = g.Key.Region,
                                                                             value = g.Sum(s => s.value)
                                                                         }).ToList<Release_Summary>();


                    listStats = (from tbl in lst_GraphData_Consolidation
                                 orderby tbl.Date
                                 group tbl by new { tbl.Month, tbl.Year, tbl.Region } into g
                                 select new Release_Summary
                                 {
                                     Month = g.Key.Month,
                                     Region = g.Key.Region,
                                     value = g.Sum(s => s.value)
                                 }).ToList<Release_Summary>();


                    //Build Details chart by Report Name
                    List<Model_Bars> oDetailsResult = new List<Model_Bars>();
                    string[] Months = listStats.Select(p => p.Month).Distinct().ToArray();
                    double[] ProjectedValue = listStats.Select(p => p.value).ToArray();

                    ViewData["Months"] = Months;
                    ViewData["ProjectedValue"] = ProjectedValue;

                    ViewData["RawData"] = lst_GraphData_Raw;
                    #endregion
                }
                else if (oSessionUser.ROLE_DESC == "Supervisor" || oSessionUser.ROLE_DESC == "CSR")
                {
                    #region Supervisor Chart

                    //lst_GraphData_Raw = service.DB_Release_Target_Projection(FromDate, ToDate, "", oSessionUser.TEAM_NAME).ToList();
                    List<Release_Summary> lst_GraphData_Main_Summary = (from tbl in lst_GraphData_Raw
                                                                        group tbl by new { tbl.Feedback.Value, tbl.TEAM_NAME, tbl.Feedback_Calc_Est_Invoice_dt_Month } into g
                                                                        select new Release_Summary
                                                                        {
                                                                            Date = g.Key.Value,
                                                                            Year = (Convert.ToDateTime(g.Key.Value).Year < System.DateTime.Now.Year || Convert.ToDateTime(g.Key.Value).Year == 1900) ? "TBC" : Convert.ToDateTime(g.Key.Value).Year.ToString(),
                                                                            Month = g.Key.Feedback_Calc_Est_Invoice_dt_Month,
                                                                            Region = g.Key.TEAM_NAME,
                                                                            value = g.Sum(s => s.BAcklog)
                                                                        }).ToList();

                    List<Release_Summary> lst_GraphData_Consolidation = (from tbl in lst_GraphData_Main_Summary
                                                                         orderby tbl.Date
                                                                         group tbl by new { tbl.Month, tbl.Year, tbl.Region } into g
                                                                         select new Release_Summary
                                                                         {
                                                                             Date = (g.Key.Year == "TBC" || g.Key.Month == "TBC") ? Convert.ToDateTime("1/1/1900") : Convert.ToDateTime("1-" + g.Key.Month + "-" + g.Key.Year),
                                                                             Month = g.Key.Year == "TBC" ? "TBC" : g.Key.Month + "-" + g.Key.Year,
                                                                             Region = g.Key.Region,
                                                                             value = g.Sum(s => s.value)
                                                                         }).ToList<Release_Summary>();


                    listStats = (from tbl in lst_GraphData_Consolidation
                                 orderby tbl.Date
                                 group tbl by new { tbl.Month, tbl.Year, tbl.Region } into g
                                 select new Release_Summary
                                 {
                                     Month = g.Key.Month,
                                     Region = g.Key.Region,
                                     value = g.Sum(s => s.value)
                                 }).ToList<Release_Summary>();


                    //Build Details chart by Report Name
                    List<Model_Bars> oDetailsResult = new List<Model_Bars>();
                    string[] Months = listStats.Select(p => p.Month).Distinct().ToArray();
                    double[] ProjectedValue = listStats.Select(p => p.value).ToArray();

                    ViewData["Months"] = Months;
                    ViewData["ProjectedValue"] = ProjectedValue;

                    ViewData["RawData"] = lst_GraphData_Raw;
                    #endregion
                }
                else
                {
                    // WW Lead Case
                    #region WW lead Chart

                    //lst_GraphData_Raw = service.DB_Release_Target_Projection(FromDate, ToDate, "", "").ToList();
                    List<Release_Summary> lst_GraphData_Main_Summary = (from tbl in lst_GraphData_Raw
                                                                        group tbl by new { tbl.Feedback.Value, tbl.REGION, tbl.Feedback_Calc_Est_Invoice_dt_Month } into g
                                                                        select new Release_Summary
                                                                        {
                                                                            Date = g.Key.Value,
                                                                            Year = (Convert.ToDateTime(g.Key.Value).Year < System.DateTime.Now.Year || Convert.ToDateTime(g.Key.Value).Year == 1900) ? "TBC" : Convert.ToDateTime(g.Key.Value).Year.ToString(),
                                                                            Month = g.Key.Feedback_Calc_Est_Invoice_dt_Month,
                                                                            Region = g.Key.REGION,
                                                                            value = g.Sum(s => s.BAcklog)
                                                                        }).ToList();

                    List<Release_Summary> lst_GraphData_Consolidation = (from tbl in lst_GraphData_Main_Summary
                                                                         orderby tbl.Date
                                                                         group tbl by new { tbl.Month, tbl.Year, tbl.Region } into g
                                                                         select new Release_Summary
                                                                         {
                                                                             Date = (g.Key.Year == "TBC" || g.Key.Month == "TBC") ? Convert.ToDateTime("1/1/1900") : Convert.ToDateTime("1-" + g.Key.Month + "-" + g.Key.Year),
                                                                             Month = g.Key.Year == "TBC" ? "TBC" : g.Key.Month + "-" + g.Key.Year,
                                                                             Region = g.Key.Region,
                                                                             value = g.Sum(s => s.value)
                                                                         }).ToList<Release_Summary>();


                    listStats = (from tbl in lst_GraphData_Consolidation
                                 orderby tbl.Date
                                 group tbl by new { tbl.Month, tbl.Year, tbl.Region } into g
                                 select new Release_Summary
                                 {
                                     Month = g.Key.Month,
                                     Region = g.Key.Region,
                                     value = g.Sum(s => s.value)
                                 }).ToList<Release_Summary>();


                    //Build Details chart by Report Name
                    List<Model_Bars> oDetailsResult = new List<Model_Bars>();
                    string[] Months = listStats.Select(p => p.Month).Distinct().ToArray();
                    double[] ProjectedValue_AFO = listStats.Where(p => p.Region == "AFO").Select(p => p.value).ToArray();
                    double[] ProjectedValue_EMEAI = listStats.Where(p => p.Region == "EMEA-I").Select(p => p.value).ToArray();
                    double[] ProjectedValue_SAPK = listStats.Where(p => p.Region == "SAPK").Select(p => p.value).ToArray();
                    double[] ProjectedValue_GCFO = listStats.Where(p => p.Region == "GCFO").Select(p => p.value).ToArray();
                    double[] ProjectedValue_JFO = listStats.Where(p => p.Region == "JFO").Select(p => p.value).ToArray();

                    ViewData["Months"] = Months;
                    ViewData["ProjectedValue_AFO"] = ProjectedValue_AFO;
                    ViewData["ProjectedValue_EMEAI"] = ProjectedValue_EMEAI;
                    ViewData["ProjectedValue_SAPK"] = ProjectedValue_SAPK;
                    ViewData["ProjectedValue_GCFO"] = ProjectedValue_GCFO;
                    ViewData["ProjectedValue_JFO"] = ProjectedValue_JFO;

                    ViewData["RawData"] = lst_GraphData_Raw;
                    #endregion
                }

                ViewData.Add("FromDate", FromDate);
                ViewData.Add("ToDate", ToDate);

                ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
                return View(lst_GraphData_Raw);
            }
            else
            {
                ViewData.Add("FromDate", FromDate);
                ViewData.Add("ToDate", ToDate);

                ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
                return View(ViewData["lst_GraphData_Raw"]);
            }
        }

        [HttpPost]
        public ActionResult DB_WaterFall(FormCollection objCollection)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            ViewBag.UserRole = oSessionUser.ROLE_DESC;
            List<Release_Summary> listStats = new List<Release_Summary>();
            List<Tbl_SNI_Release_Projection> lst_GraphData_Raw = new List<Tbl_SNI_Release_Projection>();

            string FromDate = objCollection["Chart_Range_From"].ToString();
            string ToDate = objCollection["Chart_Range_To"].ToString();


            if (!ViewData.Keys.Contains("lst_GraphData_Raw"))
            {
                if (oSessionUser.ROLE_DESC == "BPA" || oSessionUser.ROLE_DESC == "Regional Lead")
                {
                    #region BPA Chart

                    //lst_GraphData_Raw = service.DB_Release_Target_Projection(FromDate, ToDate, oSessionUser.SUPERREGION, "").ToList();
                    List<Release_Summary> lst_GraphData_Main_Summary = (from tbl in lst_GraphData_Raw
                                                                        group tbl by new { tbl.Feedback.Value, tbl.REGION, tbl.Feedback_Calc_Est_Invoice_dt_Month } into g
                                                                        select new Release_Summary
                                                                        {
                                                                            Date = g.Key.Value,
                                                                            Year = (Convert.ToDateTime(g.Key.Value).Year < System.DateTime.Now.Year || Convert.ToDateTime(g.Key.Value).Year == 1900) ? "TBC" : Convert.ToDateTime(g.Key.Value).Year.ToString(),
                                                                            Month = g.Key.Feedback_Calc_Est_Invoice_dt_Month,
                                                                            Region = g.Key.REGION,
                                                                            value = g.Sum(s => s.BAcklog)
                                                                        }).ToList();

                    List<Release_Summary> lst_GraphData_Consolidation = (from tbl in lst_GraphData_Main_Summary
                                                                         orderby tbl.Date
                                                                         group tbl by new { tbl.Month, tbl.Year, tbl.Region } into g
                                                                         select new Release_Summary
                                                                         {
                                                                             Date = (g.Key.Year == "TBC" || g.Key.Month == "TBC") ? Convert.ToDateTime("1/1/1900") : Convert.ToDateTime("1-" + g.Key.Month + "-" + g.Key.Year),
                                                                             Month = g.Key.Year == "TBC" ? "TBC" : g.Key.Month + "-" + g.Key.Year,
                                                                             Region = g.Key.Region,
                                                                             value = g.Sum(s => s.value)
                                                                         }).ToList<Release_Summary>();


                    listStats = (from tbl in lst_GraphData_Consolidation
                                 orderby tbl.Date
                                 group tbl by new { tbl.Month, tbl.Year, tbl.Region } into g
                                 select new Release_Summary
                                 {
                                     Month = g.Key.Month,
                                     Region = g.Key.Region,
                                     value = g.Sum(s => s.value)
                                 }).ToList<Release_Summary>();


                    //Build Details chart by Report Name
                    List<Model_Bars> oDetailsResult = new List<Model_Bars>();
                    string[] Months = listStats.Select(p => p.Month).Distinct().ToArray();
                    double[] ProjectedValue = listStats.Select(p => p.value).ToArray();

                    ViewData["Months"] = Months;
                    ViewData["ProjectedValue"] = ProjectedValue;

                    ViewData["RawData"] = lst_GraphData_Raw;
                    #endregion
                }
                else if (oSessionUser.ROLE_DESC == "Supervisor" || oSessionUser.ROLE_DESC == "CSR")
                {
                    #region Supervisor Chart

                    //lst_GraphData_Raw = service.DB_Release_Target_Projection(FromDate, ToDate, "", oSessionUser.TEAM_NAME).ToList();
                    List<Release_Summary> lst_GraphData_Main_Summary = (from tbl in lst_GraphData_Raw
                                                                        group tbl by new { tbl.Feedback.Value, tbl.TEAM_NAME, tbl.Feedback_Calc_Est_Invoice_dt_Month } into g
                                                                        select new Release_Summary
                                                                        {
                                                                            Date = g.Key.Value,
                                                                            Year = (Convert.ToDateTime(g.Key.Value).Year < System.DateTime.Now.Year || Convert.ToDateTime(g.Key.Value).Year == 1900) ? "TBC" : Convert.ToDateTime(g.Key.Value).Year.ToString(),
                                                                            Month = g.Key.Feedback_Calc_Est_Invoice_dt_Month,
                                                                            Region = g.Key.TEAM_NAME,
                                                                            value = g.Sum(s => s.BAcklog)
                                                                        }).ToList();

                    List<Release_Summary> lst_GraphData_Consolidation = (from tbl in lst_GraphData_Main_Summary
                                                                         orderby tbl.Date
                                                                         group tbl by new { tbl.Month, tbl.Year, tbl.Region } into g
                                                                         select new Release_Summary
                                                                         {
                                                                             Date = (g.Key.Year == "TBC" || g.Key.Month == "TBC") ? Convert.ToDateTime("1/1/1900") : Convert.ToDateTime("1-" + g.Key.Month + "-" + g.Key.Year),
                                                                             Month = g.Key.Year == "TBC" ? "TBC" : g.Key.Month + "-" + g.Key.Year,
                                                                             Region = g.Key.Region,
                                                                             value = g.Sum(s => s.value)
                                                                         }).ToList<Release_Summary>();


                    listStats = (from tbl in lst_GraphData_Consolidation
                                 orderby tbl.Date
                                 group tbl by new { tbl.Month, tbl.Year, tbl.Region } into g
                                 select new Release_Summary
                                 {
                                     Month = g.Key.Month,
                                     Region = g.Key.Region,
                                     value = g.Sum(s => s.value)
                                 }).ToList<Release_Summary>();


                    //Build Details chart by Report Name
                    List<Model_Bars> oDetailsResult = new List<Model_Bars>();
                    string[] Months = listStats.Select(p => p.Month).Distinct().ToArray();
                    double[] ProjectedValue = listStats.Select(p => p.value).ToArray();

                    ViewData["Months"] = Months;
                    ViewData["ProjectedValue"] = ProjectedValue;

                    ViewData["RawData"] = lst_GraphData_Raw;
                    #endregion
                }
                else
                {
                    // WW Lead Case
                    #region WW lead Chart

                    //lst_GraphData_Raw = service.DB_Release_Target_Projection(FromDate, ToDate, "", "").ToList();
                    List<Release_Summary> lst_GraphData_Main_Summary = (from tbl in lst_GraphData_Raw
                                                                        group tbl by new { tbl.Feedback.Value, tbl.REGION, tbl.Feedback_Calc_Est_Invoice_dt_Month } into g
                                                                        select new Release_Summary
                                                                        {
                                                                            Date = g.Key.Value,
                                                                            Year = (Convert.ToDateTime(g.Key.Value).Year < System.DateTime.Now.Year || Convert.ToDateTime(g.Key.Value).Year == 1900) ? "TBC" : Convert.ToDateTime(g.Key.Value).Year.ToString(),
                                                                            Month = g.Key.Feedback_Calc_Est_Invoice_dt_Month,
                                                                            Region = g.Key.REGION,
                                                                            value = g.Sum(s => s.BAcklog)
                                                                        }).ToList();

                    List<Release_Summary> lst_GraphData_Consolidation = (from tbl in lst_GraphData_Main_Summary
                                                                         orderby tbl.Date
                                                                         group tbl by new { tbl.Month, tbl.Year, tbl.Region } into g
                                                                         select new Release_Summary
                                                                         {
                                                                             Date = (g.Key.Year == "TBC" || g.Key.Month == "TBC") ? Convert.ToDateTime("1/1/1900") : Convert.ToDateTime("1-" + g.Key.Month + "-" + g.Key.Year),
                                                                             Month = g.Key.Year == "TBC" ? "TBC" : g.Key.Month + "-" + g.Key.Year,
                                                                             Region = g.Key.Region,
                                                                             value = g.Sum(s => s.value)
                                                                         }).ToList<Release_Summary>();


                    listStats = (from tbl in lst_GraphData_Consolidation
                                 orderby tbl.Date
                                 group tbl by new { tbl.Month, tbl.Year, tbl.Region } into g
                                 select new Release_Summary
                                 {
                                     Month = g.Key.Month,
                                     Region = g.Key.Region,
                                     value = g.Sum(s => s.value)
                                 }).ToList<Release_Summary>();


                    //Build Details chart by Report Name
                    List<Model_Bars> oDetailsResult = new List<Model_Bars>();
                    string[] Months = listStats.Select(p => p.Month).Distinct().ToArray();
                    double[] ProjectedValue_AFO = listStats.Where(p => p.Region == "AFO").Select(p => p.value).ToArray();
                    double[] ProjectedValue_EMEAI = listStats.Where(p => p.Region == "EMEA-I").Select(p => p.value).ToArray();
                    double[] ProjectedValue_SAPK = listStats.Where(p => p.Region == "SAPK").Select(p => p.value).ToArray();
                    double[] ProjectedValue_GCFO = listStats.Where(p => p.Region == "GCFO").Select(p => p.value).ToArray();
                    double[] ProjectedValue_JFO = listStats.Where(p => p.Region == "JFO").Select(p => p.value).ToArray();

                    ViewData["Months"] = Months;
                    ViewData["ProjectedValue_AFO"] = ProjectedValue_AFO;
                    ViewData["ProjectedValue_EMEAI"] = ProjectedValue_EMEAI;
                    ViewData["ProjectedValue_SAPK"] = ProjectedValue_SAPK;
                    ViewData["ProjectedValue_GCFO"] = ProjectedValue_GCFO;
                    ViewData["ProjectedValue_JFO"] = ProjectedValue_JFO;

                    ViewData["RawData"] = lst_GraphData_Raw;
                    #endregion
                }
                ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);

                ViewData.Add("FromDate", FromDate);
                ViewData.Add("ToDate", ToDate);

                return View(lst_GraphData_Raw);
            }
            else
            {
                ViewData.Add("FromDate", FromDate);
                ViewData.Add("ToDate", ToDate);

                ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
                return View(ViewData["lst_GraphData_Raw"]);
            }
        }

        public ActionResult SNI_WaterFall()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            ViewBag.UserRole = oSessionUser.ROLE_DESC;
            List<Release_Summary> listStats = new List<Release_Summary>();
            List<Tbl_SNI_Release_Projection> lst_GraphData_Raw = new List<Tbl_SNI_Release_Projection>();

            string FromDate = System.DateTime.Now.AddMonths(-2).ToShortDateString();
            string ToDate = System.DateTime.Now.AddMonths(2).ToShortDateString();

            if (!ViewData.Keys.Contains("lst_GraphData_Raw"))
            {
                if (oSessionUser.ROLE_DESC == "BPA" || oSessionUser.ROLE_DESC == "Regional Lead")
                {
                    #region BPA Chart

                    //lst_GraphData_Raw = service.SNI_Release_Target_Projection(FromDate, ToDate, oSessionUser.SUPERREGION, "").ToList();
                    List<Release_Summary> lst_GraphData_Main_Summary = (from tbl in lst_GraphData_Raw
                                                                        group tbl by new { tbl.Feedback.Value, tbl.REGION, tbl.Feedback_Calc_Est_Invoice_dt_Month } into g
                                                                        select new Release_Summary
                                                                        {
                                                                            Date = g.Key.Value,
                                                                            Year = (Convert.ToDateTime(g.Key.Value).Year < System.DateTime.Now.Year || Convert.ToDateTime(g.Key.Value).Year == 1900) ? "TBC" : Convert.ToDateTime(g.Key.Value).Year.ToString(),
                                                                            Month = g.Key.Feedback_Calc_Est_Invoice_dt_Month,
                                                                            Region = g.Key.REGION,
                                                                            value = g.Sum(s => s.BAcklog)
                                                                        }).ToList();

                    List<Release_Summary> lst_GraphData_Consolidation = (from tbl in lst_GraphData_Main_Summary
                                                                         orderby tbl.Date
                                                                         group tbl by new { tbl.Month, tbl.Year, tbl.Region } into g
                                                                         select new Release_Summary
                                                                         {
                                                                             Date = (g.Key.Year == "TBC" || g.Key.Month == "TBC") ? Convert.ToDateTime("1/1/1900") : Convert.ToDateTime("1-" + g.Key.Month + "-" + g.Key.Year),
                                                                             Month = g.Key.Year == "TBC" ? "TBC" : g.Key.Month + "-" + g.Key.Year,
                                                                             Region = g.Key.Region,
                                                                             value = g.Sum(s => s.value)
                                                                         }).ToList<Release_Summary>();


                    listStats = (from tbl in lst_GraphData_Consolidation
                                 orderby tbl.Date
                                 group tbl by new { tbl.Month, tbl.Year, tbl.Region } into g
                                 select new Release_Summary
                                 {
                                     Month = g.Key.Month,
                                     Region = g.Key.Region,
                                     value = g.Sum(s => s.value)
                                 }).ToList<Release_Summary>();


                    //Build Details chart by Report Name
                    List<Model_Bars> oDetailsResult = new List<Model_Bars>();
                    string[] Months = listStats.Select(p => p.Month).Distinct().ToArray();
                    double[] ProjectedValue = listStats.Select(p => p.value).ToArray();

                    ViewData["Months"] = Months;
                    ViewData["ProjectedValue"] = ProjectedValue;

                    ViewData["RawData"] = lst_GraphData_Raw;
                    #endregion
                }
                else if (oSessionUser.ROLE_DESC == "Supervisor" || oSessionUser.ROLE_DESC == "CSR")
                {
                    #region Supervisor Chart

                    //lst_GraphData_Raw = service.SNI_Release_Target_Projection(FromDate, ToDate, "", oSessionUser.TEAM_NAME).ToList();
                    List<Release_Summary> lst_GraphData_Main_Summary = (from tbl in lst_GraphData_Raw
                                                                        group tbl by new { tbl.Feedback.Value, tbl.TEAM_NAME, tbl.Feedback_Calc_Est_Invoice_dt_Month } into g
                                                                        select new Release_Summary
                                                                        {
                                                                            Date = g.Key.Value,
                                                                            Year = (Convert.ToDateTime(g.Key.Value).Year < System.DateTime.Now.Year || Convert.ToDateTime(g.Key.Value).Year == 1900) ? "TBC" : Convert.ToDateTime(g.Key.Value).Year.ToString(),
                                                                            Month = g.Key.Feedback_Calc_Est_Invoice_dt_Month,
                                                                            Region = g.Key.TEAM_NAME,
                                                                            value = g.Sum(s => s.BAcklog)
                                                                        }).ToList();

                    List<Release_Summary> lst_GraphData_Consolidation = (from tbl in lst_GraphData_Main_Summary
                                                                         orderby tbl.Date
                                                                         group tbl by new { tbl.Month, tbl.Year, tbl.Region } into g
                                                                         select new Release_Summary
                                                                         {
                                                                             Date = (g.Key.Year == "TBC" || g.Key.Month == "TBC") ? Convert.ToDateTime("1/1/1900") : Convert.ToDateTime("1-" + g.Key.Month + "-" + g.Key.Year),
                                                                             Month = g.Key.Year == "TBC" ? "TBC" : g.Key.Month + "-" + g.Key.Year,
                                                                             Region = g.Key.Region,
                                                                             value = g.Sum(s => s.value)
                                                                         }).ToList<Release_Summary>();


                    listStats = (from tbl in lst_GraphData_Consolidation
                                 orderby tbl.Date
                                 group tbl by new { tbl.Month, tbl.Year, tbl.Region } into g
                                 select new Release_Summary
                                 {
                                     Month = g.Key.Month,
                                     Region = g.Key.Region,
                                     value = g.Sum(s => s.value)
                                 }).ToList<Release_Summary>();


                    //Build Details chart by Report Name
                    List<Model_Bars> oDetailsResult = new List<Model_Bars>();
                    string[] Months = listStats.Select(p => p.Month).Distinct().ToArray();
                    double[] ProjectedValue = listStats.Select(p => p.value).ToArray();

                    ViewData["Months"] = Months;
                    ViewData["ProjectedValue"] = ProjectedValue;

                    ViewData["RawData"] = lst_GraphData_Raw;
                    #endregion
                }
                else
                {
                    // WW Lead Case
                    #region WW lead Chart

                    //lst_GraphData_Raw = service.SNI_Release_Target_Projection(FromDate, ToDate, "", "").ToList();
                    List<Release_Summary> lst_GraphData_Main_Summary = (from tbl in lst_GraphData_Raw
                                                                        orderby tbl.Feedback descending
                                                                        group tbl by new { tbl.Feedback.Value, tbl.REGION, tbl.Feedback_Calc_Est_Invoice_dt_Month } into g
                                                                        select new Release_Summary
                                                                        {
                                                                            Date = g.Key.Value,
                                                                            Year = (Convert.ToDateTime(g.Key.Value).Year < System.DateTime.Now.Year || Convert.ToDateTime(g.Key.Value).Year == 1900) ? "TBC" : Convert.ToDateTime(g.Key.Value).Year.ToString(),
                                                                            Month = g.Key.Feedback_Calc_Est_Invoice_dt_Month,
                                                                            Region = g.Key.REGION,
                                                                            value = g.Sum(s => s.BAcklog)
                                                                        }).ToList();

                    List<Release_Summary> lst_GraphData_Consolidation = (from tbl in lst_GraphData_Main_Summary
                                                                         orderby tbl.Date
                                                                         group tbl by new { tbl.Month, tbl.Year, tbl.Region } into g
                                                                         select new Release_Summary
                                                                         {
                                                                             Date = (g.Key.Year == "TBC" || g.Key.Month == "TBC") ? Convert.ToDateTime("1/1/1900") : Convert.ToDateTime("1-" + g.Key.Month + "-" + g.Key.Year),
                                                                             Month = g.Key.Year == "TBC" ? "TBC" : g.Key.Month + "-" + g.Key.Year,
                                                                             Region = g.Key.Region,
                                                                             value = g.Sum(s => s.value)
                                                                         }).ToList<Release_Summary>();


                    listStats = (from tbl in lst_GraphData_Consolidation
                                 orderby tbl.Date
                                 group tbl by new { tbl.Month, tbl.Year, tbl.Region } into g
                                 select new Release_Summary
                                 {
                                     Month = g.Key.Month,
                                     Region = g.Key.Region,
                                     value = g.Sum(s => s.value)
                                 }).ToList<Release_Summary>();


                    //Build Details chart by Report Name
                    List<Model_Bars> oDetailsResult = new List<Model_Bars>();
                    string[] Months = listStats.Select(p => p.Month).Distinct().ToArray();
                    double[] ProjectedValue_AFO = listStats.Where(p => p.Region == "AFO").Select(p => p.value).ToArray();
                    double[] ProjectedValue_EMEAI = listStats.Where(p => p.Region == "EMEA-I").Select(p => p.value).ToArray();
                    double[] ProjectedValue_SAPK = listStats.Where(p => p.Region == "SAPK").Select(p => p.value).ToArray();
                    double[] ProjectedValue_GCFO = listStats.Where(p => p.Region == "GCFO").Select(p => p.value).ToArray();
                    double[] ProjectedValue_JFO = listStats.Where(p => p.Region == "JFO").Select(p => p.value).ToArray();

                    ViewData["Months"] = Months;
                    ViewData["ProjectedValue_AFO"] = ProjectedValue_AFO;
                    ViewData["ProjectedValue_EMEAI"] = ProjectedValue_EMEAI;
                    ViewData["ProjectedValue_SAPK"] = ProjectedValue_SAPK;
                    ViewData["ProjectedValue_GCFO"] = ProjectedValue_GCFO;
                    ViewData["ProjectedValue_JFO"] = ProjectedValue_JFO;

                    ViewData["RawData"] = lst_GraphData_Raw;
                    #endregion
                }

                ViewData.Add("FromDate", FromDate);
                ViewData.Add("ToDate", ToDate);


                ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
                return View(lst_GraphData_Raw);
            }
            else
            {
                ViewData.Add("FromDate", FromDate);
                ViewData.Add("ToDate", ToDate);


                ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
                return View(ViewData["lst_GraphData_Raw"]);
            }
        }

        [HttpPost]
        public ActionResult SNI_WaterFall(FormCollection objCollection)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            ViewBag.UserRole = oSessionUser.ROLE_DESC;
            List<Release_Summary> listStats = new List<Release_Summary>();
            List<Tbl_SNI_Release_Projection> lst_GraphData_Raw = new List<Tbl_SNI_Release_Projection>();

            string FromDate = objCollection["Chart_Range_From"].ToString();
            string ToDate = objCollection["Chart_Range_To"].ToString();

            if (!ViewData.Keys.Contains("lst_GraphData_Raw"))
            {
                if (oSessionUser.ROLE_DESC == "BPA" || oSessionUser.ROLE_DESC == "Regional Lead")
                {
                    #region BPA Chart

                    //lst_GraphData_Raw = service.SNI_Release_Target_Projection(FromDate, ToDate, oSessionUser.SUPERREGION, "").ToList();
                    List<Release_Summary> lst_GraphData_Main_Summary = (from tbl in lst_GraphData_Raw
                                                                        group tbl by new { tbl.Feedback.Value, tbl.REGION, tbl.Feedback_Calc_Est_Invoice_dt_Month } into g
                                                                        select new Release_Summary
                                                                        {
                                                                            Date = g.Key.Value,
                                                                            Year = (Convert.ToDateTime(g.Key.Value).Year < System.DateTime.Now.Year || Convert.ToDateTime(g.Key.Value).Year == 1900) ? "TBC" : Convert.ToDateTime(g.Key.Value).Year.ToString(),
                                                                            Month = g.Key.Feedback_Calc_Est_Invoice_dt_Month,
                                                                            Region = g.Key.REGION,
                                                                            value = g.Sum(s => s.BAcklog)
                                                                        }).ToList();

                    List<Release_Summary> lst_GraphData_Consolidation = (from tbl in lst_GraphData_Main_Summary
                                                                         orderby tbl.Date
                                                                         group tbl by new { tbl.Month, tbl.Year, tbl.Region } into g
                                                                         select new Release_Summary
                                                                         {
                                                                             Date = (g.Key.Year == "TBC" || g.Key.Month == "TBC") ? Convert.ToDateTime("1/1/1900") : Convert.ToDateTime("1-" + g.Key.Month + "-" + g.Key.Year),
                                                                             Month = g.Key.Year == "TBC" ? "TBC" : g.Key.Month + "-" + g.Key.Year,
                                                                             Region = g.Key.Region,
                                                                             value = g.Sum(s => s.value)
                                                                         }).ToList<Release_Summary>();


                    listStats = (from tbl in lst_GraphData_Consolidation
                                 orderby tbl.Date
                                 group tbl by new { tbl.Month, tbl.Year, tbl.Region } into g
                                 select new Release_Summary
                                 {
                                     Month = g.Key.Month,
                                     Region = g.Key.Region,
                                     value = g.Sum(s => s.value)
                                 }).ToList<Release_Summary>();


                    //Build Details chart by Report Name
                    List<Model_Bars> oDetailsResult = new List<Model_Bars>();
                    string[] Months = listStats.Select(p => p.Month).Distinct().ToArray();
                    double[] ProjectedValue = listStats.Select(p => p.value).ToArray();

                    ViewData["Months"] = Months;
                    ViewData["ProjectedValue"] = ProjectedValue;

                    ViewData["RawData"] = lst_GraphData_Raw;
                    #endregion
                }
                else if (oSessionUser.ROLE_DESC == "Supervisor" || oSessionUser.ROLE_DESC == "CSR")
                {
                    #region Supervisor Chart

                    //lst_GraphData_Raw = service.SNI_Release_Target_Projection(FromDate, ToDate, "", oSessionUser.TEAM_NAME).ToList();
                    List<Release_Summary> lst_GraphData_Main_Summary = (from tbl in lst_GraphData_Raw
                                                                        group tbl by new { tbl.Feedback.Value, tbl.TEAM_NAME, tbl.Feedback_Calc_Est_Invoice_dt_Month } into g
                                                                        select new Release_Summary
                                                                        {
                                                                            Date = g.Key.Value,
                                                                            Year = (Convert.ToDateTime(g.Key.Value).Year < System.DateTime.Now.Year || Convert.ToDateTime(g.Key.Value).Year == 1900) ? "TBC" : Convert.ToDateTime(g.Key.Value).Year.ToString(),
                                                                            Month = g.Key.Feedback_Calc_Est_Invoice_dt_Month,
                                                                            Region = g.Key.TEAM_NAME,
                                                                            value = g.Sum(s => s.BAcklog)
                                                                        }).ToList();

                    List<Release_Summary> lst_GraphData_Consolidation = (from tbl in lst_GraphData_Main_Summary
                                                                         orderby tbl.Date
                                                                         group tbl by new { tbl.Month, tbl.Year, tbl.Region } into g
                                                                         select new Release_Summary
                                                                         {
                                                                             Date = (g.Key.Year == "TBC" || g.Key.Month == "TBC") ? Convert.ToDateTime("1/1/1900") : Convert.ToDateTime("1-" + g.Key.Month + "-" + g.Key.Year),
                                                                             Month = g.Key.Year == "TBC" ? "TBC" : g.Key.Month + "-" + g.Key.Year,
                                                                             Region = g.Key.Region,
                                                                             value = g.Sum(s => s.value)
                                                                         }).ToList<Release_Summary>();


                    listStats = (from tbl in lst_GraphData_Consolidation
                                 orderby tbl.Date
                                 group tbl by new { tbl.Month, tbl.Year, tbl.Region } into g
                                 select new Release_Summary
                                 {
                                     Month = g.Key.Month,
                                     Region = g.Key.Region,
                                     value = g.Sum(s => s.value)
                                 }).ToList<Release_Summary>();


                    //Build Details chart by Report Name
                    List<Model_Bars> oDetailsResult = new List<Model_Bars>();
                    string[] Months = listStats.Select(p => p.Month).Distinct().ToArray();
                    double[] ProjectedValue = listStats.Select(p => p.value).ToArray();

                    ViewData["Months"] = Months;
                    ViewData["ProjectedValue"] = ProjectedValue;

                    ViewData["RawData"] = lst_GraphData_Raw;
                    #endregion
                }
                else
                {
                    // WW Lead Case
                    #region WW lead Chart

                    //lst_GraphData_Raw = service.SNI_Release_Target_Projection(FromDate, ToDate, "", "").ToList();
                    List<Release_Summary> lst_GraphData_Main_Summary = (from tbl in lst_GraphData_Raw
                                                                        orderby tbl.Feedback descending
                                                                        group tbl by new { tbl.Feedback.Value, tbl.REGION, tbl.Feedback_Calc_Est_Invoice_dt_Month } into g
                                                                        select new Release_Summary
                                                                        {
                                                                            Date = g.Key.Value,
                                                                            Year = (Convert.ToDateTime(g.Key.Value).Year < System.DateTime.Now.Year || Convert.ToDateTime(g.Key.Value).Year == 1900) ? "TBC" : Convert.ToDateTime(g.Key.Value).Year.ToString(),
                                                                            Month = g.Key.Feedback_Calc_Est_Invoice_dt_Month,
                                                                            Region = g.Key.REGION,
                                                                            value = g.Sum(s => s.BAcklog)
                                                                        }).ToList();

                    List<Release_Summary> lst_GraphData_Consolidation = (from tbl in lst_GraphData_Main_Summary
                                                                         orderby tbl.Date
                                                                         group tbl by new { tbl.Month, tbl.Year, tbl.Region } into g
                                                                         select new Release_Summary
                                                                         {
                                                                             Date = (g.Key.Year == "TBC" || g.Key.Month == "TBC") ? Convert.ToDateTime("1/1/1900") : Convert.ToDateTime("1-" + g.Key.Month + "-" + g.Key.Year),
                                                                             Month = g.Key.Year == "TBC" ? "TBC" : g.Key.Month + "-" + g.Key.Year,
                                                                             Region = g.Key.Region,
                                                                             value = g.Sum(s => s.value)
                                                                         }).ToList<Release_Summary>();


                    listStats = (from tbl in lst_GraphData_Consolidation
                                 orderby tbl.Date
                                 group tbl by new { tbl.Month, tbl.Year, tbl.Region } into g
                                 select new Release_Summary
                                 {
                                     Month = g.Key.Month,
                                     Region = g.Key.Region,
                                     value = g.Sum(s => s.value)
                                 }).ToList<Release_Summary>();


                    //Build Details chart by Report Name
                    List<Model_Bars> oDetailsResult = new List<Model_Bars>();
                    string[] Months = listStats.Select(p => p.Month).Distinct().ToArray();
                    double[] ProjectedValue_AFO = listStats.Where(p => p.Region == "AFO").Select(p => p.value).ToArray();
                    double[] ProjectedValue_EMEAI = listStats.Where(p => p.Region == "EMEA-I").Select(p => p.value).ToArray();
                    double[] ProjectedValue_SAPK = listStats.Where(p => p.Region == "SAPK").Select(p => p.value).ToArray();
                    double[] ProjectedValue_GCFO = listStats.Where(p => p.Region == "GCFO").Select(p => p.value).ToArray();
                    double[] ProjectedValue_JFO = listStats.Where(p => p.Region == "JFO").Select(p => p.value).ToArray();

                    ViewData["Months"] = Months;
                    ViewData["ProjectedValue_AFO"] = ProjectedValue_AFO;
                    ViewData["ProjectedValue_EMEAI"] = ProjectedValue_EMEAI;
                    ViewData["ProjectedValue_SAPK"] = ProjectedValue_SAPK;
                    ViewData["ProjectedValue_GCFO"] = ProjectedValue_GCFO;
                    ViewData["ProjectedValue_JFO"] = ProjectedValue_JFO;

                    ViewData["RawData"] = lst_GraphData_Raw;
                    #endregion
                }

                ViewData.Add("FromDate", FromDate);
                ViewData.Add("ToDate", ToDate);

                ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
                return View(lst_GraphData_Raw);
            }
            else
            {
                ViewData.Add("FromDate", FromDate);
                ViewData.Add("ToDate", ToDate);

                ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
                return View(ViewData["lst_GraphData_Raw"]);
            }
        }

    }


}