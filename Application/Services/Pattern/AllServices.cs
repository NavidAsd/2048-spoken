using Application.Interface.Pattern;
using Application.Services.AccountLogin;
using Application.Services.GetAccountData;
using Application.Services.VerifyToken;

namespace Application.Services.Pattern
{
    public class AllServices : IAllServices
    {
        private IGenerateNewTokenService _generateNewToken;
        public IGenerateNewTokenService GenerateNewTokenService
        {
            get
            {
                return _generateNewToken = _generateNewToken ?? new GenerateNewTokenService();
            }
        }
        private IGetAccountDataService _getAccountData;
        public IGetAccountDataService GetAccountDataService
        {
            get
            {
                return _getAccountData = _getAccountData ?? new GetAccountDataService();
            }
        }
        private IVerifyActiveTokenService _verifyToken;
        public IVerifyActiveTokenService VerifyActiveTokenService
        {
            get
            {
                return _verifyToken = _verifyToken ?? new VerifyActiveTokenService();
            }
        }
    }
}
