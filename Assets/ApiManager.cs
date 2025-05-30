using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ApiManager : MonoBehaviour
{
    private const string urlVersoes = "https://tuaapi.com/api/versoes";

    private const string nomeArquivoPerguntas = "perguntas.json";
    private const string nomeArquivoExercicios = "exercicios.json";
    private const string nomeArquivoQuestionario = "questionario.json";

    private const string urlPerguntas = "https://tuaapi.com/api/perguntas/json";
    private const string urlExercicios = "https://tuaapi.com/api/exercicios/json";
    private const string urlQuestionario = "https://tuaapi.com/api/questionario/json";

    void Start()
    {
        StartCoroutine(VerificarAtualizacoes());
    }

    private IEnumerator VerificarAtualizacoes()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.LogWarning("Sem ligação à internet. Usando dados locais.");
            yield break;
        }

        using (UnityWebRequest request = UnityWebRequest.Get(urlVersoes))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Falha a obter versões da API: " + request.error);
                yield break;
            }

            var json = request.downloadHandler.text;
            var versoes = JsonUtility.FromJson<VersoesApi>(json);

            if (versoes == null)
            {
                Debug.Log("Falha ao desserializar versões da API.");
                yield break;
            }

            if (versoes.versaoPerguntas > DadosPlayer.GetVersaoPerguntas())
            {
                yield return StartCoroutine(DownloadEAtualizar(nomeArquivoPerguntas, urlPerguntas, versoes.versaoPerguntas, DadosPlayer.SetVersaoPerguntas));
            }

            if (versoes.versaoExercicios > DadosPlayer.GetVersaoExercicios())
            {
                yield return StartCoroutine(DownloadEAtualizar(nomeArquivoExercicios, urlExercicios, versoes.versaoExercicios, DadosPlayer.SetVersaoExercicios));
            }

            if (versoes.versaoQuestionario > DadosPlayer.GetVersaoQuestionario())
            {
                yield return StartCoroutine(DownloadEAtualizar(nomeArquivoQuestionario, urlQuestionario, versoes.versaoQuestionario, DadosPlayer.SetVersaoQuestionario));
            }
        }
    }

    private IEnumerator DownloadEAtualizar(string nomeArquivo, string url, int versaoNova, System.Action<int> setVersaoCallback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log($"Falha ao baixar {nomeArquivo}: {request.error}");
                yield break;
            }

            string json = request.downloadHandler.text;
            JsonFileManager.SaveJson(nomeArquivo, json);
            setVersaoCallback?.Invoke(versaoNova);
            Debug.Log($"{nomeArquivo} atualizado para versão {versaoNova}.");
        }
    }
}
