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
                .Include(e => e.Images)
                .ToList();
        }

        public Event getById(int id)
        {
            return _events.Where(e => e.EventId == id)
                .Include(e => e.Images)
                .FirstOrDefault();
        }
    }
}
