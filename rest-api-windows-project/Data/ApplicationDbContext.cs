using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Models;
using uwp_app_aalst_groep_a3.Models.Domain;

namespace stapp.Data
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
