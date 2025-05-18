namespace MedicalLaboratory
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Threading.Tasks;

    public partial class MedicalLaboratory20Entities : DbContext
    {
        public MedicalLaboratory20Entities()
            : base("name=MedicalLaboratory20Entities")
        {

        }
        public Task<int> SaveChangesAsync()
        {
            return Task.Run(() => SaveChanges());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public DbSet<Analyzers> Analyzers { get; set; }
        public DbSet<InsuranceCompanies> InsuranceCompanies { get; set; }
        public DbSet<InsuranceInvoices> InsuranceInvoices { get; set; }
        public DbSet<LoginHistory> LoginHistories { get; set; } // Изменено на множественное число
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderServices> OrderServices { get; set; }
        public DbSet<Patients> Patients { get; set; }
        public DbSet<PerformedServices> PerformedServices { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<SystemUsers> Users { get; set; } // Изменено на Users для соответствия коду
        public DbSet<UserServices> UserServices { get; set; }
    }
}