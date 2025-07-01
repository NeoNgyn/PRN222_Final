using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; // Có thể cần tùy thuộc vào các extension khác
using Newtonsoft.Json; // Cần cho Serialize/Deserialize
using Microsoft.AspNetCore.Http;
namespace Shopping_Store.Extensions
{
    public static class SessionExtensions // Có vẻ bạn đã gõ sai "Session" thành "Sesion"
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }


        public static T? GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            // Bạn cũng nên kiểm tra nếu value là null hoặc rỗng trước khi Deserialize
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}