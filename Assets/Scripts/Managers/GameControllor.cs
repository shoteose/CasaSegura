using System.Collections.Generic;
using UnityEngine;

public class GameControllor : MonoBehaviour
{
    public static GameControllor Instance;

    [Header("Prefabs")]
    public GameObject humanoPrefab;
    public GameObject botPrefab;

    private List<Player> jogadores = new List<Player>();

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

    public List<Player> InstanciarJogadores()
    {
        jogadores.Clear();
        //jogadores.Add(CriarJogador(humanoPrefab, "Joao"));
        //jogadores.Add(CriarJogador(humanoPrefab, "Paulo"));
        jogadores.Add(CriarJogador(botPrefab, "Bot 1"));
        jogadores.Add(CriarJogador(botPrefab, "Bot 2"));

        //jogadores.Add(CriarJogador(botPrefab, "Bot 1"));


        return jogadores;
    }

    private Player CriarJogador(GameObject prefab, string nome)
    {
        GameObject go = Instantiate(prefab);
        Player p = go.GetComponent<Player>();
        p.nome = nome;
        p.posicao = 0;
        return p;
    }

    public List<Player> GetJogadores() => jogadores;
}
