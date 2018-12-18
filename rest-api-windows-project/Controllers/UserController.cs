using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using stappBackend.Models;
using stappBackend.Models.IRepositories;
using stappBackend.Models.ViewModels.User;


namespace stappBackend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IConfiguration _config;
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;

        public UserController(IConfiguration config, IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _config = config;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        //returnt alle gegevens van een USER
        [HttpGet]
        public IActionResult Get()
        {
            if (User.FindFirst("userId")?.Value == null)
                return BadRequest(new { error = "De voorziene token voldoet niet aan de eisen." });

            User user =_userRepository.getById(int.Parse(User.FindFirst("userId")?.Value));

            if (user != null){
                return Ok(user);
            }
            return BadRequest(new { error = "Geen gebruiker gevonden met de opgegeven id." });
        }

        [HttpPost("CheckEmailExists")]
        [AllowAnonymous]
        public IActionResult CheckEmailExists([FromBody]CheckEmailViewModel checkRequest)
        {
            if (ModelState.IsValid)
            {
                return Ok(new { emailBestaat = CheckEmailExists(checkRequest.Email) });
            }
            //Als we hier zijn is is modelstate niet voldaan dus stuur error 400, slechte aanvraag
            string errorMsg = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(new { error = errorMsg });
        }

        [HttpPost("CheckUsernameExists")]
        [AllowAnonymous]
        public IActionResult CheckUsernameExist([FromBody]CheckUsernameViewModel checkRequest)
        {
            if (ModelState.IsValid)
            {
                return Ok(new { gebruikersnaamBestaat = CheckUsernameExists(checkRequest.Username) });
            }
            //Als we hier zijn is is modelstate niet voldaan dus stuur error 400, slechte aanvraag
            string errorMsg = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(new { error = errorMsg });
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody]LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                var user = Login(login.Username.ToLower(), login.Password);

                
                //user gevonden dus aangemeld!
                if (user != null)
                {
                    var tokenString = BuildToken(user);
                    return Ok(new { token = tokenString });
                }

                //geen user gevonden
                if (CheckUsernameExists(login.Username))
                {
                    return BadRequest(new { error = "Incorrect wachtwoord." });
                }
                else
                {
                    return BadRequest(new { error = "Incorrecte gebruikersnaam." });
                }
            }
            //Als we hier zijn is is modelstate niet voldaan dus stuur error 400, slechte aanvraag
            string errorMsg = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(new { error = "De ingevoerde waarden zijn onvolledig of voldoen niet aan de eisen voor een login. Foutboodschap: " + errorMsg });
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post([FromBody]RegisterUserViewModel userRequest)
        {
            //gebruiker model moet bij eender welk type valid zijn
            if (ModelState.IsValid)
            {
                if (CheckEmailExists(userRequest.Email))
                    return BadRequest(new { error = "Het gekozen emailadres is reeds gekoppeld aan een account" });

                if (CheckUsernameExists(userRequest.Login.Username))
                    return BadRequest(new { error = "De gekozen gebruikersnaam is reeds gekoppeld aan een account" });

                switch (userRequest.Login.Role.ToLower())
                {
                    case "customer":
                        return RegisterCustomer(userRequest);
                    case "merchant":
                        return RegisterMerchant(userRequest);
                    default:
                        return BadRequest(new { error = "Het soort gebruiker dat u wilt aanmaken bestaat niet (incorrecte rol)" });
                }
            }
            //Als we hier zijn is is modelstate niet voldaan dus stuur error 400, slechte aanvraag
            string errorMsg = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(new { error = "De ingevoerde waarden zijn onvolledig of voldoen niet aan de eisen voor een gebruiker. Foutboodschap: " + errorMsg });
        }

        [HttpPost("ChangePassword")]
        public IActionResult ChangePassword([FromBody]ChangePasswordViewModel changeRequest)
        {
            if (ModelState.IsValid)
            {
                //token heeft geen id => fout met token!!
                if (User.FindFirst("userId")?.Value == null)
                    return BadRequest(new { error = "De voorziene token voldoet niet aan de eisen." });

                ChangePassword(int.Parse(User.FindFirst("userId")?.Value), changeRequest.Password);

                return Ok(new { bericht = "Uw wachtwoord werd succesvol gewijzigd." });
            }
            //Als we hier zijn is is modelstate niet voldaan dus stuur error 400, slechte aanvraag
            string errorMsg = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(new { error = "De ingevoerde waarden zijn onvolledig of voldoen niet aan de eisen voor het wijzigen van uw wachwoord. Foutboodschap: " + errorMsg });
        }

        [HttpPost("ChangeUsername")]
        public IActionResult ChangeUsername([FromBody]ChangeUsernameViewModel changeRequest)
        {
            if (ModelState.IsValid)
            {
                //token heeft geen id => fout met token!!
                if (User.FindFirst("userId")?.Value == null)
                    return BadRequest(new { error = "De voorziene token voldoet niet aan de eisen." });

                if (CheckUsernameExists(changeRequest.Username))
                {
                    return BadRequest(new { error = "Deze gebruikersnaam is reeds in gebruik." });
                }

                ChangeUsername(int.Parse(User.FindFirst("userId")?.Value), changeRequest.Username);

                return Ok(new { bericht = "Uw gebruikersnaam werd succesvol gewijzigd." });
            }
            //Als we hier zijn is is modelstate niet voldaan dus stuur error 400, slechte aanvraag
            string errorMsg = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(new { error = "De ingevoerde waarden zijn onvolledig of voldoen niet aan de eisen voor het wijzigen van uw gebruikersnaam. Foutboodschap: " + errorMsg });
        }

        private IActionResult RegisterCustomer(RegisterUserViewModel customerRequest)
        {
            Customer newCustomer = new Customer();

            newCustomer.Email = customerRequest.Email;
            newCustomer.FirstName = customerRequest.FirstName;
            newCustomer.LastName = customerRequest.LastName;

            newCustomer.Login.Username = customerRequest.Login.Username;
            newCustomer.Login.Salt = MakeSalt();
            newCustomer.Login.Hash = MakeHash(customerRequest.Login.Password, newCustomer.Login.Salt);
            newCustomer.Login.Role = _roleRepository.GetByName(customerRequest.Login.Role);

            newCustomer.Login.User = newCustomer;

            _userRepository.Register(newCustomer);

            return Ok(new { token = BuildToken(newCustomer) });
        }

        private IActionResult RegisterMerchant(RegisterUserViewModel merchantRequest)
        {
            Merchant newMerchant = new Merchant();

            newMerchant.Email = merchantRequest.Email;
            newMerchant.FirstName = merchantRequest.FirstName;
            newMerchant.LastName = merchantRequest.LastName;

            newMerchant.Login.Username = merchantRequest.Login.Username;
            newMerchant.Login.Salt = MakeSalt();
            newMerchant.Login.Hash = MakeHash(merchantRequest.Login.Password, newMerchant.Login.Salt);
            newMerchant.Login.Role = _roleRepository.GetByName(merchantRequest.Login.Role);

            newMerchant.Login.User = newMerchant;

            _userRepository.Register(newMerchant);

            return Ok(new { token = BuildToken(newMerchant) });
        }

        private bool CheckEmailExists(string email)
        {
            return _userRepository.EmailExists(email.ToLower());
        }

        private bool CheckUsernameExists(string username)
        {
            return _userRepository.UsernameExists(username.ToLower());
        }

        private string BuildToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                expires: DateTime.Now.AddYears(10),
                signingCredentials: creds);

            token.Payload["userId"] = user.UserId;
            token.Payload["username"] = user.Login.Username;
            token.Payload["customRole"] = user.Login.Role.Name;

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User Login(string username, string password)
        {
            byte[] salt = _userRepository.GetSalt(username);

            if (salt == null)
            {
                return null;
            }

            string hash = MakeHash(password, salt);

            //probeer login met de hash, indien foutief wachtwoord returnt dit ook null
            return _userRepository.Login(username, hash);
        }

        private void ChangePassword(int userId, string newPassword)
        {
            // nieuwe salt voor beveiligingsreden
            byte[] newSalt = MakeSalt();

            string newHash = MakeHash(newPassword, newSalt);

            _userRepository.ChangePassword(userId, newSalt, newHash);
        }

        private void ChangeUsername(int userId, string newUsername)
        {
            _userRepository.ChangeUsername(userId, newUsername);
        }

        private byte[] MakeSalt()
        {
            //maak een salt adhv een random nummer
            byte[] salt = new byte[128 / 8];
            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt);
            }

            return salt;
        }

        private string MakeHash(string pasword, byte[] salt)
        {
            // build in hasher van .net (beste combo veilig en snel volgens microsoft documentatie)
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: pasword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }
    }
}