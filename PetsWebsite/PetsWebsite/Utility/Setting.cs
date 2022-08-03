using PetsWebsite.Models.ViewModels;

namespace PetsWebsite.Utility
{
    public class Setting
    {
        private readonly IConfiguration _configuration;
        private readonly SystemModel _obj = new SystemModel();

        public Setting(IConfiguration configuration)
        {
            _configuration = configuration;
            _configuration.GetSection("NewebPay").Bind(_obj);
        }
        public string? MerchantID => _obj.MerchantID;
        public string? HashKey => _obj.HashKey;
        public string? HashIV => _obj.HashIV;
        public string? ReturnURL => _obj.ReturnURL;
        public string? NotifyURL => _obj.NotifyURL;
        public string? CustomerURL => _obj.CustomerURL;
        public string? AuthUrl => _obj.AuthUrl;
        public string? Version => _obj.Version;
    }
}
