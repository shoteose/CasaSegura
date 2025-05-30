using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public static class PerguntasLoader
{
    public static IEnumerator LoadPerguntas(string fileName, System.Action<Perguntas> callback)
    {
        string jsonLocal = JsonFileManager.LoadJson(fileName);
        if (!string.IsNullOrEmpty(jsonLocal))
        {
            Perguntas perguntas = JsonUtility.FromJson<Perguntas>(jsonLocal);
            callback?.Invoke(perguntas);
            yield break;
        }

        yield return LoadFromStreamingAssets<Perguntas>(fileName, callback);
    }

    public static IEnumerator LoadFromStreamingAssets<T>(string fileName, System.Action<T> callback) where T : class
    {
        string pathStreaming = Path.Combine(Application.streamingAssetsPath, fileName + ".json");

        if (Application.platform == RuntimePlatform.Android)
        {
            using UnityWebRequest www = UnityWebRequest.Get(pathStreaming);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                T obj = JsonUtility.FromJson<T>(www.downloadHandler.text);
                callback?.Invoke(obj);
            }
            else
            {
                Debug.Log($"Erro ao carregar {fileName} do StreamingAssets (Android): {www.error}");
                callback?.Invoke(null);
            }
        }
        else
        {
            if (File.Exists(pathStreaming))
            {
                string jsonStreaming = File.ReadAllText(pathStreaming);
                T obj = JsonUtility.FromJson<T>(jsonStreaming);
                callback?.Invoke(obj);
            }
            else
            {
                Debug.Log($"Arquivo {fileName} não encontrado no StreamingAssets: {pathStreaming}");
                callback?.Invoke(null);
            }
        }
    }
}
