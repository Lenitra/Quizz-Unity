using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Collections.Generic;

public class ConfigReader
{
    private static Dictionary<string, string> configData;
    private static bool configLoaded = false;

    static ConfigReader()
    {
        LoadConfig();
    }

    private static void LoadConfig()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "config.json");
        if (path.Contains("://") || path.Contains(":///"))
        {
            UnityWebRequest www = UnityWebRequest.Get(path);
            www.SendWebRequest().completed += (asyncOperation) =>
            {
                if (www.result == UnityWebRequest.Result.ConnectionError || 
                    www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(www.error);
                }
                else
                {
                    configData = JsonConvert.DeserializeObject<Dictionary<string, string>>(www.downloadHandler.text);
                    configLoaded = true;
                }
            };
        }
        else
        {
            string jsonString = File.ReadAllText(path);
            configData = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
            configLoaded = true;
        }
    }

    public static string GetValue(string key)
    {
        if (!configLoaded)
        {
            Debug.LogError("Configuration has not been loaded yet.");
            return null;
        }

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
