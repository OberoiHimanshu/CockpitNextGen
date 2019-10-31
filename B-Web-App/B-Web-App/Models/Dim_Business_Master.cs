namespace B_Web_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Dim_Business_Master
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string BUSINESS_GROUP { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string REGION { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string DIVISION { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string SALES_ORG { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string PRODUCT_LINE { get; set; }
    }
}
