﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using stappBackend.Models;
using stappBackend.Models.IRepositories;

namespace stappBackend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class MerchantController : ControllerBase
    {
        private ICompanyRepository _companyRepository;

        public MerchantController(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        [HttpGet("Company")]
        public IActionResult Get()
        {
            if (!isMerchant())
                return BadRequest(new { error = "De voorziene token voldoet niet aan de eisen." });

            List<Company> companies = _companyRepository.getFromMerchant(int.Parse(User.FindFirst("userId")?.Value));

            return Ok(companies);
        }

        [HttpGet("Company/{id}")]
        public IActionResult Get(int id)
        {
            if (!isMerchant())
                return BadRequest(new { error = "De voorziene token voldoet niet aan de eisen." });

            Company company = _companyRepository.getById(id);

            if (company == null)
                return BadRequest(new { error = "Bedrijf niet gevonden." });

            if (company.MerchantId != int.Parse(User.FindFirst("userId")?.Value))
                return BadRequest(new { error = "Dit bedrijf behoord niet tot uw bedrijven." });

            return Ok(company);
        }

        private bool isMerchant()
        {
            return User.FindFirst("customRole")?.Value.ToLower() == "merchant" && User.FindFirst("userId")?.Value != null;
        }
    }
}