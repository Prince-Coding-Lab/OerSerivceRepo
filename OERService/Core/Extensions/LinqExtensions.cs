using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Newtonsoft.Json;
using Core.Models;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public class LinqExtensions
    {
        public static List<Dictionary<string, string>> GetDictionary(DataTable dt)
        {
            return dt.AsEnumerable().Select(
                row => dt.Columns.Cast<DataColumn>().ToDictionary(
                    column => column.ColumnName,
                    column => row[column].ToString()
                )).ToList();
        }

        public static T GeObjectFromDictionary<T>(Dictionary<string, string> dict)
        {
            string json = JsonConvert.SerializeObject(dict);
            return JsonConvert.DeserializeObject<T>(json);
        }

    }
}
