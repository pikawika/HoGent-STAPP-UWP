using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using stappBackend.Models;
using stappBackend.Models.IRepositories;
using stappBackend.Models.ViewModels.Company;

namespace stappBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private IConfiguration _config;
        private ICompanyRepository _companyRepository;

        public CompanyController(IConfiguration config, ICompanyRepository companyRepository)
        {
            _config = config;
            _companyRepository = companyRepository;
        }

        [HttpPost]
        public IActionResult Post([FromBody]AddCompanyViewModel companyToAdd)
        {
            if (!isMerchant())
                return BadRequest(new { error = "De voorziene token voldoet niet aan de eisen." });

            if (ModelState.IsValid)
            {
                Company newCompany = new Company
                {
                    Name = companyToAdd.Name
                };

                _companyRepository.addCompany(int.Parse(User.FindFirst("userId")?.Value), newCompany);
                return Ok(new { bericht = "De company werd succesvol toegevoegd." });
            }
            //Als we hier zijn is is modelstate niet voldaan dus stuur error 400, slechte aanvraag
            string errorMsg = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(new { error = errorMsg });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]ModifyCompanyViewModel editedCompany)
        {
            if (ModelState.IsValid)
            {
                if (!isMerchant())
                    return BadRequest(new { error = "De voorziene token voldoet niet aan de eisen." });

                Company company = _companyRepository.getById(id);

                if (company == null)
                    return BadRequest(new { error = "Company niet gevonden" });

                if (_companyRepository.isOwnerOfCompany(int.Parse(User.FindFirst("userId")?.Value), id))
                    return BadRequest(new { error = "Company behoord niet tot uw companies" });

                if (!string.IsNullOrEmpty(editedCompany.Name))
                    company.Name = editedCompany.Name;

                _companyRepository.SaveChanges();
                return Ok(new { bericht = "De company werd succesvol bijgewerkt." });
            }
            //Als we hier zijn is is modelstate niet voldaan dus stuur error 400, slechte aanvraag
            string errorMsg = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(new { error = errorMsg });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!isMerchant())
                return BadRequest(new { error = "De voorziene token voldoet niet aan de eisen." });

            Company company = _companyRepository.getById(id);

            if (company == null)
                return BadRequest(new { error = "Company niet gevonden" });

            if (!_companyRepository.isOwnerOfCompany(int.Parse(User.FindFirst("userId")?.Value), id))
                return BadRequest(new { error = "Company behoord niet tot uw companies" });

            _companyRepository.removeCompany(id);
            return Ok(new { bericht = "De company werd succesvol verwijderd." });
        }

        private bool isMerchant()
        {
            return User.FindFirst("customRole")?.Value == "Merchant" && User.FindFirst("userId")?.Value != null;
        }
    }
}