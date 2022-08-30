using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ds.pms.apicommon.Models;
using ds.pms.apicommon.Settings;
using ds.pms.bl.documents.IServices;
using ds.pms.bl.documents.Models;
using ds.pms.bl.documents.Services;
using ds.pms.bl.logger;
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
    public class DocumentController : BaseController
    {
        private IDocumentService documentService;
        private Logging logging;

        /// <summary>
        /// Document controller constructor
        /// </summary>
        /// <param name="documentControllerLogger"></param>
        /// <param name="documentServiceLogger"></param>
        /// <param name="userServiceLogger"></param>
        /// <param name="iConnectionSettings"></param>
        /// <param name="iIdentitySettings"></param>
        public DocumentController(ILogger<DocumentController> documentControllerLogger, ILogger<DocumentService> documentServiceLogger
            , ILogger<UserService> userServiceLogger, IOptions<ConnectionSettings> iConnectionSettings
            , IOptions<IdentitySettings> iIdentitySettings)
            : base(iConnectionSettings, iIdentitySettings, userServiceLogger)
        {
            logging = new Logging(documentControllerLogger);
            documentService = new DocumentService(
                connectionSettings.OperationsDbSqlProvider,
                connectionSettings.OperationsDbConnString,
                identitySettings.JwtSecret,
                identitySettings.JwtExpirationSeconds, documentServiceLogger);
        }

        /// <summary>
        /// get document list
        /// </summary>
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/document/getdocuments")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(PaginatedList<Document>), description: "Successful operation")]
        public virtual IActionResult GetActiveDocumentList([FromQuery] int proId, [FromQuery] string search, [FromQuery] string sort, [FromQuery] int? limit, [FromQuery] int? page)
        {
            try
            {
                if (documentService != null)
                {
                    PaginatedList<Document> documents = documentService.GetActiveDocumentList(proId,search, limit, page, sort);
                    if (documents != null)
                        return Ok(documents);
                    else
                        return Ok(new { message = "No Data found." });
                }
                else
                    return BadRequest(new { message = "Supplied document information is not valid" });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// get document by id
        /// </summary>
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/document/getdocumentbyid")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(Document), description: "Successful operation")]
        public virtual IActionResult GetDocumentById([FromQuery] int documentId)
        {
            try
            {
                if (documentService != null && documentId > 0)
                {
                    Document document = documentService.GetDocumentById(documentId);
                    if (document != null)
                        return Ok(document);
                    else
                        return Ok(new { message = "No Data found." });
                }
                else
                    return BadRequest(new { message = "Supplied document information is not valid" });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// add document 
        /// </summary>
        /// <remarks>add new document information </remarks>
        /// <param name="addDocument">The document json format.</param>
        [HttpPost]
        [Route("/v1/document/add")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(Document), description: "Successful operation")]
        public virtual IActionResult AddDocument([FromBody] Document addDocument)
        {
            try
            {
                if (documentService != null && addDocument != null)
                {
                    DocumentCommonResponse documentCommonResponse = documentService.AddDocument(addDocument, CurrentUser.UserName);
                    if (documentCommonResponse == null)
                        return BadRequest(new { message = "Supplied document information is not valid." });
                    else if (!documentCommonResponse.Success)
                        return BadRequest(new { message = documentCommonResponse.Message });
                    else
                    {
                        if (documentCommonResponse.Document != null && documentCommonResponse.Document.Id > 0)
                            return Ok(documentCommonResponse.Document);
                        else
                            return BadRequest(new { message = "Supplied document information is not valid." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied document information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// update document 
        /// </summary>
        /// <remarks>update document information </remarks>
        /// <param name="updateDocument">The document json format.</param>
        [HttpPut]
        [Route("/v1/document/update")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(Document), description: "Successful operation")]
        public virtual IActionResult UpdateDocument([FromBody] Document updateDocument)
        {
            try
            {
                if (documentService != null && updateDocument != null && updateDocument.Id > 0)
                {
                    DocumentCommonResponse documentCommonResponse = documentService.UpdateDocument(updateDocument, CurrentUser.UserName);
                    if (documentCommonResponse == null)
                        return BadRequest(new { message = "Supplied document information is not valid." });
                    else if (!documentCommonResponse.Success)
                        return BadRequest(new { message = documentCommonResponse.Message });
                    else
                    {
                        if (documentCommonResponse.Document != null && documentCommonResponse.Document.Id > 0)
                            return Ok(documentCommonResponse.Document);
                        else
                            return BadRequest(new { message = "Supplied document information is not valid." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied document information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// soft delete document 
        /// </summary>
        /// <remarks>Soft delete document </remarks>
        [HttpDelete]
        [Route("/v1/document/delete")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult SoftDeleteDocument([FromQuery] int documentId)
        {
            try
            {
                if (documentService != null && documentId > 0)
                {
                    DocumentCommonResponse documentCommonResponse = documentService.SoftDelete(documentId, CurrentUser.UserName);

                    if (documentCommonResponse == null)
                        return BadRequest(new { message = "Supplied document information is not valid." });
                    else if (!documentCommonResponse.Success)
                        return BadRequest(new { message = documentCommonResponse.Message });
                    else
                    {
                        if (documentCommonResponse.Document != null && documentCommonResponse.Document.Id > 0)
                            return Ok(true);
                        else
                            return BadRequest(new { message = "Unable to delete document." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied document information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// forcefully delete document 
        /// </summary>
        /// <remarks>forcefully delete document </remarks>
        [HttpDelete]
        [Route("/v1/document/forcedelete")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult HardDeleteDocument([FromQuery] int documentId)
        {
            try
            {
                bool isDeleted = false;
                if (documentService != null && documentId > 0)
                    isDeleted = documentService.HardDelete(documentId);

                if (isDeleted)
                    return Ok(true);

                return BadRequest(new { message = "Supplied document information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }
    }
}
