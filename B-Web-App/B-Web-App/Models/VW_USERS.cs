namespace B_Web_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VW_USERS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int USER_ID { get; set; }

        [StringLength(255)]
        public string SAP_User_Name { get; set; }

        [StringLength(50)]
        public string ROLE_DESC { get; set; }

        [StringLength(255)]
        public string TEAM_NAME { get; set; }

        [StringLength(255)]
        public string Manager { get; set; }

        [StringLength(255)]
        public string NTLOGIN { get; set; }

        [StringLength(30)]
        public string COUNTRY { get; set; }

        [StringLength(255)]
        public string FULLNAME { get; set; }

        [StringLength(255)]
        public string SUPERREGION { get; set; }

        [StringLength(255)]
        public string EMAIL { get; set; }

        [StringLength(10)]
        public string ACTIVE { get; set; }

        [StringLength(150)]
        public string PROFILE_PIC { get; set; }
    }
}
