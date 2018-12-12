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
using stappBackend.Models.ViewModels.Event;
using File = stappBackend.Models.Domain.File;

namespace stappBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private IEventRepository _eventRepository;

        public EventController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        // GET api/event
        [HttpGet]
        public IEnumerable<Event> Get()
        {
            return _eventRepository.GetAll();
        }

        // GET api/event/id
        [HttpGet("{id}")]
        public Event Get(int id)
        {
            return _eventRepository.getById(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm]AddEventViewModel eventToAdd)
        {
            if (!isMerchant())
                return BadRequest(new { error = "De voorziene token voldoet niet aan de eisen." });

            //modelstate werkt niet op lijsten :-D
            if (eventToAdd.Attachments.Files == null || !eventToAdd.Attachments.Files.Any())
                return BadRequest(new { error = "geen Images meegeven." });

            if (eventToAdd.StartDate == null)
                return BadRequest(new { error = "geen StartDate meegeven." });

            if (eventToAdd.EndDate == null)
                return BadRequest(new { error = "geen EndDate meegeven." });


            if (ModelState.IsValid)
            {
                Event newEvent = new Event
                {
                    Name = eventToAdd.Name,
                    Message = eventToAdd.Message,
                    StartDate = (DateTime)eventToAdd.StartDate,
                    EndDate = (DateTime)eventToAdd.EndDate
                };

                _eventRepository.addEvent(eventToAdd.establishmentId ?? 0, newEvent);

                //we hebben id nodig voor img path dus erna
                newEvent.Images = await ConvertFormFilesToImagesAsync(eventToAdd.Attachments.Files.ToList(), newEvent.EventId);
                newEvent.Attachments = await ConvertFormFilesToAttachmentsAsync(eventToAdd.Attachments.Files.ToList(), newEvent.EventId);
                _eventRepository.SaveChanges();

                return Ok(new { bericht = "De event werd succesvol toegevoegd." });
            }
            //Als we hier zijn is is modelstate niet voldaan dus stuur error 400, slechte aanvraag
            string errorMsg = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(new { error = errorMsg });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm]ModifyEventViewModel editedEvent)
        {
            if (ModelState.IsValid)
            {
                if (!isMerchant())
                    return BadRequest(new { error = "De voorziene token voldoet niet aan de eisen." });

                Event eventFromDb = _eventRepository.getById(id);

                if (eventFromDb == null)
                    return BadRequest(new { error = "Event niet gevonden" });

                if (!_eventRepository.isOwnerOfEvent(int.Parse(User.FindFirst("userId")?.Value), id))
                    return BadRequest(new { error = "Event behoord niet tot uw events" });

                if (!string.IsNullOrEmpty(editedEvent.Name))
                    eventFromDb.Name = editedEvent.Name;

                if (!string.IsNullOrEmpty(editedEvent.Message))
                    eventFromDb.Message = editedEvent.Message;

                if (editedEvent.StartDate != null)
                    eventFromDb.StartDate = (DateTime)editedEvent.StartDate;

                if (editedEvent.EndDate != null)
                    eventFromDb.EndDate = (DateTime)editedEvent.EndDate;

                if (editedEvent.Attachments != null && editedEvent.Attachments.Files.Any())
                {
                    eventFromDb.Images = await ConvertFormFilesToImagesAsync(editedEvent.Attachments.Files.ToList(), id);
                    eventFromDb.Attachments = await ConvertFormFilesToAttachmentsAsync(editedEvent.Attachments.Files.ToList(), id);
                }

                _eventRepository.SaveChanges();
                return Ok(new { bericht = "De event werd succesvol bijgewerkt." });
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

            if (!_eventRepository.isOwnerOfEvent(int.Parse(User.FindFirst("userId")?.Value), id))
                return BadRequest(new { error = "event behoord niet tot uw events" });

            Event eventFromDb = _eventRepository.getById(id);

            if (eventFromDb == null)
                return BadRequest(new { error = "event niet gevonden" });

            _eventRepository.removeEvent(id);
            return Ok(new { bericht = "De event werd succesvol verwijderd." });
        }

        #region Helper Functies
        private bool isMerchant()
        {
            return User.FindFirst("customRole")?.Value.ToLower() == "merchant" && User.FindFirst("userId")?.Value != null;
        }

        private async Task<List<Image>> ConvertFormFilesToImagesAsync(List<IFormFile> imageFiles, int eventId)
        {
            List<Image> images = new List<Image>();

            imageFiles.RemoveAll(i => Path.GetExtension(i.FileName) != ".jpg");

            if (imageFiles == null)
                return images;

            for (int i = 1; i <= imageFiles.Count; i++)
            {
                if (Path.GetExtension(imageFiles[(i - 1)].FileName) == ".jpg")
                {
                    string imagePath = "img/events/" + eventId + "/" + i + ".jpg";
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

        private async Task<List<File>> ConvertFormFilesToAttachmentsAsync(List<IFormFile> attachmentsFiles, int eventId)
        {
            List<File> attachments = new List<File>();

            if (attachmentsFiles == null)
                return attachments;

            for (int i = 1; i <= attachmentsFiles.Count; i++)
            {
                if (Path.GetExtension(attachmentsFiles[(i - 1)].FileName) == ".pdf")
                {
                    string imagePath = "files/events/" + eventId + "/" + i + ".pdf";
                    attachments.Add(new File { Path = imagePath, Name = attachmentsFiles[(i - 1)].FileName });
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