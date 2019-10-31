namespace B_Web_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Review_Reports
    {
        [Key]
        [StringLength(50)]
        public string ReportName { get; set; }

        [StringLength(30)]
        public string Status { get; set; }

        [StringLength(10)]
        public string Frequency { get; set; }

        [StringLength(50)]
        public string CustomFilter { get; set; }

        [StringLength(100)]
        public string SAP_T_Code { get; set; }

        public string Description1 { get; set; }

        public string Description2 { get; set; }
    }
}
