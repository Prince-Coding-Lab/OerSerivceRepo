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


namespace OERService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class URLWhiteListingController : ControllerBase
    {
        // GET: api/URLWhiteListing
        IConfiguration _iconfiguration;
        public URLWhiteListingController(IConfiguration configuration)
        {
            _iconfiguration = configuration;
        }
        /// <summary>
        /// This will get all pending URLWhiteListing requests.
        /// </summary>
        /// <returns></returns>
        // GET: api/GetWhiteListingRequests
        [HttpGet("GetWhiteListingRequests")]
        public async Task<IActionResult> GetWhiteListingRequests()
        {
            try
            {
                URLWhiteListingDataAccess _whitelistingAccess = new URLWhiteListingDataAccess(_iconfiguration);

                DatabaseResponse response = await _whitelistingAccess.GetWhiteListedUrls();

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
                    IsDomainValidationErrors = false,

                });
            }
        }

        /// <summary>
        /// This will Create a request for URLWhiteListing.
        /// </summary>
        /// <param name="request">      
        ///Body: 
        ///{
        ///	"RequestedBy" : "1",
        ///	"URL" : "www.example.com"        
        ///}
        /// </param>
        /// <returns>OperationResponse</returns>
        // POST: api/URLWhiteListing
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] WhiteListingRequest request)
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

                URLWhiteListingDataAccess _whitelistingAccess = new URLWhiteListingDataAccess(_iconfiguration);

                DatabaseResponse response = await _whitelistingAccess.RequestWhitelisting(request);              

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
        /// This will verify a URLWhiteListing request.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request">
        ///Body: 
        ///{
        ///	"Id" : "1",       
        ///	"VerifiedBy" : "1" ,
        ///	"IsApproved":true/false
        ///	"RejectedReason":"reason" -- optional
        ///}       
        /// </param>
        /// <returns>OperationsResponse</returns>
       
        // PUT: api/URLWhiteListing/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] WhiteListUrl request)
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

                URLWhiteListingDataAccess _whitelistingAccess = new URLWhiteListingDataAccess(_iconfiguration);

                DatabaseResponse response = await _whitelistingAccess.WhiteListUrl(request, id);


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
        /// This will check if a URL is already whitelisted, then returns details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("ISWhilteListed")]
        [HttpPost]
        public async Task<IActionResult> ISWhilteListed(IsWhiteListed request)
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
                URLWhiteListingDataAccess _whitelistingAccess = new URLWhiteListingDataAccess(_iconfiguration);

                DatabaseResponse response = await _whitelistingAccess.IsWhiteListed(request.URL);

                if (response.ResponseCode == (int)DbReturnValue.RecordExists)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(CommonErrors.WhiteListed),
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
                        Message = EnumExtensions.GetDescription(CommonErrors.NotWhiteListed),
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
