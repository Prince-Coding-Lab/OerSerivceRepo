using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Core.Models
{
    [DataContract]
    public class AWSS3Config
    {
        [DataMember(Name = "AWSAccessKey")]
        public string AWSAccessKey { get; set; }

        [DataMember(Name = "AWSSecretKey")]
        public string AWSSecretKey { get; set; }

        [DataMember(Name = "AWSBucketName")]
        public string AWSBucketName { get; set; }

        [DataMember(Name = "AWSUser")]
        public string AWSUser { get; set; }

        [DataMember(Name = "AWSEndPoint")]
        public string AWSEndPoint { get; set; }

    }

    public class UploadResponse
    {
        public bool HasSucceed { get; set; }

        public string FileName { get; set; }

        public string Message { get; set; }
    }

    public class DownloadResponse
    {
        public bool HasSucceed { get; set; }

        public byte[] FileObject { get; set; }

        public string Message { get; set; }
    }


}
