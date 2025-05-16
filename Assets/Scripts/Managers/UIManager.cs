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
            // Mostrar imagem e exercício
        }
        else
        {
            // Mostrar pergunta + opções de resposta
        }
    }


}
