using UnityEngine;

public class Tile : MonoBehaviour
{
    public string questao;
    public Resposta[] respostas;
    public string exercicio;
    public string url_imagem;
    public string respostaCorreta;
    public bool especial;

    public void Questoes()
    {
        if (!especial)
        {

            Perguntas perguntasArray = LoadPerguntasFromJSON();

            int randomIndex = Random.Range(0, perguntasArray.perguntas.Length);
            Pergunta perguntaAleatoria = perguntasArray.perguntas[randomIndex];

            questao = perguntaAleatoria.questao;
            respostas = perguntaAleatoria.respostas;

            foreach (var resposta in respostas)
            {
                if (resposta.correta)
                {
                    respostaCorreta = resposta.texto;
                    break;
                }
            }

        }
        else
        {

            Exercicios exerciciosArray = LoadExerciciosFromJSON();

            int randomIndex = Random.Range(0, exerciciosArray.exercicios.Length);
            Exercicio exercicioAleatorio = exerciciosArray.exercicios[randomIndex];

            exercicio = exercicioAleatorio.exercicio;
            url_imagem = exercicioAleatorio.url_imagem;

        }
    }

    private Perguntas LoadPerguntasFromJSON()
    {
        TextAsset json = Resources.Load<TextAsset>("teste");
        if (json != null)
        {
            Perguntas perguntas = JsonUtility.FromJson<Perguntas>(json.ToString());
            return perguntas;
        }
        else
        {
            Debug.LogError("Arquivo JSON não encontrado!");
            return null;
        }
    }

    private Exercicios LoadExerciciosFromJSON()
    {
        TextAsset json = Resources.Load<TextAsset>("teste");
        if (json != null)
        {
            Exercicios exercicios = JsonUtility.FromJson<Exercicios>(json.ToString());
            return exercicios;
        }
        else
        {
            Debug.LogError("Arquivo JSON não encontrado!");
            return null;
        }
    }

}

[System.Serializable]
public class Resposta
{
    public string texto;
    public bool correta;
}

[System.Serializable]
public class Pergunta
{
    public string questao;
    public Resposta[] respostas;
}

[System.Serializable]
public class Perguntas
{
    public Pergunta[] perguntas;
}

[System.Serializable]
public class Exercicio
{
    public string exercicio;
    public string url_imagem;
}

[System.Serializable]
public class Exercicios
{
    public Exercicio[] exercicios;
}
