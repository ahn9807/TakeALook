using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDScoreController : MonoBehaviour {
    public string state;
    public Text scoreText;
    public InitAIScene aiInfo;
    GameController instance;

	// Use this for initialization
	void Start () {
        instance = GameController.instance;
	}
	
	// Update is called once per frame
	void Update () {

        if(SceneManager.GetActiveScene().name != "Main_Infinite")
        {
            if (state == "Player1")
            {
                scoreText.text = "" + instance.castle1Health;
            }
            else
                scoreText.text = "" + instance.castle2Health;
        }
        else 
        {
            if (aiInfo.Player == state)
            {
                scoreText.text = "" + instance.health;
            }
            else
            {
                scoreText.text = "" + instance.points;
                scoreText.color = new Color(0.3254902f, 0.9490196f, 0.4470243f, 1f);
            }
        }
	}
}
