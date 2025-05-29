using System.Collections;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    public string nome;
    public int posicao;
    public int score;
    public Vector3 offset = new Vector3(0, 0, 0);
    public int indiceCor;
    public bool bot;

    protected MatchController matchController;
    protected BoardManager boardManager;

    public void Setup(MatchController controller, BoardManager board, Material material)
    {
        this.matchController = controller;
        this.boardManager = board;
        this.GetComponentInChildren<Renderer>().material = material;
        string _nome = "";

        switch (indiceCor) { 
            case 0:
                    _nome = "Vermelho";
                break;
            case 1:
                    _nome = "Amarelo";
                    break;
            case 2:
                    _nome = "Verde";
                    break;
            case 3:
                    _nome = "Rosa";
                    break;
            case 4:
                    _nome = "Azul Ciano";
                    break;
            case 5:
                    _nome = "Ratatui";
                    break;
        }

        this.nome = _nome;
    }
    public abstract void Jogar();

    protected void FimDoTurno()
    {
        matchController.TerminarTurno();
    }

    protected IEnumerator MoverPara(GameObject objeto, Vector3 destino)
    {
        float tempo = 0;
        Vector3 origem = objeto.transform.position;

        while (tempo < 1)
        {
            tempo += Time.deltaTime * 2f;
            objeto.transform.position = Vector3.Lerp(origem, destino + offset, tempo);
            yield return null;
        }
    }

}
