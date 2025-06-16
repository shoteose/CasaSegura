using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class ApiManager : MonoBehaviour
{
    private const string urlVersoes = "https://casaseguraapi.onrender.com/api/versoes";

    public static bool AtualizacaoTerminada = false;

    public static bool net;

    public const string nomeArquivoPerguntas = "perguntas";
    public const string nomeArquivoExercicios = "exercicios";
    public const string nomeArquivoQuestionario = "questionario";


    private const string urlPerguntas = "https://casaseguraapi.onrender.com/api/pergunta/json";
    private const string urlExercicios = "https://casaseguraapi.onrender.com/api/exercicio/json";
    private const string urlQuestionario = "https://casaseguraapi.onrender.com/api/perguntaquestionario/json";
    private const string urlRespostaquestionario = "https://casaseguraapi.onrender.com/api/respostaquestionario";

    void Start()
    {
        StartCoroutine(Setup());
     
    }

    private IEnumerator Setup()
    {
        yield return StartCoroutine(VerificarInternet());

        if (net)
        {

            StartCoroutine(VerificarAtualizacoes());

        }
    }

    private IEnumerator VerificarInternet()
    {
        UnityWebRequest request = new UnityWebRequest("https://google.com");

        yield return request.SendWebRequest();

        if (request.error != null)
        {
            net = false;
        }
        else
        {
            net = true;
        }
    }

    private IEnumerator VerificarAtualizacoes()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(urlVersoes))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Falha ao sacar as versões: " + request.error);
                AtualizacaoTerminada = true;
                yield break;
            }

            var json = request.downloadHandler.text;
            var versoes = JsonUtility.FromJson<VersoesApi>(json);

            if (versoes == null)
            {
                Debug.Log("Falha ao desserializar as versões.");
                AtualizacaoTerminada = true;
                yield break;
            }

            if (versoes.perguntas > DadosPlayer.GetVersaoPerguntas())
                yield return StartCoroutine(DownloadEAtualizar(nomeArquivoPerguntas, urlPerguntas, versoes.perguntas, DadosPlayer.SetVersaoPerguntas));

            if (versoes.exercicios > DadosPlayer.GetVersaoExercicios())
                yield return StartCoroutine(DownloadEAtualizar(nomeArquivoExercicios, urlExercicios, versoes.exercicios, DadosPlayer.SetVersaoExercicios));

            if (versoes.questionario > DadosPlayer.GetVersaoQuestionario())
                yield return StartCoroutine(DownloadEAtualizar(nomeArquivoQuestionario, urlQuestionario, versoes.questionario, DadosPlayer.SetVersaoQuestionario));

            Debug.Log("Versões atualizadas com sucesso.");
            Debug.Log(versoes.questionario + " " + versoes.exercicios + " " + versoes.perguntas);
            Debug.Log(DadosPlayer.GetVersaoQuestionario() + " " + DadosPlayer.GetVersaoExercicios() + " " + DadosPlayer.GetVersaoPerguntas());
        }

        AtualizacaoTerminada = true;
    }


    private IEnumerator DownloadEAtualizar(string nomeArquivo, string url, int versaoNova, System.Action<int> setVersaoCallback)
    {
        Debug.Log("Estou a fazer download dp " + nomeArquivo + " da versao " + versaoNova);

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log($"falha ao sacar {nomeArquivo}: {request.error}");
                yield break;
            }

            string json = request.downloadHandler.text;

            if (json.Contains("\"error\""))
            {
                Debug.LogError($"resposta da API deu erro: {nomeArquivo}:\n{json}");
                yield break;
            }

            JsonFileManager.SaveJson(nomeArquivo, json);
            setVersaoCallback?.Invoke(versaoNova);

            Debug.Log($"{nomeArquivo} atualizado para versão {versaoNova}.");
        }
    }

    public IEnumerator EnviarResposta(RespostaQuestionario rq)
    {
        Debug.Log($"UUID: {rq.utilizador_id} Pergunta_id: {rq.pergunta_id} Resposta: {rq.resposta_texto}");

        string json = JsonUtility.ToJson(rq);
        Debug.Log("A enviar JSON: " + json);

        using (UnityWebRequest request = new UnityWebRequest(urlRespostaquestionario, "POST"))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Erro ao enviar resposta: " + request.error);
            }
            else
            {
                Debug.Log("Resposta enviada com sucesso: " + request.downloadHandler.text);
            }
        }
    }

}
