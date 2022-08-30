using ds.pms.apicommon.Models;
using ds.pms.apicommon.Settings;
using ds.pms.bl.logger;
using ds.pms.bl.providers.IServices;
using ds.pms.bl.providers.Models;
using ds.pms.bl.providers.Services;
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
    public class ProviderController : BaseController
    {
        private IProviderService providerService;
        private Logging logging;

        /// <summary>
        /// Provider controller constructor
        /// </summary>
        /// <param name="providerControllerLogger"></param>
        /// <param name="providerServiceLogger"></param>
        /// <param name="userServiceLogger"></param>
        /// <param name="iConnectionSettings"></param>
        /// <param name="iIdentitySettings"></param>
        public ProviderController(ILogger<ProviderController> providerControllerLogger, ILogger<ProviderService> providerServiceLogger
            , ILogger<UserService> userServiceLogger, IOptions<ConnectionSettings> iConnectionSettings
            , IOptions<IdentitySettings> iIdentitySettings)
            : base(iConnectionSettings, iIdentitySettings, userServiceLogger)
        {
            logging = new Logging(providerControllerLogger);
            providerService = new ProviderService(
                connectionSettings.OperationsDbSqlProvider,
                connectionSettings.OperationsDbConnString,
                identitySettings.JwtSecret,
                identitySettings.JwtExpirationSeconds, providerServiceLogger);
        }

        /// <summary>
        /// get provider list
        /// </summary>
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/provider/getproviders")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(PaginatedList<Provider>), description: "Successful operation")]
        public virtual IActionResult GetActiveProviderList([FromQuery] int clientId, [FromQuery] string search, [FromQuery] string sort, [FromQuery] int? limit, [FromQuery] int? page)
        {
            try
            {
                PaginatedList<Provider> providers = providerService.GetActiveProviderList(clientId, search, limit, page, sort);
                if (providers != null)
                    return Ok(providers);
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
        /// get provider by id
        /// </summary>
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/provider/getproviderbyid")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(Provider), description: "Successful operation")]
        public virtual IActionResult GetProviderById([FromQuery] int providerId)
        {
            try
            {
                if (providerId > 0)
                {
                    Provider provider = providerService.GetProviderById(providerId);
                    if (provider != null)
                        return Ok(provider);
                    else
                        return Ok(new { message = "No Data found." });
                }
                else
                    return BadRequest(new { message = "Supplied provider information is not valid" });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }
        [HttpGet]
        [Route("/v1/provider/validateemail")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult CheckEmail([FromQuery] long? providerId, [FromQuery] string email)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    bool valid = providerService.IsValidEmail(providerId, email);
                    if (!valid)
                        return BadRequest(new { message = "Email is already in use." });
                    return Ok(valid);
                }
                else
                    return BadRequest(new { message = "Supplied provider information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }
        [Route("/v1/provider/getprovideruserbyid")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(ProviderUser), description: "Successful operation")]
        public virtual IActionResult GetProviderUserById([FromQuery] int providerId)
        {
            try
            {
                if (providerId > 0)
                {
                    ProviderUser provider = providerService.GetProiderUserById(providerId);
                    if (provider != null)
                        return Ok(provider);
                    else
                        return Ok(new { message = "No Data found." });
                }
                else
                    return BadRequest(new { message = "Supplied provider information is not valid" });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }
        /// <summary>
        /// add provider 
        /// </summary>
        /// <remarks>add new provider information </remarks>
        /// <param name="addProvider">The provider json format.</param>
        [HttpPost]
        [Route("/v1/provider/add")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(Provider), description: "Successful operation")]
        public virtual IActionResult AddProvider([FromBody] Provider addProvider)
        {
            try
            {
                if (providerService != null && addProvider != null)
                {
                    ProviderCommonResponse providerCommonResponse = providerService.AddProvider(addProvider, CurrentUser.UserName);
                    if (providerCommonResponse == null)
                        return BadRequest(new { message = "Supplied provider information is not valid." });
                    else if (!providerCommonResponse.Success)
                        return BadRequest(new { message = providerCommonResponse.Message });
                    else
                    {
                        if (providerCommonResponse.Provider != null && providerCommonResponse.Provider.Id > 0)
                            return Ok(providerCommonResponse.Provider);
                        else
                            return BadRequest(new { message = "Supplied provider information is not valid." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied provider information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }
        [HttpPost]
        [Route("/v1/provider/addprovideruser")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(ProviderUser), description: "Successful operation")]
        public virtual IActionResult AddProviderUser([FromBody] ProviderUser addProvider)
        {
            try
            {
                if (providerService != null && addProvider != null)
                {
                    ProviderCommonResponse providerCommonResponse = providerService.AddUpdateProviderUser(addProvider);
                    if (providerCommonResponse == null)
                        return BadRequest(new { message = "Supplied provider information is not valid." });
                    else if (!providerCommonResponse.Success)
                        return BadRequest(new { message = providerCommonResponse.Message });
                    else
                    {
                        if (providerCommonResponse.ProviderUser != null && providerCommonResponse.ProviderUser.Id > 0)
                            return Ok(providerCommonResponse.ProviderUser);
                        else
                            return BadRequest(new { message = "Supplied provider information is not valid." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied provider information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }



        [HttpPut]
        [Route("/v1/provider/updateprovideruser")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(ProviderUser), description: "Successful operation")]
        public virtual IActionResult UpdateProviderUser([FromBody] ProviderUser addProvider)
        {
            try
            {
                if (providerService != null && addProvider != null)
                {
                    ProviderCommonResponse providerCommonResponse = providerService.AddUpdateProviderUser(addProvider);
                    if (providerCommonResponse == null)
                        return BadRequest(new { message = "Supplied provider information is not valid." });
                    else if (!providerCommonResponse.Success)
                        return BadRequest(new { message = providerCommonResponse.Message });
                    else
                    {
                        if (providerCommonResponse.Provider != null && providerCommonResponse.Provider.Id > 0)
                            return Ok(providerCommonResponse.Provider);
                        else
                            return BadRequest(new { message = "Supplied provider information is not valid." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied provider information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }
        /// <summary>
        /// update provider 
        /// </summary>
        /// <remarks>update provider information </remarks>
        /// <param name="updateProvider">The provider json format.</param>
        [HttpPut]
        [Route("/v1/Provider/update")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(UpdateProvider), description: "Successful operation")]
        public virtual IActionResult UpdateProvider([FromBody] UpdateProvider updateProvider)
        {
            try
            {
                if (providerService != null && updateProvider != null && updateProvider.Id > 0)
                {
                    ProviderCommonResponse providerCommonResponse = providerService.UpdateProvider(updateProvider, CurrentUser.UserName);
                    if (providerCommonResponse == null)
                        return BadRequest(new { message = "Supplied provider information is not valid." });
                    else if (!providerCommonResponse.Success)
                        return BadRequest(new { message = providerCommonResponse.Message });
                    else
                    {
                        if (providerCommonResponse.UpdateProvider != null && providerCommonResponse.UpdateProvider.Id > 0)
                            return Ok(providerCommonResponse.UpdateProvider);
                        else
                            return BadRequest(new { message = "Supplied provider information is not valid." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied provider information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// check if email is not in use for new provider
        /// </summary>
       
        /// <summary>
        /// soft delete provider 
        /// </summary>
        /// <remarks>Soft delete provider </remarks>
        [HttpDelete]
        [Route("/v1/provider/delete")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult SoftDeleteProvider([FromQuery] int providerId)
        {
            try
            {
                if (providerId > 0)
                {
                    ProviderCommonResponse providerCommonResponse = providerService.SoftDelete(providerId, CurrentUser.UserName);

                    if (providerCommonResponse == null)
                        return BadRequest(new { message = "Supplied provider information is not valid." });
                    else if (!providerCommonResponse.Success)
                        return BadRequest(new { message = providerCommonResponse.Message });
                    else
                    {
                        if (providerCommonResponse.UpdateProvider != null && providerCommonResponse.UpdateProvider.Id > 0)
                            return Ok(true);
                        else
                            return BadRequest(new { message = "Unable to delete provider." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied provider information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// forcefully delete provider 
        /// </summary>
        /// <remarks>forcefully delete provider </remarks>
        [HttpDelete]
        [Route("/v1/provider/forcedelete")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult HardDeleteProvider([FromQuery] int providerId)
        {
            try
            {
                bool isDeleted = false;
                if (providerId > 0)
                    isDeleted = providerService.HardDelete(providerId);

                if (isDeleted)
                    return Ok(true);

                return BadRequest(new { message = "Supplied provider information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }
       
       
    }
}
