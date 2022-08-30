using ds.pms.apicommon.Models;
using ds.pms.apicommon.Settings;
using ds.pms.bl.logger;
using ds.pms.bl.users.IServices;
using ds.pms.bl.users.Models;
using ds.pms.bl.users.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace ds.pms.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private IUserService userService;
        private Logging logging;

        /// <summary>
        /// user controller constructor
        /// </summary>
        /// <param name="userControllerLogger"></param>
        /// <param name="userServiceLogger"></param>
        /// <param name="iConnectionSettings"></param>
        /// <param name="iIdentitySettings"></param>
        public UserController(ILogger<UserController> userControllerLogger, ILogger<UserService> userServiceLogger
            , IOptions<ConnectionSettings> iConnectionSettings, IOptions<IdentitySettings> iIdentitySettings)
            : base(iConnectionSettings, iIdentitySettings, userServiceLogger)
        {
            logging = new Logging(userControllerLogger);
            userService = new UserService(
                connectionSettings.OperationsDbSqlProvider,
                connectionSettings.OperationsDbConnString,
                identitySettings.JwtSecret,
                identitySettings.JwtExpirationSeconds, userServiceLogger);
        }

        /// <summary>
        /// authenticate user
        /// </summary>
        [AllowAnonymous]
        [HttpPost("/v1/user/authenticate")]
        [SwaggerResponse(statusCode: 200, type: typeof(LoginUser), description: "Successful operation")]
        public IActionResult Authenticate(LoginUser loginUser)
        {
            try
            {
                if (userService != null && loginUser != null)
                {
                    UserCommonResponse userCommonResponse = userService.AuthenticateUser(loginUser);
                    if (userCommonResponse == null)
                        return BadRequest(new { message = "Supplied user information is not valid." });
                    else if (!userCommonResponse.Success)
                        return BadRequest(new { message = userCommonResponse.Message });
                    else
                    {
                        if (!string.IsNullOrEmpty(userCommonResponse.Token))
                            return Ok(userCommonResponse.Token);
                        else
                            return BadRequest(new { message = "Unable to generate Token." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied user information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// get user list
        /// </summary>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/user/getusers")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(PaginatedList<User>), description: "Successful operation")]
        public virtual IActionResult GetActiveUsers([FromQuery] string search, [FromQuery] string sort, [FromQuery] int? limit, [FromQuery] int? page)
        {
            try
            {
                PaginatedList<User> users = userService.GetActiveUserList(CurrentUser.ClientId, search, limit, page, sort);
                if (users != null)
                    return Ok(users);
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
        /// get user by id
        /// </summary>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/user/getuserbyid")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(User), description: "Successful operation")]
        public virtual IActionResult GetUserById([FromQuery] int userId)
        {
            try
            {
                if (userId > 0)
                {
                    User user = userService.GetUserById(userId);
                    if (user != null)
                        return Ok(user);
                    else
                        return Ok(new { message = "No Data found." });
                }
                else
                    return BadRequest(new { message = "Supplied user information is not valid" });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }
        /// <summary>
        /// check if username is not in use for new user
        /// </summary>
        [HttpGet]
        [Route("/v1/user/validateusername")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult CheckUsername([FromQuery] int? userId, [FromQuery] string username)
        {
            try
            {
                if (!string.IsNullOrEmpty(username))
                {
                    bool valid = userService.IsValidUsername(userId, username);
                    if (!valid)
                        return BadRequest(new { message = "Username is already in use." });
                    return Ok(valid);
                }
                else
                    return BadRequest(new { message = "Supplied user information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }
        /// <summary>
        /// check if email is not in use for new user
        /// </summary>
        [HttpGet]
        [Route("/v1/user/validateemail")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult CheckEmail([FromQuery] int? userId, [FromQuery] string email)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    bool valid = userService.IsValidEmail(userId, email);
                    if (!valid)
                        return BadRequest(new { message = "Email is already in use." });
                    return Ok(valid);
                }
                else
                    return BadRequest(new { message = "Supplied user information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }
        /// <summary>
        /// get roles by user id
        /// </summary>
        /// <remarks>get roles by user </remarks>
        //[Authorize]
        [HttpGet]
        [Route("/v1/user/getrolesbyuserid")]
        //[ValidateModelState]
        //[SwaggerOperation("ChangePassword")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult GetRolesByUserId([FromQuery] int userId)
        {
            try
            {
                if (userId > 0)
                {
                    var rolesByUserId = userService.GetRolesByUserId(userId);
                    if (rolesByUserId.Count > 0)
                        return Ok(rolesByUserId);
                    else
                        return BadRequest(new { message = "No roles found for the user." });
                }
                else
                    return BadRequest(new { message = "Supplied user information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }
        [Route("/v1/user/getusersbyclient")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(PaginatedList<User>), description: "Successful operation")]
        public virtual IActionResult GetActiveUsersByClient([FromQuery] int clientId, [FromQuery] string search, [FromQuery] string sort, [FromQuery] int? limit, [FromQuery] int? page)
        {
            try
            {
                PaginatedList<User> users = userService.GetUserListByClient(clientId, search, limit, page, sort);
                if (users != null)
                    return Ok(users);
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
        /// add user 
        /// </summary>
        /// <remarks>add new user information </remarks>
        /// <param name="user">The user json format.</param>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("/v1/user/add")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(AddUser), description: "Successful operation")]
        public virtual IActionResult AddUser([FromBody] AddUser user)
        {
            try
            {
                if (userService != null && user != null)
                {
                    UserCommonResponse userCommonResponse;
                    if (CurrentUser != null)
                        userCommonResponse = userService.AddUser(user, CurrentUser.UserName);
                    else
                        userCommonResponse = userService.AddUser(user, null);
                    if (userCommonResponse == null)
                        return BadRequest(new { message = "Supplied user information is not valid." });
                    else if (!userCommonResponse.Success)
                        return BadRequest(new { message = userCommonResponse.Message });
                    else
                    {
                        if (userCommonResponse.AddUser != null && userCommonResponse.AddUser.Id > 0)
                            return Ok(userCommonResponse.AddUser);
                        else
                            return BadRequest(new { message = "Supplied user information is not valid." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied user information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }



        /// <summary>
        /// assign roles to user
        /// </summary>
        /// <remarks>assign roles to user </remarks>
        //[Authorize]
        [HttpPost]
        [Route("/v1/user/assignrolestouser")]
        //[ValidateModelState]
        //[SwaggerOperation("ChangePassword")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult AssignRolesToUser([FromBody] AssignRolesToUser assignRolesToUser)
        {
            try
            {
                bool success = false;
                if (assignRolesToUser != null && assignRolesToUser.UserId > 0 && assignRolesToUser.RoleIds.Count > 0)
                {
                    success = userService.AssignRolesToUser(assignRolesToUser);
                }
                if (success)
                    return Ok(true);

                return BadRequest(new { message = "Supplied user information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// remove roles from user
        /// </summary>
        /// <remarks>remove roles from user</remarks>
        //[Authorize]
        [HttpPost]
        [Route("/v1/user/removerolesfromuser")]
        //[ValidateModelState]
        //[SwaggerOperation("ChangePassword")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult RemoveRolesFromUser([FromBody] AssignRolesToUser assignRolesToUser)
        {
            try
            {
                bool success = false;
                if (assignRolesToUser != null && assignRolesToUser.UserId > 0 && assignRolesToUser.RoleIds.Count > 0)
                {
                    success = userService.RemoveRolesFromUser(assignRolesToUser);
                }
                if (success)
                    return Ok(true);

                return BadRequest(new { message = "Supplied user information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// add provider user
        /// </summary>
        /// <remarks>add provider user</remarks>
        //[Authorize]
        [HttpPost]
        [Route("/v1/user/addprovideruser")]
        //[ValidateModelState]
        //[SwaggerOperation("ChangePassword")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult AddProviderUser([FromQuery] int providerId, [FromQuery] int userId)
        {
            try
            {
                bool success = false;
                if (providerId > 0 && userId > 0)
                {
                    success = userService.AddProviderUser(providerId, userId);
                }
                if (success)
                    return Ok(true);

                return BadRequest(new { message = "Supplied user information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// change password
        /// </summary>
        /// <remarks>change user password </remarks>
        //[Authorize]
        [HttpPost]
        [Route("/v1/user/changepassword")]
        //[ValidateModelState]
        //[SwaggerOperation("ChangePassword")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult ChangePassword([FromQuery] int userId, [FromQuery] string password)
        {
            try
            {
                bool success = false;
                if (userId > 0 && string.IsNullOrEmpty(password))
                {
                    success = userService.ChangePassword(CurrentUser.ClientId, userId, password, CurrentUser.Id);
                }
                if (success)
                    return Ok(true);

                return BadRequest(new { message = "Supplied user information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }
        /// <summary>
        /// update user 
        /// </summary>
        /// <remarks>update user information </remarks>
        /// <param name="user">The user json format.</param>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        [Route("/v1/user/update")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(UpdateUser), description: "Successful operation")]
        public virtual IActionResult UpdateUser([FromBody] UpdateUser user)
        {
            try
            {
                if (userService != null && user != null && user.Id > 0)
                {
                    UserCommonResponse userCommonResponse = userService.UpdateUser(user, CurrentUser.UserName);
                    if (userCommonResponse == null)
                        return BadRequest(new { message = "Supplied user information is not valid." });
                    else if (!userCommonResponse.Success)
                        return BadRequest(new { message = userCommonResponse.Message });
                    else
                    {
                        if (userCommonResponse.UpdateUser != null && userCommonResponse.UpdateUser.Id > 0)
                            return Ok(userCommonResponse.UpdateUser);
                        else
                            return BadRequest(new { message = "Supplied user information is not valid." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied user information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }


        
       
        /// <summary>
        /// soft delete user 
        /// </summary>
        /// <remarks>Soft delete user </remarks>
        [HttpDelete]
        [Route("/v1/user/delete")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult SoftDeleteUser([FromQuery] int userId)
        {
            try
            {
                if (userId > 0)
                {
                    UserCommonResponse userCommonResponse = userService.SoftDelete(userId, CurrentUser.UserName);

                    if (userCommonResponse == null)
                        return BadRequest(new { message = "Supplied user information is not valid." });
                    else if (!userCommonResponse.Success)
                        return BadRequest(new { message = userCommonResponse.Message });
                    else
                    {
                        if (userCommonResponse.UpdateUser != null && userCommonResponse.UpdateUser.Id > 0)
                            return Ok(true);
                        else
                            return BadRequest(new { message = "Unable to delete user." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied user information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// forcefully delete user 
        /// </summary>
        /// <remarks>forcefully delete user </remarks>
        [HttpDelete]
        [Route("/v1/user/forcedelete")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult HardDeleteUser([FromQuery] int userId)
        {
            try
            {
                bool isDeleted = false;
                if (userId > 0)
                    isDeleted = userService.HardDelete(userId);

                if (isDeleted)
                    return Ok(true);

                return BadRequest(new { message = "Supplied user information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

     
        
    }
}
