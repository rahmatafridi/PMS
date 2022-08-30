using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ds.pms.apicommon.Models;
using ds.pms.apicommon.Settings;
using ds.pms.bl.logger;
using ds.pms.bl.notescategory.IServices;
using ds.pms.bl.notescategory.Models;
using ds.pms.bl.notescategory.Services;
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
    public class NotesCategoryController : BaseController
    {
        private INotesCategoryService notesCategoryService;
        private Logging logging;

        /// <summary>
        /// NotesCategory controller constructor
        /// </summary>
        /// <param name="notesCategoryControllerLogger"></param>
        /// <param name="notesCategoryServiceLogger"></param>
        /// <param name="userServiceLogger"></param>
        /// <param name="iConnectionSettings"></param>
        /// <param name="iIdentitySettings"></param>
        public NotesCategoryController(ILogger<NotesCategoryController> notesCategoryControllerLogger, ILogger<NotesCategoryService> notesCategoryServiceLogger
            , ILogger<UserService> userServiceLogger, IOptions<ConnectionSettings> iConnectionSettings
            , IOptions<IdentitySettings> iIdentitySettings)
            : base(iConnectionSettings, iIdentitySettings, userServiceLogger)
        {
            logging = new Logging(notesCategoryControllerLogger);
            notesCategoryService = new NotesCategoryService(
               connectionSettings.OperationsDbSqlProvider,
               connectionSettings.OperationsDbConnString,
               identitySettings.JwtSecret,
               identitySettings.JwtExpirationSeconds,notesCategoryServiceLogger);
        }

        /// <summary>
        /// get notes category list
        /// </summary>
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/notescategory/getnotescategory")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(PaginatedList<NotesCategory>), description: "Successful operation")]
        public virtual IActionResult GetActiveNotesCategoryList([FromQuery] string search, [FromQuery] string sort, [FromQuery] int? limit, [FromQuery] int? page)
        {
            try
            {
                PaginatedList<NotesCategory> notesCategory = notesCategoryService.GetActiveNotesCategoryList(search, limit, page, sort);
                if (notesCategory != null)
                    return Ok(notesCategory);
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
        /// get notes category by id
        /// </summary>
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/notescategory/getnotescategorybyid")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(NotesCategory), description: "Successful operation")]
        public virtual IActionResult GetNotesCategoryById([FromQuery] int notesCategoryId)
        {
            try
            {
                if (notesCategoryId > 0)
                {
                    NotesCategory notesCategory = notesCategoryService.GetNotesCategoryById(notesCategoryId);
                    if (notesCategory != null)
                        return Ok(notesCategory);
                    else
                        return Ok(new { message = "No Data found." });
                }
                else
                    return BadRequest(new { message = "Supplied notes category information is not valid" });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// add notes category 
        /// </summary>
        /// <remarks>add new notes category information </remarks>
        /// <param name="addNotesCategory">The notes category json format.</param>
        [HttpPost]
        [Route("/v1/notescategory/add")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(NotesCategory), description: "Successful operation")]
        public virtual IActionResult AddNotesCategory([FromBody] NotesCategory addNotesCategory)
        {
            try
            {
                if (notesCategoryService != null && addNotesCategory != null)
                {
                    NotesCategoryCommonResponse notesCategoryCommonResponse = notesCategoryService.AddNotesCategory(addNotesCategory, CurrentUser.UserName);
                    if (notesCategoryCommonResponse == null)
                        return BadRequest(new { message = "Supplied notes category information is not valid." });
                    else if (!notesCategoryCommonResponse.Success)
                        return BadRequest(new { message = notesCategoryCommonResponse.Message });
                    else
                    {
                        if (notesCategoryCommonResponse.NotesCategory != null && notesCategoryCommonResponse.NotesCategory.Id > 0)
                            return Ok(notesCategoryCommonResponse.NotesCategory);
                        else
                            return BadRequest(new { message = "Supplied notes category information is not valid." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied notes category information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// update notes category 
        /// </summary>
        /// <remarks>update notes category information </remarks>
        /// <param name="updateNotesCategory">The notes category json format.</param>
        [HttpPut]
        [Route("/v1/notescategory/update")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(NotesCategory), description: "Successful operation")]
        public virtual IActionResult UpdateNotesCategory([FromBody] NotesCategory updateNotesCategory)
        {
            try
            {
                if (notesCategoryService != null && updateNotesCategory != null && updateNotesCategory.Id > 0)
                {
                    NotesCategoryCommonResponse notesCategoryCommonResponse = notesCategoryService.UpdateNotesCategory(updateNotesCategory, CurrentUser.UserName);
                    if (notesCategoryCommonResponse == null)
                        return BadRequest(new { message = "Supplied notes category information is not valid." });
                    else if (!notesCategoryCommonResponse.Success)
                        return BadRequest(new { message = notesCategoryCommonResponse.Message });
                    else
                    {
                        if (notesCategoryCommonResponse.NotesCategory != null && notesCategoryCommonResponse.NotesCategory.Id > 0)
                            return Ok(notesCategoryCommonResponse.NotesCategory);
                        else
                            return BadRequest(new { message = "Supplied notes category information is not valid." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied notes category information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// soft delete notes category 
        /// </summary>
        /// <remarks>Soft delete notes category </remarks>
        [HttpDelete]
        [Route("/v1/notescategory/delete")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult SoftDeleteNotesCategory([FromQuery] int notesCategoryId)
        {
            try
            {
                if (notesCategoryId > 0)
                {
                    NotesCategoryCommonResponse notesCategoryCommonResponse = notesCategoryService.SoftDelete(notesCategoryId, CurrentUser.UserName);

                    if (notesCategoryCommonResponse == null)
                        return BadRequest(new { message = "Supplied notes category information is not valid." });
                    else if (!notesCategoryCommonResponse.Success)
                        return BadRequest(new { message = notesCategoryCommonResponse.Message });
                    else
                    {
                        if (notesCategoryCommonResponse.NotesCategory != null && notesCategoryCommonResponse.NotesCategory.Id > 0)
                            return Ok(true);
                        else
                            return BadRequest(new { message = "Unable to delete notes category." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied notes category information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// forcefully delete notes category 
        /// </summary>
        /// <remarks>forcefully delete notes category </remarks>
        [HttpDelete]
        [Route("/v1/notescategory/forcedelete")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult HardDeleteNotesCategory([FromQuery] int notesCategoryId)
        {
            try
            {
                bool isDeleted = false;
                if (notesCategoryId > 0)
                    isDeleted = notesCategoryService.HardDelete(notesCategoryId);

                if (isDeleted)
                    return Ok(true);

                return BadRequest(new { message = "Supplied notes category information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }
    }
}
