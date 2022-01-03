using FirstMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstMVC.Data
{
    public class CompanyContext : DbContext
    {
        public CompanyContext(DbContextOptions<CompanyContext> options) : base(options)
        {
        }

        public DbSet<Company> Company { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Departament> Departaments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().ToTable("Company");
            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<Departament>().ToTable("Departaments");

            modelBuilder.Entity<Company>()
                .HasMany(e => e.Departaments)
                .WithOne(d => d.Company)
                .HasForeignKey(e => e.CompanyID);

            modelBuilder.Entity<Departament>()
                .HasMany(e => e.Employees)
                .WithOne(d => d.Departament)
                .HasForeignKey(e => e.DepartamentID);
        }
    }
}
