using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ApiConsumer
{
    class Program
    {
        static void Main(string[] args)
        {

            
            Task POST_TOKEN =  new Task(HTTP_POST_TOKEN);
            POST_TOKEN.Start();


            Console.ReadLine();
        }

        private static async void HTTP_POST_TOKEN()
        {
            
            var TARGETURL = "";

            string grant_type = "GranType";
            string usuario = "USuario";
            string password = "Password";


            var form = new Dictionary<string, string>
             {
                 {"grant_type", grant_type},
                 {"client_id", usuario},
                 {"client_secret", password},
             };



            Console.WriteLine("POST: + " + TARGETURL);

            // ... POST HTTP.            

            HttpClient client = new HttpClient();
            HttpResponseMessage tokenResponse = await client.PostAsync(TARGETURL, new FormUrlEncodedContent(form));
            HttpContent content = tokenResponse.Content;
            var jsonContent = await tokenResponse.Content.ReadAsStringAsync();
            Response res = JsonConvert.DeserializeObject<Response>(jsonContent);

            Console.WriteLine("Response StatusCode: " + (int)tokenResponse.StatusCode);

            string result = await content.ReadAsStringAsync();


            Console.WriteLine(res.Access_token + " " + result);

         
            await HTTP_POST_NOTIFICACIONES(res, GetPerson());


        }


        static async Task HTTP_POST_NOTIFICACIONES(Response responses, Person person)
        {
            var NOTIFICACIONURL = "";

            Console.WriteLine("POST: + " + NOTIFICACIONURL);

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", responses.Access_token);

            var json = JsonConvert.SerializeObject(person);


            var data = new StringContent(json, Encoding.UTF8, "application /application/json");

            var response = await client.PostAsync(NOTIFICACIONURL, data);

            Console.WriteLine(response.StatusCode + " " + response.Headers + "  Contenido : " + response.Content);

        }

        static Person GetPerson()
        {
            return new Person
            {
                ID = 1,
                NAME = "Fryann Harold",
                LASTNAME = "Martinez Hernandez",
                DATEOFBIRTH = "08/05/1994"
            };
        }
    }

    public class Response
    {
        public string Access_token { get; set; }
        public int Expires_in { get; set; }
        public string Token_type { get; set; }
    }

    public class Person
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public string LASTNAME { get; set; }
        public string DATEOFBIRTH { get; set; }
    }
}
