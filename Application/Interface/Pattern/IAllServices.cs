using Application.Services.AccountLogin;
using Application.Services.GetAccountData;
using Application.Services.VerifyToken;

namespace Application.Interface.Pattern
{
    public interface IAllServices
    {
        IGenerateNewTokenService GenerateNewTokenService { get; }
        IGetAccountDataService GetAccountDataService { get; }
        IVerifyActiveTokenService VerifyActiveTokenService { get; }
    }
}
