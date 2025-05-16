using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameControllor : MonoBehaviour
{

    public static GameControllor Instance;
    public GameObject humanoPrefab;
    public GameObject botPrefab;
    public Player[] jogadores;

    void Awake()
    {
        Instance = this;

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        jogadores = new Player[1];
        Humano humano = new Humano();
        jogadores[0] = humano;
    }

    public Player[] GetPlayers()
    {
        List<Player> lista = new List<Player>();

        GameObject go1 = Instantiate(humanoPrefab);
        Humano h1 = go1.GetComponent<Humano>();
        h1.nome = "Jogador 1";
        lista.Add(h1);

        GameObject go2 = Instantiate(botPrefab);
        Bot b1 = go2.GetComponent<Bot>();
        b1.nome = "Bot 1";
        lista.Add(b1);

        return lista.ToArray();
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
