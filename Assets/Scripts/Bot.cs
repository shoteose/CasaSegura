using System.Collections;
using UnityEngine;

public class Bot : Player
{
    public override void Jogar()
    {
        int resultado = Random.Range(1, 7);
        Debug.Log(resultado);
        
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
                Debug.Log($"[BOT] Respondeu: {resposta.texto}");

                if (resposta.correta)
                {
                    
                    StartCoroutine(UIManagerJogo.Instance.EditarTextoTurno($"O jogador {this.nome} acertou!"));
                    score++;

                    int proximaPosicao = posicao + 1;
                    Tile tileExtra = boardManager.GetTileNaPosicao(proximaPosicao);
                    if (tileExtra != null)
                    {
                        posicao = proximaPosicao;
                        Vector3 destinoExtra = tileExtra.transform.position;
                        yield return new WaitForSeconds(1f);
                        yield return StartCoroutine(MoverPara(gameObject, destinoExtra));
                        yield return new WaitForSeconds(0.3f);
                    }
                }
                else
                {
                    StartCoroutine(UIManagerJogo.Instance.EditarTextoTurno($"O jogador {this.nome} errou!"));
                }

            }
        }

        yield return new WaitForSeconds(1f);
        FimDoTurno();
    }
}
