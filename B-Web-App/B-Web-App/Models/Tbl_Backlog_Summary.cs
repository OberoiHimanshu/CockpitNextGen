namespace B_Web_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Backlog_Summary
    {
        [Key]
        public DateTime SnapshotDate { get; set; }

        [StringLength(50)]
        public string SAP_User { get; set; }

        [StringLength(20)]
        public string Region { get; set; }

        [StringLength(10)]
        public string Sorg { get; set; }

        public double? Total_SNI { get; set; }

        public int? Total_SNI_Count { get; set; }

        public double? Total_SNI_Aged { get; set; }

        public int? Total_SNI_Aged_Count { get; set; }

        public double? Total_SNI_Expected_Release_Today { get; set; }

        public int? Total_SNI_Expected_Release_Today_Count { get; set; }

        public double? Total_SNI_Expected_Released_Passed { get; set; }

        public int? Total_SNI_Expected_Released_Passed_Count { get; set; }

        public double? Total_SNI_No_Expected_Release_Date { get; set; }

        public int? Total_SNI_No_Expected_Release_Date_Count { get; set; }

        public double? Total_SNI_MF_Invoicing_Errors { get; set; }

        public int? Total_SNI_MF_Invoicing_Errors_Count { get; set; }

        public double? Total_DB { get; set; }

        public int? Total_DB_Count { get; set; }

        public double? Total_DB_greater_90_Days { get; set; }

        public int? Total_DB_greater_90_Days_Count { get; set; }

        public double? Total_DB_Expected_Released_Passed { get; set; }

        public int? Total_DB_Expected_Released_Passed_Count { get; set; }

        public double? Total_DB_Overdue { get; set; }

        public int? Total_DB_Overdue_Count { get; set; }

        public double? Total_DB_Overdue_14_Days { get; set; }

        public int? Total_DB_Overdue_14_Days_Count { get; set; }

        public double? Total_DB_SppedtoRevenue { get; set; }

        public int? Total_DB_SppedtoRevenue_Count { get; set; }

        public double? Total_Unblocked_Orders { get; set; }

        public int? Total_Unblocked_Orders_Count { get; set; }

        public double? Total_Unblocked_Overdue { get; set; }

        public int? Total_Unblocked_Overdue_Count { get; set; }

        public double? Total_OSBR_Notification { get; set; }

        public int? Total_OSBR_Notification_Count { get; set; }
    }
}
