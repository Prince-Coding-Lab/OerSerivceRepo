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
    public class CourseDataAccess
    {
        internal DataAccessHelper _DataHelper = null;

        private IConfiguration _configuration;

        /// <summary>
        /// Constructor setting configuration
        /// </summary>
        /// <param name="configuration"></param>
        public CourseDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<DatabaseResponse> CreateCourse(CreateCourseRequest course)
        {
            try
            {
                CommonHelper helper = new CommonHelper();

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Title",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CategoryId",  SqlDbType.Int ),
                    new SqlParameter( "@SubCategoryId",  SqlDbType.Int ),
                    new SqlParameter( "@Thumbnail",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CourseDescription",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Keywords",  SqlDbType.NVarChar ),                  
                    new SqlParameter( "@CourseContent",  SqlDbType.NVarChar ),                   
                    new SqlParameter( "@CopyRightId",  SqlDbType.Int ),
                    new SqlParameter( "@IsDraft",  SqlDbType.Bit ),
                    new SqlParameter( "@CreatedBy",  SqlDbType.Int ),
                    new SqlParameter( "@EducationId",  SqlDbType.Int ),
                    new SqlParameter( "@ProfessionId",  SqlDbType.Int ),                  
                    new SqlParameter( "@References",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CourseFiles",  SqlDbType.NVarChar ),
                }; 

                parameters[0].Value = course.Title;
                parameters[1].Value = course.CategoryId;
                parameters[2].Value = course.SubCategoryId == null || course.SubCategoryId == 0 ? null : course.SubCategoryId; 
                parameters[3].Value = course.Thumbnail;
                parameters[4].Value = course.CourseDescription;
                parameters[5].Value = course.Keywords;
                parameters[6].Value = course.CourseContent;               
                parameters[7].Value = course.CopyRightId == null || course.CopyRightId == 0 ? null : course.CopyRightId;
                parameters[8].Value = course.IsDraft;
                parameters[9].Value = course.CreatedBy;
                parameters[10].Value = course.EducationId;
                parameters[11].Value = course.ProfessionId;
                parameters[12].Value = course.References != null ? helper.GetJsonString(course.References) : null; 
                parameters[13].Value = course.ResourceFiles != null ? helper.GetJsonString(course.ResourceFiles) : null;                

                _DataHelper = new DataAccessHelper("CreateCourse", parameters, _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);

                Course Createdresource = new Course();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {

                    Createdresource = (from model in ds.Tables[0].AsEnumerable()
                                  select new Course()
                                  {
                                      Id = model.Field<decimal>("Id"),
                                      Title = model.Field<string>("Title"),
                                      Category = new ShortCategory { Id = model.Field<int>("CategoryId"), Name = model.Field<string>("CategoryName") },
                                      SubCategory = model.Field<int?>("SubCategoryId") != null ? new ShortSubCategory { Id = model.Field<int>("SubCategoryId"), CategoryId = model.Field<int>("CategoryId"), Name = model.Field<string>("SubCategoryName") } : null,
                                      CopyRight  = model.Field<int?>("CopyRightId") !=null? new ShortCopyright { Id= model.Field<int>("CopyRightId"), Title= model.Field<string>("CopyrightTitle"), Description= model.Field<string>("CopyrightDescription")  }:null,
                                      Thumbnail = model.Field<string>("Thumbnail"),
                                      CourseContent = model.Field<string>("CourseContent"),
                                      CourseDescription = model.Field<string>("CourseDescription"),
                                      Keywords = model.Field<string>("Keywords"),
                                      CreatedBy = model.Field<string>("CreatedBy"),
                                      CreatedOn = model.Field<DateTime>("CreatedOn"), 
                                      IsDraft = model.Field<bool>("IsDraft"),
                                      Rating = model.Field<double>("Rating"),                                      
                                      IsApproved = model.Field<bool>("IsApproved"),
                                      ReportAbuseCount = model.Field<int>("ReportAbuseCount"),
                                      Education = model.Field<int?>("EducationId") != null ? new Education { Id = model.Field<int>("EducationId"), Name = model.Field<string>("EducationName") } : null,
                                      Profession= model.Field<int?>("ProfessionId") != null ? new Profession { Id = model.Field<int>("ProfessionId"), Name = model.Field<string>("ProfessionName") } : null

                                  }).FirstOrDefault();

                    
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {

                    Createdresource.AssociatedFiles = (from model in ds.Tables[1].AsEnumerable()
                                           select new CourseAssociatedFiles()
                                           {
                                               Id = model.Field<decimal>("Id"),
                                               CourseId = model.Field<decimal>("CourseId"),
                                               AssociatedFile = model.Field<string>("AssociatedFile"),
                                               CreatedOn = model.Field<DateTime>("CreatedOn")
                                           }).ToList();
                }


                if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                {

                    Createdresource.References = (from model in ds.Tables[2].AsEnumerable()
                                           select new CourseURLReferences()
                                           {
                                               Id = model.Field<decimal>("Id"),
                                               CourseId = model.Field<decimal>("CourseId"),
                                               URLReference = model.Field<string>("URLReference"),
                                               CreatedOn = model.Field<DateTime>("CreatedOn")                                               
                                           }).ToList();
                }

               
           

             return new DatabaseResponse { ResponseCode = result, Results = Createdresource };

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

        public async Task<DatabaseResponse> GetAllCourse()
        {
            try
            {
                _DataHelper = new DataAccessHelper("GetCourses", _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);

                List<Course> resources = new List<Course>();

                List<CourseAssociatedFiles> resourceFilesList = new List<CourseAssociatedFiles>();

                List<CourseURLReferences> references = new List<CourseURLReferences>();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {


                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                      

                        resourceFilesList = (from model in ds.Tables[1].AsEnumerable()
                                                         select new CourseAssociatedFiles()
                                                         {
                                                             Id = model.Field<decimal>("Id"),
                                                             CourseId = model.Field<decimal>("CourseId"),
                                                             AssociatedFile = model.Field<string>("AssociatedFile"),
                                                              CreatedOn = model.Field<DateTime>("CreatedOn")
                                                         }).ToList();
                    }

                  

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                    {

                        references = (from model in ds.Tables[2].AsEnumerable()
                                                      select new CourseURLReferences()
                                                      {
                                                          Id = model.Field<decimal>("Id"),
                                                          CourseId = model.Field<decimal>("CourseId"),
                                                          URLReference = model.Field<string>("URLReference"),
                                                          CreatedOn = model.Field<DateTime>("CreatedOn")
                                                      }).ToList();
                    }

                    resources = (from model in ds.Tables[0].AsEnumerable()
                                       select new Course()
                                       {
                                           Id = model.Field<decimal>("Id"),
                                           Title = model.Field<string>("Title"),
                                           Category = new ShortCategory { Id = model.Field<int>("CategoryId"), Name = model.Field<string>("CategoryName") },
                                           SubCategory = model.Field<int?>("SubCategoryId") != null ? new ShortSubCategory { Id = model.Field<int>("SubCategoryId"), CategoryId = model.Field<int>("CategoryId"), Name = model.Field<string>("SubCategoryName") } : null,
                                           CopyRight = model.Field<int?>("CopyRightId") != null ? new ShortCopyright { Id = model.Field<int>("CopyRightId"), Title = model.Field<string>("CopyrightTitle"), Description = model.Field<string>("CopyrightDescription") } : null,
                                           Thumbnail = model.Field<string>("Thumbnail"),
                                           CourseContent = model.Field<string>("CourseContent"),
                                           CourseDescription = model.Field<string>("CourseDescription"),
                                           Keywords = model.Field<string>("Keywords"),
                                           CreatedBy = model.Field<string>("CreatedBy"),
                                           CreatedOn = model.Field<DateTime>("CreatedOn"),
                                           IsDraft = model.Field<bool>("IsDraft"),
                                           Rating = model.Field<double>("Rating"),
                                           IsApproved = model.Field<bool>("IsApproved"),
                                           ReportAbuseCount = model.Field<int>("ReportAbuseCount"),
                                           Education = model.Field<int?>("EducationId") != null ? new Education { Id = model.Field<int>("EducationId"), Name = model.Field<string>("EducationName") } : null,
                                           Profession = model.Field<int?>("ProfessionId") != null ? new Profession { Id = model.Field<int>("ProfessionId"), Name = model.Field<string>("ProfessionName") } : null,
                                           References = references!=null && references.Count>0? references.Where(r=>r.CourseId == model.Field<decimal>("Id")).ToList():null,
                                           AssociatedFiles = resourceFilesList != null && resourceFilesList.Count>0? resourceFilesList.Where(f=>f.CourseId== model.Field<decimal>("Id")).ToList():null

                                       }).ToList();


                }


                return new DatabaseResponse { ResponseCode = result, Results = resources };
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

        public async Task<DatabaseResponse> GetCourse(decimal id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.NVarChar )

                };

                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("GetCourseById", parameters, _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);

                Course Createdresource = new Course();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {

                    Createdresource = (from model in ds.Tables[0].AsEnumerable()
                                       select new Course()
                                       {
                                           Id = model.Field<decimal>("Id"),
                                           Title = model.Field<string>("Title"),
                                           Category = new ShortCategory { Id = model.Field<int>("CategoryId"), Name = model.Field<string>("CategoryName") },
                                           SubCategory = model.Field<int?>("SubCategoryId") != null ? new ShortSubCategory { Id = model.Field<int>("SubCategoryId"), CategoryId = model.Field<int>("CategoryId"), Name = model.Field<string>("SubCategoryName") } : null,
                                           CopyRight = model.Field<int?>("CopyRightId") != null ? new ShortCopyright { Id = model.Field<int>("CopyRightId"), Title = model.Field<string>("CopyrightTitle"), Description = model.Field<string>("CopyrightDescription") } : null,
                                           Thumbnail = model.Field<string>("Thumbnail"),
                                           CourseContent = model.Field<string>("CourseContent"),
                                           CourseDescription = model.Field<string>("CourseDescription"),
                                           Keywords = model.Field<string>("Keywords"),
                                           CreatedBy = model.Field<string>("CreatedBy"),
                                           CreatedOn = model.Field<DateTime>("CreatedOn"),
                                           IsDraft = model.Field<bool>("IsDraft"),
                                           Rating = model.Field<double>("Rating"),
                                           IsApproved = model.Field<bool>("IsApproved"),
                                           ReportAbuseCount = model.Field<int>("ReportAbuseCount"),
                                           Education = model.Field<int?>("EducationId") != null ? new Education { Id = model.Field<int>("EducationId"), Name = model.Field<string>("EducationName") } : null,
                                           Profession = model.Field<int?>("ProfessionId") != null ? new Profession { Id = model.Field<int>("ProfessionId"), Name = model.Field<string>("ProfessionName") } : null

                                       }).FirstOrDefault();


                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {

                    Createdresource.AssociatedFiles = (from model in ds.Tables[1].AsEnumerable()
                                                       select new CourseAssociatedFiles()
                                                       {
                                                           Id = model.Field<decimal>("Id"),
                                                           CourseId = model.Field<decimal>("CourseId"),
                                                           AssociatedFile = model.Field<string>("AssociatedFile"),
                                                           CreatedOn = model.Field<DateTime>("CreatedOn")
                                                       }).ToList();
                }


                if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                {

                    Createdresource.References = (from model in ds.Tables[2].AsEnumerable()
                                                  select new CourseURLReferences()
                                                  {
                                                      Id = model.Field<decimal>("Id"),
                                                      CourseId = model.Field<decimal>("CourseId"),
                                                      URLReference = model.Field<string>("URLReference"),
                                                      CreatedOn = model.Field<DateTime>("CreatedOn")
                                                  }).ToList();
                }




                return new DatabaseResponse { ResponseCode = result, Results = Createdresource };
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

        public async Task<DatabaseResponse> DeleteCourse(decimal id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.NVarChar )

                };

                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("DeleteCourse", parameters, _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);   

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

        public async Task<DatabaseResponse> UpdateCourse(UpdateCourseRequest course, int id)
        {
            try
            {
                CommonHelper helper = new CommonHelper();

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Title",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CategoryId",  SqlDbType.Int ),
                    new SqlParameter( "@SubCategoryId",  SqlDbType.Int ),
                    new SqlParameter( "@Thumbnail",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CourseDescription",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Keywords",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CourseContent",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CopyRightId",  SqlDbType.Int ),
                    new SqlParameter( "@IsDraft",  SqlDbType.Bit ),                   
                    new SqlParameter( "@EducationId",  SqlDbType.Int ),
                    new SqlParameter( "@ProfessionId",  SqlDbType.Int ),
                    new SqlParameter( "@References",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CourseFiles",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Id",  SqlDbType.Int )
                };

                parameters[0].Value = course.Title;
                parameters[1].Value = course.CategoryId;
                parameters[2].Value = course.SubCategoryId == null || course.SubCategoryId == 0 ? null : course.SubCategoryId;
                parameters[3].Value = course.Thumbnail;
                parameters[4].Value = course.CourseDescription;
                parameters[5].Value = course.Keywords;
                parameters[6].Value = course.CourseContent;
                parameters[7].Value = course.CopyRightId == null || course.CopyRightId == 0 ? null : course.CopyRightId;
                parameters[8].Value = course.IsDraft;               
                parameters[9].Value = course.EducationId;
                parameters[10].Value = course.ProfessionId;
                parameters[11].Value = course.References != null ? helper.GetJsonString(course.References) : null;
                parameters[12].Value = course.ResourceFiles != null ? helper.GetJsonString(course.ResourceFiles) : null;
                parameters[13].Value = course.Id;

                _DataHelper = new DataAccessHelper("UpdateCourse", parameters, _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);

                Course updatedCesource = new Course();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {

                    updatedCesource = (from model in ds.Tables[0].AsEnumerable()
                                       select new Course()
                                       {
                                           Id = model.Field<decimal>("Id"),
                                           Title = model.Field<string>("Title"),
                                           Category = new ShortCategory { Id = model.Field<int>("CategoryId"), Name = model.Field<string>("CategoryName") },
                                           SubCategory = model.Field<int?>("SubCategoryId") != null ? new ShortSubCategory { Id = model.Field<int>("SubCategoryId"), CategoryId = model.Field<int>("CategoryId"), Name = model.Field<string>("SubCategoryName") } : null,
                                           CopyRight = model.Field<int?>("CopyRightId") != null ? new ShortCopyright { Id = model.Field<int>("CopyRightId"), Title = model.Field<string>("CopyrightTitle"), Description = model.Field<string>("CopyrightDescription") } : null,
                                           Thumbnail = model.Field<string>("Thumbnail"),
                                           CourseContent = model.Field<string>("CourseContent"),
                                           CourseDescription = model.Field<string>("CourseDescription"),
                                           Keywords = model.Field<string>("Keywords"),
                                           CreatedBy = model.Field<string>("CreatedBy"),
                                           CreatedOn = model.Field<DateTime>("CreatedOn"),
                                           IsDraft = model.Field<bool>("IsDraft"),
                                           Rating = model.Field<double>("Rating"),
                                           IsApproved = model.Field<bool>("IsApproved"),
                                           ReportAbuseCount = model.Field<int>("ReportAbuseCount"),
                                           Education = model.Field<int?>("EducationId") != null ? new Education { Id = model.Field<int>("EducationId"), Name = model.Field<string>("EducationName") } : null,
                                           Profession = model.Field<int?>("ProfessionId") != null ? new Profession { Id = model.Field<int>("ProfessionId"), Name = model.Field<string>("ProfessionName") } : null

                                       }).FirstOrDefault();


                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {

                    updatedCesource.AssociatedFiles = (from model in ds.Tables[1].AsEnumerable()
                                                       select new CourseAssociatedFiles()
                                                       {
                                                           Id = model.Field<decimal>("Id"),
                                                           CourseId = model.Field<decimal>("CourseId"),
                                                           AssociatedFile = model.Field<string>("AssociatedFile"),
                                                           CreatedOn = model.Field<DateTime>("CreatedOn")
                                                       }).ToList();
                }


                if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                {

                    updatedCesource.References = (from model in ds.Tables[2].AsEnumerable()
                                                  select new CourseURLReferences()
                                                  {
                                                      Id = model.Field<decimal>("Id"),
                                                      CourseId = model.Field<decimal>("CourseId"),
                                                      URLReference = model.Field<string>("URLReference"),
                                                      CreatedOn = model.Field<DateTime>("CreatedOn")
                                                  }).ToList();
                }




                return new DatabaseResponse { ResponseCode = result, Results = updatedCesource };
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

        public async Task<DatabaseResponse> ApproveCourse(decimal courseId, int approvedBy)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@CourseId",  SqlDbType.Decimal),
                    new SqlParameter( "@ApprovedBy",  SqlDbType.Int )
                };

                parameters[0].Value = courseId;

                parameters[1].Value = approvedBy;

                _DataHelper = new DataAccessHelper("ApproveCourse", parameters, _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds); // 115/116

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

        public async Task<DatabaseResponse> ResportCourse(decimal id)
        {
            try
            {
                CommonHelper helper = new CommonHelper();

                SqlParameter[] parameters =
                {
                    new SqlParameter( "@Id",  SqlDbType.Decimal )

                };

                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("ReportCourse", parameters, _configuration);

                int result = await _DataHelper.RunAsync();

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

        public async Task<DatabaseResponse> CommentOnCourse(CourseCommentRequest courseComment)
        {
            try
            {
                CommonHelper helper = new CommonHelper();

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@CourseId",  SqlDbType.Int ),
                    new SqlParameter( "@Comments",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CommentedBy",  SqlDbType.Int )                   
                };

                parameters[0].Value = courseComment.CourseId;
                parameters[1].Value = courseComment.Comments;
                parameters[2].Value = courseComment.UserId;
               

                _DataHelper = new DataAccessHelper("CommentOnCourse", parameters, _configuration);              

                int result = await _DataHelper.RunAsync();              

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

        public async Task<DatabaseResponse> ResportCourseComment(decimal id)
        {
            try
            {
                CommonHelper helper = new CommonHelper();

                SqlParameter[] parameters =
                {
                    new SqlParameter( "@Id",  SqlDbType.Decimal )

                };

                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("ReportCourseComment", parameters, _configuration);

                int result = await _DataHelper.RunAsync();

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

        public async Task<DatabaseResponse> UpdateCourseComment(CourseCommentUpdateRequest courseUpdate)
        {
            try
            {
                CommonHelper helper = new CommonHelper();

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.Decimal ),
                    new SqlParameter( "@CourseId",  SqlDbType.Decimal ),
                    new SqlParameter( "@Comments",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CommentedBy",  SqlDbType.Int )
                };

                parameters[0].Value = courseUpdate.Id;
                parameters[1].Value = courseUpdate.CourseId;
                parameters[2].Value = courseUpdate.Comments;
                parameters[3].Value = courseUpdate.UserId;

                _DataHelper = new DataAccessHelper("UpdateCourseComment", parameters, _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);

                Course Createdresource = new Course();

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

        public async Task<DatabaseResponse> DeleteCourseComment(decimal id, int requestedBy)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.Decimal ),
                    new SqlParameter( "@RequestedBy",  SqlDbType.Int )

                };

                parameters[0].Value = id;

                parameters[1].Value = requestedBy;

                _DataHelper = new DataAccessHelper("DeleteCourseComment", parameters, _configuration);

                int result = await _DataHelper.RunAsync(); //103/102/114

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

        public async Task<DatabaseResponse> HideCourseCommentByAuthor(decimal id, decimal courseId, int requestedBy)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.Decimal ),
                    new SqlParameter( "@CourseId",  SqlDbType.Decimal ),
                    new SqlParameter( "@RequestedBy",  SqlDbType.Int )
                };

                parameters[0].Value = id;
                parameters[1].Value = courseId;
                parameters[2].Value = requestedBy;

                _DataHelper = new DataAccessHelper("HideCourseCommentByAuthor", parameters, _configuration);

                int result = await _DataHelper.RunAsync(); //101/106/117

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

        public async Task<DatabaseResponse> ReportCourseWithComment(CourseReportAbuseWithComment courseAbuseComment)
        {
            try
            {
                CommonHelper helper = new CommonHelper();

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@CourseId",  SqlDbType.Decimal ),
                    new SqlParameter( "@ReportReasons",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Comments",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ReportedBy",  SqlDbType.Int )
                };

                parameters[0].Value = courseAbuseComment.CourseId;
                parameters[1].Value = courseAbuseComment.ReportReasons;
                parameters[2].Value = courseAbuseComment.Comments;
                parameters[3].Value = courseAbuseComment.ReportedBy;

                _DataHelper = new DataAccessHelper("ReportCourseWithComment", parameters, _configuration);

                int result = await _DataHelper.RunAsync();

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

        public async Task<DatabaseResponse> RateCourse(CourseRatingRequest resourceRatingRequest)
        {
            try
            {
                CommonHelper helper = new CommonHelper();

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@CourseId",  SqlDbType.Decimal ),
                    new SqlParameter( "@Rating",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Comments",  SqlDbType.NVarChar ),
                    new SqlParameter( "@RatedBy",  SqlDbType.Int )
                };

                parameters[0].Value = resourceRatingRequest.CourseId;
                parameters[1].Value = resourceRatingRequest.Rating;
                parameters[2].Value = resourceRatingRequest.Comments;
                parameters[3].Value = resourceRatingRequest.RatedBy;

                _DataHelper = new DataAccessHelper("RateCourse", parameters, _configuration);

                int result = await _DataHelper.RunAsync();

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
