using ds.pms.apicommon.Models;
using ds.pms.bl.logger;
using ds.pms.bl.rooms.Converters;
using ds.pms.bl.rooms.IServices;
using ds.pms.bl.rooms.Models;
using ds.pms.dal.CustomRepository;
using ds.pms.dal.Models;
using ds.pms.helpers.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace ds.pms.bl.rooms.Services
{
    public class RoomService : IRoomService
    {
        private RoomRepository roomRepository;
        private readonly string jwtSecretKey;
        private int tokenTimeoutSeconds;
        private Logging logging;
        private readonly string _className;
        private string _methodName;

        public RoomService(string _databaseProvider, string _connectionString, ILogger<RoomService> roomServiceLogger)
        {
            roomRepository = new RoomRepository(_databaseProvider, _connectionString);
            logging = new Logging(roomServiceLogger);
            _className = this.GetType().Name;
        }

        public RoomService(string _databaseProvider, string _connectionString, string jwtSecret, int tokenTimeoutInSeconds
            , ILogger<RoomService> roomServiceLogger)
        {
            roomRepository = new RoomRepository(_databaseProvider, _connectionString);
            jwtSecretKey = jwtSecret;
            tokenTimeoutSeconds = tokenTimeoutInSeconds;
            logging = new Logging(roomServiceLogger);
            _className = this.GetType().Name;
        }

        public PaginatedList<Room> GetActiveRoomList(int proId, string search, int? limit = 10, int? page = 1, string sort = "")
        {
            PaginatedList<Room> roomsPaginatedList = new PaginatedList<Room>();
            try
            {
                int pageSize = limit ?? 10;
                int pageNumber = page ?? 1;
                string sortBy = string.Empty, sortDirection = string.Empty;
                if (!string.IsNullOrEmpty(sort))
                {
                    string[] sorting = SortDirection.SortDir(sort);
                    if (sorting.Length > 1)
                    {
                        sortBy = sorting[0];
                        sortDirection = sorting[1];
                    }
                }
                roomsPaginatedList = Pagination.ConvertDalToBlList(roomRepository.GetActiveRoomList(proId, search, pageSize, pageNumber, sortBy, sortDirection));
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return roomsPaginatedList;
        }

        public Room GetRoomById(int roomId)
        {
            try
            {
                if (roomId > 0)
                {
                    Room room = roomRepository.GetRoomById(roomId);
                    if (room != null)
                        return room;
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return null;
        }

        public RoomCommonResponse AddRoom(Room addRoom, string userName)
        {
            RoomCommonResponse roomCommonResponse = new RoomCommonResponse();
            try
            {
                roomCommonResponse.Success = false;
                if (addRoom != null)
                {
                    TblRoom tblRoom = addRoom;
                    tblRoom.IsActive = true;
                    tblRoom.AddedBy = userName;
                    tblRoom.AddedDate = DateTime.UtcNow;
                    tblRoom.IsDeleted = false;
                    roomCommonResponse.Room = roomRepository.Add(tblRoom);
                    if (roomCommonResponse.Room != null && roomCommonResponse.Room.Id > 0)
                    {
                        roomCommonResponse.Success = true;
                        return roomCommonResponse;
                    }
                    else
                        roomCommonResponse.Message = "Unable to add room.";
                }
                else
                    roomCommonResponse.Message = "Supplied room information is not valid.";
            }
            catch (Exception ex)
            {
                roomCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {
            }
            return roomCommonResponse;
        }

        public RoomCommonResponse UpdateRoom(Room updateRoom, string userName)
        {
            RoomCommonResponse roomCommonResponse = new RoomCommonResponse();
            try
            {
                roomCommonResponse.Success = false;
                if (updateRoom != null && updateRoom.Id > 0)
                {
                    TblRoom tblRoom;
                    tblRoom = roomRepository.GetRoomById(updateRoom.Id);
                    if (tblRoom != null)
                    {
                        if (updateRoom.IsTenantIsMoving == true || updateRoom.IsTenantLeaving == true)
                        {
                            tblRoom.PropertyId = updateRoom.PropertyId;
                            tblRoom.TenantId = 0;
                            tblRoom.TenancyStartDate = null;
                            tblRoom.TenancyEndDate = null;
                            tblRoom.CoreRent = null;
                            tblRoom.ServiceCharge = null;
                            tblRoom.PersonalCharge = null;
                            tblRoom.IsTenantIsMoving = false;
                            tblRoom.IsTenantLeaving = false;
                            tblRoom.IsVacant = false;
                        }
                        else
                        {


                            tblRoom.PropertyId = updateRoom.PropertyId;
                            tblRoom.TenantId = updateRoom.TenantId;
                            tblRoom.RoomNo = updateRoom.RoomNo;
                            tblRoom.RoomName = updateRoom.RoomName;
                            tblRoom.TenancyStartDate = updateRoom.TenancyStartDate;
                            tblRoom.TenancyEndDate = updateRoom.TenancyEndDate;
                            tblRoom.CoreRent = updateRoom.CoreRent;
                            tblRoom.ServiceCharge = updateRoom.ServiceCharge;
                            tblRoom.PersonalCharge = updateRoom.PersonalCharge;
                            tblRoom.IsTenantIsMoving = updateRoom.IsTenantIsMoving;
                            tblRoom.IsTenantLeaving = updateRoom.IsTenantLeaving;
                            tblRoom.IsActive = updateRoom.IsActive;
                            tblRoom.ModifiedBy = userName;
                            tblRoom.ModifiedDate = DateTime.UtcNow;
                            tblRoom.IsVacant = true;
                        }
                        roomCommonResponse.UpdateRoom = roomRepository.Update(tblRoom);
                        if (roomCommonResponse.UpdateRoom != null && roomCommonResponse.UpdateRoom.Id > 0)
                        {
                            if (updateRoom.IsTenantIsMoving == true || updateRoom.IsTenantLeaving == true)
                            {
                                var data = new TblRoomsHistory();
                                data.IsTenantIsMoving = updateRoom.IsTenantIsMoving;
                                data.IsTenantLeaving = updateRoom.IsTenantLeaving;
                                data.PersonalCharge = updateRoom.PersonalCharge;
                                data.RoomName = updateRoom.RoomName;
                                data.RoomNo = updateRoom.RoomNo;
                                data.ServiceCharge = updateRoom.ServiceCharge;
                                data.TenancyEndDate = updateRoom.TenancyEndDate;
                                data.TenancyStartDate = updateRoom.TenancyStartDate;
                                data.TenantId = updateRoom.TenantId;
                                data.PropertyId = updateRoom.PropertyId;
                                var result = roomRepository.AddHistory(data);
                            }
                            roomCommonResponse.Success = true;
                            return roomCommonResponse;
                        }
                        else
                            roomCommonResponse.Message = "Unable to update room.";
                    }
                    else
                        roomCommonResponse.Message = "Room does not exists.";
                }
                else
                    roomCommonResponse.Message = "Supplied room information is not valid.";
            }
            catch (Exception ex)
            {
                roomCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {
            }
            return roomCommonResponse;
        }

        public RoomCommonResponse SoftDelete(int roomId, string userName)
        {
            RoomCommonResponse roomCommonResponse = new RoomCommonResponse();
            try
            {
                roomCommonResponse.Success = false;
                if (roomId > 0)
                {
                    TblRoom tblRoom = roomRepository.GetRoomById(roomId);
                    if (tblRoom != null && tblRoom.Id > 0)
                    {
                        tblRoom.ModifiedBy = userName;
                        tblRoom.ModifiedDate = DateTime.UtcNow;
                        tblRoom.IsDeleted = true;
                        tblRoom.DeletedBy = userName;
                        tblRoom.DeletedDate = DateTime.Now;
                        roomCommonResponse.UpdateRoom = roomRepository.Update(tblRoom);
                        if (roomCommonResponse.UpdateRoom != null && roomCommonResponse.UpdateRoom.Id > 0)
                        {
                            roomCommonResponse.Success = true;
                            return roomCommonResponse;
                        }
                        else
                            roomCommonResponse.Message = "Unable to delete room.";
                    }
                    else
                        roomCommonResponse.Message = "Room does not exists.";
                }
                else
                    roomCommonResponse.Message = "Supplied room information is not valid.";
            }
            catch (Exception ex)
            {
                roomCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {

            }
            return roomCommonResponse;
        }

        public bool HardDelete(int roomId)
        {
            try
            {
                if (roomId > 0)
                {
                    return roomRepository.Delete(roomId);
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return false;
        }
    }
}
