namespace B_Web_App.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CNG_Azure : DbContext
    {
        public CNG_Azure()
            : base("name=CNG_Azure")
        {
        }

        public virtual DbSet<Tbl_Unmapped_Users> Tbl_Unmapped_Users { get; set; }
        public virtual DbSet<TBL_USERS> TBL_USERS { get; set; }
        public virtual DbSet<VW_USERS> VW_USERS { get; set; }
        public virtual DbSet<TBL_TEAM_STRUCTURE> TBL_TEAM_STRUCTURE { get; set; }
        public virtual DbSet<Tbl_Country_Sorg_Orig> Tbl_Country_Sorg_Orig { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TBL_USERS>()
                .Property(e => e.COUNTRY)
                .IsFixedLength();

            modelBuilder.Entity<TBL_USERS>()
                .Property(e => e.ACTIVE)
                .IsFixedLength();

            modelBuilder.Entity<VW_USERS>()
                .Property(e => e.ROLE_DESC)
                .IsUnicode(false);

            modelBuilder.Entity<VW_USERS>()
                .Property(e => e.COUNTRY)
                .IsFixedLength();

            modelBuilder.Entity<VW_USERS>()
                .Property(e => e.ACTIVE)
                .IsFixedLength();
        }
    }
}
