namespace B_Web_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TBL_USERS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int USER_ID { get; set; }

        [StringLength(255)]
        public string USERNAME { get; set; }

        public int? TEAM_ID { get; set; }

        public int? ROLE_ID { get; set; }

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

        public int? SUBSCRIPTION_ID { get; set; }

        [StringLength(150)]
        public string PROFILE_PIC { get; set; }
    }

    public partial class TBL_TEAM_STRUCTURE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TEAM_ID { get; set; }

        public string TEAM_NAME { get; set; }
        public string SUPERVISOR { get; set; }
        public string BACKUP { get; set; }
        public Nullable<System.DateTime> BACKUPFROM { get; set; }
        public Nullable<System.DateTime> BACKUPTO { get; set; }
        public string BACKUP_COMMENTS { get; set; }
        public Nullable<int> ACTIVE { get; set; }
    }

    public partial class Tbl_Country_Sorg_Orig
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public Nullable<int> CountryID { get; set; }

        [StringLength(255)]
        public string Country { get; set; }

        [StringLength(255)]
        public string Sorg { get; set; }

        [StringLength(255)]
        public string Region { get; set; }
    }

}
