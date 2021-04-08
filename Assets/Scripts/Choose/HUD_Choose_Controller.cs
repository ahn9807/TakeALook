using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_Choose_Controller : MonoBehaviour {
    public string message;
    public GameController gameController;

    public SpriteRenderer leftSprite;
    public SpriteRenderer rightSprite;
	// Use this for initialization
	void Start () {
        leftSprite.color = new Color(1, 1, 1, 0);
        rightSprite.color = new Color(1, 1, 1, 0);
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void SetState(int state) {
        if(state == 1) {
            leftSprite.color = new Color(1, 1, 1, 1);
        }
        else if(state == 2) {
            rightSprite.color = new Color(1, 1, 1, 1);
        }
    }
}
