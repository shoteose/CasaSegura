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
    private bool net;
    public bool SetupConcluido { get; private set; } = false;


    void Start()
    {

        StartCoroutine(Setup());

    }

    private IEnumerator Setup()
    {
        while (!ApiManager.AtualizacaoTerminada)
            yield return null;

        yield return StartCoroutine(VerificarInternet());

        casasEspeciais = new List<int>();
        int nrCasasEspeciais;
        if (net)
        {
            nrCasasEspeciais = Random.Range(4, (totalTiles / 4));
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
                    //Debug.Log("sou especial no indice: " + counter);
                }

                tileComponent.Questoes();
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
    private IEnumerator VerificarInternet()
    {
        UnityWebRequest request = new UnityWebRequest("https://google.com");

        yield return request.SendWebRequest();

        if (request.error != null)
        {
            net = false;
        }
        else
        {
            net = true;
        }
    }

    public Tile GetTileNaPosicao(int pos)
    {
        if (pos >= 0 && pos < tabuleiro.Length)
            return tabuleiro[pos];

        return null;
    }

}
