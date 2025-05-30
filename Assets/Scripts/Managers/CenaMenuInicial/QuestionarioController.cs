using System.Collections;
using UnityEngine;

public class QuestionarioController : MonoBehaviour
{
    [Header("Refer�ncias UI")]
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
        Questionario loadedQuestionario = null;

        yield return QuestionarioLoader.LoadQuestionario("questionario", (q) => loadedQuestionario = q);

        questionario = loadedQuestionario;

        if (questionario == null)
            Debug.Log("Falha ao carregar question�rio.");

        AtualizarVisibilidade();
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

        // Atualiza vers�o ap�s responder
        DadosPlayer.SetVersaoQuestionario(questionario.versao);
        DadosPlayer.MarcarQuestionarioRespondido();

        panelQuestionario.SetActive(false);
    }

    public void ResetarQuestionario()
    {
        DadosPlayer.ResetarQuestionario();
        Debug.Log("Question�rio resetado.");

        AtualizarVisibilidade();
    }

    // M�todo para quando receberes nova vers�o da API
    public void VerificarNovaVersao(int versaoAPI)
    {
        if (questionario != null && questionario.versao < versaoAPI)
        {
            // Carrega nova vers�o do question�rio
            CarregarQuestionario();
            AtualizarVisibilidade();
            Debug.Log($"Nova vers�o do question�rio dispon�vel: {versaoAPI}");
        }
    }
}
