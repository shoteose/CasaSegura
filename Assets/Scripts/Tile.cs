using System.Collections;
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
        StartCoroutine(CarregarQuestoesCoroutine());
    }

    private IEnumerator CarregarQuestoesCoroutine()
    {
        
        if (!especial)
        {
            Debug.Log("Este tile é pergunta");
            Perguntas perguntasArray = null;
            yield return GenericLoader.Load<Perguntas>(ApiManager.nomeArquivoPerguntas, p => perguntasArray = p);

            if (perguntasArray?.perguntas == null || perguntasArray.perguntas.Length == 0)
                yield break;

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
            Debug.Log("Este tile é especial");
            Exercicios exerciciosArray = null;
            yield return GenericLoader.Load<Exercicios>(ApiManager.nomeArquivoExercicios, e => exerciciosArray = e);

            if (exerciciosArray?.exercicios == null || exerciciosArray.exercicios.Length == 0)
                yield break;

            int randomIndex = Random.Range(0, exerciciosArray.exercicios.Length);
            Exercicio exercicioAleatorio = exerciciosArray.exercicios[randomIndex];

            exercicio = exercicioAleatorio.exercicio;
            url_imagem = exercicioAleatorio.url_imagem;
        }
    }
}
