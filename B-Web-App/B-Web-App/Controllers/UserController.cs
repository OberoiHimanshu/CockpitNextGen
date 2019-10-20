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

        // GET: User
        public ActionResult Index()
        {
            VW_USERS User = db.VW_USERS.FirstOrDefault();
            TempData["User"] = User;

            return View(User);
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
            oUser.TEAM_ID = Convert.ToInt32(oCollection["Team"]);
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
