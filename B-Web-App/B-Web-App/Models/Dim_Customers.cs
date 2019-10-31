namespace B_Web_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Dim_Customers
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string REGION { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string SOLD_TO_COUNTRY { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string SHIP_TO_COUNTRY { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string SOLD_TO_PARTY { get; set; }

        [StringLength(255)]
        public string SOLD_TO_PARTY_NAME { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string SHIP_TO_PARTY { get; set; }

        [StringLength(255)]
        public string SHIP_TO_PARTY_NAME { get; set; }

        [StringLength(255)]
        public string ZU_ACCOUNT_NAME { get; set; }

        [StringLength(100)]
        public string ZU_COUNTRY { get; set; }

        [StringLength(100)]
        public string PAYMENT_TYPE { get; set; }
    }
}
