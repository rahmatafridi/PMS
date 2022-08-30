using ds.pms.apicommon.Models;
using ds.pms.bl.rooms.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.rooms.IServices
{
    public interface IRoomService
    {
        PaginatedList<Room> GetActiveRoomList(int proId,string search, int? limit = 10, int? page = 1, string sort = "");
        Room GetRoomById(int roomId);
        RoomCommonResponse AddRoom(Room addRoom, string userName);
        RoomCommonResponse UpdateRoom(Room updateRoom, string userName);
        RoomCommonResponse SoftDelete(int roomId, string userName);
        bool HardDelete(int roomId);
    }
}
