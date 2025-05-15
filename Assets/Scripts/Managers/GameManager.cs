using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public Player[] players;
    private int currentTurn = 0;
    private int totalTiles = 35;
    [SerializeField]
    private Tile[] tabuleiro;
    public Transform TabuleiroPai;
    public List<int> casasEspeciais; 


    void Start()
    {

        criarTabuleiro();
    }

    void criarTabuleiro()
    {
        casasEspeciais = new List<int>();
        int nrCasasEspeciais = Random.Range(1, (totalTiles / 3));

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
                    Debug.Log("sou especial no indice: " + counter);
                }

                tileComponent.Questoes();
                tabuleiro[counter] = tileComponent;
                

                counter++;
            }
        }

        foreach (Tile tile in tabuleiro)
        {

            Debug.Log(tile.questao + "  :::" + tile.respostaCorreta);
        }
    }

}
