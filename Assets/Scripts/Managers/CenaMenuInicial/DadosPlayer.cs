using UnityEngine;

public static class DadosPlayer
{
    private const string VERSAO_PERGUNTAS = "versao_perguntas";
    private const string VERSAO_EXERCICIOS = "versao_exercicios";
    private const string VERSAO_QUESTIONARIO = "versao_questionario";

    private const string QUESTIONARIO_RESPONDIDO = "questionario_respondido";

    public static int GetVersaoPerguntas() => PlayerPrefs.GetInt(VERSAO_PERGUNTAS, 0);
    public static void SetVersaoPerguntas(int v) { PlayerPrefs.SetInt(VERSAO_PERGUNTAS, v); PlayerPrefs.Save(); }

    public static int GetVersaoExercicios() => PlayerPrefs.GetInt(VERSAO_EXERCICIOS, 0);
    public static void SetVersaoExercicios(int v) { PlayerPrefs.SetInt(VERSAO_EXERCICIOS, v); PlayerPrefs.Save(); }

    public static int GetVersaoQuestionario() => PlayerPrefs.GetInt(VERSAO_QUESTIONARIO, 0);
    public static void SetVersaoQuestionario(int v) { PlayerPrefs.SetInt(VERSAO_QUESTIONARIO, v); PlayerPrefs.Save(); }

    public static bool JaRespondeuQuestionario() => PlayerPrefs.GetInt(QUESTIONARIO_RESPONDIDO, 0) == 1;
    public static void MarcarQuestionarioRespondido() { PlayerPrefs.SetInt(QUESTIONARIO_RESPONDIDO, 1); PlayerPrefs.Save(); }
    public static void ResetarQuestionario() { PlayerPrefs.DeleteKey(QUESTIONARIO_RESPONDIDO); PlayerPrefs.Save(); }

    public static bool DeveExibirQuestionario(int versaoAtual)
    {
        if (!JaRespondeuQuestionario()) return true;
        if (GetVersaoQuestionario() < versaoAtual) return true;
        return false;
    }
}


[System.Serializable]
public class VersoesApi
{
    public int versaoPerguntas;
    public int versaoExercicios;
    public int versaoQuestionario;
}
