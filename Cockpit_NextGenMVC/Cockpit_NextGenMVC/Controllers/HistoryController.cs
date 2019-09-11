using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using Cockpit_NextGenMVC.BAL;
using Cockpit_NextGenMVC.BAL_User_Mgmt;
using Cockpit_NextGenMVC.Models;

namespace Cockpit_NextGenMVC.Controllers
{
    public class HistoryController : Controller
    {
        //
        // GET: /History/

        static readonly BAL.Service1Client service = new BAL.Service1Client();
        VW_USERS oSessionUser;
        DashboardModal oDashboardModel;
        List<Tbl_Archival_Summary> Archival_Summary;
        List<VW_Orders_Info> Archival_Details;

        public ActionResult Index()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            oDashboardModel = (DashboardModal)Session["oDashboardModal"];

            var result = service.GetArchivesList().ToList();

            ViewData.Add("oDashboardModal", oDashboardModel);
            return View(result);
        }

        [HttpGet]
        public virtual ActionResult Download(string ReportPath)
        {
            string filename = ReportPath;
            string filepath = Path.Combine(Server.MapPath("~/MECExtracts"), ReportPath + ".xlsx");

            return File(filepath, "application/vnd.ms-excel", ReportPath);
        }

        public ActionResult Archives_Details(string OrderOwner)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            oDashboardModel = (DashboardModal)Session["oDashboardModal"];

            Archival_Details = service.Get_Archived_Orders("", OrderOwner).ToList();

            ViewData.Add("oDashboardModal", oDashboardModel);

            return View(Archival_Details);
        }


        public ActionResult Archives_Summary()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            oDashboardModel = (DashboardModal)Session["oDashboardModal"];


            if (oDashboardModel.UI_Type == "CSR Cockpit")
            {
                Archival_Summary = service.Get_Archived_Orders_Summary("", "", "", oSessionUser.TEAM_NAME).ToList();
            }
            else
            {
                if (oSessionUser.ROLE_DESC == "CSR")
                {
                    Archival_Summary = service.Get_Archived_Orders_Summary("", "", "", oSessionUser.TEAM_NAME).ToList();
                }
                else if (oSessionUser.ROLE_DESC == "Supervisor")
                {
                    Archival_Summary = service.Get_Archived_Orders_Summary("", "", "", oSessionUser.TEAM_NAME).ToList();
                }
                else if (oSessionUser.ROLE_DESC == "BPA" || oSessionUser.ROLE_DESC == "Regional Lead")
                {
                    Archival_Summary = service.Get_Archived_Orders_Summary(oSessionUser.SUPERREGION, "", "", "").ToList();
                }
                else
                {
                    Archival_Summary = service.Get_Archived_Orders_Summary("", "", "", "").ToList();
                }
            }

            ViewData.Add("oDashboardModal", oDashboardModel);

            return View(Archival_Summary);
        }

    }
}
