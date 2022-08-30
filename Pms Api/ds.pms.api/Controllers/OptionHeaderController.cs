using ds.pms.apicommon.Models;
using ds.pms.apicommon.Settings;
using ds.pms.bl.logger;
using ds.pms.bl.optionheaders.IServices;
using ds.pms.bl.optionheaders.Models;
using ds.pms.bl.users.Services;
using ds.pms.dal.CustomModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using pms.bl.clients.Services;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ds.pms.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OptionHeaderController : BaseController
    {
        private IOptionHeaderService headerService;
        private Logging logging;
        public OptionHeaderController(ILogger<OptionHeaderController> headerControllerLogger, ILogger<OptionHeaderService> headerServiceLogger
            , ILogger<UserService> userServiceLogger, IOptions<ConnectionSettings> iConnectionSettings
            , IOptions<IdentitySettings> iIdentitySettings)
            : base(iConnectionSettings, iIdentitySettings, userServiceLogger)
        {
            logging = new Logging(headerControllerLogger);
            headerService = new OptionHeaderService(
                connectionSettings.OperationsDbSqlProvider,
                connectionSettings.OperationsDbConnString,
                identitySettings.JwtSecret,
                identitySettings.JwtExpirationSeconds,headerServiceLogger);
        }
        /// <summary>
        /// get client list
        /// </summary>
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/optionheader/getheader")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(PaginatedList<OptionHeaderList>), description: "Successful operation")]
        public virtual IActionResult GetActiveClientList([FromQuery] int clientId,[FromQuery] string search, [FromQuery] string sort, [FromQuery] int? limit, [FromQuery] int? page)
        {
            try
            {
                PaginatedList<OptionHeaderList> headers = headerService.GetActiveOptionHeaderList(clientId,search, limit, page, sort);
                if (headers != null)
                    return Ok(headers);
                else
                    return Ok(new { message = "No Data found." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// get client by id
        /// </summary>
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/optionheader/getheaderbyid")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(OptionHeader), description: "Successful operation")]
        public virtual IActionResult GetHeaderById([FromQuery] int headerId)
        {
            try
            {
                if (headerId > 0)
                {
                    OptionHeader header = headerService.GetOptionHeaderById(headerId);
                    if (header != null)
                        return Ok(header);
                    else
                        return Ok(new { message = "No Data found." });
                }
                else
                    return BadRequest(new { message = "Supplied client information is not valid" });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        [Route("/v1/optionheader/getheaderlistbyid")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(OptionHeader), description: "Successful operation")]
        public virtual IActionResult GetHeaderListById([FromQuery] int clientId)
        {
            try
            {
                if (clientId > 0)
                {
                    var header = headerService.GetOptionHeaderListById(clientId);
                    if (header != null)
                        return Ok(header);
                    else
                        return Ok(new { message = "No Data found." });
                }
                else
                    return BadRequest(new { message = "Supplied client information is not valid" });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// add client 
        /// </summary>
        /// <remarks>add new client information </remarks>
        /// <param name="addHeader">The client json format.</param>
        [HttpPost]
        [Route("/v1/optionheader/add")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(OptionHeader), description: "Successful operation")]
        public virtual IActionResult AddHeader([FromBody] OptionHeader addHeader)
        {
            try
            {
                if (headerService != null && addHeader != null)
                {
                    OptionHeaderCommonResponse headerCommonResponse;
                    if (CurrentUser != null)
                        headerCommonResponse = headerService.AddOptionHeader(addHeader, CurrentUser.UserName);
                    else
                        headerCommonResponse = headerService.AddOptionHeader(addHeader, null);
                    if (headerCommonResponse == null)
                        return BadRequest(new { message = "Supplied client information is not valid." });
                    else if (!headerCommonResponse.Success)
                        return BadRequest(new { message = headerCommonResponse.Message });
                    else
                    {
                        if (headerCommonResponse.OptionHeader != null && headerCommonResponse.OptionHeader.Id > 0)
                            return Ok(headerCommonResponse.OptionHeader);
                        else
                            return BadRequest(new { message = "Supplied client information is not valid." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied client information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// update client 
        /// </summary>
        /// <remarks>update client information </remarks>
        /// <param name="updateHeader">The client json format.</param>
        [HttpPut]
        [Route("/v1/optionheader/update")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(OptionHeader), description: "Successful operation")]
        public virtual IActionResult UpdateHeader([FromBody] OptionHeader updateHeader)
        {
            try
            {
                if (headerService != null && updateHeader != null && updateHeader.Id > 0)
                {
                    OptionHeaderCommonResponse headerCommonResponse = headerService.UpdateOptionHeader(updateHeader, CurrentUser.UserName);
                    if (headerCommonResponse == null)
                        return BadRequest(new { message = "Supplied client information is not valid." });
                    else if (!headerCommonResponse.Success)
                        return BadRequest(new { message = headerCommonResponse.Message });
                    else
                    {
                        if (headerCommonResponse.OptionHeader != null && headerCommonResponse.OptionHeader.Id > 0)
                            return Ok(headerCommonResponse.OptionHeader);
                        else
                            return BadRequest(new { message = "Supplied client information is not valid." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied client information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }



        /// <summary>
        /// soft delete client 
        /// </summary>
        /// <remarks>Soft delete client </remarks>
        [HttpDelete]
        [Route("/v1/optionheader/delete")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult SoftDeleteClient([FromQuery] int clientId)
        {
            try
            {
                if (clientId > 0)
                {
                    OptionHeaderCommonResponse headerCommonResponse = headerService.SoftDelete(clientId, CurrentUser.UserName);

                    if (headerCommonResponse == null)
                        return BadRequest(new { message = "Supplied client information is not valid." });
                    else if (!headerCommonResponse.Success)
                        return BadRequest(new { message = headerCommonResponse.Message });
                    else
                    {
                        if (headerCommonResponse.OptionHeader != null && headerCommonResponse.OptionHeader.Id > 0)
                            return Ok(true);
                        else
                            return BadRequest(new { message = "Unable to delete client." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied client information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// forcefully delete client 
        /// </summary>
        /// <remarks>forcefully delete client </remarks>
        [HttpDelete]
        [Route("/v1/optionheader/forcedelete")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult HardDeleteClient([FromQuery] int optionId)
        {
            try
            {
                bool isDeleted = false;
                if (optionId > 0)
                    isDeleted = headerService.HardDelete(optionId);

                if (isDeleted)
                    return Ok(true);

                return BadRequest(new { message = "Supplied client information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

    }
}
