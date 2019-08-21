//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BAL_Service
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tbl_BBB_ZTRD_Staging
    {
        public string REGION { get; set; }
        public string DIVISION { get; set; }
        public string QUOTA_SF { get; set; }
        public double SALES_ORDERNO { get; set; }
        public string PRODUCT_LINE { get; set; }
        public Nullable<int> LINE_ITEM { get; set; }
        public string MATERIAL_NO { get; set; }
        public string MATERIAL_DESC { get; set; }
        public string PLANT { get; set; }
        public string PRIMARY_PRODUCT { get; set; }
        public string CUSTOMER_PO_NO { get; set; }
        public string SALES_ORG { get; set; }
        public string BUSINESS_GROUP { get; set; }
        public string ORDER_TYPE { get; set; }
        public string ORDER_CREATED_BY { get; set; }
        public string ORDER_OWNER { get; set; }
        public string BACKLOG_STATUS { get; set; }
        public string DELV_BLK_TYPE { get; set; }
        public Nullable<int> ORDER_AGE { get; set; }
        public string Aging_Bucket { get; set; }
        public Nullable<int> DB_DAYS_DELTA { get; set; }
        public string SNI_AGING_BUCKET { get; set; }
        public string DOLLAR_BUCKET { get; set; }
        public string DB_AGING_BUCKET { get; set; }
        public string SNI_CLOSURE_STATUS { get; set; }
        public string DB_CLOSURE_STATUS { get; set; }
        public Nullable<double> BACKLOG_AMT { get; set; }
        public string COMPLETE_DELIVERY_HEADER { get; set; }
        public string PL_DIVISION_DESC { get; set; }
        public Nullable<System.DateTime> INVOICE_DT { get; set; }
        public string DB_CYCLE_TIME_TGT_DAYS { get; set; }
        public string TRANSIT_DAYS { get; set; }
        public string SNI_CYCLE_TIME_TGT_DAYS { get; set; }
        public string CYCLE_TIME_TYPE { get; set; }
        public string SOLD_TO_PARTY_NAME { get; set; }
        public string SOLD_TO_PARTY { get; set; }
        public string SOLD_TO_COUNTRY { get; set; }
        public string SHIP_TO_PARTY { get; set; }
        public string SHIP_TO_PARTY_NAME { get; set; }
        public string SHIP_TO_COUNTRY { get; set; }
        public string ZU_ACCOUNT_NAME { get; set; }
        public string ZU_COUNTRY { get; set; }
        public string EN_ACCOUNT_NAME { get; set; }
        public string EN_COUNTRY { get; set; }
        public string FE_FE_DESC { get; set; }
        public string SRTATTR_AREA_MGR_NME { get; set; }
        public string SRTATTR_DISTRICT_MGR_NME { get; set; }
        public string PAYMENT_TERMS { get; set; }
        public string PAYMENT_TERMS_DESC { get; set; }
        public string PAYMENT_TYPE { get; set; }
        public string SHIPPING_POINT { get; set; }
        public string DELIVERY_BLK_HDR_CD { get; set; }
        public string DELIVERY_BLK_HDR_DESC { get; set; }
        public string DELV_BLK_LINE_CD { get; set; }
        public string DELV_BLK_LINE_DESC { get; set; }
        public string BILLING_BLOCK_CD { get; set; }
        public string BILLING_BLOCK_DESC { get; set; }
        public string ITEM_BILLING_BLK_CD { get; set; }
        public string ITEM_BILLING_BLK_DESC { get; set; }
        public string NLHD_STATUS { get; set; }
        public Nullable<System.DateTime> COMMIT_DATE { get; set; }
        public Nullable<System.DateTime> TRIO_LOAD_DATE { get; set; }
        public string DELTA_LOAD_DATE_BUCKET { get; set; }
        public Nullable<int> SNI_CLOSURE_DAYS_DELTA { get; set; }
        public Nullable<int> DB_CLOSURE_DAYS_DELTA { get; set; }
        public Nullable<System.DateTime> LOAD_REFRESH_TS { get; set; }
        public Nullable<System.DateTime> ORDER_DT { get; set; }
        public Nullable<System.DateTime> CUSTOMER_REQ_GI_DATE { get; set; }
        public string EARLY_DEL_ACCEPTABLE { get; set; }
        public string DELV_PRIO_CD_HDR { get; set; }
        public string DELV_PRIO_DESC_HDR { get; set; }
        public string SHIP_CONDTN_DESC { get; set; }
        public Nullable<System.DateTime> DELIVERY_BLOCK_CUT_OFF_DATE { get; set; }
        public Nullable<System.DateTime> SHIPMENT_CUT_OFF_DATE { get; set; }
        public string INCOTERMS { get; set; }
        public Nullable<System.DateTime> DELV_BLK_LAST_APPLD_DT { get; set; }
        public Nullable<System.DateTime> DELV_BLK_REL_DT { get; set; }
        public Nullable<int> DELV_BLK_COUNT { get; set; }
        public Nullable<int> SAP_DELV_BLK_DAYS { get; set; }
        public string NEXT_ACTION_OWNER { get; set; }
        public Nullable<System.DateTime> EXPECTED_RELEASE_DATE { get; set; }
        public string REASON_CODE { get; set; }
        public string LATEST_COMMENT { get; set; }
        public string LATEST_COMMENT_BY { get; set; }
        public Nullable<System.DateTime> LATEST_COMMENT_DATE_TIME { get; set; }
        public string INCOMPLETIONLOGHDR1 { get; set; }
        public string HEADER_ORDER_FULFILLMENT_NOTES { get; set; }
        public string HEADER_AUTHORIZATION_TEXT { get; set; }
        public string ITEM_ASSIGNED_ENGINEER_TEXT { get; set; }
        public string INCOMPLETIONLOGLINE1 { get; set; }
        public string INCOMPLETIONLOGHDR2 { get; set; }
        public string INCOMPLETIONLOGLINE2 { get; set; }
        public string INCOMPLETIONLOGLINE3 { get; set; }
        public Nullable<System.DateTime> ACTUAL_GI_DATE { get; set; }
        public string DLVY_DT_CHANGE_REASON { get; set; }
        public string SHIP_DT_CHANGE_REASON { get; set; }
        public string REPORT_QUOTE_VALIDITY { get; set; }
        public string REPORT_3PP { get; set; }
        public string REPORT_RESELLER_DISCOUNT { get; set; }
        public string REPORT_MANNUAL_PRICING { get; set; }
        public string REPORT_CHECK_SUBSEQUENT { get; set; }
        public string REPORT_CHANGE_PRICE_LIST { get; set; }
        public string REPORT_REPRICING_EXCEPTION { get; set; }
        public string REPORT_NOSCHEDULELINES { get; set; }
        public string REPORT_ORDERGREATER5MONTHS { get; set; }
        public string REPORT_VERBALORDERS { get; set; }
        public string REPORT_BILLING_ERRORS { get; set; }
        public string REPORT_CREDIT_CARD_FAILURES { get; set; }
        public string REPORT_CRDDGREATER6MONTHS { get; set; }
        public string REPORT_INCOMPLETEORDERS { get; set; }
        public string REPORT_NONSTANDARDDISCOUNT { get; set; }
        public string REPORT_REJECTED_ORDERS { get; set; }
        public string REPORT_RETURNED_BLOCKED { get; set; }
        public string REPORT_DELIVERY_BLOCK { get; set; }
        public string REPORT_FREE_OF_CHARGE { get; set; }
        public string REPORT_BILLING_BLOCK { get; set; }
        public string REPORT_SERVICE_BACKLOG { get; set; }
        public string SAP_ORDER_STATUS { get; set; }
        public string SALES_DOCUMENT_ITEM_CATEGORY { get; set; }
        public string REPORT_DB_WITH_CRDD_2DAYS { get; set; }
        public string REPORT_NO_DB_WITH_CRDD_2DAYS { get; set; }
        public Nullable<System.DateTime> BACKLOG_DATE { get; set; }
        public string PO_TYPE { get; set; }
        public string BACKLOG_ACTIVITY_ENHANCED { get; set; }
        public string ZU_ACCOUNT_ID { get; set; }
    }
}