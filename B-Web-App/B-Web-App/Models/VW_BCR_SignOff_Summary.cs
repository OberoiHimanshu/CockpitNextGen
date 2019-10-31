namespace B_Web_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VW_BCR_SignOff_Summary 
    {
        [StringLength(50)]
        public string Region { get; set; }

        [Key]
        [StringLength(50)]
        public string Report { get; set; }

        public int? Commented_Sign_Off { get; set; }

        public DateTime? snapshotdateFrom { get; set; }

        public DateTime? snapshotdateTo { get; set; }
    }
}
