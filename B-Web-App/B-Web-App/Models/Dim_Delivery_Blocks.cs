namespace B_Web_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Dim_Delivery_Blocks
    {
        [Key]
        [StringLength(50)]
        public string DELIVERY_BLK_HDR_CD { get; set; }

        [StringLength(255)]
        public string DELIVERY_BLK_HDR_DESC { get; set; }
    }
}
