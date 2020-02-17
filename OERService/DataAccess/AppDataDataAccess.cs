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
    public class AppDataDataAccess
    {
        internal DataAccessHelper _DataHelper = null;

        private IConfiguration _configuration;

        /// <summary>
        /// Constructor setting configuration
        /// </summary>
        /// <param name="configuration"></param>
        public AppDataDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }      
        public async Task <DatabaseResponse> GetProfileAppData()
        {
            try
            {             

                _DataHelper = new DataAccessHelper("GetProfileAppData", _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);

                List<Title> titles = new List<Title>();

                List<Country> countries = new List<Country>();

                List<State> states = new List<State>();

                List<SocialMedia> socialMedias = new List<SocialMedia>();

                if (ds != null && ds.Tables.Count> 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count>0)
                {
                    //titles
                    titles = (from model in ds.Tables[0].AsEnumerable()
                                  select new Title()
                                  {
                                      Id = model.Field<int>("Id"),
                                      Name = model.Field<string>("Name")   
                                  }).ToList();
                }



                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    // country
                    countries = (from model in ds.Tables[1].AsEnumerable()
                              select new Country()
                              {
                                  Id = model.Field<int>("Id"),
                                  Name = model.Field<string>("Name")
                              }).ToList();
                }



                if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                {
                    // state
                    states = (from model in ds.Tables[2].AsEnumerable()
                              select new State()
                              {
                                  Id = model.Field<int>("Id"),
                                  CountryId = model.Field<int>("CountryId"),
                                  Name = model.Field<string>("Name")
                              }).ToList();
                }



                if (ds != null && ds.Tables.Count > 0 && ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                {
                    // socialmedia
                    socialMedias = (from model in ds.Tables[3].AsEnumerable()
                              select new SocialMedia()
                              {
                                  Id = model.Field<int>("Id"),
                                  Name = model.Field<string>("Name")
                              }).ToList();
                }

                List<Gender> genders = ((GenderEnum[])Enum.GetValues(typeof(GenderEnum))).Select(c => new Gender() { Id = (int)c, Name = c.ToString() }).ToList();

                List<PortalLanguage> portalLanguages = ((PortalLanguageEnum[])Enum.GetValues(typeof(PortalLanguageEnum))).Select(c => new PortalLanguage() { Id = (int)c, Name = c.ToString() }).ToList();

                ProfileAppData appData = new ProfileAppData { Titles=titles, Countries=countries, States=states, SocialMedias=socialMedias, Genders=genders, PortalLanguages=portalLanguages};
                
                return new DatabaseResponse { ResponseCode = result, Results = appData };
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
