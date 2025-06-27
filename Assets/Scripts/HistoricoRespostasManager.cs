using System.Collections.Generic;
using UnityEngine;

public static class HistoricoRespostaManager
{
    private const string FILE_NAME = "historico_respostas";
    private static HistoricoRespostas historico;

    static HistoricoRespostaManager()
    {
        Load();
    }

    public static void AdicionarResposta(int id, bool acertou)
    {
        var existente = historico.respostas.Find(r => r.pergunta_id == id);

        if (existente != null)
        {
            if (acertou) existente.certas++;
            else existente.erradas++;
        }
        else
        {
            historico.respostas.Add(new ResultadoPergunta
            {
                pergunta_id = id,
                certas = acertou ? 1 : 0,
                erradas = acertou ? 0 : 1
            });
        }

        Save();
    }

    public static void Save()
    {
        string json = JsonUtility.ToJson(historico);
        JsonFileManager.SaveJson(FILE_NAME, json);
    }

    public static void Load()
    {
        string json = JsonFileManager.LoadJson(FILE_NAME);
        if (!string.IsNullOrEmpty(json))
            historico = JsonUtility.FromJson<HistoricoRespostas>(json);
        else
            historico = new HistoricoRespostas();
    }

    public static List<ResultadoPergunta> GetRespostasPendentes()
    {
        return historico.respostas;
    }

    public static void Limpar()
    {
        historico.respostas.Clear();
        Save();
    }

    public static void DebugImprimirRespostas()
    {
        Debug.Log("======= HISTÓRICO DE RESPOSTAS =======");
        foreach (var r in historico.respostas)
        {
            Debug.Log($"Pergunta ID: {r.pergunta_id} | Certas: {r.certas} | Erradas: {r.erradas}");
        }
        Debug.Log("======================================");
    }


}
