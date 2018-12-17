using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Models;
using Windows.Security.Credentials;
using uwp_app_aalst_groep_a3.Network.requests;
using uwp_app_aalst_groep_a3.Network.responses;

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


        public async Task SaveSubscribedEstablishemtsAsync(List<Establishment> establishments)
        {
            string json = JsonConvert.SerializeObject(establishments.ToArray());

            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile = await storageFolder.CreateFileAsync("subscribed.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            JsonSerializer serializer = new JsonSerializer();
            await Windows.Storage.FileIO.WriteTextAsync(sampleFile,JsonConvert.SerializeObject(establishments));
        }
        
        public async Task<List<Establishment>> GetSubscribedEstablishmentsAsync()
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile = await storageFolder.GetFileAsync("subscribed.txt");
            string text = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
            return JsonConvert.DeserializeObject<List<Establishment>>(text);
        }

        public async Task<bool> CheckSubbedDifferenceByJSONAsync(List<Establishment> new_establishments)
        {
            string json = JsonConvert.SerializeObject(new_establishments.ToArray());

            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile = await storageFolder.GetFileAsync("subscribed.txt");
            string text = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
            return text.Equals(json);
        }

        /* AUTHENTICATION */
        // Sign in
        public async Task<string> SignIn(string username, string password)
        {
            var token = "";
            var login = new { Username = username, Password = password };
            var loginJson = JsonConvert.SerializeObject(login);

            try
            {
                var res = await client.PostAsync(new Uri($"{ baseUrl}api/user/login"), new StringContent(loginJson, System.Text.Encoding.UTF8, "application/json"));
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
            user.UserId = -2;
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

        // Get subscriptions from a customer
        public async Task<List<Establishment>> GetSubscriptions()
        {
            List<Establishment> establishments = new List<Establishment>();
            try
            {
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
            catch
            {
                return establishments;
            }
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

        #region MERCHANT

        #region MERCHANT COMPANIES
        // Haal alle companies van aangemelde handelaar op
        public async Task<(List<Company>, string)> GetCompanies()
        {
            string errorMessage = null;
            List<Company> companiesFromMerchant = new List<Company>();

            var credentials = passwordVault.Retrieve("Stapp", "Token");
            credentials.RetrievePassword();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credentials.Password);

            try
            {
                var res = await client.GetAsync(new Uri($"{baseUrl}api/merchant/company"));
                if (res.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    ErrorMessage message = new ErrorMessage();
                    message = JsonConvert.DeserializeObject<ErrorMessage>(await res.Content.ReadAsStringAsync());
                    errorMessage = message.Error;
                }
                else
                {
                    companiesFromMerchant = JsonConvert.DeserializeObject<List<Company>>(await res.Content.ReadAsStringAsync());
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            return (companiesFromMerchant, errorMessage);
        }

        // Voegt nieuwe company voor ingelogde merchant toe
        public async Task<(string, bool)> addCompany(string naam)
        {
            string message;
            bool isSuccess = false;

            var newCompany = new { name = naam };
            var newCompanyJson = JsonConvert.SerializeObject(newCompany);

            var credentials = passwordVault.Retrieve("Stapp", "Token");
            credentials.RetrievePassword();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credentials.Password);

            try
            {
                var res = await client.PostAsync(new Uri($"{baseUrl}api/company"), new StringContent(newCompanyJson, System.Text.Encoding.UTF8, "application/json"));
                if (res.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    message = JsonConvert.DeserializeObject<ErrorMessage>(await res.Content.ReadAsStringAsync()).Error;
                }
                else
                {
                    message = JsonConvert.DeserializeObject<SuccesMessage>(await res.Content.ReadAsStringAsync()).Bericht;
                    isSuccess = true;
                }
            }
            catch (Exception e)
            {
                message = e.Message;
            }

            return (message, isSuccess);
        }

        // edit company voor ingelogde merchant
        public async Task<(string, bool)> editCompany(int companyId, string naam = "")
        {
            string message;
            bool isSuccess = false;

            if (string.IsNullOrEmpty(naam))
            {
                return ("Gelieve een nieuwe naam in te voeren", false);
            }

            var editedCompany = new { name = naam };
            var editedCompanyJson = JsonConvert.SerializeObject(editedCompany);

            var credentials = passwordVault.Retrieve("Stapp", "Token");
            credentials.RetrievePassword();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credentials.Password);

            try
            {
                var res = await client.PutAsync(new Uri($"{baseUrl}api/company/{companyId}"), new StringContent(editedCompanyJson, System.Text.Encoding.UTF8, "application/json"));
                if (res.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    message = JsonConvert.DeserializeObject<ErrorMessage>(await res.Content.ReadAsStringAsync()).Error;
                }
                else
                {
                    message = JsonConvert.DeserializeObject<SuccesMessage>(await res.Content.ReadAsStringAsync()).Bericht;
                    isSuccess = true;
                }
            }
            catch (Exception e)
            {
                message = e.Message;
            }

            return (message, isSuccess);
        }

        // delete company voor ingelogde merchant
        public async Task<(string, bool)> deleteCompany(int companyId)
        {
            string message;
            bool isSuccess = false;

            var credentials = passwordVault.Retrieve("Stapp", "Token");
            credentials.RetrievePassword();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credentials.Password);

            try
            {
                var res = await client.DeleteAsync(new Uri($"{baseUrl}api/company/{companyId}"));
                if (res.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    message = JsonConvert.DeserializeObject<ErrorMessage>(await res.Content.ReadAsStringAsync()).Error;
                }
                else
                {
                    message = JsonConvert.DeserializeObject<SuccesMessage>(await res.Content.ReadAsStringAsync()).Bericht;
                    isSuccess = true;
                }
            }
            catch (Exception e)
            {
                message = e.Message;
            }

            return (message, isSuccess);
        }
        #endregion

        #region MERCHANT ESTABLISHMENTS
        // Voegt nieuwe establishment voor ingelogde merchant toe
        public async Task<(string, bool)> addEstablishment(EstablishmentRequest newEstablishmentRequest)
        {
            string message;
            bool isSuccess = false;

            var newCompanyJson = JsonConvert.SerializeObject(newEstablishmentRequest);

            var credentials = passwordVault.Retrieve("Stapp", "Token");
            credentials.RetrievePassword();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credentials.Password);

            try
            {
                var res = await client.PostAsync(new Uri($"{baseUrl}api/establishment"), new StringContent(newCompanyJson, System.Text.Encoding.UTF8, "application/json"));
                if (res.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    message = JsonConvert.DeserializeObject<ErrorMessage>(await res.Content.ReadAsStringAsync()).Error;
                }
                else
                {
                    message = JsonConvert.DeserializeObject<SuccesMessage>(await res.Content.ReadAsStringAsync()).Bericht;
                    isSuccess = true;
                }
            }
            catch (Exception e)
            {
                message = e.Message;
            }

            return (message, isSuccess);
        }

        // edit establishment voor ingelogde merchant
        public async Task<(string, bool)> editEstablishment(int establishmentId, EstablishmentRequest editEstablishmentRequest)
        {
            string message;
            bool isSuccess = false;

            var editedEstablishmentJson = JsonConvert.SerializeObject(editEstablishmentRequest);

            var credentials = passwordVault.Retrieve("Stapp", "Token");
            credentials.RetrievePassword();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credentials.Password);

            try
            {
                var res = await client.PutAsync(new Uri($"{baseUrl}api/establishment/{establishmentId}"), new StringContent(editedEstablishmentJson, System.Text.Encoding.UTF8, "application/json"));
                if (res.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    message = JsonConvert.DeserializeObject<ErrorMessage>(await res.Content.ReadAsStringAsync()).Error;
                }
                else
                {
                    message = JsonConvert.DeserializeObject<SuccesMessage>(await res.Content.ReadAsStringAsync()).Bericht;
                    isSuccess = true;
                }
            }
            catch (Exception e)
            {
                message = e.Message;
            }

            return (message, isSuccess);
        }

        // delete establishment voor ingelogde merchant
        public async Task<(string, bool)> deleteEstablishment(int EstablishmentId)
        {
            string message;
            bool isSuccess = false;

            var credentials = passwordVault.Retrieve("Stapp", "Token");
            credentials.RetrievePassword();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credentials.Password);

            try
            {
                var res = await client.DeleteAsync(new Uri($"{baseUrl}api/establishment/{EstablishmentId}"));
                if (res.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    message = JsonConvert.DeserializeObject<ErrorMessage>(await res.Content.ReadAsStringAsync()).Error;
                }
                else
                {
                    message = JsonConvert.DeserializeObject<SuccesMessage>(await res.Content.ReadAsStringAsync()).Bericht;
                    isSuccess = true;
                }
            }
            catch (Exception e)
            {
                message = e.Message;
            }

            return (message, isSuccess);
        }

        #endregion

        #region MERCHANT PROMOTIONS

        #endregion

        #region MERCHANT EVENTS

        #endregion

        #endregion

    }
}
