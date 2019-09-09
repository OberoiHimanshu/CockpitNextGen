namespace BAL_Service_User_Mgmt
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class UM_DataModel : DbContext
    {
        public UM_DataModel()
            : base("name=UM_DataModel")
        {
        }

        public virtual DbSet<VW_USERS> VW_USERS { get; set; }

        public virtual DbSet<Tbl_Country_Sorg_Orig> Tbl_Country_Sorg_Orig { get; set; }

        public virtual DbSet<TBL_ROLE> TBL_ROLE { get; set; }

        public virtual DbSet<TBL_TEAM_STRUCTURE> TBL_TEAM_STRUCTURE { get; set; }

        public virtual DbSet<Tbl_Unmapped_Orders_By_Region_Function> Tbl_Unmapped_Orders_By_Region_Function { get; set; }

        public virtual DbSet<Tbl_Unmapped_Summary> Tbl_Unmapped_Summary { get; set; }

        public virtual DbSet<Tbl_Unmapped_Users> Tbl_Unmapped_Users { get; set; }

        public virtual DbSet<TBL_USERS> TBL_USERS { get; set; }

    }
}
