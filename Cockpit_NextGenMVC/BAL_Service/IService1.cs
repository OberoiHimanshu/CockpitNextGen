using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


namespace BAL_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetData(int value);

        // TODO: Add your service operations here

        #region User Management Module

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<VW_USERS> GetUserDetails(string NtLogin);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<VW_USERS> GetUserTeamDetails(string TeamName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<VW_USERS> GetUserRegionDetails(string RegionName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<VW_USERS> GetInActiveUserTeamDetails(string TeamName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool RegisterNewUser(TBL_USERS oUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateUserDetails(TBL_USERS oUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<VW_USERS> GetUsersByRegion(string Region);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Unmapped_Users> GetUnMappedUsersByRegion(string Region);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool ActiveDeactiveUser(TBL_USERS oUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Country_Sorg_Orig> GetCountryByRegion(string Region);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TBL_TEAM_STRUCTURE> GetManagerByTeamID(int TeamID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TBL_ROLE> GetRoleMaster();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TBL_TEAM_STRUCTURE> GetTeamMaster(string Region);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Unmapped_Orders_By_Region_Function> GetUnMappedUsersByRegionFunction(string Region, string BusinessFunction);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<VW_USERS> GetNTLoginBySAPName(string SAPName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateUserInSApandBBZTRD(string OrderOwner, string CreatedBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string getEmailIDByName(string Name);

        #endregion

        #region Generic Methods

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Order_Header_Details> GetHeaderInfo(double Sales_Order);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Order_Delivery_Info> GetDeliveryInfo(double Sales_Order);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Order_Partner_Details> GetPartnerInfo(double Sales_Order);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Order_Block_Details> GetBlockInfo(double Sales_Order);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Order_Items> GetLineItems(double Sales_Order);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void AddOrder_Ownership(string SalesOrder, string DefaultOwner, string CurrentOwner, string CurrentOwnerSAP, string Ownership_Type, DateTime Ownership_From, DateTime Ownership_To, string Assigned_By, DateTime Assigned_On);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Archives> GetArchivesList();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<VW_Orders_Info> Ge_DB_ArchivesList(int ReportID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Review_Reports> Get_Reports_Master();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Dim_Business_Master> Get_Business_Master();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Dim_Billing_Blocks> Get_Billing_Blocks_Master();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Dim_Delivery_Blocks> Get_DeliveryBlocks_Master();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Dim_Sales_Force> Get_Sales_Force_Master(string Region);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Dim_Customers> Get_Customer_Master(string Region);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string Translate_Comment_From_Google(string strComment);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string Translate_Comment_From_Microsoft(string strComment);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool Translate_Mass_Comment_From_Microsoft(string strCommentedBy, DateTime CommentedOn);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Order_Search> Search_Global_Order(double Sales_Order);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<VW_Orders_Info> Get_Archived_Orders(string CustomerName, string OrderOwner);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Archival_Summary> Get_Archived_Orders_Summary(string Region, string Sorg, string Business, string Team);

        #endregion

        #region Billing Block related Orders

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<VW_Orders_Info> Get_SNIAllOrders(Session_Filters oSession_Filter, string ReportCatagory, string UIView);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<VW_Orders_Info> Get_SNICatagoryOrders(Session_Filters oSession_Filter, string ReportCatagory, string UIView);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SNI_Excel_View> Get_SNICatagoryOrdersExport(Session_Filters oSession_Filter, string ReportCatagory, string UIView);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Order_comments> GetMasterFocusChildOrders(string Order_Number);


        #endregion

        #region Delivery Blocked Related

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Tbl_Backlog_Summary GetMasterFocusSummary(string FullName, string UIArea);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_STR_Summary> GetSTR_Summary(DateTime snapshotDateFrom, DateTime snapshotDateTo);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SNI_Excel_View> Get_DB_CatagoryOrdersExport(Session_Filters oSession_Filter, string ReportCatagory, string UIView);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<VW_Orders_Info> Get_DB_RawOrders(Session_Filters oSession_Filter, string ReportCatagory, string UIView);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<VW_DB_Speed_To_Revenue> Get_DB_SpeedToRevenue(string SAPName, string Team, string BU, string Div, string Region, string Order_Number, string DateCondition, string SnapshotDateFrom, string SnapshotDateTo, string DBCutOffDate, string ERDFlag, string ERDFrom, string ERDTo, string SAPStatus);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<VW_Orders_Info> Get_DB_Overdue(Session_Filters oSession_Filter, string ReportCatagory, string UIView);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<VW_Orders_Info> Get_DB_Greater_90_days(Session_Filters oSession_Filter, string ReportCatagory, string UIView);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<VW_Orders_Info> Get_DB_Expected_Release_Passed(Session_Filters oSession_Filter, string ReportCatagory, string UIView);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<VW_Orders_Info> Get_DB_Overdue_14_Days(Session_Filters oSession_Filter, string ReportCatagory, string UIView);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<VW_Orders_Info> Get_Unblocked_All(Session_Filters oSession_Filter, string ReportCatagory, string UIView);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<VW_DB_Overdue> GetUnblocked_Overdue(Session_Filters oSession_Filter, string ReportCatagory, string UIView);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SNI_Excel_View> Get_UB_CatagoryOrdersExport(Session_Filters oSession_Filter, string ReportCatagory, string UIView);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Order_comments> GetDB_MasterFocusChildOrders(string Order_Number);

        #endregion

        #region Control Reports Related

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Order_comments> GetControlReprotsExcel(string commentedBy, string ComemntDate, string Comments, string Cleardate, string Owner, string TeamName, string ReasonCode, string Region, string Report, string ReviewDate, string Signoff, string SignOffBy, string SignOffDate, string SnapshotDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Daily_Control_Reports_Summary> GetMyControlReportsSummary(string SAPVW_USERS, string Region, string Role, string Team);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Daily_Control_Reports_Summary> GetTeamControlReportsSummary(string SAPVW_USERS, string Region, string Role, string Team);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TBL_ORDER_COMMENT_VIEW> GetControlReportsDetails(string SAPVW_USERS, string Region, string Role, string Team, string Country, string Pending, string Sorg, string ReportID, string CockpitUI, string OrderStatus);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<VW_BCR_SignOff_Summary> GetBCR_ClosedOrdersSummary(string Region, DateTime From, DateTime To);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_History_Comments> GetBCR_ClosedOrdersDetails(string Region, string Report, string OrderNumber, string OrderOwner, string CommentedFrom, string CommentedTo, string SignOffFrom, string SignOffTo, string CommentedBy, string SignOffBy, string SnapshotDateFrom, string SnapshotDateTo);


        #endregion

        #region Followups related

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Followup_Summary> GetMyFollowupsSummary(string NtLogin, string Team_Name);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Followups> GetMyFollowupsList(string NtLogin, string Team_Name);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Followups> GetTeamFollowupsList(string NtLogin, string Team_Name);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Followup_Summary> GetTeamFollowupsSummary(string NtLogin, string Team_Name);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Followups> GetMyFollowupsPassedDueDateList(string NtLogin, string Team_Name);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Followups> GetTeamFollowupsPassedDueDateList(string NtLogin, string Team_Name);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Followups> GetMyFollowupsDueTodayList(string NtLogin, string Team_Name);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Followups> GetTeamFollowupsDueTodayList(string NtLogin, string Team_Name);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Followups> GetMyFollowupsReasignedList(string NtLogin, string Team_Name);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Followups> GetTeamFollowupsReasignedList(string NtLogin, string Team_Name);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Followups> GetMyFollowupsSystemGeneratedList(string NtLogin, string Team_Name);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Followups> GetTeamFollowupsSystemGeneratedList(string NtLogin, string Team_Name);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Followups> GetFollowupDetails(string NtLogin, int FollowupID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Followups_History> GetFollowupHistory(double FollowupID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool CreateFollowup(Tbl_Followups oFollowup);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateFollowup(int ID, double SalesOrder, string customerName, string Description, string owner, DateTime DueDate, string Backlogstatus, string comment, string status, string ModifiedBy, DateTime Modified_On, string Re_assign, string Priority);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool ReAssignFollowup(int followupID, string FullName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Followups> GetFollowupSummaryDeatilsBySalesOrder(double SalesOrder, string Status);



        #endregion

        #region OSBR Notifications

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_OSBR_NOTIFICATIONS> GetMyOSBRNotifications(string SAPVW_USERS);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_OSBR_NOTIFICATIONS> GetTeamOSBRNotifications(string Region, string TeamName);

        #endregion

        #region VW_USERS Feedbacks related

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool Add_OrderComment(Tbl_Order_comments objComment, string ActionUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool Add_OrderSignOff(Tbl_Order_comments objComment, string ActionUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddMultipleOrderComments(List<Tbl_Order_comments> lstComments, string ActionUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddMultipleOrderSignOffs(List<Tbl_Order_comments> lstComments, string ActionUser);

        #endregion

        #region Analytics Related

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_SNI_Release_Projection> SNI_Release_Target_Projection(string Fromdate, string Todate, string Region, string Team);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_SNI_Release_Projection> DB_Release_Target_Projection(string Fromdate, string Todate, string Region, string Team);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_WW_Blocked_Orders_Summary> WW_Process_Summary(string Area, string Bucket);

        #endregion

        #region Order Ownership

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tbl_Order_Action_Owner> GetReassignedOrdersOFUser(string UserName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool ReAssignUserOrders(string OrderNo, string Username);

        #endregion

    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations
    [DataContract]
    public class ServiceException
    {
        [DataMember]
        public bool Result { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public string ErrorDetails { get; set; }
    }
}
