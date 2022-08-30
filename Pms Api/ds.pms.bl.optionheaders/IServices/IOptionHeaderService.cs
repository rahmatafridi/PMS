using ds.pms.apicommon.Models;
using ds.pms.bl.optionheaders.Models;
using ds.pms.dal.CustomModels;
using ds.pms.dal.Models;
using System.Collections.Generic;

namespace ds.pms.bl.optionheaders.IServices
{
    public interface IOptionHeaderService
    {
        PaginatedList<OptionHeaderList> GetActiveOptionHeaderList(int clientId,string search, int? limit = 10, int? page = 1, string sort = "");
        TblOptionHeader GetOptionHeaderById(int headerId);
        List<TblOptionHeader> GetOptionHeaderListById(int clientId);

        OptionHeaderCommonResponse AddOptionHeader(OptionHeader addOptionHeader, string userName);
        OptionHeaderCommonResponse UpdateOptionHeader(OptionHeader OptionHeader, string userName);
       
        OptionHeaderCommonResponse SoftDelete(int clientId, string userName);
        bool HardDelete(int clientId);
    }
}
