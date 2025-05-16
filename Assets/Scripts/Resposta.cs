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