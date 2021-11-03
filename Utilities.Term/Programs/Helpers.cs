using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

#nullable enable
namespace Utilities.Term.Programs
{
    public static class Helpers
    {
        public static async Task<T?> DeserializeJsonContentAsync<T>(string path)
        {
            if (!File.Exists(path)) return default;
            using StreamReader reader = new(path);
            var jsonContent = await reader.ReadToEndAsync();
            reader.Close();
            if (string.IsNullOrEmpty(jsonContent) || string.IsNullOrWhiteSpace(jsonContent)) return default;
            var allRequestFlows = JsonConvert.DeserializeObject<T>(jsonContent) ?? default;
            return allRequestFlows;
        }
    }
}