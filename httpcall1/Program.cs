using System;
using System.Net.Http;

namespace httpcall1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Making API Call...");
                GetHttp();
                Console.ReadLine();

            }
            catch (Exception)
            {
                Console.WriteLine("Error making API Call...");
            }
        }

        private static void GetHttp()
        {
            var url = "some url here";
           
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(url);
                HttpResponseMessage response = client.GetAsync("").Result;
                response.EnsureSuccessStatusCode();
                string result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Result: " + result);
            }
        }
    }
}
