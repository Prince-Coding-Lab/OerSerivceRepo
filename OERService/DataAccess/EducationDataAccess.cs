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
    public class EducationDataAccess
    {
        internal DataAccessHelper _DataHelper = null;

        private IConfiguration _configuration;

        /// <summary>
        /// Constructor setting configuration
        /// </summary>
        /// <param name="configuration"></param>
        public EducationDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<DatabaseResponse> CreateEducation(CreateEducationMaster educationMaster)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Name",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CreatedBy",  SqlDbType.Int )                    
                };

                parameters[0].Value = educationMaster.Name;
                parameters[1].Value = educationMaster.CreatedBy;               

                _DataHelper = new DataAccessHelper("CreateEducation", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<EducationMaster> educations = new List<EducationMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    educations = (from model in dt.AsEnumerable()
                                   select new EducationMaster()
                                   {
                                        Id = model.Field<int>("Id"),
                                        Name = model.Field<string>("Name"),
                                        CreatedBy = model.Field<string>("CreatedBy"),
                                        CreatedOn = model.Field<DateTime>("CreatedOn"),
                                         UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                         UpdatedBy = model.Field<string>("UpdatedBy"),
                                         Active = model.Field<bool>("Active")

                                   }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = educations };
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

        public async Task<DatabaseResponse> GetEducations()
        {
            try
            {
                _DataHelper = new DataAccessHelper("GetEducation", _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<EducationMaster> educations = new List<EducationMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    educations = (from model in dt.AsEnumerable()
                                  select new EducationMaster()
                                  {
                                      Id = model.Field<int>("Id"),
                                      Name = model.Field<string>("Name"),
                                      CreatedBy = model.Field<string>("CreatedBy"),
                                      CreatedOn = model.Field<DateTime>("CreatedOn"),
                                      UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                      UpdatedBy = model.Field<string>("UpdatedBy"),
                                      Active = model.Field<bool>("Active")

                                  }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = educations };
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

        public async Task<DatabaseResponse> GetEducation(int id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.NVarChar )
                   
                };

                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("GetEducationById",parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<EducationMaster> educations = new List<EducationMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    educations = (from model in dt.AsEnumerable()
                                  select new EducationMaster()
                                  {
                                      Id = model.Field<int>("Id"),
                                      Name = model.Field<string>("Name"),
                                      CreatedBy = model.Field<string>("CreatedBy"),
                                      CreatedOn = model.Field<DateTime>("CreatedOn"),
                                      UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                      UpdatedBy = model.Field<string>("UpdatedBy"),
                                      Active = model.Field<bool>("Active")

                                  }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = educations };
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

        public async Task<DatabaseResponse> DeleteEducation(int id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.NVarChar )

                };

                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("DeleteEducation", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<EducationMaster> educations = new List<EducationMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    educations = (from model in dt.AsEnumerable()
                                  select new EducationMaster()
                                  {
                                      Id = model.Field<int>("Id"),
                                      Name = model.Field<string>("Name"),
                                      CreatedBy = model.Field<string>("CreatedBy"),
                                      CreatedOn = model.Field<DateTime>("CreatedOn"),
                                      UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                      UpdatedBy = model.Field<string>("UpdatedBy"),
                                      Active = model.Field<bool>("Active")

                                  }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = educations };
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

        public async Task<DatabaseResponse> UpdateEducation(UpdateEducationMaster educationMaster, int id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.Int ),
                    new SqlParameter( "@Name",  SqlDbType.NVarChar ),
                    new SqlParameter( "@UpdatedBy",  SqlDbType.Int ),
                    new SqlParameter( "@Active",  SqlDbType.Bit )
                    
                };


                parameters[0].Value = id;
                parameters[1].Value = educationMaster.Name;
                parameters[2].Value = educationMaster.UpdatedBy;
                parameters[3].Value = educationMaster.Active;
                _DataHelper = new DataAccessHelper("UpdateEducation", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<EducationMaster> educations = new List<EducationMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    educations = (from model in dt.AsEnumerable()
                                  select new EducationMaster()
                                  {
                                      Id = model.Field<int>("Id"),
                                      Name = model.Field<string>("Name"),
                                      CreatedBy = model.Field<string>("CreatedBy"),
                                      CreatedOn = model.Field<DateTime>("CreatedOn"),
                                      UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                      UpdatedBy = model.Field<string>("UpdatedBy"),
                                      Active = model.Field<bool>("Active")

                                  }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = educations };
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
