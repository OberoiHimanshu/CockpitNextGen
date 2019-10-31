namespace B_Web_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Followup_Summary
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public int? TotalFollowups { get; set; }

        public int? PassedDuedate { get; set; }

        public int? DueToday { get; set; }

        public int? ReassignedtoYou { get; set; }

        public int? SystemGenerated { get; set; }
    }
}
