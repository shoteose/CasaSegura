using System.Collections;
using UnityEngine;
using System.IO;

public static class GenericLoader
{
    public static IEnumerator Load<T>(string fileName, System.Action<T> callback) where T : class
    {


        // tenta carregar primeiro do persistent dados
        string jsonLocal = JsonFileManager.LoadJson(fileName);


        if (!string.IsNullOrEmpty(jsonLocal))
        {
            try
            {
                Debug.Log("Dados Persistentes (ja sacado)");
                T dados = JsonUtility.FromJson<T>(jsonLocal);
                callback?.Invoke(dados);
            }
            catch (System.Exception e)
            {
                callback?.Invoke(null);
            }
            yield break;
        }



        // tenta carregar do resources
        string ficheiroSemExtensao = Path.GetFileNameWithoutExtension(fileName);
        TextAsset resourceJson = Resources.Load<TextAsset>(ficheiroSemExtensao);
        if (resourceJson != null)
        {
            Debug.Log("carregeuei do resources com o " + fileName);
            T dados = JsonUtility.FromJson<T>(resourceJson.text);
            callback?.Invoke(dados);
            yield break;
        }


        // Erro
        Debug.LogWarning($"Erro total ao carregar o ficheiro {fileName}.");
        callback?.Invoke(null);
    }
}
