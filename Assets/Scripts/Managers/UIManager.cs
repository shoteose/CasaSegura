using UnityEngine.UI;
using System;
using UnityEngine;
using TMPro;

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

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
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
                botoesResposta[i].GetComponentInChildren<Text>().text = respostas[i].texto;

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

    public void MostrarExercicio(string descricao)
    {
        Debug.Log($"[UI] Exibir exercício: {descricao}");
        // Aqui podes ativar painel com imagem ou instrução
    }

}
