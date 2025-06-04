using System.Collections;
using UnityEngine;

public class QuestionarioController : MonoBehaviour
{
    [Header("Referências UI")]
    public GameObject botaoQuestionario;
    public GameObject botaoEnviar;

    public GameObject panelQuestionario;
    public QuestionarioUI questionarioUI;

    private Questionario questionario;

    void Start()
    {
        StartCoroutine(CarregarQuestionario());
    }

    private IEnumerator CarregarQuestionario()
    {
        yield return GenericLoader.Load<Questionario>(ApiManager.nomeArquivoQuestionario, q =>
        {
            questionario = q;

            if (questionario == null)
                Debug.Log("Falha ao carregar questionário.");

            AtualizarVisibilidade();
        });
    }



    private void Update()
    {
        if (questionario == null)
        {
            botaoEnviar.SetActive(false);
            return;
        }

        botaoEnviar.SetActive(questionarioUI.TodasRespondidas(questionario));
    }

    private void AtualizarVisibilidade()
    {
        if (questionario == null)
        {
            botaoQuestionario.SetActive(false);
            panelQuestionario.SetActive(false);
            return;
        }

        bool deveExibir = DadosPlayer.DeveExibirQuestionario(questionario.versao);
        botaoQuestionario.SetActive(deveExibir);
        panelQuestionario.SetActive(false);
    }

    public void AbrirQuestionario()
    {
        if (questionario == null) return;

        botaoQuestionario.SetActive(false);
        panelQuestionario.SetActive(true);
        questionarioUI.CriarFormulario(questionario);
    }

    public void FecharQuestionario()
    {
        if (questionario == null) return;

        panelQuestionario.SetActive(false);
        botaoQuestionario.SetActive(true);
   
    }

    public void EnviarRespostas()
    {
        if (questionario == null || !questionarioUI.TodasRespondidas(questionario))
        {
            Debug.LogWarning("Por favor responda todas as perguntas.");
            return;
        }

        // TODO: Implementar envio para API
        Debug.Log("Enviando respostas...");

        // Atualiza versão após responder
        DadosPlayer.SetVersaoQuestionario(questionario.versao);
        DadosPlayer.MarcarQuestionarioRespondido();

        panelQuestionario.SetActive(false);
    }

    public void ResetarQuestionario()
    {
        JsonFileManager.DeleteJson("perguntas");
        JsonFileManager.DeleteJson("exercicios");
        JsonFileManager.DeleteJson("questionario");

        // Também podes resetar versões locais, se quiseres
        DadosPlayer.SetVersaoPerguntas(0);
        DadosPlayer.SetVersaoExercicios(0);
        DadosPlayer.SetVersaoQuestionario(0);

        DadosPlayer.ResetarQuestionario();
        Debug.Log("Questionário resetado.");

        AtualizarVisibilidade();
    }

    // Método para quando receberes nova versão da API
    public void VerificarNovaVersao(int versaoAPI)
    {
        if (questionario != null && questionario.versao < versaoAPI)
        {
            // Carrega nova versão do questionário
            CarregarQuestionario();
            AtualizarVisibilidade();
            Debug.Log($"Nova versão do questionário disponível: {versaoAPI}");
        }
    }
}
