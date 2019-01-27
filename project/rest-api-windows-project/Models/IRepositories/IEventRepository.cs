using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stappBackend.Models.IRepositories
{
    public interface IEventRepository
    {
        IEnumerable<Event> GetAll();
        Event getById(int id);

        void addEvent(int establishmentId, Event newEvent);

        void removeEvent(int eventId);

        Boolean isOwnerOfEvent(int userId, int eventId);

        void SaveChanges();
    }
}
