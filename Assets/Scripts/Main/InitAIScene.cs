using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitAIScene : MonoBehaviour {
    public string AIPlayer;
    public string Player;
    public static InitAIScene singleton;
    public Sprite labelPlayer1;
    public Sprite labelPlayer2;
    public Sprite labelPlain;

    // Use this for initialization
    private void Awake()
    {
        singleton = this;

        GameObject player1 = GameObject.Find("Player1");
        GameObject player2 = GameObject.Find("Player2");

        int rnd_num = Random.Range(0, 2);
        if(rnd_num == 0) {
            GameController.instance.SetCharacter((int)GameController.instance.AIChar);
            GameController.instance.SetCharacter((int)GameController.instance.playerChar);

            AIPlayer = "Player1";
            Player = "Player2";

            player1.AddComponent<AIPlayer>();
            player2.AddComponent<PlayerController>();


            player1.GetComponent<AIPlayer>().speed = 700;
            player1.GetComponent<AIPlayer>().ground = GameObject.Find("Fence").GetComponent<Collider2D>();
            player2.GetComponent<PlayerController>().speed = 700;
            player2.GetComponent<PlayerController>().ground = GameObject.Find("Fence").GetComponent<Collider2D>();
            player2.GetComponent<PlayerController>().labelSprite = player2.transform.Find("label").GetComponent<SpriteRenderer>();
            player2.GetComponent<PlayerController>().labelPlain = labelPlain;
            player2.GetComponent<PlayerController>().labelPlayer1 = labelPlayer1;
            player2.GetComponent<PlayerController>().labelPlayer2 = labelPlayer2;
        }
        else
        {
            GameController.instance.SetCharacter((int)GameController.instance.playerChar);
            GameController.instance.SetCharacter((int)GameController.instance.AIChar);

            AIPlayer = "Player2";
            Player = "Player1";

            player1.AddComponent<PlayerController>();
            player2.AddComponent<AIPlayer>();

            player1.GetComponent<PlayerController>().speed = 700;
            player1.GetComponent<PlayerController>().ground = GameObject.Find("Fence").GetComponent<Collider2D>();
            player1.GetComponent<PlayerController>().labelSprite = player1.transform.Find("label").GetComponent<SpriteRenderer>();
            player1.GetComponent<PlayerController>().labelPlain = labelPlain;
            player1.GetComponent<PlayerController>().labelPlayer1 = labelPlayer1;
            player1.GetComponent<PlayerController>().labelPlayer2 = labelPlayer2;
            player2.GetComponent<AIPlayer>().speed = 700;
            player2.GetComponent<AIPlayer>().ground = GameObject.Find("Fence").GetComponent<Collider2D>();
        }
    }
}
