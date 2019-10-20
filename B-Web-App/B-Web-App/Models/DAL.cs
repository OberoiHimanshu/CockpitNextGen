namespace B_Web_App.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class DAL : DbContext
    {
        public DAL()
            : base("name=AzureDBConnection")
        {
        }

        public virtual DbSet<User_Model> VW_USers { get; set; }
        public virtual DbSet<ROLE_Model> Roles { get; set; }
        public virtual DbSet<Country_Sorg_Model> Country_Sorgs { get; set; }

    }
}