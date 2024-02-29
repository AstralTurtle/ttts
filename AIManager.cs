using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AIManager : MonoBehaviour
{
    int justrandomithonestly = 0;

    [HideInInspector]
    Node currentNode;

    public static AIManager instance;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.bIsPlayerTurn && GameManager.instance.winner == 0)
        {
            Debug.Log("AI is making a move");
            AIManager.instance.makeAIMove();
            justrandomithonestly++;
            if (justrandomithonestly > 30)
            {
                AIManager.instance.makeRandomMove();
                justrandomithonestly = 0;
            }
        }
    }

    void makeRandomMove()
    {
        int index = UnityEngine.Random.Range(0, 9);
        while (GameManager.instance.gameboard[index] != 0)
        {
            index = UnityEngine.Random.Range(0, 9);
        }
        GameManager.instance.makeMove(index, -1);
        GameManager.instance.bIsPlayerTurn = true;
    }

    void makeAIMove()
    {
        if (GameManager.instance.winner != 0)
            return;
        if (GameManager.instance.checkTie())
            return;
        int index = checkWinning();
        Debug.Log("check win index: " + index);
        if (index == -1)
            index = checkBlocking();
        Debug.Log("check block index: " + index);
        if (index == -1)
            index = Minimax();
        Debug.Log("minimax index: " + index);
        if (GameManager.instance.gameboard[index] == 0)
        {
            GameManager.instance.makeMove(index, -1);
            GameManager.instance.bIsPlayerTurn = true;
        }
    }

    // check if the AI has to block the player
    public int checkBlocking()
    {
        int[] board = GameManager.instance.gameboard;
        // check row
        for (int i = 0; i < 9; i += 3)
        {
            if (board[i] != 0 && board[i] == board[i + 1])
            {
                if (board[i + 2] == 0)
                    return i + 2;
            }
            else if (board[i] != 0 && board[i] == board[i + 2])
            {
                if (board[i + 1] == 0)
                    return i + 1;
            }
            else if (board[i + 1] != 0 && board[i + 1] == board[i + 2])
            {
                if (board[i] == 0)
                    return i;
            }
        }
        // check column
        for (int i = 0; i < 3; i++)
        {
            if (board[i] != 0 && board[i] == board[i + 3])
            {
                if (board[i + 6] == 0)
                    return i + 6;
            }
            else if (board[i] != 0 && board[i] == board[i + 6])
            {
                if (board[i + 3] == 0)
                    return i + 3;
            }
            else if (board[i + 3] != 0 && board[i + 3] == board[i + 6])
            {
                if (board[i] == 0)
                    return i;
            }
        }
        // check first diagonal
        if (board[0] != 0 && board[0] == board[4])
        {
            if (board[8] == 0)
                return 8;
        }
        else if (board[0] != 0 && board[0] == board[8])
        {
            if (board[4] == 0)
                return 4;
        }
        else if (board[4] != 0 && board[4] == board[8])
        {
            if (board[0] == 0)
                return 0;
        }
        // check second diagonal
        if (board[2] != 0 && board[2] == board[4])
        {
            if (board[6] == 0)
                return 6;
        }
        else if (board[2] != 0 && board[2] == board[6])
        {
            if (board[4] == 0)
                return 4;
        }
        else if (board[4] != 0 && board[4] == board[6])
        {
            if (board[2] == 0)
                return 2;
        }
        return -1;
    }

    public int checkWinning()
    {
        int[] board = GameManager.instance.gameboard;
        // check row
        for (int i = 0; i < 9; i += 3)
        {
            if (board[i] != 0 && board[i] == board[i + 1])
            {
                if (board[i] == -1)
                    return i + 2;
            }
            else if (board[i] != 0 && board[i] == board[i + 2])
            {
                if (board[i] == -1)
                    return i + 1;
            }
            else if (board[i + 1] != 0 && board[i + 1] == board[i + 2])
            {
                if (board[i + 1] == -1)
                    return i;
            }
        }
        // check column
        for (int i = 0; i < 3; i++)
        {
            if (board[i] != 0 && board[i] == board[i + 3])
            {
                if (board[i] == -1)
                    return i + 6;
            }
            else if (board[i] != 0 && board[i] == board[i + 6])
            {
                if (board[i] == -1)
                    return i + 3;
            }
            else if (board[i + 3] != 0 && board[i + 3] == board[i + 6])
            {
                if (board[i + 3] == -1)
                    return i;
            }
        }
        // check first diagonal
        if (board[0] != 0 && board[0] == board[4])
        {
            if (board[8] == -1)
                return 8;
        }
        else if (board[0] != 0 && board[0] == board[8])
        {
            if (board[4] == -1)
                return 4;
        }
        else if (board[4] != 0 && board[4] == board[8])
        {
            if (board[0] == -1)
                return 0;
        }
        // check second diagonal
        if (board[2] != 0 && board[2] == board[4])
        {
            if (board[6] == -1)
                return 6;
        }
        else if (board[2] != 0 && board[2] == board[6])
        {
            if (board[4] == -1)
                return 4;
        }
        else if (board[4] != 0 && board[4] == board[6])
        {
            if (board[2] == -1)
                return 2;
        }
        return -1;
    }

    // return the index of the best move
    int Minimax()
    {
        if (GameManager.instance.winner != 0)
            return -1;
        if (GameManager.instance.checkTie())
            return -1;
        int bestMove = UnityEngine.Random.Range(0, 9);
        int bestVal = int.MinValue;
        for (int i = 0; i < 9; i++)
        {
            if (GameManager.instance.gameboard[i] == 0)
            {
                int[] newBoard = new int[9];
                Array.Copy(GameManager.instance.gameboard, newBoard, 9);
                (int, int) moveVal = MinimaxHelper(
                    new Node(newBoard, 0, i),
                    -1,
                    int.MinValue,
                    int.MaxValue,
                    false
                );
                GameManager.instance.gameboard[i] = 0;
                if (moveVal.Item1 > bestVal)
                {
                    bestVal = moveVal.Item1;
                    bestMove = moveVal.Item2;
                }
            }
        }
        return bestMove;
    }

    (int, int) MinimaxHelper(Node node, int depth, int alpha, int beta, bool isMaximizingPlayer)
    {
        int bestMove = UnityEngine.Random.Range(0, 9);
        if (depth == 5 || node.isTerminalNode())
            return (node.eval(), bestMove);

        if (isMaximizingPlayer)
        {
            int best = int.MinValue;
            Node[] children = node.gemerateChildren();
            for (int i = 0; i < 9; i++)
            {
                if (children[i] != null)
                {
                    (int, int) vals = MinimaxHelper(children[i], depth + 1, alpha, beta, false);
                    int val = vals.Item1;
                    if (val > best)
                    {
                        Debug.Log("val: " + val + " best: " + best);
                        best = val;
                        bestMove = children[i].moveInd; // assuming i is the move
                    }
                    alpha = Mathf.Max(alpha, best);
                    if (beta <= alpha)
                        break;
                }
            }
            return (best, bestMove);
        }
        else
        {
            int best = int.MaxValue;
            Node[] children = node.gemerateChildren();
            for (int i = 0; i < 9; i++)
            {
                if (children[i] != null)
                {
                    (int, int) vals = MinimaxHelper(children[i], depth + 1, alpha, beta, true);
                    int val = vals.Item1;
                    if (val < best)
                    {
                        Debug.Log("val: " + val + " best: " + best);
                        best = val;
                        bestMove = children[i].moveInd; // assuming i is the move
                    }
                    beta = Mathf.Min(beta, best);
                    if (beta <= alpha)
                        break;
                }
            }
            return (best, bestMove);
        }
    }
}

