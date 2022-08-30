using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.rooms.Models
{
    public class RoomCommonResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Room Room { get; set; }
        public UpdateRoom UpdateRoom { get; set; }
    }
}
