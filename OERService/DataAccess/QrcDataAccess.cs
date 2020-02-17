using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Core.Helpers;
using Microsoft.Extensions.Configuration;
using Core.Models;
using Core.Enums;
using OERService.Models;
using Serilog;

namespace OERService.DataAccess
{
    public class QrcDataAccess
    {
        internal DataAccessHelper _DataHelper = null;

        private IConfiguration _configuration;

        /// <summary>
        /// Constructor setting configuration
        /// </summary>
        /// <param name="configuration"></param>
        public QrcDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<DatabaseResponse> CreateQrc(CreateQRCMaster qrcMaster)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Name",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Description",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CreatedBy",  SqlDbType.Int )
                };

                parameters[0].Value = qrcMaster.Name;
                parameters[1].Value = qrcMaster.Description;
                parameters[2].Value = qrcMaster.CreatedBy;

                _DataHelper = new DataAccessHelper("CreateQrc", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<QRCMaster> qrcs = new List<QRCMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    qrcs = (from model in dt.AsEnumerable()
                            select new QRCMaster()
                            {
                                Id = model.Field<int>("Id"),
                                Name = model.Field<string>("Name"),
                                Description = model.Field<string>("Description"),
                                CreatedBy = model.Field<string>("CreatedBy"),
                                CreatedOn = model.Field<DateTime>("CreatedOn"),
                                UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                UpdatedBy = model.Field<string>("UpdatedBy"),
                                Active = model.Field<bool>("Active")

                            }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = qrcs };
            }

            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                throw (ex);
            }
            finally
            {
                _DataHelper.Dispose();
            }
        }

        public async Task<DatabaseResponse> GetQrcies()
        {
            try
            {
                _DataHelper = new DataAccessHelper("GetQrc", _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<QRCMaster> qrcs = new List<QRCMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    qrcs = (from model in dt.AsEnumerable()
                            select new QRCMaster()
                            {
                                Id = model.Field<int>("Id"),
                                Name = model.Field<string>("Name"),
                                Description = model.Field<string>("Description"),
                                CreatedBy = model.Field<string>("CreatedBy"),
                                CreatedOn = model.Field<DateTime>("CreatedOn"),
                                UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                UpdatedBy = model.Field<string>("UpdatedBy"),
                                Active = model.Field<bool>("Active")

                            }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = qrcs };
            }

            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                throw (ex);
            }
            finally
            {
                _DataHelper.Dispose();
            }
        }

        public async Task<DatabaseResponse> GetQrc(int id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.NVarChar )

                };

                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("GetQrcById", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<QRCMaster> qrcs = new List<QRCMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    qrcs = (from model in dt.AsEnumerable()
                            select new QRCMaster()
                            {
                                Id = model.Field<int>("Id"),
                                Name = model.Field<string>("Name"),
                                Description = model.Field<string>("Description"),
                                CreatedBy = model.Field<string>("CreatedBy"),
                                CreatedOn = model.Field<DateTime>("CreatedOn"),
                                UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                UpdatedBy = model.Field<string>("UpdatedBy"),
                                Active = model.Field<bool>("Active")

                            }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = qrcs };
            }

            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                throw (ex);
            }
            finally
            {
                _DataHelper.Dispose();
            }
        }

        public async Task<DatabaseResponse> DeleteQrc(int id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.NVarChar )

                };

                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("DeleteQrc", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<QRCMaster> qrcs = new List<QRCMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    qrcs = (from model in dt.AsEnumerable()
                            select new QRCMaster()
                            {
                                Id = model.Field<int>("Id"),
                                Name = model.Field<string>("Name"),
                                Description = model.Field<string>("Description"),
                                CreatedBy = model.Field<string>("CreatedBy"),
                                CreatedOn = model.Field<DateTime>("CreatedOn"),
                                UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                UpdatedBy = model.Field<string>("UpdatedBy"),
                                Active = model.Field<bool>("Active")
                            }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = qrcs };
            }

            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                throw (ex);
            }
            finally
            {
                _DataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> GetUsers(int qrcId, int categoryId)
        {
            try
            {
                _DataHelper = new DataAccessHelper("sps_GetUserForQRC", _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<QRCUsers> users = new List<QRCUsers>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    users = (from model in dt.AsEnumerable()
                            select new QRCUsers()
                            {
                                UserId = model.Field<int>("Id"),
                                UserName = model.Field<string>("UserName"),
                                ResourceContributed = model.Field<int>("ResourceContributed"),
                                CourseCreated = model.Field<int>("CourseCreated"),
                                CurrentQRCS = model.Field<int>("CurrentQRCS"),
                                NoOfReviews = model.Field<int>("NoOfReviews")
                               

                            }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = users };
            }

            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                throw (ex);
            }
            finally
            {
                _DataHelper.Dispose();
            }
        }
        public async Task<DatabaseResponse> AddQRCUsers(DataTable dt)
        {

            try
            {

                SqlParameter[] parameters =
            {
                    new SqlParameter( "@QRCUserDetails",  SqlDbType.Structured )

                };

                parameters[0].Value = dt;


                _DataHelper = new DataAccessHelper("USP_Insert_QRCUsers", parameters, _configuration);

                DataTable dt1 = new DataTable();

                int result = await _DataHelper.RunAsync(dt);
                return new DatabaseResponse { ResponseCode = result, Results = null };
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<DatabaseResponse> UpdateQrc(UpdateQRCMaster qrcMaster, int id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.Int ),
                    new SqlParameter( "@Name",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Description",  SqlDbType.NVarChar ),
                    new SqlParameter( "@UpdatedBy",  SqlDbType.Int ),
                    new SqlParameter( "@Active",  SqlDbType.Bit )

                };


                parameters[0].Value = id;
                parameters[1].Value = qrcMaster.Name;
                parameters[2].Value = qrcMaster.Description;
                parameters[3].Value = qrcMaster.UpdatedBy;
                parameters[4].Value = qrcMaster.Active;
                _DataHelper = new DataAccessHelper("UpdateQrc", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<QRCMaster> qrcs = new List<QRCMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    qrcs = (from model in dt.AsEnumerable()
                            select new QRCMaster()
                            {
                                Id = model.Field<int>("Id"),
                                Name = model.Field<string>("Name"),
                                Description = model.Field<string>("Description"),
                                CreatedBy = model.Field<string>("CreatedBy"),
                                CreatedOn = model.Field<DateTime>("CreatedOn"),
                                UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                UpdatedBy = model.Field<string>("UpdatedBy"),
                                Active = model.Field<bool>("Active")

                            }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = qrcs };
            }

            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));

                throw (ex);
            }
            finally
            {
                _DataHelper.Dispose();
            }
        }

    }
}
