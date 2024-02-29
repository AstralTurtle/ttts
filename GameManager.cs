using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public static GameManager instance;

    public int[] gameboard = new int[9];

    [SerializeField]
    Button[] buttons = new Button[9];

    public int winner = 0;

    public bool bIsPlayerTurn = true;

    [SerializeField]
    TextMeshProUGUI winnerText;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        winnerText.text = "";
    }

    public void reset()
    {
        for (int i = 0; i < 9; i++)
        {
            gameboard[i] = 0;
            buttons[i].interactable = true;
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
        winner = 0;
        winnerText.text = "";
        bIsPlayerTurn = true;
    }

    // what
    public void makeMove(int index, int player)
    {
        if (player == 1)
            bIsPlayerTurn = false;
        gameboard[index] = player;
        buttons[index].interactable = false;
        buttons[index].GetComponentInChildren<TextMeshProUGUI>().text = (player == 1 ? "X" : "O");
        winner = checkWinner();
        if (winner != 0)
        {
            winnerText.text = "Player " + winner + " wins!";
            Debug.Log("Player " + winner + " wins!");
            return;
        }
        if (checkTie())
        {
            winnerText.text = "It's a tie!";
            Debug.Log("It's a tie!");
            return;
        }
    }

    int checkWinner()
    {
        // check row
        for (int i = 0; i < 9; i += 3)
        {
            if (
                gameboard[i] != 0
                && gameboard[i] == gameboard[i + 1]
                && gameboard[i] == gameboard[i + 2]
            )
            {
                return gameboard[i];
            }
        }
        // check column
        for (int i = 0; i < 3; i++)
        {
            if (
                gameboard[i] != 0
                && gameboard[i] == gameboard[i + 3]
                && gameboard[i + 3] == gameboard[i + 6]
            )
            {
                return gameboard[i];
            }
        }
        // check first diagonal
        if (gameboard[0] != 0 && gameboard[0] == gameboard[4] && gameboard[4] == gameboard[8])
            return gameboard[0];
        else if (gameboard[2] != 0 && gameboard[2] == gameboard[4] && gameboard[2] == gameboard[6])
            return gameboard[2];
        return 0;
    }

    public bool checkTie()
    {
        if (winner != 0)
            return false;
        foreach (int i in gameboard)
        {
            if (i == 0)
                return false;
        }
        return true;
    }

    // Update is called once per frame
    void Update() { }
}
