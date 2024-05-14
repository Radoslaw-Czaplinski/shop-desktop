using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public static class LocalDataStore
{
    private static string filePath = "postsDates.json";

    public static void SavePostsDates(Dictionary<int, DateTimeOffset> postsDates)
    {
        var json = JsonConvert.SerializeObject(postsDates, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    public static Dictionary<int, DateTimeOffset> LoadPostsDates()
    {
        if (!File.Exists(filePath))
        {
            return new Dictionary<int, DateTimeOffset>();
        }

        var json = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<Dictionary<int, DateTimeOffset>>(json);
    }
}
