using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchController : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private BoardManager boardManager;

    [Header("Turnos")]
    private Player[] players;
    private int currentTurn = 0;

    [Header("Materiais")]
    [SerializeField] private Material[] materiaisPlayers;

    [Header("Offset")]
    [SerializeField] private float raio = 0.6f;

    private void Start()
    {
        StartCoroutine(setup());
    }

    private IEnumerator setup()
    {
        yield return new WaitForSeconds(2f);

        yield return new WaitUntil(() => boardManager.SetupConcluido);

        List<Player> jogadores = GameControllor.Instance.InstanciarJogadores();
        players = jogadores.ToArray();


        Tile tile0 = boardManager.GetTileNaPosicao(0);
        Vector3 basePos = tile0.transform.position;

        for (int i = 0; i < players.Length; i++)
        {
            Vector3 offset = CalcularOffset(players.Length, i);
            players[i].offset = offset;
            players[i].transform.position = basePos + offset;
            players[i].Setup(this, boardManager, materiaisPlayers[players[i].indiceCor]);
        }

        UIManagerJogo.Instance.StopLoading();

        StartCoroutine(IniciarTurnoCoroutine());
    }
    private Vector3 CalcularOffset(int total, int index)
    {
        float angulo = ((360f / total) * index) + 45;
        return new Vector3(Mathf.Cos(angulo * Mathf.Deg2Rad), 0, Mathf.Sin(angulo * Mathf.Deg2Rad)) * this.raio;
        
    }


    private IEnumerator IniciarTurnoCoroutine()
    {
        Player jogadorAtual = players[currentTurn];
        yield return StartCoroutine(UIManagerJogo.Instance.MostrarTextoTurno(jogadorAtual));
        jogadorAtual.Jogar();
    }

    public void TerminarTurno()
    {
        Player jogadorAtual = players[currentTurn];
        if (jogadorAtual.posicao >= boardManager.totalTiles)
        {
            GameOver(jogadorAtual);
            return;
        }

        currentTurn = (currentTurn + 1) % players.Length;
        StartCoroutine(IniciarTurnoCoroutine());
    }

    private void GameOver(Player vencedor)
    {
        Debug.Log($"Jogo terminado! Vencedor: {vencedor.nome}");
  
        MostrarGameOverUI(vencedor);
    }

    private void MostrarGameOverUI(Player vencedor)
    {

       UIManagerJogo.Instance.MostrarPainelGameOver(vencedor);
        
        
    }

}
