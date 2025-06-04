using System.IO;
using UnityEngine;

public static class JsonFileManager
{
    public static void SaveJson(string fileName, string json)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName + ".json");
        File.WriteAllText(path, json);
    }

    public static string LoadJson(string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName + ".json");
        if (File.Exists(path))
            return File.ReadAllText(path);
        return null;
    }

    public static void DeleteJson(string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName + ".json");
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"[JsonFileManager] Apagado: {fileName}.json");
        }
    }

}
