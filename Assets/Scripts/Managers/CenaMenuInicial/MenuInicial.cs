using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    public void MenuGestaoPlayers()
    {
        
        SceneManager.LoadScene("CenaGestao");
    }

    public void FecharJogo()
    {
        Application.Quit();
    }


}
