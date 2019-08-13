using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cockpit_NextGenMVC.BAL;
using Cockpit_NextGenMVC.Models;

using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace Cockpit_NextGenMVC.Controllers
{
    [SessionExpire]
    public class OSBRController : Controller
    {
        //
        // GET: /OSBR/

        static BAL.Service1Client service = new BAL.Service1Client();
        VW_USERS oSessionUser;
        string currentView = string.Empty;
        DashboardModal oDashboardModal;

        public ActionResult Index()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            ViewBag.UserRole = oSessionUser.ROLE_DESC;
            
            oDashboardModal = (DashboardModal)Session["oDashboardModal"];
            currentView = oDashboardModal.UI_Type;

            if (oDashboardModal.UI_Type == "CSR Cockpit")
            {
                var lst_results = service.GetMyOSBRNotifications(oSessionUser.SAP_User_Name);
                ViewData.Add("lst_OSBRNotifications", lst_results);

                ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
                return View(lst_results);
            }
            else
            {
                if (oSessionUser.ROLE_DESC == "Supervisor")
                {
                    var lst_results = service.GetTeamOSBRNotifications("", oSessionUser.TEAM_NAME);
                    ViewData.Add("lst_OSBRNotifications", lst_results);

                    ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
                    return View(lst_results);
                }
                else if (oSessionUser.ROLE_DESC == "BPA" || oSessionUser.ROLE_DESC == "Regional Lead")
                {
                    var lst_results = service.GetTeamOSBRNotifications(oSessionUser.SUPERREGION, "");
                    ViewData.Add("lst_OSBRNotifications", lst_results);

                    ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
                    return View(lst_results);
                }
                else
                {
                    var lst_results = service.GetTeamOSBRNotifications("", "");
                    ViewData.Add("lst_OSBRNotifications", lst_results);

                    ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
                    return View(lst_results);
                }
                
            }
        }

        public JsonResult ReadSelectedOrder(string Sales_Order)
        {
            List<Tbl_Order_comments> oComments = new List<Tbl_Order_comments>();

            if (Sales_Order != "" && ModelState.IsValid)
            {
                oComments = service.GetMasterFocusChildOrders(Sales_Order).ToList();
                ViewData.Add("CommentsHistory", oComments);
            }

            return Json(oComments);
        }

        //public JsonResult ReadSelectedOrderDetails(string Sales_Order)
        //{
        //    List<VW_Orders_Info> oInfo = new List<VW_Orders_Info>();

        //    if (Sales_Order != "" && ModelState.IsValid)
        //    {
        //        oInfo = service.Get_OSBR_Order_Info(Sales_Order).ToList();
        //        ViewData.Add("OSBROrdersInfo", oInfo);
        //    }

        //    return Json(oInfo);
        //}

        public JsonResult ReadOrderLineItems(string Sales_Order)
        {
            List<Tbl_Order_Items> oItems = new List<Tbl_Order_Items>();

            if (Sales_Order != "" && ModelState.IsValid)
            {
                oItems = service.GetLineItems(Convert.ToDouble(Sales_Order)).ToList();
            }

            return Json(oItems);
        }

    }
}
