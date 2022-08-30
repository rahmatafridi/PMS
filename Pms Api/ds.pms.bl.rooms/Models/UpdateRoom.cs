using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.rooms.Models
{
    public class UpdateRoom : Room
    {
        public bool? IsTenantIsMoving { get; set; } // bit
        public bool? IsTenantLeaving { get; set; } // bit
        public bool IsActive { get; set; } // bit

        public static implicit operator UpdateRoom(TblRoom dbRoom)
        {
            if (dbRoom != null)
            {
                UpdateRoom dlRoom = new UpdateRoom()
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
                    IsTenantIsMoving = dbRoom.IsTenantIsMoving,
                    IsTenantLeaving = dbRoom.IsTenantLeaving,
                    IsActive = dbRoom.IsActive,
                };
                return dlRoom;
            }
            return null;
        }

        public static implicit operator TblRoom(UpdateRoom dlRoom)
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
                    IsTenantIsMoving = dlRoom.IsTenantIsMoving,
                    IsTenantLeaving = dlRoom.IsTenantLeaving,
                    IsActive = dlRoom.IsActive,
                };
                return dbRoom;
            }
            return null;
        }
    }
}
