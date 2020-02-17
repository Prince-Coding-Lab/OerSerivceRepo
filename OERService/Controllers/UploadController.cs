using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.Http.Headers;


using System.Drawing;

namespace AspCoreServer.Controllers
{
    [Produces("application/json")]
    [Route("api/Upload")]
    public class UploadController : Controller
    {
    //    IConfiguration _iconfiguration;
    //    private IHostingEnvironment _hostingEnvironment;
    //    AdHelper helper = new AdHelper();

    //    OnBoardingHelper onboardHelper = new OnBoardingHelper();
    //    ImageHelper imgHelper = new ImageHelper();
    //    public UploadController(IHostingEnvironment hostingEnvironment)
    //    {
    //        _hostingEnvironment = hostingEnvironment;
    //    }

    //[HttpPost, DisableRequestSizeLimit]
    //public ActionResult UploadFile([FromForm] PostAd ad)
    //{
    //  try
    //  {
    //    BonzerContext context = HttpContext.RequestServices.GetService(typeof(BonzerContext)) as BonzerContext;
    //    var files = Request.Form.Files;
    //    // get formdata
    //    var model = Request.Form["adDetails"];
    //    var userModel = Request.Form["adUser"];

    //    //serialize formdata
    //    User adUser = JsonConvert.DeserializeObject<User>(userModel, new IsoDateTimeConverter());
    //    PostAd adDetails = JsonConvert.DeserializeObject<PostAd>(model, new IsoDateTimeConverter());
    //    adDetails.AdUser = adUser;

    //    if (adDetails == null || adDetails.AdUser == null)
    //    {
    //      return Json(new OperationResponse
    //      {
    //        HasSucceeded = false,
    //        Message = "Validation Error",
    //        IsDomainValidationErrors = true,
    //        StatusCode = "200"
    //      });
    //    }
    //    ServerResponse response = new ServerResponse();

    //    if (adDetails.AdUser.UserId == 0)
    //    {

    //      response = onboardHelper.GetUser(adDetails.AdUser.Phone, context);

    //      if (response.HasSucceeded)
    //      {
    //        adDetails.AdUser = (User)response.Result;
    //      }
    //      else
    //      {
    //        response = onboardHelper.GetUser(ad.AdUser.Email, context);

    //        if (response.HasSucceeded)
    //        {
    //          adDetails.AdUser = (User)response.Result;
    //        }
    //      }

    //      if (!response.HasSucceeded)
    //      {
    //        OperationResponse resOp = onboardHelper.RegisterUser(new RegisterUser { FirstName = adDetails.AdUser.FirstName, Email = adDetails.AdUser.Email, Phone = adDetails.AdUser.Phone, Password = "temp", UserType = 1, Gender = 1 }, context);

    //        if (resOp.HasSucceeded)
    //        {
    //          response = onboardHelper.GetUser(adDetails.AdUser.Email, context);

    //          adDetails.AdUser = (User)response.Result;
    //        }
    //      }
    //    }

    //    OperationResponse resp = helper.PostAd(adDetails, context);

    //    if (resp.HasSucceeded)
    //    {
    //      int postedAdId = int.Parse(resp.Message);
    //      string folderName = "Upload/" + adDetails.AdUser.UserId.ToString() + "/" + postedAdId.ToString();
    //      string webRootPath = _hostingEnvironment.WebRootPath;
    //      string newPath = Path.Combine(webRootPath, folderName);
    //      string tempPath = Path.Combine(webRootPath, "Upload/temp/"  +adDetails.AdUser.UserId.ToString() + "/" + postedAdId.ToString()); 
          
    //      //Image uploadedImage=
        
    //      if (!Directory.Exists(newPath))
    //      {
    //        Directory.CreateDirectory(newPath);
    //      }

    //      //if (Directory.Exists(Path.Combine(webRootPath, "Upload/temp/" + adDetails.AdUser.UserId.ToString())))
    //      //{
    //      //  imgHelper.ClearAttributes(Path.Combine(webRootPath, "Upload/temp/" + adDetails.AdUser.UserId.ToString()));

    //      //  Directory.Delete(Path.Combine(webRootPath, "Upload/temp/" + adDetails.AdUser.UserId.ToString()), true);
    //      //}
    //      if (!Directory.Exists(tempPath))
    //      {
    //        Directory.CreateDirectory(tempPath);            
    //      }
    //      int fileCount = 0;
    //      foreach (var file in files)
    //      {
    //        fileCount++;
    //        if (file.Length > 0)
    //        {
    //          AdImage image = new AdImage();

    //          Guid imageId = Guid.NewGuid();

    //          string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

    //          string fullPath = Path.Combine(newPath, fileName);

    //          string fullPathTempPath = Path.Combine(tempPath, fileName);

    //          string extension = Path.GetExtension(fullPath);
    //          image.AdId = postedAdId;
    //          image.ImageUrl = imageId + extension;
    //          image.OriginalFileName = fileName;             

    //          using (var stream = new FileStream(Path.Combine(tempPath, imageId + extension), FileMode.Create))
    //          {
    //            file.CopyTo(stream);
    //            stream.Flush();
    //            stream.Dispose();
    //          }
              
    //          imgHelper.AddWaterMark(Path.Combine(tempPath, imageId + extension), Path.Combine(newPath, imageId  + extension), Path.Combine(newPath, "thumb_image" + extension),fileCount==adDetails.SelectedThumb?true:false);

    //          OperationResponse adImageResponse = helper.PostAdImage(image, context); // insert image details to ad_images table here

    //          if (adImageResponse.HasSucceeded==true && fileCount==1)
    //          {
    //            helper.UpdatedThumb(image.AdId, "thumb_image" + extension, context);
    //          }              
    //        }
    //      }
    //      if (Directory.Exists(Path.Combine(webRootPath, "Upload/temp/" + adDetails.AdUser.UserId.ToString())))
    //      {
    //        imgHelper.ClearAttributes(Path.Combine(webRootPath, "Upload/temp/" + adDetails.AdUser.UserId.ToString()));

    //        Directory.Delete(Path.Combine(webRootPath, "Upload/temp/" + adDetails.AdUser.UserId.ToString()), true);
    //      }

    //    }
        
    //    return Json(new OperationResponse
    //    {
    //      HasSucceeded = true,
    //      Message = "Ad successfully posted",
    //      IsDomainValidationErrors = false,
    //      StatusCode = "200"
    //    });
    //  }
    //  catch (System.Exception ex)
    //  {
    //    return Json(new OperationResponse
    //    {
    //      HasSucceeded = false,
    //      Message = ex.Message,
    //      IsDomainValidationErrors = false,
    //      StatusCode = "500"
    //    });
    //  }
    //}
  }
}
