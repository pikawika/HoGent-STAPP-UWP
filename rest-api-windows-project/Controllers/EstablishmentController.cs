using System;
using System.Collections.Generic;
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

        public EstablishmentController(IEstablishmentRepository establishmentRepository, ICategoryRepository categoryRepository, ICompanyRepository companyRepository)
        {
            _establishmentRepository = establishmentRepository;
            _categoryRepository = categoryRepository;
            _companyRepository = companyRepository;
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
            if (isMerchant())
                return BadRequest(new { error = "U bent geen handelaar." });

            if (!_companyRepository.isOwnerOfCompany(int.Parse(User.FindFirst("userId")?.Value), establishmentToAdd.CompanyId))
                return BadRequest(new { error = "De company waaraan u deze establishment wilt toevoegen is niet van u." });

            if (ModelState.IsValid)
            {
                // Ophalen van Latitude en Longitude op basis van het meegegeven adres
                var adress = $"{establishmentToAdd.Street}+{establishmentToAdd.HouseNumber},+{establishmentToAdd.PostalCode}+{establishmentToAdd.City},+België";



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

                    ExceptionalDays = null,
                    OpenDays = null,
                    EstablishmentSocialMedias = null,
                };

                //we hebben id nodig voor img path dus erna
                newEstablishment.Images = await ConvertFormFilesToImagesAsync(establishmentToAdd.Images.Files.ToList(), newEstablishment.EstablishmentId);

                _establishmentRepository.addEstablishment(establishmentToAdd.CompanyId, newEstablishment);
                return Ok(new { bericht = "De company werd succesvol toegevoegd." });
            }
            //Als we hier zijn is is modelstate niet voldaan dus stuur error 400, slechte aanvraag
            string errorMsg = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(new { error = errorMsg });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromForm]ModifyEstablishmentViewModel editedEstablishment)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok();
        }

        private bool isMerchant()
        {
            return User.FindFirst("role")?.Value == "Merchant" && User.FindFirst("userId")?.Value != null;
        }

        #region Helper Functies
        private async Task<List<Image>> ConvertFormFilesToImagesAsync(List<IFormFile> imageFiles, int establishmentId)
        {
            List<Image> images = new List<Image>();

            for (int i = 1; i <= imageFiles.Count; i++)
            {
                string imagePath = "/lunches/lunch" + establishmentId + "/" + i + ".jpg";
                images.Add(new Image { Path = imagePath });
                string filePath = @"wwwroot" + imagePath;
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                await imageFiles[(i - 1)].CopyToAsync(fileStream);
                fileStream.Close();
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
        #endregion

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
    }
}