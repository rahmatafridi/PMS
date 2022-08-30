using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ds.pms.apicommon.Models;
using ds.pms.apicommon.Settings;
using ds.pms.bl.logger;
using ds.pms.bl.properties.IService;
using ds.pms.bl.properties.Models;
using ds.pms.bl.properties.Service;
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
    public class PropertyController : BaseController
    {
        private IPropertyService propertyService;
        private Logging logging;

        /// <summary>
        /// Property controller constructor
        /// </summary>
        /// <param name="propertyControllerLogger"></param>
        /// <param name="propertyServiceLogger"></param>
        /// <param name="userServiceLogger"></param>
        /// <param name="iConnectionSettings"></param>
        /// <param name="iIdentitySettings"></param>
        public PropertyController(ILogger<PropertyController> propertyControllerLogger, ILogger<PropertyService> propertyServiceLogger
            , ILogger<UserService> userServiceLogger, IOptions<ConnectionSettings> iConnectionSettings
            , IOptions<IdentitySettings> iIdentitySettings)
            : base(iConnectionSettings, iIdentitySettings, userServiceLogger)
        {
            logging = new Logging(propertyControllerLogger);
            propertyService = new PropertyService(
                connectionSettings.OperationsDbSqlProvider,
                connectionSettings.OperationsDbConnString,
                identitySettings.JwtSecret,
                identitySettings.JwtExpirationSeconds, propertyServiceLogger);
        }

        /// <summary>
        /// get property list
        /// </summary>
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/property/getproperties")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(PaginatedList<Property>), description: "Successful operation")]
        public virtual IActionResult GetActivePropertyList([FromQuery] int clientId, [FromQuery] int providerId, [FromQuery] string search, [FromQuery] string sort, [FromQuery] int? limit, [FromQuery] int? page)
        {
            try
            {
                if (propertyService != null)
                {
                    PaginatedList<Property> Properties = propertyService.GetActivePropertyList(clientId, providerId, search, limit, page, sort);
                    if (Properties != null)
                        return Ok(Properties);
                    else
                        return Ok(new { message = "No Data found." });
                }
                else
                    return BadRequest(new { message = "Supplied property information is not valid" });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// get property by id
        /// </summary>
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/property/getpropertybyid")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(Property), description: "Successful operation")]
        public virtual IActionResult GetPropertyById([FromQuery] int propertyId)
        {
            try
            {
                if (propertyService != null && propertyId > 0)
                {
                    Property Property = propertyService.GetPropertyById(propertyId);
                    if (Property != null)
                        return Ok(Property);
                    else
                        return Ok(new { message = "No Data found." });
                }
                else
                    return BadRequest(new { message = "Supplied property information is not valid" });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// add property 
        /// </summary>
        /// <remarks>add new property information </remarks>
        /// <param name="addProperty">The property json format.</param>
        [HttpPost]
        [Route("/v1/property/add")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(Property), description: "Successful operation")]
        public virtual IActionResult AddProperty([FromBody] Property addProperty)
        {
            try
            {
                if (propertyService != null && addProperty != null)
                {
                    PropertyCommonResponse propertyCommonResponse = propertyService.AddProperty(addProperty, CurrentUser.UserName);
                    if (propertyCommonResponse == null)
                        return BadRequest(new { message = "Supplied property information is not valid." });
                    else if (!propertyCommonResponse.Success)
                        return BadRequest(new { message = propertyCommonResponse.Message });
                    else
                    {
                        if (propertyCommonResponse.Property != null && propertyCommonResponse.Property.Id > 0)
                            return Ok(propertyCommonResponse.Property);
                        else
                            return BadRequest(new { message = "Supplied property information is not valid." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied property information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }
        [HttpPost]
        [Route("/v1/property/addcomplianceproperty")]
        //[ValidateModelState]
        //[SwaggerOperation("ChangePassword")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult AddComplianceProperty([FromBody] AddComplianceProperty addComplianceProperty)
        {
            try
            {
                bool success = false;
                if (propertyService != null && addComplianceProperty != null && addComplianceProperty.PropertyId > 0 && addComplianceProperty.ComplianceIds.Count > 0)
                {
                    success = propertyService.AddComplianceProperty(addComplianceProperty);
                }
                if (success)
                    return Ok(true);

                return BadRequest(new { message = "Supplied compliance property information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }
        /// <summary>
        /// update property 
        /// </summary>
        /// <remarks>update property information </remarks>
        /// <param name="updateProperty">The property json format.</param>
        [HttpPut]
        [Route("/v1/property/update")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(Property), description: "Successful operation")]
        public virtual IActionResult UpdateProperty([FromBody] Property updateProperty)
        {
            try
            {
                if (propertyService != null && updateProperty != null && updateProperty.Id > 0)
                {
                    PropertyCommonResponse propertyCommonResponse = propertyService.UpdateProperty(updateProperty, CurrentUser.UserName);
                    if (propertyCommonResponse == null)
                        return BadRequest(new { message = "Supplied property information is not valid." });
                    else if (!propertyCommonResponse.Success)
                        return BadRequest(new { message = propertyCommonResponse.Message });
                    else
                    {
                        if (propertyCommonResponse.Property != null && propertyCommonResponse.Property.Id > 0)
                            return Ok(propertyCommonResponse.Property);
                        else
                            return BadRequest(new { message = "Supplied property information is not valid." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied property information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// soft delete property 
        /// </summary>
        /// <remarks>Soft delete property </remarks>
        [HttpDelete]
        [Route("/v1/property/delete")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult SoftDeleteProperty([FromQuery] int propertyId)
        {
            try
            {
                if (propertyService != null && propertyId > 0)
                {
                    PropertyCommonResponse propertyCommonResponse = propertyService.SoftDelete(propertyId, CurrentUser.UserName);

                    if (propertyCommonResponse == null)
                        return BadRequest(new { message = "Supplied property information is not valid." });
                    else if (!propertyCommonResponse.Success)
                        return BadRequest(new { message = propertyCommonResponse.Message });
                    else
                    {
                        if (propertyCommonResponse.Property != null && propertyCommonResponse.Property.Id > 0)
                            return Ok(true);
                        else
                            return BadRequest(new { message = "Unable to delete Property." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied property information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// forcefully delete property 
        /// </summary>
        /// <remarks>forcefully delete property </remarks>
        [HttpDelete]
        [Route("/v1/property/forcedelete")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult HardDeleteProperty([FromQuery] int propertyId)
        {
            try
            {
                bool isDeleted = false;
                if (propertyService != null && propertyId > 0)
                    isDeleted = propertyService.HardDelete(propertyId);

                if (isDeleted)
                    return Ok(true);

                return BadRequest(new { message = "Supplied property information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// add compliance property
        /// </summary>
        /// <remarks>add compliance property </remarks>
        //[Authorize]
       
    }
}
