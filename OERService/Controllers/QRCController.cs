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
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace OERService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QRCController : ControllerBase
    {
        IConfiguration _iconfiguration;
        public QRCController(IConfiguration configuration)
        {
            _iconfiguration = configuration;
        }
        /// <summary>
        /// This will get all records from QRC.
        /// </summary>
        /// <returns>OperationsResponse</returns>
        // GET: api/qrc
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                QrcDataAccess _qrcAccess = new QrcDataAccess(_iconfiguration);

                DatabaseResponse response = await _qrcAccess.GetQrcies();

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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>OperationsResponse</returns>
        // GET: api/qrc/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
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
                QrcDataAccess _qrcAccess = new QrcDataAccess(_iconfiguration);

                DatabaseResponse response = await _qrcAccess.GetQrc(id);

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
        /// This will Create a QRC entry.
        /// </summary>
        /// <param name="qrc">
        /// CreateQRCMaster
        ///Body: 
        ///{
        ///	"Name" : "",
        ///	"Description" : "",
        ///	"CreatedBy" : "1"        
        ///}
        /// </param>
        /// <returns>OperationResponse</returns>
        // POST: api/qrc
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateQRCMaster qrc)
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

                QrcDataAccess _qrcAccess = new QrcDataAccess(_iconfiguration);

                DatabaseResponse response = await _qrcAccess.CreateQrc(qrc);

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
        /// This will update qrc entry by id and details passed.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="qrc">
        ///Body: 
        ///{
        ///	"Name" : "", 
        ///	"Description" : "",
        ///	"UpdatedBy" : "1" ,
        ///	"Active":true/false
        ///}       
        /// </param>
        /// <returns>OperationsResponse</returns>
        // PUT: api/qrc/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateQRCMaster qrc)
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

                QrcDataAccess _qrcAccess = new QrcDataAccess(_iconfiguration);

                DatabaseResponse response = await _qrcAccess.UpdateQrc(qrc, id);

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
        /// This will delete a qrc record by the id passed
        /// </summary>
        /// <param name="id"></param>
        /// <returns>OperationResponse</returns>
        // DELETE: api/qrc/5
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
                QrcDataAccess _qrcAccess = new QrcDataAccess(_iconfiguration);

                DatabaseResponse response = await _qrcAccess.DeleteQrc(id);

                if (response.ResponseCode == (int)DbReturnValue.DeleteSuccess)
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = true,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.DeleteSuccess),
                        ReturnedObject = response.Results
                    });
                }
                else if (response.ResponseCode == (int)DbReturnValue.ActiveTryDelete)
                {
                    Log.Error(EnumExtensions.GetDescription(DbReturnValue.ActiveTryDelete));

                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.ActiveTryDelete),
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

        [HttpGet]
        [Route("GetUsers/{qrcId}/{categoryId}")]
        public async Task<IActionResult> GetAllUserAsync(int qrcId, int categoryId)
        {
            QrcDataAccess _qrcAccess = new QrcDataAccess(_iconfiguration);
            DatabaseResponse response = await _qrcAccess.GetUsers(qrcId, categoryId);
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
        [HttpPost("AddQRCUsers")]
        public async Task<IActionResult> AddQRCUserInfo([FromBody] List<QRCUserMapping> objQRCUserMapping)
        {
            try
            {
                if (objQRCUserMapping.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("QRCId", typeof(int));
                    dt.Columns.Add("CategoryId", typeof(int));
                    dt.Columns.Add("UserId", typeof(int));
                    dt.Columns.Add("CreatedBy", typeof(int));
                    DataRow dr = null;
                    foreach (var item in objQRCUserMapping)
                    {
                        dr = dt.NewRow();
                        dr["QRCId"] = item.QRCId;
                        dr["CategoryId"] = item.CategoryId;
                        dr["UserId"] = item.UserId;
                        dr["CreatedBy"] = item.CreatedBy;
                        dt.Rows.Add(dr);
                    }
                    QrcDataAccess _qrcAccess = new QrcDataAccess(_iconfiguration);
                    DatabaseResponse response = await _qrcAccess.AddQRCUsers(dt);


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
                else
                {
                    return Ok(new OperationResponse
                    {
                        HasSucceeded = false,
                        IsDomainValidationErrors = false,
                        Message = EnumExtensions.GetDescription(DbReturnValue.RecordExists),
                        ReturnedObject = null
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
                    StatusCode = ((int) ResponseStatus.ServerError).ToString(),
                    IsDomainValidationErrors = false
                });
            }
        }
    }
}
