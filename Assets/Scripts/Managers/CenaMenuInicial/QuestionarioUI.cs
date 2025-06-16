using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class QuestionarioUI : MonoBehaviour
{
    [Header("Referencias UI")]
    public RectTransform contentPanel;
    public GameObject textoPrivacidade;
    public GameObject botoes;
    public GameObject prefabInputField;
    public GameObject prefabDropdown;

    private readonly Dictionary<int, string> respostas = new();

    public void CriarFormulario(Questionario questionario)
    {
        LimparFormulario();

        Instantiate(textoPrivacidade, contentPanel);

        foreach (var pergunta in questionario.perguntas)
        {
            Debug.Log($"Pergunta : {pergunta.pergunta}; id: {pergunta.id}");
            switch (pergunta.tipo)
            {
                case "texto":
                    CriarInputTexto(pergunta);
                    break;
                case "dropdown":
                    CriarDropdown(pergunta);
                    break;
            }
        }

        //Instantiate(botoes, contentPanel);

    }

    private void LimparFormulario()
    {
        respostas.Clear();
        foreach (Transform child in contentPanel) Destroy(child.gameObject);
    }

    private void CriarInputTexto(PerguntaQuestionario pergunta)
    {
        var go = Instantiate(prefabInputField, contentPanel);
        Debug.Log(pergunta.id);
        go.GetComponentInChildren<TMP_Text>().text = pergunta.pergunta;

        var input = go.GetComponentInChildren<TMP_InputField>();
        input.onEndEdit.AddListener(valor => respostas[pergunta.id] = valor);
    }

    private void CriarDropdown(PerguntaQuestionario pergunta)
    {
        var go = Instantiate(prefabDropdown, contentPanel);

        var textoPergunta = go.GetComponentInChildren<TMP_Text>();
        textoPergunta.text = pergunta.pergunta;
        Debug.Log(pergunta.id + "No dropdown");
        var dropdown = go.GetComponentInChildren<TMP_Dropdown>();
        dropdown.ClearOptions();
        dropdown.AddOptions(pergunta.opcoes.ToList());

        dropdown.onValueChanged.AddListener(indice =>
        {
            respostas[pergunta.id] = pergunta.opcoes[indice];
        });
    }

    public string GetResposta(int pergunta)
    {
        return respostas.TryGetValue(pergunta, out var resposta) ? resposta : null;
    }
        

    public bool TodasRespondidas(Questionario questionario) { 
        return questionario.perguntas.All(p =>
            respostas.ContainsKey(p.id) &&
            !string.IsNullOrEmpty(respostas[p.id]));
    }
    public Dictionary<int, string> GetTodasRespostas() { 
        return new Dictionary<int, string>(respostas);
    }

    public int getCountRespostas()
    {
        return respostas.Count;
    }
}