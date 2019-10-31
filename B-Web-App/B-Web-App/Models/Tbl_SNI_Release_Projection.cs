namespace B_Web_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_SNI_Release_Projection
    {
        [Key]
        [Column(Order = 0)]
        public double SALES_ORDERNO { get; set; }

        [StringLength(50)]
        public string BUSINESS_SCENARIO { get; set; }

        [StringLength(255)]
        public string BACKLOG_STATUS { get; set; }

        [StringLength(255)]
        public string ORDER_CREATED_BY { get; set; }

        [StringLength(255)]
        public string SALES_ORG { get; set; }

        [Required]
        [StringLength(255)]
        public string REGION { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(30)]
        public string DIVISION { get; set; }

        [StringLength(255)]
        public string BILLING_BLOCK_CD { get; set; }

        [StringLength(255)]
        public string BILLING_BLOCK_DESC { get; set; }

        [StringLength(255)]
        public string DELIVERY_BLK_HDR_DESC { get; set; }

        public DateTime? Frcst_EstLoadDate { get; set; }

        public DateTime? Feedback { get; set; }

        [StringLength(30)]
        public string Feedback_MonthName { get; set; }

        public int? BB_CycleTimeTarget { get; set; }

        public DateTime? Feedback_Calc_Est_Invoice_dt { get; set; }

        [StringLength(30)]
        public string Feedback_Calc_Est_Invoice_dt_Month { get; set; }

        public DateTime? Est_InvoiceDate { get; set; }

        [StringLength(30)]
        public string Est_MonthName { get; set; }

        [Key]
        [Column(Order = 2)]
        public double BAcklog { get; set; }

        [StringLength(255)]
        public string PRIMARY_PRODUCT { get; set; }

        [StringLength(150)]
        public string TEAM_NAME { get; set; }
    }
}
