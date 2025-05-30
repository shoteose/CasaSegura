using System.Collections;
using System.IO;
using UnityEngine;

public static class QuestionarioLoader
{
    public static IEnumerator LoadQuestionario(string fileName, System.Action<Questionario> callback)
    {
        string jsonLocal = JsonFileManager.LoadJson(fileName);
        if (!string.IsNullOrEmpty(jsonLocal))
        {
            Questionario questionario = JsonUtility.FromJson<Questionario>(jsonLocal);
            callback?.Invoke(questionario);
            yield break;
        }

        yield return PerguntasLoader.LoadFromStreamingAssets<Questionario>(fileName, callback);
    }
}
