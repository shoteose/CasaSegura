using System.Collections;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    public string nome;
    public int posicao;
    public int score;

    protected MatchController matchController;
    protected BoardManager boardManager;

    public void Setup(MatchController controller, BoardManager board)
    {
        this.matchController = controller;
        this.boardManager = board;
    }

    public abstract void Jogar();

    protected void FimDoTurno()
    {
        matchController.TerminarTurno();
    }

    protected void IniciarCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }


    protected IEnumerator MoverPara(GameObject objeto, Vector3 destino)
    {
        float t = 0;
        Vector3 origem = objeto.transform.position;

        while (t < 1)
        {
            t += Time.deltaTime * 2f;
            objeto.transform.position = Vector3.Lerp(origem, destino, t);
            yield return null;
        }
    }

}
