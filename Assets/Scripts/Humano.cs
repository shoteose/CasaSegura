using System.Collections;
using UnityEngine;

public class Humano : Player
{
    public int index;
    public bool jogou = false;
    public override void Jogar()
    {
        UIManagerJogo.Instance.MostrarBotaoLancarDado(() =>
        {
            if (!jogou)
            {
                this.jogou = true;
                int resultado = Random.Range(1, 7);
                Debug.Log("[Humano " + this.nome + " ] Lancou o dado com " + resultado);
                //StartCoroutine(UIManagerJogo.Instance.RodarDado(resultado));

                StartCoroutine(MoverEResponder(resultado));
            }

        });
    }

    private IEnumerator MoverEResponder(int passos)
    {

        yield return StartCoroutine(UIManagerJogo.Instance.RodarDado(passos));

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
                yield return StartCoroutine(UIManagerJogo.Instance.MostrarExercicio(tileAtual.exercicio, tileAtual.url_imagem));
                //yield return new WaitForSeconds(1.5f);
                FimDoTurno();
            }
            else
            {
                UIManagerJogo.Instance.MostrarPergunta(tileAtual.questao, tileAtual.respostas, (respostaEscolhida) =>
                {
                    if (respostaEscolhida.correta)
                    {
                        score++;
                        StartCoroutine(AvancarMaisUmaCasaSePossivel());
                    }
                    else
                    {
                        FimDoTurno();
                    }
                });

                yield break; // aguarda resposta
            }
        }
        else
        {
            yield return new WaitForSeconds(1f);
            FimDoTurno();
        }
    }

    protected new virtual void FimDoTurno() {

        matchController.TerminarTurno();
        this.jogou = false;
    }

    private IEnumerator AvancarMaisUmaCasaSePossivel()
    {
        int proximaPosicao = posicao + 1;
        Tile tileExtra = boardManager.GetTileNaPosicao(proximaPosicao);
        if (tileExtra != null)
        {
            posicao = proximaPosicao;
            Vector3 destinoExtra = tileExtra.transform.position;
            yield return StartCoroutine(MoverPara(gameObject, destinoExtra));
            //yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(1f);
        FimDoTurno();
    }
}
