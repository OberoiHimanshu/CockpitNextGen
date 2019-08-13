using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using Cockpit_NextGenMVC.BAL;
using Cockpit_NextGenMVC.Models;
using System.Web.Mvc;

using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace Cockpit_NextGenMVC.Controllers
{
    [SessionExpire]
    public class FollowupController : Controller
    {
        static BAL.Service1Client service = new BAL.Service1Client();
        VW_USERS oSessionUser;

        DashboardModal oDashboardModal;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Summary()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];

            var my_summary = service.GetMyFollowupsSummary(oSessionUser.NTLOGIN, oSessionUser.TEAM_NAME);

            ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
            
            return View(my_summary);

        }

        public JsonResult CloseMultipleFollowups(string selectedFollowupIds)
        {
            bool result = false;
            oSessionUser = (VW_USERS)Session["UserProfile"];

            if(selectedFollowupIds != "")
            {
                string []followups = selectedFollowupIds.Split(",".ToCharArray());

                foreach (string folowupID in followups)
                {
                    bool res = service.UpdateFollowup(Convert.ToInt32(folowupID), 0,"", "", "", Convert.ToDateTime("1/1/1900"), "", "", "Closed", oSessionUser.NTLOGIN, System.DateTime.Now, "", "");
                }

                result = true;
            }

            return Json(result);
        }

        public JsonResult AssignMultipleFollowups(string selectedFollowupIds, string NewOwnerName)
        {
            bool result = false;
            oSessionUser = (VW_USERS)Session["UserProfile"];

            if (selectedFollowupIds != "")
            {
                string[] followups = selectedFollowupIds.Split(",".ToCharArray());

                foreach (string folowupID in followups)
                {
                    bool res = service.UpdateFollowup(Convert.ToInt32(folowupID), 0, "", "", "", Convert.ToDateTime("1/1/1900"), "", "", "Open", oSessionUser.NTLOGIN, System.DateTime.Now, NewOwnerName, "");
                }

                result = true;
            }

            return Json(result);
        }


        public JsonResult ReadFollowupHistory(int Followup_ID)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            List<Tbl_Followups_History> oItems = new List<Tbl_Followups_History>();

            if (Followup_ID != 0 && ModelState.IsValid)
            {
                oItems = service.GetFollowupHistory(Convert.ToDouble(Followup_ID)).ToList();
            }

            return Json(oItems);
        }


        [HttpPost]
        public ActionResult PushFollowup(FormCollection objCollection)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];

            string []emailTo = objCollection[0].ToString().Split(";".ToCharArray());
            string []emailCC = objCollection[1].ToString().Split(";".ToCharArray());
            string subject = objCollection[2].ToString();
            string body = objCollection[3].ToString().Replace("&gt;",">").Replace("&lt;","<");
            body = "<html><head></head><body>" + body + "</body></html>";

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("lscabc_tools@agilent.com");
            if (emailTo.Length > 0)
            {
                foreach (string strEmail in emailTo)
                {
                    try
                    {
                    msg.To.Add(new MailAddress(strEmail));
                    }
                    catch(Exception ex)
                    {}
                }
            }
            else
            {
                try
                    {
                        msg.To.Add(new MailAddress(emailTo[0]));
                    }
                    catch(Exception ex)
                    {}
            }

            if (emailCC.Length > 0)
            {
                foreach (string strEmail in emailCC)
                {
                    try
                    {
                        msg.CC.Add(new MailAddress(strEmail));
                    }
                    catch (Exception ex)
                    { }
                }
            }
            else
            {
                try
                {
                    msg.CC.Add(new MailAddress(emailCC[0]));
                }
                catch (Exception ex)
                { }
            }


            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = true;

            try
            {
                SmtpClient smtp = new SmtpClient("cos.smtp.agilent.com");
                smtp.Send(msg);
            }
            catch (Exception ex)
            {
            }

            return RedirectToAction("MyFollowupsList");
        }

        [HttpPost]
        public ActionResult ReassignFollowup(FormCollection objCollection)
        {
            //Tbl_Followups oFollowup = new Tbl_Followups();
            bool IsReassigned = false;
            Session["ReassignedMsg"] = null;
            int Followupid = Convert.ToInt32(objCollection["lblFollowupID"]);
            string Owner = objCollection["Role"].ToString();
            IsReassigned = service.ReAssignFollowup(Followupid, Owner);
            if (IsReassigned == true)
            {
                Session["ReassignedMsg"] = "Follow-Up Id " + Followupid + "successfully reassigned to " + Owner;
            }
            else
            {
                Session["ReassignedMsg"] = "Re-assign of follow-up is unsuccessfull! Please try again.";
            }

            return RedirectToAction("MyFollowupsList");
        }



        [HttpPost]
        public ActionResult UpdateFollowups(FormCollection oCollection)
        {
            try
            {
                oSessionUser = (VW_USERS)Session["UserProfile"];
                Tbl_Followups Followup = new Tbl_Followups();
                Followup.Followupid = Convert.ToInt32(oCollection["Followupid"]);
                Followup.Sales_Order = Convert.ToDouble(oCollection["Sales_Order"]);
                Followup.CustomerName = oCollection["CustomerName"].ToString();
                Followup.Description = oCollection["Description"].ToString();
                Followup.Owner = oCollection["Owner"].ToString();
                Followup.DueDate = Convert.ToDateTime(oCollection["DueDate"]);
                Followup.BacklogStatus = "";
                Followup.Comment = oCollection["Comment"].ToString();
                if (oCollection["rdoStatus"] != null)
                    Followup.Status = oCollection["rdoStatus"].ToString();
                else
                    Followup.Status = "Open";

                Followup.Modified_By = oSessionUser.NTLOGIN.ToString();
                Followup.Modified_On = DateTime.Now;
                Followup.Re_Assigned_To = "";
                Followup.Priority = oCollection["Priority"].ToString();

                bool result = service.UpdateFollowup(Followup.Followupid, Convert.ToDouble(Followup.Sales_Order), Followup.CustomerName, Followup.Description, Followup.Owner, Convert.ToDateTime(Followup.DueDate), Followup.BacklogStatus, Followup.Comment, Followup.Status, Followup.Modified_By, Convert.ToDateTime(Followup.Modified_On), Followup.Re_Assigned_To, Followup.Priority);

                Session["FolowupUpdated"] = "Followup Id : " + Followup.Followupid.ToString() + " Updated Successfully.";
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Redirect(Request.UrlReferrer.OriginalString);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BatchUpdateFollowup([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<Tbl_Followups> Followups)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];

            if (Followups != null && ModelState.IsValid)
            {
                foreach (Tbl_Followups Followup in Followups)
                {
                    Followup.Modified_By = oSessionUser.NTLOGIN;
                    Followup.Modified_On = System.DateTime.Now;

                    bool result = service.UpdateFollowup(Followup.Followupid,Convert.ToDouble(Followup.Sales_Order), Followup.CustomerName, Followup.Description, Followup.Owner, Convert.ToDateTime(Followup.DueDate), Followup.BacklogStatus, Followup.Comment, Followup.Status, Followup.Modified_By, Convert.ToDateTime(Followup.Modified_On), Followup.Re_Assigned_To, Followup.Priority);

                    
                }
            }

            return Json(Followups.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteFollowup([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<Tbl_Followups> Followups)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];

            if (Followups != null && ModelState.IsValid)
            {
                foreach (var Followup in Followups)
                {
                    //productService.Update(Followup);
                }
            }

            return Json(Followups.ToDataSourceResult(request, ModelState));
        }


        [HttpPost]
        public ActionResult FollowupDetails(int FollowupID)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            var my_Details = service.GetFollowupDetails(oSessionUser.NTLOGIN, FollowupID);

            ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
            return View(my_Details);
        }

        public ActionResult MyFollowupsList()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            oDashboardModal = (DashboardModal)Session["oDashboardModal"];

            if (oDashboardModal.UI_Type == "CSR Cockpit")
            {
                var my_Details = service.GetMyFollowupsList(oSessionUser.NTLOGIN, oSessionUser.TEAM_NAME);

                ViewData.Add("oDashboardModal", oDashboardModal);
                return View(my_Details);
            }
            else
            {
                var my_Details = service.GetTeamFollowupsList(oSessionUser.NTLOGIN, oSessionUser.TEAM_NAME);

                ViewData.Add("oDashboardModal", oDashboardModal);
                return View("MyFollowupsList", my_Details);
            }
        }

        public JsonResult MyFollowupsListJson()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
             oDashboardModal = (DashboardModal)Session["oDashboardModal"];

             if (oDashboardModal.UI_Type == "CSR Cockpit")
             {
                 var my_Details = service.GetMyFollowupsList(oSessionUser.NTLOGIN, oSessionUser.TEAM_NAME);

                 ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
                 return Json(my_Details);
             }
             else
             {
                 var my_Details = service.GetTeamFollowupsList(oSessionUser.NTLOGIN, oSessionUser.TEAM_NAME);

                 ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
                 return Json(my_Details);
             }
        }

        public ActionResult MyPasseddueDate()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
             oDashboardModal = (DashboardModal)Session["oDashboardModal"];

             if (oDashboardModal.UI_Type == "CSR Cockpit")
             {
                 var my_Details = service.GetMyFollowupsPassedDueDateList(oSessionUser.NTLOGIN, oSessionUser.TEAM_NAME);

                 ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
                 return View(my_Details);
             }
             else
             {
                 var my_Details = service.GetTeamFollowupsPassedDueDateList(oSessionUser.NTLOGIN, oSessionUser.TEAM_NAME);

                 ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
                 return View(my_Details);
             }
        }


        public ActionResult MyDueToday()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            var my_Details = service.GetMyFollowupsDueTodayList(oSessionUser.NTLOGIN, oSessionUser.TEAM_NAME);

            ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
            return View(my_Details);
        }

        public ActionResult MyReassigned()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            oDashboardModal = (DashboardModal)Session["oDashboardModal"];

            if (oDashboardModal.UI_Type == "CSR Cockpit")
            {
                var my_Details = service.GetMyFollowupsReasignedList(oSessionUser.NTLOGIN, oSessionUser.TEAM_NAME);
                ViewData.Add("oDashboardModal", oDashboardModal);
                return View(my_Details);
            }
            else
            {
                var my_Details = service.GetMyFollowupsReasignedList(oSessionUser.NTLOGIN, oSessionUser.TEAM_NAME);
                ViewData.Add("oDashboardModal", oDashboardModal);
                return View(my_Details);
            }
        }

        public ActionResult MySystemGenerated()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            oDashboardModal = (DashboardModal)Session["oDashboardModal"];

            if (oDashboardModal.UI_Type == "CSR Cockpit")
            {
                var my_Details = service.GetMyFollowupsSystemGeneratedList(oSessionUser.NTLOGIN, "");

                ViewData.Add("oDashboardModal", oDashboardModal);
                return View(my_Details);
            }
            else
            {
                var my_Details = service.GetTeamFollowupsSystemGeneratedList(oSessionUser.NTLOGIN, oSessionUser.TEAM_NAME);

                ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
                return View("MyFollowupsList", my_Details);
            }            
        }


        public ActionResult TeamFollowupsList(string catagory)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];

            var my_Details = service.GetTeamFollowupsList(oSessionUser.NTLOGIN, oSessionUser.TEAM_NAME);

            ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
            return View("MyFollowupsList", my_Details);
        }

        public ActionResult TeamPasseddueDate()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];

            var my_Details = service.GetTeamFollowupsPassedDueDateList(oSessionUser.NTLOGIN, oSessionUser.TEAM_NAME);

            ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
            return View("MyPasseddueDate", my_Details);
        }

        public ActionResult TeamDueToday()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];

            var my_Details = service.GetTeamFollowupsDueTodayList(oSessionUser.NTLOGIN, oSessionUser.TEAM_NAME);

            ViewData.Add("oDashboardModal", (DashboardModal)Session["oDashboardModal"]);
            return View("MyDueToday", my_Details);
        }
    }
}
