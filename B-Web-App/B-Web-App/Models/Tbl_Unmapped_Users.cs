namespace B_Web_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Unmapped_Users
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string REGION { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string USERGROUP { get; set; }

        public int? Order_Count { get; set; }

        public double? Total_NV { get; set; }

        public int? TotalUser { get; set; }
    }
}
