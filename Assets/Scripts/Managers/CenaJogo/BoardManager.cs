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
    public bool SetupConcluido { get; private set; } = false;


    void Start()
    {

        StartCoroutine(Setup());

    }

    private IEnumerator Setup()
    {

        while (!ApiManager.AtualizacaoTerminada && ApiManager.net)
        {
            Debug.Log("esperando");
            yield return null;
        }


        casasEspeciais = new List<int>();
        int nrCasasEspeciais;
        if (ApiManager.net)
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

    public Tile GetTileNaPosicao(int pos)
    {
        if (pos >= 0 && pos < tabuleiro.Length)
            return tabuleiro[pos];

        return null;
    }

}
