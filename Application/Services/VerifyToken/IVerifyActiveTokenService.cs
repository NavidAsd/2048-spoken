using Shared;

namespace Application.Services.VerifyToken
{
    public interface IVerifyActiveTokenService
    {
        Task<ResultMessageApi> VerifyToeknAsync(string token);
    }
    public class VerifyActiveTokenService : IVerifyActiveTokenService
    {
        async Task<ResultMessageApi> IVerifyActiveTokenService.VerifyToeknAsync(string token)
        {
            string pairs = $"authorization:{token}";

            StringContent content = new StringContent(pairs, System.Text.Encoding.UTF8, "application/json");

            var client = new HttpClient();

            var response = await client.PostAsync("https://ent.persianspeech.com/api/auth/login", content);

            var responseString = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return new ResultMessageApi
                {
                    Success = true,
                    StatusCode = response.StatusCode,
                };
            }
            return new ResultMessageApi
            {
                Success = false,
                StatusCode = response.StatusCode,
            };
        }
    }
}
