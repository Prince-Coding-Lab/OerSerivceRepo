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
using OERService.Helpers;
using System.Net.Mail;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace OERService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        IConfiguration _iconfiguration;
        private IHostingEnvironment _hostingEnvironment;

        public ProfileController(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _iconfiguration = configuration;

            _hostingEnvironment = hostingEnvironment;
        }
        /// <summary>
        /// This will return user profile details for given email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>OperationsResponse</returns>
        [HttpGet("{email}")]
        public async Task<IActionResult> GetById([FromRoute] string email)
        {
            try
            {
                try

                {
                    MailAddress mail = new MailAddress(email);
                }
                catch
                {
                    Log.Error(StatusMessages.DomainValidationError);
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = true,
                        Message = EnumExtensions.GetDescription(CommonErrors.InvalidEmail),
                    });

                }

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
                ProfileDataAccess _profileAccess = new ProfileDataAccess(_iconfiguration);

                DatabaseResponse response = await _profileAccess.GetUserProfile(email);

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
        /// This will create a user profile
        /// </summary>
        /// <param name="profile"></param>
        /// <returns>OperationsResponse</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserProfileRequest profile)
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

                ProfileDataAccess _profileAccess = new ProfileDataAccess(_iconfiguration);               

                //if (profile != null && profile.Photo!=null)
                //{

                //    DataImage image = DataImage.TryParse(profile.Photo);

                //    var imageDataByteArray = Convert.FromBase64String(profile.Photo);

                //    string apiRootPath = _hostingEnvironment.WebRootPath;                  

                //    string uploadPath = Path.Combine(apiRootPath, _iconfiguration.GetSection("Uploads.ResourcePath").ToString(),new Guid()+ image.MimeType);

                //    System.IO.File.WriteAllBytes(uploadPath, imageDataByteArray);
                   
                //}
                               

                DatabaseResponse response = await _profileAccess.CreateProfile(profile);

                if (response.ResponseCode == (int)DbReturnValue.CreateSuccess)
                {
                    DatabaseResponse userResponse = await _profileAccess.GetUserProfileById(( (UserMaster) response.Results).Id);
                    if(userResponse.ResponseCode==  (int)DbReturnValue.RecordExists)
                    {
                        return Ok(new OperationResponse
                        {
                            HasSucceeded = true,
                            IsDomainValidationErrors = false,
                            Message = EnumExtensions.GetDescription(DbReturnValue.CreateSuccess),
                            ReturnedObject = userResponse.Results
                        });

                    }
                    
                    else
                    {
                        return Ok(new OperationResponse
                        {
                            HasSucceeded = true,
                            IsDomainValidationErrors = false,
                            Message = EnumExtensions.GetDescription(DbReturnValue.CreationFailed)
                           
                        });
                    }
                   
                }

                else if(response.ResponseCode == (int)DbReturnValue.EmailExists)
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.EmailExists));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.EmailExists),
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
                        Message = EnumExtensions.GetDescription(DbReturnValue.CreationFailed),
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


        [HttpPut]
        public async Task<IActionResult> Put([FromForm] UpdateUserProfileRequest profile)
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

                ProfileDataAccess _profileAccess = new ProfileDataAccess(_iconfiguration);

                DatabaseResponse response = await _profileAccess.UpdateProfile(profile);

                if (response.ResponseCode == (int)DbReturnValue.CreateSuccess)
                {
                    DatabaseResponse userResponse = await _profileAccess.GetUserProfileById(((UserMaster)response.Results).Id);
                    if (userResponse.ResponseCode == (int)DbReturnValue.RecordExists)
                    {
                        return Ok(new OperationResponse
                        {
                            HasSucceeded = true,
                            IsDomainValidationErrors = false,
                            Message = EnumExtensions.GetDescription(DbReturnValue.CreateSuccess),
                            ReturnedObject = userResponse.Results
                        });

                    }

                    else
                    {
                        return Ok(new OperationResponse
                        {
                            HasSucceeded = true,
                            IsDomainValidationErrors = false,
                            Message = EnumExtensions.GetDescription(DbReturnValue.CreationFailed)

                        });
                    }


                }
                else
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.CreationFailed));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.CreationFailed),
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