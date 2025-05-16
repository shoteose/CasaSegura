using System.Collections;
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
        players = GameControllor.Instance.GetPlayers();

        foreach (var p in players)
        {
            p.Setup(this, boardManager);
        }

        IniciarTurno();
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
