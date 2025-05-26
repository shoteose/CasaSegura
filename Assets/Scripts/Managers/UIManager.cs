using UnityEngine.UI;
using System;
using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Botões")]
    [SerializeField] private GameObject painelLancarDado;
    [SerializeField] private Button botaoLancarDado;

    [Header("Pergunta")]
    [SerializeField] private GameObject painelPergunta;
    [SerializeField] private TextMeshProUGUI textoPergunta;
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
        HolderTextoTurno.SetActive(false);
        
    }
    public void MostrarBotaoLancarDado(Action onClick)
    {
        painelLancarDado.SetActive(true);

        botaoLancarDado.onClick.RemoveAllListeners();

        botaoLancarDado.onClick.AddListener(() =>
        {
            painelLancarDado.SetActive(false);
            onClick?.Invoke();
        });
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

    public void MostrarExercicio(string descricao)
    {
        Debug.Log($"[UI] Exibir exercício: {descricao}");
        // ATIVAR PAINEL NESTEA PARTR
    }

}
