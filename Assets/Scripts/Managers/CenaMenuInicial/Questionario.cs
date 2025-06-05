using System.Collections.Generic;
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
    public int id;
    public string tipo; 
    public string[] opcoes;
}

[System.Serializable]
public class RespostaQuestionario
{
    public string utilizador_id;
    public int pergunta_id;
    public string resposta_texto;

}

[System.Serializable]
public class RespostasQuestionario
{
    public string utilizador_id;
    public Dictionary<int, string> respostas;

}
