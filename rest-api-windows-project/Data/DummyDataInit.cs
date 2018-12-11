using System;
using stappBackend.Models;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using stappBackend.Models.Domain;

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
                // BEGIN CATEGORIES
                Category restrauntCategory = new Category() {Name = "Restaurant"};
                Category winkelCategory = new Category() { Name = "Winkel" };
                Category cafeCategory = new Category() { Name = "Café" };
                Category schoolCategory = new Category() { Name = "School" };

                var categories = new List<Category>
                {
                    restrauntCategory, winkelCategory, cafeCategory, schoolCategory
                };
                // END CATEGORIES

                // BEGIN ROLE
                Role customerRole = new Role(){ Name = "Customer"};
                Role merchantRole = new Role() { Name = "Merchant" };

                var roles = new List<Role>
                {
                    customerRole, merchantRole
                };
                //END ROLE

                // BEGIN USERS
                Customer customerLennert = new Customer() { FirstName = "Lennert", LastName = "Bontinck", Email = "info@lennertbontinck.com"};
                customerLennert.Login = new Login(){ Role = customerRole, Username = "lennert"};

                Customer customerBram = new Customer() { FirstName = "Bram", LastName = "De Coninck", Email = "info@bramdeconinck.com" };
                customerBram.Login = new Login() { Role = customerRole, Username = "bram" };

                Customer customerJodi = new Customer() { FirstName = "Jodi", LastName = "De Loof", Email = "info@jodideloof.be" };
                customerJodi.Login = new Login() { Role = customerRole, Username = "jodi" };

                Merchant merchantRestaurantSpaghetti = new Merchant() { FirstName = "Spaghetti", LastName = "Verantwoordelijke", Email = "info@mister-spaghetti.com" };
                merchantRestaurantSpaghetti.Login = new Login() { Role = merchantRole, Username = "mrspaghetti" };

                Merchant merchantWinkelFnac = new Merchant() { FirstName = "Fnac", LastName = "Verantwoordelijke", Email = "info@fnac.be" };
                merchantWinkelFnac.Login = new Login() { Role = merchantRole, Username = "fnac" };

                Merchant merchantCafeSafir = new Merchant() { FirstName = "Safir", LastName = "Verantwoordelijke", Email = "info@cafesafir.be" };
                merchantCafeSafir.Login = new Login() { Role = merchantRole, Username = "safir" };

                Merchant merchantSchoolHoGent = new Merchant() { FirstName = "HoGent", LastName = "Verantwoordelijke", Email = "info@hogent.be" };
                merchantSchoolHoGent.Login = new Login() { Role = merchantRole, Username = "hogent" };

                var customers = new List<Customer>
                {
                    customerLennert, customerBram, customerJodi
                };
                var merchants = new List<Merchant>
                {
                    merchantRestaurantSpaghetti, merchantWinkelFnac, merchantCafeSafir, merchantSchoolHoGent
                };
                // END USERS

                // START SET PASSWORDS 
                byte[] salt = new byte[128 / 8];
                using (var randomGetal = RandomNumberGenerator.Create())
                {
                    randomGetal.GetBytes(salt);
                }

                string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: "Wachtwoord123",
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));

                foreach (User user in customers)
                {
                    user.Login.Salt = salt;
                    user.Login.Hash = hash;
                }

                foreach (User user in merchants)
                {
                    user.Login.Salt = salt;
                    user.Login.Hash = hash;
                }
                // END SET PASSWORDS

                // START SOCIAL MEDIA
                SocialMedia facebookSocialMedia = new SocialMedia(){LogoPath = "img/socialMediaLogos/facebook/facebook.png", Name = "Facebook"};
                SocialMedia instagramSocialMedia = new SocialMedia() { LogoPath = "img/socialMediaLogos/instagram/instagram.png", Name = "Instagram" };
                SocialMedia twitterSocialMedia = new SocialMedia() { LogoPath = "img/socialMediaLogos/twitter/twitter.png", Name = "Twitter" };

                var socialsMedias = new List<SocialMedia>
                {
                    facebookSocialMedia, instagramSocialMedia, twitterSocialMedia
                };
                // END SOCIAL MEDIA

                // START COMPANIES
                Company mrspaghettiCompany = new Company(){ Name = "Mr Spaghetti"};
                Company fnacCompany = new Company() { Name = "Fnac" };
                Company safirCompany = new Company() { Name = "Safir" };
                Company hogentCompany = new Company() { Name = "HoGent" };
                // END COMPANIES

                // START ESTABLISHMENT
                // Day of week 0 = maandag, openinghours null = gesloten 
                Establishment mrspaghettiAalstEstablishment = new Establishment(){ Name = "Restaurant Mr Spaghetti", Description = "Kom langs bij Mister Spaghetti en laat je verbazen door de pasta bij uitstek!", PostalCode = "9300", City = "Aalst", Street = "Hopmarkt", HouseNumber = "33", Latitude = 50.937142, Longitude = 4.036673 };

                mrspaghettiAalstEstablishment.EstablishmentCategories.Add(new EstablishmentCategory() { Category = restrauntCategory });
                mrspaghettiAalstEstablishment.EstablishmentCategories.Add(new EstablishmentCategory() { Category = cafeCategory });

                mrspaghettiAalstEstablishment.EstablishmentSocialMedias.Add(new EstablishmentSocialMedia() {SocialMedia = facebookSocialMedia, Url = "https://www.facebook.com/WeLoveMisterSpaghettiAalst/" });

                mrspaghettiAalstEstablishment.Images.Add(new Image() { Path = "img/establishments/1/1.jpg" });
                mrspaghettiAalstEstablishment.Images.Add(new Image() { Path = "img/establishments/1/2.jpg" });

                mrspaghettiAalstEstablishment.OpenDays.Add(new OpenDay() { DayOfTheWeek = 0 });
                mrspaghettiAalstEstablishment.OpenDays.Add(new OpenDay() { DayOfTheWeek = 1 });
                mrspaghettiAalstEstablishment.OpenDays.Add(new OpenDay() { DayOfTheWeek = 2, OpenHours = new List<OpenHour>(){ new OpenHour() { StartHour = 11, Startminute = 30, EndHour = 14,  EndMinute = 00 }, new OpenHour() { StartHour = 18, Startminute = 00, EndHour = 22, EndMinute = 00 } } }); 
                mrspaghettiAalstEstablishment.OpenDays.Add(new OpenDay() { DayOfTheWeek = 3, OpenHours = new List<OpenHour>(){ new OpenHour() { StartHour = 11, Startminute = 30, EndHour = 14,  EndMinute = 00 }, new OpenHour() { StartHour = 18, Startminute = 00, EndHour = 22, EndMinute = 00 } } });
                mrspaghettiAalstEstablishment.OpenDays.Add(new OpenDay() { DayOfTheWeek = 4, OpenHours = new List<OpenHour>(){ new OpenHour() { StartHour = 11, Startminute = 30, EndHour = 14,  EndMinute = 00 }, new OpenHour() { StartHour = 18, Startminute = 00, EndHour = 22, EndMinute = 00 } } });
                mrspaghettiAalstEstablishment.OpenDays.Add(new OpenDay() { DayOfTheWeek = 5, OpenHours = new List<OpenHour>(){ new OpenHour() { StartHour = 11, Startminute = 30, EndHour = 14,  EndMinute = 00 }, new OpenHour() { StartHour = 18, Startminute = 00, EndHour = 22, EndMinute = 00 } } });
                mrspaghettiAalstEstablishment.OpenDays.Add(new OpenDay() { DayOfTheWeek = 6, OpenHours = new List<OpenHour>(){ new OpenHour() { StartHour = 11, Startminute = 30, EndHour = 14, EndMinute = 00 }, new OpenHour() { StartHour = 18, Startminute = 00, EndHour = 22, EndMinute = 00 } } });

                mrspaghettiAalstEstablishment.ExceptionalDays.Add(new ExceptionalDay(){Day = DateTime.Today, Message = "Gesloten wegens familiale redenen."});
                mrspaghettiAalstEstablishment.ExceptionalDays.Add(new ExceptionalDay() { Day = DateTime.Today.AddDays(4), Message = "All you can eat event!" });
                mrspaghettiAalstEstablishment.ExceptionalDays.Add(new ExceptionalDay() { Day = DateTime.Today.AddDays(10), Message = "Ladies night event!" });

                mrspaghettiAalstEstablishment.Events.Add(new Event(){StartDate = DateTime.Today.AddDays(4).AddHours(11).AddMinutes(30), EndDate = DateTime.Today.AddDays(4).AddHours(22),
                    Name = "All you can eat",
                    Message = "Bij het all you can eat event betaal je een inkom van 20 euro en krijg je een ganse avond spaghetti voorgeschoteld! De normale openingsuren gelden.",
                    Images = new List<Image>(){new Image(){Path = "img/events/1/1.jpg"}, new Image() { Path = "img/events/1/2.jpg" } }
                });
                mrspaghettiAalstEstablishment.Events.Add(new Event(){StartDate = DateTime.Today.AddDays(10).AddHours(18), EndDate = DateTime.Today.AddDays(10).AddHours(23),
                    Name = "Ladies night",
                    Message = "Voor deze ladies night kunnen alle meiden vanaf 6u savonds terrecht bij Mr Spaghetti te Aalst voor een hapje en een drankje terwijl er een Sturm Der Liebe marathon afspeeld op het groot scherm!",
                    Images = new List<Image>() { new Image() { Path = "img/events/2/1.jpg" }, new Image() { Path = "img/events/2/2.jpg" } }
                });

                mrspaghettiAalstEstablishment.Promotions.Add(new Promotion() { StartDate = DateTime.Today.AddDays(11), EndDate = DateTime.Today.AddDays(18),
                    Name = "€ 5 korting op een spaghetti naar keuze",
                    Message = "€ 5 korting op een spaghetti naar keuze bij het vermelden van de couponcode 'Spaghet5'.",
                    Images = new List<Image>() { new Image() { Path = "img/promotions/1/1.jpg" }, new Image() { Path = "img/promotions/1/2.jpg" } }
                });

                mrspaghettiCompany.Establishments.Add(mrspaghettiAalstEstablishment);
                //-------
                Establishment fnacAalstEstablishment = new Establishment() { Name = "Fnac Aalst", Description = "Ontdek onze nieuwe Fnac winkel, en vind al je onmisbare artikelen: Boeken, CD's, Computers, Telefoons en nog veel meer.", PostalCode = "9300", City = "Aalst", Street = "Kattestraat", HouseNumber = "17", Latitude = 50.939538, Longitude = 4.037435 };

                fnacAalstEstablishment.EstablishmentCategories.Add(new EstablishmentCategory() { Category = winkelCategory });

                fnacAalstEstablishment.EstablishmentSocialMedias.Add(new EstablishmentSocialMedia() { SocialMedia = facebookSocialMedia, Url = "https://www.facebook.com/FnacAalst/" });
                fnacAalstEstablishment.EstablishmentSocialMedias.Add(new EstablishmentSocialMedia() { SocialMedia = twitterSocialMedia, Url = "https://twitter.com/fnacbelgie" });

                fnacAalstEstablishment.Images.Add(new Image() { Path = "img/establishments/2/1.jpg" });
                fnacAalstEstablishment.Images.Add(new Image() { Path = "img/establishments/2/2.jpg" });
                fnacAalstEstablishment.Images.Add(new Image() { Path = "img/establishments/2/3.jpg" });

                fnacAalstEstablishment.OpenDays.Add(new OpenDay() { DayOfTheWeek = 0, OpenHours = new List<OpenHour>() { new OpenHour() { StartHour = 9, Startminute = 30, EndHour = 18, EndMinute = 00 } } });
                fnacAalstEstablishment.OpenDays.Add(new OpenDay() { DayOfTheWeek = 1, OpenHours = new List<OpenHour>() { new OpenHour() { StartHour = 9, Startminute = 30, EndHour = 18, EndMinute = 00 } } });
                fnacAalstEstablishment.OpenDays.Add(new OpenDay() { DayOfTheWeek = 2, OpenHours = new List<OpenHour>() { new OpenHour() { StartHour = 9, Startminute = 30, EndHour = 18, EndMinute = 00 } } });
                fnacAalstEstablishment.OpenDays.Add(new OpenDay() { DayOfTheWeek = 3, OpenHours = new List<OpenHour>() { new OpenHour() { StartHour = 9, Startminute = 30, EndHour = 18, EndMinute = 00 } } });
                fnacAalstEstablishment.OpenDays.Add(new OpenDay() { DayOfTheWeek = 4, OpenHours = new List<OpenHour>() { new OpenHour() { StartHour = 9, Startminute = 30, EndHour = 18, EndMinute = 00 } } });
                fnacAalstEstablishment.OpenDays.Add(new OpenDay() { DayOfTheWeek = 5, OpenHours = new List<OpenHour>() { new OpenHour() { StartHour = 9, Startminute = 30, EndHour = 18, EndMinute = 00 } } });
                fnacAalstEstablishment.OpenDays.Add(new OpenDay() { DayOfTheWeek = 6 });

                fnacAalstEstablishment.ExceptionalDays.Add(new ExceptionalDay() { Day = DateTime.Today.AddDays(2), Message = "Gesloten wegens werken" });
                
                fnacAalstEstablishment.Promotions.Add(new Promotion() { StartDate = DateTime.Today.AddDays(12), EndDate = DateTime.Today.AddDays(16),
                    Name = "Week van de smartphone.",
                    Message = "Tot wel 50% korting op ons assortiment smartphones. Kom eens binnen en ontdek welk toestel onze experts u aanbevelen.",
                    Images = new List<Image>() { new Image() { Path = "img/promotions/2/1.jpg" }, new Image() { Path = "img/promotions/2/2.jpg" } }
                });

                fnacCompany.Establishments.Add(fnacAalstEstablishment);
                //-------
                Establishment safirAalstEstablishment = new Establishment() { Name = "Café Safir", Description = "Het café van Aalst voor jong en oud! Verschillende snack en lunch mogelijkheden aanwezig.", PostalCode = "9300", City = "Aalst", Street = "Grote Markt", HouseNumber = "22", Latitude = 50.938424, Longitude = 4.038867 };

                safirAalstEstablishment.EstablishmentCategories.Add(new EstablishmentCategory() { Category = cafeCategory });
                safirAalstEstablishment.EstablishmentCategories.Add(new EstablishmentCategory() { Category = restrauntCategory });

                safirAalstEstablishment.EstablishmentSocialMedias.Add(new EstablishmentSocialMedia() { SocialMedia = facebookSocialMedia, Url = "https://www.facebook.com/pages/category/Cafe/Safir-188724374609159/" });
                safirAalstEstablishment.Images.Add(new Image() { Path = "img/establishments/3/1.jpg" });
                safirAalstEstablishment.Images.Add(new Image() { Path = "img/establishments/3/2.jpg" });
                safirAalstEstablishment.Images.Add(new Image() { Path = "img/establishments/3/3.jpg" });

                safirAalstEstablishment.OpenDays.Add(new OpenDay() { DayOfTheWeek = 0, OpenHours = new List<OpenHour>() { new OpenHour() { StartHour = 9, Startminute = 30, EndHour = 0, EndMinute = 00 } } });
                safirAalstEstablishment.OpenDays.Add(new OpenDay() { DayOfTheWeek = 1, OpenHours = new List<OpenHour>() { new OpenHour() { StartHour = 9, Startminute = 30, EndHour = 0, EndMinute = 00 } } });
                safirAalstEstablishment.OpenDays.Add(new OpenDay() { DayOfTheWeek = 2, OpenHours = new List<OpenHour>() { new OpenHour() { StartHour = 9, Startminute = 30, EndHour = 0, EndMinute = 00 } } });
                safirAalstEstablishment.OpenDays.Add(new OpenDay() { DayOfTheWeek = 3, OpenHours = new List<OpenHour>() { new OpenHour() { StartHour = 9, Startminute = 30, EndHour = 0, EndMinute = 00 } } });
                safirAalstEstablishment.OpenDays.Add(new OpenDay() { DayOfTheWeek = 4, OpenHours = new List<OpenHour>() { new OpenHour() { StartHour = 9, Startminute = 30, EndHour = 0, EndMinute = 00 } } });
                safirAalstEstablishment.OpenDays.Add(new OpenDay() { DayOfTheWeek = 5, OpenHours = new List<OpenHour>() { new OpenHour() { StartHour = 9, Startminute = 30, EndHour = 0, EndMinute = 00 } } });
                safirAalstEstablishment.OpenDays.Add(new OpenDay() { DayOfTheWeek = 6 });

                safirAalstEstablishment.Promotions.Add(new Promotion() { StartDate = DateTime.Today.AddDays(2), EndDate = DateTime.Today.AddDays(2).AddHours(5),
                    Name = "Happy hours!",
                    Message = "2 pintjes voor de prijs van 1!",
                    Images = new List<Image>() { new Image() { Path = "img/promotions/3/1.jpg" }, new Image() { Path = "img/promotions/3/2.jpg" } }
                });

                safirCompany.Establishments.Add(safirAalstEstablishment);
                //-------
                Establishment hogentAalstEstablishment = new Establishment() { Name = "HoGent Campus Aalst", Description = "De hogeschool Gent Campus Aalst inspireert en stimuleert mensen om, op eigen wijze, het verschil te maken in en voor de samenleving.", PostalCode = "9300", City = "Aalst", Street = "Arbeidstraat", HouseNumber = "14", Latitude = 51.141550, Longitude = 4.559644 };

                hogentAalstEstablishment.EstablishmentCategories.Add(new EstablishmentCategory() { Category = schoolCategory });

                hogentAalstEstablishment.EstablishmentSocialMedias.Add(new EstablishmentSocialMedia() { SocialMedia = facebookSocialMedia, Url = "https://www.facebook.com/HoGentCampusAalst/" });
                hogentAalstEstablishment.EstablishmentSocialMedias.Add(new EstablishmentSocialMedia() { SocialMedia = twitterSocialMedia, Url = "https://twitter.com/hogeschool_gent" });
                hogentAalstEstablishment.EstablishmentSocialMedias.Add(new EstablishmentSocialMedia() { SocialMedia = instagramSocialMedia, Url = "https://www.instagram.com/explore/locations/420243736/hogent-stadscampus-aalst" });

                hogentAalstEstablishment.Images.Add(new Image() { Path = "img/establishments/4/1.jpg" });
                hogentAalstEstablishment.Images.Add(new Image() { Path = "img/establishments/4/2.jpg" });

                hogentAalstEstablishment.OpenDays.Add(new OpenDay() { DayOfTheWeek = 0, OpenHours = new List<OpenHour>() { new OpenHour() { StartHour = 8, Startminute = 30, EndHour = 12, EndMinute = 00 }, new OpenHour() { StartHour = 13, Startminute = 00, EndHour = 16, EndMinute = 30 } } });
                hogentAalstEstablishment.OpenDays.Add(new OpenDay() { DayOfTheWeek = 1, OpenHours = new List<OpenHour>() { new OpenHour() { StartHour = 8, Startminute = 30, EndHour = 12, EndMinute = 00 }, new OpenHour() { StartHour = 13, Startminute = 00, EndHour = 16, EndMinute = 30 } } });
                hogentAalstEstablishment.OpenDays.Add(new OpenDay() { DayOfTheWeek = 2, OpenHours = new List<OpenHour>() { new OpenHour() { StartHour = 8, Startminute = 30, EndHour = 12, EndMinute = 00 } } });
                hogentAalstEstablishment.OpenDays.Add(new OpenDay() { DayOfTheWeek = 3, OpenHours = new List<OpenHour>() { new OpenHour() { StartHour = 8, Startminute = 30, EndHour = 12, EndMinute = 00 }, new OpenHour() { StartHour = 13, Startminute = 00, EndHour = 16, EndMinute = 30 } } });
                hogentAalstEstablishment.OpenDays.Add(new OpenDay() { DayOfTheWeek = 4, OpenHours = new List<OpenHour>() { new OpenHour() { StartHour = 8, Startminute = 30, EndHour = 12, EndMinute = 00 } } });
                hogentAalstEstablishment.OpenDays.Add(new OpenDay() { DayOfTheWeek = 5 });
                hogentAalstEstablishment.OpenDays.Add(new OpenDay() { DayOfTheWeek = 6 });

                hogentAalstEstablishment.Events.Add(new Event()
                {
                    StartDate = new DateTime(2019, 3, 5).AddHours(14), EndDate = new DateTime(2019, 3, 8).AddHours(18),
                    Name = "Open lessen dagen!",
                    Message =
                        "Een goede manier om een toekomstige opleiding te kiezen, is gewoon komen proeven. Proeven van de leerstof, van de manier van lesgeven, van de sfeer op de campus. Gewoon een échte les meemaken tussen onze huidige studenten. Het aanbod aan Live! lessen is zeer divers. Pik eventueel ook een les mee uit een richting die je minder bekend in de oren klinkt.",
                    Images = new List<Image>() { new Image() { Path = "img/events/3/1.jpg" }, new Image() { Path = "img/events/3/2.jpg" } }
                });

                hogentCompany.Establishments.Add(hogentAalstEstablishment);
                // END ESTABLISHMENT

                // START ASSIGNING COMPANY TO MERCHANT 
                merchantRestaurantSpaghetti.Companies.Add(mrspaghettiCompany);
                merchantWinkelFnac.Companies.Add(fnacCompany);
                merchantCafeSafir.Companies.Add(safirCompany);
                merchantSchoolHoGent.Companies.Add(hogentCompany);
                // END ASSIGN COMPANY TO MERCHANT 

                // START SUBSCRIPTIONS 
                customerLennert.EstablishmentSubscriptions.Add(new EstablishmentSubscription(){DateAdded = DateTime.Today.AddDays(-10), Establishment = fnacAalstEstablishment});
                customerLennert.EstablishmentSubscriptions.Add(new EstablishmentSubscription(){DateAdded = DateTime.Today.AddDays(-4), Establishment = hogentAalstEstablishment});
                customerBram.EstablishmentSubscriptions.Add(new EstablishmentSubscription(){DateAdded = DateTime.Today.AddDays(-8), Establishment = fnacAalstEstablishment});
                customerBram.EstablishmentSubscriptions.Add(new EstablishmentSubscription(){DateAdded = DateTime.Today.AddDays(-3), Establishment = mrspaghettiAalstEstablishment});
                customerJodi.EstablishmentSubscriptions.Add(new EstablishmentSubscription(){DateAdded = DateTime.Today.AddDays(-6), Establishment = safirAalstEstablishment});
                customerJodi.EstablishmentSubscriptions.Add(new EstablishmentSubscription(){DateAdded = DateTime.Today.AddDays(-1), Establishment = mrspaghettiAalstEstablishment});
                // END SUBSCRIPTIONS 

                // BEGIN SAVE CHANGES
                _dbContext.Categories.AddRange(categories);
                _dbContext.Roles.AddRange(roles);
                _dbContext.Merchants.AddRange(merchants);
                _dbContext.Customers.AddRange(customers);
                _dbContext.SaveChanges();
                _dbContext.SocialMedias.AddRange(socialsMedias);
                // END SAVE CHANGES
            }
        }
    }
}
