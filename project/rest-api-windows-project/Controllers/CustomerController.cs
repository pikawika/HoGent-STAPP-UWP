﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using stappBackend.Models;
using stappBackend.Models.IRepositories;
using stappBackend.Models.ViewModels.Customer;

namespace stappBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IEstablishmentRepository _establishmentRepository;

        public CustomerController(ICustomerRepository customerRepository, IEstablishmentRepository establishmentRepository)
        {
            _customerRepository = customerRepository;
            _establishmentRepository = establishmentRepository;
        }

        [HttpPost]
        public IActionResult Post([FromBody]ModifySubscriptionViewModel addSubscriptionViewModel)
        {
            if (ModelState.IsValid)
            {
                if (IsMerchant())
                    return BadRequest(new { error = "Handelaars kunnen zich niet abonneren op andere handelaars." });
                    
                if (!IsCustomer())
                    return BadRequest(new { error = "De voorziene token voldoet niet aan de eisen." });

                Establishment establishment = _establishmentRepository.getById(addSubscriptionViewModel.EstablishmentId);

                if (establishment == null)
                    return BadRequest(new { error = "Het opgegeven vestiging bestaat niet." });

                Customer customer = _customerRepository.getById(int.Parse(User.FindFirst("userId")?.Value));

                if (customer.EstablishmentSubscriptions.Any(es => es.EstablishmentId == establishment.EstablishmentId))
                    return BadRequest(new { error = "U bent reeds geabonneerd op deze vestiging." });

                EstablishmentSubscription establishmentSubscription = new EstablishmentSubscription() { Customer = customer, Establishment = establishment, DateAdded = DateTime.Now, EstablishmentId = establishment.EstablishmentId };

                _customerRepository.addSubscription(customer.UserId, establishmentSubscription);

                return Ok(new { message = "U bent succesvol geabonneerd op deze vestiging!" });
            }
            //Als we hier zijn is is modelstate niet voldaan dus stuur error 400, slechte aanvraag
            string errorMsg = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(new { error = "De ingevoerde waarden zijn onvolledig of voldoen niet aan de eisen voor een login. Foutboodschap: " + errorMsg });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                if (IsMerchant())
                    return BadRequest(new { error = "Handelaars kunnen zich niet abonneren op andere handelaars." });
                    
                if (!IsCustomer())
                    return BadRequest(new { error = "De voorziene token voldoet niet aan de eisen." });

                Establishment establishment = _establishmentRepository.getById(id);

                if (establishment == null)
                    return BadRequest(new { error = "Geen vestiging met de meegegeven id." });

                Customer customer = _customerRepository.getById(int.Parse(User.FindFirst("userId")?.Value));

                EstablishmentSubscription establishmentSubscription =
                    customer.EstablishmentSubscriptions.SingleOrDefault(
                        es => es.EstablishmentId == establishment.EstablishmentId);

                if (establishmentSubscription == null)
                    return BadRequest(new { error = "Deze vestiging behoord niet tot u." });

                _customerRepository.removeSubscription(customer.UserId, establishmentSubscription);

                return Ok(new { message = "Verwijderd!" });
            }
            //Als we hier zijn is is modelstate niet voldaan dus stuur error 400, slechte aanvraag
            string errorMsg = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(new { error = "De ingevoerde waarden zijn onvolledig of voldoen niet aan de eisen voor een login. Foutboodschap: " + errorMsg });
        }

        //returnt alle events en promotions van een customer zijn subscriptions 
        [HttpGet("subscriptions")]
        public IActionResult Get()
        {
            if (!IsCustomer())
                return BadRequest(new { error = "De voorziene token voldoet niet aan de eisen." });


            List<Establishment> subscriptions = _customerRepository.GetEstablishmentSubscriptions(int.Parse(User.FindFirst("userId")?.Value));

            return Ok(subscriptions);
        }

        private bool IsCustomer()
        {
            return User.FindFirst("userId")?.Value != null &&
                   User.FindFirst("customRole")?.Value.ToLower() == "customer";
        }
        
        private bool IsMerchant()
        {
            return User.FindFirst("userId")?.Value != null &&
                   User.FindFirst("customRole")?.Value.ToLower() == "merchant";
        }

    }
}