using UnityEngine;

public class GameControllor : MonoBehaviour
{

    public static GameControllor Instance;
    public string x = "aaaaaaaaaaaaa";

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
