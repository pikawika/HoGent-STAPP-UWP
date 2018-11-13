using Microsoft.EntityFrameworkCore;
using stappBackend.Models;
using stappBackend.Models.Domain;

namespace stappBackend.Data
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }

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
                .HasForeignKey(es => es.UserId);

            modelBuilder.Entity<EstablishmentSubscription>()
                .HasOne(es => es.Establishment)
                .WithMany(e => e.EstablishmentSubscriptions)
                .HasForeignKey(es => es.EstablishmentId);
            //EINDE CUSTOMER ESTABLISHMENT
        }
    }
}
