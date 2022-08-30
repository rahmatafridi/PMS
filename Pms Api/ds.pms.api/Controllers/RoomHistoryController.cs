using ds.pms.apicommon.Models;
using ds.pms.apicommon.Settings;
using ds.pms.bl.logger;
using ds.pms.bl.roomhistory.IServices;
using ds.pms.bl.roomhistory.Models;
using ds.pms.bl.roomhistory.Services;
using ds.pms.bl.users.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ds.pms.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomHistoryController : BaseController
    {
        private IRoomHistoryService roomService;
        private Logging logging;

        public RoomHistoryController(ILogger<RoomController> roomControllerLogger, ILogger<RoomHistoryService> roomServiceLogger
           , ILogger<UserService> userServiceLogger, IOptions<ConnectionSettings> iConnectionSettings
           , IOptions<IdentitySettings> iIdentitySettings)
           : base(iConnectionSettings, iIdentitySettings, userServiceLogger)
        {
            logging = new Logging(roomControllerLogger);
            roomService = new RoomHistoryService(
                connectionSettings.OperationsDbSqlProvider,
                connectionSettings.OperationsDbConnString,
                identitySettings.JwtSecret,
                identitySettings.JwtExpirationSeconds, roomServiceLogger);
        }
        [Route("/v1/roomhistory/getroombyid")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(PaginatedList<RoomHistory>), description: "Successful operation")]
        public virtual IActionResult GetActiveRoomList([FromQuery] int tenantId)
        {
            try
            {
                if (roomService != null)
                {
                    PaginatedList<RoomHistory> Rooms = roomService.GetRoomHistoryList(tenantId);
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


    }

}
