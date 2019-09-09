using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Cockpit_NextGenMVC.BAL;
using Cockpit_NextGenMVC.BAL_User_Mgmt;
using Cockpit_NextGenMVC.Models;

namespace Cockpit_NextGenMVC.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /VW_USERS/

        static readonly BAL_User_Mgmt.Service1Client Users_service = new BAL_User_Mgmt.Service1Client();
        static readonly BAL.Service1Client service = new BAL.Service1Client();

        List<VW_USERS> oSessionUserProfiles;
        List<SelectListItem> lstRoles;
        VW_USERS oSessionUser;
        List<TBL_ROLE> lst_Roles;
        List<TBL_TEAM_STRUCTURE> lst_Teams;
        DashboardModal oDashboardModal;
        Tbl_Unmapped_Orders_By_Region_Function[] lstUnmappedUsers;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserOrdersRollback()
        {
            return View("");
        }

        public JsonResult GetReassignedOrdersOFUser(string Username)
        {
            List<Tbl_Order_Action_Owner> OrdList = new List<Tbl_Order_Action_Owner>();
            OrdList = service.GetReassignedOrdersOFUser(Username).ToList();
            return Json(OrdList);
        }

        public JsonResult RollbackMultipleOrders(string selectedOrders, string username)
        {
            bool result = false;
            oSessionUser = (VW_USERS)Session["UserProfile"];
            if (selectedOrders != "")
            {
                string[] salesOrders = selectedOrders.Split(",".ToCharArray());
                foreach (string SalesID in salesOrders)
                {
                    bool res = service.ReAssignUserOrders(SalesID, username);
                }
                result = true;
            }
            return Json(result);
        }

        public ActionResult UserProfile()
        {

            if (Session["UserProfile"] == null)
            {
                oSessionUserProfiles = Users_service.GetUserDetails(User.Identity.Name.Replace("AGILENT\\", "")).ToList();
                oSessionUser = oSessionUserProfiles.FirstOrDefault();

                Session["UserProfile"] = oSessionUser;
                Session["Role"] = oSessionUser.ROLE_DESC.ToString();

                lst_Roles = Users_service.GetRoleMaster().ToList();
                Session["RolesMaster"] = lst_Roles;

                ViewData.Add("Role_Master", new SelectList(lst_Roles, "ROLE_ID", "ROLE_DESC", oSessionUser.ROLE_DESC));
            }
            else
            {
                oSessionUser = (VW_USERS)Session["UserProfile"];
                Session["Role"] = oSessionUser.ROLE_DESC.ToString();

                oDashboardModal = (DashboardModal)Session["oDashboardModal"];
                ViewData.Add("oDashboardModal", oDashboardModal);

                lst_Roles = Users_service.GetRoleMaster().ToList();
                Session["RolesMaster"] = lst_Roles;
                ViewData.Add("Role_Master", new SelectList(lst_Roles, "ROLE_ID", "ROLE_DESC", oSessionUser.ROLE_DESC));

                lst_Teams = Users_service.GetTeamMaster(oSessionUser.SUPERREGION).ToList();
                Session["TeamsMaster"] = lst_Teams;
                ViewData.Add("Team_Master", new SelectList(lst_Teams, "TEAM_ID", "TEAM_NAME", oSessionUser.TEAM_NAME));
            }

            if (!ViewData.Keys.Contains("oDashboardModal"))
                ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);

            return View(oSessionUser);
        }

        public ActionResult InActiveTeamMembers(string TeamName)
        {
            var userProfile = Users_service.GetInActiveUserTeamDetails(TeamName);
            oDashboardModal = (DashboardModal)Session["oDashboardModal"];
            ViewData.Add("oDashboardModal", oDashboardModal);

            return View(userProfile);
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }


        [HttpPost]
        public ActionResult ActivateUser(int UserID, string email, string fullname)
        {
            oSessionUserProfiles = Users_service.GetUserDetails(User.Identity.Name.Replace("AGILENT\\", "")).ToList();
            oSessionUser = oSessionUserProfiles.FirstOrDefault();

            try
            {
                TBL_USERS oUser = new TBL_USERS();
                oUser.USER_ID = UserID;
                Users_service.ActiveDeactiveUser(oUser);

                var userProfile = Users_service.GetInActiveUserTeamDetails(oSessionUser.TEAM_NAME);

                string subject = "Welcome to CNG NextGen";
                string body;
                body = "<html><head></head><body> Dear " + fullname + " , <br/><br/> Your CNG Next Generation tool user account has been activated.You can use the below link to access CNG Next Generation tool. <br/><a href='http://cockpitnextgen.smseworldwide.ind.agilent.com/'>http://cockpitnextgen.smseworldwide.ind.agilent.com/</a> . <br/><br/> Thanks, <br/> Cockpit Next Generation Support Team.</body></html>";

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress("lscabc_tools@agilent.com");
                msg.To.Add(new MailAddress(email));
                msg.Subject = subject;
                msg.Body = body;
                msg.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("cos.smtp.agilent.com");
                smtp.Send(msg);
                return Json(userProfile);
            }
            catch (Exception ex)
            {
                return Json(" ");
            }

        }

        public ActionResult UserLogin()
        {
            string DowntimeFlag = System.Configuration.ConfigurationManager.AppSettings["DowntimeFlag"].ToString();
            string domainName = System.Configuration.ConfigurationManager.AppSettings["DomainName"].ToString();

            Session["DomainName"] = domainName;

            if (DowntimeFlag == "false")
            {
                oSessionUserProfiles = Users_service.GetUserDetails(User.Identity.Name.Replace(domainName + "\\", "")).ToList();

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
                        var TeamProfile = Users_service.GetUserTeamDetails(oSessionUser.TEAM_NAME);
                        var UniqueMembers = (from tbl in TeamProfile group tbl by new { tbl.FULLNAME, tbl.NTLOGIN } into g select new Model_Pie { category = g.Key.FULLNAME, Color = g.Key.NTLOGIN }).ToList();
                        var UniqueCSRs = (from tbl in TeamProfile where tbl.ROLE_DESC == "CSR" group tbl by new { tbl.FULLNAME, tbl.NTLOGIN } into g select new Model_Pie { category = g.Key.FULLNAME, Color = g.Key.NTLOGIN }).ToList();

                        Session["TeamProfile"] = UniqueMembers;
                        Session["UniqueCSRs"] = UniqueCSRs;
                    }
                    else
                    {
                        var TeamProfile = Users_service.GetUserRegionDetails(oSessionUser.SUPERREGION);
                        var UniqueMembers = (from tbl in TeamProfile group tbl by new { tbl.FULLNAME, tbl.NTLOGIN } into g select new Model_Pie { category = g.Key.FULLNAME, Color = g.Key.NTLOGIN }).ToList();
                        var UniqueCSRs = (from tbl in TeamProfile where tbl.ROLE_DESC == "CSR" group tbl by new { tbl.FULLNAME, tbl.NTLOGIN } into g select new Model_Pie { category = g.Key.FULLNAME, Color = g.Key.NTLOGIN }).ToList();

                        Session["TeamProfile"] = UniqueMembers;
                        Session["UniqueCSRs"] = UniqueCSRs;
                    }

                    Session["lstRoles"] = lstRoles;
                    Session["UserProfile"] = oSessionUser;
                    Session["ntlogin"] = User.Identity.Name.Replace("AGILENT\\", "");

                    return View();
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

        public ActionResult TeamMembers(string TeamName)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];

            oDashboardModal = (DashboardModal)Session["oDashboardModal"];
            ViewData.Add("oDashboardModal", oDashboardModal);

            lst_Roles = Users_service.GetRoleMaster().ToList();
            Session["RolesMaster"] = lst_Roles;
            ViewData.Add("Role_Master", new SelectList(lst_Roles, "ROLE_ID", "ROLE_DESC", oSessionUser.ROLE_DESC));

            lst_Teams = Users_service.GetTeamMaster(oSessionUser.SUPERREGION).ToList();
            Session["TeamsMaster"] = lst_Teams;
            ViewData.Add("Team_Master", new SelectList(lst_Teams, "TEAM_ID", "TEAM_NAME", oSessionUser.TEAM_NAME));


            if (oSessionUser.ROLE_DESC == "Supervisor" || oSessionUser.ROLE_DESC == "CSR" || oSessionUser.ROLE_DESC == "WW Lead")
            {
                var userProfile = Users_service.GetUserTeamDetails(TeamName);
                return View(userProfile);
            }
            else
            {
                var userProfile = Users_service.GetUserRegionDetails(oSessionUser.SUPERREGION);
                return View(userProfile);
            }
        }


        public ActionResult UnAuthorizedAccess()
        {
            return View();
        }

        public ActionResult PageNotFound(string aspxerrorpath)
        {
            oDashboardModal = (DashboardModal)Session["oDashboardModal"];
            if (oDashboardModal == null)
                oDashboardModal = new DashboardModal();

            ViewData.Add("oDashboardModal", oDashboardModal);

            ViewBag.LookupPagePath = aspxerrorpath;

            return View();
        }

        public ActionResult UserSessionExpired()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserSessionRefreshed(FormCollection oCollection)
        {
            oSessionUserProfiles = Users_service.GetUserDetails(User.Identity.Name.Replace("AGILENT\\", "")).ToList();
            oSessionUser = oSessionUserProfiles.FirstOrDefault();
            Session["UserProfile"] = oSessionUser;

            return RedirectToAction("Index", "Home");
        }


        public ActionResult UserRegistration()
        {
            lst_Roles = Users_service.GetRoleMaster().ToList();
            Session["RolesMaster"] = lst_Roles;

            ViewData.Add("Role_Master", new SelectList(lst_Roles, "ROLE_ID", "ROLE_DESC"));

            return View();
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
            oUser.TEAM_ID = Convert.ToInt32(oCollection["Team"]);
            oUser.PROFILE_PIC = "User_Profiles/default-profile-big.png";
            bool result = Users_service.RegisterNewUser(oUser);

            if (result)
            {
                Session["Registermsg"] = "Registration done Successfully.";
                string managerEmail = Users_service.getEmailIDByName(oCollection["txtManager"].ToString());
                // sending mail
                string emailTo = oCollection["txtEmail"].ToString();
                string emailCC = "himanshu_oberoi@agilent.com";
                //string emailCC = managerEmail;

                string subject = "CNG : New User Registration request for Approval.";
                string body = "Dear " + oCollection["txtFullname"].ToString() + ",";
                body += "<br/><br/>" + "Thanks for placing user access request for “Cockpit Next Generation” Tool with below specifications.";
                body += "<br/><br/> Full Name : " + oCollection["txtFullname"].ToString();
                body += "<br/> NT Login : " + oCollection["nttlogin"].ToString();
                body += "<br/> SAP Login : " + oCollection["txtUsername"].ToString();
                body += "<br/> Email : " + oCollection["txtEmail"].ToString();
                body += "<br/> Region : " + oCollection["ddlRegion"].ToString();
                body += "<br/> Team Name  : " + oCollection["txtteamname"].ToString();
                body += "<br/> Team Manager Name  : " + oCollection["txtManager"].ToString();
                body += "<br/> Business Justification : " + oCollection["txtComment"].ToString();
                body += "<br/><br/> Dear " + oCollection["txtManager"].ToString();
                body += "<br/> A new <b>Cockpit Next Generation</b> User request is pending for your review & Approval. Please user below link for approval.   ";
                body += "<br/><br/><a href='http://cockpitnextgen.smseworldwide.ind.agilent.com/'> http://cockpitnextgen.smseworldwide.ind.agilent.com/ </a>";
                body += "<br/><br/> Kind Regards,";
                body += "<br/><br/> Cockpit Support Team";
                body = "<html><head></head><body>" + body + "</body></html>";

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress("himanshu.oberoi@stryker.com");
                msg.To.Add(new MailAddress(emailTo));
                msg.CC.Add(new MailAddress(emailCC));

                msg.Subject = subject;
                msg.Body = body;
                msg.IsBodyHtml = true;

                try
                {
                    SmtpClient smtp = new SmtpClient("cos.smtp.stryker.com");
                    //SmtpClient smtp = new SmtpClient("cos.smtp.agilent.com");
                    smtp.Send(msg);
                }
                catch (Exception ex)
                {
                    ViewData.Add("User Access " + oUser.FULLNAME + " request Submission Error. " + ex.Message.ToString(), "SubmissionConfirmation");
                }

                ViewData.Add("User Access " + oUser.FULLNAME + " request Submited.Approval pending by Manager " + oCollection["txtteamname"].ToString(), "SubmissionConfirmation");
            }
            else
            {
                ViewData.Add("User Access request Submission Error. ", "SubmissionConfirmation");
            }


            return View();
        }

        [HttpPost]
        public JsonResult SaveUserRegistrationNew(TBL_USERS details)
        {
            //string username = oCollection["nttlogin"].ToString();
            string Msg;
            bool IsUserAdded = false;
            string[] RequestPath = HttpContext.Request.UrlReferrer.AbsolutePath.ToString().Split("/".ToCharArray());
            TBL_USERS oUser = new TBL_USERS();
            oUser.USER_ID = 0;
            oUser.COUNTRY = details.COUNTRY.ToString();
            oUser.EMAIL = details.EMAIL.ToString();
            oUser.FULLNAME = details.FULLNAME.ToString();
            oUser.NTLOGIN = details.NTLOGIN.ToString();
            oUser.ROLE_ID = details.ROLE_ID;
            oUser.SUPERREGION = details.SUPERREGION.ToString();
            oUser.USERNAME = details.USERNAME.ToString();
            oUser.TEAM_ID = Convert.ToInt32(details.TEAM_ID);
            oUser.PROFILE_PIC = "User_Profiles/default-profile-big.png";
            IsUserAdded = Users_service.RegisterNewUser(oUser);
            Users_service.UpdateUserInSApandBBZTRD(details.FULLNAME.ToString(), details.USERNAME.ToString());

            // sending mail
            string emailTo = details.EMAIL.ToString();
            string subject = "New VW_USERS Registration";
            string body = details.FULLNAME.ToString();
            body += "<br/>" + details.COUNTRY.ToString();
            body += "<br/>" + details.SUPERREGION.ToString();
            body += "<br/>" + details.ROLE_ID;
            body = "<html><head></head><body> Dear VW_USERS , <br/> Below are the details for new VW_USERS activation" + body + "</body></html>";

            MailMessage msg = new MailMessage();
            //msg.From = new MailAddress("lscabc_tools@agilent.com");
            msg.From = new MailAddress("himanshu.oberoi@stryker.com");

            msg.To.Add(new MailAddress(emailTo));

            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = true;

            try
            {
                //SmtpClient smtp = new SmtpClient("cos.smtp.agilent.com");
                SmtpClient smtp = new SmtpClient("cos.smtp.stryker.com");
                smtp.Send(msg);
            }
            catch (Exception ex)
            {
            }

            if (IsUserAdded == true)
            {
                Msg = "User " + oUser.USERNAME + " Mapped Successfully!";
            }
            else
            {
                Msg = "Opps, we encounter some issue. Please try gain.";
            }
            return Json(Msg);
        }

        [HttpPost]
        public JsonResult SaveUserDetails(TBL_USERS details)
        {
            bool result = false;

            try
            {
                oSessionUser = (VW_USERS)Session["UserProfile"];
                HttpPostedFileBase photo = Request.Files["ProfilePic"];

                TBL_USERS oUser = new TBL_USERS();
                oUser.USER_ID = Convert.ToInt32(details.USER_ID);
                oUser.FULLNAME = details.FULLNAME.ToString();
                oUser.EMAIL = details.EMAIL.ToString();
                oUser.USERNAME = details.USERNAME.ToString();
                oUser.ROLE_ID = Convert.ToInt32(details.ROLE_ID);
                oUser.TEAM_ID = Convert.ToInt32(details.TEAM_ID);
                oUser.PROFILE_PIC = details.PROFILE_PIC;
                oUser.NTLOGIN = details.NTLOGIN.ToString();

                Users_service.UpdateUserDetails(oUser);

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }

            return Json(result);
        }


        public JsonResult GetmanagerName(string Team)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            List<VW_USERS> oItems = new List<VW_USERS>();

            if (Team != "" && ModelState.IsValid)
            {
                oItems = Users_service.GetUserTeamDetails(Team).Where(p => p.ROLE_DESC == "Supervisor").ToList();
            }

            return Json(oItems);
        }


        public ActionResult UnMappedUsers()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];

            oDashboardModal = (DashboardModal)Session["oDashboardModal"];
            ViewData.Add("oDashboardModal", oDashboardModal);

            var userUnMappedList = Users_service.GetUnMappedUsersByRegion(oSessionUser.SUPERREGION);
            return View(userUnMappedList);
        }

        public ActionResult UnMappedUserBySalesOrg(string Region, string UserGroup)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];

            oDashboardModal = (DashboardModal)Session["oDashboardModal"];
            ViewData.Add("oDashboardModal", oDashboardModal);
            lstUnmappedUsers = Users_service.GetUnMappedUsersByRegionFunction(Region, UserGroup);

            return View(lstUnmappedUsers);
        }

        public ActionResult RegisterNewUser(string SAPName)
        {
            ViewBag.SAPName = SAPName;
            return View();
        }

        public JsonResult CheckuserExistByNtLogin(string NtLogin)
        {
            List<VW_USERS> UserExist;
            VW_USERS oUser;

            UserExist = Users_service.GetUserDetails(NtLogin).ToList();
            oUser = UserExist.FirstOrDefault();
            return Json(oUser);
        }

        public JsonResult GetUserDetailsByNTLogin(string NtLogin)
        {
            LDAP_Model ldap = new LDAP_Model();
            var NTLoginUserDeatis = ldap.FetchUserDataByNtlogin(NtLogin, true);
            return Json(NTLoginUserDeatis);
        }
    }
}
