using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ds.pms.apicommon.Models;
using ds.pms.apicommon.Settings;
using ds.pms.bl.compliancepropertydocs.IServices;
using ds.pms.bl.compliancepropertydocs.Models;
using ds.pms.bl.compliancepropertydocs.Services;
using ds.pms.bl.logger;
using ds.pms.bl.users.Services;
using ds.pms.dal.CustomModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;

namespace ds.pms.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompliancePropertyDocController : BaseController
    {
        private ICompliancePropertyDocService compliancePropertyDocService;
        private Logging logging;

        /// <summary>
        /// CompliancePropertyDoc controller constructor
        /// </summary>
        /// <param name="compliancePropertyDocControllerLogger"></param>
        /// <param name="compliancePropertyDocServiceLogger"></param>
        /// <param name="userServiceLogger"></param>
        /// <param name="iConnectionSettings"></param>
        /// <param name="iIdentitySettings"></param>
        public CompliancePropertyDocController(ILogger<CompliancePropertyDocController> compliancePropertyDocControllerLogger, ILogger<CompliancePropertyDocService> compliancePropertyDocServiceLogger
            , ILogger<UserService> userServiceLogger, IOptions<ConnectionSettings> iConnectionSettings
            , IOptions<IdentitySettings> iIdentitySettings)
            : base(iConnectionSettings, iIdentitySettings, userServiceLogger)
        {
            logging = new Logging(compliancePropertyDocControllerLogger);
            compliancePropertyDocService = new CompliancePropertyDocService(
                connectionSettings.OperationsDbSqlProvider,
                connectionSettings.OperationsDbConnString,
                identitySettings.JwtSecret,
                identitySettings.JwtExpirationSeconds, compliancePropertyDocServiceLogger);
        }

        /// <summary>
        /// get compliance property doc list
        /// </summary>
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/compliancePropertyDoc/getcompliancePropertyDocs")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(PaginatedList<CompliancePropertyDoc>), description: "Successful operation")]
        public virtual IActionResult GetActiveCompliancePropertyDocList([FromQuery] int proId,[FromQuery] string search, [FromQuery] string sort, [FromQuery] int? limit, [FromQuery] int? page)
        {
            try
            {
                if (compliancePropertyDocService != null)
                {
                    PaginatedList<PropertyCompianceDocsList> compliancePropertyDocs = compliancePropertyDocService.GetActiveCompliancePropertyDocList(proId,search, limit, page, sort);
                    if (compliancePropertyDocs != null)
                        return Ok(compliancePropertyDocs);
                    else
                        return Ok(new { message = "No Data found." });
                }
                else
                    return BadRequest(new { message = "Supplied compliancePropertyDoc information is not valid" });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// get compliance property doc by id
        /// </summary>
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/compliancePropertyDoc/getcompliancePropertyDocbyid")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(CompliancePropertyDoc), description: "Successful operation")]
        public virtual IActionResult GetCompliancePropertyDocById([FromQuery] int compliancePropertyDocId)
        {
            try
            {
                if (compliancePropertyDocService != null && compliancePropertyDocId > 0)
                {
                    CompliancePropertyDoc compliancePropertyDoc = compliancePropertyDocService.GetCompliancePropertyDocById(compliancePropertyDocId);
                    if (compliancePropertyDoc != null)
                        return Ok(compliancePropertyDoc);
                    else
                        return Ok(new { message = "No Data found." });
                }
                else
                    return BadRequest(new { message = "Supplied compliancePropertyDoc information is not valid" });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// add compliance property doc 
        /// </summary>
        /// <remarks>add new compliance property doc information </remarks>
        /// <param name="addCompliancePropertyDoc">The compliance property doc json format.</param>
        [HttpPost]
        [Route("/v1/compliancePropertyDoc/add")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(CompliancePropertyDoc), description: "Successful operation")]
        public virtual IActionResult AddCompliancePropertyDoc([FromBody] CompliancePropertyDoc addCompliancePropertyDoc)
        {
            try
            {
                if (compliancePropertyDocService != null && addCompliancePropertyDoc != null && addCompliancePropertyDoc.DocObject != null && addCompliancePropertyDoc.DocObject.Length > 0)
                {
                    CompliancePropertyDocCommonResponse compliancePropertyDocCommonResponse = compliancePropertyDocService.AddCompliancePropertyDoc(addCompliancePropertyDoc,"");
                    if (compliancePropertyDocCommonResponse == null)
                        return BadRequest(new { message = "Supplied compliancePropertyDoc information is not valid." });
                    else if (!compliancePropertyDocCommonResponse.Success)
                        return BadRequest(new { message = compliancePropertyDocCommonResponse.Message });
                    else
                    {
                        if (compliancePropertyDocCommonResponse.CompliancePropertyDoc != null && compliancePropertyDocCommonResponse.CompliancePropertyDoc.Id > 0)
                            return Ok(compliancePropertyDocCommonResponse.CompliancePropertyDoc);
                        else
                            return BadRequest(new { message = "Supplied compliancePropertyDoc information is not valid." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied compliancePropertyDoc information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// update compliance property doc 
        /// </summary>
        /// <remarks>update compliance property doc information </remarks>
        /// <param name="updateCompliancePropertyDoc">The compliance property doc json format.</param>
        [HttpPut]
        [Route("/v1/compliancePropertyDoc/update")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(UpdateCompliancePropertyDoc), description: "Successful operation")]
        public virtual IActionResult UpdateCompliancePropertyDoc([FromBody] UpdateCompliancePropertyDoc updateCompliancePropertyDoc)
        {
            try
            {
                if (compliancePropertyDocService != null && updateCompliancePropertyDoc != null && updateCompliancePropertyDoc.Id > 0)
                {
                    CompliancePropertyDocCommonResponse compliancePropertyDocCommonResponse = compliancePropertyDocService.UpdateCompliancePropertyDoc(updateCompliancePropertyDoc, CurrentUser.UserName);
                    if (compliancePropertyDocCommonResponse == null)
                        return BadRequest(new { message = "Supplied compliancePropertyDoc information is not valid." });
                    else if (!compliancePropertyDocCommonResponse.Success)
                        return BadRequest(new { message = compliancePropertyDocCommonResponse.Message });
                    else
                    {
                        if (compliancePropertyDocCommonResponse.UpdateCompliancePropertyDoc != null && compliancePropertyDocCommonResponse.UpdateCompliancePropertyDoc.Id > 0)
                            return Ok(compliancePropertyDocCommonResponse.UpdateCompliancePropertyDoc);
                        else
                            return BadRequest(new { message = "Supplied compliancePropertyDoc information is not valid." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied compliancePropertyDoc information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// soft delete compliance property doc 
        /// </summary>
        /// <remarks>Soft delete compliance property doc </remarks>
        [HttpDelete]
        [Route("/v1/compliancePropertyDoc/delete")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult SoftDeleteCompliancePropertyDoc([FromQuery] int compliancePropertyDocId)
        {
            try
            {
                if (compliancePropertyDocService != null && compliancePropertyDocId > 0)
                {
                    CompliancePropertyDocCommonResponse compliancePropertyDocCommonResponse = compliancePropertyDocService.SoftDelete(compliancePropertyDocId, CurrentUser.UserName);

                    if (compliancePropertyDocCommonResponse == null)
                        return BadRequest(new { message = "Supplied compliancePropertyDoc information is not valid." });
                    else if (!compliancePropertyDocCommonResponse.Success)
                        return BadRequest(new { message = compliancePropertyDocCommonResponse.Message });
                    else
                    {
                        if (compliancePropertyDocCommonResponse.UpdateCompliancePropertyDoc != null && compliancePropertyDocCommonResponse.UpdateCompliancePropertyDoc.Id > 0)
                            return Ok(true);
                        else
                            return BadRequest(new { message = "Unable to delete compliancePropertyDoc." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied compliancePropertyDoc information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// forcefully delete compliance property doc 
        /// </summary> 
        /// <remarks>forcefully delete compliance property doc </remarks>
        [HttpDelete]
        [Route("/v1/compliancePropertyDoc/forcedelete")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult HardDeleteCompliancePropertyDoc([FromQuery] int compliancePropertyDocId)
        {
            try
            {
                bool isDeleted = false;
                if (compliancePropertyDocService != null && compliancePropertyDocId > 0)
                    isDeleted = compliancePropertyDocService.HardDelete(compliancePropertyDocId);

                if (isDeleted)
                    return Ok(true);

                return BadRequest(new { message = "Supplied compliancePropertyDoc information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }
    }
}
