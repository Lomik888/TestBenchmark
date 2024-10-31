using Microsoft.EntityFrameworkCore;
using TestBenchmark.Entities;

namespace TestBenchmark
{
    public class ApplicationDbContext : DbContext
    {
        private readonly string largeDbConnection = "Host=localhost;Database=testdb;Username=postgres;Password=1234";
        private readonly string smallDbConnection = "Host=localhost;Database=testdbsmall;Username=postgres;Password=1234";

        public string connectionString;

        public DbSet<PeopleEntity> Peoples { get; set; }
        public DbSet<OrganizationEntity> Organizations { get; set; }
        public DbSet<ComplimentEntity> Compliments { get; set; }

        public ApplicationDbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrganizationEntity>(x =>
            {
                x.Property(x => x.Id).ValueGeneratedOnAdd();
                x.Property(x => x.Name).IsRequired();
                x.Property(x => x.WebSite).IsRequired();
                x.Property(x => x.Country).IsRequired();
                x.Property(x => x.Description).IsRequired();
                x.Property(x => x.Founded).IsRequired();
                x.Property(x => x.Industry).IsRequired();

                x.HasMany(x => x.Employees)
                .WithOne(x => x.Organization)
                .HasForeignKey(x => x.OrganizationId)
                .HasPrincipalKey(x => x.Id);
            });

            modelBuilder.Entity<PeopleEntity>(x =>
            {
                x.Property(x => x.Id).ValueGeneratedOnAdd();
                x.Property(x => x.FirstName).IsRequired();
                x.Property(x => x.LastName).IsRequired();
                x.Property(x => x.Sex).IsRequired();
                x.Property(x => x.Email).IsRequired();
                x.Property(x => x.JobTitle).IsRequired();

                x.HasMany(x => x.Compliments)
                .WithMany(x => x.Peoples)
                .UsingEntity(x => x.ToTable("PeoplesAndCompliments"));
            });

            modelBuilder.Entity<ComplimentEntity>(x =>
            {
                x.Property(x => x.Id).ValueGeneratedOnAdd();
                x.Property(x => x.Compliment).IsRequired();
            });
        }
    }
}