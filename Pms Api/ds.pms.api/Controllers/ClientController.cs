using ds.pms.apicommon.Models;
using ds.pms.apicommon.Settings;
using ds.pms.bl.clients.IServices;
using ds.pms.bl.clients.Models;
using ds.pms.bl.logger;
using ds.pms.bl.users.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using pms.bl.clients.Services;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace ds.pms.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : BaseController
    {
        private IClientService clientService;
        private Logging logging;

        /// <summary>
        /// client controller constructor
        /// </summary>
        /// <param name="clientControllerLogger"></param>
        /// <param name="clientServiceLogger"></param>
        /// <param name="userServiceLogger"></param>
        /// <param name="iConnectionSettings"></param>
        /// <param name="iIdentitySettings"></param>
        public ClientController(ILogger<ClientController> clientControllerLogger, ILogger<ClientService> clientServiceLogger
            , ILogger<UserService> userServiceLogger, IOptions<ConnectionSettings> iConnectionSettings
            , IOptions<IdentitySettings> iIdentitySettings)
            : base(iConnectionSettings, iIdentitySettings, userServiceLogger)
        {
            logging = new Logging(clientControllerLogger);
            clientService = new ClientService(
                connectionSettings.OperationsDbSqlProvider,
                connectionSettings.OperationsDbConnString,
                identitySettings.JwtSecret,
                identitySettings.JwtExpirationSeconds, clientServiceLogger);
        }

        /// <summary>
        /// get client list
        /// </summary>
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/client/getclients")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(PaginatedList<Client>), description: "Successful operation")]
        public virtual IActionResult GetActiveClientList([FromQuery] string search, [FromQuery] string sort, [FromQuery] int? limit, [FromQuery] int? page)
        {
            try
            {
                PaginatedList<Client> clients = clientService.GetActiveClientList(search, limit, page, sort);
                if (clients != null)
                    return Ok(clients);
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
        [Route("/v1/client/getclientbyid")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(Client), description: "Successful operation")]
        public virtual IActionResult GetClientById([FromQuery] int clientId)
        {
            try
            {
                if (clientId > 0)
                {
                    Client client = clientService.GetClientById(clientId);
                    if (client != null)
                        return Ok(client);
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
        [Route("/v1/client/copyroles")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult CopyRoleToNewlyAddedClient([FromQuery] int clientId)
        {
            try
            {
                bool isRolesCopied = false;
                if (clientId > 0)
                {
                    if (CurrentUser != null)
                        isRolesCopied = clientService.CopyRoleToNewlyAddedClient(clientId, CurrentUser.UserName);
                    else
                        isRolesCopied = clientService.CopyRoleToNewlyAddedClient(clientId);
                }

                if (isRolesCopied)
                    return Ok(true);

                return BadRequest(new { message = "Supplied client information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }


        /// <summary>
        /// check if email is not in use for new client
        /// </summary>
        [HttpGet]
        [Route("/v1/client/validateemail")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult CheckEmail([FromQuery] long? clientId, [FromQuery] string email)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    bool valid = clientService.IsValidEmail(clientId, email);
                    if (!valid)
                        return BadRequest(new { message = "Email is already in use." });
                    return Ok(valid);
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
        /// add client 
        /// </summary>
        /// <remarks>add new client information </remarks>
        /// <param name="addClient">The client json format.</param>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("/v1/client/add")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(Client), description: "Successful operation")]
        public virtual IActionResult AddClient([FromBody] Client addClient)
        {
            try
            {
                if (clientService != null && addClient != null)
                {
                    ClientCommonResponse clientCommonResponse;
                    if (CurrentUser != null)
                        clientCommonResponse = clientService.AddClient(addClient, CurrentUser.UserName);
                    else
                        clientCommonResponse = clientService.AddClient(addClient, null);
                    if (clientCommonResponse == null)
                        return BadRequest(new { message = "Supplied client information is not valid." });
                    else if (!clientCommonResponse.Success)
                        return BadRequest(new { message = clientCommonResponse.Message });
                    else
                    {
                        if (clientCommonResponse.Client != null && clientCommonResponse.Client.Id > 0)
                            return Ok(clientCommonResponse.Client);
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
        /// <param name="updateClient">The client json format.</param>
        [HttpPut]
        [Route("/v1/client/update")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(UpdateClient), description: "Successful operation")]
        public virtual IActionResult UpdateClient([FromBody] UpdateClient updateClient)
        {
            try
            {
                if (clientService != null && updateClient != null && updateClient.Id > 0)
                {
                    ClientCommonResponse clientCommonResponse = clientService.UpdateClient(updateClient, CurrentUser.UserName);
                    if (clientCommonResponse == null)
                        return BadRequest(new { message = "Supplied client information is not valid." });
                    else if (!clientCommonResponse.Success)
                        return BadRequest(new { message = clientCommonResponse.Message });
                    else
                    {
                        if (clientCommonResponse.UpdateClient != null && clientCommonResponse.UpdateClient.Id > 0)
                            return Ok(clientCommonResponse.UpdateClient);
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
        [Route("/v1/client/delete")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult SoftDeleteClient([FromQuery] int clientId)
        {
            try
            {
                if (clientId > 0)
                {
                    ClientCommonResponse clientCommonResponse = clientService.SoftDelete(clientId, CurrentUser.UserName);

                    if (clientCommonResponse == null)
                        return BadRequest(new { message = "Supplied client information is not valid." });
                    else if (!clientCommonResponse.Success)
                        return BadRequest(new { message = clientCommonResponse.Message });
                    else
                    {
                        if (clientCommonResponse.UpdateClient != null && clientCommonResponse.UpdateClient.Id > 0)
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
        [Route("/v1/client/forcedelete")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult HardDeleteClient([FromQuery] int clientId)
        {
            try
            {
                bool isDeleted = false;
                if (clientId > 0)
                    isDeleted = clientService.HardDelete(clientId);

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

        /// <summary>
        /// copy roles to newly created client
        /// </summary>
        /// <remarks>copy roles to newly created client</remarks>
        
    }
}
