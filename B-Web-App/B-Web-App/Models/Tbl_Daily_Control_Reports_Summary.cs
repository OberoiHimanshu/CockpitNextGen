namespace B_Web_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Daily_Control_Reports_Summary
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string ReportName { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string OrderOwner { get; set; }

        public int? Pending_Comments { get; set; }

        public int? Pending_Review { get; set; }

        public int? Pending_Sign_Off { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string Region { get; set; }
    }
}
