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
    int tries = 3;

    public TextDisplay txtDisplay;
    public GameObject playerInputMenu;

    [ContextMenu("TestFunction")]
    void Test()
    {

    }

    void Start()
    {
        foreach (var item in FindObjectsOfType<Ball>()) {
            item.po.enabled = false;
        }
        txtDisplay.UpdateTextSimple("PLAY!!!!");
        Time.timeScale = 3.0f;


        //players = GameObject.FindObjectsOfType<Tank>();
    }

    void Update () 
    {
        switch (gameState)
        {
            case GameState.Turn:
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

    public void GetTurnInput(int command)
    {
        switch (command) {
            case 0:
                //Dropdabase
                DropBall();
                break;
            case 1:
                MoveBall(1);
                //Move to right
                break;
            case -1:
                MoveBall(-1);
                //Move to left
                break;
            default:
                break;
        }
    }

    void DropBall() 
    {
        foreach (var item in FindObjectsOfType<Ball>()) {
            item.po.enabled = true;
        }
        playerInputMenu.SetActive(false);
    }

    void MoveBall(float dir) {
        foreach (var item in FindObjectsOfType<Ball>()) {
            item.transform.position = new Vector3(
                Mathf.Clamp(item.transform.position.x + dir * 0.5f, -2.5f, 2.5f),
                item.transform.position.y);
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
        //Spawn new ball
        NextTurn();
    }

    void EndTurn()
    {
        gameState = GameState.WaitProjectile;
    }

    void NextTurn()
    {
        playerInputMenu.SetActive(true);
        gameState = GameState.Menu;
        if (playerTurn == tries) {
            txtDisplay.UpdateTextSimple("OUT OF TRIES NEW GAME?");
            return;
        }
        txtDisplay.UpdateTextSimple("OUT OF TRIES TRY AGAIN");
        gameState = GameState.Turn;
    }

    public void RoundOver(bool won) {
        if (won) {
            txtDisplay.UpdateTextSimple("YOU WON");
            gameState = GameState.Menu;

        } else {
            NextTurn();
        }

    }
}