class Node
{
    public int[] gameboard;

    // Node[] children = new Node[9];
    public int score;
    public int depth;
    public int moveInd;

    public Node(int[] gameboard, int depth, int index)
    {
        this.gameboard = gameboard;
        this.depth = depth;
        moveInd = index;
    }

    public Node[] gemerateChildren()
    {
        Node[] children = new Node[9];
        for (int i = 0; i < 9; i++)
        {
            if (gameboard[i] == 0)
            {
                int[] newGameboard = gameboard;
                newGameboard[i] = -1;
                children[i] = new Node(newGameboard, depth + 1, i);
            }
        }
        return children;
    }

    public bool isTerminalNode()
    {
        return gemerateChildren().Length == 0;
    }

    public int eval()
    {
        int score = 0;
        // check row
        for (int i = 0; i < 9; i += 3)
        {
            if (
                gameboard[i] != 0
                && gameboard[i] == gameboard[i + 1]
                && gameboard[i] == gameboard[i + 2]
            )
            {
                if (gameboard[i] == 1)
                    score += 100;
                else
                    score -= 1;
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
                if (gameboard[i] == 1)
                    score += 100;
                else
                    score -= 1;
            }
            ;
        }
        // check first diagonal
        if (gameboard[0] != 0 && gameboard[0] == gameboard[4] && gameboard[4] == gameboard[8])
        {
            if (gameboard[0] == 1)
                score += 100;
            else
                score -= 1;
        }
        else if (gameboard[2] != 0 && gameboard[2] == gameboard[5] && gameboard[2] == gameboard[7])
        {
            if (gameboard[2] == 1)
                score += 100;
            else
                score -= 1;
        }
        return score;
    }
}
