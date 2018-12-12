using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using stappBackend.Models;
using stappBackend.Models.IRepositories;

namespace stappBackend.Data.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly DbSet<Event> _events;
        private readonly DbSet<Establishment> _establishments;
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
            _events = context.Events;
            _establishments = context.Establishments;
        }

        public IEnumerable<Event> GetAll()
        {
            return _events
                .Where(e => e.EndDate >= DateTime.Today && !e.isDeleted)
                .Include(e => e.Images)
                .Include(e => e.Attachments)
                .Include(e => e.Establishment).ThenInclude(e => e.EstablishmentCategories).ThenInclude(ec => ec.Category)
                .Include(e => e.Establishment).ThenInclude(e => e.EstablishmentSocialMedias).ThenInclude(esm => esm.SocialMedia)
                .Include(e => e.Establishment).ThenInclude(e => e.OpenDays).ThenInclude(od => od.OpenHours)
                .Include(e => e.Establishment).ThenInclude(e => e.ExceptionalDays)
                .ToList();
        }

        public Event getById(int id)
        {
            return _events.Where(e => e.EventId == id && e.EndDate >= DateTime.Today && !e.isDeleted)
                .Include(e => e.Images)
                .Include(e => e.Attachments)
                .Include(e => e.Establishment).ThenInclude(e => e.EstablishmentCategories).ThenInclude(ec => ec.Category)
                .Include(e => e.Establishment).ThenInclude(e => e.EstablishmentSocialMedias).ThenInclude(esm => esm.SocialMedia)
                .Include(e => e.Establishment).ThenInclude(e => e.OpenDays).ThenInclude(od => od.OpenHours)
                .Include(e => e.Establishment).ThenInclude(e => e.ExceptionalDays)
                .FirstOrDefault();
        }

        public void addEvent(int establishmentId, Event newEvent)
        {
            _establishments.FirstOrDefault(e => e.EstablishmentId == establishmentId)?.Events.Add(newEvent);
            SaveChanges();
        }

        public void removeEvent(int eventId)
        {
            Event eventToDelete = _events.FirstOrDefault(c => c.EventId == eventId);
            if (eventToDelete != null)
            {
                eventToDelete.isDeleted = true;
                SaveChanges();
            }
        }

        public bool isOwnerOfEvent(int userId, int eventId)
        {
            return _events.Any(e => !e.isDeleted && e.EventId == eventId && e.Establishment.Company.Merchant.UserId == userId);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
