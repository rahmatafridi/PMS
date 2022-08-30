using ds.pms.apicommon.Models;
using ds.pms.bl.options.Models;
using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.options.IServices
{
   public interface IOptionService
    {
        PaginatedList<Option> GetActiveOptionList(string search, int? limit = 10, int? page = 1, string sort = "");
        TblOption GetOptionById(int clientId);
        List<TblOption> GetOptionListById(int headerId);
        bool IsValidOption(int id,int optionId,string value, string title);

        OptionCommonResponse AddOption(Option addOption, string userName);
        OptionCommonResponse UpdateOption(Option option, string userName);

        OptionCommonResponse SoftDelete(int clientId, string userName);
        List<Option> LoadOption(string headerName);
        bool HardDelete(int clientId);
    }
}
