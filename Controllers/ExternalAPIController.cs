using cnxdevsoftware.Models;
using cnxdevsoftware.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace cnxdevsoftware.Controllers
{
    public class ExternalAPIController : Controller
    {
        private readonly ClientCredentialsService _credentialsService;
        private string SpotifyToken => " https://accounts.spotify.com/api/token";
        private string ApiUrl => " https://api.spotify.com/v1/browse/categories";

        public ExternalAPIController(ClientCredentialsService clientCredentialsService)
        {
            this._credentialsService = clientCredentialsService;

        }
        public async Task<IActionResult>  Index()
        {
            string token_type = string.Empty;
            string access_token = string.Empty;
            int Second = 0;

            var (isErr, errMsg, token) = await _credentialsService.GeTokenFormDb(); 

            if(isErr)
                return View(errMsg);

            if(token == null)
            {
                var credentials = await SpotifyAccessToken();
                token_type = credentials.token_type;
                access_token = credentials.access_token;
            }
            else
            {
                var DateNow = DateTime.Now;
                var DateStart = token.LastUpdate;

                var SumDate = DateNow - DateStart;
                Second = (int)SumDate.TotalSeconds;
            }


            if (Second > token.ExpiresIn )
            {
                var credentials = await SpotifyAccessToken();
                token_type = credentials.token_type;
                access_token = credentials.access_token;
            }
            else
            {
                token_type = token.TokenType;
                access_token = token.AccessToken1;
            }

           
            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token_type, access_token);

            HttpResponseMessage result = await client.GetAsync(ApiUrl);

            var Content = await result.Content.ReadAsStringAsync();
            ViewBag.JsonData = JValue.Parse(Content).ToString(Formatting.Indented);
            return View();
            
        }

        private async Task<ClientCredentials?> SpotifyAccessToken()
        {
            ClientCredentials? credentials = new ();
            string clientId = "ad61ffe2b1d8438a8c28865b269c3198";
            string clientSecret = "2a980ffcb0044ad6bcbcfc353be3c142";

            string base64Auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));

            using (HttpClient client = new())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Auth);

                var requestData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                });

                HttpResponseMessage tokenResponse = await client.PostAsync(SpotifyToken, requestData);

                if (tokenResponse.IsSuccessStatusCode)
                {
                    string tokenContent = await tokenResponse.Content.ReadAsStringAsync();
                    
                    if(!string.IsNullOrEmpty(tokenContent))
                    {
                        credentials = Newtonsoft.Json.JsonConvert.DeserializeObject<ClientCredentials>(tokenContent);
                        await _credentialsService.SaveAccessToken(credentials);
                    }
                        
                }
            }
            return credentials;
        }
    }
}
