using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class QuestionarioUI : MonoBehaviour
{
    [Header("Referencias UI")]
    public RectTransform contentPanel;
    public GameObject prefabInputField;
    public GameObject prefabDropdown;

    private readonly Dictionary<string, string> respostas = new();

    public void CriarFormulario(Questionario questionario)
    {
        LimparFormulario();

        foreach (var pergunta in questionario.perguntas)
        {
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
    }

    private void LimparFormulario()
    {
        respostas.Clear();
        foreach (Transform child in contentPanel)
            Destroy(child.gameObject);
    }

    private void CriarInputTexto(PerguntaQuestionario pergunta)
    {
        var go = Instantiate(prefabInputField, contentPanel);

        go.GetComponentInChildren<TMP_Text>().text = pergunta.pergunta;

        var input = go.GetComponentInChildren<TMP_InputField>();
        input.onEndEdit.AddListener(valor => respostas[pergunta.pergunta] = valor);
    }

    private void CriarDropdown(PerguntaQuestionario pergunta)
    {
        var go = Instantiate(prefabDropdown, contentPanel);

        var textoPergunta = go.GetComponentInChildren<TMP_Text>();
        textoPergunta.text = pergunta.pergunta;

        var dropdown = go.GetComponentInChildren<TMP_Dropdown>();
        dropdown.ClearOptions();
        dropdown.AddOptions(pergunta.opcoes.ToList());

        dropdown.onValueChanged.AddListener(indice =>
        {
            respostas[pergunta.pergunta] = pergunta.opcoes[indice];
        });
    }

    public string GetResposta(string pergunta)
    {
        return respostas.TryGetValue(pergunta, out var resposta) ? resposta : null;
    }
        

    public bool TodasRespondidas(Questionario questionario) { 
        return questionario.perguntas.All(p =>
            respostas.ContainsKey(p.pergunta) &&
            !string.IsNullOrEmpty(respostas[p.pergunta]));
    }
    public Dictionary<string, string> GetTodasRespostas() { 
        return new Dictionary<string, string>(respostas);
    }
}