using UnityEngine;

public class UIManager : MonoBehaviour
{
    void Awake()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MostrarPerguntaOuExercicio(Tile tile)
    {
        if (tile.especial)
        {
            // Mostrar imagem e exerc�cio
        }
        else
        {
            // Mostrar pergunta + op��es de resposta
        }
    }


}
