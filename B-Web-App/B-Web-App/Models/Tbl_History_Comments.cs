namespace B_Web_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_History_Comments
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string Report { get; set; }

        [Key]
        [Column(Order = 1)]
        public double Sales_Ord { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(30)]
        public string Material { get; set; }

        [StringLength(50)]
        public string OrderOwner { get; set; }

        [StringLength(50)]
        public string Region { get; set; }

        [StringLength(100)]
        public string Reason_Code { get; set; }

        public string NextAction { get; set; }

        public DateTime? Cleardate { get; set; }

        public string Comment { get; set; }

        public DateTime? ReviewDate { get; set; }

        [StringLength(3)]
        public string SignOff { get; set; }

        public DateTime? snapshotdate { get; set; }

        [Key]
        [Column(Order = 3)]
        public DateTime Comment_Date { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string Commented_By { get; set; }

        [StringLength(50)]
        public string SignOff_By { get; set; }

        public DateTime? SignOff_Date { get; set; }

        public DateTime? Approved_To_Date { get; set; }

        public int? Line_Item { get; set; }

        [StringLength(10)]
        public string Sales_Org { get; set; }

        public DateTime? Order_Date { get; set; }

        public double? Net_Value { get; set; }

        [StringLength(50)]
        public string Sold_To_Customer_Name { get; set; }
    }
}
