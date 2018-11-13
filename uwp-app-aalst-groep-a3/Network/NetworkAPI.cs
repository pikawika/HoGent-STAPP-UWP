using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Models;
using uwp_app_aalst_groep_a3.Models.Domain;

namespace uwp_app_aalst_groep_a3.Network
{
    public static class NetworkAPI
    {
        // The HttpClient used by all methods
        private static HttpClient client { get; } = new HttpClient();

        // The base URL of our backend
        private static String baseUrl { get; } = "http://localhost:53128/api";

        /* ESTABLISHMENTS */
        // Get all establishments
        public static async Task<ObservableCollection<Establishment>> GetAllEstablishments()
        {
            ObservableCollection<Establishment> establishments = new ObservableCollection<Establishment>();
            try
            {
                var json = await client.GetStringAsync(new Uri($"{baseUrl}/establishment"));
                establishments =  JsonConvert.DeserializeObject<ObservableCollection<Establishment>>(json);
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine($"Er is een error opgetreden tijdens het" +
                                $"ophalen van alle establishments uit de databank:" +
                                $"{e}");
            }
            return establishments;
        }

        // Get establishment by id
        public static async Task<Establishment> GetEstablishmentById(int id)
        {
            Establishment establishment = new Establishment();
            try
            {
                var json = await client.GetStringAsync(new Uri($"{baseUrl}/establishment/{id}"));
                establishment = JsonConvert.DeserializeObject<Establishment>(json);
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine($"Er is een error opgetreden tijdens het" +
                                $"ophalen van een specifiek establishment uit de databank:" +
                                $"{e}");
            }
            return establishment;
        }

        /* PROMOTIONS */
        // Get all promotions
        public static async Task<ObservableCollection<Promotion>> GetAllPromotions()
        {
            ObservableCollection<Promotion> promotions = new ObservableCollection<Promotion>();
            try
            {
                var json = await client.GetStringAsync(new Uri($"{baseUrl}/promotion"));
                promotions = JsonConvert.DeserializeObject<ObservableCollection<Promotion>>(json);
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine($"Er is een error opgetreden tijdens het" +
                                $"ophalen van alle promotions uit de databank:" +
                                $"{e}");
            }
            return promotions;
        }

        // Get promotion by id
        public static async Task<Promotion> GetPromotionById(int id)
        {
            Promotion promotion = new Promotion();
            try
            {
                var json = await client.GetStringAsync(new Uri($"{baseUrl}/promotion/{id}"));
                promotion = JsonConvert.DeserializeObject<Promotion>(json);
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine($"Er is een error opgetreden tijdens het" +
                                $"ophalen een specifieke promotion uit de databank:" +
                                $"{e}");
            }
            return promotion;
        }

    }
}
