using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ds.pms.apicommon.Models;
using ds.pms.apicommon.Settings;
using ds.pms.bl.compliances.IServices;
using ds.pms.bl.compliances.Models;
using ds.pms.bl.compliances.Services;
using ds.pms.bl.logger;
using ds.pms.bl.users.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;

namespace ds.pms.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplianceController : BaseController
    {
        private IComplianceService complianceService;
        private Logging logging;

        /// <summary>
        /// Compliance controller constructor
        /// </summary>
        /// <param name="complianceControllerLogger"></param>
        /// <param name="complianceServiceLogger"></param>
        /// <param name="userServiceLogger"></param>
        /// <param name="iConnectionSettings"></param>
        /// <param name="iIdentitySettings"></param>
        public ComplianceController(ILogger<ComplianceController> complianceControllerLogger, ILogger<ComplianceService> complianceServiceLogger
            , ILogger<UserService> userServiceLogger, IOptions<ConnectionSettings> iConnectionSettings
            , IOptions<IdentitySettings> iIdentitySettings)
            : base(iConnectionSettings, iIdentitySettings, userServiceLogger)
        {
            logging = new Logging(complianceControllerLogger);
            complianceService = new ComplianceService(
                connectionSettings.OperationsDbSqlProvider,
                connectionSettings.OperationsDbConnString,
                identitySettings.JwtSecret,
                identitySettings.JwtExpirationSeconds, complianceServiceLogger);
        }

        /// <summary>
        /// get compliance list
        /// </summary>
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/compliance/getcompliances")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(PaginatedList<Compliance>), description: "Successful operation")]
        public virtual IActionResult GetActiveComplianceList([FromQuery] string search, [FromQuery] string sort, [FromQuery] int? limit, [FromQuery] int? page)
        {
            try
            {
                if (complianceService != null)
                {
                    PaginatedList<Compliance> Compliances = complianceService.GetActiveComplianceList(search, limit, page, sort);
                    if (Compliances != null)
                        return Ok(Compliances);
                    else
                        return Ok(new { message = "No Data found." });
                }
                else
                    return BadRequest(new { message = "Supplied compliance information is not valid" });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// get compliance by id
        /// </summary>
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/compliance/getcompliancebyid")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(Compliance), description: "Successful operation")]
        public virtual IActionResult GetComplianceById([FromQuery] int complianceId)
        {
            try
            {
                if (complianceService != null && complianceId > 0)
                {
                    Compliance compliance = complianceService.GetComplianceById(complianceId);
                    if (compliance != null)
                        return Ok(compliance);
                    else
                        return Ok(new { message = "No Data found." });
                }
                else
                    return BadRequest(new { message = "Supplied compliance information is not valid" });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }
        [HttpGet]
        [Route("/v1/compliance/getbyclient")]
        [SwaggerResponse(statusCode: 200, type: typeof(PaginatedList<Compliance>), description: "Successful operation")]

        public virtual IActionResult GetComplinceByClient([FromQuery] int clientId)
        {
            try
            {
                if (complianceService != null)
                {
                    PaginatedList<Compliance> Compliances = complianceService.GetComplianceByClient(clientId);
                    if (Compliances != null)
                        return Ok(Compliances);
                    else
                        return Ok(new { message = "No Data found." });
                }
                else
                    return BadRequest(new { message = "Supplied compliance information is not valid" });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }

        }
        /// <summary>
        /// add compliance 
        /// </summary>
        /// <remarks>add new compliance information </remarks>
        /// <param name="addCompliance">The compliance json format.</param>
        [HttpPost]
        [Route("/v1/compliance/add")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(Compliance), description: "Successful operation")]
        public virtual IActionResult AddCompliance([FromBody] Compliance addCompliance)
        {
            try
            {
                if (complianceService != null && addCompliance != null)
                {
                    ComplianceCommonResponse complianceCommonResponse = complianceService.AddCompliance(addCompliance, CurrentUser.UserName);
                    if (complianceCommonResponse == null)
                        return BadRequest(new { message = "Supplied compliance information is not valid." });
                    else if (!complianceCommonResponse.Success)
                        return BadRequest(new { message = complianceCommonResponse.Message });
                    else
                    {
                        if (complianceCommonResponse.Compliance != null && complianceCommonResponse.Compliance.Id > 0)
                            return Ok(complianceCommonResponse.Compliance);
                        else
                            return BadRequest(new { message = "Supplied compliance information is not valid." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied compliance information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// update compliance 
        /// </summary>
        /// <remarks>update compliance information </remarks>
        /// <param name="updateCompliance">The compliance json format.</param>
        [HttpPut]
        [Route("/v1/compliance/update")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(UpdateCompliance), description: "Successful operation")]
        public virtual IActionResult UpdateCompliance([FromBody] UpdateCompliance updateCompliance)
        {
            try
            {
                if (complianceService != null && updateCompliance != null && updateCompliance.Id > 0)
                {
                    ComplianceCommonResponse complianceCommonResponse = complianceService.UpdateCompliance(updateCompliance, CurrentUser.UserName);
                    if (complianceCommonResponse == null)
                        return BadRequest(new { message = "Supplied compliance information is not valid." });
                    else if (!complianceCommonResponse.Success)
                        return BadRequest(new { message = complianceCommonResponse.Message });
                    else
                    {
                        if (complianceCommonResponse.UpdateCompliance != null && complianceCommonResponse.UpdateCompliance.Id > 0)
                            return Ok(complianceCommonResponse.UpdateCompliance);
                        else
                            return BadRequest(new { message = "Supplied compliance information is not valid." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied compliance information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// soft delete compliance 
        /// </summary>
        /// <remarks>Soft delete compliance </remarks>
        [HttpDelete]
        [Route("/v1/compliance/delete")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult SoftDeleteCompliance([FromQuery] int complianceId)
        {
            try
            {
                if (complianceService != null && complianceId > 0)
                {
                    ComplianceCommonResponse complianceCommonResponse = complianceService.SoftDelete(complianceId, CurrentUser.UserName);

                    if (complianceCommonResponse == null)
                        return BadRequest(new { message = "Supplied compliance information is not valid." });
                    else if (!complianceCommonResponse.Success)
                        return BadRequest(new { message = complianceCommonResponse.Message });
                    else
                    {
                        if (complianceCommonResponse.UpdateCompliance != null && complianceCommonResponse.UpdateCompliance.Id > 0)
                            return Ok(true);
                        else
                            return BadRequest(new { message = "Unable to delete compliance." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied compliance information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// forcefully delete compliance 
        /// </summary>
        /// <remarks>forcefully delete compliance </remarks>
        [HttpDelete]
        [Route("/v1/compliance/forcedelete")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult HardDeleteCompliance([FromQuery] int complianceId)
        {
            try
            {
                bool isDeleted = false;
                if (complianceService != null && complianceId > 0)
                    isDeleted = complianceService.HardDelete(complianceId);

                if (isDeleted)
                    return Ok(true);

                return BadRequest(new { message = "Supplied compliance information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }
        
    }
}
