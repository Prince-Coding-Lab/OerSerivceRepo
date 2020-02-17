using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Core.Models;


namespace Core.Helpers
{
    public class AmazonS3
    {
        public AmazonS3(AWSS3Config s3Config)
        {
            accessKey = s3Config.AWSAccessKey;
            accessSecret = s3Config.AWSSecretKey;
            bucket = s3Config.AWSBucketName;
        }

        private static String accessKey { get; set; }
        private static String accessSecret { get; set; }
        private static String bucket { get; set; }


        public  async Task<UploadResponse> UploadFile(IFormFile file,string fileName)
        {
            try
            {
                // connecting to the client
                var client = new AmazonS3Client(accessKey, accessSecret, Amazon.RegionEndpoint.USEast2);

                // get the file and convert it to the byte[]
                byte[] fileBytes = new Byte[file.Length];

                file.OpenReadStream().Read(fileBytes, 0, Int32.Parse(file.Length.ToString()));

                // create unique file name
              //  var fileName = Guid.NewGuid() + Path.GetExtension( file.FileName);

                PutObjectResponse response = null;

                using (var stream = new MemoryStream(fileBytes))
                {
                    var request = new PutObjectRequest
                    {
                        BucketName = bucket,
                        Key = fileName,
                        InputStream = stream,
                        ContentType = file.ContentType,
                        CannedACL = S3CannedACL.BucketOwnerFullControl
                    };

                    response = await client.PutObjectAsync(request);
                };

                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {

                    return new UploadResponse
                    {
                        HasSucceed = true,
                        FileName = fileName
                    };
                }
                else
                {

                    return new UploadResponse
                    {
                        HasSucceed = false,
                        FileName = fileName
                    };
                }
            }
            catch(Exception ex)
            {
                return new UploadResponse
                {
                    HasSucceed = false,
                    Message = ex.Message

                };

            }
        }

        public async Task<UploadResponse> RemoveUploadedFile(String fileName)
        {
            try
            {
                var client = new AmazonS3Client(accessKey, accessSecret, Amazon.RegionEndpoint.EUCentral1);

                var request = new DeleteObjectRequest
                {
                    BucketName = bucket,
                    Key = fileName
                };

                var response = await client.DeleteObjectAsync(request);

                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    return new UploadResponse
                    {
                        HasSucceed = true,
                        FileName = fileName
                    };
                }
                else
                {
                    return new UploadResponse
                    {
                        HasSucceed = false,
                        FileName = fileName
                    };
                }
            }
            catch(Exception ex)
            {
                return new UploadResponse
                {
                    HasSucceed = false,
                    Message = ex.Message
                };

            }
        }

        public async Task<DownloadResponse> DownloadFile( string objectKey)
        {
            try
            {
                // connecting to the client
                var client = new AmazonS3Client(accessKey, accessSecret, Amazon.RegionEndpoint.APSoutheast1);

                // create a get request
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = bucket,
                    Key = objectKey
                };
               
                // Issue request and remember to dispose of the response
                using (GetObjectResponse response = await client.GetObjectAsync(request))
                {
                    using (Stream responseStream = response.ResponseStream)
                    {
                        return new DownloadResponse
                        {
                            HasSucceed = false,
                            FileObject= ReadStream(responseStream)
                        };
                        
                    }
                }
              
            }
            catch (Exception ex)
            {
                return new DownloadResponse
                {
                    HasSucceed = false,
                    Message = ex.Message

                };

            }
        }

        private static byte[] ReadStream(Stream responseStream)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        public async Task<DeleteObjectResponse> MoveObjectAsync()
        {
            // test method need to pass dynamic resource path 
            var client = new AmazonS3Client(accessKey, accessSecret, Amazon.RegionEndpoint.USEast2);

            CopyObjectRequest cpRequest = new CopyObjectRequest
            {
                SourceBucket = bucket,
                SourceKey = "temp/profile/1558090788277Mu5EShGiYu.jpg",
                DestinationBucket = bucket,
                DestinationKey = "profile/1558090788277Mu5EShGiYu.jpg"
            };

            CopyObjectResponse cpResponse = await client.CopyObjectAsync(cpRequest);

            var request = new DeleteObjectRequest
            {
                BucketName = bucket,
                Key = "temp/profile/1558090788277Mu5EShGiYu.jpg",
                 
            };           

            var response = await client.DeleteObjectAsync(request);

            return response;
           
        }

        
    }
}
