using System;
using stappBackend.Models;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
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
                User customerLennert = new Customer() { FirstName = "Lennert", LastName = "Bontinck", Email = "info@lennertbontinck.com"};
                customerLennert.Login = new Login(){ Role = customerRole, Username = "lennert"};

                User customerBram = new Customer() { FirstName = "Bram", LastName = "De Coninck", Email = "info@bramdeconinck.com" };
                customerBram.Login = new Login() { Role = customerRole, Username = "bram" };

                User customerJodi = new Customer() { FirstName = "Jodi", LastName = "De Loof", Email = "info@jodideloof.be" };
                customerJodi.Login = new Login() { Role = customerRole, Username = "jodi" };

                User merchantRestaurantSpaghetti = new Merchant() { FirstName = "Spaghetti", LastName = "Verantwoordelijke", Email = "info@mister-spaghetti.com" };
                merchantRestaurantSpaghetti.Login = new Login() { Role = merchantRole, Username = "mrspaghetti" };

                User merchantWinkelFnac = new Merchant() { FirstName = "Fnac", LastName = "Verantwoordelijke", Email = "info@fnac.be" };
                merchantWinkelFnac.Login = new Login() { Role = merchantRole, Username = "fnac" };

                User merchantCafeSafir = new Merchant() { FirstName = "Safir", LastName = "Verantwoordelijke", Email = "info@cafesafir.be" };
                merchantCafeSafir.Login = new Login() { Role = merchantRole, Username = "safir" };

                User merchantSchoolHoGent = new Merchant() { FirstName = "HoGent", LastName = "Verantwoordelijke", Email = "info@hogent.be" };
                merchantSchoolHoGent.Login = new Login() { Role = merchantRole, Username = "hogent" };

                var users = new List<User>
                {
                    customerLennert, customerBram, customerJodi,
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

                foreach (User user in users)
                {
                    user.Login.Salt = salt;
                    user.Login.Hash = hash;
                }
                // END SET PASSWORDS

                // START SOCIAL MEDIA
                SocialMedia facebookSocialMedia = new SocialMedia(){LogoPath = "img/socialMediaLogo/facebook.png", Name = "Facebook"};
                SocialMedia instagramSocialMedia = new SocialMedia() { LogoPath = "img/socialMediaLogo/isntagram.png", Name = "Instagram" };
                SocialMedia twitterSocialMedia = new SocialMedia() { LogoPath = "img/socialMediaLogo/twitter.png", Name = "Twitter" };

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

                ((Merchant)merchantRestaurantSpaghetti).Companies.Add(mrspaghettiCompany);
                ((Merchant)merchantWinkelFnac).Companies.Add(fnacCompany);
                ((Merchant)merchantCafeSafir).Companies.Add(safirCompany);
                ((Merchant)merchantSchoolHoGent).Companies.Add(hogentCompany);
                // END COMPANIES

                // START ESTABLISHMENT
                Establishment mrspaghettiAalstEstablishment = new Establishment();
                Establishment fnacAalstEstablishment = new Establishment();
                Establishment safirAalstEstablishment = new Establishment();
                Establishment hogentAalstEstablishment = new Establishment();

                // END ESTABLISHMENT

                // BEGIN SAVE CHANGES
                _dbContext.Categories.AddRange(categories);
                _dbContext.Roles.AddRange(roles);
                _dbContext.Users.AddRange(users);
                _dbContext.SaveChanges();
                _dbContext.SocialMedias.AddRange(socialsMedias);
                // END SAVE CHANGES
            }
        }
    }
}
