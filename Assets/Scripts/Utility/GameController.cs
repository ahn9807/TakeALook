using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    const int MAXHEALTH = 21;

    public enum CHARACTER
    {
        GIST,   //그냥 고양이
        KAIST,  //공돌이 고양이
        UNIST,  //그냥 개
        PHONICS //공돌이 개
    };

    public enum State
    {
        GameIn,
        Game,
        GameChanging,
        GameOut,
        GameEnd,
    };

    public State state;

    GameObject timerObj;
    float gameTime;
    int gameCount;
    const int GAMECOUNT_MAX = 3;
    const int TIME_LIMIT = 10;

    public int player1Scores = 0;
    public int player2Scores = 0;

    public int castle1Health = 10;
    public int castle2Health = 10;

    public static GameController instance;

    private GameObject gameResult;

    int player1 = -1;
    int player2 = -1;

    public Vector2 cloudOffset = Vector2.zero;

    // AI 
    public AIPlayer.CHARACTER AIChar = AIPlayer.CHARACTER.None;
    public PlayerController.CHARACTER playerChar;

    // Infinite
    public int points;
    public int health;

    // Use this for initialization
    private void Awake()
    {

    }

    void Start() {
        if (instance == null) {
            instance = this;
        }
        else
            Destroy(this.gameObject);
        
        DontDestroyOnLoad(this);
        state = State.GameIn;
        gameTime = 0;
        gameCount = 0;
    }

    private void FixedUpdate()
    {
        switch(state) {
            case State.GameIn:
                UpdateGameIn();
                break;
            case State.Game:
                UpdateGame();
                break;
            case State.GameChanging:
                UpdateGameChanging();
                break;
            case State.GameOut:
                UpdateGameOut();
                break;
            case State.GameEnd:
                UpdateGameEnd();
                break;
        }
    }

    void UpdateGameIn() {
        castle1Health = MAXHEALTH;
        castle2Health = MAXHEALTH;
        points = 0;
        health = MAXHEALTH;
        if(SceneManager.GetActiveScene().name.Contains("Main")) {
            gameResult = GameObject.Find("GameResult");
            gameResult.SetActive(false);
            gameTime = 0;
            state = State.Game;
            GameObject.Find("WinController1").GetComponent<WinController>().DeleteWin();
            GameObject.Find("WinController2").GetComponent<WinController>().DeleteWin();
            GameObject.Find("Castle1").GetComponent<CastleController>().Reset();
            GameObject.Find("Castle2").GetComponent<CastleController>().Reset();
        }
    }

    void UpdateGame() {
        gameTime += Time.fixedDeltaTime;
        if (castle1Health <= 0 || castle2Health <= 0 || health <= 0)
            state = State.GameChanging;
    }

    void UpdateGameChanging() {
        if (castle1Health <= 0)
        {
            GameObject.Find("WinController2").GetComponent<WinController>().SetWin();
            state = State.GameOut;
        }
        else if (castle2Health <= 0) {
            GameObject.Find("WinController1").GetComponent<WinController>().SetWin();
            state = State.GameOut;
        }
        else if (health <= 0) {
            state = State.GameOut;
        }
    }

    //게임 종료 준비 
    void UpdateGameOut() {
        gameResult.SetActive(true);
        ResultController result = GameObject.Find("GameResult").GetComponent<ResultController>();
        if (castle1Health > castle2Health)
        {
            player1Scores++;
            result.player1score = player1Scores;
            result.leftWin = true;
            result.rightWin = false;
            GameObject.Find("Castle2").GetComponent<CastleController>().Collapse();
        }
        else if (castle1Health < castle2Health)
        {
            player2Scores++;
            result.player2score = player2Scores;
            result.rightWin = true;
            result.leftWin = false;
            GameObject.Find("Castle1").GetComponent<CastleController>().Collapse();
        }
        else if (health <= 0) {
            result.infPoints = points;
            if(InitAIScene.singleton.Player == "Player1")
                GameObject.Find("Castle1").GetComponent<CastleController>().Collapse();
            else
                GameObject.Find("Castle2").GetComponent<CastleController>().Collapse();
        }
        //UpdateGameEnd 는 ResultController 에게 권한을 넘긴다 
    }

    //게임 종료 연출 
    void UpdateGameEnd() {
        player1 = 0;
        player2 = 0;
        player1Scores = 0;
        player2Scores = 0;
        castle1Health = 10;
        castle2Health = 10;
        health = 0;
        points = 0;
        SceneManager.LoadScene("Menu_Title");
        Destroy(gameObject);
    }   
        
    public bool IsEnd() {
        return state == State.GameEnd;
    }

    public void SetCharacter(int select) {
        if (player1 == -1)
            player1 = select;
        else
            player2 = select;
    }

    public int GetPlayer1() {
        return player1;
    }

    public int GetPlayer2()
    {
        return player2;
    }

	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator IEEndGame() {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(2);
        Time.timeScale = 1;
        state = State.GameIn;
        SceneManager.LoadScene("Menu_Title");
    }
}
