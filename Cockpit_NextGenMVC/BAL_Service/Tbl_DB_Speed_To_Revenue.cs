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
    
    public partial class Tbl_DB_Speed_To_Revenue
    {
        public string BACKLOG_STATUS { get; set; }
        public Nullable<int> PRIORITY { get; set; }
        public Nullable<double> SALES_ORDERNO { get; set; }
        public string ORDER_OWNER { get; set; }
        public string PL { get; set; }
        public Nullable<int> QUOTA_SF { get; set; }
        public string BUSINESS_GROUP { get; set; }
        public string DIVISION { get; set; }
        public string REGION { get; set; }
        public string SALES_ORG { get; set; }
        public string FE_FE_DESC { get; set; }
        public string SRTATTR_AREA_MGR_NME { get; set; }
        public string SRTATTR_DISTRICT_MGR_NME { get; set; }
        public string PRIMARY_PRODUCT { get; set; }
        public string SOLD_TO_PARTY_NAME { get; set; }
        public string SHIP_TO_COUNTRY { get; set; }
        public string BILLING_BLOCK_CD { get; set; }
        public string BILLING_BLOCK_DESC { get; set; }
        public string DELIVERY_BLK_HDR_CD { get; set; }
        public string DELIVERY_BLK_HDR_DESC { get; set; }
        public Nullable<System.DateTime> ReqDlyDate { get; set; }
        public string EARLY_DEL_ACCEPTABLE { get; set; }
        public Nullable<int> ORDER_AGE { get; set; }
        public string Aging_Bucket { get; set; }
        public Nullable<System.DateTime> DELIVERY_BLOCK_CUT_OFF_DATE { get; set; }
        public Nullable<System.DateTime> CUSTOMER_REQ_GI_DATE { get; set; }
        public Nullable<System.DateTime> SHIPMENT_CUT_OFF_DATE { get; set; }
        public Nullable<double> BACKLOG_AMT { get; set; }
        public string INVOICING_STATUS { get; set; }
        public string BUSINESS_SCENARIO { get; set; }
        public Nullable<int> DB_CLOSURE_DAYS_DELTA { get; set; }
        public string DB_CLOSURE_STATUS { get; set; }
        public Nullable<int> ClosuredaysDelta { get; set; }
        public Nullable<System.DateTime> RCD { get; set; }
        public string Dollar_Bucket { get; set; }
        public System.DateTime SnapshotDate { get; set; }
        public string QUARTER { get; set; }
        public string ACTION_OWNER { get; set; }
        public Nullable<System.DateTime> EXPECTED_RELEASE_DATE { get; set; }
        public string REASON_CODE { get; set; }
        public string LATEST_COMMENT { get; set; }
    }
}
