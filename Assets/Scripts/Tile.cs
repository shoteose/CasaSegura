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
            Perguntas perguntasArray = null;
            yield return PerguntasLoader.LoadPerguntas("perguntas", p => perguntasArray = p);

            if (perguntasArray == null || perguntasArray.perguntas == null || perguntasArray.perguntas.Length == 0)
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
            Exercicios exerciciosArray = null;
            yield return ExerciciosLoader.LoadExercicios("exercicios", e => exerciciosArray = e);

            if (exerciciosArray == null || exerciciosArray.exercicios == null || exerciciosArray.exercicios.Length == 0)
                yield break;

            int randomIndex = Random.Range(0, exerciciosArray.exercicios.Length);
            Exercicio exercicioAleatorio = exerciciosArray.exercicios[randomIndex];

            exercicio = exercicioAleatorio.exercicio;
            url_imagem = exercicioAleatorio.url_imagem;
        }
    }

}
