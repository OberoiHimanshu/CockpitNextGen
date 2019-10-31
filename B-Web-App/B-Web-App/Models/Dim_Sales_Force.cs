namespace B_Web_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Dim_Sales_Force
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string REGION { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string QUOTA_SF { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string FE_FE_DESC { get; set; }

        [StringLength(100)]
        public string SRTATTR_AREA_MGR_NME { get; set; }

        [StringLength(100)]
        public string SRTATTR_DISTRICT_MGR_NME { get; set; }
    }
}
