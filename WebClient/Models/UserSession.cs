using System.Text.Json;

namespace WebClient.Models
{
    public static class UserSession
    {

        private const string key = "s3cr3cK3y";
        public static void Set<T>(this ISession session, T value)
        {
            session.SetString(key,  JsonSerializer.Serialize<T>(value, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
        }

        public static T? Get<T>(this ISession session,string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
    }
}
