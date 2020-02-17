using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Core.Helpers
{
    public class CommonHelper
    {
        public static JsonSerializerSettings MicrosoftDateFormatSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
            }
        }

        public string GetJsonString(object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj, MicrosoftDateFormatSettings);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
