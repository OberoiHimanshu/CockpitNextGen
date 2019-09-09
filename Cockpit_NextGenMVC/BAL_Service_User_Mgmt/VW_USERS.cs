namespace BAL_Service_User_Mgmt
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

    public partial class TBL_ROLE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ROLE_ID { get; set; }

        [StringLength(255)]
        public string ROLE_DESC { get; set; }
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

    public partial class Tbl_Unmapped_Summary
    {
        [Key, Column(Order = 0)]
        public string REGION { get; set; }

        [Key, Column(Order = 1)]
        public string SALES_ORG { get; set; }

        public Nullable<int> Order_Count { get; set; }
        public Nullable<double> Total_NV { get; set; }
        public Nullable<int> TotalUser { get; set; }
    }

    public partial class Tbl_Unmapped_Users
    {
        [Key, Column(Order = 0)]
        public string REGION { get; set; }

        [Key, Column(Order = 1)]
        public string USERGROUP { get; set; }

        public Nullable<int> Order_Count { get; set; }
        public Nullable<double> Total_NV { get; set; }
        public Nullable<int> TotalUser { get; set; }
    }

    public partial class Tbl_Unmapped_Orders_By_Region_Function
    {
        [Key, Column(Order = 0)]
        public string REGION { get; set; }

        [Key, Column(Order = 1)]
        public string ORDER_CREATED_BY { get; set; }

        public Nullable<int> Orders { get; set; }
        public Nullable<double> Total_NV { get; set; }
    }

    public partial class Usp_Unmapped_User_DetailsBySalesOrg
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public double SALES_ORDERNO { get; set; }

        public string REGION { get; set; }
        public string SALES_ORG { get; set; }
        public Nullable<double> Total_NV { get; set; }
        public string ORDER_CREATED_BY { get; set; }
    }

    public partial class Usp_Unmapped_User_Summary
    {
        [Key, Column(Order = 0)]
        public string REGION { get; set; }

        [Key, Column(Order = 1)]
        public string SALES_ORG { get; set; }

        public Nullable<int> Order_Count { get; set; }
        public Nullable<double> Total_NV { get; set; }
        public Nullable<int> TotalUser { get; set; }
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

    public partial class TBL_USERS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int USER_ID { get; set; }

        public string USERNAME { get; set; }
        public Nullable<int> TEAM_ID { get; set; }
        public Nullable<int> ROLE_ID { get; set; }
        public string NTLOGIN { get; set; }
        public string COUNTRY { get; set; }
        public string FULLNAME { get; set; }
        public string SUPERREGION { get; set; }
        public string EMAIL { get; set; }
        public string ACTIVE { get; set; }
        public Nullable<int> SUBSCRIPTION_ID { get; set; }
        public string PROFILE_PIC { get; set; }
    }

}
