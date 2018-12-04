using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using stappBackend.Models;
using stappBackend.Models.IRepositories;
using stappBackend.Models.ViewModels.Customer;
using stappBackend.Models.ViewModels.User;

namespace stappBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private IConfiguration _config;
        private ICustomerRepository _customerRepository;
        private IUserRepository _userRepository;
        private IEstablishmentRepository _establishmentRepository;

        public CustomerController(IConfiguration config, ICustomerRepository customerRepository, IEstablishmentRepository establishmentRepository, IUserRepository iUserRepository)
        {
            _config = config;
            _customerRepository = customerRepository;
            _establishmentRepository = establishmentRepository;
            _userRepository = iUserRepository;
        }

        [HttpPost("subscribe")]
        public IActionResult Post([FromBody]ModifySubscriptionViewModel addSubscriptionViewModel)
        {
            if (ModelState.IsValid)
            {
                if (User.FindFirst("userId")?.Value == null || User.FindFirst("customRole")?.Value.ToLower() != "customer")
                    return BadRequest(new { error = "De voorziene token voldoet niet aan de eisen." });

                Establishment establishment = _establishmentRepository.getById(addSubscriptionViewModel.establishmentId);

                if (establishment == null)
                    return BadRequest(new { error = "Geen establishment met de meegegeven id" });

                Customer customer = _customerRepository.getById(int.Parse(User.FindFirst("userId")?.Value));

                if (customer.EstablishmentSubscriptions.Any(es => es.EstablishmentId == establishment.EstablishmentId))
                    return BadRequest(new { error = "U bent reeds subscribed aan deze establishment" });

                EstablishmentSubscription establishmentSubscription = new EstablishmentSubscription() { Customer = customer, Establishment = establishment, DateAdded = DateTime.Now, EstablishmentId = establishment.EstablishmentId };

                _customerRepository.addSubscription(customer.UserId, establishmentSubscription);

                return Ok(new { message = "Toegevoegd!" });
            }
            //Als we hier zijn is is modelstate niet voldaan dus stuur error 400, slechte aanvraag
            string errorMsg = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(new { error = "De ingevoerde waarden zijn onvolledig of voldoen niet aan de eisen voor een login. Foutboodschap: " + errorMsg });
        }

        [HttpDelete("subscribe")]
        public IActionResult Delete([FromBody]ModifySubscriptionViewModel addSubscriptionViewModel)
        {
            if (ModelState.IsValid)
            {
                if (User.FindFirst("userId")?.Value == null || User.FindFirst("customRole")?.Value.ToLower() != "customer")
                    return BadRequest(new { error = "De voorziene token voldoet niet aan de eisen." });

                Establishment establishment = _establishmentRepository.getById(addSubscriptionViewModel.establishmentId);

                if (establishment == null)
                    return BadRequest(new { error = "Geen establishment met de meegegeven id" });

                Customer customer = _customerRepository.getById(int.Parse(User.FindFirst("userId")?.Value));

                EstablishmentSubscription establishmentSubscription =
                    customer.EstablishmentSubscriptions.SingleOrDefault(
                        es => es.EstablishmentId == establishment.EstablishmentId);

                if (establishmentSubscription == null)
                    return BadRequest(new { error = "Deze establishment staat niet in uw lijst van subscriptions" });

                _customerRepository.removeSubscription(customer.UserId, establishmentSubscription);

                return Ok(new { message = "Verwijderd!" });
            }
            //Als we hier zijn is is modelstate niet voldaan dus stuur error 400, slechte aanvraag
            string errorMsg = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(new { error = "De ingevoerde waarden zijn onvolledig of voldoen niet aan de eisen voor een login. Foutboodschap: " + errorMsg });
        }

    }
}