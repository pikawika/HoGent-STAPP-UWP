using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Models;
using Windows.Security.Credentials;

namespace uwp_app_aalst_groep_a3.Network
{
    public class NetworkAPI
    {
        // The HttpClient used for all REST API calls
        private HttpClient client { get; }
        private PasswordVault passwordVault = new PasswordVault();

        // The base URL of our backend
        public static string baseUrl { get; } = "https://localhost:44315/";

        public NetworkAPI()
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            client = new HttpClient(httpClientHandler);
        }

        #region AUTHENTICATION
        /* AUTHENTICATION */
        // Sign in
        public async Task<string> SignIn(string username, string password)
        {
            var token = "";
            var login = new { Username = username, Password = password };
            var loginJson = JsonConvert.SerializeObject(login);

            try
            {
                var res = await client.PostAsync(new Uri($"{baseUrl}api/user/login"), new StringContent(loginJson, System.Text.Encoding.UTF8, "application/json"));
                var userToken = JsonConvert.DeserializeObject<UserToken>(res.Content.ReadAsStringAsync().Result);
                token = userToken.Token;
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine($"Er is een error opgetreden tijdens het aanmelden: " +
                                $"{e}");
            }

            return token;
        }

        // Create an account
        public async Task<string> CreateAccount(string firstname, string lastname, string emailaddress, string username, string password, string role)
        {
            var token = "";
            var login = new { FirstName = firstname, LastName = lastname, Email = emailaddress, Login = new { Username = username, Password = password, Role = role } };
            var loginJson = JsonConvert.SerializeObject(login);

            try
            {
                var res = await client.PostAsync(new Uri($"{baseUrl}api/user/"), new StringContent(loginJson, System.Text.Encoding.UTF8, "application/json"));
                var userToken = JsonConvert.DeserializeObject<UserToken>(res.Content.ReadAsStringAsync().Result);
                token = userToken.Token;
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine($"Er is een error opgetreden tijdens het aanmelden: " +
                                $"{e}");
            }

            return token;
        }
        #endregion

        #region USER

