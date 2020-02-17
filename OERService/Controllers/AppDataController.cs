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
    public class AppDataController : ControllerBase
    {
        IConfiguration _iconfiguration;
        public AppDataController(IConfiguration configuration)
        {
            _iconfiguration = configuration;
        }
        /// <summary>
        /// This will return dropdown data collection for profile page
        /// </summary>
        /// <returns></returns>
        // GET: api/AppData
        [HttpGet]
        public async Task<IActionResult> GetProfileAppData()
        {
            try
            {

                AppDataDataAccess _appDataAccess = new AppDataDataAccess(_iconfiguration);              

                DatabaseResponse response = await _appDataAccess.GetProfileAppData();

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
       
      
    }
}
