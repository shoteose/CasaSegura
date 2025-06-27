using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Resposta
{
    public string texto;
    public bool correta;
}

[System.Serializable]
public class Pergunta
{
    public int id;
    public string questao;
    public Resposta[] respostas;
}

[System.Serializable]
public class Perguntas
{
    public int versao;
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
    public int versao;
    public Exercicio[] exercicios;
}
[System.Serializable]

public class ResultadoPergunta
{
    public int pergunta_id;
    public int certas;
    public int erradas;
}

[System.Serializable]
public class HistoricoRespostas
{
    public List<ResultadoPergunta> respostas = new();
}
