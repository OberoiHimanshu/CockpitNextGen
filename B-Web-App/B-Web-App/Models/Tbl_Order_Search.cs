namespace B_Web_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Order_Search
    {
        public double? Sales_Order { get; set; }

        [Key]
        [StringLength(50)]
        public string Found_in_Area { get; set; }

        [StringLength(10)]
        public string Area_UI_Path { get; set; }

        [StringLength(100)]
        public string Area_UI_Bucket { get; set; }

        [StringLength(10)]
        public string Bucket_Security_On { get; set; }

        [StringLength(50)]
        public string Current_Owner { get; set; }
    }
}
