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
    }
}
