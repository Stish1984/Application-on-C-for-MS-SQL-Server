namespace Sales_registration
{
    using System.Data.Entity;
  
    public partial class Context : DbContext
    {
        public Context()
            : base("name=ConnectionString")
        {
        }

        public virtual DbSet<Manager> Managers { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
