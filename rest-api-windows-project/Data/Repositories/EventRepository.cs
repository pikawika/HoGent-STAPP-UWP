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
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
            _events = context.Events;
        }

        public IEnumerable<Event> GetAll()
        {
            return _events
                .Where(e => e.EndDate >= DateTime.Today && !e.Establishment.isDeleted)
                .Include(e => e.Images)
                .Include(e => e.Establishment).ThenInclude(e => e.EstablishmentCategories).ThenInclude(ec => ec.Category)
                .Include(e => e.Establishment).ThenInclude(e => e.EstablishmentSocialMedias).ThenInclude(esm => esm.SocialMedia)
                .Include(e => e.Establishment).ThenInclude(e => e.OpenDays).ThenInclude(od => od.OpenHours)
                .Include(e => e.Establishment).ThenInclude(e => e.ExceptionalDays)
                .ToList();
        }

        public Event getById(int id)
        {
            return _events.Where(e => e.EventId == id && e.EndDate >= DateTime.Today && !e.Establishment.isDeleted)
                .Include(e => e.Images)
                .Include(e => e.Establishment).ThenInclude(e => e.EstablishmentCategories).ThenInclude(ec => ec.Category)
                .Include(e => e.Establishment).ThenInclude(e => e.EstablishmentSocialMedias).ThenInclude(esm => esm.SocialMedia)
                .Include(e => e.Establishment).ThenInclude(e => e.OpenDays).ThenInclude(od => od.OpenHours)
                .Include(e => e.Establishment).ThenInclude(e => e.ExceptionalDays)
                .FirstOrDefault();
        }
    }
}
