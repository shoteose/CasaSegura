using UnityEngine;

public static class PerguntasLoader
{
    public static Perguntas LoadPerguntas(string path)
    {
        TextAsset json = Resources.Load<TextAsset>(path);
        if (json != null)
        {
            Perguntas perguntas = JsonUtility.FromJson<Perguntas>(json.text);
            return perguntas;
        }
        else
        {
            Debug.LogError($"Arquivo JSON de perguntas '{path}' não encontrado!");
            return null;
        }
    }

    public static Exercicios LoadExercicios(string path)
    {
        TextAsset json = Resources.Load<TextAsset>(path);
        if (json != null)
        {
            Exercicios exercicios = JsonUtility.FromJson<Exercicios>(json.text);
            return exercicios;
        }
        else
        {
            Debug.LogError($"Arquivo JSON de exercícios '{path}' não encontrado!");
            return null;
        }
    }
}
