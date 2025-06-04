using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class ApiManager : MonoBehaviour
{
    private const string urlVersoes = "https://casaseguraapi.onrender.com/api/versoes";

    public static bool AtualizacaoTerminada = false;


    public const string nomeArquivoPerguntas = "perguntas";
    public const string nomeArquivoExercicios = "exercicios";
    public const string nomeArquivoQuestionario = "questionario";


    private const string urlPerguntas = "https://casaseguraapi.onrender.com/api/pergunta/json";
    private const string urlExercicios = "https://casaseguraapi.onrender.com/api/exercicio/json";
    private const string urlQuestionario = "https://casaseguraapi.onrender.com/api/perguntaquestionario/json";

    void Start()
    {
        StartCoroutine(VerificarAtualizacoes());
    }

    private IEnumerator VerificarAtualizacoes()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.LogWarning("Sem net");
            yield break;
        }

        using (UnityWebRequest request = UnityWebRequest.Get(urlVersoes))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Falha ao sacar as versões: " + request.error);
                yield break;
            }

            var json = request.downloadHandler.text;
            var versoes = JsonUtility.FromJson<VersoesApi>(json);

            if (versoes == null)
            {
                Debug.Log("Falha ao desserializar as versões.");
                yield break;
            }

            if (versoes.perguntas > DadosPlayer.GetVersaoPerguntas())
                yield return StartCoroutine(DownloadEAtualizar(nomeArquivoPerguntas, urlPerguntas, versoes.perguntas, DadosPlayer.SetVersaoPerguntas));

            if (versoes.exercicios > DadosPlayer.GetVersaoExercicios())
                yield return StartCoroutine(DownloadEAtualizar(nomeArquivoExercicios, urlExercicios, versoes.exercicios, DadosPlayer.SetVersaoExercicios));

            if (versoes.questionario > DadosPlayer.GetVersaoExercicios())
                yield return StartCoroutine(DownloadEAtualizar(nomeArquivoQuestionario, urlQuestionario, versoes.questionario, DadosPlayer.SetVersaoQuestionario));


            Debug.Log(versoes.questionario + " " + versoes.exercicios + " "+ versoes.perguntas);
            Debug.Log(DadosPlayer.GetVersaoExercicios()+ " " + DadosPlayer.GetVersaoExercicios()+ " " + DadosPlayer.GetVersaoPerguntas());
            
        }
        AtualizacaoTerminada = true;

    }

    private IEnumerator DownloadEAtualizar(string nomeArquivo, string url, int versaoNova, System.Action<int> setVersaoCallback)
    {
        Debug.Log("Estou a fazer download dp " +  nomeArquivo + " da versao " + versaoNova);

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
}
