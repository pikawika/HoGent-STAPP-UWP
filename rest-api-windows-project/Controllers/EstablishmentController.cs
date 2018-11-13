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
    public class EstablishmentController : ControllerBase
    {
        private IEstablishmentRepository _establishmentRepository;

        public EstablishmentController(IEstablishmentRepository establishmentRepository)
        {
            _establishmentRepository = establishmentRepository;
        }

        // GET api/Establishment
        [HttpGet]
        public IEnumerable<Establishment> Get()
        {
            return _establishmentRepository.GetAll();
        }

        // GET api/Establishment/id
        [HttpGet("{id}")]
        public Establishment Get(int id)
        {
            return _establishmentRepository.getById(id);
        }

    }
}