using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using stappBackend.Models;
using stappBackend.Models.Domain;
using stappBackend.Models.IRepositories;
using stappBackend.Models.ViewModels.Attachments;
using stappBackend.Models.ViewModels.Promotion;
using File = stappBackend.Models.Domain.File;

namespace stappBackend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly IEstablishmentRepository _establishmentRepository;

        public PromotionController(IPromotionRepository promotionRepository, IEstablishmentRepository establishmentRepository)
        {
            _promotionRepository = promotionRepository;
            _establishmentRepository = establishmentRepository;
        }

        // GET api/promotion
        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<Promotion> Get()
        {
            return _promotionRepository.GetAll();
        }

        // GET api/promotion/id
        [HttpGet("{id}")]
        [AllowAnonymous]
        public Promotion Get(int id)
        {
            return _promotionRepository.getById(id);
        }

        [HttpPost]
        public IActionResult Post([FromBody]AddPromotionViewModel promotionToAdd)
        {
            if (!IsMerchant())
                return BadRequest(new { error = "De voorziene token voldoet niet aan de eisen." });

            //modelstate werkt niet op lijsten :-D
            if (promotionToAdd.Images == null || !promotionToAdd.Images.Any())
                return BadRequest(new { error = "geen Images meegeven." });

            if (!ContainsJpgs(promotionToAdd.Images))
                return BadRequest(new { error = "geen jpg images gevonden" });

            if (promotionToAdd.StartDate == null)
                return BadRequest(new { error = "geen StartDate meegeven." });

            if (promotionToAdd.EndDate == null)
                return BadRequest(new { error = "geen EndDate meegeven." });


            if (ModelState.IsValid)
            {
                Establishment establishmentFromDb = _establishmentRepository.getById(promotionToAdd.EstablishmentId ?? 0);

                if (establishmentFromDb == null)
                    return BadRequest(new { error = "Establishment niet gevonden" });

                if (!_establishmentRepository.isOwnerOfEstablishment(int.Parse(User.FindFirst("userId")?.Value), establishmentFromDb.EstablishmentId))
                    return BadRequest(new { error = "Establishment behoord niet tot uw establishments" });

                Promotion newPromotion = new Promotion
                {
                    Name = promotionToAdd.Name,
                    Message = promotionToAdd.Message,
                    StartDate = (DateTime)promotionToAdd.StartDate,
                    EndDate = (DateTime)promotionToAdd.EndDate
                };

                _promotionRepository.addPromotion(promotionToAdd.EstablishmentId ?? 0, newPromotion);

                //we hebben id nodig voor img path dus erna
                newPromotion.Images = ConvertFileViewModelToImages(promotionToAdd.Images, newPromotion.PromotionId);
                newPromotion.Attachments = ConvertFileViewModelToAttachments(promotionToAdd.Attachments, newPromotion.PromotionId);
                _promotionRepository.SaveChanges();

                return Ok(new { bericht = "De promotion werd succesvol toegevoegd." });
            }
            //Als we hier zijn is is modelstate niet voldaan dus stuur error 400, slechte aanvraag
            string errorMsg = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(new { error = errorMsg });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]ModifyPromotionViewModel editedPromotion)
        {
            if (ModelState.IsValid)
            {
                if (!IsMerchant())
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

                if (editedPromotion.Attachments != null && editedPromotion.Attachments.Any())
                {
                    var attachments = ConvertFileViewModelToAttachments(editedPromotion.Attachments, id);
                    if (attachments.Any())
                        promotion.Attachments = attachments;
                }

                if (editedPromotion.Images != null && editedPromotion.Images.Any())
                {
                    var images = ConvertFileViewModelToImages(editedPromotion.Images, id);
                    if (images.Any())
                        promotion.Images = images;
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
            if (!IsMerchant())
                return BadRequest(new { error = "De voorziene token voldoet niet aan de eisen." });

            if (!_promotionRepository.isOwnerOfPromotion(int.Parse(User.FindFirst("userId")?.Value), id))
                return BadRequest(new { error = "promotion behoord niet tot uw promotions" });

            Promotion promotion = _promotionRepository.getById(id);

            if (promotion == null)
                return BadRequest(new { error = "promotion niet gevonden" });

            _promotionRepository.removePromotion(id);
            return Ok(new { bericht = "De promotion werd succesvol verwijderd." });
        }

        [HttpPost("ownerof/{id}")]
        public Boolean Post(int id)
        {
            return _promotionRepository.isOwnerOfPromotion(int.Parse(User.FindFirst("userId")?.Value), id);
        }

        #region Helper Functies
        private bool IsMerchant()
        {
            return User.FindFirst("customRole")?.Value.ToLower() == "merchant" && User.FindFirst("userId")?.Value != null;
        }

        private List<Image> ConvertFileViewModelToImages(List<FileViewModel> imageFiles, int promotionId)
        {
            List<Image> images = new List<Image>();

            if (imageFiles == null)
                return images;

            for (int i = 1; i <= imageFiles.Count; i++)
            {
                if (Path.GetExtension(imageFiles[(i - 1)].FullFileName) == ".jpg")
                {
                    string imagePath = "img/promotions/" + promotionId + "/" + i + ".jpg";
                    images.Add(new Image { Path = imagePath });
                    string filePath = @"wwwroot/" + imagePath;

                    var bytes = Convert.FromBase64String(imageFiles[(i - 1)].Base64File);

                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    FileStream fileStream = new FileStream(filePath, FileMode.Create);
                    if (bytes.Length > 0)
                        fileStream.Write(bytes, 0, bytes.Length);
                    fileStream.Close();
                }
            }

            return images;
        }


        private List<File> ConvertFileViewModelToAttachments(List<FileViewModel> attachmentsFiles, int promotionId)
        {
            List<File> attachments = new List<File>();

            if (attachmentsFiles == null)
                return attachments;

            for (int i = 1; i <= attachmentsFiles.Count; i++)
            {
                if (Path.GetExtension(attachmentsFiles[(i - 1)].FullFileName) == ".pdf")
                {
                    string attachmentPath = "files/promotions/" + promotionId + "/" + i + ".pdf";
                    attachments.Add(new File { Path = attachmentPath, Name = attachmentsFiles[(i - 1)].Name });
                    string filePath = @"wwwroot/" + attachmentPath;

                    var bytes = Convert.FromBase64String(attachmentsFiles[(i - 1)].Base64File);

                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    FileStream fileStream = new FileStream(filePath, FileMode.Create);
                    if (bytes.Length > 0)
                        fileStream.Write(bytes, 0, bytes.Length);
                    fileStream.Close();
                }
            }
            return attachments;
        }

        private bool ContainsJpgs(List<FileViewModel> files)
        {
            return files.Any(f => Path.GetExtension(f.FullFileName) == ".jpg");
        }

        #endregion

    }
}