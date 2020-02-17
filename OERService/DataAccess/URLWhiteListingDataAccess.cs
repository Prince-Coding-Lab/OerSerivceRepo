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
    public class URLWhiteListingDataAccess
    {
        internal DataAccessHelper _DataHelper = null;

        private IConfiguration _configuration;

        /// <summary>
        /// Constructor setting configuration
        /// </summary>
        /// <param name="configuration"></param>
        public URLWhiteListingDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<DatabaseResponse> RequestWhitelisting(WhiteListingRequest request)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@URL",  SqlDbType.NVarChar ),

                    new SqlParameter( "@RequestedBy",  SqlDbType.Int )                    
                };
                parameters[0].Value = request.URL;

                parameters[1].Value = request.RequestedBy;               

                _DataHelper = new DataAccessHelper("CreateWhiteListingRequest", parameters, _configuration);
               

                int result = await _DataHelper.RunAsync(); 

                return new DatabaseResponse { ResponseCode = result};
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

        public async Task<DatabaseResponse> GetWhiteListedUrls()
        {
            try
            {
                _DataHelper = new DataAccessHelper("GetWhitelistingRequests", _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<WhiteListingURLs> whiteListingURLs = new List<WhiteListingURLs>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    whiteListingURLs = (from model in dt.AsEnumerable()
                                  select new WhiteListingURLs()
                                  {
                                      Id = model.Field<decimal>("Id"),
                                      RequestedBy = model.Field<string>("RequestedBy"),
                                      RequestedOn = model.Field<DateTime>("RequestedOn"),
                                      URL = model.Field<string>("URL"),
                                      VerifiedBy = model.Field<string>("VerifiedBy"),
                                      VerifiedOn = model.Field<DateTime?>("VerifiedOn")==null?null: model.Field<DateTime?>("VerifiedOn"),
                                      IsApproved = model.Field<bool>("IsApproved"),                                      
                                      RejectedReason  = model.Field<string>("RejectedReason")
                                  }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = whiteListingURLs };
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
      
        public async Task<DatabaseResponse> WhiteListUrl(WhiteListUrl request, int id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.Decimal ),
                    new SqlParameter( "@VerifiedBy",  SqlDbType.Int ),
                    new SqlParameter( "@IsApproved",  SqlDbType.Bit ),
                    new SqlParameter( "@RejectedReason",  SqlDbType.NVarChar )
                };


                parameters[0].Value = id;
                parameters[1].Value = request.VerifiedBy;
                parameters[2].Value = request.IsApproved;
                parameters[3].Value = request.RejectedReason;

                _DataHelper = new DataAccessHelper("VerifyWhiteListingRequest", parameters, _configuration);               

                int result = await _DataHelper.RunAsync();             

                return new DatabaseResponse { ResponseCode = result};
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

        public async Task<DatabaseResponse> IsWhiteListed(string url)
        {
            try
            {
              

                SqlParameter[] parameters =
                 {
                    new SqlParameter( "@URL",  SqlDbType.NVarChar )
                    
                };
                parameters[0].Value = url;

                _DataHelper = new DataAccessHelper("UrlIsWhitelisted", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

               WhiteListingURLs whiteListingURL = new WhiteListingURLs();

                if (dt != null && dt.Rows.Count > 0)
                {

                    whiteListingURL = (from model in dt.AsEnumerable()
                                        select new WhiteListingURLs()
                                        {
                                            Id = model.Field<decimal>("Id"),
                                            RequestedBy = model.Field<string>("RequestedBy"),
                                            RequestedOn = model.Field<DateTime>("RequestedOn"),
                                            URL = model.Field<string>("URL"),
                                            VerifiedBy = model.Field<string>("VerifiedBy"),
                                            VerifiedOn = model.Field<DateTime?>("VerifiedOn") == null ? null : model.Field<DateTime?>("VerifiedOn"),
                                            IsApproved = model.Field<bool>("IsApproved"),
                                            RejectedReason = model.Field<string>("RejectedReason")
                                        }).FirstOrDefault();
                }

                return new DatabaseResponse { ResponseCode = result, Results = whiteListingURL };
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
