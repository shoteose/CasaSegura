using System.Collections;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    public string nome;
    public int posicao;
    public int score;
    public Vector3 offset = new Vector3(0, 0, 0);

    protected MatchController matchController;
    protected BoardManager boardManager;

    public void Setup(MatchController controller, BoardManager board, Material material)
    {
        this.matchController = controller;
        this.boardManager = board;
        this.GetComponentInChildren<Renderer>().material = material;
    }
    public abstract void Jogar();

    protected void FimDoTurno()
    {
        matchController.TerminarTurno();
    }

    protected virtual void IniciarCoroutine(IEnumerator coroutine)
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
            objeto.transform.position = Vector3.Lerp(origem, destino + offset, t);
            yield return null;
        }
    }

}
