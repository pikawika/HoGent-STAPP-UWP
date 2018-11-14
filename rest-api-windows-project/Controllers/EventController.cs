using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using stappBackend.Models;
using stappBackend.Models.IRepositories;

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
    }
}