using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class BoardManager : MonoBehaviour
{

    public int totalTiles = 35;
    [SerializeField]
    private Tile[] tabuleiro;
    public Transform TabuleiroPai;
    public List<int> casasEspeciais;
    private List<Pergunta> perguntasDisponiveis;
    private List<Exercicio> exerciciosDisponiveis;

    public bool SetupConcluido { get; private set; } = false;


    void Start()
    {

        StartCoroutine(Setup());

    }

    private IEnumerator Setup()
    {
        Perguntas perguntasArray = null;
        yield return GenericLoader.Load<Perguntas>(ApiManager.nomeArquivoPerguntas, p => perguntasArray = p);
        perguntasDisponiveis = new List<Pergunta>(perguntasArray.perguntas);

        Exercicios exerciciosArray = null;
        yield return GenericLoader.Load<Exercicios>(ApiManager.nomeArquivoExercicios, e => exerciciosArray = e);
        exerciciosDisponiveis = new List<Exercicio>(exerciciosArray.exercicios);


        float timeout = 10f; 
        float timer = 0f;

        while (!ApiManager.AtualizacaoTerminada && ApiManager.net && timer < timeout)
        {
            Debug.Log("esperando...");
            timer += Time.deltaTime;
            yield return null;
        }

        if (!ApiManager.AtualizacaoTerminada)
        {
            Debug.LogWarning("Setup da API não terminou dentro do tempo. Continuando com dados locais.");
        }


        casasEspeciais = new List<int>();
        int nrCasasEspeciais;
        if (ApiManager.net)
        {
            nrCasasEspeciais = Random.Range(6, (totalTiles / 4));
            //nrCasasEspeciais = 30;
        }
        else
        {

            nrCasasEspeciais = 0;
        }

        while (casasEspeciais.Count < nrCasasEspeciais)
        {
            int x = Random.Range(1, totalTiles + 1);

            if (!casasEspeciais.Contains(x)) casasEspeciais.Add(x);
        }

        tabuleiro = new Tile[totalTiles + 1];

        int counter = 0;
        foreach (Transform tile in TabuleiroPai)
        {

            Tile tileComponent = tile.GetComponent<Tile>();
            if (tileComponent != null)
            {
                if (casasEspeciais.Contains(counter))
                {
                    tileComponent.especial = true;

                    if (exerciciosDisponiveis.Count > 0)
                    {
                        int index = Random.Range(0, exerciciosDisponiveis.Count);
                        var exercicio = exerciciosDisponiveis[index];
                        tileComponent.DefinirExercicio(exercicio);
                        exerciciosDisponiveis.RemoveAt(index);
                    }
                }
                else
                {
                    if (perguntasDisponiveis.Count > 0)
                    {
                        int index = Random.Range(0, perguntasDisponiveis.Count);
                        var pergunta = perguntasDisponiveis[index];
                        tileComponent.DefinirPergunta(pergunta);
                        perguntasDisponiveis.RemoveAt(index);
                    }
                }

                tabuleiro[counter] = tileComponent;
                counter++;
            }

        }


        foreach (Transform tile in TabuleiroPai)
        {

            Tile tileComponent = tile.GetComponent<Tile>();
            if (tileComponent.especial)
            {
                if (casasEspeciais.Contains(counter))
                {
                    Debug.Log("sou especial no indice: " + counter);
                }


            }
        }

        SetupConcluido = true;

    }

    public Tile GetTileNaPosicao(int pos)
    {
        if (pos >= 0 && pos < tabuleiro.Length)
            return tabuleiro[pos];

        return null;
    }

}
