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
    private bool aguardandoJogada = false;

    void Start()
    {
        players = GameControllor.Instance.GetPlayers();
        IniciarTurno();
    }

    void Update()
    {
        if (aguardandoJogada)
        {
            Player jogadorAtual = players[currentTurn];

            if (jogadorAtual is Humano humano)
            {
                // Aqui esperas pelo input do jogador
                // Por exemplo, botão na UI chama MatchController.LancarDado()
            }
            else if (jogadorAtual is Bot bot)
            {
                JogarAutomaticamente(bot);
            }
        }
    }

    public void LancarDado()
    {
        if (!aguardandoJogada) return;

        Player jogadorAtual = players[currentTurn];
        if (!(jogadorAtual is Humano)) return;

        aguardandoJogada = false;

        int resultado = Random.Range(1, 7);
        Debug.Log($"{jogadorAtual.nome} lançou o dado e vai andar {resultado} casas.");

        StartCoroutine(MoverJogador(jogadorAtual, resultado));
    }



    void IniciarTurno()
    {
        Player jogadorAtual = players[currentTurn];
        Debug.Log($"É o turno de {jogadorAtual.nome}");

        aguardandoJogada = true;
    }

    public void TerminarTurno()
    {
        currentTurn = (currentTurn + 1) % players.Length;
        IniciarTurno();
    }

    private void JogarAutomaticamente(Bot bot)
    {
        aguardandoJogada = false;

        int resultado = Random.Range(1, 7);
        Debug.Log($"Bot {bot.nome} lançou o dado e vai andar {resultado} casas.");

        StartCoroutine(MoverBotEResponder(bot, resultado));
    }


    private IEnumerator MoverBotEResponder(Bot bot, int passos)
    {
        for (int i = 0; i < passos; i++)
        {
            bot.posicao++;
            Tile tileDestino = boardManager.GetTileNaPosicao(bot.posicao);
            if (tileDestino != null)
            {
                Vector3 destino = tileDestino.transform.position;
                yield return StartCoroutine(MoverPara(bot.gameObject, destino));
            }
            yield return new WaitForSeconds(0.3f);
        }

        // Depois de se mover, interage com o tile
        Tile tileAtual = boardManager.GetTileNaPosicao(bot.posicao);
        if (tileAtual != null)
        {
            if (tileAtual.especial)
            {
                Debug.Log($"Bot chegou a uma casa especial: {tileAtual.exercicio}");
            }
            else
            {
                Debug.Log($"Bot chegou à pergunta: {tileAtual.questao}");

                // Escolhe aleatoriamente uma resposta
                int randomIndex = Random.Range(0, tileAtual.respostas.Length);
                Resposta respostaEscolhida = tileAtual.respostas[randomIndex];

                Debug.Log($"Bot respondeu: {respostaEscolhida.texto}");

                if (respostaEscolhida.correta)
                {
                    Debug.Log("Bot acertou!");
                    bot.score++;
                }
                else
                {
                    Debug.Log("Bot errou!");
                }
            }
        }

        // Espera e termina turno
        yield return new WaitForSeconds(2f);
        TerminarTurno();
    }



    private IEnumerator MoverJogador(Player jogador, int passos)
    {
        for (int i = 0; i < passos; i++)
        {
            int novaPos = jogador.posicao + 1;
            Tile tileDestino = boardManager.GetTileNaPosicao(novaPos);

            if (tileDestino != null)
            {
                jogador.posicao = novaPos;
                yield return StartCoroutine(MoverPara(jogador.gameObject, tileDestino.transform.position));
            }
            else
            {
                Debug.LogWarning("Fim do tabuleiro ou tile inválido!");
                break;
            }

            yield return new WaitForSeconds(0.3f);
        }

        Tile tileAtual = boardManager.GetTileNaPosicao(jogador.posicao);
        uiManager.MostrarPerguntaOuExercicio(tileAtual);
    }

    private IEnumerator MoverPara(GameObject objeto, Vector3 destino)
    {
        float t = 0;
        Vector3 origem = objeto.transform.position;
        while (t < 1)
        {
            t += Time.deltaTime * 2; // velocidade
            objeto.transform.position = Vector3.Lerp(origem, destino, t);
            yield return null;
        }
    }


}
