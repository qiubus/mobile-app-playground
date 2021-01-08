using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MobileAppPlayground.Droid.Services
{
    public class MobileApiService
    {
        public async Task<bool> SendAndroidTokenToApiAsync(string userId, string token)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
#if DEBUG
            // for local host
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
#endif

            using (var client = new HttpClient(clientHandler))
            {
                var data = new
                {
                    clientId = userId,
                    token = token
                };

                var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

                var response = await client.PutAsync("https://172.17.2.159:3443/AppTokenRegister", content);

                return response.IsSuccessStatusCode;
            }
        }
    }
}