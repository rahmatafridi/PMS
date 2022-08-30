using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ds.pms.apicommon.Models;
using ds.pms.apicommon.Settings;
using ds.pms.bl.logger;
using ds.pms.bl.roles.IServices;
using ds.pms.bl.roles.Models;
using ds.pms.bl.roles.Services;
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
    public class RoleController : BaseController
    {
        private IRoleService roleService;
        private Logging logging;

        /// <summary>
        /// role controller constructor
        /// </summary>
        /// <param name="roleControllerLogger"></param>
        /// <param name="roleServiceLogger"></param>
        /// <param name="userServiceLogger"></param>
        /// <param name="iConnectionSettings"></param>
        /// <param name="iIdentitySettings"></param>
        public RoleController(ILogger<RoleController> roleControllerLogger, ILogger<RoleService> roleServiceLogger
            , ILogger<UserService> userServiceLogger, IOptions<ConnectionSettings> iConnectionSettings
            , IOptions<IdentitySettings> iIdentitySettings)
            : base(iConnectionSettings, iIdentitySettings, userServiceLogger)
        {
            logging = new Logging(roleControllerLogger);
            roleService = new RoleService(
                connectionSettings.OperationsDbSqlProvider,
                connectionSettings.OperationsDbConnString,
                identitySettings.JwtSecret,
                identitySettings.JwtExpirationSeconds, roleServiceLogger);
        }

        /// <summary>
        /// get role list
        /// </summary>
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/role/getroles")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(PaginatedList<Role>), description: "Successful operation")]
        public virtual IActionResult GetActiveRoleList([FromQuery] int clientId, [FromQuery] string search, [FromQuery] string sort, [FromQuery] int? limit, [FromQuery] int? page)
        {
            try
            {
                PaginatedList<Role> roles = roleService.GetActiveRoleList(clientId, search, limit, page, sort);
                if (roles != null)
                    return Ok(roles);
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
        /// get role by id
        /// </summary>
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/role/getrolebyid")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(Role), description: "Successful operation")]
        public virtual IActionResult GetRoleById([FromQuery] int roleId)
        {
            try
            {
                if (roleId > 0)
                {
                    Role role = roleService.GetRoleById(roleId);
                    if (role != null)
                        return Ok(role);
                    else
                        return Ok(new { message = "No Data found." });
                }
                else
                    return BadRequest(new { message = "Supplied role information is not valid" });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// get role by client id
        /// </summary>
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/role/getrolesbyclientid")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(List<Role>), description: "Successful operation")]
        public virtual IActionResult GetRolesByClientId([FromQuery] int clientId)
        {
            try
            {
                if (clientId > 0)
                {
                    List<Role> roles = roleService.GetRolesByClientId(clientId);
                    if (roles != null && roles.Count > 0)
                        return Ok(roles);
                    else
                        return Ok(new { message = "No Data found." });
                }
                else
                    return BadRequest(new { message = "Supplied role information is not valid" });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// check if rolename is not in use for new role
        /// </summary>
        [HttpGet]
        [Route("/v1/role/validaterolename")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult CheckRoleName([FromQuery] int id, [FromQuery] int clientId, [FromQuery] string name)
        {
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    bool valid = roleService.IsValidRoleName(id, clientId, name);
                    if (!valid)
                        return BadRequest(new { message = "rolename is already in use." });
                    return Ok(valid);
                }
                else
                    return BadRequest(new { message = "Supplied role information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        [Route("/v1/role/getpermission")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(List<RoleFeatureModel>), description: "Successful operation")]
        public virtual IActionResult GetPermission([FromQuery] string roleId, [FromQuery] int clientId)
        {
            try
            {
                if (roleService != null)
                {
                    List <RoleFeatureModel> data  = roleService.LoadPermission(Convert.ToInt32(roleId), clientId);
                    if (data != null)
                        return Ok(data);
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

        /// <summary>
        /// add role 
        /// </summary>
        /// <remarks>add new role information </remarks>
        /// <param name="addRole">The role json format.</param>
        [HttpPost]
        [Route("/v1/role/add")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(Role), description: "Successful operation")]
        public virtual IActionResult AddRole([FromBody] Role addRole)
        {
            try
            {
                if (roleService != null && addRole != null)
                {
                    RoleCommonResponse roleCommonResponse = roleService.AddRole(addRole, CurrentUser.UserName);
                    if (roleCommonResponse == null)
                        return BadRequest(new { message = "Supplied role information is not valid." });
                    else if (!roleCommonResponse.Success)
                        return BadRequest(new { message = roleCommonResponse.Message });
                    else
                    {
                        if (roleCommonResponse.Role != null && roleCommonResponse.Role.Id > 0)
                            return Ok(roleCommonResponse.Role);
                        else
                            return BadRequest(new { message = "Supplied role information is not valid." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied role information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        [HttpPost]
        [Route("/v1/role/addpermission")]
        public virtual IActionResult AddPermission([FromBody] List<String> lines)
        {
            try
            {
                if (roleService != null && lines != null)
                {
                    var data = roleService.AddPermission(lines);
                    if (data == null)
                        return BadRequest(new { message = "Supplied role information is not valid." });
                   
                    else
                    {
                        if (data != null)
                            return Ok(data);
                        else
                            return BadRequest(new { message = "Supplied role information is not valid." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied role information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// update role 
        /// </summary>
        /// <remarks>update role information </remarks>
        /// <param name="updateRole">The role json format.</param>
        [HttpPut]
        [Route("/v1/role/update")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(UpdateRole), description: "Successful operation")]
        public virtual IActionResult UpdateRole([FromBody] UpdateRole updateRole)
        {
            try
            {
                if (roleService != null && updateRole != null && updateRole.Id > 0)
                {
                    RoleCommonResponse roleCommonResponse = roleService.UpdateRole(updateRole, CurrentUser.UserName);
                    if (roleCommonResponse == null)
                        return BadRequest(new { message = "Supplied role information is not valid." });
                    else if (!roleCommonResponse.Success)
                        return BadRequest(new { message = roleCommonResponse.Message });
                    else
                    {
                        if (roleCommonResponse.UpdateRole != null && roleCommonResponse.UpdateRole.Id > 0)
                            return Ok(roleCommonResponse.UpdateRole);
                        else
                            return BadRequest(new { message = "Supplied role information is not valid." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied role information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// soft delete role 
        /// </summary>
        /// <remarks>Soft delete role </remarks>
        [HttpDelete]
        [Route("/v1/role/delete")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult SoftDeleteRole([FromQuery] int roleId)
        {
            try
            {
                if (roleId > 0)
                {
                    RoleCommonResponse clientCommonResponse = roleService.SoftDelete(roleId, CurrentUser.UserName);

                    if (clientCommonResponse == null)
                        return BadRequest(new { message = "Supplied role information is not valid." });
                    else if (!clientCommonResponse.Success)
                        return BadRequest(new { message = clientCommonResponse.Message });
                    else
                    {
                        if (clientCommonResponse.UpdateRole != null && clientCommonResponse.UpdateRole.Id > 0)
                            return Ok(true);
                        else
                            return BadRequest(new { message = "Unable to delete role." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied role information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }
    }
}
