using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

public class ConfigReader
{
    private static Dictionary<string, string> configData;

    static ConfigReader()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "config.json");
        if (File.Exists(path))
        {
            string jsonString = File.ReadAllText(path);
            configData = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
        }
        else
        {
            Debug.LogError("Le fichier config.json n'a pas été trouvé.");
        }
    }

    public static string GetValue(string key)
    {
        if (configData != null && configData.ContainsKey(key))
        {
            return configData[key];
        }
        else
        {
            Debug.LogError($"La clé '{key}' n'existe pas dans le fichier config.json ou le fichier n'a pas été chargé.");
            return null;
        }
    }
}
