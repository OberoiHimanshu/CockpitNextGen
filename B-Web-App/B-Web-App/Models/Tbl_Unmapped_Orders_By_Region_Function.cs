namespace B_Web_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Unmapped_Orders_By_Region_Function
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string REGION { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string ORDER_CREATED_BY { get; set; }

        public int? Orders { get; set; }

        public double? Total_NV { get; set; }
    }
}
