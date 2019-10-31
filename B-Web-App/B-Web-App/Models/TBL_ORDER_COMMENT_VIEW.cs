namespace B_Web_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TBL_ORDER_COMMENT_VIEW
    {
        [Key]
        [Column(Order = 0)]
        public double SALES_ORDERNO { get; set; }

        [StringLength(100)]
        public string CUSTOMER_PO_NO { get; set; }

        [StringLength(10)]
        public string SALES_ORG { get; set; }

        [StringLength(100)]
        public string ORDER_OWNER { get; set; }

        [Key]
        [Column(Order = 1)]
        public double BACKLOG_AMT { get; set; }

        [StringLength(100)]
        public string SOLD_TO_PARTY_NAME { get; set; }

        public DateTime? EXPECTED_RELEASE_DATE { get; set; }

        public DateTime? REVIEW_DATE { get; set; }

        [StringLength(100)]
        public string REASON_CODE { get; set; }

        public string LATEST_COMMENT { get; set; }

        public DateTime? COMMENTED_DATE { get; set; }

        [StringLength(10)]
        public string COMMENTED_BY { get; set; }

        [StringLength(10)]
        public string REGION { get; set; }

        public DateTime? ORDER_DT { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ORDER_AGE { get; set; }

        public DateTime? APPROVED_TO_DATE { get; set; }

        public DateTime? SNAPSHOT_DATE { get; set; }

        public DateTime? CUSTOMER_REQ_GI_DATE { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CRDD_Age { get; set; }

        [StringLength(100)]
        public string FE_FE_DESC { get; set; }

        [StringLength(100)]
        public string ReportName { get; set; }
    }
}
