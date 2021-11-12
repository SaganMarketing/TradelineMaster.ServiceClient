using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;

namespace TradelineMaster.ServiceClient
{
    public class Tradeline
    {
        public long Id { get; set; }
        public decimal Price { get; set; }
        public int SpotsAvailable { get; set; }
        public string Lender { get; set; }
        public decimal Limit { get; set; }
        public DateTime DateOpened { get; set; }
        public DateTime StatementDate { get; set; }
        public DateTime PostingDate { get; set; }
        public long CardholderAddressID { get; set; }
        public int Cycles { get; set; }
    }

    class Program
    {
        private const string brokerWebsite = "www.YOURSITE.com";
        private const string userKey = "YOUR API User Key";
        private const string passKey = "YOUR API Pass Key";
        
        private const string requestController = "Tradeline/";
        private const string apiUrl = "https://www.tradelinemaster.com/api/";
        private const int apiVersion = 3;

        static void Main(string[] args)
        {
            List<Tradeline> tradelines;

            var client = new RestClient(apiUrl);

            client.AddDefaultHeader("Accept", "application/json");
            client.AddDefaultHeader("Referrer", brokerWebsite);            
            client.AddDefaultHeader("APIVersion", $"{ apiVersion }");

            var authParameterBytes = System.Text.Encoding.UTF8.GetBytes(userKey + ":" + passKey);
            var authorizationParameter = Convert.ToBase64String(authParameterBytes);
            client.AddDefaultHeader("Authorization", "Basic " + authorizationParameter);

            var request = new RestRequest(requestController, Method.GET);

            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful && response.ResponseStatus == ResponseStatus.Completed)
                tradelines = JsonConvert.DeserializeObject<List<Tradeline>>(response.Content);
            else
                throw new Exception($"Unable to get tradelines. Response Phrase: { response.StatusDescription } Status Code: { response.StatusCode }");

            foreach (var tradeline in tradelines)
            {
                Console.WriteLine(JsonConvert.SerializeObject(tradeline, Formatting.Indented));
            }

            Console.ReadKey();
        }
    }
}
