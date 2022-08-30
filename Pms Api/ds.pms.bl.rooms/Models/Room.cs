using ds.pms.dal.CustomModels;
using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.rooms.Models
{
    public class Room
    {
        public int Id { get; set; } // int
        public int PropertyId { get; set; } // int
        public int TenantId { get; set; } // int
        public string Tenant { get;set; }
        public int? RoomNo { get; set; } // int
        public string RoomName { get; set; } // varchar(50)
        public DateTime? TenancyStartDate { get; set; } // datetime
        public DateTime? TenancyEndDate { get; set; } // datetime
        public decimal? CoreRent { get; set; } // decimal(18, 2)
        public decimal? ServiceCharge { get; set; } // decimal(18, 2)
        public decimal? PersonalCharge { get; set; } // decimal(18, 2)
        public bool? IsTenantIsMoving { get; set; } // bit
        public bool? IsTenantLeaving { get; set; } // bit
        public bool IsActive { get; set; } // bit

        public static implicit operator Room(TblRoom dbRoom)
        {
            if (dbRoom != null)
            {
                Room dlRoom = new Room()
                {
                    Id = dbRoom.Id,
                    PropertyId = dbRoom.PropertyId,
                    TenantId = dbRoom.TenantId,
                    RoomNo = dbRoom.RoomNo,
                    RoomName = dbRoom.RoomName,
                    TenancyStartDate = dbRoom.TenancyStartDate,
                    TenancyEndDate = dbRoom.TenancyEndDate,
                    CoreRent = dbRoom.CoreRent,
                    ServiceCharge = dbRoom.ServiceCharge,
                    PersonalCharge = dbRoom.PersonalCharge,
                };
                return dlRoom;
            }
            return null;
        }

        public static implicit operator TblRoom(Room dlRoom)
        {
            if (dlRoom != null)
            {
                TblRoom dbRoom = new TblRoom()
                {
                    Id = dlRoom.Id,
                    PropertyId = dlRoom.PropertyId,
                    TenantId = dlRoom.TenantId,
                    RoomNo = dlRoom.RoomNo,
                    RoomName = dlRoom.RoomName,
                    TenancyStartDate = dlRoom.TenancyStartDate,
                    TenancyEndDate = dlRoom.TenancyEndDate,
                    CoreRent = dlRoom.CoreRent,
                    ServiceCharge = dlRoom.ServiceCharge,
                    PersonalCharge = dlRoom.PersonalCharge,
                };
                return dbRoom;
            }
            return null;
        }


        public static implicit operator Room(RoomList db)
        {
            if (db != null)
            {
                Room dl = new Room()
                {
                    Id= db.Id,
                    CoreRent= db.CoreRent,
                    PersonalCharge= db.PersonalCharge,
                    RoomName= db.RoomName,
                    ServiceCharge= db.ServiceCharge,
                    RoomNo= db.RoomNo,
                    TenancyStartDate= db.TenancyStartDate,
                    Tenant= db.Tenant,
                    TenantId= db.TenantId
                };
                return dl;
            }
            return null;
        }

        public static implicit operator RoomList(Room dl)
        {
            if (dl != null)
            {
                RoomList db = new RoomList()
                {
                    Id = dl.Id,
                    CoreRent = dl.CoreRent,
                    PersonalCharge = dl.PersonalCharge,
                    RoomName = dl.RoomName,
                    ServiceCharge = dl.ServiceCharge,
                    RoomNo = dl.RoomNo,
                    TenancyStartDate = dl.TenancyStartDate,
                    Tenant = dl.Tenant,
                    TenantId = dl.TenantId
                };
                return db;
            }
            return null;
        }
    }
}
