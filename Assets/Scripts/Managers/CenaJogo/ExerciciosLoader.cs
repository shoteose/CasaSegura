using System.Collections;
using System.IO;
using UnityEngine;

public static class ExerciciosLoader
{
    public static IEnumerator LoadExercicios(string fileName, System.Action<Exercicios> callback)
    {
        string jsonLocal = JsonFileManager.LoadJson(fileName);
        if (!string.IsNullOrEmpty(jsonLocal))
        {
            Exercicios exercicios = JsonUtility.FromJson<Exercicios>(jsonLocal);
            callback?.Invoke(exercicios);
            yield break;
        }

        yield return PerguntasLoader.LoadFromStreamingAssets<Exercicios>(fileName, callback);
    }
}
