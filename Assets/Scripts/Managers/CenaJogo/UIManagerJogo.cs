using UnityEngine.UI;
using System;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;

public class UIManagerJogo : MonoBehaviour
{
    public static UIManagerJogo Instance;

    [SerializeField] private GameObject loadingImage;

    [Header("Paienl Botões")]
    [SerializeField] private GameObject painelLancarDado;
    [SerializeField] private Button botaoLancarDado;

    [Header("Painel Pausa")]
    [SerializeField] private Button botaoPausa;
    [SerializeField] private GameObject painelPausa;
    [SerializeField] private Button botaoContinuar;
    [SerializeField] private Button botaoMenu;

    [Header("Dados")]
    [SerializeField] private RawImage dado;
    [SerializeField] private Texture[] imagensDados;

    [Header("Painel Exercicios")]
    [SerializeField] private GameObject painelExercicios;
    [SerializeField] private TextMeshProUGUI textoExercicio;
    [SerializeField] private RawImage imagemExercicio;
    [SerializeField] private Texture noWifi;
    private Texture downlaodedTexture;

    [Header("Painel GameOver")]
    [SerializeField] private GameObject painelGameOver;
    [SerializeField] private TextMeshProUGUI textoGameOver;
    [SerializeField] private Button[] botoesGameOver;
    

    [Header("Pergunta")]
    [SerializeField] private GameObject painelPergunta;
    [SerializeField] private TextMeshProUGUI textoPergunta;
    [SerializeField] private TextMeshProUGUI textoTempo;
    [SerializeField] private Button[] botoesResposta;

    [Header("GameState")]
    [SerializeField] private GameObject HolderTextoTurno;



    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        painelPergunta.SetActive(false);
        painelExercicios.SetActive(false);
        HolderTextoTurno.SetActive(false);
        painelLancarDado.SetActive(false);
        painelGameOver.SetActive(false);
        painelPausa.SetActive(false);
        botaoPausa.onClick.AddListener(AbrirPausa);


    }

    public void StopLoading()
    {

        loadingImage.SetActive(false);

    }
    public void MostrarBotaoLancarDado(Action onClick)
    {
        painelLancarDado.SetActive(true);

        botaoLancarDado.onClick.RemoveAllListeners();

        botaoLancarDado.onClick.AddListener(() =>
        {
            onClick?.Invoke();
        });
    }

    public IEnumerator EditarTextoTurno(string mens)
    {
        HolderTextoTurno.SetActive(true);
        HolderTextoTurno.GetComponentInChildren<TextMeshProUGUI>().text = mens;

        yield return new WaitForSeconds(1f);
        HolderTextoTurno.SetActive(false);
    }

    public void MostrarPainelGameOver(Player player)
    {
        Debug.Log("TA AQUI CARALHO");
        painelGameOver.SetActive(true);
        textoGameOver.text = $"Parabéns o jogador {player.nome} ganhou!!";
       
    }

    private void AbrirPausa()
    {
        Time.timeScale = 0f;
        painelPausa.SetActive(true);

        botaoContinuar.onClick.RemoveAllListeners();
        botaoContinuar.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            painelPausa.SetActive(false);
        });

        botaoMenu.onClick.RemoveAllListeners();
        botaoMenu.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            VoltarMenuPrincipal();
        });
    }


    public void RestartGame()
    {
        GameControllor.Instance.RestartGame();

    }

    public void VoltarMenuPrincipal()
    {
        GameControllor.Instance.VoltarMenuPrincipal();
    }

    public IEnumerator RodarDado(int valor)
    {
        int numeroGiros = UnityEngine.Random.Range(8, 16);
        int totalFaces = imagensDados.Length;

        for (int i = 0; i < numeroGiros; i++)
        {
            int faceAleatoria = UnityEngine.Random.Range(0, totalFaces);
            dado.texture = imagensDados[faceAleatoria];
            yield return new WaitForSeconds(0.1f);
        }

        dado.texture = imagensDados[valor - 1];
        yield return new WaitForSeconds(1.25f);
        painelLancarDado.SetActive(false);
    }


    public void MostrarPergunta(string pergunta, Resposta[] respostas, Action<Resposta> aoResponder)
    {
        painelPergunta.SetActive(true);
        textoPergunta.text = pergunta;

        for (int i = 0; i < botoesResposta.Length; i++)
        {
            if (i < respostas.Length)
            {
                botoesResposta[i].gameObject.SetActive(true);
                botoesResposta[i].GetComponentInChildren<TextMeshProUGUI>().text = respostas[i].texto;

                int idx = i;
                botoesResposta[i].onClick.RemoveAllListeners();
                botoesResposta[i].onClick.AddListener(() =>
                {
                    painelPergunta.SetActive(false);
                    aoResponder?.Invoke(respostas[idx]);
                });
            }
            else
            {
                botoesResposta[i].gameObject.SetActive(false);
            }
        }
    }

    public IEnumerator MostrarTextoTurno(Player playerVez)
    {
        HolderTextoTurno.SetActive(true);
        HolderTextoTurno.GetComponentInChildren<TextMeshProUGUI>().text = "É a vez do " + playerVez.nome;
        yield return new WaitForSeconds(1.5f);
        HolderTextoTurno.SetActive(false);
    }

    public IEnumerator MostrarExercicio(string descricao, string url)
    {
        Debug.Log($"[UI] Exibir exercício: {descricao}");
       
        yield return StartCoroutine(downloadImagemFromUrl(url));

        painelExercicios.SetActive(true);
        textoExercicio.text = descricao;

        for (int i = 1; i <= 10; i++)
        {
            while (Time.timeScale == 0f)
                yield return null;

            textoTempo.text = $"{11 - i} segundos";
            yield return new WaitForSecondsRealtime(1f);
        }


        painelExercicios.SetActive(false);

    }

    private IEnumerator downloadImagemFromUrl(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success) {

            downlaodedTexture = DownloadHandlerTexture.GetContent(www);

            Texture textura = downlaodedTexture;

            imagemExercicio.texture = textura;
            Debug.Log("deu");
        }
        else
        {

            imagemExercicio.texture = noWifi;
            Debug.Log($"Erro a fazer o download {url}");
        }


    }

}
