using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class GestaoPlayerCena : MonoBehaviour
{
    [Header("BotoesJogo")]
    [SerializeField] private Button botaoJogar;
    [SerializeField] private Button botaoVoltar;
    [SerializeField] private Button botaoSozinho;
    [SerializeField] private Button botaoComputador;

    [Header("PaineisPessoas")]
    [SerializeField] private GameObject[] paineis;
    [SerializeField] private RawImage[] imagens;
    [SerializeField] private Button[] botoesRemover;

    [Header("Texturas")]
    [SerializeField] private Texture texturaAdicionar;
    [SerializeField] private Texture[] texturasAtivas;

    [Header("Painel Solo/Bot")]
    [SerializeField] private GameObject paiPainel;

    [Header("Jogadores Ativos")]
    [SerializeField] private List<PlayerInfo> jogadoresAtivos = new List<PlayerInfo>();

    [Header("Numero de Bots")]
    [SerializeField] private int nrBots;

    private void Awake()
    {

        paiPainel.SetActive(false);

        for (int i = 0; i < imagens.Length; i++)
        {
            imagens[i].texture = texturaAdicionar;
            imagens[i].gameObject.SetActive(true);
            imagens[i].GetComponent<Button>().enabled = true;

            int index = i;
            imagens[i].GetComponent<Button>().onClick.AddListener(() => AdicionarJogador(index));
            botoesRemover[i].onClick.AddListener(() => RemoverJogador(index));
        }

        botaoJogar.onClick.AddListener(Jogar);
        botaoVoltar.onClick.AddListener(Voltar);
        botaoSozinho.onClick.AddListener(JogarSozinho);
        botaoComputador.onClick.AddListener(JogarContraPc);

        AtualizarEstadoBotaoJogar();
    }

    private void Update()
    {
        for (int i = 0; i < botoesRemover.Length; i++)
        {
            Image img = botoesRemover[i].GetComponent<Image>();
            Color cor = img.color;

            bool existe = false;
            foreach (var j in jogadoresAtivos)
            {
                if (j.indice == i)
                {
                    existe = true;
                    break;
                }
            }
            cor.a = existe ? 1f : 0.3f;


            img.color = cor;
        }
    }

    private void AdicionarJogador(int index)
    {
        foreach (var j in jogadoresAtivos)
        {
            if (j.indice == index)
                return;
        }

        int nrTextura = Random.Range(0, texturasAtivas.Length);
        Texture texturaSelecionada = texturasAtivas[nrTextura];

        imagens[index].texture = texturaSelecionada;
        imagens[index].GetComponent<Button>().enabled = false;

        jogadoresAtivos.Add(new PlayerInfo { indice = index, textura = texturaSelecionada });

        AtualizarEstadoBotaoJogar();
    }



    private void RemoverJogador(int index)
    {
        PlayerInfo jogador = null;
        foreach (var j in jogadoresAtivos)
        {
            if (j.indice == index)
            {
                jogador = j;
                break;
            }
        }

        if (jogador == null) return;

        imagens[index].texture = texturaAdicionar;
        imagens[index].GetComponent<Button>().enabled = true;

        jogadoresAtivos.Remove(jogador);
        AtualizarEstadoBotaoJogar();
    }


    private void AtualizarEstadoBotaoJogar()
    {
        botaoJogar.interactable = jogadoresAtivos.Count > 0;
    }

    private void Jogar()
    {
        
        if (jogadoresAtivos.Count == 1)
        {
            Debug.Log("SOU SO UM MALUQUINHO");
            paiPainel.SetActive(true);
            
        }
        else
        {
            GameControllor.Instance.DefinirJogadoresPorInfo(jogadoresAtivos);
            GameControllor.Instance.Jogar();
        }

    }

    private void Voltar()
    {
        
        GameControllor.Instance.VoltarMenuPrincipal();
    }

    private void JogarSozinho()
    {
        
        GameControllor.Instance.DefinirJogadoresPorInfo(jogadoresAtivos);
        GameControllor.Instance.Jogar();
    }

    private void JogarContraPc()
    {
        for (int i = 0; i < nrBots; i++)
        {
            int indiceBot = 4 + i;

            bool jaExiste = false;
            foreach (var j in jogadoresAtivos)
            {
                if (j.indice == indiceBot)
                {
                    jaExiste = true;
                    break;
                }
            }

            if (jaExiste) continue;

            int nrTextura = Random.Range(0, texturasAtivas.Length);
            Texture texturaSelecionada = texturasAtivas[nrTextura];

            jogadoresAtivos.Add(new PlayerInfo { indice = indiceBot, textura = texturaSelecionada });
        }

        GameControllor.Instance.DefinirJogadoresPorInfo(jogadoresAtivos);
        GameControllor.Instance.Jogar();
    }



}

public class PlayerInfo
{
    public int indice;
    public Texture textura;

}