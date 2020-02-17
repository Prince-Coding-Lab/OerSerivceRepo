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
using Core.Extensions;
using Serilog;

namespace OERService.DataAccess
{
    public class ConfigDataAccess
    {
        internal DataAccessHelper _DataHelper = null;

        private IConfiguration _configuration;

        /// <summary>
        /// Constructor setting configuration
        /// </summary>
        /// <param name="configuration"></param>
        public ConfigDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<DatabaseResponse> GetConfiguration(string configType)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@ConfigType",  SqlDbType.NVarChar )

                };

                parameters[0].Value = configType;          

                _DataHelper = new DataAccessHelper("GetConfigurationsByType", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt); // 102 /105

                DatabaseResponse response = new DatabaseResponse();

                if (result == 105)
                {

                    List<Dictionary<string, string>> configDictionary = new List<Dictionary<string, string>>();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        configDictionary = LinqExtensions.GetDictionary(dt);
                    }

                    response = new DatabaseResponse { ResponseCode = result, Results = configDictionary };

                }

                else
                {
                    response = new DatabaseResponse { ResponseCode = result };
                }

                return response;
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
