using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Nfl.Rushing.FrontEnd.Infrastructure.Utils
{
    public static class Base64ConversionHelper
    {
        public static string ConvertToBase64String<T>(T value)
        {
            return Convert.ToBase64String(Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(value)));
        }

        public static T ConvertFromBase64String<T>(string base64Value)
        {
            return JsonConvert.DeserializeObject<T>(
                Encoding.Unicode.GetString(Convert.FromBase64String(base64Value)));
        }
    }
}
