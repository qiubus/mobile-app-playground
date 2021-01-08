//using System.Net.Http;
//using System.Runtime.Serialization.Json;
//using System.Text.Json;
//using System.Threading.Tasks;
//using Xamarin.Forms.Internals;

//namespace MobileAppPlayground.Services
//{
//    public class MobileApiService
//    {
//        public async Task<bool> SendAndroidTokenToApiAsync(string userId, string token)
//        {
//            HttpClientHandler clientHandler = new HttpClientHandler();

//#if DEBUG
//            // for local host
//            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
//#endif

//            using (var client = new HttpClient(clientHandler))
//            {
//                var data = new
//                {
//                    userId = userId,
//                    appToken = token
//                };

//                var content = new StringContent(JsonSerializer.Serialize(data));

//                var response = await client.PutAsync("https://172.17.2.159:3443/AppTokenRegister", content);

//                return response.IsSuccessStatusCode;
//            }
//        }
//    }
//}