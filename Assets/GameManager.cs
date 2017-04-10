using UnityEngine;
using UnityEngine.UI;
using System.Collections;


//Turn states: Turn going on (gets inputs), turn ended (wait for projectile to fly), game over (menu)

public class GameManager : Singleton<GameManager> {
    protected GameManager() { }

    enum GameState
    {
        Turn,
        WaitProjectile,
        Menu
    }

    GameState gameState = GameState.Turn;
    int playerTurn = 0;
    public Tank[] players;

    public TextDisplay txtDisplay;

    [ContextMenu("TestFunction")]
    void Test()
    {
        DeclareWinner(0);
    }

    void Start()
    {
        //players = GameObject.FindObjectsOfType<Tank>();
    }

	void Update () 
    {
        switch (gameState)
        {
            case GameState.Turn:
                GetTurnInput();
                break;
            case GameState.WaitProjectile:
                break;
            case GameState.Menu:
                GetMenuInput();
                break;
            default:
                break;
        }
	}

    void GetTurnInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            players[playerTurn].RotateCannon();
        }
        if (Input.GetKey(KeyCode.D))
        {
            players[playerTurn].RotateCannon(false);
        }
        if (Input.GetKey(KeyCode.W))
        {
            players[playerTurn].AddForce();
        }
        if (Input.GetKey(KeyCode.S))
        {
            players[playerTurn].AddForce(false);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            players[playerTurn].Shoot();
            EndTurn();
        }
    }

    void GetMenuInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NewGame();
        }
    }

    void NewGame()
    {
        foreach (Tank t in players)
        {
            t.Init();
        }
        NextTurn();
    }

    void EndTurn()
    {
        gameState = GameState.WaitProjectile;
    }

    internal void ProjectileHit()
    {
        NextTurn();
    }

    void NextTurn()
    {
        if (Winner())
        {
            return;
        }

        if (playerTurn == players.Length - 1)
        {
            playerTurn = 0;
        }
        else
            playerTurn++;

        txtDisplay.UpdateTextSimple("P" + (playerTurn + 1) + " TURN");
        gameState = GameState.Turn;
    }

    int WinnerNumber()
    {
        int playerAlive = -1;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].Alive())
            {
                if (playerAlive == -1)
                {
                    playerAlive = i;
                }
                else
                {
                    return -1;
                }
            }
        }
        return playerAlive;
    }

    bool Winner()
    {
        int winner = WinnerNumber();
        if (winner != -1)
        {
            DeclareWinner(winner);
            return true;
        }
        return false;
    }

    void DeclareWinner(int winner)
    {
        gameState = GameState.Menu;
        txtDisplay.UpdateTextSimple("P" + (winner + 1) + " WINS\nPRESS SPACE");
    }
}
