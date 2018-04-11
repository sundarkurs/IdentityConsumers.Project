using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ID.ResourceClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string accessToken = RequestToken();
            Console.WriteLine("Access Token: {0}", accessToken);

            string apiResponse = CallApi(accessToken);
            Console.WriteLine("Api Response: {0}", apiResponse);

            Console.ReadLine();
        }

        public static string RequestToken()
        {
            string accessToken = string.Empty;
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.BaseAddress = new Uri("https://qa.idm.globusfamily.com.au/api/Login");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var request = new
            {
                UserName = "wyd4@yopmail.com",
                Password = "abc123"
            };

            string jsonRequest = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var response = client.SendAsync(new HttpRequestMessage
            {
                Method = new HttpMethod("POST"),
                RequestUri = client.BaseAddress,
                Content = content
            }).Result;

            if (response.IsSuccessStatusCode)
            {
                dynamic id3TokenResponse = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
                accessToken = id3TokenResponse.AccessToken.Value.ToString();
            }

            return accessToken;
        }

        public static string CallApi(string accessToken)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://..."); // Pass in the IDS3 secured web-api URL
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers.Add("Authorization", "Bearer " + accessToken);

            string result;
            WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }
    }
}
