using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllor : MonoBehaviour
{
    public static GameControllor Instance;

    [Header("Prefabs")]
    [SerializeField] private GameObject humanoPrefab;
    [SerializeField] private GameObject botPrefab;

    [SerializeField] private List<Player> jogadores = new List<Player>();
    [SerializeField] private List<PlayerInfo> jogadoresSelecionados = new();


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void DefinirJogadoresPorInfo(List<PlayerInfo> lista)
    {
        jogadoresSelecionados = new List<PlayerInfo>(lista);
    }

    public List<Player> InstanciarJogadores()
    {
        jogadores.Clear();

        foreach (PlayerInfo info in jogadoresSelecionados)
        {
            GameObject prefab = (info.indice != 4) ? humanoPrefab : botPrefab;

            Player p = Instantiate(prefab).GetComponent<Player>();
            p.indiceCor = info.indice;
            p.posicao = 0;
            p.texturaPersonagem = info.textura; 

            jogadores.Add(p);
        }


        return jogadores;
    }


    public List<Player> GetJogadores() => jogadores;

    public void Jogar()
    {
        SceneManager.LoadScene("CenaJogo");
    }

    public void VoltarMenuPrincipal()
    {
        SceneManager.LoadScene("CenaMenuInicial");
    }

    public void MenuGestaoPlayers()
    {
        Debug.Log("que passa caralho");
        SceneManager.LoadScene("CenaGestao");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
