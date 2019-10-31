namespace B_Web_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Dim_Billing_Blocks
    {
        [Key]
        [StringLength(50)]
        public string BILLING_BLOCK_CD { get; set; }

        [StringLength(255)]
        public string BILLING_BLOCK_DESC { get; set; }
    }
}
