using Shared;
using Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization.Metadata;

namespace Application.Services.AccountLogin
{
    public interface IGenerateNewTokenService
    {
        Task<ResultMessageApi<Account>> ExecuteAsync(RequestGenerateNewTokenDto request);
    }
    public class RequestGenerateNewTokenDto
    {
        public string username { set; get; }
        public string password { set; get; }
    }
    public class GenerateNewTokenService : IGenerateNewTokenService
    {
        async Task<ResultMessageApi<Account>> IGenerateNewTokenService.ExecuteAsync(RequestGenerateNewTokenDto request)
        {

            string pairs = JsonConvert.SerializeObject(request);

            StringContent content = new StringContent(pairs, System.Text.Encoding.UTF8, "application/json");

            var client = new HttpClient();

            var response = await client.PostAsync("https://ent.persianspeech.com/api/auth/login", content);

            var responseString = await response.Content.ReadAsStringAsync();
            var json = JToken.Parse(responseString);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return new ResultMessageApi<Account>
                {
                    Success = true,
                    StatusCode = response.StatusCode,
                    Enything = new Account
                    {
                        token = (string)json["token"],
                        username = request.username,
                        password = request.password
                    },
                };
            }
            return new ResultMessageApi<Account>
            {
                Success = false,
                StatusCode = response.StatusCode,
                Message = "login failed"
            };
        }
    }
}
