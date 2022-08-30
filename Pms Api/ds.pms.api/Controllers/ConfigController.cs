using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ds.pms.apicommon.Models;
using ds.pms.apicommon.Settings;
using ds.pms.bl.configs.IServices;
using ds.pms.bl.configs.Models;
using ds.pms.bl.configs.Services;
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
    public class ConfigController : BaseController
    {
        private IConfigService configService;
        private Logging logging;

        /// <summary>
        /// Config controller constructor
        /// </summary>
        /// <param name="configControllerLogger"></param>
        /// <param name="configServiceLogger"></param>
        /// <param name="userServiceLogger"></param>
        /// <param name="iConnectionSettings"></param>
        /// <param name="iIdentitySettings"></param>
        public ConfigController(ILogger<ConfigController> configControllerLogger, ILogger<ConfigService> configServiceLogger
            , ILogger<UserService> userServiceLogger, IOptions<ConnectionSettings> iConnectionSettings
            , IOptions<IdentitySettings> iIdentitySettings)
            : base(iConnectionSettings, iIdentitySettings, userServiceLogger)
        {
            logging = new Logging(configControllerLogger);
            configService = new ConfigService(
                connectionSettings.OperationsDbSqlProvider,
                connectionSettings.OperationsDbConnString,
                identitySettings.JwtSecret,
                identitySettings.JwtExpirationSeconds, configServiceLogger);
        }

        /// <summary>
        /// get config list
        /// </summary>
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/config/getconfigs")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(PaginatedList<Config>), description: "Successful operation")]
        public virtual IActionResult GetActiveConfigList([FromQuery] int clientId, [FromQuery] string search, [FromQuery] string sort, [FromQuery] int? limit, [FromQuery] int? page)
        {
            try
            {
                if (configService != null)
                {
                    PaginatedList<Config> configs = configService.GetActiveConfigList(clientId,search, limit, page, sort);
                    if (configs != null)
                        return Ok(configs);
                    else
                        return Ok(new { message = "No Data found." });
                }
                else
                    return BadRequest(new { message = "Supplied config information is not valid" });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// get config by id
        /// </summary>
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/config/getconfigbyid")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(Config), description: "Successful operation")]
        public virtual IActionResult GetConfigById([FromQuery] int id)
        {
            try
            {
                if (configService != null && id > 0)
                {
                    Config config = configService.GetConfigById(id);
                    if (config != null)
                        return Ok(config);
                    else
                        return Ok(new { message = "No Data found." });
                }
                else
                    return BadRequest(new { message = "Supplied config information is not valid" });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// add config 
        /// </summary>
        /// <remarks>add new config information </remarks>
        /// <param name="addConfig">The config json format.</param>
        [HttpPost]
        [Route("/v1/config/add")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(Config), description: "Successful operation")]
        public virtual IActionResult AddConfig([FromBody] Config addConfig)
        {
            try
            {
                if (configService != null && addConfig != null)
                {
                    ConfigCommonResponse configCommonResponse = configService.AddConfig(addConfig, CurrentUser.UserName);
                    if (configCommonResponse == null)
                        return BadRequest(new { message = "Supplied config information is not valid." });
                    else if (!configCommonResponse.Success)
                        return BadRequest(new { message = configCommonResponse.Message });
                    else
                    {
                        if (configCommonResponse.Config != null && configCommonResponse.Config.Id > 0)
                            return Ok(configCommonResponse.Config);
                        else
                            return BadRequest(new { message = "Supplied config information is not valid." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied config information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// update config 
        /// </summary>
        /// <remarks>update config information </remarks>
        /// <param name="updateConfig">The config json format.</param>
        [HttpPut]
        [Route("/v1/config/update")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(Config), description: "Successful operation")]
        public virtual IActionResult UpdateConfig([FromBody] Config updateConfig)
        {
            try
            {
                if (configService != null && updateConfig != null && updateConfig.Id > 0)
                {
                    ConfigCommonResponse configCommonResponse = configService.UpdateConfig(updateConfig, CurrentUser.UserName);
                    if (configCommonResponse == null)
                        return BadRequest(new { message = "Supplied config information is not valid." });
                    else if (!configCommonResponse.Success)
                        return BadRequest(new { message = configCommonResponse.Message });
                    else
                    {
                        if (configCommonResponse.Config != null && configCommonResponse.Config.Id > 0)
                            return Ok(configCommonResponse.Config);
                        else
                            return BadRequest(new { message = "Supplied config information is not valid." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied config information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// soft delete config 
        /// </summary>
        /// <remarks>Soft delete config </remarks>
        [HttpDelete]
        [Route("/v1/config/delete")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult SoftDeleteConfig([FromQuery] int configId)
        {
            try
            {
                if (configService != null && configId > 0)
                {
                    ConfigCommonResponse configCommonResponse = configService.SoftDelete(configId, CurrentUser.UserName);

                    if (configCommonResponse == null)
                        return BadRequest(new { message = "Supplied config information is not valid." });
                    else if (!configCommonResponse.Success)
                        return BadRequest(new { message = configCommonResponse.Message });
                    else
                    {
                        if (configCommonResponse.Config != null && configCommonResponse.Config.Id > 0)
                            return Ok(true);
                        else
                            return BadRequest(new { message = "Unable to delete config." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied config information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// forcefully delete config 
        /// </summary>
        /// <remarks>forcefully delete config </remarks>
        [HttpDelete]
        [Route("/v1/config/forcedelete")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult HardDeleteConfig([FromQuery] int configId)
        {
            try
            {
                bool isDeleted = false;
                if (configService != null && configId > 0)
                    isDeleted = configService.HardDelete(configId);

                if (isDeleted)
                    return Ok(true);

                return BadRequest(new { message = "Supplied config information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }
    }
}