        /* USER */
        // Get account details
        public async Task<User> GetUser()
        {
            User user = new User();
            try
            {
                var credentials = passwordVault.Retrieve("Stapp", "Token");
                credentials.RetrievePassword();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credentials.Password);

                var json = await client.GetStringAsync(new Uri($"{baseUrl}api/user/"));

                user = JsonConvert.DeserializeObject<User>(json);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Er is een error opgetreden tijdens het ophalen van de gegevens van de gebruiker: " +
                                $"{e}");
            }
            return user;
        }

        #endregion

        #region CUSTOMER

        // Subscribe to an establishment
        public async Task<string> Subscribe(int establishmentId)
        {
            var data = new { EstablishmentId = establishmentId };
            var dataJson = JsonConvert.SerializeObject(data);
            string errorMessage = null;

            var credentials = passwordVault.Retrieve("Stapp", "Token");
            credentials.RetrievePassword();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credentials.Password);

            try
            {
                var res = await client.PostAsync(new Uri($"{baseUrl}api/customer"), new StringContent(dataJson, System.Text.Encoding.UTF8, "application/json"));
                if (res.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    ErrorMessage message = new ErrorMessage();
                    message = JsonConvert.DeserializeObject<ErrorMessage>(await res.Content.ReadAsStringAsync());
                    errorMessage = message.Error;
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            return errorMessage;
        }

        // Unsubscribe to an establishment
        public async Task<string> Unsubscribe(int establishmentId)
        {
            string errorMessage = null;

            var credentials = passwordVault.Retrieve("Stapp", "Token");
            credentials.RetrievePassword();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credentials.Password);

            try
            {
                var res = await client.DeleteAsync(new Uri($"{baseUrl}api/customer/{establishmentId}"));
                if (res.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    ErrorMessage message = new ErrorMessage();
                    message = JsonConvert.DeserializeObject<ErrorMessage>(await res.Content.ReadAsStringAsync());
                    errorMessage = message.Error;
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            return errorMessage;
        }

        public async Task<List<Establishment>> GetSubscriptions()
        {
            List<Establishment> establishments = new List<Establishment>();
            var credentials = passwordVault.Retrieve("Stapp", "Token");
            credentials.RetrievePassword();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credentials.Password);
            try
            {
                var json = await client.GetStringAsync(new Uri($"{baseUrl}api/customer/subscriptions"));
                establishments = JsonConvert.DeserializeObject<List<Establishment>>(json);
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine($"Er is een error opgetreden tijdens het " +
                                $"ophalen van alle subscriptions uit de databank: " +
                                $"{e}");
            }
            return establishments;
        }

        #endregion

        #region ESTABLISHMENT

        /* ESTABLISHMENTS */
        // Get all establishments
        public async Task<List<Establishment>> GetAllEstablishments()
        {
            List<Establishment> establishments = new List<Establishment>();
            try
            {
                var json = await client.GetStringAsync(new Uri($"{baseUrl}api/establishment"));
                establishments = JsonConvert.DeserializeObject<List<Establishment>>(json);
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine($"Er is een error opgetreden tijdens het " +
                                $"ophalen van alle establishments uit de databank: " +
                                $"{e}");
            }
            return establishments;
        }

        // Get establishment by id
        public async Task<Establishment> GetEstablishmentById(int id)
        {
            Establishment establishment = new Establishment();
            try
            {
                var json = await client.GetStringAsync(new Uri($"{baseUrl}api/establishment/{id}"));
                establishment = JsonConvert.DeserializeObject<Establishment>(json);

            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine($"Er is een error opgetreden tijdens het " +
                                $"ophalen van een specifiek establishment uit de databank: " +
                                $"{e}");
            }
            return establishment;
        }

        #endregion

        #region EVENT

        /* EVENTS */
        // Get all events
        public async Task<List<Event>> GetAllEvents()
        {
            List<Event> events = new List<Event>();
            try
            {
                var json = await client.GetStringAsync(new Uri($"{baseUrl}api/event"));
                events = JsonConvert.DeserializeObject<List<Event>>(json);
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine($"Er is een error opgetreden tijdens het " +
                                $"ophalen van alle events uit de databank: " +
                                $"{e}");
            }
            return events;
        }

        // Get event by id
        public async Task<Event> GetEventById(int id)
        {
            Event ev = new Event();
            try
            {
                var json = await client.GetStringAsync(new Uri($"{baseUrl}api/event/{id}"));
                ev = JsonConvert.DeserializeObject<Event>(json);

            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine($"Er is een error opgetreden tijdens het " +
                                $"ophalen van een specifiek event uit de databank: " +
                                $"{e}");
            }
            return ev;
        }

        #endregion

        #region PROMOTION

        /* PROMOTIONS */
        // Get all promotions
        public async Task<List<Promotion>> GetAllPromotions()
        {
            List<Promotion> promotions = new List<Promotion>();
            try
            {
                var json = await client.GetStringAsync(new Uri($"{baseUrl}api/promotion"));
                promotions = JsonConvert.DeserializeObject<List<Promotion>>(json);
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine($"Er is een error opgetreden tijdens het " +
                                $"ophalen van alle promotions uit de databank: " +
                                $"{e}");
            }
            return promotions;
        }

        // Get promotion by id
        public async Task<Promotion> GetPromotionById(int id)
        {
            Promotion promotion = new Promotion();
            try
            {
                var json = await client.GetStringAsync(new Uri($"{baseUrl}api/promotion/{id}"));
                promotion = JsonConvert.DeserializeObject<Promotion>(json);
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine($"Er is een error opgetreden tijdens het " +
                                $"ophalen een specifieke promotion uit de databank: " +
                                $"{e}");
            }
            return promotion;
        }

        #endregion

    }
}
