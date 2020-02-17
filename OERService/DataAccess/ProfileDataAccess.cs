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
    public class ProfileDataAccess
    {
        internal DataAccessHelper _DataHelper = null;

        private IConfiguration _configuration;

        /// <summary>
        /// Constructor setting configuration
        /// </summary>
        /// <param name="configuration"></param>
        public ProfileDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<DatabaseResponse> CreateProfile(CreateUserProfileRequest profileRequest)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@TitleId",  SqlDbType.Int ),
                    new SqlParameter( "@FirstName",  SqlDbType.NVarChar ),
                    new SqlParameter( "@MiddleName",  SqlDbType.NVarChar ),
                    new SqlParameter( "@LastName",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CountryId",  SqlDbType.Int ),
                    new SqlParameter( "@StateId",  SqlDbType.Int ),
                    new SqlParameter( "@Gender",  SqlDbType.Int ),
                    new SqlParameter( "@Email",  SqlDbType.NVarChar ),
                    new SqlParameter( "@PortalLanguageId",  SqlDbType.Int ),
                   // new SqlParameter( "@DepartmentId",  SqlDbType.Int ),
                   // new SqlParameter( "@DesignationId",  SqlDbType.Int ),
                    new SqlParameter( "@DateOfBirth",  SqlDbType.DateTime ),
                    new SqlParameter( "@Photo",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ProfileDescription",  SqlDbType.NVarChar ),
                    new SqlParameter( "@SubjectsInterested",  SqlDbType.NVarChar ),
                    new SqlParameter( "@IsContributor",  SqlDbType.NVarChar ),
                    new SqlParameter( "@UserCertifications",  SqlDbType.NVarChar ),
                    new SqlParameter( "@UserEducations",  SqlDbType.NVarChar ),
                    new SqlParameter( "@UserExperiences",  SqlDbType.NVarChar ),
                    new SqlParameter( "@UserLanguages",  SqlDbType.NVarChar ),
                    new SqlParameter( "@UserSocialMedias",  SqlDbType.NVarChar ),
                };

                CommonHelper helper = new CommonHelper();

                parameters[0].Value =  profileRequest.TitleId==null || profileRequest.TitleId==0 ? null: profileRequest.TitleId;
                parameters[1].Value =  profileRequest.FirstName;
                parameters[2].Value =  profileRequest.MiddleName;
                parameters[3].Value =  profileRequest.LastName;
                parameters[4].Value =  profileRequest.CountryId==null || profileRequest.CountryId==0?null: profileRequest.CountryId;
                parameters[5].Value =  profileRequest.StateId == null || profileRequest.StateId == 0 ? null : profileRequest.StateId; 
                parameters[6].Value =  profileRequest.Gender;
                parameters[7].Value =  profileRequest.Email;
                parameters[8].Value =  profileRequest.PortalLanguageId == null || profileRequest.PortalLanguageId == 0 ? null : profileRequest.PortalLanguageId;             
                parameters[9].Value =  profileRequest.DateOfBirth;
                parameters[10].Value = profileRequest.Photo;
                parameters[11].Value = profileRequest.ProfileDescription;
                parameters[12].Value = profileRequest.SubjectsInterested;
                parameters[13].Value = profileRequest.IsContributor;
                parameters[14].Value = profileRequest.UserCertifications!=null? helper.GetJsonString(profileRequest.UserCertifications):null;
                parameters[15].Value = profileRequest.UserEducations!=null?helper.GetJsonString(profileRequest.UserEducations):null;
                parameters[16].Value = profileRequest.UserExperiences!=null?helper.GetJsonString(profileRequest.UserExperiences):null;
                parameters[17].Value = profileRequest.UserLanguages!=null? helper.GetJsonString(profileRequest.UserLanguages):null;
                parameters[18].Value = profileRequest.UserSocialMedias!=null? helper.GetJsonString(profileRequest.UserSocialMedias):null;

                _DataHelper = new DataAccessHelper("CreateUserProfile", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                UserMaster user = new UserMaster();

                if (dt != null && dt.Rows.Count > 0)
                {

                    user = (from model in dt.AsEnumerable()
                                  select new UserMaster()
                                  {
                                      Id = model.Field<int>("Id")                                     
                                  }).FirstOrDefault();
                }

                return new DatabaseResponse { ResponseCode = result, Results = user };
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
        public async Task<DatabaseResponse> GetUserProfile(string email)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Email",  SqlDbType.NVarChar )

                };

                parameters[0].Value = email;

                _DataHelper = new DataAccessHelper("GetUserProfileByEmail", parameters, _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);

                UserProfile profile = new UserProfile();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0]!=null && ds.Tables[0].Rows.Count > 0)
                {

                    profile = (from model in ds.Tables[0].AsEnumerable()
                                  select new UserProfile()
                                  {
                                      Id = model.Field<int>("Id"),
                                      Title = model.Field<int?>("TitleId") == null ? null : new Title { Id = model.Field<int>("TitleId"), Name = model.Field<string>("Title") },
                                      FirstName = model.Field<string>("FirstName"),
                                      MiddleName = model.Field<string>("MiddleName"),
                                      LastName = model.Field<string>("LastName"),
                                      Country = model.Field<int?>("CountryId") == null ? null : new Country { Id = model.Field<int>("CountryId"), Name = model.Field<string>("Country") },
                                      State = model.Field<int?>("StateId") == null ? null : new State { Id = model.Field<int>("StateId"), Name = model.Field<string>("State") },
                                      Gender = new Gender { Id = model.Field<int>("Gender"), Name = ((GenderEnum)model.Field<int>("Gender")).ToString() },
                                      Email = model.Field<string>("Email"),
                                      PortalLanguage = model.Field<int?>("PortalLanguageId") == null ? null : new PortalLanguage { Id = model.Field<int>("PortalLanguageId"), Name = ((PortalLanguageEnum)model.Field<int>("PortalLanguageId")).ToString() },
                                      // Department = new Department { Id = model.Field<int>("DepartmentId"), Name = model.Field<string>("Department") },
                                      // Designation = new Designation { Id = model.Field<int>("DesignationId"), Name = model.Field<string>("Designation") },
                                      DateOfBirth = model.Field<DateTime>("DateOfBirth"),
                                      Photo = model.Field<string>("Photo"),
                                      ProfileDescription = model.Field<string>("ProfileDescription"),
                                      SubjectsInterested = model.Field<string>("SubjectsInterested"),
                                      ApprovalStatus = model.Field<bool>("ApprovalStatus"),
                                      CreatedOn = model.Field<DateTime>("CreatedOn"),
                                      UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                      Active = model.Field<bool>("Active"),
                                      IsContributor = model.Field<bool>("IsContributor"),

                                  }).FirstOrDefault();
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {

                   profile.UserCertifications = (from model in ds.Tables[1].AsEnumerable()
                               select new UserCertification()
                               {
                                   Id = model.Field<decimal>("Id"),                                    
                                   CertificationName = model.Field<string>("CertificationName"),
                                   CreatedOn = model.Field<DateTime>("CreatedOn"),
                                   Year= model.Field<int>("Year"),
                               }).ToList();
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                {

                    profile.UserEducations = (from model in ds.Tables[2].AsEnumerable()
                                                  select new UserEducation()
                                                  {
                                                      Id = model.Field<decimal>("Id"),
                                                      UniversitySchool = model.Field<string>("UniversitySchool"),
                                                      Major = model.Field<string>("Major"),
                                                      FromDate=model.Field<DateTime>("FromDate"),
                                                      ToDate = model.Field<DateTime>("ToDate"),
                                                      CreatedOn = model.Field<DateTime>("CreatedOn")
                                                    
                                                  }).ToList();
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                {

                    profile.UserLanguages = (from model in ds.Tables[3].AsEnumerable()
                                              select new UserLanguage()
                                              {
                                                   Id = model.Field<decimal>("Id"),
                                                   Language = new Language { Id = model.Field<int>("LanguageId"), Name = model.Field<string>("Language") },
                                                   IsRead = model.Field<bool>("IsRead"),
                                                   IsSpeak = model.Field<bool>("IsSpeak"),
                                                   IsWrite = model.Field<bool>("IsWrite"),
                                                   CreatedOn = model.Field<DateTime>("CreatedOn")

                                              }).ToList();
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
                {

                    profile.UserExperiences = (from model in ds.Tables[4].AsEnumerable()
                                             select new UserExperience()
                                             {
                                                 Id = model.Field<decimal>("Id"),
                                                 Designation = model.Field<string>("Designation"),
                                                 OrganizationName = model.Field<string>("OrganizationName"),
                                                 FromDate = model.Field<DateTime>("FromDate"),
                                                 ToDate = model.Field<DateTime>("ToDate"),
                                                 CreatedOn = model.Field<DateTime>("CreatedOn")

                                             }).ToList();
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[5] != null && ds.Tables[5].Rows.Count > 0)
                {

                    profile.UserSocialMedias = (from model in ds.Tables[5].AsEnumerable()
                                               select new UserSocialMedia()
                                               {
                                                   Id = model.Field<decimal>("Id"),
                                                   SocialMedia = new SocialMedia { Id = model.Field<int>("SocialMediaId"), Name = model.Field<string>("SocialMedia") },
                                                   URL = model.Field<string>("URL"),                                                  
                                                   CreatedOn = model.Field<DateTime>("CreatedOn")

                                               }).ToList();
                }
                return new DatabaseResponse { ResponseCode = result, Results = profile };
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

        public async Task<DatabaseResponse> GetUserProfileById(int  id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.Int )

                };

                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("GetUserProfileById", parameters, _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);

                UserProfile profile = new UserProfile();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {

                    profile = (from model in ds.Tables[0].AsEnumerable()
                               select new UserProfile()
                               {
                                   Id = model.Field<int>("Id"),
                                   Title = model.Field<int?>("TitleId") == null ? null : new Title { Id = model.Field<int>("TitleId"), Name = model.Field<string>("Title") },
                                   FirstName = model.Field<string>("FirstName"),
                                   MiddleName = model.Field<string>("MiddleName"),
                                   LastName = model.Field<string>("LastName"),
                                   Country = model.Field<int?>("CountryId") == null ? null : new Country { Id = model.Field<int>("CountryId"), Name = model.Field<string>("Country") },
                                   State = model.Field<int?>("StateId") == null ? null : new State { Id = model.Field<int>("StateId"), Name = model.Field<string>("State") },
                                   Gender = new Gender { Id = model.Field<int>("Gender"), Name = ((GenderEnum)model.Field<int>("Gender")).ToString() },
                                   Email = model.Field<string>("Email"),
                                   PortalLanguage = model.Field<int?>("PortalLanguageId") == null ? null : new PortalLanguage { Id = model.Field<int>("PortalLanguageId"), Name = ((PortalLanguageEnum)model.Field<int>("PortalLanguageId")).ToString() },
                                   // Department = new Department { Id = model.Field<int>("DepartmentId"), Name = model.Field<string>("Department") },
                                   // Designation = new Designation { Id = model.Field<int>("DesignationId"), Name = model.Field<string>("Designation") },
                                   DateOfBirth = model.Field<DateTime>("DateOfBirth"),
                                   Photo = model.Field<string>("Photo"),
                                   ProfileDescription = model.Field<string>("ProfileDescription"),
                                   SubjectsInterested = model.Field<string>("SubjectsInterested"),
                                   ApprovalStatus = model.Field<bool>("ApprovalStatus"),
                                   CreatedOn = model.Field<DateTime>("CreatedOn"),
                                   UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                   Active = model.Field<bool>("Active"),
                                   IsContributor = model.Field<bool>("IsContributor"),

                               }).FirstOrDefault();
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {

                    profile.UserCertifications = (from model in ds.Tables[1].AsEnumerable()
                                                  select new UserCertification()
                                                  {
                                                      Id = model.Field<decimal>("Id"),
                                                      CertificationName = model.Field<string>("CertificationName"),
                                                      CreatedOn = model.Field<DateTime>("CreatedOn"),
                                                      Year = model.Field<int>("Year"),
                                                  }).ToList();
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                {

                    profile.UserEducations = (from model in ds.Tables[2].AsEnumerable()
                                              select new UserEducation()
                                              {
                                                  Id = model.Field<decimal>("Id"),
                                                  UniversitySchool = model.Field<string>("UniversitySchool"),
                                                  Major = model.Field<string>("Major"),
                                                  FromDate = model.Field<DateTime>("FromDate"),
                                                  ToDate = model.Field<DateTime>("ToDate"),
                                                  CreatedOn = model.Field<DateTime>("CreatedOn")

                                              }).ToList();
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                {

                    profile.UserLanguages = (from model in ds.Tables[3].AsEnumerable()
                                             select new UserLanguage()
                                             {
                                                 Id = model.Field<decimal>("Id"),
                                                 Language = new Language { Id = model.Field<int>("LanguageId"), Name = model.Field<string>("Language") },
                                                 IsRead = model.Field<bool>("IsRead"),
                                                 IsSpeak = model.Field<bool>("IsSpeak"),
                                                 IsWrite = model.Field<bool>("IsWrite"),
                                                 CreatedOn = model.Field<DateTime>("CreatedOn")

                                             }).ToList();
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
                {

                    profile.UserExperiences = (from model in ds.Tables[4].AsEnumerable()
                                               select new UserExperience()
                                               {
                                                   Id = model.Field<decimal>("Id"),
                                                   Designation = model.Field<string>("Designation"),
                                                   OrganizationName = model.Field<string>("OrganizationName"),
                                                   FromDate = model.Field<DateTime>("FromDate"),
                                                   ToDate = model.Field<DateTime>("ToDate"),
                                                   CreatedOn = model.Field<DateTime>("CreatedOn")

                                               }).ToList();
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[5] != null && ds.Tables[5].Rows.Count > 0)
                {

                    profile.UserSocialMedias = (from model in ds.Tables[5].AsEnumerable()
                                                select new UserSocialMedia()
                                                {
                                                    Id = model.Field<decimal>("Id"),
                                                    SocialMedia = new SocialMedia { Id = model.Field<int>("SocialMediaId"), Name = model.Field<string>("SocialMedia") },
                                                    URL = model.Field<string>("URL"),
                                                    CreatedOn = model.Field<DateTime>("CreatedOn")

                                                }).ToList();
                }
                return new DatabaseResponse { ResponseCode = result, Results = profile };
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

        public async Task<DatabaseResponse> UpdateProfile(UpdateUserProfileRequest profileRequest)
        {
            try
            {

                SqlParameter[] parameters =
                {
                    new SqlParameter( "@TitleId",  SqlDbType.Int ),
                    new SqlParameter( "@FirstName",  SqlDbType.NVarChar ),
                    new SqlParameter( "@MiddleName",  SqlDbType.NVarChar ),
                    new SqlParameter( "@LastName",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CountryId",  SqlDbType.Int ),
                    new SqlParameter( "@StateId",  SqlDbType.Int ),
                    new SqlParameter( "@Gender",  SqlDbType.Int ),
                    new SqlParameter( "@Email",  SqlDbType.NVarChar ),
                    new SqlParameter( "@PortalLanguageId",  SqlDbType.Int ),
                   // new SqlParameter( "@DepartmentId",  SqlDbType.Int ),
                   // new SqlParameter( "@DesignationId",  SqlDbType.Int ),
                    new SqlParameter( "@DateOfBirth",  SqlDbType.DateTime ),
                    new SqlParameter( "@Photo",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ProfileDescription",  SqlDbType.NVarChar ),
                    new SqlParameter( "@SubjectsInterested",  SqlDbType.NVarChar ),
                    new SqlParameter( "@IsContributor",  SqlDbType.NVarChar ),
                    new SqlParameter( "@UserCertifications",  SqlDbType.NVarChar ),
                    new SqlParameter( "@UserEducations",  SqlDbType.NVarChar ),
                    new SqlParameter( "@UserExperiences",  SqlDbType.NVarChar ),
                    new SqlParameter( "@UserLanguages",  SqlDbType.NVarChar ),
                    new SqlParameter( "@UserSocialMedias",  SqlDbType.NVarChar ),
                     new SqlParameter( "@Id",  SqlDbType.Int ),
                };

                CommonHelper helper = new CommonHelper();

                parameters[0].Value = profileRequest.TitleId == null || profileRequest.TitleId == 0 ? null : profileRequest.TitleId;
                parameters[1].Value = profileRequest.FirstName;
                parameters[2].Value = profileRequest.MiddleName;
                parameters[3].Value = profileRequest.LastName;
                parameters[4].Value = profileRequest.CountryId == null || profileRequest.CountryId == 0 ? null : profileRequest.CountryId;
                parameters[5].Value = profileRequest.StateId == null || profileRequest.StateId == 0 ? null : profileRequest.StateId;
                parameters[6].Value = profileRequest.Gender;
                parameters[7].Value = profileRequest.Email;
                parameters[8].Value = profileRequest.PortalLanguageId == null || profileRequest.PortalLanguageId == 0 ? null : profileRequest.PortalLanguageId;
                parameters[9].Value = profileRequest.DateOfBirth;
                parameters[10].Value = profileRequest.Photo;
                parameters[11].Value = profileRequest.ProfileDescription;
                parameters[12].Value = profileRequest.SubjectsInterested;
                parameters[13].Value = profileRequest.IsContributor;
                parameters[14].Value = profileRequest.UserCertifications != null ? helper.GetJsonString(profileRequest.UserCertifications) : null;
                parameters[15].Value = profileRequest.UserEducations != null ? helper.GetJsonString(profileRequest.UserEducations) : null;
                parameters[16].Value = profileRequest.UserExperiences != null ? helper.GetJsonString(profileRequest.UserExperiences) : null;
                parameters[17].Value = profileRequest.UserLanguages != null ? helper.GetJsonString(profileRequest.UserLanguages) : null;
                parameters[18].Value = profileRequest.UserSocialMedias != null ? helper.GetJsonString(profileRequest.UserSocialMedias) : null;
                parameters[19].Value = profileRequest.Id;
                _DataHelper = new DataAccessHelper("UpdateUserProfile", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);                         

                return new DatabaseResponse { ResponseCode = result };
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
