using System.Collections;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public string questao;
    public int id;
    public Resposta[] respostas;
    public string exercicio;
    public string url_imagem;
    public string respostaCorreta;
    public bool especial;
    [SerializeField] private Material[] materiais;
    [SerializeField] private GameObject[] planes;

    public void DefinirPergunta(Pergunta p)
    {
        id = p.id;
        questao = p.questao;
        respostas = p.respostas;
        foreach (var resposta in respostas)
        {
            if (resposta.correta)
            {
                respostaCorreta = resposta.texto;
                break;
            }
        }
    }

    public void DefinirExercicio(Exercicio e)
    {
        exercicio = e.exercicio;
        url_imagem = e.url_imagem;
    }

}
