namespace B_Web_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TBL_ROLE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ROLE_ID { get; set; }

        [StringLength(50)]
        public string ROLE_DESC { get; set; }
    }
}
