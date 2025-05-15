using UnityEngine;

public class Tile : MonoBehaviour
{
    public string questao;
    public Resposta[] respostas;
    public string respostaCorreta;
    public bool especial;

    public void Questoes()
    {
        if (!especial)
        {

            Perguntas bancoPerguntas = LoadPerguntasFromJSON();

            int randomIndex = Random.Range(0, bancoPerguntas.perguntas.Length);
            Pergunta perguntaAleatoria = bancoPerguntas.perguntas[randomIndex];

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
    }

    private Perguntas LoadPerguntasFromJSON()
    {
        TextAsset json = Resources.Load<TextAsset>("teste");
        if (json != null)
        {
            Perguntas banco = JsonUtility.FromJson<Perguntas>(json.ToString());
            return banco;
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

