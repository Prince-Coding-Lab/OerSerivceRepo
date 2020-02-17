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
    public class MaterialTypeDataAccess
    {
        internal DataAccessHelper _DataHelper = null;

        private IConfiguration _configuration;

        /// <summary>
        /// Constructor setting configuration
        /// </summary>
        /// <param name="configuration"></param>
        public MaterialTypeDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<DatabaseResponse> CreateMaterialType(CreateMaterialTypeMaster materialType)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Name",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CreatedBy",  SqlDbType.Int )                    
                };

                parameters[0].Value = materialType.Name;
                parameters[1].Value = materialType.CreatedBy;               

                _DataHelper = new DataAccessHelper("CreateMaterialType", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<MaterialTypeMaster> categories = new List<MaterialTypeMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    categories = (from model in dt.AsEnumerable()
                                   select new MaterialTypeMaster()
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

        public async Task<DatabaseResponse> GetMaterialTypes()
        {
            try
            {
                _DataHelper = new DataAccessHelper("GetMaterialType", _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<MaterialTypeMaster> categories = new List<MaterialTypeMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    categories = (from model in dt.AsEnumerable()
                                  select new MaterialTypeMaster()
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

        public async Task<DatabaseResponse> GetMaterialType(int id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.NVarChar )
                   
                };

                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("GetMaterialTypeById",parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<MaterialTypeMaster> categories = new List<MaterialTypeMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    categories = (from model in dt.AsEnumerable()
                                  select new MaterialTypeMaster()
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

        public async Task<DatabaseResponse> DeleteMaterialType(int id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.NVarChar )

                };

                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("DeleteMaterialType", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<MaterialTypeMaster> categories = new List<MaterialTypeMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    categories = (from model in dt.AsEnumerable()
                                  select new MaterialTypeMaster()
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

        public async Task<DatabaseResponse> UpdateMaterialType(UpdateMaterialTypeMaster materialType, int id)
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
                parameters[1].Value = materialType.Name;
                parameters[2].Value = materialType.UpdatedBy;
                parameters[3].Value = materialType.Active;
                _DataHelper = new DataAccessHelper("UpdateMaterialType", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<MaterialTypeMaster> categories = new List<MaterialTypeMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    categories = (from model in dt.AsEnumerable()
                                  select new MaterialTypeMaster()
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
