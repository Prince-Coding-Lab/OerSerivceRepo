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
    public class CopyrightDataAccess
    {
        internal DataAccessHelper _DataHelper = null;

        private IConfiguration _configuration;

        /// <summary>
        /// Constructor setting configuration
        /// </summary>
        /// <param name="configuration"></param>
        public CopyrightDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<DatabaseResponse> CreateCopyright(CreateCopyrightMaster copyright)
        {
            try
            {

                SqlParameter[] parameters =
                {                    
                    new SqlParameter( "@Title",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Description",  SqlDbType.Text ),
                    new SqlParameter( "@CreatedBy",  SqlDbType.Int )  
                };

                parameters[0].Value = copyright.Title;
                parameters[1].Value = copyright.Description;
                parameters[2].Value = copyright.CreatedBy;              

                _DataHelper = new DataAccessHelper("CreateCopyright", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<CopyrightMaster> copyrights = new List<CopyrightMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    copyrights = (from model in dt.AsEnumerable()
                                   select new CopyrightMaster()
                                   {
                                        Id = model.Field<int>("Id"),
                                         Title = model.Field<string>("Title"),
                                        Description = model.Field<string>("Description"),
                                       CreatedBy = model.Field<string>("CreatedBy"),
                                        CreatedOn = model.Field<DateTime>("CreatedOn"),
                                         UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                         UpdatedBy = model.Field<string>("UpdatedBy"),
                                         Active = model.Field<bool>("Active")

                                   }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = copyrights };
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

        public async Task<DatabaseResponse> GetCopyrights()
        {
            try
            {
                _DataHelper = new DataAccessHelper("GetCopyright", _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<CopyrightMaster> copyrights = new List<CopyrightMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    copyrights = (from model in dt.AsEnumerable()
                                  select new CopyrightMaster()
                                  {
                                      Id = model.Field<int>("Id"),
                                      Title = model.Field<string>("Title"),
                                      Description = model.Field<string>("Description"),
                                      CreatedBy = model.Field<string>("CreatedBy"),
                                      CreatedOn = model.Field<DateTime>("CreatedOn"),
                                      UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                      UpdatedBy = model.Field<string>("UpdatedBy"),
                                      Active = model.Field<bool>("Active")

                                  }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = copyrights };
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

        public async Task<DatabaseResponse> GetCopyright(int id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.NVarChar )
                   
                };

                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("GetCopyrightById", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<CopyrightMaster> copyrights = new List<CopyrightMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    copyrights = (from model in dt.AsEnumerable()
                                  select new CopyrightMaster()
                                  {
                                      Id = model.Field<int>("Id"),
                                      Title = model.Field<string>("Title"),
                                      Description = model.Field<string>("Description"),
                                      CreatedBy = model.Field<string>("CreatedBy"),
                                      CreatedOn = model.Field<DateTime>("CreatedOn"),
                                      UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                      UpdatedBy = model.Field<string>("UpdatedBy"),
                                      Active = model.Field<bool>("Active")

                                  }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = copyrights };
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

        public async Task<DatabaseResponse> DeleteCopyright(int id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.NVarChar )

                };

                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("DeleteCopyright", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<CopyrightMaster> copyrights = new List<CopyrightMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    copyrights = (from model in dt.AsEnumerable()
                                  select new CopyrightMaster()
                                  {
                                      Id = model.Field<int>("Id"),
                                      Title = model.Field<string>("Title"),
                                      Description = model.Field<string>("Description"),
                                      CreatedBy = model.Field<string>("CreatedBy"),
                                      CreatedOn = model.Field<DateTime>("CreatedOn"),
                                      UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                      UpdatedBy = model.Field<string>("UpdatedBy"),
                                      Active = model.Field<bool>("Active")

                                  }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = copyrights };
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

        public async Task<DatabaseResponse> UpdateCopyright(UpdateCopyrightMaster Copyright, int id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.Int ),
                    new SqlParameter( "@Title",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Description",  SqlDbType.Text ),
                    new SqlParameter( "@UpdatedBy",  SqlDbType.Int ),
                    new SqlParameter( "@Active",  SqlDbType.Bit )
                    
                };


                parameters[0].Value = id;
                parameters[1].Value = Copyright.Title;
                parameters[2].Value = Copyright.Description;
                parameters[3].Value = Copyright.Active;
                parameters[4].Value = Copyright.UpdatedBy;
                _DataHelper = new DataAccessHelper("UpdateCopyright", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<CopyrightMaster> copyrights = new List<CopyrightMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    copyrights = (from model in dt.AsEnumerable()
                                  select new CopyrightMaster()
                                  {
                                      Id = model.Field<int>("Id"),
                                      Title = model.Field<string>("Title"),
                                      Description = model.Field<string>("Description"),
                                      CreatedBy = model.Field<string>("CreatedBy"),
                                      CreatedOn = model.Field<DateTime>("CreatedOn"),
                                      UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                      UpdatedBy = model.Field<string>("UpdatedBy"),
                                      Active = model.Field<bool>("Active")

                                  }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = copyrights };
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
