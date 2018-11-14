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
    public class PromotionController : ControllerBase
    {
        private IPromotionRepository _promotionRepository;

        public PromotionController(IPromotionRepository promotionRepository)
        {
            _promotionRepository = promotionRepository;
        }

        // GET api/promotion
        [HttpGet]
        public IEnumerable<Promotion> Get()
        {
            return _promotionRepository.GetAll();
        }

        // GET api/promotion/id
        [HttpGet("{id}")]
        public Promotion Get(int id)
        {
            return _promotionRepository.getById(id);
        }
    }
}