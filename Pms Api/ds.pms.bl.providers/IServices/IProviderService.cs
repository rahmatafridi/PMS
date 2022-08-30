using ds.pms.apicommon.Models;
using ds.pms.bl.providers.Models;

namespace ds.pms.bl.providers.IServices
{
    public interface IProviderService
    {
        PaginatedList<Provider> GetActiveProviderList(int clientId, string search, int? limit = 10, int? page = 1, string sort = "");
        Provider GetProviderById(int ProviderId);
        ProviderUser GetProiderUserById(int ProviderId);
        ProviderCommonResponse AddProvider(Provider addProvider, string userName);
        ProviderCommonResponse UpdateProvider(UpdateProvider updateProvider, string userName);
        bool IsValidEmail(string email);
        bool IsValidEmail(long? ProviderId, string email);
        ProviderCommonResponse SoftDelete(int ProviderId, string userName);
        bool HardDelete(int ProviderId);
        ProviderCommonResponse AddUpdateProviderUser(ProviderUser user);

    }
}
