using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BAL_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
        Cockpit_NewGenerationEntities db = new Cockpit_NewGenerationEntities();
        //DataModel_CF db1 = new DataModel_CF();

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        #region User Management

        public List<VW_USERS> GetUserDetails(string NtLogin)
        {
            var userDtls = from user in db.VW_USERS
                           where user.NTLOGIN == NtLogin
                           select user;
            List<VW_USERS> objResult = userDtls.ToList();

            return objResult;
        }

        public List<VW_USERS> GetUserTeamDetails(string TeamName)
        {

            var userDtls = from user in db.VW_USERS
                           where user.TEAM_NAME == TeamName && user.ACTIVE == "True"
                           select user;
            List<VW_USERS> objResult = userDtls.ToList();

            return objResult;
        }

        public List<VW_USERS> GetUserRegionDetails(string RegionName)
        {

            var userDtls = from user in db.VW_USERS
                           where user.SUPERREGION == RegionName && user.ACTIVE == "True"
                           select user;
            List<VW_USERS> objResult = userDtls.ToList();

            return objResult;
        }


        public List<VW_USERS> GetInActiveUserTeamDetails(string TeamName)
        {
            var userDtls = from user in db.VW_USERS
                           where user.TEAM_NAME == TeamName && user.ACTIVE == "False"
                           select user;
            List<VW_USERS> objResult = userDtls.ToList();

            return objResult;
        }

        public bool RegisterNewUser(TBL_USERS oUser)
        {
            bool result = false;

            try
            {
                db.usp_CreateNewUser(oUser.USERNAME, oUser.ROLE_ID, oUser.TEAM_ID, oUser.NTLOGIN, oUser.SUPERREGION, oUser.COUNTRY, oUser.EMAIL, oUser.FULLNAME, oUser.PROFILE_PIC);
                db.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {
                ServiceException exception = new ServiceException();

                exception.Result = false;
                exception.ErrorMessage = "Error Occured";
                exception.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exception, ex.ToString());
            }

            return result;
        }

        public bool UpdateUserDetails(TBL_USERS oUser)
        {
            bool result = false;

            try
            {

                db.usp_UpdateUser(oUser.USER_ID, oUser.NTLOGIN, oUser.USERNAME, oUser.TEAM_ID, oUser.ROLE_ID, oUser.EMAIL, oUser.FULLNAME, oUser.PROFILE_PIC);

                db.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public List<VW_USERS> GetUsersByRegion(string Region)
        {
            List<VW_USERS> oresult = db.VW_USERS.Where(p => p.SUPERREGION == Region).Distinct().ToList();

            return oresult;
        }

        public string getEmailIDByName(string name)
        {
            List<VW_USERS> oresult = db.VW_USERS.Where(p => p.FULLNAME == name).Distinct().ToList();
            string Manageremail = oresult[0].EMAIL.ToString();
            return Manageremail;
        }

        public List<Tbl_Unmapped_Users> GetUnMappedUsersByRegion(string Region)
        {
            List<Tbl_Unmapped_Users> oresult = db.Usp_Unmapped_Users_Summary(Region).ToList();

            return oresult;
        }

        public List<Tbl_Unmapped_Orders_By_Region_Function> GetUnMappedUsersByRegionFunction(string Region, string BusinessFunction)
        {
            List<Tbl_Unmapped_Orders_By_Region_Function> oresult = new List<Tbl_Unmapped_Orders_By_Region_Function>();
            //    oresult = db.Usp_Unmapped_User_DetailsByFunction(Region, BusinessFunction).ToList();

            return oresult;
        }

        public List<Tbl_Country_Sorg_Orig> GetCountryByRegion(string Region)
        {
            List<Tbl_Country_Sorg_Orig> oresult = db.Tbl_Country_Sorg_Orig.Where(p => p.Region == Region).Distinct().ToList();

            return oresult;
        }

        public bool ActiveDeactiveUser(TBL_USERS oUser)
        {
            bool result = false;

            try
            {

                db.usp_ActiveDeactiveUser(oUser.USER_ID);

                db.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public List<TBL_TEAM_STRUCTURE> GetManagerByTeamID(int TeamID)
        {

            var userDtls = db.TBL_TEAM_STRUCTURE.Where(p => p.TEAM_ID == TeamID).ToList();
            List<TBL_TEAM_STRUCTURE> objResult = userDtls.ToList();

            return objResult;
        }

        public List<TBL_ROLE> GetRoleMaster()
        {
            return db.TBL_ROLE.ToList();
        }

        public List<TBL_TEAM_STRUCTURE> GetTeamMaster(string Region)
        {
            if (Region == "WW" || Region == "")
            {
                return db.TBL_TEAM_STRUCTURE.ToList();
            }
            else
            {
                List<TBL_TEAM_STRUCTURE> lst_results = new List<TBL_TEAM_STRUCTURE>();

                var unique_Teams = (from t1 in db.VW_USERS
                                    where t1.SUPERREGION == Region
                                    select t1.TEAM_NAME).Distinct();
                foreach (string Team in unique_Teams)
                {
                    TBL_TEAM_STRUCTURE tmp_Team = (from t2 in db.TBL_TEAM_STRUCTURE where t2.TEAM_NAME == Team select t2).FirstOrDefault();
                    lst_results.Add(tmp_Team);
                }

                return lst_results;
            }
        }


        public List<VW_USERS> GetNTLoginBySAPName(string SAPName)
        {
            List<VW_USERS> oresult = db.VW_USERS.Where(p => p.SAP_User_Name == SAPName).Distinct().ToList();

            return oresult;
        }

        public bool UpdateUserInSApandBBZTRD(string OrderOwner, string CreatedBy)
        {
            bool result = false;

            try
            {
                db.usp_UpdateUserInSAPZTRD(OrderOwner, CreatedBy);
                db.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        #endregion

        #region Generic Methods

        public List<Tbl_Order_Header_Details> GetHeaderInfo(double Sales_Order)
        {
            List<Tbl_Order_Header_Details> result = new List<Tbl_Order_Header_Details>();
            result = db.usp_GetOrder_Header_Info(Sales_Order).ToList();
            return result;
        }

        public List<Tbl_Order_Items> GetLineItems(double Sales_Order)
        {
            List<Tbl_Order_Items> lstresult = db.usp_GetOrder_LineItems(Sales_Order).ToList();

            return lstresult;
        }

        public List<Tbl_Order_Delivery_Info> GetDeliveryInfo(double Sales_Order)
        {
            List<Tbl_Order_Delivery_Info> result = new List<Tbl_Order_Delivery_Info>();
            result = db.usp_GetOrder_Delivery_Info(Sales_Order).ToList();
            return result;
        }

        public List<Tbl_Order_Partner_Details> GetPartnerInfo(double Sales_Order)
        {
            List<Tbl_Order_Partner_Details> result = new List<Tbl_Order_Partner_Details>();
            result = db.usp_GetOrder_Partner_Info(Sales_Order).ToList();
            return result;
        }

        public List<Tbl_Order_Block_Details> GetBlockInfo(double Sales_Order)
        {
            List<Tbl_Order_Block_Details> result = new List<Tbl_Order_Block_Details>();
            result = db.usp_GetOrder_Block_Info(Sales_Order).ToList();
            return result;
        }

        public void AddOrder_Ownership(string SalesOrder, string DefaultOwner, string CurrentOwner, string CurrentOwnerSAP, string Ownership_Type, DateTime Ownership_From, DateTime Ownership_To, string Assigned_By, DateTime Assigned_On)
        {
            db.usp_Add_Order_Ownership(SalesOrder, DefaultOwner, CurrentOwner, CurrentOwnerSAP, Ownership_Type, Ownership_From, Ownership_To, Assigned_By, Assigned_On);
        }

        public List<Tbl_Review_Reports> Get_Reports_Master()
        {
            return db.Tbl_Review_Reports.ToList();
        }

        public List<VW_Orders_Info> Get_Archived_Orders(string CustomerName, string OrderOwner)
        {
            List<VW_Orders_Info> result = new List<VW_Orders_Info>();

            if (CustomerName == null) CustomerName = "";

            result = db.usp_Get_Archived_Orders(CustomerName, OrderOwner).ToList();

            return result;
        }

        public List<Tbl_Archival_Summary> Get_Archived_Orders_Summary(string Region, string Sorg, string Business, string Team)
        {
            List<Tbl_Archival_Summary> result = new List<Tbl_Archival_Summary>();

            result = db.usp_Get_Archived_Orders_Summary(Region, Sorg, Business, Team).ToList();

            return result;
        }

        public List<Dim_Business_Master> Get_Business_Master()
        {
            List<Dim_Business_Master> oResult = new List<Dim_Business_Master>();

            oResult = db.Dim_Business_Master.ToList();

            return oResult;
        }

        public List<Dim_Billing_Blocks> Get_Billing_Blocks_Master()
        {
            return db.Dim_Billing_Blocks.ToList();
        }

        public List<Dim_Delivery_Blocks> Get_DeliveryBlocks_Master()
        {
            return db.Dim_Delivery_Blocks.ToList();
        }

        public List<Dim_Sales_Force> Get_Sales_Force_Master(string Region)
        {
            try
            {
                return db.usp_Get_Region_SalesForce(Region).ToList();
            }
            catch (Exception ex)
            {
                ServiceException exception = new ServiceException();

                exception.Result = false;
                exception.ErrorMessage = "Error Occured";
                exception.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exception, ex.ToString());
            }
        }

        public List<Dim_Customers> Get_Customer_Master(string Region)
        {
            var result = new List<Dim_Customers>();
            result = db.usp_Get_Region_Customers(Region).ToList();
            return result;
        }

        public string Translate_Comment_From_Google(string strComment)
        {
            throw new NotImplementedException();
        }

        public string Translate_Comment_From_Microsoft(string strComment)
        {
            throw new NotImplementedException();
        }

        public bool Translate_Mass_Comment_From_Microsoft(string strCommentedBy, DateTime CommentedOn)
        {
            throw new NotImplementedException();
        }


        public List<Tbl_Order_Search> Search_Global_Order(double Sales_Order)
        {
            List<Tbl_Order_Search> result = new List<Tbl_Order_Search>();

            result = db.usp_Search_Order(Sales_Order).ToList();

            return result;
        }

        #endregion

        #region Billing Blocked Related

        public List<VW_Orders_Info> Get_SNIAllOrders(Session_Filters oSession_Filter, string ReportCatagory, string UIView)
        {
            var objResult = (from Tbl in db.usp_SNI_All_Orders(oSession_Filter.Business, oSession_Filter.Division, oSession_Filter.PrimaryProduct,
                                 oSession_Filter.Region, oSession_Filter.Sorg, oSession_Filter.CreatedBy, oSession_Filter.OrderOwner, oSession_Filter.SNIClosureStatus,
                                 oSession_Filter.DBClosureStatus, oSession_Filter.AgingBucket, oSession_Filter.SNIAgingBucket, oSession_Filter.DeltaLoaddateBucket,
                                 oSession_Filter.DollarBucket, oSession_Filter.BacklogStatus,
                                 oSession_Filter.CustomerPONumber, oSession_Filter.SoldToAccountID, oSession_Filter.ShipToAccountID, oSession_Filter.ZUAccountID, oSession_Filter.PL,
                                 oSession_Filter.SoldToCountry, oSession_Filter.ShipToCountry, oSession_Filter.ZUCountry,
                                 oSession_Filter.BillingBlock_Code, oSession_Filter.DeliveryBlock_Code, oSession_Filter.PaymentTerm, oSession_Filter.SalesRep,
                                 oSession_Filter.BTM, oSession_Filter.BTM_Manager,
                                 oSession_Filter.ClosuredaysDeltaFrom, oSession_Filter.ClosuredaysDeltaTo, UIView)
                             select Tbl).ToList();

            return objResult;
        }

        public List<VW_Orders_Info> Get_SNICatagoryOrders(Session_Filters oSession_Filter, string ReportCatagory, string UIView)
        {
            List<VW_Orders_Info> objResult = new List<VW_Orders_Info>();

            switch (ReportCatagory)
            {
                case "SNI_MF":
                    var SNI_MF = (from Tbl in db.usp_SNI_Master_Focus_Orders(oSession_Filter.Business, oSession_Filter.Division, oSession_Filter.PrimaryProduct,
                                 oSession_Filter.Region, oSession_Filter.Sorg, oSession_Filter.CreatedBy, oSession_Filter.OrderOwner, oSession_Filter.SNIClosureStatus,
                                 oSession_Filter.DBClosureStatus, oSession_Filter.AgingBucket, oSession_Filter.SNIAgingBucket, oSession_Filter.DeltaLoaddateBucket,
                                 oSession_Filter.DollarBucket, oSession_Filter.BacklogStatus,
                                 oSession_Filter.CustomerPONumber, oSession_Filter.SoldToAccountID, oSession_Filter.ShipToAccountID, oSession_Filter.ZUAccountID, oSession_Filter.PL,
                                 oSession_Filter.SoldToCountry, oSession_Filter.ShipToCountry, oSession_Filter.ZUCountry,
                                 oSession_Filter.BillingBlock_Code, oSession_Filter.DeliveryBlock_Code, oSession_Filter.PaymentTerm, oSession_Filter.SalesRep,
                                 oSession_Filter.BTM, oSession_Filter.BTM_Manager,
                                 oSession_Filter.ClosuredaysDeltaFrom, oSession_Filter.ClosuredaysDeltaTo, UIView)
                                  select Tbl).ToList();

                    objResult = SNI_MF;
                    break;
                case "SNI_ExpectedReleasePassed":
                    var SNI_ExpectedReleasePassed = from Tbl in db.usp_SNI_ExpectedReleasePassed(oSession_Filter.Business, oSession_Filter.Division, oSession_Filter.PrimaryProduct,
                                 oSession_Filter.Region, oSession_Filter.Sorg, oSession_Filter.CreatedBy, oSession_Filter.OrderOwner, oSession_Filter.SNIClosureStatus,
                                 oSession_Filter.DBClosureStatus, oSession_Filter.AgingBucket, oSession_Filter.SNIAgingBucket, oSession_Filter.DeltaLoaddateBucket,
                                 oSession_Filter.DollarBucket, oSession_Filter.BacklogStatus,
                                 oSession_Filter.CustomerPONumber, oSession_Filter.SoldToAccountID, oSession_Filter.ShipToAccountID, oSession_Filter.ZUAccountID, oSession_Filter.PL,
                                 oSession_Filter.SoldToCountry, oSession_Filter.ShipToCountry, oSession_Filter.ZUCountry,
                                 oSession_Filter.BillingBlock_Code, oSession_Filter.DeliveryBlock_Code, oSession_Filter.PaymentTerm, oSession_Filter.SalesRep,
                                 oSession_Filter.BTM, oSession_Filter.BTM_Manager,
                                 oSession_Filter.ClosuredaysDeltaFrom, oSession_Filter.ClosuredaysDeltaTo, UIView)
                                                    select Tbl;

                    objResult = SNI_ExpectedReleasePassed.ToList();
                    break;
                case "SNI_ExpectedReleaseToday":
                    var SNI_ExpectedReleaseToday = from Tbl in db.usp_SNI_ExpectedReleaseToday(oSession_Filter.Business, oSession_Filter.Division, oSession_Filter.PrimaryProduct,
                                 oSession_Filter.Region, oSession_Filter.Sorg, oSession_Filter.CreatedBy, oSession_Filter.OrderOwner, oSession_Filter.SNIClosureStatus,
                                 oSession_Filter.DBClosureStatus, oSession_Filter.AgingBucket, oSession_Filter.SNIAgingBucket, oSession_Filter.DeltaLoaddateBucket,
                                 oSession_Filter.DollarBucket, oSession_Filter.BacklogStatus,
                                 oSession_Filter.CustomerPONumber, oSession_Filter.SoldToAccountID, oSession_Filter.ShipToAccountID, oSession_Filter.ZUAccountID, oSession_Filter.PL,
                                 oSession_Filter.SoldToCountry, oSession_Filter.ShipToCountry, oSession_Filter.ZUCountry,
                                 oSession_Filter.BillingBlock_Code, oSession_Filter.DeliveryBlock_Code, oSession_Filter.PaymentTerm, oSession_Filter.SalesRep,
                                 oSession_Filter.BTM, oSession_Filter.BTM_Manager,
                                 oSession_Filter.ClosuredaysDeltaFrom, oSession_Filter.ClosuredaysDeltaTo, UIView)
                                                   select Tbl;

                    objResult = SNI_ExpectedReleaseToday.ToList();
                    break;
                case "SNI_No_ExpectedRelease":
                    var SNI_No_ExpectedRelease = from Tbl in db.usp_SNI_NoExpectedRelease(oSession_Filter.Business, oSession_Filter.Division, oSession_Filter.PrimaryProduct,
                                 oSession_Filter.Region, oSession_Filter.Sorg, oSession_Filter.CreatedBy, oSession_Filter.OrderOwner, oSession_Filter.SNIClosureStatus,
                                 oSession_Filter.DBClosureStatus, oSession_Filter.AgingBucket, oSession_Filter.SNIAgingBucket, oSession_Filter.DeltaLoaddateBucket,
                                 oSession_Filter.DollarBucket, oSession_Filter.BacklogStatus,
                                 oSession_Filter.CustomerPONumber, oSession_Filter.SoldToAccountID, oSession_Filter.ShipToAccountID, oSession_Filter.ZUAccountID, oSession_Filter.PL,
                                 oSession_Filter.SoldToCountry, oSession_Filter.ShipToCountry, oSession_Filter.ZUCountry,
                                 oSession_Filter.BillingBlock_Code, oSession_Filter.DeliveryBlock_Code, oSession_Filter.PaymentTerm, oSession_Filter.SalesRep,
                                 oSession_Filter.BTM, oSession_Filter.BTM_Manager,
                                 oSession_Filter.ClosuredaysDeltaFrom, oSession_Filter.ClosuredaysDeltaTo, UIView)
                                                 select Tbl;

                    objResult = SNI_No_ExpectedRelease.ToList();
                    break;
                case "SNI_InvoicingError":
                    var SNI_InvoicingError = from Tbl in db.usp_SNI_Invoicing_Errors(oSession_Filter.Business, oSession_Filter.Division, oSession_Filter.PrimaryProduct,
                                 oSession_Filter.Region, oSession_Filter.Sorg, oSession_Filter.CreatedBy, oSession_Filter.OrderOwner, oSession_Filter.SNIClosureStatus,
                                 oSession_Filter.DBClosureStatus, oSession_Filter.AgingBucket, oSession_Filter.SNIAgingBucket, oSession_Filter.DeltaLoaddateBucket,
                                 oSession_Filter.DollarBucket, oSession_Filter.BacklogStatus,
                                 oSession_Filter.CustomerPONumber, oSession_Filter.SoldToAccountID, oSession_Filter.ShipToAccountID, oSession_Filter.ZUAccountID, oSession_Filter.PL,
                                 oSession_Filter.SoldToCountry, oSession_Filter.ShipToCountry, oSession_Filter.ZUCountry,
                                 oSession_Filter.BillingBlock_Code, oSession_Filter.DeliveryBlock_Code, oSession_Filter.PaymentTerm, oSession_Filter.SalesRep,
                                 oSession_Filter.BTM, oSession_Filter.BTM_Manager,
                                 oSession_Filter.ClosuredaysDeltaFrom, oSession_Filter.ClosuredaysDeltaTo, UIView)
                                             select Tbl;

                    objResult = SNI_InvoicingError.ToList();
                    break;
            }
            return objResult;
        }

        public List<SNI_Excel_View> Get_SNICatagoryOrdersExport(Session_Filters oSession_Filter, string ReportCatagory, string UIView)
        {
            List<SNI_Excel_View> objResult = new List<SNI_Excel_View>();

            switch (ReportCatagory)
            {
                case "SNI_MF":
                    var SNI_MF = (from Tbl in db.usp_SNI_Excel_Export(oSession_Filter.Business, oSession_Filter.Division, oSession_Filter.PrimaryProduct,
                                 oSession_Filter.Region, oSession_Filter.Sorg, oSession_Filter.CreatedBy, oSession_Filter.OrderOwner, oSession_Filter.SNIClosureStatus,
                                 oSession_Filter.DBClosureStatus, oSession_Filter.AgingBucket, oSession_Filter.SNIAgingBucket, oSession_Filter.DeltaLoaddateBucket,
                                 oSession_Filter.DollarBucket, oSession_Filter.BacklogStatus,
                                 oSession_Filter.CustomerPONumber, oSession_Filter.SoldToAccountID, oSession_Filter.ShipToAccountID, oSession_Filter.ZUAccountID, oSession_Filter.PL,
                                 oSession_Filter.SoldToCountry, oSession_Filter.ShipToCountry, oSession_Filter.ZUCountry,
                                 oSession_Filter.BillingBlock_Code, oSession_Filter.DeliveryBlock_Code, oSession_Filter.PaymentTerm, oSession_Filter.SalesRep,
                                 oSession_Filter.BTM, oSession_Filter.BTM_Manager,
                                 oSession_Filter.ClosuredaysDeltaFrom, oSession_Filter.ClosuredaysDeltaTo, UIView)
                                  select Tbl).ToList();

                    objResult = SNI_MF;
                    break;
                case "SNI_ExpectedReleasePassed":
                    var SNI_ExpectedReleasePassed = from Tbl in db.usp_SNI_Excel_Export(oSession_Filter.Business, oSession_Filter.Division, oSession_Filter.PrimaryProduct,
                                 oSession_Filter.Region, oSession_Filter.Sorg, oSession_Filter.CreatedBy, oSession_Filter.OrderOwner, oSession_Filter.SNIClosureStatus,
                                 oSession_Filter.DBClosureStatus, oSession_Filter.AgingBucket, oSession_Filter.SNIAgingBucket, oSession_Filter.DeltaLoaddateBucket, 
                                 oSession_Filter.DollarBucket, oSession_Filter.BacklogStatus,
                                 oSession_Filter.CustomerPONumber, oSession_Filter.SoldToAccountID, oSession_Filter.ShipToAccountID, oSession_Filter.ZUAccountID, oSession_Filter.PL,
                                 oSession_Filter.SoldToCountry, oSession_Filter.ShipToCountry, oSession_Filter.ZUCountry,
                                 oSession_Filter.BillingBlock_Code, oSession_Filter.DeliveryBlock_Code, oSession_Filter.PaymentTerm, oSession_Filter.SalesRep, 
                                 oSession_Filter.BTM, oSession_Filter.BTM_Manager,
                                 oSession_Filter.ClosuredaysDeltaFrom, oSession_Filter.ClosuredaysDeltaTo, UIView)
                                                    select Tbl;

                    objResult = SNI_ExpectedReleasePassed.ToList();
                    break;
                case "SNI_ExpectedReleaseToday":
                    var SNI_ExpectedReleaseToday = from Tbl in db.usp_SNI_Excel_Export(oSession_Filter.Business, oSession_Filter.Division, oSession_Filter.PrimaryProduct,
                                 oSession_Filter.Region, oSession_Filter.Sorg, oSession_Filter.CreatedBy, oSession_Filter.OrderOwner, oSession_Filter.SNIClosureStatus,
                                 oSession_Filter.DBClosureStatus, oSession_Filter.AgingBucket, oSession_Filter.SNIAgingBucket, oSession_Filter.DeltaLoaddateBucket,
                                 oSession_Filter.DollarBucket, oSession_Filter.BacklogStatus,
                                 oSession_Filter.CustomerPONumber, oSession_Filter.SoldToAccountID, oSession_Filter.ShipToAccountID, oSession_Filter.ZUAccountID, oSession_Filter.PL,
                                 oSession_Filter.SoldToCountry, oSession_Filter.ShipToCountry, oSession_Filter.ZUCountry,
                                 oSession_Filter.BillingBlock_Code, oSession_Filter.DeliveryBlock_Code, oSession_Filter.PaymentTerm, oSession_Filter.SalesRep,
                                 oSession_Filter.BTM, oSession_Filter.BTM_Manager,
                                 oSession_Filter.ClosuredaysDeltaFrom, oSession_Filter.ClosuredaysDeltaTo, UIView)
                                                   select Tbl;

                    objResult = SNI_ExpectedReleaseToday.ToList();
                    break;
                case "SNI_No_ExpectedRelease":
                    var SNI_No_ExpectedRelease = from Tbl in db.usp_SNI_Excel_Export(oSession_Filter.Business, oSession_Filter.Division, oSession_Filter.PrimaryProduct,
                                 oSession_Filter.Region, oSession_Filter.Sorg, oSession_Filter.CreatedBy, oSession_Filter.OrderOwner, oSession_Filter.SNIClosureStatus,
                                 oSession_Filter.DBClosureStatus, oSession_Filter.AgingBucket, oSession_Filter.SNIAgingBucket, oSession_Filter.DeltaLoaddateBucket,
                                 oSession_Filter.DollarBucket, oSession_Filter.BacklogStatus,
                                 oSession_Filter.CustomerPONumber, oSession_Filter.SoldToAccountID, oSession_Filter.ShipToAccountID, oSession_Filter.ZUAccountID, oSession_Filter.PL,
                                 oSession_Filter.SoldToCountry, oSession_Filter.ShipToCountry, oSession_Filter.ZUCountry,
                                 oSession_Filter.BillingBlock_Code, oSession_Filter.DeliveryBlock_Code, oSession_Filter.PaymentTerm, oSession_Filter.SalesRep,
                                 oSession_Filter.BTM, oSession_Filter.BTM_Manager,
                                 oSession_Filter.ClosuredaysDeltaFrom, oSession_Filter.ClosuredaysDeltaTo, UIView)
                                                 select Tbl;

                    objResult = SNI_No_ExpectedRelease.ToList();
                    break;
                case "SNI_All":
                    var SNI_All = from Tbl in db.usp_SNI_Excel_Export(oSession_Filter.Business, oSession_Filter.Division, oSession_Filter.PrimaryProduct,
                                 oSession_Filter.Region, oSession_Filter.Sorg, oSession_Filter.CreatedBy, oSession_Filter.OrderOwner, oSession_Filter.SNIClosureStatus,
                                 oSession_Filter.DBClosureStatus, oSession_Filter.AgingBucket, oSession_Filter.SNIAgingBucket, oSession_Filter.DeltaLoaddateBucket,
                                 oSession_Filter.DollarBucket, oSession_Filter.BacklogStatus,
                                 oSession_Filter.CustomerPONumber, oSession_Filter.SoldToAccountID, oSession_Filter.ShipToAccountID, oSession_Filter.ZUAccountID, oSession_Filter.PL,
                                 oSession_Filter.SoldToCountry, oSession_Filter.ShipToCountry, oSession_Filter.ZUCountry,
                                 oSession_Filter.BillingBlock_Code, oSession_Filter.DeliveryBlock_Code, oSession_Filter.PaymentTerm, oSession_Filter.SalesRep,
                                 oSession_Filter.BTM, oSession_Filter.BTM_Manager,
                                 oSession_Filter.ClosuredaysDeltaFrom, oSession_Filter.ClosuredaysDeltaTo, UIView)
                                  select Tbl;

                    objResult = SNI_All.ToList();
                    break;
                case "SNI_InvoicingError":
                    var SNI_InvoicingError = from Tbl in db.usp_SNI_Excel_Export(oSession_Filter.Business, oSession_Filter.Division, oSession_Filter.PrimaryProduct,
                                 oSession_Filter.Region, oSession_Filter.Sorg, oSession_Filter.CreatedBy, oSession_Filter.OrderOwner, oSession_Filter.SNIClosureStatus,
                                 oSession_Filter.DBClosureStatus, oSession_Filter.AgingBucket, oSession_Filter.SNIAgingBucket, oSession_Filter.DeltaLoaddateBucket,
                                 oSession_Filter.DollarBucket, oSession_Filter.BacklogStatus,
                                 oSession_Filter.CustomerPONumber, oSession_Filter.SoldToAccountID, oSession_Filter.ShipToAccountID, oSession_Filter.ZUAccountID, oSession_Filter.PL,
                                 oSession_Filter.SoldToCountry, oSession_Filter.ShipToCountry, oSession_Filter.ZUCountry,
                                 oSession_Filter.BillingBlock_Code, oSession_Filter.DeliveryBlock_Code, oSession_Filter.PaymentTerm, oSession_Filter.SalesRep,
                                 oSession_Filter.BTM, oSession_Filter.BTM_Manager,
                                 oSession_Filter.ClosuredaysDeltaFrom, oSession_Filter.ClosuredaysDeltaTo, UIView)
                                             select Tbl;

                    objResult = SNI_InvoicingError.ToList();
                    break;
            }
            return objResult;
        }

        public List<Tbl_Order_comments> GetMasterFocusChildOrders(string Order_Number)
        {
            List<Tbl_Order_comments> objResult = new List<Tbl_Order_comments>();

            try
            {
                var Result = from Tbl in db.usp_Get_DB_Mfchilds(Order_Number)
                             select Tbl;

                objResult = Result.ToList();

            }
            catch (Exception ex)
            { }

            return objResult;
        }

        public List<Tbl_Archives> GetArchivesList()
        {
            var lstArchives = db.Tbl_Archives.ToList();
            return lstArchives;
        }

        #endregion

        #region Delivery Blocked Related


        public Tbl_Backlog_Summary GetMasterFocusSummary(string FullName, string UIArea)
        {
            var lstTodaySummary = db.usp_Get_Blocked_Orders_Summary(FullName, UIArea).FirstOrDefault();

            return lstTodaySummary;
        }

        public List<Tbl_Order_comments> GetDB_MasterFocusChildOrders(string Order_Number)
        {
            List<Tbl_Order_comments> objResult;

            var Result = from Tbl in db.usp_Get_DB_Mfchilds(Order_Number)
                         select Tbl;

            try
            {
                objResult = new List<Tbl_Order_comments>();
                foreach (Tbl_Order_comments objComment in Result.ToList())
                {
                    objResult.Add(objComment);
                }

            }
            catch (Exception ex)
            {
                objResult = new List<Tbl_Order_comments>();
            }

            return objResult;
        }

        public List<VW_Orders_Info> Ge_DB_ArchivesList(int ReportID)
        {
            throw new NotImplementedException();
        }

        public List<SNI_Excel_View> Get_DB_CatagoryOrdersExport(Session_Filters oSession_Filter, string ReportCatagory, string UIView)
        {
            List<SNI_Excel_View> objResult = new List<SNI_Excel_View>();

            switch (ReportCatagory)
            {
                case "All_DB":
                    var SNI_MF = (from Tbl in db.usp_DB_Excel_Export(oSession_Filter.Region, oSession_Filter.Division, oSession_Filter.SalesOrder,
                                 oSession_Filter.CustomerPONumber, oSession_Filter.Sorg, oSession_Filter.Business, oSession_Filter.PL, oSession_Filter.PrimaryProduct,
                                 oSession_Filter.OwnerName, oSession_Filter.OrderOwner, oSession_Filter.BacklogStatus, oSession_Filter.DollarBucket,
                                 oSession_Filter.DollarBucket, oSession_Filter.Aging, oSession_Filter.AgingBucket, oSession_Filter.SNIAgingBucket, oSession_Filter.SNIClosureStatus,
                                 oSession_Filter.DBClosureStatus, oSession_Filter.BacklogAmt, oSession_Filter.SoldToAccountID, oSession_Filter.ShipToAccountID, 
                                 oSession_Filter.SoldToAccount, oSession_Filter.ShipToAccount, oSession_Filter.ZUAccountID, oSession_Filter.SoldToCountry, oSession_Filter.ShipToCountry, 
                                 oSession_Filter.ZUAccount, oSession_Filter.ZUCountry, oSession_Filter.SalesRep, oSession_Filter.BTM, oSession_Filter.BTM_Manager, 
                                 oSession_Filter.BillingBlock_Code, oSession_Filter.DeliveryBlock_Code, oSession_Filter.BillingBlock_HeaderText, 
                                 oSession_Filter.DeliveryBlock_HeaderText, oSession_Filter.DeliveryBlock_HeaderText, oSession_Filter.BillingBlock_ItemText, 
                                 oSession_Filter.DeliveryBlock_ItemText, oSession_Filter.DeltaLoaddate, oSession_Filter.DeltaLoaddateBucket, oSession_Filter.ClosuredaysDeltaFrom, 
                                 oSession_Filter.ClosuredaysDeltaTo, oSession_Filter.NLHD, oSession_Filter.LoaDDate, oSession_Filter.TrioLoaDDate, oSession_Filter.CRDD, 
                                 oSession_Filter.ExpReleaseDate, oSession_Filter.ReasonCode,
                                 UIView, "All_DB")
                                  select Tbl).ToList();

                    objResult = SNI_MF;
                    break;
                case "DB_OverDue":
                    var SNI_ExpectedReleasePassed = from Tbl in db.usp_DB_Excel_Export(oSession_Filter.Region, oSession_Filter.Division, oSession_Filter.SalesOrder,
                                 oSession_Filter.CustomerPONumber, oSession_Filter.Sorg, oSession_Filter.Business, oSession_Filter.PL, oSession_Filter.PrimaryProduct,
                                 oSession_Filter.OwnerName, oSession_Filter.OrderOwner, oSession_Filter.BacklogStatus, oSession_Filter.DollarBucket,
                                 oSession_Filter.DollarBucket, oSession_Filter.Aging, oSession_Filter.AgingBucket, oSession_Filter.SNIAgingBucket, oSession_Filter.SNIClosureStatus,
                                 oSession_Filter.DBClosureStatus, oSession_Filter.BacklogAmt, oSession_Filter.SoldToAccountID, oSession_Filter.ShipToAccountID,
                                 oSession_Filter.SoldToAccount, oSession_Filter.ShipToAccount, oSession_Filter.ZUAccountID, oSession_Filter.SoldToCountry, oSession_Filter.ShipToCountry,
                                 oSession_Filter.ZUAccount, oSession_Filter.ZUCountry, oSession_Filter.SalesRep, oSession_Filter.BTM, oSession_Filter.BTM_Manager,
                                 oSession_Filter.BillingBlock_Code, oSession_Filter.DeliveryBlock_Code, oSession_Filter.BillingBlock_HeaderText,
                                 oSession_Filter.DeliveryBlock_HeaderText, oSession_Filter.DeliveryBlock_HeaderText, oSession_Filter.BillingBlock_ItemText,
                                 oSession_Filter.DeliveryBlock_ItemText, oSession_Filter.DeltaLoaddate, oSession_Filter.DeltaLoaddateBucket, oSession_Filter.ClosuredaysDeltaFrom,
                                 oSession_Filter.ClosuredaysDeltaTo, oSession_Filter.NLHD, oSession_Filter.LoaDDate, oSession_Filter.TrioLoaDDate, oSession_Filter.CRDD,
                                 oSession_Filter.ExpReleaseDate, oSession_Filter.ReasonCode,
                                 UIView, "DB_OverDue")
                                                    select Tbl;

                    objResult = SNI_ExpectedReleasePassed.ToList();
                    break;
                case "DB_OverDue_14_Days":
                    var SNI_ExpectedReleaseToday = from Tbl in db.usp_DB_Excel_Export(oSession_Filter.Region, oSession_Filter.Division, oSession_Filter.SalesOrder,
                                 oSession_Filter.CustomerPONumber, oSession_Filter.Sorg, oSession_Filter.Business, oSession_Filter.PL, oSession_Filter.PrimaryProduct,
                                 oSession_Filter.OwnerName, oSession_Filter.OrderOwner, oSession_Filter.BacklogStatus, oSession_Filter.DollarBucket,
                                 oSession_Filter.DollarBucket, oSession_Filter.Aging, oSession_Filter.AgingBucket, oSession_Filter.SNIAgingBucket, oSession_Filter.SNIClosureStatus,
                                 oSession_Filter.DBClosureStatus, oSession_Filter.BacklogAmt, oSession_Filter.SoldToAccountID, oSession_Filter.ShipToAccountID,
                                 oSession_Filter.SoldToAccount, oSession_Filter.ShipToAccount, oSession_Filter.ZUAccountID, oSession_Filter.SoldToCountry, oSession_Filter.ShipToCountry,
                                 oSession_Filter.ZUAccount, oSession_Filter.ZUCountry, oSession_Filter.SalesRep, oSession_Filter.BTM, oSession_Filter.BTM_Manager,
                                 oSession_Filter.BillingBlock_Code, oSession_Filter.DeliveryBlock_Code, oSession_Filter.BillingBlock_HeaderText,
                                 oSession_Filter.DeliveryBlock_HeaderText, oSession_Filter.DeliveryBlock_HeaderText, oSession_Filter.BillingBlock_ItemText,
                                 oSession_Filter.DeliveryBlock_ItemText, oSession_Filter.DeltaLoaddate, oSession_Filter.DeltaLoaddateBucket, oSession_Filter.ClosuredaysDeltaFrom,
                                 oSession_Filter.ClosuredaysDeltaTo, oSession_Filter.NLHD, oSession_Filter.LoaDDate, oSession_Filter.TrioLoaDDate, oSession_Filter.CRDD,
                                 oSession_Filter.ExpReleaseDate, oSession_Filter.ReasonCode,
                                 UIView, "DB_OverDue_14_Days")
                                                   select Tbl;

                    objResult = SNI_ExpectedReleaseToday.ToList();
                    break;
                case "DB_Aged_90_Days":
                    var SNI_No_ExpectedRelease = from Tbl in db.usp_DB_Excel_Export(oSession_Filter.Region, oSession_Filter.Division, oSession_Filter.SalesOrder,
                                 oSession_Filter.CustomerPONumber, oSession_Filter.Sorg, oSession_Filter.Business, oSession_Filter.PL, oSession_Filter.PrimaryProduct,
                                 oSession_Filter.OwnerName, oSession_Filter.OrderOwner, oSession_Filter.BacklogStatus, oSession_Filter.DollarBucket,
                                 oSession_Filter.DollarBucket, oSession_Filter.Aging, oSession_Filter.AgingBucket, oSession_Filter.SNIAgingBucket, oSession_Filter.SNIClosureStatus,
                                 oSession_Filter.DBClosureStatus, oSession_Filter.BacklogAmt, oSession_Filter.SoldToAccountID, oSession_Filter.ShipToAccountID,
                                 oSession_Filter.SoldToAccount, oSession_Filter.ShipToAccount, oSession_Filter.ZUAccountID, oSession_Filter.SoldToCountry, oSession_Filter.ShipToCountry,
                                 oSession_Filter.ZUAccount, oSession_Filter.ZUCountry, oSession_Filter.SalesRep, oSession_Filter.BTM, oSession_Filter.BTM_Manager,
                                 oSession_Filter.BillingBlock_Code, oSession_Filter.DeliveryBlock_Code, oSession_Filter.BillingBlock_HeaderText,
                                 oSession_Filter.DeliveryBlock_HeaderText, oSession_Filter.DeliveryBlock_HeaderText, oSession_Filter.BillingBlock_ItemText,
                                 oSession_Filter.DeliveryBlock_ItemText, oSession_Filter.DeltaLoaddate, oSession_Filter.DeltaLoaddateBucket, oSession_Filter.ClosuredaysDeltaFrom,
                                 oSession_Filter.ClosuredaysDeltaTo, oSession_Filter.NLHD, oSession_Filter.LoaDDate, oSession_Filter.TrioLoaDDate, oSession_Filter.CRDD,
                                 oSession_Filter.ExpReleaseDate, oSession_Filter.ReasonCode,
                                 UIView, "DB_Aged_90_Days")
                                                 select Tbl;

                    objResult = SNI_No_ExpectedRelease.ToList();
                    break;
                case "DB_PassedDueDate":
                    var SNI_InvoicingError = from Tbl in db.usp_DB_Excel_Export(oSession_Filter.Region, oSession_Filter.Division, oSession_Filter.SalesOrder,
                                 oSession_Filter.CustomerPONumber, oSession_Filter.Sorg, oSession_Filter.Business, oSession_Filter.PL, oSession_Filter.PrimaryProduct,
                                 oSession_Filter.OwnerName, oSession_Filter.OrderOwner, oSession_Filter.BacklogStatus, oSession_Filter.DollarBucket,
                                 oSession_Filter.DollarBucket, oSession_Filter.Aging, oSession_Filter.AgingBucket, oSession_Filter.SNIAgingBucket, oSession_Filter.SNIClosureStatus,
                                 oSession_Filter.DBClosureStatus, oSession_Filter.BacklogAmt, oSession_Filter.SoldToAccountID, oSession_Filter.ShipToAccountID,
                                 oSession_Filter.SoldToAccount, oSession_Filter.ShipToAccount, oSession_Filter.ZUAccountID, oSession_Filter.SoldToCountry, oSession_Filter.ShipToCountry,
                                 oSession_Filter.ZUAccount, oSession_Filter.ZUCountry, oSession_Filter.SalesRep, oSession_Filter.BTM, oSession_Filter.BTM_Manager,
                                 oSession_Filter.BillingBlock_Code, oSession_Filter.DeliveryBlock_Code, oSession_Filter.BillingBlock_HeaderText,
                                 oSession_Filter.DeliveryBlock_HeaderText, oSession_Filter.DeliveryBlock_HeaderText, oSession_Filter.BillingBlock_ItemText,
                                 oSession_Filter.DeliveryBlock_ItemText, oSession_Filter.DeltaLoaddate, oSession_Filter.DeltaLoaddateBucket, oSession_Filter.ClosuredaysDeltaFrom,
                                 oSession_Filter.ClosuredaysDeltaTo, oSession_Filter.NLHD, oSession_Filter.LoaDDate, oSession_Filter.TrioLoaDDate, oSession_Filter.CRDD,
                                 oSession_Filter.ExpReleaseDate, oSession_Filter.ReasonCode,
                                 UIView, "DB_PassedDueDate")
                                             select Tbl;

                    objResult = SNI_InvoicingError.ToList();
                    break;
            }
            return objResult;
        }

        public List<VW_Orders_Info> Get_DB_RawOrders(Session_Filters oSession_Filter, string ReportCatagory, string UIView)
        {
            var result = (from tbl in db.usp_Get_DB_Raw(oSession_Filter.Business, oSession_Filter.Division, oSession_Filter.PrimaryProduct,
                                 oSession_Filter.Region, oSession_Filter.Sorg, oSession_Filter.CreatedBy, oSession_Filter.OrderOwner, oSession_Filter.SNIClosureStatus,
                                 oSession_Filter.DBClosureStatus, oSession_Filter.AgingBucket, oSession_Filter.SNIAgingBucket, oSession_Filter.DeltaLoaddateBucket,
                                 oSession_Filter.DollarBucket, oSession_Filter.BacklogStatus,
                                 oSession_Filter.CustomerPONumber, oSession_Filter.SoldToAccountID, oSession_Filter.ShipToAccountID, oSession_Filter.ZUAccountID, oSession_Filter.PL,
                                 oSession_Filter.SoldToCountry, oSession_Filter.ShipToCountry, oSession_Filter.ZUCountry,
                                 oSession_Filter.BillingBlock_Code, oSession_Filter.DeliveryBlock_Code, oSession_Filter.PaymentTerm, oSession_Filter.SalesRep,
                                 oSession_Filter.BTM, oSession_Filter.BTM_Manager,
                                 oSession_Filter.ClosuredaysDeltaFrom, oSession_Filter.ClosuredaysDeltaTo, UIView)
                          select tbl).ToList();

            return result;
        }

        public List<Tbl_STR_Summary> GetSTR_Summary(DateTime snapshotDateFrom, DateTime snapshotDateTo)
        {
            List<Tbl_STR_Summary> results = new List<Tbl_STR_Summary>();
            results = db.usp_STR_Summary(snapshotDateFrom, snapshotDateTo).ToList();

            return results;
        }

        public List<VW_DB_Speed_To_Revenue> Get_DB_SpeedToRevenue(string SAPName, string Team, string BU, string Div, string Region, string Order_Number, string DateCondition, string SnapshotDateFrom, string SnapshotDateTo, string DBCutOffDate, string ERDFlag, string ERDFrom, string ERDTo, string SAPStatus)
        {
            var result = (from tbl in db.usp_Get_DB_Speed_to_Revenue(Region, "", "", "", BU, Div, "", "", "", "", "", "", "", "", "", "", "", "", SAPStatus, SAPName, Team) select tbl).ToList();

            return result;
        }

        public List<VW_Orders_Info> Get_DB_Overdue(Session_Filters oSession_Filter, string ReportCatagory, string UIView)
        {
            var result = (from tbl in db.usp_Get_DB_Overdue(oSession_Filter.Business, oSession_Filter.Division, oSession_Filter.PrimaryProduct,
                                 oSession_Filter.Region, oSession_Filter.Sorg, oSession_Filter.CreatedBy, oSession_Filter.OrderOwner, oSession_Filter.SNIClosureStatus,
                                 oSession_Filter.DBClosureStatus, oSession_Filter.AgingBucket, oSession_Filter.SNIAgingBucket, oSession_Filter.DeltaLoaddateBucket,
                                 oSession_Filter.DollarBucket, oSession_Filter.BacklogStatus,
                                 oSession_Filter.CustomerPONumber, oSession_Filter.SoldToAccountID, oSession_Filter.ShipToAccountID, oSession_Filter.ZUAccountID, oSession_Filter.PL,
                                 oSession_Filter.SoldToCountry, oSession_Filter.ShipToCountry, oSession_Filter.ZUCountry,
                                 oSession_Filter.BillingBlock_Code, oSession_Filter.DeliveryBlock_Code, oSession_Filter.PaymentTerm, oSession_Filter.SalesRep,
                                 oSession_Filter.BTM, oSession_Filter.BTM_Manager,
                                 oSession_Filter.ClosuredaysDeltaFrom, oSession_Filter.ClosuredaysDeltaTo, UIView)
                          select tbl).ToList();

            return result;
        }

        public List<VW_Orders_Info> Get_DB_Greater_90_days(Session_Filters oSession_Filter, string ReportCatagory, string UIView)
        {
            var result = (from tbl in db.usp_Get_DB_Greater_90days(oSession_Filter.Business, oSession_Filter.Division, oSession_Filter.PrimaryProduct,
                                 oSession_Filter.Region, oSession_Filter.Sorg, oSession_Filter.CreatedBy, oSession_Filter.OrderOwner, oSession_Filter.SNIClosureStatus,
                                 oSession_Filter.DBClosureStatus, oSession_Filter.AgingBucket, oSession_Filter.SNIAgingBucket, oSession_Filter.DeltaLoaddateBucket, 
                                 oSession_Filter.DollarBucket, oSession_Filter.BacklogStatus,
                                 oSession_Filter.CustomerPONumber, oSession_Filter.SoldToAccountID, oSession_Filter.ShipToAccountID, oSession_Filter.ZUAccountID, oSession_Filter.PL,
                                 oSession_Filter.SoldToCountry, oSession_Filter.ShipToCountry, oSession_Filter.ZUCountry,
                                 oSession_Filter.BillingBlock_Code, oSession_Filter.DeliveryBlock_Code, oSession_Filter.PaymentTerm, oSession_Filter.SalesRep, 
                                 oSession_Filter.BTM, oSession_Filter.BTM_Manager,
                                 oSession_Filter.ClosuredaysDeltaFrom, oSession_Filter.ClosuredaysDeltaTo, UIView)
                          select tbl).ToList();

            return result;
        }

        public List<VW_Orders_Info> Get_DB_Expected_Release_Passed(Session_Filters oSession_Filter, string ReportCatagory, string UIView)
        {
            var result = (from tbl in db.usp_Get_DB_Expected_Release_Passed(oSession_Filter.Business, oSession_Filter.Division, oSession_Filter.PrimaryProduct,
                                 oSession_Filter.Region, oSession_Filter.Sorg, oSession_Filter.CreatedBy, oSession_Filter.OrderOwner, oSession_Filter.SNIClosureStatus,
                                 oSession_Filter.DBClosureStatus, oSession_Filter.AgingBucket, oSession_Filter.SNIAgingBucket, oSession_Filter.DeltaLoaddateBucket,
                                 oSession_Filter.DollarBucket, oSession_Filter.BacklogStatus,
                                 oSession_Filter.CustomerPONumber, oSession_Filter.SoldToAccountID, oSession_Filter.ShipToAccountID, oSession_Filter.ZUAccountID, oSession_Filter.PL,
                                 oSession_Filter.SoldToCountry, oSession_Filter.ShipToCountry, oSession_Filter.ZUCountry,
                                 oSession_Filter.BillingBlock_Code, oSession_Filter.DeliveryBlock_Code, oSession_Filter.PaymentTerm, oSession_Filter.SalesRep,
                                 oSession_Filter.BTM, oSession_Filter.BTM_Manager,
                                 oSession_Filter.ClosuredaysDeltaFrom, oSession_Filter.ClosuredaysDeltaTo, UIView)
                          select tbl).ToList();

            return result;
        }

        public List<VW_Orders_Info> Get_DB_Overdue_14_Days(Session_Filters oSession_Filter, string ReportCatagory, string UIView)
        {
            var result = (from tbl in db.usp_Get_DB_OverDue_in_14Days(oSession_Filter.Business, oSession_Filter.Division, oSession_Filter.PrimaryProduct,
                                 oSession_Filter.Region, oSession_Filter.Sorg, oSession_Filter.CreatedBy, oSession_Filter.OrderOwner, oSession_Filter.SNIClosureStatus,
                                 oSession_Filter.DBClosureStatus, oSession_Filter.AgingBucket, oSession_Filter.SNIAgingBucket, oSession_Filter.DeltaLoaddateBucket,
                                 oSession_Filter.DollarBucket, oSession_Filter.BacklogStatus,
                                 oSession_Filter.CustomerPONumber, oSession_Filter.SoldToAccountID, oSession_Filter.ShipToAccountID, oSession_Filter.ZUAccountID, oSession_Filter.PL,
                                 oSession_Filter.SoldToCountry, oSession_Filter.ShipToCountry, oSession_Filter.ZUCountry,
                                 oSession_Filter.BillingBlock_Code, oSession_Filter.DeliveryBlock_Code, oSession_Filter.PaymentTerm, oSession_Filter.SalesRep,
                                 oSession_Filter.BTM, oSession_Filter.BTM_Manager,
                                 oSession_Filter.ClosuredaysDeltaFrom, oSession_Filter.ClosuredaysDeltaTo, UIView)
                          select tbl).ToList();

            return result;
        }

        #endregion

        #region Clean Orders

        public List<SNI_Excel_View> Get_UB_CatagoryOrdersExport(Session_Filters oSession_Filter, string ReportCatagory, string UIView)
        {
            List<SNI_Excel_View> objResult = new List<SNI_Excel_View>();

            switch (ReportCatagory)
            {
                case "All_UB":
                    var SNI_MF = (from Tbl in db.usp_UB_Excel_Export(oSession_Filter.Region, oSession_Filter.Division, oSession_Filter.SalesOrder,
                                 oSession_Filter.CustomerPONumber, oSession_Filter.Sorg, oSession_Filter.Business, oSession_Filter.PL, oSession_Filter.PrimaryProduct,
                                 oSession_Filter.OwnerName, oSession_Filter.OrderOwner, oSession_Filter.BacklogStatus, oSession_Filter.DollarBucket,
                                 oSession_Filter.DollarBucket, oSession_Filter.Aging, oSession_Filter.AgingBucket, oSession_Filter.SNIAgingBucket, oSession_Filter.SNIClosureStatus,
                                 oSession_Filter.DBClosureStatus, oSession_Filter.BacklogAmt, oSession_Filter.SoldToAccountID, oSession_Filter.ShipToAccountID,
                                 oSession_Filter.SoldToAccount, oSession_Filter.ShipToAccount, oSession_Filter.ZUAccountID, oSession_Filter.SoldToCountry, oSession_Filter.ShipToCountry,
                                 oSession_Filter.ZUAccount, oSession_Filter.ZUCountry, oSession_Filter.SalesRep, oSession_Filter.BTM, oSession_Filter.BTM_Manager,
                                 oSession_Filter.BillingBlock_Code, oSession_Filter.DeliveryBlock_Code, oSession_Filter.BillingBlock_HeaderText,
                                 oSession_Filter.DeliveryBlock_HeaderText, oSession_Filter.DeliveryBlock_HeaderText, oSession_Filter.BillingBlock_ItemText,
                                 oSession_Filter.DeliveryBlock_ItemText, oSession_Filter.DeltaLoaddate, oSession_Filter.DeltaLoaddateBucket, oSession_Filter.ClosuredaysDeltaFrom,
                                 oSession_Filter.ClosuredaysDeltaTo, oSession_Filter.NLHD, oSession_Filter.LoaDDate, oSession_Filter.TrioLoaDDate, oSession_Filter.CRDD,
                                 oSession_Filter.ExpReleaseDate, oSession_Filter.ReasonCode,
                                 UIView, "All_Unblocked")
                                  select Tbl).ToList();

                    objResult = SNI_MF;
                    break;
                case "UB_OverDue":
                    var SNI_ExpectedReleasePassed = from Tbl in db.usp_UB_Excel_Export(oSession_Filter.Region, oSession_Filter.Division, oSession_Filter.SalesOrder,
                                 oSession_Filter.CustomerPONumber, oSession_Filter.Sorg, oSession_Filter.Business, oSession_Filter.PL, oSession_Filter.PrimaryProduct,
                                 oSession_Filter.OwnerName, oSession_Filter.OrderOwner, oSession_Filter.BacklogStatus, oSession_Filter.DollarBucket,
                                 oSession_Filter.DollarBucket, oSession_Filter.Aging, oSession_Filter.AgingBucket, oSession_Filter.SNIAgingBucket, oSession_Filter.SNIClosureStatus,
                                 oSession_Filter.DBClosureStatus, oSession_Filter.BacklogAmt, oSession_Filter.SoldToAccountID, oSession_Filter.ShipToAccountID,
                                 oSession_Filter.SoldToAccount, oSession_Filter.ShipToAccount, oSession_Filter.ZUAccountID, oSession_Filter.SoldToCountry, oSession_Filter.ShipToCountry,
                                 oSession_Filter.ZUAccount, oSession_Filter.ZUCountry, oSession_Filter.SalesRep, oSession_Filter.BTM, oSession_Filter.BTM_Manager,
                                 oSession_Filter.BillingBlock_Code, oSession_Filter.DeliveryBlock_Code, oSession_Filter.BillingBlock_HeaderText,
                                 oSession_Filter.DeliveryBlock_HeaderText, oSession_Filter.DeliveryBlock_HeaderText, oSession_Filter.BillingBlock_ItemText,
                                 oSession_Filter.DeliveryBlock_ItemText, oSession_Filter.DeltaLoaddate, oSession_Filter.DeltaLoaddateBucket, oSession_Filter.ClosuredaysDeltaFrom,
                                 oSession_Filter.ClosuredaysDeltaTo, oSession_Filter.NLHD, oSession_Filter.LoaDDate, oSession_Filter.TrioLoaDDate, oSession_Filter.CRDD,
                                 oSession_Filter.ExpReleaseDate, oSession_Filter.ReasonCode,
                                 UIView, "All_Unblocked_Overdue")
                                                    select Tbl;

                    objResult = SNI_ExpectedReleasePassed.ToList();
                    break;
            }
            return objResult;
        }


        public List<VW_Orders_Info> Get_Unblocked_All(Session_Filters oSession_Filter, string ReportCatagory, string UIView)
        {
            var result = (from tbl in db.usp_Get_Unblocked_Orders(oSession_Filter.Business, oSession_Filter.Division, oSession_Filter.PrimaryProduct,
                                 oSession_Filter.Region, oSession_Filter.Sorg, oSession_Filter.CreatedBy, oSession_Filter.OrderOwner, oSession_Filter.SNIClosureStatus,
                                 oSession_Filter.DBClosureStatus, oSession_Filter.Aging, oSession_Filter.SNIAging, oSession_Filter.AgingBucket, oSession_Filter.SNIAgingBucket, oSession_Filter.DeltaLoaddate, 
                                 oSession_Filter.DeltaLoaddateBucket, oSession_Filter.DollarBucket, oSession_Filter.BacklogStatus,
                                 oSession_Filter.CustomerPONumber, oSession_Filter.SoldToAccountID, oSession_Filter.ShipToAccountID, oSession_Filter.SoldToAccount, oSession_Filter.ShipToAccount,
                                 oSession_Filter.ZUAccountID, oSession_Filter.ZUAccount ,oSession_Filter.PL,
                                 oSession_Filter.SoldToCountry, oSession_Filter.ShipToCountry, oSession_Filter.ZUCountry,
                                 oSession_Filter.BillingBlock_Code, oSession_Filter.DeliveryBlock_Code, oSession_Filter.BillingBlock_HeaderText , oSession_Filter.DeliveryBlock_HeaderText,
                                 oSession_Filter.BillingBlock_ItemText, oSession_Filter.DeliveryBlock_ItemText,
                                 oSession_Filter.PaymentTerm, oSession_Filter.SalesRep,
                                 oSession_Filter.BTM, oSession_Filter.BTM_Manager,
                                 oSession_Filter.ClosuredaysDeltaFrom, oSession_Filter.ClosuredaysDeltaTo, UIView)
                          select tbl).ToList();

            return result;
        }

        public List<VW_DB_Overdue> GetUnblocked_Overdue(Session_Filters oSession_Filter, string ReportCatagory, string UIView)
        {
            var result = new List<VW_DB_Overdue>();

            result = (from tbl in db.usp_Get_NoDB_OverDue(oSession_Filter.Business, oSession_Filter.Division, oSession_Filter.PrimaryProduct,
                                 oSession_Filter.Region, oSession_Filter.Sorg, oSession_Filter.CreatedBy, oSession_Filter.OrderOwner, oSession_Filter.SNIClosureStatus,
                                 oSession_Filter.DBClosureStatus, oSession_Filter.AgingBucket, oSession_Filter.SNIAgingBucket, oSession_Filter.DeltaLoaddateBucket,
                                 oSession_Filter.DollarBucket, oSession_Filter.BacklogStatus,
                                 oSession_Filter.CustomerPONumber, oSession_Filter.SoldToAccountID, oSession_Filter.ShipToAccountID, oSession_Filter.ZUAccountID, oSession_Filter.PL,
                                 oSession_Filter.SoldToCountry, oSession_Filter.ShipToCountry, oSession_Filter.ZUCountry,
                                 oSession_Filter.BillingBlock_Code, oSession_Filter.DeliveryBlock_Code, oSession_Filter.PaymentTerm, oSession_Filter.SalesRep,
                                 oSession_Filter.BTM, oSession_Filter.BTM_Manager,
                                 oSession_Filter.ClosuredaysDeltaFrom, oSession_Filter.ClosuredaysDeltaTo, UIView)
                      select tbl).ToList();

            return result;
        }


        #endregion

        #region Control Reprots related

        public List<Tbl_Order_comments> GetControlReprotsExcel(string commentedBy, string ComemntDate, string Comments, string Cleardate, string Owner, string TeamName, string ReasonCode, string Region, string Report, string ReviewDate, string Signoff, string SignOffBy, string SignOffDate, string SnapshotDate)
        {
            var lstBCRExtract = db.usp_BCR_Excel_Export(Report, Owner, TeamName, Region, ReasonCode, ReviewDate, Cleardate, Comments, commentedBy, ComemntDate, Signoff, SignOffBy, SignOffDate, SnapshotDate).ToList();
            return lstBCRExtract;
        }

        public List<Tbl_Daily_Control_Reports_Summary> GetMyControlReportsSummary(string SAPVW_USERS, string Region, string Role, string Team)
        {
            var lstArchives = db.usp_BCR_GetSummary(SAPVW_USERS, Region, Role, Team).ToList();
            return lstArchives;
        }

        public List<Tbl_Daily_Control_Reports_Summary> GetTeamControlReportsSummary(string SAPVW_USERS, string Region, string Role, string Team)
        {
            var lstArchives = db.usp_BCR_GetSummary(SAPVW_USERS, Region, Role, Team).ToList();
            return lstArchives;
        }


        public List<TBL_ORDER_COMMENT_VIEW> GetControlReportsDetails(string SAPVW_USERS, string Region, string Role, string Team, string Country, string Pending, string Sorg, string ReportID, string CockpitUI, string OrderStatus)
        {
            var lstArchives = db.usp_GetCommentstoWork(SAPVW_USERS, Region, Role, Team, Country, Pending, "", Sorg, ReportID, CockpitUI, OrderStatus).ToList();
            return lstArchives;
        }

        public List<VW_BCR_SignOff_Summary> GetBCR_ClosedOrdersSummary(string Region, DateTime From, DateTime To)
        {
            var Summary = db.usp_Get_closed_BCR_Summary(Region, From, To).ToList();
            return Summary;
        }

        public List<Tbl_History_Comments> GetBCR_ClosedOrdersDetails(string Region, string Report, string OrderNumber, string OrderOwner, string CommentedFrom, string CommentedTo, string SignOffFrom, string SignOffTo, string CommentedBy, string SignOffBy, string SnapshotDateFrom, string SnapshotDateTo)
        {
            //var Details = db.usp_BCR_History_Details(Region, Report, Convert.ToDouble(OrderNumber), OrderOwner, CommentedFrom, CommentedTo, SignOffFrom, SignOffTo, CommentedBy, SignOffBy, SnapshotDateFrom, SnapshotDateTo).ToList();
            var Details = db.usp_BCR_History_Details(Region, Report, Convert.ToDouble(OrderNumber), OrderOwner, CommentedFrom, CommentedTo, SignOffFrom, SignOffTo, CommentedBy, SignOffBy).ToList();
            return Details;
        }

        #endregion

        #region Followups related

        public List<Tbl_Followup_Summary> GetMyFollowupsSummary(string NtLogin, string Team_Name)
        {
            var lstSummary = db.usp_Followup_Summary(NtLogin, Team_Name);
            return lstSummary.ToList();
        }

        public List<Tbl_Followup_Summary> GetTeamFollowupsSummary(string NtLogin, string Team_Name)
        {
            var lstSummary = db.usp_Followup_Summary(NtLogin, Team_Name);
            return lstSummary.ToList();
        }

        public List<Tbl_Followups> GetMyFollowupsList(string NtLogin, string Team_Name)
        {
            var lstSummary = db.usp_Catagory_Followups("TotalFollowups", NtLogin, "User");
            return lstSummary.ToList();
        }

        public List<Tbl_Followups> GetTeamFollowupsList(string NtLogin, string Team_Name)
        {
            var lstSummary = db.usp_Catagory_Followups("TotalFollowups", NtLogin, Team_Name);
            return lstSummary.ToList();
        }

        public List<Tbl_Followups> GetMyFollowupsPassedDueDateList(string NtLogin, string Team_Name)
        {
            var lstSummary = db.usp_Catagory_Followups("PassedDueDate", NtLogin, "User");
            return lstSummary.ToList();
        }

        public List<Tbl_Followups> GetTeamFollowupsPassedDueDateList(string NtLogin, string Team_Name)
        {
            var lstSummary = db.usp_Catagory_Followups("PassedDueDate", NtLogin, Team_Name);
            return lstSummary.ToList();
        }

        public List<Tbl_Followups> GetMyFollowupsDueTodayList(string NtLogin, string Team_Name)
        {
            var lstSummary = db.usp_Catagory_Followups("DueToday", NtLogin, "User");
            return lstSummary.ToList();
        }

        public List<Tbl_Followups> GetTeamFollowupsDueTodayList(string NtLogin, string Team_Name)
        {
            var lstSummary = db.usp_Catagory_Followups("DueToday", NtLogin, Team_Name);
            return lstSummary.ToList();
        }

        public List<Tbl_Followups> GetMyFollowupsReasignedList(string NtLogin, string Team_Name)
        {
            var lstSummary = db.usp_Catagory_Followups("ReasignedtoYou", NtLogin, "User");
            return lstSummary.ToList();
        }

        public List<Tbl_Followups> GetTeamFollowupsReasignedList(string NtLogin, string Team_Name)
        {
            var lstSummary = db.usp_Catagory_Followups("ReasignedtoYou", NtLogin, Team_Name);
            return lstSummary.ToList();
        }

        public List<Tbl_Followups> GetMyFollowupsSystemGeneratedList(string NtLogin, string Team_Name)
        {
            var lstSummary = db.usp_Catagory_Followups("SystemGenerated", NtLogin, "User");
            return lstSummary.ToList();
        }

        public List<Tbl_Followups> GetTeamFollowupsSystemGeneratedList(string NtLogin, string Team_Name)
        {
            var lstSummary = db.usp_Catagory_Followups("SystemGenerated", NtLogin, Team_Name);
            return lstSummary.ToList();
        }

        public List<Tbl_Followups> GetFollowupDetails(string NtLogin, int FollowupID)
        {
            var lstSummary = db.usp_Followups(FollowupID);
            return lstSummary.ToList();
        }

        public List<Tbl_Followups_History> GetFollowupHistory(double FollowupID)
        {
            var lstSummary = db.usp_GetFollowupHistory_Details(FollowupID);
            return lstSummary.ToList();
        }

        public bool CreateFollowup(Tbl_Followups oFollowup)
        {
            bool result = false;

            try
            {
                int MaxFollowupId = db.Tbl_Followups.Select(p => p.Followupid).Max();

                oFollowup.Followupid = MaxFollowupId;

                db.usp_CreateFollowup(oFollowup.Sales_Order, oFollowup.CustomerName, oFollowup.Description, oFollowup.Owner, oFollowup.DueDate, oFollowup.BacklogStatus, oFollowup.Comment, oFollowup.Status, oFollowup.Created_By, oFollowup.Created_On, oFollowup.Re_Assigned_To, oFollowup.Priority);
                db.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public bool UpdateFollowup(int ID, double SalesOrder, string customerName, string Description, string owner, DateTime DueDate, string Backlogstatus, string comment, string status, string ModifiedBy, DateTime Modified_On, string Re_assign, string Priority)
        {
            bool result = false;

            try
            {
                db.usp_UpdateFollowup(ID, SalesOrder, customerName, Description, owner, DueDate, Backlogstatus, comment, status, ModifiedBy, Modified_On, Re_assign, Priority);

                //db.AcceptAllChanges();
                result = true;
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public bool ReAssignFollowup(int followupID, string FullName)
        {
            bool result = false;

            try
            {
                db.usp_Reassign_Followup(followupID, FullName);

                //db.AcceptAllChanges();
                result = true;
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public List<Tbl_Followups> GetFollowupSummaryDeatilsBySalesOrder(double SalesOrder, string Status)
        {
            var lstSummary = db.usp_FollowUps_By_SalesOrderNumber(SalesOrder, Status);
            return lstSummary.ToList();
        }


        #endregion

        #region Adding VW_USERS Feedback

        public bool Add_OrderComment(Tbl_Order_comments objComment, string ActionUser)
        {
            int result = db.usp_Update_User_Comments_SignOff(objComment.Report, objComment.Sales_Ord, objComment.Sales_Org, objComment.Sold_To_Customer_Name, objComment.Order_Date, objComment.Net_Value,
                objComment.Line_Item, objComment.Material, objComment.OrderOwner, objComment.NextAction, objComment.Reason_Code, objComment.Reviewdate, objComment.Approved_To_Date, objComment.Cleardate, objComment.Comment, objComment.SignOff, ActionUser);

            if (result > 0)
                return true;
            else
                return false;
        }

        public bool Add_OrderSignOff(Tbl_Order_comments objComment, string ActionUser)
        {
            int result = db.usp_Update_User_Comments_SignOff(objComment.Report, objComment.Sales_Ord, objComment.Sales_Org, objComment.Sold_To_Customer_Name, objComment.Order_Date, objComment.Net_Value,
                objComment.Line_Item, objComment.Material, objComment.OrderOwner, objComment.NextAction, objComment.Reason_Code, objComment.Reviewdate, objComment.Approved_To_Date, objComment.Cleardate, objComment.Comment, objComment.SignOff, ActionUser);


            if (result > 0)
                return true;
            else
                return false;
        }

        public bool AddMultipleOrderComments(List<Tbl_Order_comments> lstComments, string ActionUser)
        {
            bool resultVar = false;

            foreach (Tbl_Order_comments objComment in lstComments)
            {
                int result = db.usp_Update_User_Comments_SignOff(objComment.Report, objComment.Sales_Ord, objComment.Sales_Org, objComment.Sold_To_Customer_Name, objComment.Order_Date, objComment.Net_Value,
                objComment.Line_Item, objComment.Material, objComment.OrderOwner, objComment.NextAction, objComment.Reason_Code, objComment.Reviewdate, objComment.Approved_To_Date, objComment.Cleardate, objComment.Comment, objComment.SignOff, ActionUser);
                resultVar = true;
            }
            if (resultVar)
                return true;
            else
                return false;
        }

        public bool AddMultipleOrderSignOffs(List<Tbl_Order_comments> lstComments, string ActionUser)
        {
            bool resultVar = false;

            foreach (Tbl_Order_comments objComment in lstComments)
            {
                int result = db.usp_Update_User_Comments_SignOff(objComment.Report, objComment.Sales_Ord, objComment.Sales_Org, objComment.Sold_To_Customer_Name, objComment.Order_Date, objComment.Net_Value,
                objComment.Line_Item, objComment.Material, objComment.OrderOwner, objComment.NextAction, objComment.Reason_Code, objComment.Reviewdate, objComment.Approved_To_Date, objComment.Cleardate, objComment.Comment, objComment.SignOff, ActionUser);
                resultVar = true;
            }
            if (resultVar)
                return true;
            else
                return false;
        }

        #endregion

        #region OSBR Notifications

        public List<Tbl_OSBR_NOTIFICATIONS> GetMyOSBRNotifications(string SAPVW_USERS)
        {
            List<Tbl_OSBR_NOTIFICATIONS> results = new List<Tbl_OSBR_NOTIFICATIONS>();

            results = db.usp_OSBR_Notifications(SAPVW_USERS, "", "").ToList();

            return results;
        }


        public List<Tbl_OSBR_NOTIFICATIONS> GetTeamOSBRNotifications(string Region, string TeamName)
        {
            List<Tbl_OSBR_NOTIFICATIONS> results = new List<Tbl_OSBR_NOTIFICATIONS>();

            results = db.usp_OSBR_Notifications("", TeamName, Region).ToList();

            return results;
        }

        //public List<VW_Orders_Info> Get_OSBR_Order_Info(string SalesOrderNumber)
        //{
        //    List<VW_Orders_Info> results = new List<VW_Orders_Info>();

        //    results = (from Tbl in db.usp_SNI_All_Orders("", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 0, 0, 0, 0, SalesOrderNumber, "", "N", "")
        //               select Tbl).ToList();

        //    return results;
        //}

        #endregion

        #region Analytics related

        public List<Tbl_SNI_Release_Projection> SNI_Release_Target_Projection(string Fromdate, string Todate, string Region, string Team)
        {
            List<Tbl_SNI_Release_Projection> lstResults = new List<Tbl_SNI_Release_Projection>();

            lstResults = db.usp_SNI_Release_Target_and_Invoice(Fromdate, Todate, Region, Team).ToList();

            return lstResults;
        }

        public List<Tbl_SNI_Release_Projection> DB_Release_Target_Projection(string Fromdate, string Todate, string Region, string Team)
        {
            List<Tbl_SNI_Release_Projection> lstResults = new List<Tbl_SNI_Release_Projection>();

            lstResults = db.usp_DB_Release_Target_and_Invoice(Fromdate, Todate, Region, Team).ToList();

            return lstResults;
        }

        public List<Tbl_WW_Blocked_Orders_Summary> WW_Process_Summary(string Area, string Bucket)
        {
            List<Tbl_WW_Blocked_Orders_Summary> lstResults = new List<Tbl_WW_Blocked_Orders_Summary>();

            lstResults = db.usp_WW_Process_Summary(Area, Bucket).ToList();

            return lstResults;
        }

        #endregion

        #region Order Ownership

        public List<Tbl_Order_Action_Owner> GetReassignedOrdersOFUser(string Username)
        {
            List<Tbl_Order_Action_Owner> lstResults = new List<Tbl_Order_Action_Owner>();

            lstResults = db.usp_get_User_Order_Ownership(Username).ToList();
            return lstResults;
        }

        public bool ReAssignUserOrders(string OrderNo, string Username)
        {
            bool result = false;
            try
            {
                db.usp_Rollback_Order_Ownership(OrderNo, Username);
                result = true;
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        #endregion
    }
}
