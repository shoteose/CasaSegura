using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchController : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private UIManager uiManager;

    [Header("Turnos")]
    private Player[] players;
    private int currentTurn = 0;

    private void Start()
    {
        List<Player> jogadores = GameControllor.Instance.InstanciarJogadores();
        players = jogadores.ToArray();

        Tile tile0 = boardManager.GetTileNaPosicao(0);
        Vector3 basePos = tile0.transform.position;

        for (int i = 0; i < players.Length; i++)
        {
            Vector3 offset = CalcularOffset(players.Length, i);
            players[i].offset = offset;
            players[i].transform.position = basePos + offset;
            players[i].Setup(this, boardManager);
        }

        IniciarTurno();
    }

    private Vector3 CalcularOffset(int total, int index)
    {
        float espaco = 0.25f;

        switch (total)
        {
            case 1:
                return Vector3.zero;

            case 2:
                return new Vector3((index == 0 ? -espaco : espaco), 0, 0);

            case 3:
            case 4:
                Vector3[] posicoes = new Vector3[]
                {
                new Vector3(-espaco, 0, espaco),
                new Vector3(espaco, 0, espaco),
                new Vector3(-espaco, 0, -espaco),
                new Vector3(espaco, 0, -espaco)
                };
                return posicoes[index];

            default:
                float ang = (360f / total) * index;
                float radius = 0.8f;
                return new Vector3(Mathf.Cos(ang * Mathf.Deg2Rad), 0, Mathf.Sin(ang * Mathf.Deg2Rad)) * radius;
        }
    }


    private void IniciarTurno()
    {
        Player jogadorAtual = players[currentTurn];
        jogadorAtual.Jogar();
    }

    public void TerminarTurno()
    {
        currentTurn = (currentTurn + 1) % players.Length;
        IniciarTurno();
    }


}
