using Abp.AutoMapper;
using Zhn.Template.Authentication.External;

namespace Zhn.Template.Models.TokenAuth
{
    [AutoMapFrom(typeof(ExternalLoginProviderInfo))]
    public class ExternalLoginProviderInfoModel
    {
        public string Name { get; set; }

        public string ClientId { get; set; }
    }
}


