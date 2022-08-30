using ds.pms.apicommon.Models;
using ds.pms.apicommon.Settings;
using ds.pms.bl.logger;
using ds.pms.bl.options.IServices;
using ds.pms.bl.options.Models;
using ds.pms.bl.options.Services;
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
    public class OptionController : BaseController
    {
        private IOptionService headerService;
        private Logging logging;
        public OptionController(ILogger<OptionController> optionControllerLogger, ILogger<OptionService> headerServiceLogger
            , ILogger<UserService> userServiceLogger, IOptions<ConnectionSettings> iConnectionSettings
            , IOptions<IdentitySettings> iIdentitySettings)
            : base(iConnectionSettings, iIdentitySettings, userServiceLogger)
        {
            logging = new Logging(optionControllerLogger);
            headerService = new OptionService(
                connectionSettings.OperationsDbSqlProvider,
                connectionSettings.OperationsDbConnString,
                identitySettings.JwtSecret,
                identitySettings.JwtExpirationSeconds, headerServiceLogger);
        }
        /// <summary>
        /// get client list
        /// </summary>
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/option/getoption")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(PaginatedList<Option>), description: "Successful operation")]
        public virtual IActionResult GetActiveHeaderList([FromQuery] string search, [FromQuery] string sort, [FromQuery] int? limit, [FromQuery] int? page)
        {
            try
            {
                PaginatedList<Option> headers = headerService.GetActiveOptionList(search, limit, page, sort);
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


        [Route("/v1/option/getoptionheader")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(List<Option>), description: "Successful operation")]
        public virtual IActionResult GetOptionHeader([FromQuery] string header)
        {
            try
            {
               List<Option> headers = headerService.LoadOption(header);
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
        [Route("/v1/option/getoptionbyid")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(Option), description: "Successful operation")]
        public virtual IActionResult GetHeaderById([FromQuery] int optionId)
        {
            try
            {
                if (optionId > 0)
                {
                    Option header = headerService.GetOptionById(optionId);
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

        [Route("/v1/option/getoptionlistbyid")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(Option), description: "Successful operation")]
        public virtual IActionResult GetHeaderlistById([FromQuery] int headerId)
        {
            try
            {
                if (headerId > 0)
                {
                    var header = headerService.GetOptionListById(headerId);
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

        [HttpGet]
        [Route("/v1/option/validateoption")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult ValidateOption([FromQuery] int id, [FromQuery] int headerId, [FromQuery] string value, [FromQuery] string title)
        {
            try
            {
                if (!string.IsNullOrEmpty(value))
                {
                    bool valid = headerService.IsValidOption(id, headerId, value, title);
                    if (!valid)
                        return BadRequest(new { message = "option is already in use." });
                    return Ok(valid);
                }
                else
                    return BadRequest(new { message = "Supplied  information is not valid." });
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
        [Route("/v1/option/add")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(Option), description: "Successful operation")]
        public virtual IActionResult AddHeader([FromBody] Option addHeader)
        {
            try
            {
                if (headerService != null && addHeader != null)
                {
                    OptionCommonResponse headerCommonResponse;
                    if (CurrentUser != null)
                        headerCommonResponse = headerService.AddOption(addHeader, CurrentUser.UserName);
                    else
                        headerCommonResponse = headerService.AddOption(addHeader, null);
                    if (headerCommonResponse == null)
                        return BadRequest(new { message = "Supplied client information is not valid." });
                    else if (!headerCommonResponse.Success)
                        return BadRequest(new { message = headerCommonResponse.Message });
                    else
                    {
                        if (headerCommonResponse.Option != null && headerCommonResponse.Option.Id > 0)
                            return Ok(headerCommonResponse.Option);
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
        [Route("/v1/option/update")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(Option), description: "Successful operation")]
        public virtual IActionResult UpdateHeader([FromBody] Option updateHeader)
        {
            try
            {
                if (headerService != null && updateHeader != null && updateHeader.Id > 0)
                {
                    OptionCommonResponse headerCommonResponse = headerService.UpdateOption(updateHeader, CurrentUser.UserName);
                    if (headerCommonResponse == null)
                        return BadRequest(new { message = "Supplied client information is not valid." });
                    else if (!headerCommonResponse.Success)
                        return BadRequest(new { message = headerCommonResponse.Message });
                    else
                    {
                        if (headerCommonResponse.Option != null && headerCommonResponse.Option.Id > 0)
                            return Ok(headerCommonResponse.Option);
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
        [Route("/v1/option/delete")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult SoftDeleteClient([FromQuery] int optionId)
        {
            try
            {
                if (optionId > 0)
                {
                    OptionCommonResponse headerCommonResponse = headerService.SoftDelete(optionId, CurrentUser.UserName);

                    if (headerCommonResponse == null)
                        return BadRequest(new { message = "Supplied client information is not valid." });
                    else if (!headerCommonResponse.Success)
                        return BadRequest(new { message = headerCommonResponse.Message });
                    else
                    {
                        if (headerCommonResponse.Option != null && headerCommonResponse.Option.Id > 0)
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
        [Route("/v1/option/forcedelete")]
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
