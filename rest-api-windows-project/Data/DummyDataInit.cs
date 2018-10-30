using stappBackend.Models;
using System.Collections.Generic;

namespace stappBackend.Data
{
    public class DummyDataInit
    {
        private readonly ApplicationDbContext _dbContext;

        public DummyDataInit(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void InitializeData()
        {
            _dbContext.Database.EnsureDeleted();
            if (_dbContext.Database.EnsureCreated())
            {
                //categories
                EstablishmentCategory restaurant = new EstablishmentCategory() { Category = new Category() { Name = "Restaurant"} };
                _dbContext.EstablishmentCategories.Add(restaurant);

                //socials 
                SocialMedia facebook = new SocialMedia() { Name = "Facebook", LogoPath = "/logo/facebook.png" };
                _dbContext.SocialMedias.Add(facebook);

                //openingsuren
                List<OpenHour> openHours = new List<OpenHour>();
                openHours.Add(new OpenHour() { StartHour = 7, EndHour = 12, Startminute = 30, EndMinute = 30 });
                openHours.Add(new OpenHour() { StartHour = 13, EndHour = 18, Startminute = 30, EndMinute = 30 });

                List<OpenDay> openDays = new List<OpenDay>();
                openDays.Add(new OpenDay() { DayOfTheWeek = 0, OpenHours = openHours });
                openDays.Add(new OpenDay() { DayOfTheWeek = 1, OpenHours = openHours });
                openDays.Add(new OpenDay() { DayOfTheWeek = 2, OpenHours = openHours });
                openDays.Add(new OpenDay() { DayOfTheWeek = 3, OpenHours = openHours });
                openDays.Add(new OpenDay() { DayOfTheWeek = 4, OpenHours = openHours });
                openDays.Add(new OpenDay() { DayOfTheWeek = 5, OpenHours = openHours });
                openDays.Add(new OpenDay() { DayOfTheWeek = 6, OpenHours = openHours });


                Establishment mrspaghettiAalst = new Establishment() { Name="Mr. Spaghetti Aalst", Straatnaam = "Hopmarkt", Huisnummer = "25", Postcode ="9300", Gemeente = "Aalst" };
                mrspaghettiAalst.Categories.Add(restaurant);
                mrspaghettiAalst.Socials.Add(new EstablishmentSocialMedia() { SocialMediaInfo = facebook, url = "https://facebook.com/mrspaghettiaalst" });
                mrspaghettiAalst.OpenDays = openDays;

                List<Establishment> estList = new List<Establishment>();
                estList.Add(mrspaghettiAalst);
                
                Company mrspaghetti = new Company() { Name = "Mr. Spaghetti", Establishments = estList };
                _dbContext.Companies.Add(mrspaghetti);

                //user
                Merchant merchant = new Merchant() { FirstName = "Joske", LastName = "Spaghetti",Email = "joskespaghetti@mrspaghetti.com" };
                merchant.Companies.Add(mrspaghetti);
                Customer customer = new Customer() { FirstName = "Jodi", LastName = "De Loof", Email = "jodi@jodideloof.be" };
                customer.Subscriptions.Add(new EstablishmentSubscription() { Establishment = mrspaghettiAalst });
                _dbContext.Merchants.Add(merchant);
                _dbContext.Customers.Add(customer);


                // SAVE CHANGES
                _dbContext.SaveChanges();
            }
        }
    }
}
