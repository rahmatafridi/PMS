using ds.pms.apicommon.Settings;
using ds.pms.bl.dashboard.IServices;
using ds.pms.bl.logger;
using ds.pms.bl.roomhistory.Services;
using ds.pms.bl.users.Services;
using ds.pms.dal.CustomModels;
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
    public class DashboardController : BaseController
    {
        private IDashboradService dashboardService;
        private Logging logging;
        public DashboardController(ILogger<DashboardController> dashboardControllerLogger, ILogger<DashboardService> dashboardServiceLogger
           , ILogger<UserService> userServiceLogger, IOptions<ConnectionSettings> iConnectionSettings
           , IOptions<IdentitySettings> iIdentitySettings)
           : base(iConnectionSettings, iIdentitySettings, userServiceLogger)
        {
            logging = new Logging(dashboardControllerLogger);
            dashboardService = new DashboardService(
                connectionSettings.OperationsDbSqlProvider,
                connectionSettings.OperationsDbConnString,
                identitySettings.JwtSecret,
                identitySettings.JwtExpirationSeconds, dashboardServiceLogger);
        }
        [Route("/v1/dashboard/getdata")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(Dashboard), description: "Successful operation")]
        public virtual IActionResult GetData([FromQuery] int clientId)
        {
            try
            {
                if (dashboardService != null)
                {
                    Dashboard dashboard = dashboardService.LoadDashboardData(clientId);
                    if (dashboard != null)
                        return Ok(dashboard);
                    else
                        return Ok(new { message = "No Data found." });
                }
                else
                    return BadRequest(new { message = "Supplied Dashboard information is not valid" });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

    }
}
