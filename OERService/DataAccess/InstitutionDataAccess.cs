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
    public class InstitutionDataAccess
    {
        internal DataAccessHelper _DataHelper = null;

        private IConfiguration _configuration;

        /// <summary>
        /// Constructor setting configuration
        /// </summary>
        /// <param name="configuration"></param>
        public InstitutionDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<DatabaseResponse> CreateInstitution(CreateInstitutionMaster InstitutionMaster)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Name",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CreatedBy",  SqlDbType.Int )                    
                };

                parameters[0].Value = InstitutionMaster.Name;
                parameters[1].Value = InstitutionMaster.CreatedBy;               

                _DataHelper = new DataAccessHelper("CreateInstitution", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<InstitutionMaster> categories = new List<InstitutionMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    categories = (from model in dt.AsEnumerable()
                                   select new InstitutionMaster()
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

                return new DatabaseResponse { ResponseCode = result, Results = categories };
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

        public async Task<DatabaseResponse> GetInstitutions()
        {
            try
            {
                _DataHelper = new DataAccessHelper("GetInstitution", _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<InstitutionMaster> categories = new List<InstitutionMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    categories = (from model in dt.AsEnumerable()
                                  select new InstitutionMaster()
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

                return new DatabaseResponse { ResponseCode = result, Results = categories };
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

        public async Task<DatabaseResponse> GetInstitution(int id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.NVarChar )
                   
                };

                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("GetInstitutionById",parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<InstitutionMaster> categories = new List<InstitutionMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    categories = (from model in dt.AsEnumerable()
                                  select new InstitutionMaster()
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

                return new DatabaseResponse { ResponseCode = result, Results = categories };
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

        public async Task<DatabaseResponse> DeleteInstitution(int id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.NVarChar )

                };

                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("DeleteInstitution", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<InstitutionMaster> categories = new List<InstitutionMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    categories = (from model in dt.AsEnumerable()
                                  select new InstitutionMaster()
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

                return new DatabaseResponse { ResponseCode = result, Results = categories };
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

        public async Task<DatabaseResponse> UpdateInstitution(UpdateInstitutionMaster InstitutionMaster, int id)
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
                parameters[1].Value = InstitutionMaster.Name;
                parameters[2].Value = InstitutionMaster.UpdatedBy;
                parameters[3].Value = InstitutionMaster.Active;
                _DataHelper = new DataAccessHelper("UpdateInstitution", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<InstitutionMaster> categories = new List<InstitutionMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    categories = (from model in dt.AsEnumerable()
                                  select new InstitutionMaster()
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

                return new DatabaseResponse { ResponseCode = result, Results = categories };
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
