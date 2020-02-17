using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;
using Microsoft.AspNetCore.Mvc;
using Core.Enums;
using Core.Helpers;
using Core.Models;
using Core.Extensions;
using OERService.Models;
using OERService.DataAccess;
using Microsoft.AspNetCore.Authorization;

namespace OERService.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 

    public class ResourceController : ControllerBase
    {
        IConfiguration _iconfiguration;
        public ResourceController(IConfiguration configuration)
        {
            _iconfiguration = configuration;
        }
        /// <summary>
        /// This will get all records from Resource Master.
        /// </summary>
        /// <returns></returns>
        // GET: api/Resources
       // [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {

                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.GetAllResource();

                if (response.ResponseCode == (int)DbReturnValue.RecordExists)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.RecordExists),
                        ReturnedObject = response.Results
                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.NoRecords));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.NoRecords)

                    });
                }

            }
            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                return Ok(new OperationResponse
                {
                    HasSucceeded = false,
                    Message = StatusMessages.ServerError,
                    StatusCode = ((int)ResponseStatus.ServerError).ToString(),
                    IsDomainValidationErrors = false
                });
            }
        }
        /// <summary>
        /// Get a resource referenced by the Id passed
        /// </summary>
        /// <param name="id"></param>
        /// <returns>OperationsResponse</returns>
        // GET: api/Resources/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = true,
                        StatusCode = ((int)ResponseStatus.BadRequest).ToString(),
                        Message = string.Join("; ", ModelState.Values
                                                 .SelectMany(x => x.Errors)
                                                 .Select(x => x.ErrorMessage))
                    };
                }
                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.GetResource(id);

                if (response.ResponseCode == (int)DbReturnValue.RecordExists)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.RecordExists),
                        ReturnedObject = response.Results
                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.NotExists));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.NotExists),
                        ReturnedObject = response.Results
                    });
                }

            }
            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                return Ok(new OperationResponse
                {
                    HasSucceeded = false,
                    Message = StatusMessages.ServerError,
                    StatusCode = ((int)ResponseStatus.ServerError).ToString(),
                    IsDomainValidationErrors = false
                });
            }
        }
        /// <summary>
        /// This will Create a ResourceMaster entry.
        /// </summary>
        /// <param name="resource">
        /// CreateResourceMaster
        ///Body: 
        ///{ 
        ///  "title": "Resource8_updated",
        ///  "categoryId": 1,
        ///  "subCategoryId": 1,
        ///  "thumbnail": "Not yet uploaded",
        ///  "resourceDescription": "This description is updated",
        ///  "keywords": "No keywords are given",
        ///  "resourceContent": "No content given",  
        ///  "materialTypeId": 2,
        ///  "copyRightId": 2,
        ///  "isDraft": false,
        ///  "createdBy": 1,
        ///  "references": [
        ///  { "urlReference": "ref1" }, 
        ///  { "urlReference": "ref2" },
        ///  { "urlReference": "ref3" }],
        ///  "resourceFiles": [
        ///  {"associatedFile": "file1"},
        ///  {"associatedFile": "file2"}]}}
        /// </param>
        /// <returns>OperationResponse</returns>
        // POST: api/Resources
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateResourceRequest resource)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = true,
                        StatusCode = ((int)ResponseStatus.BadRequest).ToString(),
                        Message = string.Join("; ", ModelState.Values
                                                   .SelectMany(x => x.Errors)
                                                   .Select(x => x.ErrorMessage))
                    });
                }

                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.CreateResource(resource);

                if (response.ResponseCode == (int)DbReturnValue.CreateSuccess)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.CreateSuccess),
                        ReturnedObject = response.Results
                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.CreationFailed));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.RecordExists),
                        ReturnedObject = response.Results
                    });
                }

            }
            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                return Ok(new OperationResponse
                {
                    HasSucceeded = false,
                    Message = StatusMessages.ServerError,
                    StatusCode = ((int)ResponseStatus.ServerError).ToString(),
                    IsDomainValidationErrors = false
                });
            }
        }
        /// <summary>
        /// This will update ResourceMaster entry by id and details passed.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource">
        ///Body: 
        ///{   
        ///{  "id":8,
        ///  "title": "Resource8_updated",
        ///  "categoryId": 1,
        ///  "subCategoryId": 1,
        ///  "thumbnail": "Not yet uploaded",
        ///  "resourceDescription": "This description is updated",
        ///  "keywords": "No keywords are given",
        ///  "resourceContent": "No content given",  
        ///  "materialTypeId": 2,
        ///  "copyRightId": 2,
        ///  "isDraft": false,
        ///  "createdBy": 1,
        ///  "references": [
        ///  { "urlReference": "ref1" }, 
        ///  { "urlReference": "ref2" },
        ///  { "urlReference": "ref3" }],
        ///  "resourceFiles": [
        ///  {"associatedFile": "file1"},
        ///  {"associatedFile": "file2"}]}}
        /// </param>
        /// <returns>OperationsResponse</returns>
        // PUT: api/Resources/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateResourceRequest resource)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = true,
                        StatusCode = ((int)ResponseStatus.BadRequest).ToString(),
                        Message = string.Join("; ", ModelState.Values
                                                   .SelectMany(x => x.Errors)
                                                   .Select(x => x.ErrorMessage))
                    });
                }

                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.UpdateResource(resource, id);

                if (response.ResponseCode == (int)DbReturnValue.UpdateSuccess)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.UpdateSuccess),
                        ReturnedObject = response.Results
                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.UpdationFailed));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.NotExists),
                        ReturnedObject = response.Results
                    });
                }

            }
            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                return Ok(new OperationResponse
                {
                    HasSucceeded = false,
                    Message = StatusMessages.ServerError,
                    StatusCode = ((int)ResponseStatus.ServerError).ToString(),
                    IsDomainValidationErrors = false
                });
            }
        }
        /// <summary>
        /// This will delete a resource record by the id passed
        /// </summary>
        /// <param name="id"></param>
        /// <returns>OperationResponse</returns>
        // DELETE: api/Resources/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = true,
                        StatusCode = ((int)ResponseStatus.BadRequest).ToString(),
                        Message = string.Join("; ", ModelState.Values
                                                   .SelectMany(x => x.Errors)
                                                   .Select(x => x.ErrorMessage))
                    });
                }
                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.DeleteResource(id);

                if (response.ResponseCode == (int)DbReturnValue.DeleteSuccess)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.DeleteSuccess)
                      
                    });
                }
                else if (response.ResponseCode == (int)DbReturnValue.ActiveTryDelete)
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.ActiveTryDelete));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.ActiveTryDelete)
                        
                    });
                }

                else if (response.ResponseCode == (int)DbReturnValue.DeleteFailed)
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.DeleteFailed));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.DeleteFailed)
                        
                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.NotExists));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.NotExists)
                       
                    });
                }

            }
            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                return Ok(new OperationResponse
                {
                    HasSucceeded = false,
                    Message = StatusMessages.ServerError,
                    StatusCode = ((int)ResponseStatus.ServerError).ToString(),
                    IsDomainValidationErrors = false
                });
            }
        }

        /// <summary>
        /// This will approve a resource
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]     
        [Route("ApproveResource/{id}/{userId}")]
        public async Task<IActionResult> ApproveResource(decimal id, int userId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = true,
                        StatusCode = ((int)ResponseStatus.BadRequest).ToString(),
                        Message = string.Join("; ", ModelState.Values
                                                 .SelectMany(x => x.Errors)
                                                 .Select(x => x.ErrorMessage))
                    };
                }
                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.ApproveResource(id, userId);

                if (response.ResponseCode == (int)DbReturnValue.Approved)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.Approved),
                        ReturnedObject = response.Results
                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.AlreadyApproved));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.AlreadyApproved),
                        ReturnedObject = response.Results
                    });
                }

            }
            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                return Ok(new OperationResponse
                {
                    HasSucceeded = false,
                    Message = StatusMessages.ServerError,
                    StatusCode = ((int)ResponseStatus.ServerError).ToString(),
                    IsDomainValidationErrors = false
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("ReportResource/{id}")]     
        public async Task<IActionResult> ReportResource(decimal id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = true,
                        StatusCode = ((int)ResponseStatus.BadRequest).ToString(),
                        Message = string.Join("; ", ModelState.Values
                                                 .SelectMany(x => x.Errors)
                                                 .Select(x => x.ErrorMessage))
                    };
                }
                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.ResportResource(id);

                if (response.ResponseCode == (int)DbReturnValue.UpdateSuccess)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(CommonErrors.AbuseReported),
                        ReturnedObject = response.Results
                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(CommonErrors.AbuseReportFailed));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(CommonErrors.AbuseReportFailed),
                        ReturnedObject = response.Results
                    });
                }

            }
            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                return Ok(new OperationResponse
                {
                    HasSucceeded = false,
                    Message = StatusMessages.ServerError,
                    StatusCode = ((int)ResponseStatus.ServerError).ToString(),
                    IsDomainValidationErrors = false
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceComment"></param>
        /// <returns></returns>
        [HttpPost("CommentOnResource")]
        public async Task<IActionResult> CommentOnResource([FromBody] ResourceCommentRequest resourceComment)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = true,
                        StatusCode = ((int)ResponseStatus.BadRequest).ToString(),
                        Message = string.Join("; ", ModelState.Values
                                                   .SelectMany(x => x.Errors)
                                                   .Select(x => x.ErrorMessage))
                    });
                }

                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.CommentOnResource(resourceComment);

                if (response.ResponseCode == (int)DbReturnValue.CreateSuccess)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.CreateSuccess),
                        ReturnedObject = response.Results
                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.CreationFailed));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.RecordExists),
                        ReturnedObject = response.Results
                    });
                }

            }
            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                return Ok(new OperationResponse
                {
                    HasSucceeded = false,
                    Message = StatusMessages.ServerError,
                    StatusCode = ((int)ResponseStatus.ServerError).ToString(),
                    IsDomainValidationErrors = false
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceUpdate"></param>
        /// <returns></returns>
        [HttpPost("UpdateResourceComment")]
        public async Task<IActionResult> UpdateResourceComment([FromBody] ResourceCommentUpdateRequest resourceUpdate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = true,
                        StatusCode = ((int)ResponseStatus.BadRequest).ToString(),
                        Message = string.Join("; ", ModelState.Values
                                                   .SelectMany(x => x.Errors)
                                                   .Select(x => x.ErrorMessage))
                    });
                }

                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.UpdateResourceComment(resourceUpdate);

                if (response.ResponseCode == (int)DbReturnValue.UpdateSuccess)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.UpdateSuccess),
                        ReturnedObject = response.Results
                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.UpdationFailed));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.NotExists),
                        ReturnedObject = response.Results
                    });
                }

            }
            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                return Ok(new OperationResponse
                {
                    HasSucceeded = false,
                    Message = StatusMessages.ServerError,
                    StatusCode = ((int)ResponseStatus.ServerError).ToString(),
                    IsDomainValidationErrors = false
                });
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("ReportResourceComment/{id}")]
        public async Task<IActionResult> ReportResourceComment(decimal id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = true,
                        StatusCode = ((int)ResponseStatus.BadRequest).ToString(),
                        Message = string.Join("; ", ModelState.Values
                                                 .SelectMany(x => x.Errors)
                                                 .Select(x => x.ErrorMessage))
                    };
                }
                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.ResportResourceComment(id);

                if (response.ResponseCode == (int)DbReturnValue.UpdateSuccess)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(CommonErrors.AbuseReported),
                        ReturnedObject = response.Results
                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(CommonErrors.AbuseReportFailed));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(CommonErrors.AbuseReportFailed),
                        ReturnedObject = response.Results
                    });
                }

            }
            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                return Ok(new OperationResponse
                {
                    HasSucceeded = false,
                    Message = StatusMessages.ServerError,
                    StatusCode = ((int)ResponseStatus.ServerError).ToString(),
                    IsDomainValidationErrors = false
                });
            }
        }


        // DELETE: api/Resources/5
        [HttpDelete("DeleteResourceComment/{id}/{requestedBy}")]
        public async Task<IActionResult> DeleteResourceComment(decimal id, int requestedBy)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = true,
                        StatusCode = ((int)ResponseStatus.BadRequest).ToString(),
                        Message = string.Join("; ", ModelState.Values
                                                   .SelectMany(x => x.Errors)
                                                   .Select(x => x.ErrorMessage))
                    });
                }
                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.DeleteResourceComment(id,requestedBy);

                if (response.ResponseCode == (int)DbReturnValue.DeleteSuccess)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.DeleteSuccess)

                    });
                }               

                else if (response.ResponseCode == (int)DbReturnValue.DeleteFailed)
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.DeleteFailed));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.DeleteFailed)

                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.NotExists));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.NotExists)

                    });
                }

            }
            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                return Ok(new OperationResponse
                {
                    HasSucceeded = false,
                    Message = StatusMessages.ServerError,
                    StatusCode = ((int)ResponseStatus.ServerError).ToString(),
                    IsDomainValidationErrors = false
                });
            }
        }

       /// <summary>
       /// Hide a resource comment. Can only be done by the resource owner
       /// </summary>
       /// <param name="id"></param>
       /// <param name="resourceId"></param>
       /// <param name="requestedBy"></param>
       /// <returns></returns>
        [HttpGet("HideResourceComment/{id}/{resourceId}/{requestedBy}")]
        public async Task<IActionResult> HideResourceComment(decimal id,decimal resourceId, int requestedBy)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = true,
                        StatusCode = ((int)ResponseStatus.BadRequest).ToString(),
                        Message = string.Join("; ", ModelState.Values
                                                 .SelectMany(x => x.Errors)
                                                 .Select(x => x.ErrorMessage))
                    };
                }
                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.HideResourceCommentByAuthor(id,resourceId,requestedBy);

                if (response.ResponseCode == (int)DbReturnValue.UpdateSuccess)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(CommonErrors.CommentHide),
                        ReturnedObject = response.Results
                    });
                }
                else if (response.ResponseCode == (int)DbReturnValue.UpdationFailed)
                {
                    Log.Error(EnumExtensions.GetDescription(CommonErrors.CommentHideFailed));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(CommonErrors.CommentHideFailed),
                        ReturnedObject = response.Results
                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.ReasouceOnlyHiddenByAuthor));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.ReasouceOnlyHiddenByAuthor),
                        ReturnedObject = response.Results
                    });
                }

            }
            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                return Ok(new OperationResponse
                {
                    HasSucceeded = false,
                    Message = StatusMessages.ServerError,
                    StatusCode = ((int)ResponseStatus.ServerError).ToString(),
                    IsDomainValidationErrors = false
                });
            }
        }
        
        /// <summary>
        /// This will report a resource with comment and reason/s to report
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("ReportResourceWithComment")]
        public async Task<IActionResult> ReportResourceWithComment(ResourceReportAbuseWithComment request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = true,
                        StatusCode = ((int)ResponseStatus.BadRequest).ToString(),
                        Message = string.Join("; ", ModelState.Values
                                                 .SelectMany(x => x.Errors)
                                                 .Select(x => x.ErrorMessage))
                    };
                }
                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.ReportResourceWithComment(request);

                if (response.ResponseCode == (int)DbReturnValue.UpdateSuccess)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(CommonErrors.AbuseReported),
                        ReturnedObject = response.Results
                    });
                }
                else if(response.ResponseCode == (int)DbReturnValue.RecordExists)
                {
                    Log.Error(EnumExtensions.GetDescription(CommonErrors.AlreadyReportedResource));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(CommonErrors.AlreadyReportedResource),
                        ReturnedObject = response.Results
                    });
                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(CommonErrors.AbuseReportFailed));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(CommonErrors.AbuseReportFailed),
                        ReturnedObject = response.Results
                    });
                }

            }
            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                return Ok(new OperationResponse
                {
                    HasSucceeded = false,
                    Message = StatusMessages.ServerError,
                    StatusCode = ((int)ResponseStatus.ServerError).ToString(),
                    IsDomainValidationErrors = false
                });
            }
        }

        /// <summary>
        /// Rate a resource with comment
        /// </summary>
        /// <param name="request">
        /// 
        /// </param>
        /// <returns></returns>
        /// 
        [HttpPost("RateResource")]
        public async Task<IActionResult> RateResource(ResourceRatingRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = true,
                        StatusCode = ((int)ResponseStatus.BadRequest).ToString(),
                        Message = string.Join("; ", ModelState.Values
                                                 .SelectMany(x => x.Errors)
                                                 .Select(x => x.ErrorMessage))
                    };
                }
                ResourceDataAccess _resourceAccess = new ResourceDataAccess(_iconfiguration);

                DatabaseResponse response = await _resourceAccess.RateResource(request);

                if (response.ResponseCode == (int)DbReturnValue.CreateSuccess)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(CommonErrors.RatingSucess),
                        ReturnedObject = response.Results
                    });
                }
                else if(response.ResponseCode == (int)DbReturnValue.CreationFailed)
                {
                    Log.Error(EnumExtensions.GetDescription(CommonErrors.RatingFailed));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(CommonErrors.RatingFailed),
                        ReturnedObject = response.Results
                    });
                }

                else
                {
                    Log.Error(EnumExtensions.GetDescription(CommonErrors.AlreadyRatedResource));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(CommonErrors.AlreadyRatedResource),
                        ReturnedObject = response.Results
                    });
                }


            }
            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                return Ok(new OperationResponse
                {
                    HasSucceeded = false,
                    Message = StatusMessages.ServerError,
                    StatusCode = ((int)ResponseStatus.ServerError).ToString(),
                    IsDomainValidationErrors = false
                });
            }
        }     

    }
}