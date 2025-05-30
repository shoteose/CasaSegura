using System.IO;
using UnityEngine;

public static class JsonFileManager
{
    private static string GetPath(string fileName) =>
        Path.Combine(Application.persistentDataPath, fileName);

    public static void SaveJson(string fileName, string jsonContent)
    {
        string path = GetPath(fileName);
        File.WriteAllText(path, jsonContent);
        Debug.Log($"JSON salvo em: {path}");
    }

    public static string LoadJson(string fileName)
    {
        string path = GetPath(fileName);
        if (File.Exists(path))
            return File.ReadAllText(path);
        return null;
    }
}
