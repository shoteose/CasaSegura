using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class MatchController : MonoBehaviour
{
    [Header("Players")]
    public Player[] players;
    private int currentTurn = 0;

    [Header("Managers")]
    [SerializeField]
    private BoardManager boardManager;
    [SerializeField]
    private UIManager uiManager;


    void Start()
    {
        Debug.Log(GameControllor.Instance.x);
    }

}
