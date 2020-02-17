using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;

namespace OERService.Helpers
{
    public class MiscHelper
    {
        public AWSS3Config GeAWSConfig(List<Dictionary<string, string>> configDict)
        {
            AWSS3Config config = new AWSS3Config();            
            config.AWSAccessKey = configDict.Single(x => x["key"] == "AWSAccessKey")["value"];
            config.AWSSecretKey = configDict.Single(x => x["key"] == "AWSSecretKey")["value"];
            config.AWSBucketName = configDict.Single(x => x["key"] == "AWSBucketName")["value"];
            config.AWSUser = configDict.Single(x => x["key"] == "AWSUser")["value"];
            config.AWSEndPoint = configDict.Single(x => x["key"] == "AWSEndPoint")["value"];
            return config;
        }
    }
}
