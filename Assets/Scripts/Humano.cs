using System.Collections;
using UnityEngine;

public class Humano : Player
{
    public override void Jogar()
    {
        UIManager.Instance.MostrarBotaoLancarDado(() =>
        {
            int resultado = Random.Range(1, 7);
            StartCoroutine(MoverEResponder(resultado));
        });
    }

    private IEnumerator MoverEResponder(int passos)
    {
        for (int i = 0; i < passos; i++)
        {
            posicao++;
            Tile tileDestino = boardManager.GetTileNaPosicao(posicao);
            if (tileDestino != null)
            {
                Vector3 destino = tileDestino.transform.position;
                yield return StartCoroutine(MoverPara(gameObject, destino));
            }
            yield return new WaitForSeconds(0.3f);
        }

        Tile tileAtual = boardManager.GetTileNaPosicao(posicao);
        if (tileAtual != null)
        {
            if (tileAtual.especial)
            {
                UIManager.Instance.MostrarExercicio(tileAtual.exercicio);
            }
            else
            {
                UIManager.Instance.MostrarPergunta(tileAtual.questao, tileAtual.respostas, (respostaEscolhida) =>
                {
                    if (respostaEscolhida.correta)
                    {
                        score++;
                    }

                    FimDoTurno();
                });
                yield break; // espera resposta antes de terminar o turno
            }
        }

        yield return new WaitForSeconds(1.5f);
        FimDoTurno();
    }
}
