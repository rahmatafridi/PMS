using ds.pms.apicommon.Models;
using ds.pms.bl.configs.Models;
using ds.pms.dal.CustomModels;

namespace ds.pms.bl.configs.IServices
{
    public interface IConfigService
    {
        PaginatedList<Config> GetActiveConfigList(int clientId, string search, int? limit = 10, int? page = 1, string sort = "");
        Config GetConfigById(int configId);
        ConfigCommonResponse AddConfig(Config addConfig, string userName);
        ConfigCommonResponse UpdateConfig(Config updateConfig, string userName);
        ConfigCommonResponse SoftDelete(int configId, string userName);
        bool HardDelete(int configId);
    }
}
