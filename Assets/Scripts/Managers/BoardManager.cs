using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

    private int totalTiles = 35;
    [SerializeField]
    private Tile[] tabuleiro;
    public Transform TabuleiroPai;
    public List<int> casasEspeciais;

    void Start()
    {
        casasEspeciais = new List<int>();
        int nrCasasEspeciais = Random.Range(4, (totalTiles / 4));

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

        foreach (Tile tile in tabuleiro)
        {
            if (tile.especial)
            {
                //Debug.Log(tile.exercicio + "  :::" + tile.url_imagem);

            }
            else
            {
                //Debug.Log(tile.questao + "  :::" + tile.respostaCorreta);
            }
        }


    }


    public void Imprime()
    {
        foreach (Tile tile in tabuleiro)
        {
            string mens;
            mens = tile.questao;
            if (tile.especial) mens = tile.exercicio;

            Debug.Log(mens);
        }
    }

    public Tile GetTileNaPosicao(int pos)
    {
        if (pos >= 0 && pos < tabuleiro.Length)
            return tabuleiro[pos];

        return null;
    }

}
