using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using stappBackend.Models;
using stappBackend.Models.IRepositories;
using stappBackend.Models.ViewModels.Establishment;

namespace stappBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EstablishmentController : ControllerBase
    {
        private IEstablishmentRepository _establishmentRepository;

        public EstablishmentController(IEstablishmentRepository establishmentRepository)
        {
            _establishmentRepository = establishmentRepository;
        }

        // GET api/establishment
        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<Establishment> Get()
        {
            return _establishmentRepository.GetAll();
        }

        // GET api/establishment/id
        [AllowAnonymous]
        [HttpGet("{id}")]
        public Establishment Get(int id)
        {
            return _establishmentRepository.getById(id);
        }







        

        private bool isMerchant()
        {
            return User.FindFirst("role")?.Value == "Merchant" && User.FindFirst("userId")?.Value != null;
        }

    }
}