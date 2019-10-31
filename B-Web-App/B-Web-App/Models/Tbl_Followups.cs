namespace B_Web_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Followups
    {
        [Key]
        public int Followupid { get; set; }

        public double? Sales_Order { get; set; }

        [StringLength(200)]
        public string CustomerName { get; set; }

        public string Description { get; set; }

        [StringLength(200)]
        public string Owner { get; set; }

        public DateTime? DueDate { get; set; }

        [StringLength(200)]
        public string BacklogStatus { get; set; }

        [Required]
        public string Comment { get; set; }

        [Required]
        [StringLength(200)]
        public string Status { get; set; }

        [StringLength(50)]
        public string Created_By { get; set; }

        public DateTime Created_On { get; set; }

        [StringLength(200)]
        public string Re_Assigned_To { get; set; }

        [StringLength(50)]
        public string Priority { get; set; }

        [StringLength(50)]
        public string Modified_By { get; set; }

        public DateTime Modified_On { get; set; }

        [StringLength(50)]
        public string Owner_Full_Name { get; set; }
    }
}
