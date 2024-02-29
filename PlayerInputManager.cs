using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;

    // Start is called before the first frame update
    public void makePlayerMove(int index)
    {
        if (GameManager.instance.bIsPlayerTurn)
            GameManager.instance.makeMove(index, 1);
    }
}
