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
    [SerializeField] private List<int> jogadoresSelecionados = new();

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

    public void DefinirJogadoresPorIndice(List<int> lista)
    {
        jogadoresSelecionados = new List<int>(lista);
    }

    public List<Player> InstanciarJogadores()
    {
        jogadores.Clear();

        foreach (int index in jogadoresSelecionados)
        {
            GameObject prefab;

            if (index != 4)
            {
                prefab = humanoPrefab;
            }
            else
            {
                prefab = botPrefab;
            }

            Player p = Instantiate(prefab).GetComponent<Player>();
            //p.nome = $"Jogador {index + 1}";
            p.indiceCor = index;
            p.posicao = 0;

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
