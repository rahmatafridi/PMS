using ds.pms.dal.CustomModels;
using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.roomhistory.Models
{
    public class RoomHistory
    {
        public int Id { get; set; } // int
        public int? PropertyId { get; set; } // int
        public int? TenantId { get; set; } // int
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
        public string Property { get; set; }
        public static implicit operator RoomHistory(TblRoomsHistory dbRoom)
        {
            if (dbRoom != null)
            {
                RoomHistory dlRoom = new RoomHistory()
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

        public static implicit operator TblRoomsHistory(RoomHistory dlRoom)
        {
            if (dlRoom != null)
            {
                TblRoomsHistory dbRoom = new TblRoomsHistory()
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


        public static implicit operator RoomHistory(RoomHistoryList db)
        {
            if (db != null)
            {
                RoomHistory dl = new RoomHistory()
                {
                    Id = db.Id,
                    IsTenantIsMoving = db.IsTenantIsMoving,
                    IsTenantLeaving = db.IsTenantLeaving,
                    Property = db.Property,
                    RoomName = db.RoomName,
                    RoomNo = db.RoomNo,
                    TenancyStartDate = db.TenancyStartDate,
                    TenancyEndDate = db.TenancyEndDate,
                    Tenant = db.Tenant,
                    TenantId = db.TenantId
                };
                return dl;
            }
            return null;
        }

        public static implicit operator RoomHistoryList(RoomHistory dl)
        {
            if (dl != null)
            {
                RoomHistoryList db = new RoomHistoryList()
                {
                    Id = dl.Id,
                    IsTenantIsMoving = dl.IsTenantIsMoving,
                    IsTenantLeaving =dl.IsTenantLeaving,
                    Property =dl.Property,
                    RoomName = dl.RoomName,
                    RoomNo = dl.RoomNo,
                    TenancyStartDate = dl.TenancyStartDate,
                    TenancyEndDate = dl.TenancyEndDate,
                    Tenant = dl.Tenant,
                    TenantId = dl.TenantId
                };
                return db;
            }
            return null;
        }
    }
}
