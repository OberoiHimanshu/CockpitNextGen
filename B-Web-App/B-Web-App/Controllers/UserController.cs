using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using B_Web_App.Models;

namespace B_Web_App.Controllers
{
    public class UserController : Controller
    {
        private CNG_Azure db = new CNG_Azure();

        List<VW_USERS> oSessionUserProfiles;
        List<SelectListItem> lstRoles;
        VW_USERS oSessionUser;
        List<TBL_ROLE> lst_Roles;
        List<TBL_TEAM_STRUCTURE> lst_Teams;
        DashboardModal oDashboardModal;
        Tbl_Unmapped_Orders_By_Region_Function[] lstUnmappedUsers;


        // GET: User
        public ActionResult Index()
        {
            VW_USERS User = db.VW_USERS.FirstOrDefault();
            TempData["User"] = User;

            string DowntimeFlag = System.Configuration.ConfigurationManager.AppSettings["DowntimeFlag"].ToString();
            string domainName = System.Configuration.ConfigurationManager.AppSettings["DomainName"].ToString();

            Session["DomainName"] = domainName;

            if (DowntimeFlag == "false")
            {
                //oSessionUserProfiles = Users_service.GetUserDetails(User.Identity.Name.Replace(domainName + "\\", "")).ToList();
                oSessionUserProfiles = db.VW_USERS.ToList();

                if (oSessionUserProfiles.Count != 0)
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

                    if (oSessionUser.ROLE_DESC == "Supervisor" || oSessionUser.ROLE_DESC == "CSR")
                    {
                        //var TeamProfile = Users_service.GetUserTeamDetails(oSessionUser.TEAM_NAME);
                        var TeamProfile = (from tbl in db.VW_USERS where tbl.TEAM_NAME == oSessionUser.TEAM_NAME select tbl).ToList();

                        var UniqueMembers = (from tbl in TeamProfile group tbl by new { tbl.FULLNAME, tbl.NTLOGIN } into g select new Model_Pie { category = g.Key.FULLNAME, Color = g.Key.NTLOGIN }).ToList();
                        var UniqueCSRs = (from tbl in TeamProfile where tbl.ROLE_DESC == "CSR" group tbl by new { tbl.FULLNAME, tbl.NTLOGIN } into g select new Model_Pie { category = g.Key.FULLNAME, Color = g.Key.NTLOGIN }).ToList();

                        Session["TeamProfile"] = UniqueMembers;
                        Session["UniqueCSRs"] = UniqueCSRs;
                    }
                    else
                    {
                        //var TeamProfile = Users_service.GetUserRegionDetails(oSessionUser.SUPERREGION);
                        var TeamProfile = (from tbl in db.VW_USERS where tbl.SUPERREGION == oSessionUser.SUPERREGION select tbl).ToList();
                        var UniqueMembers = (from tbl in TeamProfile group tbl by new { tbl.FULLNAME, tbl.NTLOGIN } into g select new Model_Pie { category = g.Key.FULLNAME, Color = g.Key.NTLOGIN }).ToList();
                        var UniqueCSRs = (from tbl in TeamProfile where tbl.ROLE_DESC == "CSR" group tbl by new { tbl.FULLNAME, tbl.NTLOGIN } into g select new Model_Pie { category = g.Key.FULLNAME, Color = g.Key.NTLOGIN }).ToList();

                        Session["TeamProfile"] = UniqueMembers;
                        Session["UniqueCSRs"] = UniqueCSRs;
                    }

                    Session["lstRoles"] = lstRoles;
                    Session["UserProfile"] = oSessionUser;
                    Session["ntlogin"] = User.NTLOGIN;

                    return View(User);
                }
                else
                {
                    return RedirectToAction("UserRegistration", "User");
                }
            }
            else
            {
                return View("Downtime");
            }

        }

        public ActionResult RegisterNewUser()
        {
            if(TempData.Keys.Contains("User"))
            {
                VW_USERS User = (VW_USERS)TempData["User"];
                return View(User);
            }
            else
            {
                VW_USERS User = db.VW_USERS.FirstOrDefault();
                return View(User);
            }
            
        }

        [HttpPost]
        public ActionResult SaveUserRegistration(FormCollection oCollection)
        {
            //string username = oCollection["nttlogin"].ToString();
            TBL_USERS oUser = new TBL_USERS();
            oUser.USER_ID = 0;
            oUser.COUNTRY = oCollection["ddlCountry"].ToString();
            oUser.EMAIL = oCollection["txtEmail"].ToString();
            oUser.FULLNAME = oCollection["txtFullname"].ToString();
            oUser.NTLOGIN = oCollection["nttlogin"].ToString();
            oUser.ROLE_ID = Convert.ToInt32(oCollection["ddlRole"]);
            oUser.SUPERREGION = oCollection["ddlRegion"].ToString();
            oUser.USERNAME = oCollection["txtUsername"].ToString();
            //oUser.TEAM_ID = Convert.ToInt32(oCollection["Team"]);
            oUser.PROFILE_PIC = "User_Profiles/default-profile-big.png";

            try
            {
                db.TBL_USERS.Add(oUser);
                db.SaveChanges();  

                Session["Registermsg"] = "Registration done Successfully.";
                //ViewData.Add("User Access " + oUser.FULLNAME + " request Submited.Approval pending by Manager " + oCollection["txtteamname"].ToString(), "SubmissionConfirmation");

            }
            catch (Exception ex)
            {
                ViewData.Add("User Access request Submission Error. ", "SubmissionConfirmation");
            }

            return View();
        }



    }
}
