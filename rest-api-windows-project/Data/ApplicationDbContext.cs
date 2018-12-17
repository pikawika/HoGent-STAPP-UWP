using Microsoft.EntityFrameworkCore;
using stappBackend.Models;
using stappBackend.Models.Domain;

namespace stappBackend.Data
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Establishment> Establishments { get; set; }
        public DbSet<EstablishmentCategory> EstablishmentCategories { get; set; }
        public DbSet<EstablishmentSocialMedia> EstablishmentSocialMedias { get; set; }
        public DbSet<EstablishmentSubscription> EstablishmentSubscriptions { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<ExceptionalDay> ExceptionalDays { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<OpenDay> OpenDays { get; set; }
        public DbSet<OpenHour> OpenHours { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //BEGIN LOGIN
            modelBuilder.Entity<User>()
                .HasOne(u => u.Login)
                .WithOne(l => l.User)
                .HasForeignKey<Login>(b => b.UserLoginId);
            //EINDE LOGIN

            //BEGIN ESTABLISMENT CATEGORY ESTABLISHMENT
            modelBuilder.Entity<EstablishmentCategory>()
                .HasKey(ec => new { ec.EstablishmentId, ec.CategoryId });

            modelBuilder.Entity<EstablishmentCategory>()
                .HasOne(ec => ec.Establishment)
                .WithMany(e => e.EstablishmentCategories)
                .HasForeignKey(ec => ec.EstablishmentId);

            modelBuilder.Entity<EstablishmentCategory>()
                .HasOne(ec => ec.Category)
                .WithMany(c => c.EstablishmentCategories)
                .HasForeignKey(ec => ec.CategoryId);
            //EINDE ESTABLISMENT CATEGORY ESTABLISHMENT

            //BEGIN ESTABLISMENT SOCIALMEDIA ESTABLISHMENT
            modelBuilder.Entity<EstablishmentSocialMedia>()
                .HasKey(es => new { es.EstablishmentId, es.SocialMediaId });

            modelBuilder.Entity<EstablishmentSocialMedia>()
                .HasOne(es => es.Establishment)
                .WithMany(e => e.EstablishmentSocialMedias)
                .HasForeignKey(es => es.EstablishmentId);

            modelBuilder.Entity<EstablishmentSocialMedia>()
                .HasOne(es => es.SocialMedia)
                .WithMany(s => s.EstablishmentSocialMedias)
                .HasForeignKey(es => es.SocialMediaId);
            //EINDE ESTABLISMENT SOCIALMEDIA ESTABLISHMENT

            //BEGIN CUSTOMER ESTABLISHMENT
            modelBuilder.Entity<EstablishmentSubscription>()
                .HasKey(es => new { es.UserId, es.EstablishmentId });

            modelBuilder.Entity<EstablishmentSubscription>()
                .HasOne(es => es.Customer)
                .WithMany(c => c.EstablishmentSubscriptions)
                .HasForeignKey(es => es.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EstablishmentSubscription>()
                .HasOne(es => es.Establishment)
                .WithMany(e => e.EstablishmentSubscriptions)
                .HasForeignKey(es => es.EstablishmentId)
                .OnDelete(DeleteBehavior.Restrict);
            //EINDE CUSTOMER ESTABLISHMENT

            //BEGIN MERCHANT COMPANY
            modelBuilder.Entity<Company>()
                .HasOne(c => c.Merchant)
                .WithMany(m => m.Companies)
                .HasForeignKey(c => c.MerchantId);
            //EINDE MERCHANT COMPANY

            //BEGIN COMPANY ESTABLISHMENT
            modelBuilder.Entity<Establishment>()
                .HasOne(c => c.Company)
                .WithMany(m => m.Establishments)
                .HasForeignKey(c => c.CompanyId);
            //EINDE COMPANY ESTABLISHMENT
        }
    }
}
