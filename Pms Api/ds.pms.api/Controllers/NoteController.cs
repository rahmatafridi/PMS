using ds.pms.apicommon.Models;
using ds.pms.apicommon.Settings;
using ds.pms.bl.logger;
using ds.pms.bl.notes.IServices;
using ds.pms.bl.notes.Models;
using ds.pms.bl.notes.Services;
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
    public class NoteController : BaseController
    {
        private INoteService noteService;
        private Logging logging;

        /// <summary>
        /// Note controller constructor
        /// </summary>
        /// <param name="noteControllerLogger"></param>
        /// <param name="noteServiceLogger"></param>
        /// <param name="userServiceLogger"></param>
        /// <param name="iConnectionSettings"></param>
        /// <param name="iIdentitySettings"></param>
        public NoteController(ILogger<NoteController> noteControllerLogger, ILogger<NoteService> noteServiceLogger
            , ILogger<UserService> userServiceLogger, IOptions<ConnectionSettings> iConnectionSettings
            , IOptions<IdentitySettings> iIdentitySettings)
            : base(iConnectionSettings, iIdentitySettings, userServiceLogger)
        {
            logging = new Logging(noteControllerLogger);
            noteService = new NoteService(
                connectionSettings.OperationsDbSqlProvider,
                connectionSettings.OperationsDbConnString,
                identitySettings.JwtSecret,
                identitySettings.JwtExpirationSeconds, noteServiceLogger);
        }

        /// <summary>
        /// get note list
        /// </summary>
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/note/getnote")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(PaginatedList<Note>), description: "Successful operation")]
        public virtual IActionResult GetActiveNoteList([FromQuery] int proId,[FromQuery] string search, [FromQuery] string sort, [FromQuery] int? limit, [FromQuery] int? page)
        {
            try
            {
                PaginatedList<Note> notes = noteService.GetActiveNoteList(proId, search, limit, page, sort);
                if (notes != null)
                    return Ok(notes);
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
        /// get note by id
        /// </summary>
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/note/getnotebyid")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(Note), description: "Successful operation")]
        public virtual IActionResult GetNoteById([FromQuery] int noteId, [FromQuery] int typeId)
        {
            try
            {
                if (noteId > 0)
                {
                    Note note = noteService.GetNoteById(noteId, typeId);
                    if (note != null)
                        return Ok(note);
                    else
                        return Ok(new { message = "No Data found." });
                }
                else
                    return BadRequest(new { message = "Supplied note information is not valid" });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// get note by id
        /// </summary>
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [Route("/v1/note/getnotebyperantid")]
        [HttpGet]
        [SwaggerResponse(statusCode: 200, type: typeof(Note), description: "Successful operation")]
        public virtual IActionResult GetNoteByParentId([FromQuery] int parentId, [FromQuery] int typeId)
        {
            try
            {
                if (parentId > 0)
                {
                    Note note = noteService.GetNoteByParent(parentId, typeId);
                    if (note != null)
                        return Ok(note);
                    else
                        return Ok(new { message = "No Data found." });
                }
                else
                    return BadRequest(new { message = "Supplied note information is not valid" });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// add note 
        /// </summary>
        /// <remarks>add new note information </remarks>
        /// <param name="addNote">The note json format.</param>
        [HttpPost]
        [Route("/v1/note/add")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(Note), description: "Successful operation")]
        public virtual IActionResult AddNote([FromBody] Note addNote)
        {
            try
            {
                if (noteService != null && addNote != null)
                {
                    NoteCommonResponse noteCommonResponse = noteService.AddNote(addNote, CurrentUser.UserName);
                    if (noteCommonResponse == null)
                        return BadRequest(new { message = "Supplied note information is not valid." });
                    else if (!noteCommonResponse.Success)
                        return BadRequest(new { message = noteCommonResponse.Message });
                    else
                    {
                        if (noteCommonResponse.Note != null && noteCommonResponse.Note.Id > 0)
                            return Ok(noteCommonResponse.Note);
                        else
                            return BadRequest(new { message = "Supplied note information is not valid." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied note information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// update note 
        /// </summary>
        /// <remarks>update note information </remarks>
        /// <param name="updateNote">The note json format.</param>
        [HttpPut]
        [Route("/v1/note/update")]
        //[ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(Note), description: "Successful operation")]
        public virtual IActionResult UpdateNote([FromBody] Note updateNote)
        {
            try
            {
                if (noteService != null && updateNote != null && updateNote.Id > 0)
                {
                    NoteCommonResponse noteCommonResponse = noteService.UpdateNote(updateNote, CurrentUser.UserName);
                    if (noteCommonResponse == null)
                        return BadRequest(new { message = "Supplied note information is not valid." });
                    else if (!noteCommonResponse.Success)
                        return BadRequest(new { message = noteCommonResponse.Message });
                    else
                    {
                        if (noteCommonResponse.Note != null && noteCommonResponse.Note.Id > 0)
                            return Ok(noteCommonResponse.Note);
                        else
                            return BadRequest(new { message = "Supplied note information is not valid." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied note information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// soft delete note 
        /// </summary>
        /// <remarks>Soft delete note </remarks>
        [HttpDelete]
        [Route("/v1/note/delete")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult SoftDeleteNote([FromQuery] int noteId)
        {
            try
            {
                if (noteId > 0)
                {
                    NoteCommonResponse noteCommonResponse = noteService.SoftDelete(noteId, CurrentUser.UserName);

                    if (noteCommonResponse == null)
                        return BadRequest(new { message = "Supplied note information is not valid." });
                    else if (!noteCommonResponse.Success)
                        return BadRequest(new { message = noteCommonResponse.Message });
                    else
                    {
                        if (noteCommonResponse.Note != null && noteCommonResponse.Note.Id > 0)
                            return Ok(true);
                        else
                            return BadRequest(new { message = "Unable to delete note." });
                    }
                }
                else
                    return BadRequest(new { message = "Supplied note information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }

        /// <summary>
        /// forcefully delete note 
        /// </summary>
        /// <remarks>forcefully delete note </remarks>
        [HttpDelete]
        [Route("/v1/Note/forcedelete")]
        [SwaggerResponse(statusCode: 200, type: typeof(bool), description: "Successful operation")]
        public virtual IActionResult HardDeleteNote([FromQuery] int noteId)
        {
            try
            {
                bool isDeleted = false;
                if (noteId > 0)
                    isDeleted = noteService.HardDelete(noteId);

                if (isDeleted)
                    return Ok(true);

                return BadRequest(new { message = "Supplied note information is not valid." });
            }
            catch (Exception ex)
            {
                logging.LogError(ex, logging.GetExceptionMessage(this.ControllerContext));
                return BadRequest(new { message = logging.GetExceptionMessage(this.ControllerContext) + " STACK TRACE : " + ex.ToString() });
            }
        }



     
    }
}
