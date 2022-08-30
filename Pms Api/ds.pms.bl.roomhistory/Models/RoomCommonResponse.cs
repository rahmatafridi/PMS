using ds.pms.bl.roomhistory.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.roomhistory.Models
{
    public class RoomCommonResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public RoomHistory Room { get; set; }
    }
}
