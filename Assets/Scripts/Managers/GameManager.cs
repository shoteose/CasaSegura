using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public Player[] players;
    private int currentTurn = 0;
    private int totalTiles = 35;
    [SerializeField]
    private Tile[] tabuleiro;
    public Transform TabuleiroPai;


    void Start()
    {

        criarTabuleiro();
    }

    void criarTabuleiro()
    {
        tabuleiro = new Tile[totalTiles];

        int counter = 0;
        foreach (Transform tile in TabuleiroPai)
        {

            Tile tileComponent = tile.GetComponent<Tile>();
            if (tileComponent != null)
            {
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
