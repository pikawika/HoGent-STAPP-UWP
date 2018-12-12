using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using stappBackend.Models;
using stappBackend.Models.Domain;
using stappBackend.Models.IRepositories;
using stappBackend.Models.ViewModels.Establishment;

namespace stappBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EstablishmentController : ControllerBase
    {
        private IEstablishmentRepository _establishmentRepository;
        private ICategoryRepository _categoryRepository;
        private ICompanyRepository _companyRepository;
        private ISocialMediaRepository _socialMediaRepository;

        public EstablishmentController(IEstablishmentRepository establishmentRepository, ICategoryRepository categoryRepository, ICompanyRepository companyRepository, ISocialMediaRepository socialMediaRepository)
        {
            _establishmentRepository = establishmentRepository;
            _categoryRepository = categoryRepository;
            _companyRepository = companyRepository;
            _socialMediaRepository = socialMediaRepository;
        }

        // GET api/establishment
        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<Establishment> Get()
        {
            return _establishmentRepository.GetAll();
        }

        // GET api/establishment/id
        [AllowAnonymous]
        [HttpGet("{id}")]
        public Establishment Get(int id)
        {
            return _establishmentRepository.getById(id);
        }



        [HttpPost]
        public async Task<IActionResult> Post([FromForm]AddEstablishmentViewModel establishmentToAdd)
        {
            if (ModelState.IsValid)
            {
                if (!isMerchant())
                    return BadRequest(new { error = "U bent geen handelaar." });

                //modelstate werkt niet op lijsten :-D
                if (establishmentToAdd.Categories == null || !establishmentToAdd.Categories.Any())
                    return BadRequest(new { error = "geen categories meegeven." });

                //modelstate werkt niet op lijsten :-D
                if (establishmentToAdd.SocialMedias == null || !establishmentToAdd.SocialMedias.Any())
                    return BadRequest(new { error = "geen SocialMedias meegeven." });

                //modelstate werkt niet op lijsten :-D
                if (establishmentToAdd.OpenDays == null || !establishmentToAdd.OpenDays.Any())
                    return BadRequest(new { error = "geen OpenDays meegeven." });

                //modelstate werkt niet op lijsten :-D
                if (establishmentToAdd.Images.Files == null || !establishmentToAdd.Images.Files.Any())
                    return BadRequest(new { error = "geen Images meegeven." });

                if (!_companyRepository.isOwnerOfCompany(int.Parse(User.FindFirst("userId")?.Value), establishmentToAdd.CompanyId ?? 0))
                    return BadRequest(new { error = "De company waaraan u deze establishment wilt toevoegen is niet van u." });

                // Ophalen van Latitude en Longitude op basis van het meegegeven adres
                var adress = $"{establishmentToAdd.Street}+{establishmentToAdd.HouseNumber},+{establishmentToAdd.PostalCode}+{establishmentToAdd.City},+België";

                Debug.WriteLine("HALLO", "lol " + establishmentToAdd.Categories[0].Name);

                List<double> latAndLong = await GetLatAndLongFromAddressAsync(adress);

                Establishment newEstablishment = new Establishment
                {
                    Name = establishmentToAdd.Name,
                    Description = establishmentToAdd.Description,

                    Street = establishmentToAdd.Street,
                    HouseNumber = establishmentToAdd.HouseNumber,
                    PostalCode = establishmentToAdd.PostalCode,
                    City = establishmentToAdd.City,
                    Latitude = latAndLong[0],
                    Longitude = latAndLong[1],

                    EstablishmentCategories = ConvertCategoryViewModelsToCategory(establishmentToAdd.Categories),

                    OpenDays = ConvertOpenDaysViewModelsToOpenDays(establishmentToAdd.OpenDays),
                    ExceptionalDays = ConvertExceptionalDaysViewModelsToExceptionalDays(establishmentToAdd.ExceptionalDays),
                    
                    EstablishmentSocialMedias = ConvertEstablishmentSocialMediasViewModelsToEstablishmentSocialMedias(establishmentToAdd.SocialMedias)
                };

                _establishmentRepository.addEstablishment(establishmentToAdd.CompanyId ?? 0, newEstablishment);

                //we hebben id nodig voor img path dus erna
                newEstablishment.Images = await ConvertFormFilesToImagesAsync(establishmentToAdd.Images.Files.ToList(), newEstablishment.EstablishmentId);
                _establishmentRepository.SaveChanges();
                return Ok(new { bericht = "De establishment werd succesvol toegevoegd." });
            }
            //Als we hier zijn is is modelstate niet voldaan dus stuur error 400, slechte aanvraag
            string errorMsg = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(new { error = errorMsg });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm]ModifyEstablishmentViewModel editedEstablishment)
        {
            if (ModelState.IsValid)
            {
                if (!isMerchant())
                    return BadRequest(new { error = "De voorziene token voldoet niet aan de eisen." });

                Establishment establishment = _establishmentRepository.getById(id);

                if (establishment == null)
                    return BadRequest(new { error = "Establishment niet gevonden" });

                if (!_establishmentRepository.isOwnerOfEstablishment(int.Parse(User.FindFirst("userId")?.Value), id))
                    return BadRequest(new { error = "Establishment behoord niet tot uw establishments" });

                //alles ok, mag editen 

                if (!string.IsNullOrEmpty(editedEstablishment.Name))
                    establishment.Name = editedEstablishment.Name;

                if (!string.IsNullOrEmpty(editedEstablishment.Description))
                    establishment.Description = editedEstablishment.Description;

                if (!string.IsNullOrEmpty(editedEstablishment.PostalCode))
                    establishment.PostalCode = editedEstablishment.PostalCode;

                if (!string.IsNullOrEmpty(editedEstablishment.City))
                    establishment.City = editedEstablishment.City;

                if (!string.IsNullOrEmpty(editedEstablishment.Street))
                    establishment.Street = editedEstablishment.Street;

                if (!string.IsNullOrEmpty(editedEstablishment.HouseNumber))
                    establishment.HouseNumber = editedEstablishment.HouseNumber;

                if (editedEstablishment.Categories != null && editedEstablishment.Categories.Any())
                    establishment.EstablishmentCategories = ConvertCategoryViewModelsToCategory(editedEstablishment.Categories);

                if (editedEstablishment.SocialMedias != null && editedEstablishment.SocialMedias.Any())
                    establishment.EstablishmentSocialMedias =
                        ConvertEstablishmentSocialMediasViewModelsToEstablishmentSocialMedias(editedEstablishment
                            .SocialMedias);

                if (editedEstablishment.OpenDays != null && editedEstablishment.OpenDays.Any())
                    establishment.OpenDays =  ConvertOpenDaysViewModelsToOpenDays(editedEstablishment.OpenDays);

                if (editedEstablishment.ExceptionalDays != null && editedEstablishment.ExceptionalDays.Any())
                    establishment.ExceptionalDays = ConvertExceptionalDaysViewModelsToExceptionalDays(editedEstablishment.ExceptionalDays);

                if (editedEstablishment.Images != null && editedEstablishment.Images.Files.Any())
                    establishment.Images = await ConvertFormFilesToImagesAsync(editedEstablishment.Images.Files.ToList(), id);


                _companyRepository.SaveChanges();
                return Ok(new { bericht = "De company werd succesvol bijgewerkt." });
            }
            //Als we hier zijn is is modelstate niet voldaan dus stuur error 400, slechte aanvraag
            string errorMsg = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(new { error = errorMsg });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!isMerchant())
                return BadRequest(new { error = "De voorziene token voldoet niet aan de eisen." });

            Establishment establishment = _establishmentRepository.getById(id);

            if (establishment == null)
                return BadRequest(new { error = "Establishment niet gevonden" });

            if (!_establishmentRepository.isOwnerOfEstablishment(int.Parse(User.FindFirst("userId")?.Value), id))
                return BadRequest(new { error = "Establishment behoord niet tot uw Establishments" });

            _establishmentRepository.removeEstablishment(id);
            return Ok(new { bericht = "De establishment werd succesvol verwijderd." });
        }

        #region Helper Functies
        private bool isMerchant()
        {
            return User.FindFirst("customRole")?.Value.ToLower() == "merchant" && User.FindFirst("userId")?.Value != null;
        }

        private async Task<List<Image>> ConvertFormFilesToImagesAsync(List<IFormFile> imageFiles, int promotionId)
        {
            List<Image> images = new List<Image>();

            if (imageFiles == null)
                return images;

            for (int i = 1; i <= imageFiles.Count; i++)
            {
                if (Path.GetExtension(imageFiles[(i - 1)].FileName) == ".jpg")
                {
                    string imagePath = "img/promotions/" + promotionId + "/" + i + ".jpg";
                    images.Add(new Image { Path = imagePath });
                    string filePath = @"wwwroot/" + imagePath;
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    FileStream fileStream = new FileStream(filePath, FileMode.Create);
                    await imageFiles[(i - 1)].CopyToAsync(fileStream);
                    fileStream.Close();
                }
            }

            return images;
        }

        // Methode voor het ophalen van de Latitude en Longitude van een bepaald adres
        private async Task<List<double>> GetLatAndLongFromAddressAsync(string adress)
        {
            var httpClient = new HttpClient();

            // We maken gebruik van Open Street Maps i.p.v. Google Maps omdat dit gratis en net zo goed werkt
            var url = $"https://nominatim.openstreetmap.org/search?q={adress}&format=json&polygon=1&addressdetails=1";

            // De enige van Nominatim is dat we een User-Agent meegeven aan onze request
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Lunchers");

            var httpResult = await httpClient.GetAsync(url);

            var result = await httpResult.Content.ReadAsStringAsync();

            // De request geeft ons een array met een aantal gegevens, waaronder de Latitude en Longitude
            var r = (JArray)JsonConvert.DeserializeObject(result);

            var latString = ((JValue)r[0]["lat"]);
            var longString = ((JValue)r[0]["lon"]);

            // We krijgen strings terug, dus het enige dat we nog moeten doen is ze omzetten naar doubles
            var latDouble = latString.ToObject<double>();
            var longDouble = longString.ToObject<double>();

            return new List<double>() { latDouble, longDouble };
        }

        private List<EstablishmentCategory> ConvertCategoryViewModelsToCategory(List<CategoryViewModel> categoriesvm)
        {
            List<EstablishmentCategory> establishmentCategories = new List<EstablishmentCategory>();
            foreach (CategoryViewModel categoryvm in categoriesvm)
            {
                Category category = _categoryRepository.GetByName(categoryvm.Name);
                if (category == null)
                {
                    //nog geen cat met die naam dus nieuwe maken!
                    category = new Category {Name = category.Name};
                    _categoryRepository.Add(category);
                }
                EstablishmentCategory establishmentCategory = new EstablishmentCategory { Category = category};
                establishmentCategories.Add(establishmentCategory);
            }
            return establishmentCategories;
        }

        private List<OpenDay> ConvertOpenDaysViewModelsToOpenDays(List<OpenDayViewModel> openDaysVm)
        {
            List<OpenDay> OpenDays = new List<OpenDay>();
            for (int i = 0; i <= 6; i++)
            {
                OpenDays.Add(new OpenDay{DayOfTheWeek = i});
                foreach (OpenHourViewModel openHourVm in openDaysVm.FirstOrDefault(od => od.DayOfTheWeek == i)?.OpenHours ?? new List<OpenHourViewModel>())
                {
                    OpenDays[i].OpenHours.Add(new OpenHour{EndHour = openHourVm.EndHour, EndMinute = openHourVm.EndMinute, StartHour = openHourVm.StartHour, Startminute = openHourVm.Startminute});
                }
                
            }
            return OpenDays;
        }

        private List<ExceptionalDay> ConvertExceptionalDaysViewModelsToExceptionalDays(List<ExceptionalDayViewModel> daysvm)
        {
            List<ExceptionalDay> exceptionalDays = new List<ExceptionalDay>();
            if (daysvm == null)
                return exceptionalDays;
            foreach (ExceptionalDayViewModel dayvm in daysvm)
            {
                exceptionalDays.Add(new ExceptionalDay{Day = dayvm.Day, Message = dayvm.Message});
            }
            return exceptionalDays;
        }

        private List<EstablishmentSocialMedia> ConvertEstablishmentSocialMediasViewModelsToEstablishmentSocialMedias(List<SocialMediaViewModel> socialsVm)
        {
            List<EstablishmentSocialMedia> establishmentSocialMedia = new List<EstablishmentSocialMedia>();
            foreach (SocialMediaViewModel socialVm in socialsVm)
            {
                SocialMedia socialMedia = _socialMediaRepository.getByName(socialVm.Name);
                if (socialMedia != null)
                    establishmentSocialMedia.Add(new EstablishmentSocialMedia{SocialMedia = socialMedia, Url = socialVm.Url});
            }
            return establishmentSocialMedia;
        }
        #endregion
    }
}