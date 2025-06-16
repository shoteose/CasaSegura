using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class QuestionarioController : MonoBehaviour
{
    [Header("Referências UI")]
    public GameObject botaoQuestionario;
    //public GameObject botaoEnviarPai;
    public GameObject botaoEnviar;

    public GameObject panelQuestionario;
    public QuestionarioUI questionarioUI;

    private Questionario questionario;

    [Header("Referência API MANAGER")]
    [SerializeField] private ApiManager apiManager; 


    void Start()
    {
        StartCoroutine(CarregarQuestionario());

       // Button[] botoesFilhos = botaoEnviarPai.GetComponentsInChildren<Button>();
       // botaoEnviar = botoesFilhos[0].gameObject;
    }

    private IEnumerator CarregarQuestionario()
    {
        yield return GenericLoader.Load<Questionario>(ApiManager.nomeArquivoQuestionario, q =>
        {
            questionario = q;
            Debug.Log(questionario.perguntas[0].id + " ----------");
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

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            botaoQuestionario.SetActive(false);
            panelQuestionario.SetActive(false);

        }
        else
        {

            //bool deveExibir = DadosPlayer.DeveExibirQuestionario(questionario.versao);
            botaoQuestionario.SetActive(true);
            panelQuestionario.SetActive(false);

        }


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
            Debug.LogWarning("Falta responder");
            return;
        }
        //envio das respostas
        Debug.Log("Enviando respostas...");

        DadosPlayer.SetVersaoQuestionario(questionario.versao);
        DadosPlayer.MarcarQuestionarioRespondido();
        //Debug.Log(questionarioUI.GetTodasRespostas());
        RespostasQuestionario respostasQuestionario = new()
        {
            utilizador_id = System.Guid.NewGuid().ToString(),
            respostas = questionarioUI.GetTodasRespostas()
        };

        foreach (var item in respostasQuestionario.respostas)
        {
            RespostaQuestionario rq = new()
            {
                utilizador_id = respostasQuestionario.utilizador_id,
                pergunta_id = item.Key,
                resposta_texto = item.Value
            };

            StartCoroutine(apiManager.EnviarResposta(rq));

        }
        panelQuestionario.SetActive(false);

    }

    public void ResetarQuestionario()
    {
        JsonFileManager.DeleteJson("perguntas");
        JsonFileManager.DeleteJson("exercicios");
        JsonFileManager.DeleteJson("questionario");

        DadosPlayer.SetVersaoPerguntas(0);
        DadosPlayer.SetVersaoExercicios(0);
        DadosPlayer.SetVersaoQuestionario(0);

        DadosPlayer.ResetarQuestionario();
        Debug.Log("Questionário resetado.");

        AtualizarVisibilidade();
    }

}
