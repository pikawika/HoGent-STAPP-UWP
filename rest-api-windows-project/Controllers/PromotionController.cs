using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using stappBackend.Models;
using stappBackend.Models.Domain;
using stappBackend.Models.IRepositories;
using stappBackend.Models.ViewModels.Promotion;
using File = stappBackend.Models.Domain.File;

namespace stappBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private IPromotionRepository _promotionRepository;

        public PromotionController(IPromotionRepository promotionRepository)
        {
            _promotionRepository = promotionRepository;
        }

        // GET api/promotion
        [HttpGet]
        public IEnumerable<Promotion> Get()
        {
            return _promotionRepository.GetAll();
        }

        // GET api/promotion/id
        [HttpGet("{id}")]
        public Promotion Get(int id)
        {
            return _promotionRepository.getById(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm]AddPromotionViewModel promotionToAdd)
        {
            if (!isMerchant())
                return BadRequest(new { error = "De voorziene token voldoet niet aan de eisen." });

            //modelstate werkt niet op lijsten :-D
            if (promotionToAdd.Attachments.Files == null || !promotionToAdd.Attachments.Files.Any())
                return BadRequest(new { error = "geen Images meegeven." });

            if (promotionToAdd.StartDate == null)
                return BadRequest(new { error = "geen StartDate meegeven." });

            if (promotionToAdd.EndDate == null)
                return BadRequest(new { error = "geen EndDate meegeven." });


            if (ModelState.IsValid)
            {
                Promotion newPromotion = new Promotion
                {
                    Name = promotionToAdd.Name,
                    Message = promotionToAdd.Message,
                    StartDate = (DateTime)promotionToAdd.StartDate,
                    EndDate = (DateTime)promotionToAdd.EndDate
                };

                _promotionRepository.addPromotion(promotionToAdd.establishmentId ?? 0, newPromotion);

                //we hebben id nodig voor img path dus erna
                newPromotion.Images = await ConvertFormFilesToImagesAsync(promotionToAdd.Attachments.Files.ToList(), newPromotion.PromotionId);
                newPromotion.Attachments = await ConvertFormFilesToAttachmentsAsync(promotionToAdd.Attachments.Files.ToList(), newPromotion.PromotionId);
                _promotionRepository.SaveChanges();

                return Ok(new { bericht = "De promotion werd succesvol toegevoegd." });
            }
            //Als we hier zijn is is modelstate niet voldaan dus stuur error 400, slechte aanvraag
            string errorMsg = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(new { error = errorMsg });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm]ModifyPromotionViewModel editedPromotion)
        {
            if (ModelState.IsValid)
            {
                if (!isMerchant())
                    return BadRequest(new { error = "De voorziene token voldoet niet aan de eisen." });

                Promotion promotion = _promotionRepository.getById(id);

                if (promotion == null)
                    return BadRequest(new { error = "Promotion niet gevonden" });

                if (!_promotionRepository.isOwnerOfPromotion(int.Parse(User.FindFirst("userId")?.Value), id))
                    return BadRequest(new { error = "promotion behoord niet tot uw promotions" });

                if (!string.IsNullOrEmpty(editedPromotion.Name))
                    promotion.Name = editedPromotion.Name;

                if (!string.IsNullOrEmpty(editedPromotion.Message))
                    promotion.Message = editedPromotion.Message;

                if (editedPromotion.StartDate != null)
                    promotion.StartDate = (DateTime)editedPromotion.StartDate;

                if (editedPromotion.EndDate != null)
                    promotion.EndDate = (DateTime)editedPromotion.EndDate;

                if (editedPromotion.Attachments != null && editedPromotion.Attachments.Files.Any())
                {
                    var images = await ConvertFormFilesToImagesAsync(editedPromotion.Attachments.Files.ToList(), id);
                    var attachments = await ConvertFormFilesToAttachmentsAsync(editedPromotion.Attachments.Files.ToList(), id);
                    if (images.Any())
                        promotion.Images = images;
                    if (attachments.Any())
                        promotion.Attachments = attachments;
                }

                _promotionRepository.SaveChanges();
                return Ok(new { bericht = "De promotion werd succesvol bijgewerkt." });
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

            if (!_promotionRepository.isOwnerOfPromotion(int.Parse(User.FindFirst("userId")?.Value), id))
                return BadRequest(new { error = "promotion behoord niet tot uw promotions" });

            Promotion promotion = _promotionRepository.getById(id);

            if (promotion == null)
                return BadRequest(new { error = "promotion niet gevonden" });

            _promotionRepository.removePromotion(id);
            return Ok(new { bericht = "De promotion werd succesvol verwijderd." });
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

        private async Task<List<File>> ConvertFormFilesToAttachmentsAsync(List<IFormFile> attachmentsFiles, int promotionId)
        {
            List<File> attachments = new List<File>();
            
            if (attachmentsFiles == null)
                return attachments;

            for (int i = 1; i <= attachmentsFiles.Count; i++)
            {
                if (Path.GetExtension(attachmentsFiles[(i - 1)].FileName) == ".pdf")
                {
                    string imagePath = "files/promotions/" + promotionId + "/" + i + ".pdf";
                    attachments.Add(new File {Path = imagePath, Name = attachmentsFiles[(i - 1)].FileName});
                    string filePath = @"wwwroot/" + imagePath;
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    FileStream fileStream = new FileStream(filePath, FileMode.Create);
                    await attachmentsFiles[(i - 1)].CopyToAsync(fileStream);
                    fileStream.Close();
                }
            }

            return attachments;
        }

        #endregion

    }
}