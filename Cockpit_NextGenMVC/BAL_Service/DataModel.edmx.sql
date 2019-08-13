
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/26/2019 16:47:46
-- Generated from EDMX file: C:\Users\HOberoi\Documents\GitHub\A.Com-Projects\Cockpit_NextGenMVC\BAL_Service\DataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Cockpit_NewGen_Dev];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[Dim_Billing_Blocks]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[Dim_Billing_Blocks];
GO
IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[Dim_Business_Master]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[Dim_Business_Master];
GO
IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[Dim_Customers]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[Dim_Customers];
GO
IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[Dim_Delivery_Blocks]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[Dim_Delivery_Blocks];
GO
IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[Dim_Sales_Force]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[Dim_Sales_Force];
GO
IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[Tbl_Archival_Summary]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[Tbl_Archival_Summary];
GO
IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[Tbl_Archives]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[Tbl_Archives];
GO
IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[Tbl_Backlog_Summary]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[Tbl_Backlog_Summary];
GO
IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[Tbl_Country_Sorg_Orig]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[Tbl_Country_Sorg_Orig];
GO
IF OBJECT_ID(N'[dbo].[Tbl_Daily_Control_Reports_Summary]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tbl_Daily_Control_Reports_Summary];
GO
IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[Tbl_Followup_Summary]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[Tbl_Followup_Summary];
GO
IF OBJECT_ID(N'[dbo].[Tbl_Followups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tbl_Followups];
GO
IF OBJECT_ID(N'[dbo].[Tbl_Followups_History]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tbl_Followups_History];
GO
IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[Tbl_History_Comments]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[Tbl_History_Comments];
GO
IF OBJECT_ID(N'[dbo].[Tbl_Order_Action_Owner]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tbl_Order_Action_Owner];
GO
IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[Tbl_Order_Block_Details]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[Tbl_Order_Block_Details];
GO
IF OBJECT_ID(N'[dbo].[TBL_ORDER_COMMENT_VIEW]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TBL_ORDER_COMMENT_VIEW];
GO
IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[Tbl_Order_comments]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[Tbl_Order_comments];
GO
IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[Tbl_Order_Delivery_Info]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[Tbl_Order_Delivery_Info];
GO
IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[Tbl_Order_Header_Details]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[Tbl_Order_Header_Details];
GO
IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[Tbl_Order_Items]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[Tbl_Order_Items];
GO
IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[Tbl_Order_Partner_Details]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[Tbl_Order_Partner_Details];
GO
IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[Tbl_Order_Search]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[Tbl_Order_Search];
GO
IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[Tbl_OSBR_NOTIFICATIONS]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[Tbl_OSBR_NOTIFICATIONS];
GO
IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[Tbl_Review_Reports]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[Tbl_Review_Reports];
GO
IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[TBL_ROLE]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[TBL_ROLE];
GO
IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[Tbl_SNI_Release_Projection]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[Tbl_SNI_Release_Projection];
GO
IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[Tbl_STR_Summary]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[Tbl_STR_Summary];
GO
IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[TBL_TEAM_STRUCTURE]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[TBL_TEAM_STRUCTURE];
GO
IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[Tbl_Unmapped_Orders_By_Region_Function]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[Tbl_Unmapped_Orders_By_Region_Function];
GO
IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[Tbl_Unmapped_Users]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[Tbl_Unmapped_Users];
GO
IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[TBL_USERS]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[TBL_USERS];
GO
IF OBJECT_ID(N'[dbo].[Tbl_WW_Blocked_Orders_Summary]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tbl_WW_Blocked_Orders_Summary];
GO
IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[SNI_Excel_View]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[SNI_Excel_View];
GO
IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[VW_BCR_SignOff_Summary]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[VW_BCR_SignOff_Summary];
GO
IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[VW_DB_All_Orders]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[VW_DB_All_Orders];
GO
IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[VW_DB_Speed_To_Revenue]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[VW_DB_Speed_To_Revenue];
GO
IF OBJECT_ID(N'[Cockpit_NewGenerationModelStoreContainer].[VW_USERS]', 'U') IS NOT NULL
    DROP TABLE [Cockpit_NewGenerationModelStoreContainer].[VW_USERS];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Tbl_Archives'
CREATE TABLE [dbo].[Tbl_Archives] (
    [ID] int  NOT NULL,
    [ReportName] varchar(50)  NULL,
    [ArchiveDate] int  NOT NULL,
    [Year] char(4)  NULL,
    [Month] char(30)  NULL,
    [Report_Path] varchar(100)  NOT NULL
);
GO

-- Creating table 'Tbl_Followups'
CREATE TABLE [dbo].[Tbl_Followups] (
    [Followupid] int  NOT NULL,
    [Sales_Order] float  NULL,
    [CustomerName] varchar(200)  NULL,
    [Description] varchar(max)  NULL,
    [Owner] varchar(200)  NULL,
    [DueDate] datetime  NULL,
    [BacklogStatus] varchar(200)  NULL,
    [Comment] varchar(max)  NOT NULL,
    [Status] varchar(200)  NOT NULL,
    [Created_By] nvarchar(50)  NULL,
    [Created_On] datetime  NOT NULL,
    [Re_Assigned_To] varchar(200)  NULL,
    [Priority] nvarchar(50)  NULL,
    [Modified_By] nvarchar(50)  NULL,
    [Modified_On] datetime  NOT NULL,
    [Owner_Full_Name] nvarchar(50)  NULL
);
GO

-- Creating table 'Tbl_Followup_Summary'
CREATE TABLE [dbo].[Tbl_Followup_Summary] (
    [id] int  NOT NULL,
    [TotalFollowups] int  NULL,
    [PassedDuedate] int  NULL,
    [DueToday] int  NULL,
    [ReassignedtoYou] int  NULL,
    [SystemGenerated] int  NULL
);
GO

-- Creating table 'Tbl_Daily_Control_Reports_Summary'
CREATE TABLE [dbo].[Tbl_Daily_Control_Reports_Summary] (
    [ReportName] nvarchar(50)  NOT NULL,
    [OrderOwner] nvarchar(50)  NOT NULL,
    [Pending_Comments] int  NULL,
    [Pending_Review] int  NULL,
    [Pending_Sign_Off] int  NULL,
    [Region] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Tbl_STR_Summary'
CREATE TABLE [dbo].[Tbl_STR_Summary] (
    [Region] nvarchar(50)  NOT NULL,
    [TotalDBFocusList] float  NULL,
    [TotalOrders] int  NULL,
    [TargetPercentageQExit] nvarchar(50)  NULL,
    [TargetReduction] float  NULL,
    [DBReleased] float  NULL,
    [PercentageReleased] nvarchar(50)  NULL,
    [DBReleasedOrders] int  NULL,
    [DBOrders] float  NULL,
    [DBOrdersNo] int  NULL,
    [CancelledOrders] float  NULL,
    [CancelledOrdersNo] int  NULL,
    [AdjustedTotalDBFocusList] float  NULL,
    [AdjustedPerformance] nvarchar(50)  NULL,
    [LastRefreshDate] datetime  NULL
);
GO

-- Creating table 'VW_DB_Speed_To_Revenue'
CREATE TABLE [dbo].[VW_DB_Speed_To_Revenue] (
    [SALES_ORDERNO] float  NOT NULL,
    [SOLD_TO_PARTY_NAME] nvarchar(100)  NULL,
    [ORDER_OWNER] nvarchar(50)  NULL,
    [PRIMARY_PRODUCT] nvarchar(50)  NULL,
    [SALES_ORG] nvarchar(15)  NULL,
    [DB_CLOSURE_STATUS] varchar(50)  NULL,
    [BACKLOG_AMT] float  NULL,
    [ORDER_AGE] int  NULL,
    [Aging_Bucket] nvarchar(50)  NULL,
    [Dollar_Bucket] varchar(50)  NULL,
    [DELIVERY_BLK_HDR_CD] nvarchar(10)  NULL,
    [DELIVERY_BLK_HDR_DESC] nvarchar(100)  NULL,
    [BILLING_BLOCK_CD] nvarchar(10)  NULL,
    [BILLING_BLOCK_DESC] nvarchar(100)  NULL,
    [PRIORITY] int  NULL,
    [DELIVERY_BLOCK_CUT_OFF_DATE] datetime  NULL,
    [SHIPMENT_CUT_OFF_DATE] datetime  NULL,
    [BACKLOG_STATUS] nvarchar(100)  NULL,
    [ReqDlyDate] datetime  NULL,
    [EARLY_DEL_ACCEPTABLE] nchar(10)  NULL,
    [SHIP_TO_COUNTRY] nvarchar(50)  NULL,
    [QUOTA_SF] int  NULL,
    [FE_FE_DESC] nvarchar(100)  NULL,
    [SRTATTR_AREA_MGR_NME] nvarchar(100)  NULL,
    [SRTATTR_DISTRICT_MGR_NME] nvarchar(100)  NULL,
    [SnapshotDate] datetime  NOT NULL,
    [INVOICING_STATUS] nvarchar(50)  NULL,
    [REGION] nvarchar(50)  NULL,
    [ACTION_OWNER] varchar(50)  NULL,
    [EXPECTED_RELEASE_DATE] datetime  NULL,
    [REASON_CODE] nvarchar(100)  NULL,
    [LATEST_COMMENT] nvarchar(max)  NULL
);
GO

-- Creating table 'Tbl_Order_comments'
CREATE TABLE [dbo].[Tbl_Order_comments] (
    [Report] nvarchar(50)  NOT NULL,
    [Sales_Ord] float  NOT NULL,
    [Material] nvarchar(30)  NOT NULL,
    [OrderOwner] nvarchar(50)  NOT NULL,
    [Reason_Code] nvarchar(100)  NULL,
    [NextAction] nvarchar(max)  NULL,
    [Reviewdate] datetime  NULL,
    [Cleardate] datetime  NULL,
    [Comment] nvarchar(max)  NOT NULL,
    [SignOff] varchar(3)  NULL,
    [snapshotdate] datetime  NULL,
    [Comment_Date] datetime  NOT NULL,
    [Commented_By] varchar(50)  NULL,
    [SignOff_By] varchar(50)  NULL,
    [SignOff_Date] datetime  NULL,
    [Region] nvarchar(50)  NULL,
    [Approved_To_Date] datetime  NULL,
    [Line_Item] int  NULL,
    [Sales_Org] nvarchar(10)  NULL,
    [Order_Date] datetime  NULL,
    [Net_Value] float  NULL,
    [Sold_To_Customer_Name] nvarchar(100)  NULL
);
GO

-- Creating table 'Tbl_Review_Reports'
CREATE TABLE [dbo].[Tbl_Review_Reports] (
    [ReportName] varchar(50)  NOT NULL,
    [Status] varchar(30)  NULL,
    [Frequency] nchar(10)  NULL,
    [CustomFilter] varchar(50)  NULL,
    [SAP_T_Code] nvarchar(100)  NULL,
    [Description1] nvarchar(max)  NULL,
    [Description2] nvarchar(max)  NULL
);
GO

-- Creating table 'Tbl_Backlog_Summary'
CREATE TABLE [dbo].[Tbl_Backlog_Summary] (
    [SnapshotDate] datetime  NOT NULL,
    [SAP_User] nvarchar(50)  NULL,
    [Region] nvarchar(20)  NULL,
    [Sorg] nvarchar(10)  NULL,
    [Total_SNI] float  NULL,
    [Total_SNI_Count] int  NULL,
    [Total_SNI_Aged] float  NULL,
    [Total_SNI_Aged_Count] int  NULL,
    [Total_SNI_Expected_Release_Today] float  NULL,
    [Total_SNI_Expected_Release_Today_Count] int  NULL,
    [Total_SNI_Expected_Released_Passed] float  NULL,
    [Total_SNI_Expected_Released_Passed_Count] int  NULL,
    [Total_SNI_No_Expected_Release_Date] float  NULL,
    [Total_SNI_No_Expected_Release_Date_Count] int  NULL,
    [Total_SNI_MF_Invoicing_Errors] float  NULL,
    [Total_SNI_MF_Invoicing_Errors_Count] int  NULL,
    [Total_DB] float  NULL,
    [Total_DB_Count] int  NULL,
    [Total_DB_greater_90_Days] float  NULL,
    [Total_DB_greater_90_Days_Count] int  NULL,
    [Total_DB_Expected_Released_Passed] float  NULL,
    [Total_DB_Expected_Released_Passed_Count] int  NULL,
    [Total_DB_Overdue] float  NULL,
    [Total_DB_Overdue_Count] int  NULL,
    [Total_DB_Overdue_14_Days] float  NULL,
    [Total_DB_Overdue_14_Days_Count] int  NULL,
    [Total_DB_SppedtoRevenue] float  NULL,
    [Total_DB_SppedtoRevenue_Count] int  NULL,
    [Total_Unblocked_Orders] float  NULL,
    [Total_Unblocked_Orders_Count] int  NULL,
    [Total_Unblocked_Overdue] float  NULL,
    [Total_Unblocked_Overdue_Count] int  NULL,
    [Total_OSBR_Notification] float  NULL,
    [Total_OSBR_Notification_Count] int  NULL
);
GO

-- Creating table 'Dim_Business_Master'
CREATE TABLE [dbo].[Dim_Business_Master] (
    [BUSINESS_GROUP] nvarchar(50)  NOT NULL,
    [REGION] nvarchar(50)  NOT NULL,
    [DIVISION] nvarchar(50)  NOT NULL,
    [SALES_ORG] nvarchar(50)  NOT NULL,
    [PRODUCT_LINE] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Dim_Billing_Blocks'
CREATE TABLE [dbo].[Dim_Billing_Blocks] (
    [BILLING_BLOCK_CD] nvarchar(50)  NOT NULL,
    [BILLING_BLOCK_DESC] nvarchar(255)  NULL
);
GO

-- Creating table 'Dim_Delivery_Blocks'
CREATE TABLE [dbo].[Dim_Delivery_Blocks] (
    [DELIVERY_BLK_HDR_CD] nvarchar(50)  NOT NULL,
    [DELIVERY_BLK_HDR_DESC] nvarchar(255)  NULL
);
GO

-- Creating table 'Dim_Customers'
CREATE TABLE [dbo].[Dim_Customers] (
    [REGION] nvarchar(50)  NOT NULL,
    [SOLD_TO_COUNTRY] nvarchar(100)  NOT NULL,
    [SHIP_TO_COUNTRY] nvarchar(50)  NOT NULL,
    [SOLD_TO_PARTY] nvarchar(50)  NOT NULL,
    [SOLD_TO_PARTY_NAME] nvarchar(255)  NULL,
    [SHIP_TO_PARTY] nvarchar(50)  NOT NULL,
    [SHIP_TO_PARTY_NAME] nvarchar(255)  NULL,
    [ZU_ACCOUNT_NAME] nvarchar(255)  NULL,
    [ZU_COUNTRY] nvarchar(100)  NULL,
    [PAYMENT_TYPE] nvarchar(100)  NULL
);
GO

-- Creating table 'Dim_Sales_Force'
CREATE TABLE [dbo].[Dim_Sales_Force] (
    [REGION] nvarchar(50)  NOT NULL,
    [QUOTA_SF] nvarchar(50)  NOT NULL,
    [FE_FE_DESC] nvarchar(100)  NOT NULL,
    [SRTATTR_AREA_MGR_NME] nvarchar(100)  NULL,
    [SRTATTR_DISTRICT_MGR_NME] nvarchar(100)  NULL
);
GO

-- Creating table 'Tbl_SNI_Release_Projection'
CREATE TABLE [dbo].[Tbl_SNI_Release_Projection] (
    [SALES_ORDERNO] float  NOT NULL,
    [BUSINESS_SCENARIO] varchar(50)  NULL,
    [BACKLOG_STATUS] nvarchar(255)  NULL,
    [ORDER_CREATED_BY] nvarchar(255)  NULL,
    [SALES_ORG] nvarchar(255)  NULL,
    [REGION] nvarchar(255)  NOT NULL,
    [DIVISION] nchar(30)  NOT NULL,
    [BILLING_BLOCK_CD] nvarchar(255)  NULL,
    [BILLING_BLOCK_DESC] nvarchar(255)  NULL,
    [DELIVERY_BLK_HDR_DESC] nvarchar(255)  NULL,
    [Frcst_EstLoadDate] datetime  NULL,
    [Feedback] datetime  NULL,
    [Feedback_MonthName] nvarchar(30)  NULL,
    [BB_CycleTimeTarget] int  NULL,
    [Feedback_Calc_Est_Invoice_dt] datetime  NULL,
    [Feedback_Calc_Est_Invoice_dt_Month] nvarchar(30)  NULL,
    [Est_InvoiceDate] datetime  NULL,
    [Est_MonthName] nvarchar(30)  NULL,
    [BAcklog] float  NOT NULL,
    [PRIMARY_PRODUCT] nvarchar(255)  NULL,
    [TEAM_NAME] nvarchar(150)  NULL
);
GO

-- Creating table 'Tbl_OSBR_NOTIFICATIONS'
CREATE TABLE [dbo].[Tbl_OSBR_NOTIFICATIONS] (
    [SALES_DOC] float  NOT NULL,
    [ITEM] int  NOT NULL,
    [MATERIAL] nvarchar(255)  NULL,
    [SORG] nvarchar(50)  NULL,
    [SATY] nvarchar(20)  NULL,
    [CREATED_BY] nvarchar(50)  NULL,
    [CUSTOMER] nvarchar(255)  NULL,
    [PL_DV] nvarchar(50)  NULL,
    [ASSIGNED_ENGINEER_TEXT] nvarchar(max)  NULL,
    [ORDER_DATE] datetime  NULL,
    [QTY] int  NULL,
    [CURR] nvarchar(50)  NULL,
    [TOT_LC_PRICE] float  NULL,
    [TOT_USD_PRICE] float  NULL,
    [DELIVERY_STATUS] nvarchar(100)  NULL,
    [INVOICING_STATUS] nvarchar(100)  NULL,
    [OWNER] nvarchar(100)  NULL,
    [REGION] nvarchar(50)  NULL
);
GO

-- Creating table 'TBL_USERS'
CREATE TABLE [dbo].[TBL_USERS] (
    [USER_ID] int IDENTITY(1,1) NOT NULL,
    [USERNAME] nvarchar(255)  NULL,
    [TEAM_ID] int  NULL,
    [ROLE_ID] int  NULL,
    [NTLOGIN] nvarchar(255)  NULL,
    [COUNTRY] nchar(30)  NULL,
    [FULLNAME] nvarchar(255)  NULL,
    [SUPERREGION] nvarchar(255)  NULL,
    [EMAIL] nvarchar(255)  NULL,
    [ACTIVE] nchar(10)  NULL,
    [SUBSCRIPTION_ID] int  NULL,
    [PROFILE_PIC] nvarchar(150)  NULL
);
GO

-- Creating table 'VW_USERS'
CREATE TABLE [dbo].[VW_USERS] (
    [USER_ID] int  NOT NULL,
    [SAP_User_Name] nvarchar(255)  NULL,
    [ROLE_DESC] varchar(50)  NULL,
    [TEAM_NAME] nvarchar(255)  NULL,
    [MANAGER] nvarchar(50)  NULL,
    [NTLOGIN] nvarchar(255)  NULL,
    [COUNTRY] nchar(30)  NULL,
    [FULLNAME] nvarchar(255)  NULL,
    [SUPERREGION] nvarchar(255)  NULL,
    [EMAIL] nvarchar(255)  NULL,
    [ACTIVE] nchar(10)  NULL,
    [PROFILE_PIC] nvarchar(150)  NULL
);
GO

-- Creating table 'Tbl_Order_Search'
CREATE TABLE [dbo].[Tbl_Order_Search] (
    [Sales_Order] float  NULL,
    [Found_in_Area] nvarchar(50)  NOT NULL,
    [Area_UI_Path] nvarchar(10)  NULL,
    [Area_UI_Bucket] nvarchar(100)  NULL,
    [Bucket_Security_On] nvarchar(10)  NULL,
    [Current_Owner] nvarchar(50)  NULL
);
GO

-- Creating table 'Tbl_Order_Block_Details'
CREATE TABLE [dbo].[Tbl_Order_Block_Details] (
    [SALES_ORDERNO] float  NOT NULL,
    [BACKLOG_STATUS] nvarchar(255)  NULL,
    [BACKLOG_AMT] float  NULL,
    [COMPLETE_DELIVERY_HEADER] nvarchar(50)  NULL,
    [NLHD_STATUS] nvarchar(50)  NULL,
    [DELIVERY_BLK_HDR_CD] nvarchar(50)  NULL,
    [DELIVERY_BLK_HDR_DESC] nvarchar(255)  NULL,
    [BILLING_BLOCK_CD] nvarchar(50)  NULL,
    [BILLING_BLOCK_DESC] nvarchar(255)  NULL,
    [DELIVERY_BLOCK_CUT_OFF_DATE] datetime  NULL,
    [DELV_BLK_COUNT] int  NULL,
    [SAP_DELV_BLK_DAYS] int  NULL,
    [DELV_BLK_LAST_APPLD_DT] datetime  NULL,
    [DELV_BLK_REL_DT] datetime  NULL,
    [DLVY_DT_CHANGE_REASON] nvarchar(max)  NULL,
    [DELV_BLK_TYPE] nvarchar(50)  NULL
);
GO

-- Creating table 'Tbl_Order_Delivery_Info'
CREATE TABLE [dbo].[Tbl_Order_Delivery_Info] (
    [SALES_ORDERNO] float  NOT NULL,
    [COMPLETE_DELIVERY_HEADER] nvarchar(50)  NULL,
    [NLHD_STATUS] nvarchar(50)  NULL,
    [CUSTOMER_REQ_GI_DATE] datetime  NULL,
    [ACTUAL_GI_DATE] datetime  NULL,
    [DELV_PRIO_CD_HDR] nvarchar(50)  NULL,
    [DELV_PRIO_DESC_HDR] nvarchar(255)  NULL,
    [EARLY_DEL_ACCEPTABLE] nvarchar(50)  NULL,
    [SHIPPING_POINT] nvarchar(50)  NULL,
    [SHIPMENT_CUT_OFF_DATE] datetime  NULL,
    [SHIP_CONDTN_DESC] nvarchar(255)  NULL,
    [SHIP_DT_CHANGE_REASON] nvarchar(max)  NULL,
    [DLVY_DT_CHANGE_REASON] nvarchar(max)  NULL
);
GO

-- Creating table 'Tbl_Order_Header_Details'
CREATE TABLE [dbo].[Tbl_Order_Header_Details] (
    [SALES_ORDERNO] float  NOT NULL,
    [CUSTOMER_PO_NO] nvarchar(100)  NULL,
    [ORDER_CREATED_BY] nvarchar(50)  NULL,
    [ORDER_OWNER] nvarchar(50)  NULL,
    [REGION] nvarchar(50)  NULL,
    [PRIMARY_PRODUCT] nvarchar(100)  NULL,
    [ORDER_AGE] int  NULL,
    [Aging_Bucket] nvarchar(50)  NULL,
    [BACKLOG_STATUS] nvarchar(255)  NULL,
    [BACKLOG_AMT] float  NULL,
    [PAYMENT_TERMS] nvarchar(50)  NULL,
    [PAYMENT_TERMS_DESC] nvarchar(255)  NULL,
    [PAYMENT_TYPE] nvarchar(100)  NULL,
    [COMPLETE_DELIVERY_HEADER] nvarchar(50)  NULL,
    [ORDER_DT] datetime  NULL,
    [DB_CLOSURE_DAYS_DELTA] int  NULL,
    [DELTA_LOAD_DATE_BUCKET] nvarchar(50)  NULL,
    [SNI_CLOSURE_DAYS_DELTA] int  NULL,
    [SNI_AGE] int  NULL,
    [SNI_AGING_BUCKET] nvarchar(50)  NULL,
    [OVERALL_INSTALLATION_STATUS] nvarchar(100)  NULL
);
GO

-- Creating table 'Tbl_Order_Items'
CREATE TABLE [dbo].[Tbl_Order_Items] (
    [SALES_ORDERNO] float  NOT NULL,
    [LINE_ITEM] int  NOT NULL,
    [MATERIAL_NO] nvarchar(50)  NULL,
    [MATERIAL_DESC] nvarchar(200)  NULL,
    [BUSINESS_GROUP] nvarchar(50)  NULL,
    [DIVISION] nvarchar(50)  NULL,
    [PRODUCT_LINE] nvarchar(50)  NULL,
    [PL_DIVISION_DESC] nvarchar(255)  NULL,
    [ITEM_BILLING_BLK_CD] nvarchar(50)  NOT NULL,
    [ITEM_BILLING_BLK_DESC] nvarchar(255)  NOT NULL,
    [DELV_BLK_LINE_CD] nvarchar(50)  NOT NULL,
    [DELV_BLK_LINE_DESC] nvarchar(255)  NOT NULL,
    [BACKLOG_STATUS] nvarchar(255)  NULL,
    [BACKLOG_AMT] float  NULL,
    [CUSTOMER_REQ_GI_DATE] datetime  NULL,
    [TRIO_LOAD_DATE] datetime  NULL,
    [COMMIT_DATE] datetime  NULL,
    [SERVICE_ORDER] nvarchar(max)  NULL,
    [CRM_ITEM_STATUS] nvarchar(50)  NULL,
    [CRM_INSTALL_START_DATE] nvarchar(max)  NULL,
    [CRM_INSTALL_END_DATE] nvarchar(max)  NULL,
    [CRM_INSTALL_COMPLETE_DATE] nvarchar(max)  NULL,
    [FSE_NAME] nvarchar(50)  NULL
);
GO

-- Creating table 'Tbl_Order_Partner_Details'
CREATE TABLE [dbo].[Tbl_Order_Partner_Details] (
    [SALES_ORDERNO] float  NOT NULL,
    [SOLD_TO_PARTY] nvarchar(50)  NULL,
    [SOLD_TO_PARTY_NAME] nvarchar(255)  NULL,
    [SOLD_TO_COUNTRY] nvarchar(100)  NULL,
    [SHIP_TO_PARTY_NAME] nvarchar(255)  NULL,
    [SHIP_TO_PARTY] nvarchar(50)  NULL,
    [SHIP_TO_COUNTRY] nvarchar(50)  NULL,
    [ZU_ACCOUNT_NAME] nvarchar(255)  NULL,
    [ZU_COUNTRY] nvarchar(100)  NULL,
    [QUOTA_SF] nvarchar(50)  NULL,
    [FE_FE_DESC] nvarchar(100)  NULL,
    [SRTATTR_AREA_MGR_NME] nvarchar(100)  NULL,
    [SRTATTR_DISTRICT_MGR_NME] nvarchar(100)  NULL,
    [ZU_ACCOUNT_ID] nvarchar(50)  NULL,
    [EN_ACCOUNT_NAME] nvarchar(255)  NULL,
    [EN_COUNTRY] nvarchar(100)  NULL
);
GO

-- Creating table 'VW_Orders_Info'
CREATE TABLE [dbo].[VW_Orders_Info] (
    [SALES_ORDERNO] float  NOT NULL,
    [CUSTOMER_PO_NO] nvarchar(100)  NULL,
    [SALES_ORG] nvarchar(50)  NULL,
    [ORDER_OWNER] nvarchar(50)  NULL,
    [ORDER_AGE] int  NULL,
    [SNI_AGING_BUCKET] nvarchar(50)  NOT NULL,
    [BACKLOG_AMT] float  NULL,
    [SOLD_TO_PARTY_NAME] nvarchar(255)  NULL,
    [PAYMENT_TERMS] nvarchar(50)  NULL,
    [DELIVERY_BLK_HDR_CD] nvarchar(50)  NULL,
    [BILLING_BLOCK_CD] nvarchar(50)  NULL,
    [NLHD_STATUS] nvarchar(50)  NULL,
    [COMMIT_DATE] datetime  NULL,
    [TRIO_LOAD_DATE] datetime  NOT NULL,
    [CUSTOMER_REQ_GI_DATE] datetime  NULL,
    [INCOTERMS] nvarchar(50)  NULL,
    [SNI_CLOSURE_STATUS] nvarchar(255)  NOT NULL,
    [DB_CLOSURE_STATUS] nvarchar(255)  NOT NULL,
    [EXPECTED_RELEASE_DATE] datetime  NULL,
    [REASON_CODE] nvarchar(255)  NULL,
    [LATEST_COMMENT] nvarchar(max)  NULL,
    [DB_AGING_BUCKET] nvarchar(50)  NOT NULL,
    [REGION] nvarchar(50)  NULL,
    [ZU_ACCOUNT_NAME] nvarchar(255)  NULL,
    [ZU_COUNTRY] nvarchar(100)  NULL,
    [DELTA_LOAD_DATE_BUCKET] nvarchar(50)  NOT NULL,
    [FE_FE_DESC] nvarchar(100)  NULL,
    [SNI_AGE] int  NOT NULL,
    [OVERALL_INSTALLATION_STATUS] nvarchar(50)  NULL,
    [LATEST_COMMENT_DATE_TIME] datetime  NULL,
    [QUOTA_SF] nvarchar(50)  NULL
);
GO

-- Creating table 'Tbl_Country_Sorg_Orig'
CREATE TABLE [dbo].[Tbl_Country_Sorg_Orig] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CountryID] int  NULL,
    [Country] varchar(30)  NULL,
    [Sorg] varchar(10)  NULL,
    [Region] varchar(30)  NULL
);
GO

-- Creating table 'TBL_TEAM_STRUCTURE'
CREATE TABLE [dbo].[TBL_TEAM_STRUCTURE] (
    [TEAM_ID] int  NOT NULL,
    [TEAM_NAME] nvarchar(255)  NULL,
    [SUPERVISOR] nvarchar(255)  NULL,
    [BACKUP] nvarchar(255)  NULL,
    [BACKUPFROM] datetime  NULL,
    [BACKUPTO] datetime  NULL,
    [BACKUP_COMMENTS] nvarchar(255)  NULL,
    [ACTIVE] int  NULL
);
GO

-- Creating table 'Tbl_WW_Blocked_Orders_Summary'
CREATE TABLE [dbo].[Tbl_WW_Blocked_Orders_Summary] (
    [Bucket_Rank] int  NOT NULL,
    [Bucket] nvarchar(30)  NOT NULL,
    [AFO] float  NULL,
    [EMEAI] float  NULL,
    [JFO] float  NULL,
    [SAPK] float  NULL,
    [GCFO] float  NULL,
    [WW] float  NULL,
    [BucketName] nvarchar(30)  NULL
);
GO

-- Creating table 'TBL_ROLE'
CREATE TABLE [dbo].[TBL_ROLE] (
    [ROLE_ID] int  NOT NULL,
    [ROLE_DESC] varchar(50)  NULL
);
GO

-- Creating table 'Tbl_Archival_Summary'
CREATE TABLE [dbo].[Tbl_Archival_Summary] (
    [Order_Owner] nvarchar(50)  NOT NULL,
    [Orders] int  NULL,
    [NetValue] float  NULL
);
GO

-- Creating table 'Tbl_Unmapped_Orders_By_Region_Function'
CREATE TABLE [dbo].[Tbl_Unmapped_Orders_By_Region_Function] (
    [REGION] nvarchar(50)  NOT NULL,
    [ORDER_CREATED_BY] nvarchar(50)  NOT NULL,
    [Orders] int  NULL,
    [Total_NV] float  NULL
);
GO

-- Creating table 'Tbl_Unmapped_Users'
CREATE TABLE [dbo].[Tbl_Unmapped_Users] (
    [REGION] nvarchar(50)  NOT NULL,
    [USERGROUP] nvarchar(50)  NOT NULL,
    [Order_Count] int  NULL,
    [Total_NV] float  NULL,
    [TotalUser] int  NULL
);
GO

-- Creating table 'Tbl_Followups_History'
CREATE TABLE [dbo].[Tbl_Followups_History] (
    [HistoryID] int IDENTITY(1,1) NOT NULL,
    [Followupid] int  NOT NULL,
    [Sales_Order] float  NULL,
    [CustomerName] varchar(200)  NULL,
    [Description] varchar(max)  NULL,
    [Owner] varchar(200)  NULL,
    [DueDate] datetime  NULL,
    [BacklogStatus] varchar(200)  NULL,
    [Comment] varchar(max)  NULL,
    [Status] varchar(200)  NOT NULL,
    [Created_By] nvarchar(50)  NULL,
    [Created_On] datetime  NULL,
    [Re_Assigned_To] varchar(200)  NULL,
    [Priority] nvarchar(50)  NULL,
    [Modified_By] nvarchar(50)  NULL,
    [Modified_On] datetime  NOT NULL,
    [Owner_Full_Name] nvarchar(50)  NULL
);
GO

-- Creating table 'TBL_ORDER_COMMENT_VIEW'
CREATE TABLE [dbo].[TBL_ORDER_COMMENT_VIEW] (
    [SALES_ORDERNO] float  NOT NULL,
    [CUSTOMER_PO_NO] nvarchar(100)  NULL,
    [SALES_ORG] nvarchar(10)  NULL,
    [ORDER_OWNER] nvarchar(100)  NULL,
    [BACKLOG_AMT] float  NOT NULL,
    [SOLD_TO_PARTY_NAME] nvarchar(100)  NULL,
    [EXPECTED_RELEASE_DATE] datetime  NULL,
    [REVIEW_DATE] datetime  NULL,
    [REASON_CODE] nvarchar(100)  NULL,
    [LATEST_COMMENT] nvarchar(max)  NULL,
    [COMMENTED_DATE] datetime  NULL,
    [COMMENTED_BY] nvarchar(10)  NULL,
    [REGION] nvarchar(10)  NULL,
    [ORDER_DT] datetime  NULL,
    [ORDER_AGE] int  NOT NULL,
    [APPROVED_TO_DATE] datetime  NULL,
    [SNAPSHOT_DATE] datetime  NULL,
    [CUSTOMER_REQ_GI_DATE] datetime  NULL,
    [CRDD_Age] int  NOT NULL,
    [FE_FE_DESC] nvarchar(100)  NULL,
    [ReportName] nvarchar(100)  NULL
);
GO

-- Creating table 'Tbl_History_Comments'
CREATE TABLE [dbo].[Tbl_History_Comments] (
    [Report] nvarchar(50)  NOT NULL,
    [Sales_Ord] float  NOT NULL,
    [Material] nvarchar(30)  NOT NULL,
    [OrderOwner] nvarchar(50)  NULL,
    [Region] nvarchar(50)  NULL,
    [Reason_Code] nvarchar(100)  NULL,
    [NextAction] nvarchar(max)  NULL,
    [Cleardate] datetime  NULL,
    [Comment] nvarchar(max)  NULL,
    [ReviewDate] datetime  NULL,
    [SignOff] varchar(3)  NULL,
    [snapshotdate] datetime  NULL,
    [Comment_Date] datetime  NOT NULL,
    [Commented_By] nvarchar(50)  NOT NULL,
    [SignOff_By] nvarchar(50)  NULL,
    [SignOff_Date] datetime  NULL,
    [Approved_To_Date] datetime  NULL,
    [Line_Item] int  NULL,
    [Sales_Org] nvarchar(10)  NULL,
    [Order_Date] datetime  NULL,
    [Net_Value] float  NULL,
    [Sold_To_Customer_Name] nvarchar(50)  NULL
);
GO

-- Creating table 'VW_BCR_SignOff_Summary'
CREATE TABLE [dbo].[VW_BCR_SignOff_Summary] (
    [Region] nvarchar(50)  NULL,
    [Report] nvarchar(50)  NOT NULL,
    [Commented_Sign_Off] int  NULL,
    [snapshotdateFrom] datetime  NULL,
    [snapshotdateTo] datetime  NULL
);
GO

-- Creating table 'SNI_Excel_View'
CREATE TABLE [dbo].[SNI_Excel_View] (
    [REGION] nvarchar(50)  NULL,
    [SALES_ORDERNO] float  NOT NULL,
    [CUSTOMER_PO_NO] nvarchar(100)  NULL,
    [SALES_ORG] nvarchar(50)  NULL,
    [BUSINESS_GROUP] nvarchar(50)  NOT NULL,
    [PRIMARY_PRODUCT] nvarchar(100)  NULL,
    [ORDER_CREATED_BY] nvarchar(50)  NULL,
    [ORDER_OWNER] nvarchar(50)  NULL,
    [BACKLOG_STATUS] nvarchar(255)  NULL,
    [DELV_BLK_TYPE] nvarchar(50)  NOT NULL,
    [ORDER_AGE] int  NULL,
    [Aging_Bucket] nvarchar(50)  NULL,
    [SNI_AGE] int  NOT NULL,
    [SNI_AGING_BUCKET] nvarchar(50)  NOT NULL,
    [SNI_CLOSURE_STATUS] nvarchar(255)  NOT NULL,
    [DB_CLOSURE_STATUS] nvarchar(255)  NOT NULL,
    [BACKLOG_AMT] float  NOT NULL,
    [COMPLETE_DELIVERY_HEADER] nvarchar(50)  NULL,
    [SOLD_TO_PARTY_NAME] nvarchar(255)  NULL,
    [SHIP_TO_PARTY_NAME] nvarchar(255)  NULL,
    [ZU_ACCOUNT_NAME] nvarchar(255)  NULL,
    [FE_FE_DESC] nvarchar(100)  NULL,
    [SRTATTR_AREA_MGR_NME] nvarchar(100)  NULL,
    [SRTATTR_DISTRICT_MGR_NME] nvarchar(100)  NULL,
    [PAYMENT_TERMS] nvarchar(50)  NULL,
    [PAYMENT_TYPE] nvarchar(100)  NULL,
    [SHIPPING_POINT] nvarchar(50)  NULL,
    [DELIVERY_BLK_HDR_CD] nvarchar(50)  NOT NULL,
    [DELIVERY_BLK_HDR_DESC] nvarchar(255)  NOT NULL,
    [BILLING_BLOCK_CD] nvarchar(50)  NULL,
    [BILLING_BLOCK_DESC] nvarchar(255)  NULL,
    [NLHD_STATUS] nvarchar(50)  NOT NULL,
    [COMMIT_DATE] datetime  NULL,
    [DELTA_LOAD_DATE_BUCKET] nvarchar(50)  NULL,
    [SNI_CLOSURE_DAYS_DELTA] int  NOT NULL,
    [DB_CLOSURE_DAYS_DELTA] int  NOT NULL,
    [ORDER_DT] datetime  NULL,
    [CUSTOMER_REQ_GI_DATE] datetime  NULL,
    [EARLY_DEL_ACCEPTABLE] nvarchar(50)  NULL,
    [DELIVERY_BLOCK_CUT_OFF_DATE] datetime  NOT NULL,
    [EXPECTED_RELEASE_DATE] datetime  NULL,
    [REASON_CODE] nvarchar(255)  NULL,
    [LATEST_COMMENT] nvarchar(max)  NULL,
    [LATEST_COMMENT_BY] nvarchar(50)  NULL,
    [LATEST_COMMENT_DATE_TIME] datetime  NULL,
    [OVERALL_INSTALLATION_STATUS] nvarchar(50)  NULL,
    [SOLD_TO_PARTY] nvarchar(50)  NULL,
    [ZU_ACCOUNT_ID] nvarchar(max)  NULL,
    [QUOTA_SF] nvarchar(50)  NULL
);
GO

-- Creating table 'Tbl_Order_Action_Owner'
CREATE TABLE [dbo].[Tbl_Order_Action_Owner] (
    [SALES_ORDERNO] float  NOT NULL,
    [Default_Owner] nvarchar(50)  NOT NULL,
    [Current_Owner] nvarchar(50)  NOT NULL,
    [Current_Owner_SAPName] nvarchar(50)  NULL,
    [Ownership_Type] nvarchar(50)  NULL,
    [Ownership_From] datetime  NULL,
    [Ownership_To] datetime  NULL,
    [Assigned_By] nvarchar(50)  NOT NULL,
    [Assigned_On] datetime  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'Tbl_Archives'
ALTER TABLE [dbo].[Tbl_Archives]
ADD CONSTRAINT [PK_Tbl_Archives]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Followupid] in table 'Tbl_Followups'
ALTER TABLE [dbo].[Tbl_Followups]
ADD CONSTRAINT [PK_Tbl_Followups]
    PRIMARY KEY CLUSTERED ([Followupid] ASC);
GO

-- Creating primary key on [id] in table 'Tbl_Followup_Summary'
ALTER TABLE [dbo].[Tbl_Followup_Summary]
ADD CONSTRAINT [PK_Tbl_Followup_Summary]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [ReportName], [OrderOwner], [Region] in table 'Tbl_Daily_Control_Reports_Summary'
ALTER TABLE [dbo].[Tbl_Daily_Control_Reports_Summary]
ADD CONSTRAINT [PK_Tbl_Daily_Control_Reports_Summary]
    PRIMARY KEY CLUSTERED ([ReportName], [OrderOwner], [Region] ASC);
GO

-- Creating primary key on [Region] in table 'Tbl_STR_Summary'
ALTER TABLE [dbo].[Tbl_STR_Summary]
ADD CONSTRAINT [PK_Tbl_STR_Summary]
    PRIMARY KEY CLUSTERED ([Region] ASC);
GO

-- Creating primary key on [SALES_ORDERNO] in table 'VW_DB_Speed_To_Revenue'
ALTER TABLE [dbo].[VW_DB_Speed_To_Revenue]
ADD CONSTRAINT [PK_VW_DB_Speed_To_Revenue]
    PRIMARY KEY CLUSTERED ([SALES_ORDERNO] ASC);
GO

-- Creating primary key on [Report], [Sales_Ord], [Material], [Comment_Date], [Comment] in table 'Tbl_Order_comments'
ALTER TABLE [dbo].[Tbl_Order_comments]
ADD CONSTRAINT [PK_Tbl_Order_comments]
    PRIMARY KEY CLUSTERED ([Report], [Sales_Ord], [Material], [Comment_Date], [Comment] ASC);
GO

-- Creating primary key on [ReportName] in table 'Tbl_Review_Reports'
ALTER TABLE [dbo].[Tbl_Review_Reports]
ADD CONSTRAINT [PK_Tbl_Review_Reports]
    PRIMARY KEY CLUSTERED ([ReportName] ASC);
GO

-- Creating primary key on [SnapshotDate] in table 'Tbl_Backlog_Summary'
ALTER TABLE [dbo].[Tbl_Backlog_Summary]
ADD CONSTRAINT [PK_Tbl_Backlog_Summary]
    PRIMARY KEY CLUSTERED ([SnapshotDate] ASC);
GO

-- Creating primary key on [BUSINESS_GROUP], [REGION], [DIVISION], [SALES_ORG], [PRODUCT_LINE] in table 'Dim_Business_Master'
ALTER TABLE [dbo].[Dim_Business_Master]
ADD CONSTRAINT [PK_Dim_Business_Master]
    PRIMARY KEY CLUSTERED ([BUSINESS_GROUP], [REGION], [DIVISION], [SALES_ORG], [PRODUCT_LINE] ASC);
GO

-- Creating primary key on [BILLING_BLOCK_CD] in table 'Dim_Billing_Blocks'
ALTER TABLE [dbo].[Dim_Billing_Blocks]
ADD CONSTRAINT [PK_Dim_Billing_Blocks]
    PRIMARY KEY CLUSTERED ([BILLING_BLOCK_CD] ASC);
GO

-- Creating primary key on [DELIVERY_BLK_HDR_CD] in table 'Dim_Delivery_Blocks'
ALTER TABLE [dbo].[Dim_Delivery_Blocks]
ADD CONSTRAINT [PK_Dim_Delivery_Blocks]
    PRIMARY KEY CLUSTERED ([DELIVERY_BLK_HDR_CD] ASC);
GO

-- Creating primary key on [REGION], [SHIP_TO_COUNTRY], [SOLD_TO_COUNTRY], [SOLD_TO_PARTY], [SHIP_TO_PARTY] in table 'Dim_Customers'
ALTER TABLE [dbo].[Dim_Customers]
ADD CONSTRAINT [PK_Dim_Customers]
    PRIMARY KEY CLUSTERED ([REGION], [SHIP_TO_COUNTRY], [SOLD_TO_COUNTRY], [SOLD_TO_PARTY], [SHIP_TO_PARTY] ASC);
GO

-- Creating primary key on [REGION], [QUOTA_SF], [FE_FE_DESC] in table 'Dim_Sales_Force'
ALTER TABLE [dbo].[Dim_Sales_Force]
ADD CONSTRAINT [PK_Dim_Sales_Force]
    PRIMARY KEY CLUSTERED ([REGION], [QUOTA_SF], [FE_FE_DESC] ASC);
GO

-- Creating primary key on [SALES_ORDERNO], [DIVISION], [BAcklog] in table 'Tbl_SNI_Release_Projection'
ALTER TABLE [dbo].[Tbl_SNI_Release_Projection]
ADD CONSTRAINT [PK_Tbl_SNI_Release_Projection]
    PRIMARY KEY CLUSTERED ([SALES_ORDERNO], [DIVISION], [BAcklog] ASC);
GO

-- Creating primary key on [SALES_DOC], [ITEM] in table 'Tbl_OSBR_NOTIFICATIONS'
ALTER TABLE [dbo].[Tbl_OSBR_NOTIFICATIONS]
ADD CONSTRAINT [PK_Tbl_OSBR_NOTIFICATIONS]
    PRIMARY KEY CLUSTERED ([SALES_DOC], [ITEM] ASC);
GO

-- Creating primary key on [USER_ID] in table 'TBL_USERS'
ALTER TABLE [dbo].[TBL_USERS]
ADD CONSTRAINT [PK_TBL_USERS]
    PRIMARY KEY CLUSTERED ([USER_ID] ASC);
GO

-- Creating primary key on [USER_ID] in table 'VW_USERS'
ALTER TABLE [dbo].[VW_USERS]
ADD CONSTRAINT [PK_VW_USERS]
    PRIMARY KEY CLUSTERED ([USER_ID] ASC);
GO

-- Creating primary key on [Found_in_Area] in table 'Tbl_Order_Search'
ALTER TABLE [dbo].[Tbl_Order_Search]
ADD CONSTRAINT [PK_Tbl_Order_Search]
    PRIMARY KEY CLUSTERED ([Found_in_Area] ASC);
GO

-- Creating primary key on [SALES_ORDERNO] in table 'Tbl_Order_Block_Details'
ALTER TABLE [dbo].[Tbl_Order_Block_Details]
ADD CONSTRAINT [PK_Tbl_Order_Block_Details]
    PRIMARY KEY CLUSTERED ([SALES_ORDERNO] ASC);
GO

-- Creating primary key on [SALES_ORDERNO] in table 'Tbl_Order_Delivery_Info'
ALTER TABLE [dbo].[Tbl_Order_Delivery_Info]
ADD CONSTRAINT [PK_Tbl_Order_Delivery_Info]
    PRIMARY KEY CLUSTERED ([SALES_ORDERNO] ASC);
GO

-- Creating primary key on [SALES_ORDERNO] in table 'Tbl_Order_Header_Details'
ALTER TABLE [dbo].[Tbl_Order_Header_Details]
ADD CONSTRAINT [PK_Tbl_Order_Header_Details]
    PRIMARY KEY CLUSTERED ([SALES_ORDERNO] ASC);
GO

-- Creating primary key on [SALES_ORDERNO], [LINE_ITEM] in table 'Tbl_Order_Items'
ALTER TABLE [dbo].[Tbl_Order_Items]
ADD CONSTRAINT [PK_Tbl_Order_Items]
    PRIMARY KEY CLUSTERED ([SALES_ORDERNO], [LINE_ITEM] ASC);
GO

-- Creating primary key on [SALES_ORDERNO] in table 'Tbl_Order_Partner_Details'
ALTER TABLE [dbo].[Tbl_Order_Partner_Details]
ADD CONSTRAINT [PK_Tbl_Order_Partner_Details]
    PRIMARY KEY CLUSTERED ([SALES_ORDERNO] ASC);
GO

-- Creating primary key on [SALES_ORDERNO], [TRIO_LOAD_DATE], [SNI_CLOSURE_STATUS], [DB_CLOSURE_STATUS], [SNI_AGING_BUCKET], [DB_AGING_BUCKET], [DELTA_LOAD_DATE_BUCKET], [SNI_AGE] in table 'VW_Orders_Info'
ALTER TABLE [dbo].[VW_Orders_Info]
ADD CONSTRAINT [PK_VW_Orders_Info]
    PRIMARY KEY CLUSTERED ([SALES_ORDERNO], [TRIO_LOAD_DATE], [SNI_CLOSURE_STATUS], [DB_CLOSURE_STATUS], [SNI_AGING_BUCKET], [DB_AGING_BUCKET], [DELTA_LOAD_DATE_BUCKET], [SNI_AGE] ASC);
GO

-- Creating primary key on [Id] in table 'Tbl_Country_Sorg_Orig'
ALTER TABLE [dbo].[Tbl_Country_Sorg_Orig]
ADD CONSTRAINT [PK_Tbl_Country_Sorg_Orig]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [TEAM_ID] in table 'TBL_TEAM_STRUCTURE'
ALTER TABLE [dbo].[TBL_TEAM_STRUCTURE]
ADD CONSTRAINT [PK_TBL_TEAM_STRUCTURE]
    PRIMARY KEY CLUSTERED ([TEAM_ID] ASC);
GO

-- Creating primary key on [Bucket_Rank], [Bucket] in table 'Tbl_WW_Blocked_Orders_Summary'
ALTER TABLE [dbo].[Tbl_WW_Blocked_Orders_Summary]
ADD CONSTRAINT [PK_Tbl_WW_Blocked_Orders_Summary]
    PRIMARY KEY CLUSTERED ([Bucket_Rank], [Bucket] ASC);
GO

-- Creating primary key on [ROLE_ID] in table 'TBL_ROLE'
ALTER TABLE [dbo].[TBL_ROLE]
ADD CONSTRAINT [PK_TBL_ROLE]
    PRIMARY KEY CLUSTERED ([ROLE_ID] ASC);
GO

-- Creating primary key on [Order_Owner] in table 'Tbl_Archival_Summary'
ALTER TABLE [dbo].[Tbl_Archival_Summary]
ADD CONSTRAINT [PK_Tbl_Archival_Summary]
    PRIMARY KEY CLUSTERED ([Order_Owner] ASC);
GO

-- Creating primary key on [REGION], [ORDER_CREATED_BY] in table 'Tbl_Unmapped_Orders_By_Region_Function'
ALTER TABLE [dbo].[Tbl_Unmapped_Orders_By_Region_Function]
ADD CONSTRAINT [PK_Tbl_Unmapped_Orders_By_Region_Function]
    PRIMARY KEY CLUSTERED ([REGION], [ORDER_CREATED_BY] ASC);
GO

-- Creating primary key on [REGION], [USERGROUP] in table 'Tbl_Unmapped_Users'
ALTER TABLE [dbo].[Tbl_Unmapped_Users]
ADD CONSTRAINT [PK_Tbl_Unmapped_Users]
    PRIMARY KEY CLUSTERED ([REGION], [USERGROUP] ASC);
GO

-- Creating primary key on [HistoryID] in table 'Tbl_Followups_History'
ALTER TABLE [dbo].[Tbl_Followups_History]
ADD CONSTRAINT [PK_Tbl_Followups_History]
    PRIMARY KEY CLUSTERED ([HistoryID] ASC);
GO

-- Creating primary key on [SALES_ORDERNO], [ORDER_AGE], [CRDD_Age], [BACKLOG_AMT] in table 'TBL_ORDER_COMMENT_VIEW'
ALTER TABLE [dbo].[TBL_ORDER_COMMENT_VIEW]
ADD CONSTRAINT [PK_TBL_ORDER_COMMENT_VIEW]
    PRIMARY KEY CLUSTERED ([SALES_ORDERNO], [ORDER_AGE], [CRDD_Age], [BACKLOG_AMT] ASC);
GO

-- Creating primary key on [Report], [Sales_Ord], [Material], [Comment_Date], [Commented_By] in table 'Tbl_History_Comments'
ALTER TABLE [dbo].[Tbl_History_Comments]
ADD CONSTRAINT [PK_Tbl_History_Comments]
    PRIMARY KEY CLUSTERED ([Report], [Sales_Ord], [Material], [Comment_Date], [Commented_By] ASC);
GO

-- Creating primary key on [Report] in table 'VW_BCR_SignOff_Summary'
ALTER TABLE [dbo].[VW_BCR_SignOff_Summary]
ADD CONSTRAINT [PK_VW_BCR_SignOff_Summary]
    PRIMARY KEY CLUSTERED ([Report] ASC);
GO

-- Creating primary key on [SALES_ORDERNO], [DELV_BLK_TYPE], [SNI_CLOSURE_STATUS], [DB_CLOSURE_STATUS], [DELIVERY_BLK_HDR_CD], [DELIVERY_BLK_HDR_DESC], [NLHD_STATUS], [SNI_CLOSURE_DAYS_DELTA], [DB_CLOSURE_DAYS_DELTA], [DELIVERY_BLOCK_CUT_OFF_DATE], [BUSINESS_GROUP], [BACKLOG_AMT] in table 'SNI_Excel_View'
ALTER TABLE [dbo].[SNI_Excel_View]
ADD CONSTRAINT [PK_SNI_Excel_View]
    PRIMARY KEY CLUSTERED ([SALES_ORDERNO], [DELV_BLK_TYPE], [SNI_CLOSURE_STATUS], [DB_CLOSURE_STATUS], [DELIVERY_BLK_HDR_CD], [DELIVERY_BLK_HDR_DESC], [NLHD_STATUS], [SNI_CLOSURE_DAYS_DELTA], [DB_CLOSURE_DAYS_DELTA], [DELIVERY_BLOCK_CUT_OFF_DATE], [BUSINESS_GROUP], [BACKLOG_AMT] ASC);
GO

-- Creating primary key on [SALES_ORDERNO], [Default_Owner], [Current_Owner] in table 'Tbl_Order_Action_Owner'
ALTER TABLE [dbo].[Tbl_Order_Action_Owner]
ADD CONSTRAINT [PK_Tbl_Order_Action_Owner]
    PRIMARY KEY CLUSTERED ([SALES_ORDERNO], [Default_Owner], [Current_Owner] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------