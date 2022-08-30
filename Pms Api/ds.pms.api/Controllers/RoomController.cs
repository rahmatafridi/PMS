using ds.pms.apicommon.Models;
using ds.pms.apicommon.Settings;
using ds.pms.bl.logger;
using ds.pms.bl.rooms.IServices;
using ds.pms.bl.rooms.Models;
using ds.pms.bl.rooms.Services;
using ds.pms.bl.users.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace ds.pms.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : BaseController
    {
        private IRoomService roomService;
        private Logging logging;

        /// <summary>
        /// Room controller constructor
        /// </summary>
        /// <param name="roomControllerLogger"></param>
        /// <param name="roomServiceLogger"></param>
        /// <param name="userServiceLogger"></param>
        /// <param name="iConnectionSettings"></param>
        /// <param name="iIdentitySettings"></param>
        public RoomController(ILogger<RoomController> roomControllerLogger, ILogger<RoomService> roomServiceLogger
            , ILogger<UserService> userServiceLogger, IOptions<ConnectionSettings> iConnectionSettings
            , IOptions<IdentitySettings> iIdentitySettings)
            : base(iConnectionSettings, iIdentitySettings, userServiceLogger)
        {
            logging = new Logging(roomControllerLogger);
            roomService = new RoomService(
                connectionSettings.OperationsDbSqlProvider,
                connectionSettings.OperationsDbConnString,
                identitySettings.JwtSecret,
                identitySettings.JwtExpirationSeconds, roomServiceLogger);
        }

        /// <summary>
        /// get room list
        /// </summary>
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/room/getrooms")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(PaginatedList<Room>), description: "Successful operation")]
        public virtual IActionResult GetActiveRoomList([FromQuery] int proId, [FromQuery] string search, [FromQuery] string sort, [FromQuery] int? limit, [FromQuery] int? page)
        {
            try
            {
                if (roomService != null)
                {
                    PaginatedList<Room> Rooms = roomService.GetActiveRoomList(proId, search, limit, page, sort);
                    if (Rooms != null)
                        return Ok(Rooms);
                    else
                        return Ok(new { message = "No Data found." });
                }
                else
                    return BadRequest(new { message = "Supplied room information is not valid" });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// get room by id
        /// </summary>
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/room/getroombyid")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(Room), description: "Successful operation")]
        public virtual IActionResult GetRoomById([FromQuery] int id)
        {
            try
            {
                if (roomService != null && id > 0)
                {
                    Room room = roomService.GetRoomById(id);
                    if (room != null)
                        return Ok(room);
                    else
                        return Ok(new { message = "No Data found." });
                }
                else
                    return BadRequest(new { message = "Supplied room information is not valid" });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// add room 
        /// </summary>
        /// <remarks>add new room information </remarks>
        /// <param name="addRoom">The room json format.</param>
        [HttpPost]
        [Route("/v1/room/add")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(Room), description: "Successful operation")]
        public virtual IActionResult AddRoom([FromBody] Room addRoom)
        {
            try
            {
                if (roomService != null && addRoom != null)
                {
                    RoomCommonResponse roomCommonResponse = roomService.AddRoom(addRoom, CurrentUser.UserName);
                    if (roomCommonResponse == null)
                        return BadRequest(new { message = "Supplied room information is not valid." });
                    else if (!roomCommonResponse.Success)
                        return BadRequest(new { message = roomCommonResponse.Message });
                    else
                    {
                        if (roomCommonResponse.Room != null && roomCommonResponse.Room.Id > 0)
                            return Ok(roomCommonResponse.Room);
                        else
                            return BadRequest(new { message = "Supplied room information is not valid." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied room information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// update room 
        /// </summary>
        /// <remarks>update room information </remarks>
        /// <param name="updateRoom">The room json format.</param>
        [HttpPut]
        [Route("/v1/room/update")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(Room), description: "Successful operation")]
        public virtual IActionResult UpdateRoom([FromBody] Room updateRoom)
        {
            try
            {
                if (roomService != null && updateRoom != null && updateRoom.Id > 0)
                {
                    RoomCommonResponse roomCommonResponse = roomService.UpdateRoom(updateRoom, CurrentUser.UserName);
                    if (roomCommonResponse == null)
                        return BadRequest(new { message = "Supplied room information is not valid." });
                    else if (!roomCommonResponse.Success)
                        return BadRequest(new { message = roomCommonResponse.Message });
                    else
                    {
                        if (roomCommonResponse.UpdateRoom != null && roomCommonResponse.UpdateRoom.Id > 0)
                            return Ok(roomCommonResponse.UpdateRoom);
                        else
                            return BadRequest(new { message = "Supplied room information is not valid." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied room information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// soft delete room 
        /// </summary>
        /// <remarks>Soft delete room </remarks>
        [HttpDelete]
        [Route("/v1/room/delete")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult SoftDeleteRoom([FromQuery] int roomId)
        {
            try
            {
                if (roomService != null && roomId > 0)
                {
                    RoomCommonResponse roomCommonResponse = roomService.SoftDelete(roomId, CurrentUser.UserName);

                    if (roomCommonResponse == null)
                        return BadRequest(new { message = "Supplied room information is not valid." });
                    else if (!roomCommonResponse.Success)
                        return BadRequest(new { message = roomCommonResponse.Message });
                    else
                    {
                        if (roomCommonResponse.UpdateRoom != null && roomCommonResponse.UpdateRoom.Id > 0)
                            return Ok(true);
                        else
                            return BadRequest(new { message = "Unable to delete room." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied room information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// forcefully delete room 
        /// </summary>
        /// <remarks>forcefully delete room </remarks>
        [HttpDelete]
        [Route("/v1/room/forcedelete")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult HardDeleteRoom([FromQuery] int roomId)
        {
            try
            {
                bool isDeleted = false;
                if (roomService != null && roomId > 0)
                    isDeleted = roomService.HardDelete(roomId);

                if (isDeleted)
                    return Ok(true);

                return BadRequest(new { message = "Supplied room information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }
    }
}
