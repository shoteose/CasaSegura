using System.Collections;
using UnityEngine;
using System.IO;

public static class GenericLoader
{
    public static IEnumerator Load<T>(string fileName, System.Action<T> callback) where T : class
    {


        // tenta carregar primeiro do persistent data
        string jsonLocal = JsonFileManager.LoadJson(fileName);


        if (!string.IsNullOrEmpty(jsonLocal))
        {
            try
            {
                T data = JsonUtility.FromJson<T>(jsonLocal);
                callback?.Invoke(data);
            }
            catch (System.Exception e)
            {
                callback?.Invoke(null);
            }
            yield break;
        }



        // tenta carregar do resources
        string fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
        TextAsset resourceJson = Resources.Load<TextAsset>(fileNameWithoutExt);
        if (resourceJson != null)
        {
            T data = JsonUtility.FromJson<T>(resourceJson.text);
            callback?.Invoke(data);
            yield break;
        }


        // Erro
        Debug.LogWarning($"Erro total ao carregar o ficheiro {fileName}.");
        callback?.Invoke(null);
    }
}
