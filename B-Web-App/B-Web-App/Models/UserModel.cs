using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace B_Web_App.Models
{
    
    public class User_Model
    {
        [Key]
        public int USER_ID { get; set; }
        public string SAP_User_Name { get; set; }
        public string ROLE_DESC { get; set; }
        public string TEAM_NAME { get; set; }
        public string Manager { get; set; }
        public string NTLOGIN { get; set; }
        public string COUNTRY { get; set; }
        public string FULLNAME { get; set; }
        public string SUPERREGION { get; set; }
        public string EMAIL { get; set; }
        public string ACTIVE { get; set; }
        public string PROFILE_PIC { get; set; }
    }

    public class ROLE_Model
    {
        [Key]
        public int ROLE_ID { get; set; }
        public string ROLE_DESC { get; set; }
    }

    public class Country_Sorg_Model
    {
        [Key]
        public int Id { get; set; }
        public Nullable<int> CountryID { get; set; }
        public string Country { get; set; }
        public string Sorg { get; set; }
        public string Region { get; set; }
    }

    public class Unmapped_Summary_Model
    {
        [Key]
        [Column (Order =0)]
        public string REGION { get; set; }
        [Key]
        [Column(Order = 1)]
        public string SALES_ORG { get; set; }

        public Nullable<int> Order_Count { get; set; }
        public Nullable<double> Total_NV { get; set; }
        public Nullable<int> TotalUser { get; set; }
    }

    public class Unmapped_Users_Model
    {
        [Key]
        public string REGION { get; set; }
        public string USERGROUP { get; set; }
        public Nullable<int> Order_Count { get; set; }
        public Nullable<double> Total_NV { get; set; }
        public Nullable<int> TotalUser { get; set; }
    }

}
