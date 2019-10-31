using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using B_Web_App.Models;

using Kendo.Mvc.Extensions;


namespace B_Web_App.Controllers
{
    public class GenericController : Controller
    {
        private CNG_Azure db = new CNG_Azure();
        // Private Attributes

        VW_USERS oSessionUser;
        List<Dim_Billing_Blocks> lstBillingBlocks;
        List<Dim_Delivery_Blocks> lstDeliveryBlocks;
        List<Dim_Customers> lstCustomers;
        List<Dim_Sales_Force> lstSalesForce;
        List<Dim_Business_Master> lstBusienssMaster;
        string CockpitUI;
        DashboardModal oDashboardModal;
        Tbl_Followup_Summary[] oMyFollowups;
        Tbl_Followups[] oSystemGeneratedFollowups;

        //


        // GET: Genric
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetTeamsByRegion(string RegionName)
        {
            if (RegionName == "WW" || RegionName == "")
            {
                var oresult = db.TBL_TEAM_STRUCTURE.ToList();
                return Json(oresult);
            }
            else
            {
                List<TBL_TEAM_STRUCTURE> lst_results = new List<TBL_TEAM_STRUCTURE>();

                var unique_Teams = (from t1 in db.VW_USERS
                                    where t1.SUPERREGION == RegionName
                                    select t1.TEAM_NAME).Distinct();
                foreach (string Team in unique_Teams)
                {
                    TBL_TEAM_STRUCTURE tmp_Team = (from t2 in db.TBL_TEAM_STRUCTURE where t2.TEAM_NAME == Team select t2).FirstOrDefault();
                    lst_results.Add(tmp_Team);
                }

                return Json(lst_results);
            }
        }

        public JsonResult GetCountryByRegion(string Region)
        {
            var userProfile = (from t1 in db.Tbl_Country_Sorg_Orig
                               where t1.Region == Region
                               select t1).Distinct().ToList();

            return Json(userProfile);
        }

        public JsonResult GetmanagerName(string Team)
        {
            List<TBL_TEAM_STRUCTURE> oItems = new List<TBL_TEAM_STRUCTURE>();

            if (Team != "" && ModelState.IsValid)
            {
                var userDtls = from user in db.TBL_TEAM_STRUCTURE
                               where user.TEAM_NAME == Team 
                               select user;
                oItems = userDtls.Distinct().ToList();
            }
            
            return Json(oItems);
        }


    }
}