using Microsoft.EntityFrameworkCore;
using stappBackend.Models;

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
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<OpenDay> OpenDays { get; set; }
        public DbSet<OpenHour> OpenHours { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
