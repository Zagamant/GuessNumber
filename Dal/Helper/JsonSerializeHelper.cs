using Newtonsoft.Json;

namespace Dal.Helper
{
    public static class JsonSerializeHelper
    {
        public static string Serialize<T>(T model)
        {
            var json = JsonConvert.SerializeObject(model);

            return json;
        }

        public static T Deserialize<T>(string json)
        {
            var player = JsonConvert.DeserializeObject<T>(json);
            return player;
        }
    }
}
