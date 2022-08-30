using ds.pms.apicommon.Settings;
using ds.pms.bl.logger;
using ds.pms.bl.reports.IServices;
using ds.pms.bl.reports.Services;
using ds.pms.bl.users.Services;
using ds.pms.dal.CustomModels;
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
    public class ReportController : BaseController
    {
        private IReportService reportService;
        private Logging logging;

       
        public ReportController(ILogger<ReportController> reportControllerLogger, ILogger<ReportService> reportServiceLogger
            , ILogger<UserService> userServiceLogger, IOptions<ConnectionSettings> iConnectionSettings
            , IOptions<IdentitySettings> iIdentitySettings)
            : base(iConnectionSettings, iIdentitySettings, userServiceLogger)
        {
            logging = new Logging(reportControllerLogger);
            reportService = new ReportService(
                connectionSettings.OperationsDbSqlProvider,
                connectionSettings.OperationsDbConnString,
                identitySettings.JwtSecret,
                identitySettings.JwtExpirationSeconds, reportServiceLogger);
        }
     
        
       
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/report/gettenantreport")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(List<TenantRoportList>), description: "Successful operation")]
        public virtual IActionResult GetTenantReport([FromQuery] int typeId)
        {
            try
            {
                if (typeId > 0)
                {
                    List<TenantRoportList> report = reportService.TenantReport(typeId);
                    if (report != null)
                        return Ok(report);
                    else
                        return Ok(new { message = "No Data found." });
                }
                else
                    return BadRequest(new { message = "Supplied  information is not valid" });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }
      
        [Route("/v1/report/emptyroomsreport")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(List<TenantRoportList>), description: "Successful operation")]
        public virtual IActionResult GetEmptyRooms()
        {
            try
            {
               
                    List<EmptyRoomList> report = reportService.EmptyRoomReport();
                    if (report != null)
                        return Ok(report);
                    else
                        return Ok(new { message = "No Data found." });
               
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/report/getmissingdocreport")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(List<MissingDocumentList>), description: "Successful operation")]
        public virtual IActionResult GetMissingDocReport([FromQuery] int clientId,[FromQuery] int id)
        {
            try
            {
                //if (id > 0)
                //{
                    List<MissingDocumentList> report = reportService.MissingDocumentReport(clientId, id);
                    if (report != null)
                        return Ok(report);
                    else
                        return Ok(new { message = "No Data found." });
                //}
                //else
                //    return BadRequest(new { message = "Supplied  information is not valid" });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        [Route("/v1/report/getexpirydocreport")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(List<ExpiryDocumentList>), description: "Successful operation")]
        public virtual IActionResult GetExpiryDocReport([FromQuery] int days, [FromQuery] int typeId)
        {
            try
            {
                //if (id > 0)
                //{
                List<ExpiryDocumentList> report = reportService.ExpiyDocumentReport(days,typeId);
                if (report != null)
                    return Ok(report);
                else
                    return Ok(new { message = "No Data found." });
                //}
                //else
                //    return BadRequest(new { message = "Supplied  information is not valid" });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

    }
}
