using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ds.pms.apicommon.Models;
using ds.pms.apicommon.Settings;
using ds.pms.bl.logger;
using ds.pms.bl.tenants.IServices;
using ds.pms.bl.tenants.Models;
using ds.pms.bl.tenants.Services;
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
    public class TenantController : BaseController
    {
        private ITenantService tenantService;
        private Logging logging;

        /// <summary>
        /// Tenant controller constructor
        /// </summary>
        /// <param name="tenantControllerLogger"></param>
        /// <param name="tenantServiceLogger"></param>
        /// <param name="userServiceLogger"></param>
        /// <param name="iConnectionSettings"></param>
        /// <param name="iIdentitySettings"></param>
        public TenantController(ILogger<TenantController> tenantControllerLogger, ILogger<TenantService> tenantServiceLogger
            , ILogger<UserService> userServiceLogger, IOptions<ConnectionSettings> iConnectionSettings
            , IOptions<IdentitySettings> iIdentitySettings)
            : base(iConnectionSettings, iIdentitySettings, userServiceLogger)
        {
            logging = new Logging(tenantControllerLogger);
            tenantService = new TenantService(
                connectionSettings.OperationsDbSqlProvider,
                connectionSettings.OperationsDbConnString,
                identitySettings.JwtSecret,
                identitySettings.JwtExpirationSeconds, tenantServiceLogger);
        }

        /// <summary>
        /// get tenant list
        /// </summary>
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/tenant/gettenants")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(PaginatedList<Tenant>), description: "Successful operation")]
        public virtual IActionResult GetActiveTenantList([FromQuery] int clientId, [FromQuery] string search, [FromQuery] string sort, [FromQuery] int? limit, [FromQuery] int? page)
        {
            try
            {
                PaginatedList<Tenant> tenants = tenantService.GetActiveTenantList(clientId,search, limit, page, sort);
                if (tenants != null)
                    return Ok(tenants);
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
        /// get tenant by id
        /// </summary>
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/tenant/gettenantbyid")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(Tenant), description: "Successful operation")]
        public virtual IActionResult GetTenantById([FromQuery] int tenantId)
        {
            try
            {
                if (tenantId > 0)
                {
                    Tenant Tenant = tenantService.GetTenantById(tenantId);
                    if (Tenant != null)
                        return Ok(Tenant);
                    else
                        return Ok(new { message = "No Data found." });
                }
                else
                    return BadRequest(new { message = "Supplied tenant information is not valid" });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// add tenant 
        /// </summary>
        /// <remarks>add new tenant information </remarks>
        /// <param name="addTenant">The tenant json format.</param>
        [HttpPost]
        [Route("/v1/tenant/add")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(Tenant), description: "Successful operation")]
        public virtual IActionResult AddTenant([FromBody] Tenant addTenant)
        {
            try
            {
                if (tenantService != null && addTenant != null)
                {
                    TenantCommonResponse tenantCommonResponse = tenantService.AddTenant(addTenant, CurrentUser.UserName);
                    if (tenantCommonResponse == null)
                        return BadRequest(new { message = "Supplied tenant information is not valid." });
                    else if (!tenantCommonResponse.Success)
                        return BadRequest(new { message = tenantCommonResponse.Message });
                    else
                    {
                        if (tenantCommonResponse.Tenant != null && tenantCommonResponse.Tenant.Id > 0)
                            return Ok(tenantCommonResponse.Tenant);
                        else
                            return BadRequest(new { message = "Supplied tenant information is not valid." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied tenant information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// update tenant 
        /// </summary>
        /// <remarks>update tenant information </remarks>
        /// <param name="updateTenant">The tenant json format.</param>
        [HttpPut]
        [Route("/v1/tenant/update")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(Tenant), description: "Successful operation")]
        public virtual IActionResult UpdateTenant([FromBody] Tenant updateTenant)
        {
            try
            {
                if (tenantService != null && updateTenant != null && updateTenant.Id > 0)
                {
                    TenantCommonResponse tenantCommonResponse = tenantService.UpdateTenant(updateTenant, CurrentUser.UserName);
                    if (tenantCommonResponse == null)
                        return BadRequest(new { message = "Supplied tenant information is not valid." });
                    else if (!tenantCommonResponse.Success)
                        return BadRequest(new { message = tenantCommonResponse.Message });
                    else
                    {
                        if (tenantCommonResponse.Tenant != null && tenantCommonResponse.Tenant.Id > 0)
                            return Ok(tenantCommonResponse.Tenant);
                        else
                            return BadRequest(new { message = "Supplied tenant information is not valid." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied tenant information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// soft delete tenant 
        /// </summary>
        /// <remarks>Soft delete tenant </remarks>
        [HttpDelete]
        [Route("/v1/tenant/delete")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult SoftDeleteTenant([FromQuery] int tenantId)
        {
            try
            {
                if (tenantId > 0)
                {
                    TenantCommonResponse tenantCommonResponse = tenantService.SoftDelete(tenantId, CurrentUser.UserName);

                    if (tenantCommonResponse == null)
                        return BadRequest(new { message = "Supplied tenant information is not valid." });
                    else if (!tenantCommonResponse.Success)
                        return BadRequest(new { message = tenantCommonResponse.Message });
                    else
                    {
                        if (tenantCommonResponse.Tenant != null && tenantCommonResponse.Tenant.Id > 0)
                            return Ok(true);
                        else
                            return BadRequest(new { message = "Unable to delete Tenant." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied tenant information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// forcefully delete tenant 
        /// </summary>
        /// <remarks>forcefully delete tenant </remarks>
        [HttpDelete]
        [Route("/v1/tenant/forcedelete")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult HardDeleteTenant([FromQuery] int tenantId)
        {
            try
            {
                bool isDeleted = false;
                if (tenantId > 0)
                    isDeleted = tenantService.HardDelete(tenantId);

                if (isDeleted)
                    return Ok(true);

                return BadRequest(new { message = "Supplied tenant information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }
    }
}
