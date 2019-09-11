using System;
using System.ServiceModel;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

using Cockpit_NextGenMVC.BAL;
using Cockpit_NextGenMVC.BAL_User_Mgmt;
using Cockpit_NextGenMVC.Models;

using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;


namespace Cockpit_NextGenMVC.Controllers
{
    [SessionExpire]
    public class GenericController : Controller
    {
        static BAL.Service1Client service = new BAL.Service1Client();
        static BAL_User_Mgmt.Service1Client um_service = new BAL_User_Mgmt.Service1Client();

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


        void BuildSessionFilterMasters(string Region)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];

            try
            {
                lstSalesForce = service.Get_Sales_Force_Master(Region).ToList();
                lstCustomers = service.Get_Customer_Master(Region).ToList();
            }
            catch (Exception WCFEx)
            {
                //throw new Exception(WCFEx.Detail.ErrorMessage);
            }

            Session["lstSalesForce"] = lstSalesForce;
            Session["lstCustomers"] = lstCustomers;
        }

        [HttpPost]
        public JsonResult SearchGlobalOrder(string Sales_Order)
        {
            List<Tbl_Order_Search> result = service.Search_Global_Order(Convert.ToDouble(Sales_Order)).ToList();

            return Json(result);
        }


        public JsonResult ResetSessionFiltersApplied()
        {
            oDashboardModal = (DashboardModal)Session["oDashboardModal"];
            oSessionUser = (VW_USERS)Session["UserProfile"];

            #region Update Session Dasboard Model class for Session Filters used

            oDashboardModal.oSessionFilters.SalesOrder = "";
            oDashboardModal.oSessionFilters.CustomerPONumber = "";
            oDashboardModal.oSessionFilters.Sorg = "";
            oDashboardModal.oSessionFilters.OwnerName = oSessionUser.FULLNAME;
            oDashboardModal.oSessionFilters.AgingBucket = "";
            oDashboardModal.oSessionFilters.SNIAging = "";
            oDashboardModal.oSessionFilters.SNIAgingBucket = "";
            oDashboardModal.oSessionFilters.DBClosureStatus = "";
            oDashboardModal.oSessionFilters.SNIClosureStatus = "";
            oDashboardModal.oSessionFilters.SoldToAccount = "";
            oDashboardModal.oSessionFilters.ZUAccount = "";
            oDashboardModal.oSessionFilters.SalesRep = "";
            oDashboardModal.oSessionFilters.PaymentTerm = "";
            oDashboardModal.oSessionFilters.BillingBlock_Code = "";
            oDashboardModal.oSessionFilters.DeliveryBlock_Code = "";
            oDashboardModal.oSessionFilters.NLHD = "";
            oDashboardModal.oSessionFilters.DeltaLoaddateBucket = "";
            oDashboardModal.oSessionFilters.LoaDDate = "";
            oDashboardModal.oSessionFilters.TrioLoaDDate = "";
            oDashboardModal.oSessionFilters.CRDD = "";
            oDashboardModal.oSessionFilters.ExpReleaseDate = "";
            oDashboardModal.oSessionFilters.ReasonCode = "";

            oDashboardModal.oSessionFilters.Region = "";
            oDashboardModal.oSessionFilters.Business = "";
            oDashboardModal.oSessionFilters.Division = "";
            oDashboardModal.oSessionFilters.PrimaryProduct = "";
            oDashboardModal.oSessionFilters.PL = "";
            oDashboardModal.oSessionFilters.BacklogStatus = "";
            oDashboardModal.oSessionFilters.CreatedBy = "";
            oDashboardModal.oSessionFilters.DollarBucket = "";
            oDashboardModal.oSessionFilters.PaymentTerm = "";
            oDashboardModal.oSessionFilters.BillingBlock_HeaderText = "";
            oDashboardModal.oSessionFilters.DeliveryBlock_HeaderText = "";
            oDashboardModal.oSessionFilters.BillingBlock_ItemText = "";
            oDashboardModal.oSessionFilters.DeliveryBlock_ItemText = "";
            oDashboardModal.oSessionFilters.SoldToAccountID = "";
            oDashboardModal.oSessionFilters.SoldToCountry = "";
            oDashboardModal.oSessionFilters.ShipToAccountID = "";
            oDashboardModal.oSessionFilters.ShipToAccount = "";
            oDashboardModal.oSessionFilters.ShipToCountry = "";
            oDashboardModal.oSessionFilters.ZUAccountID = "";
            oDashboardModal.oSessionFilters.BTM = "";
            oDashboardModal.oSessionFilters.BTM_Manager = "";
            oDashboardModal.oSessionFilters.DeltaLoaddate = "";
            oDashboardModal.oSessionFilters.ClosuredaysDeltaFrom = 0;
            oDashboardModal.oSessionFilters.ClosuredaysDeltaTo = 0;


            Session["oDashboardModal"] = oDashboardModal;
            #endregion

            return Json("Success");
        }

        [HttpPost]
        public void SaveLocalFilters(Local_Filters Filters)
        {
            oDashboardModal = (DashboardModal)Session["oDashboardModal"];

            #region Update Session Dasboard Model class for Session Filters used
            oDashboardModal.oSessionFilters.SalesOrder = Filters.SalesOrder;
            oDashboardModal.oSessionFilters.CustomerPONumber = Filters.CustomerPONumber;
            oDashboardModal.oSessionFilters.Sorg = Filters.Sorg;
            oDashboardModal.oSessionFilters.OwnerName = Filters.OwnerName;
            oDashboardModal.oSessionFilters.AgingBucket = Filters.AgingBucket;
            oDashboardModal.oSessionFilters.SNIAging = Filters.SNIAging;
            oDashboardModal.oSessionFilters.SNIAgingBucket = Filters.SNIAgingBucket;
            oDashboardModal.oSessionFilters.DBClosureStatus = Filters.DBClosureStatus;
            oDashboardModal.oSessionFilters.SNIClosureStatus = Filters.SNIClosureStatus;
            oDashboardModal.oSessionFilters.SoldToAccount = Filters.SoldToAccount;
            oDashboardModal.oSessionFilters.ZUAccount = Filters.ZUAccount;
            oDashboardModal.oSessionFilters.SalesRep = Filters.SalesRep;
            oDashboardModal.oSessionFilters.PaymentTerm = Filters.PaymentTerm;
            oDashboardModal.oSessionFilters.BillingBlock_Code = Filters.BillingBlock;
            oDashboardModal.oSessionFilters.DeliveryBlock_Code = Filters.DeliveryBlock;
            oDashboardModal.oSessionFilters.NLHD = Filters.NLHD;
            oDashboardModal.oSessionFilters.DeltaLoaddateBucket = Filters.DeltaLoaddateBucket;
            oDashboardModal.oSessionFilters.LoaDDate = Filters.LoaDDate;
            oDashboardModal.oSessionFilters.TrioLoaDDate = Filters.TrioLoaDDate;
            oDashboardModal.oSessionFilters.CRDD = Filters.CRDD;
            oDashboardModal.oSessionFilters.ExpReleaseDate = Filters.ExpReleaseDate;

            oDashboardModal.oSessionFilters.ReasonCode = Filters.ReasonCode;
            Session["oDashboardModal"] = oDashboardModal;
            #endregion
        }

        [HttpPost]
        public ActionResult NewFollowup(FormCollection objCollection)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];

            Tbl_Followups oFollowup = new Tbl_Followups();
            oFollowup.Followupid = 0;
            oFollowup.Sales_Order = Convert.ToDouble(objCollection[0].ToString());
            oFollowup.Owner = objCollection["Followup_owner"].ToString();
            oFollowup.Status = objCollection["status"].ToString();
            oFollowup.CustomerName = objCollection["Followup_Customername"].ToString();
            oFollowup.Description = objCollection["Followup_Description"].ToString();
            oFollowup.DueDate = Convert.ToDateTime(objCollection["dt_DueDate"]);
            oFollowup.Priority = objCollection["drpPriority"].ToString();
            oFollowup.Comment = HttpUtility.HtmlEncode(objCollection["Followup_Comments"].ToString());

            oFollowup.Created_By = oSessionUser.NTLOGIN;
            oFollowup.Created_On = System.DateTime.Now;

            service.CreateFollowup(oFollowup);

            //Update Overall Followup Summary for top Bar

            oDashboardModal = (DashboardModal)Session["oDashboardModal"];

            oMyFollowups = service.GetMyFollowupsSummary(oSessionUser.NTLOGIN, "");
            oDashboardModal.FollowupSummary = oMyFollowups;

            oDashboardModal.TotalFollowups = Convert.ToInt32(oMyFollowups[0].TotalFollowups);
            oDashboardModal.FollowupSummary = oMyFollowups;

            oSystemGeneratedFollowups = service.GetMyFollowupsSystemGeneratedList(oSessionUser.NTLOGIN, "");
            oDashboardModal.SystemGeneratedFollowups = oSystemGeneratedFollowups;

            // End Update

            return Redirect(Request.UrlReferrer.OriginalString);
        }

        public JsonResult NewFollowup_AJAX(string SalesOrder, string Owner, string Status, string Customer, string Description, string DueDate, string Priority, string Comments)
        {
            bool result = true;
            try
            {
                oSessionUser = (VW_USERS)Session["UserProfile"];

                Tbl_Followups oFollowup = new Tbl_Followups();
                oFollowup.Followupid = 0;
                oFollowup.Sales_Order = Convert.ToDouble(SalesOrder);
                oFollowup.Owner = Owner;
                oFollowup.Status = Status;
                oFollowup.CustomerName = Customer;
                oFollowup.Description = Description;
                oFollowup.DueDate = Convert.ToDateTime(DueDate);
                oFollowup.Priority = Priority;
                oFollowup.Comment = HttpUtility.HtmlEncode(Comments);

                oFollowup.Created_By = oSessionUser.NTLOGIN;
                oFollowup.Created_On = System.DateTime.Now;

                service.CreateFollowup(oFollowup);
            }
            catch (Exception ex)
            {
                result = false;
            }

            return Json(result);
        }


        [HttpPost]
        public ActionResult AddOrderOwnership(FormCollection oCollection)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            string ownershipForSelected, SalesOrders, DefaultOwner, CurrentOwner, CurrentOwnerSAP, OwnershipType, Assigned_By;
            DateTime ownershipFrom, ownershipTo, Assigned_On;

            string[] RequestPath = HttpContext.Request.UrlReferrer.AbsolutePath.ToString().Split("/".ToCharArray());

            ownershipForSelected = oCollection["Ownership_For"].ToString();

            if (oCollection["lbl_OrdersSelected"] != null) SalesOrders = oCollection["lbl_OrdersSelected"].ToString(); else SalesOrders = "0";

            if (ownershipForSelected == "All Orders")
            {
                OwnershipType = oCollection["Ownership_Type"].ToString();

                if (OwnershipType.Contains("Temporary"))
                {
                    DefaultOwner = "";
                    CurrentOwnerSAP = "";

                    ownershipFrom = Convert.ToDateTime(oCollection["dt_Duration_From"]);
                    ownershipTo = Convert.ToDateTime(oCollection["dt_Duration_To"]);
                }
                else
                {
                    DefaultOwner = oCollection["cmb_ExistingOwner"].ToString();
                    CurrentOwnerSAP = oCollection["cmb_ExistingOwner"].ToString();

                    ownershipFrom = Convert.ToDateTime("1/1/1900");
                    ownershipTo = Convert.ToDateTime("1/1/1900");
                }

                CurrentOwner = oCollection["cmb_NewOwner"].ToString();

                Assigned_By = oSessionUser.NTLOGIN;
                Assigned_On = System.DateTime.Now;

                service.AddOrder_Ownership("0", DefaultOwner, CurrentOwner, CurrentOwnerSAP, OwnershipType, ownershipFrom, ownershipTo, Assigned_By, Assigned_On);
            }
            else
            {

                string[] Orders = SalesOrders.Split(",".ToCharArray());

                foreach (string order in Orders)
                {
                    OwnershipType = oCollection["Ownership_Type"].ToString();

                    if (OwnershipType.Contains("Temporary"))
                    {
                        DefaultOwner = "";
                        CurrentOwnerSAP = "";

                        ownershipFrom = Convert.ToDateTime(oCollection["dt_Duration_From"]);
                        ownershipTo = Convert.ToDateTime(oCollection["dt_Duration_To"]);
                    }
                    else
                    {
                        DefaultOwner = "";
                        CurrentOwnerSAP = "";

                        ownershipFrom = Convert.ToDateTime("1/1/1900");
                        ownershipTo = Convert.ToDateTime("1/1/1900");
                    }

                    CurrentOwner = oCollection["cmb_NewOwner"].ToString();

                    Assigned_By = oSessionUser.NTLOGIN;
                    Assigned_On = System.DateTime.Now;

                    service.AddOrder_Ownership(order, DefaultOwner, CurrentOwner, CurrentOwnerSAP, OwnershipType, ownershipFrom, ownershipTo, Assigned_By, Assigned_On);

                }
            }

            return Redirect(Request.UrlReferrer.OriginalString);
        }

        [HttpPost]
        public JsonResult AdvancedFilter(string Report, string Region, string Business, string Division, string PrimaryProduct, string PL,
            string BacklogStatus, string CreatedBy, string DollarBucket,
            string SoldToAccountID, string ShipToAccountID, string ZUAccountID, string SoldToCountry, string ShipToAccount, string ShipToCountry, 
            string BTM, string BTM_Manager, string PaymentTerm, 
            string BillingBlock_Header, string DeliveryBlock_Header, string BillingBlock_Item, string DeliveryBlock_Item,
            string DeltaLoaddate, string ClosuredaysDeltaFrom, string ClosuredaysDeltaTo)
        {
                oSessionUser = (VW_USERS)Session["UserProfile"];
                CockpitUI = Session["CockpitUI"].ToString();

                if (Region == "WW") Region = "";

                if (Business.Contains("Select")) Business = "";
                if (Division.Contains("Select")) Division = "";
                if (PrimaryProduct.Contains("Select")) PrimaryProduct = "";
                if (PL.Contains("Select")) PL = "";
                if (BacklogStatus.Contains("Select")) BacklogStatus = "";
                if (CreatedBy.Contains("Select")) CreatedBy = "";
                if (DollarBucket.Contains("Select")) DollarBucket = "";
                if (ShipToAccountID.Contains("Select")) ShipToAccountID = "";
                if (SoldToAccountID.Contains("Select")) SoldToAccountID = "";
                if (ZUAccountID.Contains("Select")) ZUAccountID = "";
                if (SoldToCountry.Contains("Select")) SoldToCountry = "";
                if (ShipToAccount.Contains("Select")) ShipToAccount = "";
                if (ShipToCountry.Contains("Select")) ShipToCountry = "";
                if (BTM.Contains("Select")) BTM = "";
                if (BTM_Manager.Contains("Select")) BTM_Manager = "";
                if (PaymentTerm.Contains("Select")) PaymentTerm = "";
                if (BillingBlock_Header.Contains("Select")) BillingBlock_Header = "";
                if (DeliveryBlock_Header.Contains("Select")) DeliveryBlock_Header = "";
                if (BillingBlock_Item.Contains("Select")) BillingBlock_Item = "";
                if (DeliveryBlock_Item.Contains("Select")) DeliveryBlock_Item = "";
                if (ClosuredaysDeltaFrom == "") { ClosuredaysDeltaFrom = "0"; }
                if (ClosuredaysDeltaTo == "") ClosuredaysDeltaTo = "0";


                oDashboardModal = (DashboardModal)Session["oDashboardModal"];

                if (oDashboardModal.oSessionFilters == null)
                {
                    oDashboardModal.oSessionFilters = new  Models.Session_Filters();
                }

                #region Update Session Dasboard Model class for Session Filters used
                if (oDashboardModal.oSessionFilters.OrderOwner == null) oDashboardModal.oSessionFilters.OrderOwner = oSessionUser.FULLNAME;
                oDashboardModal.oSessionFilters.Region = Region;
                oDashboardModal.oSessionFilters.Business = Business;
                oDashboardModal.oSessionFilters.Division = Division;
                oDashboardModal.oSessionFilters.PrimaryProduct = PrimaryProduct;
                oDashboardModal.oSessionFilters.PL = PL;
                oDashboardModal.oSessionFilters.BacklogStatus = BacklogStatus;
                oDashboardModal.oSessionFilters.CreatedBy = CreatedBy;
                oDashboardModal.oSessionFilters.DollarBucket = DollarBucket;
                oDashboardModal.oSessionFilters.PaymentTerm = PaymentTerm;
                oDashboardModal.oSessionFilters.BillingBlock_HeaderText = BillingBlock_Header;
                oDashboardModal.oSessionFilters.DeliveryBlock_HeaderText = DeliveryBlock_Header;
                oDashboardModal.oSessionFilters.BillingBlock_ItemText = BillingBlock_Item;
                oDashboardModal.oSessionFilters.DeliveryBlock_ItemText = DeliveryBlock_Item;
                oDashboardModal.oSessionFilters.SoldToAccountID = SoldToAccountID;
                oDashboardModal.oSessionFilters.SoldToCountry = SoldToCountry;
                oDashboardModal.oSessionFilters.ShipToAccountID = ShipToAccountID;
                oDashboardModal.oSessionFilters.ShipToAccount = ShipToAccount;
                oDashboardModal.oSessionFilters.ShipToCountry = ShipToCountry;
                oDashboardModal.oSessionFilters.ZUAccountID = ZUAccountID;
                oDashboardModal.oSessionFilters.BTM = BTM;
                oDashboardModal.oSessionFilters.BTM_Manager = BTM_Manager;
                oDashboardModal.oSessionFilters.DeltaLoaddate = DeltaLoaddate;
                oDashboardModal.oSessionFilters.ClosuredaysDeltaFrom = Convert.ToInt32(ClosuredaysDeltaFrom);
                oDashboardModal.oSessionFilters.ClosuredaysDeltaTo = Convert.ToInt32(ClosuredaysDeltaTo);
                Session["oDashboardModal"] = oDashboardModal;
                #endregion

                List<VW_Orders_Info> lst_SNIResult = new List<VW_Orders_Info>();
                BAL.Session_Filters oFil = oDashboardModal.oSessionFilters.Set_ServiceFilters();

                switch (Report)
                {
                    case "SNI All Orders":
                        lst_SNIResult = Get_AllSNI_Orders(lst_SNIResult, oFil);

                        break;
                    case "Aged SNI Orders":
                        lst_SNIResult = Get_Catagory_SNI_Orders(lst_SNIResult, oFil, "SNI_MF");

                        break;
                    case "SNI Invoicing Errors":
                        lst_SNIResult = Get_Catagory_SNI_Orders(lst_SNIResult, oFil, "SNI_InvoicingError");
                        break;
                    case "SNI Release Passed":
                        lst_SNIResult = Get_Catagory_SNI_Orders(lst_SNIResult, oFil, "SNI_ExpectedReleasePassed");
                        break;
                    case "SNI Release Due Today":
                        lst_SNIResult = Get_Catagory_SNI_Orders(lst_SNIResult, oFil, "SNI_ExpectedReleaseToday");
                        break;
                    case "SNI Release No Release Date":
                        lst_SNIResult = Get_Catagory_SNI_Orders(lst_SNIResult, oFil, "SNI_No_ExpectedRelease");
                        break;
                    case "DB All Orders":
                        lst_SNIResult = Get_AllDB_Orders(lst_SNIResult, oFil);
                        break;
                    case "DB Overdue Orders":
                        lst_SNIResult = service.Get_DB_Overdue(oFil,
                            oFil.Report, oFil.CockpitUI).ToList();
                        break;
                    case "DB Aged > 90 Orders":
                        lst_SNIResult = service.Get_DB_Greater_90_days(oFil,
                            oFil.Report, oFil.CockpitUI).ToList();
                        break;
                    case "DB ReleasePassed Orders":
                        lst_SNIResult = service.Get_DB_Expected_Release_Passed(oFil,
                            oFil.Report, oFil.CockpitUI).ToList();
                        break;
                    case "DB Overdue in 14 Days Orders":
                        lst_SNIResult = service.Get_DB_Overdue_14_Days(oFil,
                            oFil.Report, oFil.CockpitUI).ToList();
                        break;
                    case "All Unblocked Orders":
                        lst_SNIResult = service.Get_Unblocked_All(oFil,
                            oFil.Report, oFil.CockpitUI).ToList();
                        break;
                    case "Unblocked Overdue Orders":
                        //lst_SNIResult = service.GetUnblocked_Overdue(oFil, oFil.Report, oFil.CockpitUI).ToList();
                        break;
                }

            return Json(lst_SNIResult);
        }

        private static List<VW_Orders_Info> Get_AllDB_Orders(List<VW_Orders_Info> lst_SNIResult, BAL.Session_Filters oFil)
        {
            lst_SNIResult =
                service.Get_DB_RawOrders(oFil,
                    oFil.Report, oFil.CockpitUI).ToList();
            return lst_SNIResult;
        }

        private static List<VW_Orders_Info> Get_Catagory_SNI_Orders(List<VW_Orders_Info> lst_SNIResult, BAL.Session_Filters oFil, string CatagoryName)
        {
            lst_SNIResult =
                service.Get_SNICatagoryOrders(oFil,
                    CatagoryName, oFil.CockpitUI).ToList();
            return lst_SNIResult;
        }

        private static List<VW_Orders_Info> Get_AllSNI_Orders(List<VW_Orders_Info> lst_SNIResult, BAL.Session_Filters oFil)
        {
            lst_SNIResult =
                service.Get_SNIAllOrders(oFil,
                    oFil.Report, oFil.CockpitUI).ToList();
            return lst_SNIResult;
        }

        /* Commented Code 
         
        [HttpPost]
        //public ActionResult AddComment(FormCollection oCollection)
        //{
        //    oSessionUser = (VW_USERS)Session["UserProfile"];

        //    Tbl_Order_comments oComment = new Tbl_Order_comments();
        //    if (oCollection["cmb_Materials"] != null) // Material Level commenting
        //    {
        //        string[] MultipleMaterials = oCollection["cmb_Materials"].ToString().Split(",".ToCharArray());
        //        foreach (string strMaterial in MultipleMaterials)
        //        {
        //            try
        //            {
        //                oComment.Report = Session["Report"].ToString();
        //                oComment.Sales_Ord = Convert.ToDouble(oCollection["cmt_lblSalesOrder"]);
        //                oComment.Material = strMaterial;
        //                oComment.OrderOwner = oCollection["cmt_lblorderowner"].ToString();
        //                oComment.Reason_Code = oCollection["cmt_ReasonCode"].ToString();
        //                oComment.NextAction = "";
        //                if (oCollection["cmt_ReviewDate"] != null && oCollection["cmt_ReviewDate"] != "") oComment.Reviewdate = Convert.ToDateTime(oCollection["cmt_ReviewDate"]);
        //                if (oCollection["cmt_ClearDate"] != null && oCollection["cmt_ClearDate"] != "") oComment.Cleardate = Convert.ToDateTime(oCollection["cmt_ClearDate"]);
        //                oComment.Comment = oCollection["cmt_Comments"].ToString();
        //                oComment.SignOff = "No";
        //                oComment.Commented_By = oSessionUser.NTLOGIN;
        //                oComment.SignOff_By = "";

        //                service.Add_OrderComment(oComment, oSessionUser.NTLOGIN);
        //            }
        //            catch (Exception ex)
        //            { }
        //        }
        //    }
        //    else // Header Level commenting
        //    {
        //        if (oCollection["cmt_lblSalesOrder"] != null && oCollection["cmt_Comments"] != null)
        //        {
        //            try
        //            {
        //                if (Session["Report"] != null)
        //                    oComment.Report = Session["Report"].ToString();
        //                else
        //                    oComment.Report = oCollection[0].Replace("- Key Control", "").Trim(); //Bringing reprot name from help description

        //                oComment.Sales_Ord = Convert.ToDouble(oCollection["cmt_lblSalesOrder"]);
        //                oComment.Material = "";
        //                oComment.OrderOwner = oCollection["cmt_lblorderowner"].ToString();
        //                oComment.Reason_Code = oCollection["cmt_ReasonCode"].ToString();
        //                oComment.NextAction = "";
        //                if (oCollection["cmt_ReviewDate"] != null && oCollection["cmt_ReviewDate"] != "") oComment.Reviewdate = Convert.ToDateTime(oCollection["cmt_ReviewDate"]);
        //                if (oCollection["cmt_ClearDate"] != null && oCollection["cmt_ClearDate"] != "") oComment.Cleardate = Convert.ToDateTime(oCollection["cmt_ClearDate"]);
        //                oComment.Comment = oCollection["cmt_Comments"].ToString();
        //                oComment.SignOff = "No";
        //                oComment.Commented_By = oSessionUser.NTLOGIN;
        //                oComment.SignOff_By = "";

        //                service.Add_OrderComment(oComment, oSessionUser.NTLOGIN);
        //            }
        //            catch (Exception ex)
        //            { }
        //        }
        //    }

        //    return Redirect(Request.UrlReferrer.OriginalString);
        //}

        */

        public JsonResult GetTeamsByRegion(string RegionName)
        {
            var oresult = um_service.GetTeamMaster(RegionName);

            return Json(oresult);
        }

        public JsonResult GetCountryByRegion(string Region)
        {
            var userProfile = um_service.GetCountryByRegion(Region);

            return Json(userProfile);
        }


        public JsonResult GetSummaryBucket(string Area, string BucketName)
        {
            List<Tbl_WW_Blocked_Orders_Summary> result = service.WW_Process_Summary(Area, BucketName).ToList();

            ViewBag.BucketName = BucketName;
            return Json(result);
        }


        public JsonResult AddComment_Ajax(string SalesOrder, string Materials, string Sales_Org, string Sold_To_Customer, string Order_Date, string Net_Value, string OrderOwner, string ReasonCode, string ReviewDate, string ApprovedToDate, string ClearDate, string Comments, string ReportHelpDescr)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            string result = "Success";

            Tbl_Order_comments oComment = new Tbl_Order_comments();

            if (Materials != null) // Material Level commenting
            {
                string[] MultipleMaterials = Materials.Split(",".ToCharArray());
                foreach (string strMaterial in MultipleMaterials)
                {
                    try
                    {
                        if (SalesOrder.Contains(","))
                        {
                            string[] Orders = SalesOrder.Split(",".ToCharArray());

                            foreach (string Order in Orders)
                            {
                                SetMaterialComment(OrderOwner, ReasonCode, ReviewDate, ApprovedToDate, ClearDate, Comments, ReportHelpDescr, oComment, Order, strMaterial, Sales_Org, Sold_To_Customer, Order_Date, Net_Value);
                                service.Add_OrderComment(oComment, oSessionUser.NTLOGIN);
                            }
                        }
                        else
                        {
                            SetMaterialComment(OrderOwner, ReasonCode, ReviewDate, ApprovedToDate, ClearDate, Comments, ReportHelpDescr, oComment, SalesOrder, strMaterial, Sales_Org, Sold_To_Customer, Order_Date, Net_Value);
                            service.Add_OrderComment(oComment, oSessionUser.NTLOGIN);
                        }
                    }
                    catch (Exception ex)
                    { result = ex.Message.ToString(); }
                }
            }
            else // Header Level commenting
            {
                if (SalesOrder != null && Comments != null)
                {
                    try
                    {
                        if (SalesOrder.Contains(","))
                        {
                            string[] Orders = SalesOrder.Split(",".ToCharArray());

                            foreach (string Order in Orders)
                            {
                                SetHeaderComment(OrderOwner, ReasonCode, ReviewDate, ApprovedToDate, ClearDate, Comments, ReportHelpDescr, oComment, Order);
                                service.Add_OrderComment(oComment, oSessionUser.NTLOGIN);
                            }
                        }
                        else
                        {
                            SetHeaderComment(OrderOwner, ReasonCode, ReviewDate, ApprovedToDate, ClearDate, Comments, ReportHelpDescr, oComment, SalesOrder);
                            service.Add_OrderComment(oComment, oSessionUser.NTLOGIN);
                        }
                    }
                    catch (Exception ex)
                    { result = ex.Message.ToString(); }
                }
            }

            return Json(result);
        }

        private void SetHeaderComment(string OrderOwner, string ReasonCode, string ReviewDate, string ApprovedToDate,
            string ClearDate, string Comments, string ReportHelpDescr, Tbl_Order_comments oComment, string Order)
        {
            oComment.Report = ReportHelpDescr.Replace("- Key Control", "").Trim(); //Bringing reprot name from help description
            oComment.Sales_Ord = Convert.ToDouble(Order);
            oComment.Material = "";
            oComment.OrderOwner = OrderOwner;
            oComment.Reason_Code = ReasonCode;
            oComment.NextAction = "";
            if (ReviewDate != null && ReviewDate != "") oComment.Reviewdate = Convert.ToDateTime(ReviewDate);
            if (ApprovedToDate != null && ApprovedToDate != "") oComment.Approved_To_Date = Convert.ToDateTime(ApprovedToDate);
            if (ClearDate != null && ClearDate != "") oComment.Cleardate = Convert.ToDateTime(ClearDate);
            oComment.Comment = Comments;
            oComment.SignOff = "No";
            oComment.Commented_By = oSessionUser.NTLOGIN;
            oComment.SignOff_By = "";
        }

        private void SetMaterialComment(string OrderOwner, string ReasonCode, string ReviewDate, string ApprovedToDate,
            string ClearDate, string Comments, string ReportHelpDescr, Tbl_Order_comments oComment, string Order,
            string strMaterial, string Sales_Org, string SoldToCustomer, string Order_Date, string Net_Value)
        {
            oComment.Report = ReportHelpDescr.Replace("- Key Control", "").Trim();
            oComment.Sales_Ord = Convert.ToDouble(Order);
            oComment.Material = strMaterial;
            oComment.Line_Item = 0;
            oComment.Sales_Org = Sales_Org;
            oComment.Sold_To_Customer_Name = SoldToCustomer;
            oComment.Net_Value = Convert.ToDouble(Net_Value);
            oComment.Order_Date = Convert.ToDateTime(Order_Date);
            oComment.OrderOwner = OrderOwner;
            oComment.Reason_Code = ReasonCode;
            oComment.NextAction = "";
            if (ReviewDate != null && ReviewDate != "") oComment.Reviewdate = Convert.ToDateTime(ReviewDate);
            if (ApprovedToDate != null && ApprovedToDate != "") oComment.Approved_To_Date = Convert.ToDateTime(ApprovedToDate);
            if (ClearDate != null && ClearDate != "") oComment.Cleardate = Convert.ToDateTime(ClearDate);
            oComment.Comment = Comments.ToString();
            oComment.SignOff = "No";
            oComment.Commented_By = oSessionUser.NTLOGIN;
            oComment.SignOff_By = "";
        }

        public JsonResult SelectedReportData(string ReportName)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];

            List<Model_Bars> oDetailsResultbyReport = new List<Model_Bars>();
            Tbl_Daily_Control_Reports_Summary[] BCRSummary;

            if (Session["BCRSummary"] == null)
            {
                if (oSessionUser.ROLE_DESC == "CSR")
                {
                    BCRSummary = service.GetMyControlReportsSummary(oSessionUser.FULLNAME, oSessionUser.SUPERREGION, oSessionUser.ROLE_DESC, "");
                }
                else
                {
                    BCRSummary = service.GetTeamControlReportsSummary(oSessionUser.FULLNAME, oSessionUser.SUPERREGION, oSessionUser.ROLE_DESC, oSessionUser.TEAM_NAME);
                }
                Session["BCRSummary"] = BCRSummary;
            }
            else
                BCRSummary = (Tbl_Daily_Control_Reports_Summary[])Session["BCRSummary"];

            List<Backlogstats> listStatsbyCSR = new List<Backlogstats>();
            string[] UniqueReports2;

            if (oSessionUser.ROLE_DESC == "WW Lead")
            {
                listStatsbyCSR = (from tbl in BCRSummary
                                  where tbl.ReportName == ReportName
                                  group tbl by new { tbl.Region } into g
                                  select new Backlogstats
                                  {
                                      Region = g.Key.Region,
                                      PendingComments = (int)g.Sum(s => s.Pending_Comments),
                                      PendingReview = (int)g.Sum(s => s.Pending_Review),
                                      PendingSignOff = (int)g.Sum(s => s.Pending_Sign_Off)
                                  }).ToList<Backlogstats>();
                UniqueReports2 = listStatsbyCSR.Select(p => p.Region).Distinct().ToArray();
            }
            else
            {
                listStatsbyCSR = (from tbl in BCRSummary
                                  where tbl.ReportName == ReportName
                                  group tbl by new { tbl.OrderOwner } into g
                                  select new Backlogstats
                                  {
                                      OrderOwner = g.Key.OrderOwner,
                                      ReportName = ReportName,
                                      PendingComments = (int)g.Sum(s => s.Pending_Comments),
                                      PendingReview = (int)g.Sum(s => s.Pending_Review),
                                      PendingSignOff = (int)g.Sum(s => s.Pending_Sign_Off)
                                  }).ToList<Backlogstats>();
                UniqueReports2 = listStatsbyCSR.Select(p => p.Region).Distinct().ToArray();
            }


            double[] PendingComments2 = listStatsbyCSR.Select(p => p.PendingComments).ToArray();
            double[] PendingReview2 = listStatsbyCSR.Select(p => p.PendingReview).ToArray();
            double[] PendingSignOff2 = listStatsbyCSR.Select(p => p.PendingSignOff).ToArray();



            Model_Bars oBar = new Model_Bars();
            oBar.category = "PendingComments2";
            oBar.value = PendingComments2;
            oDetailsResultbyReport.Add(oBar);

            oBar = new Model_Bars();
            oBar.category = "PendingReview2";
            oBar.value = PendingReview2;
            oDetailsResultbyReport.Add(oBar);

            oBar = new Model_Bars();
            oBar.category = "PendingSignOff2";
            oBar.value = PendingSignOff2;
            oDetailsResultbyReport.Add(oBar);

            oBar = new Model_Bars();
            oBar.category = "UniqueReports2";
            oBar.othervalue = UniqueReports2;
            oDetailsResultbyReport.Add(oBar);

            oBar = new Model_Bars();
            oBar.category = "RawData2";
            oBar.RawData = listStatsbyCSR;
            oDetailsResultbyReport.Add(oBar);

            return Json(oDetailsResultbyReport);
        }

        public JsonResult ReadOrderTabsInfo(string Sales_Order)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            OrderAttributes oAttributes = new OrderAttributes();

            List<Tbl_Order_comments> oComments = new List<Tbl_Order_comments>();
            List<Tbl_Order_Header_Details> oHeader = new List<Tbl_Order_Header_Details>();
            List<Tbl_Order_Items> oItems = new List<Tbl_Order_Items>();
            List<Tbl_Order_Partner_Details> oPartner = new List<Tbl_Order_Partner_Details>();
            List<Tbl_Order_Delivery_Info> oDelivery = new List<Tbl_Order_Delivery_Info>();
            List<Tbl_Order_Block_Details> oBlock = new List<Tbl_Order_Block_Details>();

            List<Tbl_Followups> oOpenFollowup = new List<Tbl_Followups>();
            List<Tbl_Followups> oClosedFollowup = new List<Tbl_Followups>();

            if (Sales_Order != "" && ModelState.IsValid)
            {
                oComments = service.GetDB_MasterFocusChildOrders(Sales_Order).ToList();
                oHeader = service.GetHeaderInfo(Convert.ToDouble(Sales_Order)).ToList();
                oItems = service.GetLineItems(Convert.ToDouble(Sales_Order)).ToList();
                oPartner = service.GetPartnerInfo(Convert.ToDouble(Sales_Order)).ToList();
                oDelivery = service.GetDeliveryInfo(Convert.ToDouble(Sales_Order)).ToList();
                oBlock = service.GetBlockInfo(Convert.ToDouble(Sales_Order)).ToList();

                oOpenFollowup = service.GetFollowupSummaryDeatilsBySalesOrder(Convert.ToDouble(Sales_Order), "Open").ToList();
                oClosedFollowup = service.GetFollowupSummaryDeatilsBySalesOrder(Convert.ToDouble(Sales_Order), "Closed").ToList();

                oAttributes.Header = oHeader;
                oAttributes.Items = oItems;
                oAttributes.Block = oBlock;
                oAttributes.Delivery = oDelivery;
                oAttributes.Partner = oPartner;
                oAttributes.Comments = oComments;
                oAttributes.OpenFollowup = oOpenFollowup;
                oAttributes.ClosedFollowup = oClosedFollowup;
            }

            return Json(oAttributes);
        }


        public JsonResult GetFollowUpSummaryBySalesOrderOpen(int Sales_Order)
        {
            double salesorderno = (double)Sales_Order;
            string status = "Open";
            List<Tbl_Followups> oresult = service.GetFollowupSummaryDeatilsBySalesOrder(salesorderno, status).ToList();

            return Json(oresult);
        }

        public JsonResult GetFollowUpSummaryBySalesOrderClose(int Sales_Order)
        {
            double salesorderno = (double)Sales_Order;
            string status = "Closed";
            List<Tbl_Followups> oresult = service.GetFollowupSummaryDeatilsBySalesOrder(salesorderno, status).ToList();

            return Json(oresult);
        }

        public JsonResult ReadSelectedOrder(string Sales_Order)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            List<Tbl_Order_comments> oComments = new List<Tbl_Order_comments>();

            if (Sales_Order != "" && ModelState.IsValid)
            {
                oComments = service.GetDB_MasterFocusChildOrders(Sales_Order).OrderByDescending(p => p.Comment_Date).ToList();
                ViewData.Add("CommentsHistory", oComments);
            }

            return Json(oComments);
        }

        public JsonResult ReadOrderHeaderInfo(string Sales_Order)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            List<Tbl_Order_Header_Details> oItems = new List<Tbl_Order_Header_Details>();

            if (Sales_Order != "" && ModelState.IsValid)
            {
                oItems = service.GetHeaderInfo(Convert.ToDouble(Sales_Order)).ToList();
            }

            return Json(oItems);
        }

        public JsonResult ReadOrderLineItems(string Sales_Order)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            List<Tbl_Order_Items> oItems = new List<Tbl_Order_Items>();

            if (Sales_Order != "" && ModelState.IsValid)
            {
                oItems = service.GetLineItems(Convert.ToDouble(Sales_Order)).ToList();
            }

            return Json(oItems);
        }

        public JsonResult ReadOrderPartnerInfo(string Sales_Order)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            List<Tbl_Order_Partner_Details> oItems = new List<Tbl_Order_Partner_Details>();

            if (Sales_Order != "" && ModelState.IsValid)
            {
                oItems = service.GetPartnerInfo(Convert.ToDouble(Sales_Order)).ToList();
            }

            return Json(oItems);
        }

        public JsonResult ReadOrderDeliveryInfo(string Sales_Order)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            List<Tbl_Order_Delivery_Info> oItems = new List<Tbl_Order_Delivery_Info>();

            if (Sales_Order != "" && ModelState.IsValid)
            {
                oItems = service.GetDeliveryInfo(Convert.ToDouble(Sales_Order)).ToList();
            }

            return Json(oItems);
        }

        public JsonResult ReadOrderBlockInfo(string Sales_Order)
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            List<Tbl_Order_Block_Details> oItems = new List<Tbl_Order_Block_Details>();

            if (Sales_Order != "" && ModelState.IsValid)
            {
                oItems = service.GetBlockInfo(Convert.ToDouble(Sales_Order)).ToList();
            }

            return Json(oItems);
        }

        public JsonResult GetFilterValues(string RegionFilter)
        {
            BuildSessionFilterMasters(RegionFilter);

            List<List<string>> FinalResult = new List<List<string>>();
            List<string> oResult;

            oResult = new List<string>();
            oResult = lstSalesForce.Select(p => p.SRTATTR_AREA_MGR_NME).Distinct().ToList();
            FinalResult.Add(oResult);

            oResult = new List<string>();
            oResult = lstSalesForce.Select(p => p.SRTATTR_DISTRICT_MGR_NME).Distinct().ToList();
            FinalResult.Add(oResult);

            oResult = new List<string>();
            oResult = lstCustomers.Select(p => p.SHIP_TO_COUNTRY).Distinct().ToList();
            FinalResult.Add(oResult);

            oResult = new List<string>();
            oResult = lstCustomers.Select(p => p.SOLD_TO_COUNTRY).Distinct().ToList();
            FinalResult.Add(oResult);


            return Json(FinalResult);
        }


        public JsonResult GetRegionTeams()
        {
            oSessionUser = (VW_USERS)Session["UserProfile"];
            var userProfile = um_service.GetUsersByRegion(oSessionUser.SUPERREGION);

            List<MultiSelectResult> oresult = userProfile.Select(p => new MultiSelectResult { Text = p.TEAM_NAME, Value = p.TEAM_NAME }).Distinct().ToList();

            return Json(oresult);
        }

    }

    public class MultiSelectResult
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
}
