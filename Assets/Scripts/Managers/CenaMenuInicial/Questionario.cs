using UnityEngine;

[System.Serializable]
public class Questionario
{
    public int versao;
    public PerguntaQuestionario[] perguntas;
}

[System.Serializable]
public class PerguntaQuestionario
{
    public string pergunta;
    public string tipo; 
    public string[] opcoes;
}

