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
using stappBackend.Models.ViewModels.Event;
using File = stappBackend.Models.Domain.File;

namespace stappBackend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEstablishmentRepository _establishmentRepository;

        public EventController(IEventRepository eventRepository, IEstablishmentRepository establishmentRepository)
        {
            _eventRepository = eventRepository;
            _establishmentRepository = establishmentRepository;
        }

        // GET api/event
        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<Event> Get()
        {
            return _eventRepository.GetAll();
        }

        // GET api/event/id
        [HttpGet("{id}")]
        [AllowAnonymous]
        public Event Get(int id)
        {
            return _eventRepository.getById(id);
        }

        [HttpPost]
        public IActionResult Post([FromBody]AddEventViewModel eventToAdd)
        {
            if (!IsMerchant())
                return BadRequest(new { error = "De voorziene token voldoet niet aan de eisen." });

            //modelstate werkt niet op lijsten :-D
            if (eventToAdd.Images == null || !eventToAdd.Images.Any())
                return BadRequest(new { error = "Geen afbeelding(en) meegeven." });

            if (!ContainsJpgs(eventToAdd.Images))
                return BadRequest(new { error = "Geen jpg afbeelding(en) meegeven" });

            if (eventToAdd.StartDate == null)
                return BadRequest(new { error = "Geen start datum meegeven." });

            if (eventToAdd.EndDate == null)
                return BadRequest(new { error = "Geen eind datum meegeven." });


            if (ModelState.IsValid)
            {
                Establishment establishmentFromDb = _establishmentRepository.getById(eventToAdd.EstablishmentId ?? 0);

                if (establishmentFromDb == null)
                    return BadRequest(new { error = "Vestiging met opgegeven id niet gevonden" });

                if (!_establishmentRepository.isOwnerOfEstablishment(int.Parse(User.FindFirst("userId")?.Value), establishmentFromDb.EstablishmentId))
                    return BadRequest(new { error = "Vestiging behoord niet tot uw vestigingen" });

                Event newEvent = new Event
                {
                    Name = eventToAdd.Name,
                    Message = eventToAdd.Message,
                    StartDate = (DateTime)eventToAdd.StartDate,
                    EndDate = (DateTime)eventToAdd.EndDate
                };

                _eventRepository.addEvent(eventToAdd.EstablishmentId ?? 0, newEvent);

                //we hebben id nodig voor img path dus erna
                newEvent.Images = ConvertFileViewModelToImages(eventToAdd.Images, newEvent.EventId);
                newEvent.Attachments = ConvertFileViewModelToAttachments(eventToAdd.Attachments, newEvent.EventId);
                _eventRepository.SaveChanges();

                return Ok(new { bericht = "Het evenement werd succesvol toegevoegd." });
            }
            //Als we hier zijn is is modelstate niet voldaan dus stuur error 400, slechte aanvraag
            string errorMsg = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(new { error = errorMsg });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]ModifyEventViewModel editedEvent)
        {
            if (ModelState.IsValid)
            {
                if (!IsMerchant())
                    return BadRequest(new { error = "De voorziene token voldoet niet aan de eisen." });

                Event eventFromDb = _eventRepository.getById(id);

                if (eventFromDb == null)
                    return BadRequest(new { error = "Evenement niet gevonden." });

                if (!_eventRepository.isOwnerOfEvent(int.Parse(User.FindFirst("userId")?.Value), id))
                    return BadRequest(new { error = "Evenement behoord niet tot uw evenementen." });

                if (!string.IsNullOrEmpty(editedEvent.Name))
                    eventFromDb.Name = editedEvent.Name;

                if (!string.IsNullOrEmpty(editedEvent.Message))
                    eventFromDb.Message = editedEvent.Message;

                if (editedEvent.StartDate != null)
                    eventFromDb.StartDate = (DateTime)editedEvent.StartDate;

                if (editedEvent.EndDate != null)
                    eventFromDb.EndDate = (DateTime)editedEvent.EndDate;

                if (editedEvent.Images != null && editedEvent.Images.Any())
                {
                    var images = ConvertFileViewModelToImages(editedEvent.Images, id);
                    if (images.Any())
                        eventFromDb.Images = images;
                }

                if (editedEvent.Attachments != null && editedEvent.Attachments.Any())
                {
                    var attachments = ConvertFileViewModelToAttachments(editedEvent.Attachments, id);
                    if (attachments.Any())
                        eventFromDb.Attachments = attachments;
                }

                _eventRepository.SaveChanges();
                return Ok(new { bericht = "Het evenement werd succesvol bijgewerkt." });
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

            if (!_eventRepository.isOwnerOfEvent(int.Parse(User.FindFirst("userId")?.Value), id))
                return BadRequest(new { error = "Evenement behoord niet tot uw evenementen." });

            Event eventFromDb = _eventRepository.getById(id);

            if (eventFromDb == null)
                return BadRequest(new { error = "Evenement niet gevonden." });

            _eventRepository.removeEvent(id);
            return Ok(new { bericht = "Het evenement werd succesvol verwijderd." });
        }

        [HttpPost("ownerof/{id}")]
        public Boolean Post(int id)
        {
            return _eventRepository.isOwnerOfEvent(int.Parse(User.FindFirst("userId")?.Value), id);
        }

        #region Helper Functies
        private bool IsMerchant()
        {
            return User.FindFirst("customRole")?.Value.ToLower() == "merchant" && User.FindFirst("userId")?.Value != null;
        }

        private List<Image> ConvertFileViewModelToImages(List<FileViewModel> imageFiles, int eventId)
        {
            List<Image> images = new List<Image>();

            if (imageFiles == null)
                return images;

            for (int i = 1; i <= imageFiles.Count; i++)
            {
                if (Path.GetExtension(imageFiles[(i - 1)].FullFileName) == ".jpg")
                {
                    string imagePath = "img/events/" + eventId + "/" + i + ".jpg";
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


        private List<File> ConvertFileViewModelToAttachments(List<FileViewModel> attachmentsFiles, int eventId)
        {
            List<File> attachments = new List<File>();

            if (attachmentsFiles == null)
                return attachments;

            for (int i = 1; i <= attachmentsFiles.Count; i++)
            {
                if (Path.GetExtension(attachmentsFiles[(i - 1)].FullFileName) == ".pdf")
                {
                    string attachmentPath = "files/events/" + eventId + "/" + i + ".pdf";
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