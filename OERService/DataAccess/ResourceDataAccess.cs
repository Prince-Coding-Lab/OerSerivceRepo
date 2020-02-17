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
    public class ResourceDataAccess
    {
        internal DataAccessHelper _DataHelper = null;

        private IConfiguration _configuration;

        /// <summary>
        /// Constructor setting configuration
        /// </summary>
        /// <param name="configuration"></param>
        public ResourceDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<DatabaseResponse> CreateResource(CreateResourceRequest resource)
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
                    new SqlParameter( "@ResourceDescription",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Keywords",  SqlDbType.NVarChar ),                  
                    new SqlParameter( "@ResourceContent",  SqlDbType.NVarChar ),
                    new SqlParameter( "@MaterialTypeId",  SqlDbType.Int ),
                    new SqlParameter( "@CopyRightId",  SqlDbType.Int ),
                    new SqlParameter( "@IsDraft",  SqlDbType.Bit ),
                    new SqlParameter( "@CreatedBy",  SqlDbType.Int ),
                    new SqlParameter( "@References",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ResourceFiles",  SqlDbType.NVarChar ),
                }; 

                parameters[0].Value = resource.Title;
                parameters[1].Value = resource.CategoryId;
                parameters[2].Value = resource.SubCategoryId == null || resource.SubCategoryId == 0 ? null : resource.SubCategoryId; 
                parameters[3].Value = resource.Thumbnail;
                parameters[4].Value = resource.ResourceDescription;
                parameters[5].Value = resource.Keywords;
                parameters[6].Value = resource.ResourceContent;
                parameters[7].Value = resource.MaterialTypeId;
                parameters[8].Value = resource.CopyRightId == null || resource.CopyRightId == 0 ? null : resource.CopyRightId;
                parameters[9].Value = resource.IsDraft;
                parameters[10].Value = resource.CreatedBy;
                parameters[11].Value = resource.References != null ? helper.GetJsonString(resource.References) : null; 
                parameters[12].Value = resource.ResourceFiles != null ? helper.GetJsonString(resource.ResourceFiles) : null; 

               _DataHelper = new DataAccessHelper("CreateResource", parameters, _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);

                Resource Createdresource = new Resource();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {

                    Createdresource = (from model in ds.Tables[0].AsEnumerable()
                                  select new Resource()
                                  {
                                      Id = model.Field<decimal>("Id"),
                                      Title = model.Field<string>("Title"),
                                      Category = new ShortCategory { Id = model.Field<int>("CategoryId"), Name = model.Field<string>("CategoryName") },
                                      SubCategory = model.Field<int?>("SubCategoryId")!=null? new ShortSubCategory { Id= model.Field<int>("SubCategoryId"), CategoryId= model.Field<int>("CategoryId"), Name= model.Field<string>("SubCategoryName") }:null,
                                      CopyRight  = model.Field<int?>("CopyRightId")!=null? new ShortCopyright { Id= model.Field<int>("CopyRightId"), Title= model.Field<string>("CopyrightTitle"), Description= model.Field<string>("CopyrightDescription")  }:null,
                                      Thumbnail = model.Field<string>("Thumbnail"),
                                      ResourceContent = model.Field<string>("ResourceContent"),                                     
                                      MaterialType = new ShortMaterialType {Id= model.Field<int>("MaterialTypeId"), Name = model.Field<string>("MaterialTypeName")},
                                      ResourceDescription = model.Field<string>("ResourceDescription"),
                                      Keywords = model.Field<string>("Keywords"),
                                      CreatedBy = model.Field<string>("CreatedBy"),
                                      CreatedOn = model.Field<DateTime>("CreatedOn"), 
                                      IsDraft = model.Field<bool>("IsDraft"),
                                      Rating = model.Field<double>("Rating"),
                                      AlignmentRating = model.Field<double>("AlignmentRating"),
                                      IsApproved = model.Field<bool>("IsApproved"),
                                      ReportAbuseCount = model.Field<int>("ReportAbuseCount"),                                     
                                          
                                  }).FirstOrDefault();

                    
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {

                    Createdresource.ResourceFiles = (from model in ds.Tables[1].AsEnumerable()
                                           select new ResourceFileMaster()
                                           {
                                               Id = model.Field<decimal>("Id"),
                                               ResourceId = model.Field<decimal>("ResourceId"),
                                               AssociatedFile = model.Field<string>("AssociatedFile"),
                                               UploadedDate = model.Field<DateTime>("UploadedDate")
                                           }).ToList();
                }


                if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                {

                    Createdresource.References = (from model in ds.Tables[2].AsEnumerable()
                                           select new ReferenceMaster()
                                           {
                                               Id = model.Field<decimal>("Id"),
                                               ResourceId = model.Field<decimal>("ResourceId"),
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

        public async Task<DatabaseResponse> GetAllResource()
        {
            try
            {
                _DataHelper = new DataAccessHelper("GetResource", _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);

                List<Resource> resources = new List<Resource>();

                List<ResourceFileMaster> resourceFilesList = new List<ResourceFileMaster>();

                List<ReferenceMaster> references = new List<ReferenceMaster>();

                List<ResourceComments> comments = new List<ResourceComments>();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {                     

                        resourceFilesList = (from model in ds.Tables[1].AsEnumerable()
                                                         select new ResourceFileMaster()
                                                         {
                                                             Id = model.Field<decimal>("Id"),
                                                             ResourceId = model.Field<decimal>("ResourceId"),
                                                             AssociatedFile = model.Field<string>("AssociatedFile"),
                                                             UploadedDate = model.Field<DateTime>("UploadedDate")
                                                         }).ToList();
                    }
                  

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                    {

                        references = (from model in ds.Tables[2].AsEnumerable()
                                                      select new ReferenceMaster()
                                                      {
                                                          Id = model.Field<decimal>("Id"),
                                                          ResourceId = model.Field<decimal>("ResourceId"),
                                                          URLReference = model.Field<string>("URLReference"),
                                                          CreatedOn = model.Field<DateTime>("CreatedOn")
                                                      }).ToList();
                    }

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                    {

                        comments = (from model in ds.Tables[3].AsEnumerable()
                                      select new ResourceComments()
                                      {
                                          Id = model.Field<decimal>("Id"),
                                          ResourceId = model.Field<decimal>("ResourceId"),
                                          Comments = model.Field<string>("Comments"),
                                          CommentedBy = model.Field<string>("CommentedBy"),
                                          CommentDate = model.Field<DateTime>("CommentDate"),
                                          ReportAbuseCount = model.Field<int>("ReportAbuseCount"),
                                      }).ToList();
                    }


                    resources = (from model in ds.Tables[0].AsEnumerable()
                                       select new Resource()
                                       {
                                           Id = model.Field<decimal>("Id"),
                                           Title = model.Field<string>("Title"),
                                           Category = new ShortCategory { Id = model.Field<int>("CategoryId"), Name = model.Field<string>("CategoryName") },
                                           SubCategory = model.Field<int?>("SubCategoryId") != null ? new ShortSubCategory { Id = model.Field<int>("SubCategoryId"), CategoryId = model.Field<int>("CategoryId"), Name = model.Field<string>("SubCategoryName") } : null,
                                           CopyRight = model.Field<int?>("CopyRightId") != null ? new ShortCopyright { Id = model.Field<int>("CopyRightId"), Title = model.Field<string>("CopyrightTitle"), Description = model.Field<string>("CopyrightDescription") } : null,
                                           Thumbnail = model.Field<string>("Thumbnail"),
                                           ResourceContent = model.Field<string>("ResourceContent"),
                                           MaterialType = new ShortMaterialType { Id = model.Field<int>("MaterialTypeId"), Name = model.Field<string>("MaterialTypeName") },
                                           ResourceDescription = model.Field<string>("ResourceDescription"),
                                           Keywords = model.Field<string>("Keywords"),
                                           CreatedBy = model.Field<string>("CreatedBy"),
                                           CreatedOn = model.Field<DateTime>("CreatedOn"),
                                           IsDraft = model.Field<bool>("IsDraft"),
                                           Rating = model.Field<double>("Rating"),
                                           AlignmentRating = model.Field<double>("AlignmentRating"),
                                           IsApproved = model.Field<bool>("IsApproved"),
                                           ReportAbuseCount = model.Field<int>("ReportAbuseCount"),
                                           References = references!=null && references.Count>0? references.Where(r=>r.ResourceId == model.Field<decimal>("Id")).ToList():null,
                                           ResourceFiles= resourceFilesList != null && resourceFilesList.Count>0? resourceFilesList.Where(f=>f.ResourceId== model.Field<decimal>("Id")).ToList():null,
                                           ResourceComments= comments != null && comments.Count > 0 ? comments.Where(c => c.ResourceId == model.Field<decimal>("Id")).ToList() : null
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

        public async Task<DatabaseResponse> GetResource(decimal id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.NVarChar )

                };

                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("GetResourceById", parameters, _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);

                List<Resource> resources = new List<Resource>();

                List<ResourceFileMaster> resourceFilesList = new List<ResourceFileMaster>();

                List<ReferenceMaster> references = new List<ReferenceMaster>();

                List<ResourceComments> comments = new List<ResourceComments>();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {


                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {


                        resourceFilesList = (from model in ds.Tables[1].AsEnumerable()
                                             select new ResourceFileMaster()
                                             {
                                                 Id = model.Field<decimal>("Id"),
                                                 ResourceId = model.Field<decimal>("ResourceId"),
                                                 AssociatedFile = model.Field<string>("AssociatedFile"),
                                                 UploadedDate = model.Field<DateTime>("UploadedDate")
                                             }).ToList();
                    }



                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                    {

                        references = (from model in ds.Tables[2].AsEnumerable()
                                      select new ReferenceMaster()
                                      {
                                          Id = model.Field<decimal>("Id"),
                                          ResourceId = model.Field<decimal>("ResourceId"),
                                          URLReference = model.Field<string>("URLReference"),
                                          CreatedOn = model.Field<DateTime>("CreatedOn")
                                      }).ToList();
                    }

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                    {

                        comments = (from model in ds.Tables[3].AsEnumerable()
                                    select new ResourceComments()
                                    {
                                        Id = model.Field<decimal>("Id"),
                                        ResourceId = model.Field<decimal>("ResourceId"),
                                        Comments = model.Field<string>("Comments"),
                                        CommentedBy = model.Field<string>("CommentedBy"),
                                        CommentDate = model.Field<DateTime>("CommentDate"),
                                        ReportAbuseCount = model.Field<int>("ReportAbuseCount"),
                                    }).ToList();
                    }

                    resources = (from model in ds.Tables[0].AsEnumerable()
                                 select new Resource()
                                 {
                                     Id = model.Field<decimal>("Id"),
                                     Title = model.Field<string>("Title"),
                                     Category = new ShortCategory { Id = model.Field<int>("CategoryId"), Name = model.Field<string>("CategoryName") },
                                     SubCategory = model.Field<int?>("SubCategoryId") != null ? new ShortSubCategory { Id = model.Field<int>("SubCategoryId"), CategoryId = model.Field<int>("CategoryId"), Name = model.Field<string>("SubCategoryName") } : null,
                                     CopyRight = model.Field<int?>("CopyRightId") != null ? new ShortCopyright { Id = model.Field<int>("CopyRightId"), Title = model.Field<string>("CopyrightTitle"), Description = model.Field<string>("CopyrightDescription") } : null,
                                     Thumbnail = model.Field<string>("Thumbnail"),
                                     ResourceContent = model.Field<string>("ResourceContent"),
                                     MaterialType = new ShortMaterialType { Id = model.Field<int>("MaterialTypeId"), Name = model.Field<string>("MaterialTypeName") },
                                     ResourceDescription = model.Field<string>("ResourceDescription"),
                                     Keywords = model.Field<string>("Keywords"),
                                     CreatedBy = model.Field<string>("CreatedBy"),
                                     CreatedOn = model.Field<DateTime>("CreatedOn"),
                                     IsDraft = model.Field<bool>("IsDraft"),
                                     Rating = model.Field<double>("Rating"),
                                     AlignmentRating = model.Field<double>("AlignmentRating"),
                                     IsApproved = model.Field<bool>("IsApproved"),
                                     ReportAbuseCount = model.Field<int>("ReportAbuseCount"),
                                     References = references != null && references.Count > 0 ? references.Where(r => r.ResourceId == model.Field<decimal>("Id")).ToList() : null,
                                     ResourceFiles = resourceFilesList != null && resourceFilesList.Count > 0 ? resourceFilesList.Where(f => f.ResourceId == model.Field<decimal>("Id")).ToList() : null,
                                     ResourceComments = comments != null && comments.Count > 0 ? comments.Where(c => c.ResourceId == model.Field<decimal>("Id")).ToList() : null
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

        public async Task<DatabaseResponse> DeleteResource(int id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.Decimal )

                };

                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("DeleteResource", parameters, _configuration);              

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

        public async Task<DatabaseResponse> UpdateResource(UpdateResourceRequest resource, int id)
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
                    new SqlParameter( "@ResourceDescription",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Keywords",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ResourceContent",  SqlDbType.NVarChar ),
                    new SqlParameter( "@MaterialTypeId",  SqlDbType.Int ),
                    new SqlParameter( "@CopyRightId",  SqlDbType.Int ),
                    new SqlParameter( "@IsDraft",  SqlDbType.Bit ),                 
                    new SqlParameter( "@References",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ResourceFiles",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Id",  SqlDbType.Decimal ),

                };

                parameters[0].Value = resource.Title;
                parameters[1].Value = resource.CategoryId;
                parameters[2].Value = resource.SubCategoryId == null || resource.SubCategoryId == 0 ? null : resource.SubCategoryId;
                parameters[3].Value = resource.Thumbnail;
                parameters[4].Value = resource.ResourceDescription;
                parameters[5].Value = resource.Keywords;
                parameters[6].Value = resource.ResourceContent;
                parameters[7].Value = resource.MaterialTypeId;
                parameters[8].Value = resource.CopyRightId == null || resource.CopyRightId == 0 ? null : resource.CopyRightId;
                parameters[9].Value = resource.IsDraft;               
                parameters[10].Value = resource.References != null ? helper.GetJsonString(resource.References) : null;
                parameters[11].Value = resource.ResourceFiles != null ? helper.GetJsonString(resource.ResourceFiles) : null;
                parameters[12].Value = resource.Id;

                _DataHelper = new DataAccessHelper("UpdateResource", parameters, _configuration);

                DataSet ds = new DataSet();

                int result = await _DataHelper.RunAsync(ds);

                List<Resource> resources = new List<Resource>();

                List<ResourceFileMaster> resourceFilesList = new List<ResourceFileMaster>();

                List<ReferenceMaster> references = new List<ReferenceMaster>();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {


                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {


                        resourceFilesList = (from model in ds.Tables[1].AsEnumerable()
                                             select new ResourceFileMaster()
                                             {
                                                 Id = model.Field<decimal>("Id"),
                                                 ResourceId = model.Field<decimal>("ResourceId"),
                                                 AssociatedFile = model.Field<string>("AssociatedFile"),
                                                 UploadedDate = model.Field<DateTime>("UploadedDate")
                                             }).ToList();
                    }



                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                    {

                        references = (from model in ds.Tables[2].AsEnumerable()
                                      select new ReferenceMaster()
                                      {
                                          Id = model.Field<decimal>("Id"),
                                          ResourceId = model.Field<decimal>("ResourceId"),
                                          URLReference = model.Field<string>("URLReference"),
                                          CreatedOn = model.Field<DateTime>("CreatedOn")
                                      }).ToList();
                    }

                    resources = (from model in ds.Tables[0].AsEnumerable()
                                 select new Resource()
                                 {
                                     Id = model.Field<decimal>("Id"),
                                     Title = model.Field<string>("Title"),
                                     Category = new ShortCategory { Id = model.Field<int>("CategoryId"), Name = model.Field<string>("CategoryName") },
                                     SubCategory = model.Field<int?>("SubCategoryId") != null ? new ShortSubCategory { Id = model.Field<int>("SubCategoryId"), CategoryId = model.Field<int>("CategoryId"), Name = model.Field<string>("SubCategoryName") } : null,
                                     CopyRight = model.Field<int?>("CopyRightId") != null ? new ShortCopyright { Id = model.Field<int>("CopyRightId"), Title = model.Field<string>("CopyrightTitle"), Description = model.Field<string>("CopyrightDescription") } : null,
                                     Thumbnail = model.Field<string>("Thumbnail"),
                                     ResourceContent = model.Field<string>("ResourceContent"),
                                     MaterialType = new ShortMaterialType { Id = model.Field<int>("MaterialTypeId"), Name = model.Field<string>("MaterialTypeName") },
                                     ResourceDescription = model.Field<string>("ResourceDescription"),
                                     Keywords = model.Field<string>("Keywords"),
                                     CreatedBy = model.Field<string>("CreatedBy"),
                                     CreatedOn = model.Field<DateTime>("CreatedOn"),
                                     IsDraft = model.Field<bool>("IsDraft"),
                                     Rating = model.Field<double>("Rating"),
                                     AlignmentRating = model.Field<double>("AlignmentRating"),
                                     IsApproved = model.Field<bool>("IsApproved"),
                                     ReportAbuseCount = model.Field<int>("ReportAbuseCount"),
                                     References = references != null && references.Count > 0 ? references.Where(r => r.ResourceId == model.Field<decimal>("Id")).ToList() : null,
                                     ResourceFiles = resourceFilesList != null && resourceFilesList.Count > 0 ? resourceFilesList.Where(f => f.ResourceId == model.Field<decimal>("Id")).ToList() : null

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

        public async Task<DatabaseResponse> ApproveResource(decimal resourceId, int approvedBy)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@ResourceId",  SqlDbType.Decimal),
                    new SqlParameter( "@ApprovedBy",  SqlDbType.Int )
                };

                parameters[0].Value = resourceId;
                parameters[1].Value = approvedBy;

                _DataHelper = new DataAccessHelper("ApproveResource", parameters, _configuration);

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

        public async Task<DatabaseResponse> ResportResource(decimal id)
        {
            try
            {
                CommonHelper helper = new CommonHelper();

                SqlParameter[] parameters =
                {                    
                    new SqlParameter( "@Id",  SqlDbType.Decimal )

                };
               
                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("ReportResource", parameters, _configuration);

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

        public async Task<DatabaseResponse> CommentOnResource(ResourceCommentRequest resourceComment)
        {
            try
            {
                CommonHelper helper = new CommonHelper();

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@ResourceId",  SqlDbType.Decimal ),
                    new SqlParameter( "@Comments",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CommentedBy",  SqlDbType.Int )
                };

                parameters[0].Value = resourceComment.ResourceId;
                parameters[1].Value = resourceComment.Comments;
                parameters[2].Value = resourceComment.UserId;


                _DataHelper = new DataAccessHelper("CommentOnResource", parameters, _configuration);
                             
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

        public async Task<DatabaseResponse> ResportResourceComment(decimal id)
        {
            try
            {
                CommonHelper helper = new CommonHelper();

                SqlParameter[] parameters =
                {
                    new SqlParameter( "@Id",  SqlDbType.Decimal )

                };

                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("ReportResourceComment", parameters, _configuration);

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

        public async Task<DatabaseResponse> UpdateResourceComment(ResourceCommentUpdateRequest resourceUpdate)
        {
            try
            {
                CommonHelper helper = new CommonHelper();

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.Decimal ),
                    new SqlParameter( "@ResourceId",  SqlDbType.Decimal ),
                    new SqlParameter( "@Comments",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CommentedBy",  SqlDbType.Int )
                };

                parameters[0].Value = resourceUpdate.Id;
                parameters[1].Value = resourceUpdate.ResourceId;
                parameters[2].Value = resourceUpdate.Comments;
                parameters[3].Value = resourceUpdate.UserId;

                _DataHelper = new DataAccessHelper("UpdateResourceComment", parameters, _configuration);

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

        public async Task<DatabaseResponse> DeleteResourceComment(decimal id, int requestedBy)
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

                _DataHelper = new DataAccessHelper("DeleteResourceComment", parameters, _configuration);               

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

        public async Task<DatabaseResponse> HideResourceCommentByAuthor(decimal id, decimal resourceId, int requestedBy)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.Decimal ),
                    new SqlParameter( "@ResourceId",  SqlDbType.Decimal ),
                    new SqlParameter( "@RequestedBy",  SqlDbType.Int )
                };

                parameters[0].Value = id;
                parameters[1].Value = resourceId;
                parameters[2].Value = requestedBy;

                _DataHelper = new DataAccessHelper("HideResourceCommentByAuthor", parameters, _configuration);

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
               
        public async Task<DatabaseResponse> ReportResourceWithComment(ResourceReportAbuseWithComment resourceAbuseComment)
        {
            try
            {
                CommonHelper helper = new CommonHelper();

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@ResourceId",  SqlDbType.Decimal ),
                    new SqlParameter( "@ReportReasons",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Comments",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ReportedBy",  SqlDbType.Int )
                };

                parameters[0].Value = resourceAbuseComment.ResourceId;
                parameters[1].Value = resourceAbuseComment.ReportReasons;
                parameters[2].Value = resourceAbuseComment.Comments;
                parameters[3].Value = resourceAbuseComment.ReportedBy;

                _DataHelper = new DataAccessHelper("ReportResourceWithComment", parameters, _configuration);

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

        public async Task<DatabaseResponse> RateResource(ResourceRatingRequest resourceRatingRequest)
        {
            try
            {
                CommonHelper helper = new CommonHelper();

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@ResourceId",  SqlDbType.Decimal ),
                    new SqlParameter( "@Rating",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Comments",  SqlDbType.NVarChar ),
                    new SqlParameter( "@RatedBy",  SqlDbType.Int )
                };

                parameters[0].Value = resourceRatingRequest.ResourceId;
                parameters[1].Value = resourceRatingRequest.Rating;
                parameters[2].Value = resourceRatingRequest.Comments;
                parameters[3].Value = resourceRatingRequest.RatedBy;

                _DataHelper = new DataAccessHelper("RateResource", parameters, _configuration);

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
