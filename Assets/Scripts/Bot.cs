using System.Collections;
using UnityEngine;

public class Bot : Player
{
    public override void Jogar()
    {
        int resultado = Random.Range(1, 7);
        StartCoroutine(MoverEResponder(resultado));
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
                Debug.Log($"[BOT] Casa especial: {tileAtual.exercicio}");
            }
            else
            {
                Debug.Log($"[BOT] Pergunta: {tileAtual.questao}");
                int index = Random.Range(0, tileAtual.respostas.Length);
                var resposta = tileAtual.respostas[index];
                Debug.Log($"[BOT] Respondeu: {resposta.texto} ({(resposta.correta ? "certo" : "x")})");

                if (resposta.correta)
                {
                    score++;

                    int proximaPosicao = posicao + 1;
                    Tile tileExtra = boardManager.GetTileNaPosicao(proximaPosicao);
                    if (tileExtra != null)
                    {
                        posicao = proximaPosicao;
                        Vector3 destinoExtra = tileExtra.transform.position;
                        yield return StartCoroutine(MoverPara(gameObject, destinoExtra));
                        yield return new WaitForSeconds(0.3f);
                    }
                }
            }
        }

        yield return new WaitForSeconds(1.5f);
        FimDoTurno();
    }
}
