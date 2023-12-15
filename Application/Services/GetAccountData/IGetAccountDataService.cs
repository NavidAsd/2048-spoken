using Domain.Entities;
using System.Text.Json;

namespace Application.Services.GetAccountData
{
    public interface IGetAccountDataService
    {
        Task<Account> GetAccountDataAsync();
        Task UpdateAccountDataAsync(Account Account);
        Task UpdateTokenAsync(string token);
    }
    public class GetAccountDataService : IGetAccountDataService
    {
        async Task<Account> IGetAccountDataService.GetAccountDataAsync()
        {
            var jsonFile = ($"Account.json");
            string jsonString = File.ReadAllText(jsonFile);
            Account Account = JsonSerializer.Deserialize<Account>(jsonString);
            return Account;

        }
        async Task IGetAccountDataService.UpdateAccountDataAsync(Domain.Entities.Account Account)
        {
            var jsonFile = ($"Account.json");
            if (!File.Exists(jsonFile))
            {
                //
            }
            string jsonString = File.ReadAllText(jsonFile);
            Account OldAccount = JsonSerializer.Deserialize<Account>(jsonString);
            Account.token = OldAccount.token;
            string updatedJsonString = JsonSerializer.Serialize(Account);
            File.WriteAllText(jsonFile, updatedJsonString);
        }
        async Task IGetAccountDataService.UpdateTokenAsync(string token)
        {
            var jsonFile = ($"Account.json");
            if (!File.Exists(jsonFile))
            {
                //
            }
            string jsonString = File.ReadAllText(jsonFile);
            Account Account = JsonSerializer.Deserialize<Account>(jsonString);
            Account.token = token;
            string updatedJsonString = JsonSerializer.Serialize(Account);
            File.WriteAllText(jsonFile, updatedJsonString);
            //  var path = File.my
            await Console.Out.WriteLineAsync();
        }
    }
}
