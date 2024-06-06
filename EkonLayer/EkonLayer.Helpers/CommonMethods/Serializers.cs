using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkonLayer.Helpers.CommonMethods
{
    public static class Serializers
    {
        public static string SerializeJson(object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static byte[] Serialize(object obj)
        {
            byte[] res;
            try
            {
                res = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));

            }
            catch (Exception)
            {
                return new byte[0];

            }
            return res;
        }

        public static T DeserializeJson<T>(string obj, string dateFormat = "dd.MM.yyyy")
        {
            try
            {
                var jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                jsonSerializerSettings.DateFormatString = dateFormat;

                return JsonConvert.DeserializeObject<T>(obj, jsonSerializerSettings);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
    }
}
