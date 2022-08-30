using ds.pms.apicommon.Models;
using ds.pms.bl.roomhistory.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.roomhistory.IServices
{
    public interface IRoomHistoryService
    {
        PaginatedList<RoomHistory> GetRoomHistoryList(int tenantId);
 
    }
}
