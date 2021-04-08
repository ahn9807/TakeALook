using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    protected int key_Vertical = 0;
    protected int key_Horizontal = 0;
    protected bool grounded = false;
    protected Collider2D collider;

    public int playerState = 1;
    public int playerPosition = 1;
    public int speed;
    public Collider2D ground;
    public SpriteRenderer labelSprite;
    public Sprite labelPlayer1;
    public Sprite labelPlayer2;
    public Sprite labelPlain;
    protected GameController GameController;

    int vspd;
    int hspd;

    Touch leftTouch;
    Vector2 leftTouchInitPos;
    Touch rightTouch;
    Vector2 rightTouchInitPos;
    Vector2 movePos;

    public enum CHARACTER
    {
        GIST,
        KAIST,
        UNIST,
        PHONICS
    };


    // Use this for initialization
    void Start () {
        GameController = GameObject.Find("GameController").GetComponent<GameController>();
        if (this.name == "Player1")
            SetState(GameController.GetPlayer1());
        else
            SetState(GameController.GetPlayer2());
        GetComponent<Rigidbody2D>().gravityScale = 0;

        StartCoroutine(IEInitialLabel());
	}
	
	// Update is called once per frame
	void Update () {
        if(playerPosition == 1) {
            key_Vertical = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
            key_Horizontal = Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0;
        }
        else {
            key_Vertical = Input.GetKey(KeyCode.UpArrow) ? 1 : Input.GetKey(KeyCode.DownArrow) ? -1 : 0;
            key_Horizontal = Input.GetKey(KeyCode.LeftArrow) ? -1 : Input.GetKey(KeyCode.RightArrow) ? 1 : 0;
        }

        // Touch 
        // get began touch 
        if(Input.touchCount != 0) {
            Touch[] touch = Input.touches;
            for (int i = 0; i < touch.Length;i++) {
                var touchPos = Camera.main.ScreenToWorldPoint(touch[i].position);
                if (touchPos.x < 0)
                {
                    leftTouch = touch[i];
                }
                else if(touchPos.x > 0)
                {
                    rightTouch = touch[i];
                }
            }

            speed = 1400;
        }

        if(gameObject.name == "Player1") {
            movePos = Camera.main.ScreenToWorldPoint(leftTouch.position);
        }
        if (gameObject.name == "Player2")
        {
            movePos = Camera.main.ScreenToWorldPoint(rightTouch.position);
        }
    }

    private void FixedUpdate()
    {
        if(Input.touchCount != 0) 
        {
            var tvspd = movePos.y;
            var thspd = movePos.x;
            Vector3 moveDes = Vector3.MoveTowards(transform.position, new Vector3(thspd, tvspd, -5), speed * Time.fixedDeltaTime);
            gameObject.GetComponent<Rigidbody2D>().MovePosition(moveDes);
        }
        else 
        {
            vspd = speed * key_Vertical;
            hspd = speed * key_Horizontal;
            GetComponent<Rigidbody2D>().velocity = new Vector2(hspd, vspd);
        }
    }

    private bool CheckGroudeded() {
        return collider.IsTouching(ground);
    }

    public void SetState(int state) {
        var Kaist = transform.Find("Kaist").gameObject;
        var Unist = transform.Find("Unist").gameObject;
        var Gist = transform.Find("Gist").gameObject;
        var Phonics = transform.Find("Phonics").gameObject;

        playerState = state;
        if(this.name == "Player1") {
            SetPosition(1);
        }
        else {
            SetPosition(2);
        }

        string character_name;
        if (state == (int)CHARACTER.GIST)
        {
            character_name = "Gist";
            Destroy(Kaist);
            Destroy(Unist);
            Destroy(Phonics);
            if(playerPosition == 1) {
                GameObject.Find("Castle1").GetComponent<CastleController>().SetSpriteCat();
            }
            else if(playerPosition == 2) {
                GameObject.Find("Castle2").GetComponent<CastleController>().SetSpriteCat();
            }
                
        }
        else if (state == (int)CHARACTER.KAIST)
        {
            character_name = "Kaist";
            Destroy(Gist);
            Destroy(Unist);
            Destroy(Phonics);
            if (playerPosition == 1)
            {
                GameObject.Find("Castle1").GetComponent<CastleController>().SetSpriteCat();
            }
            else if (playerPosition == 2)
            {
                GameObject.Find("Castle2").GetComponent<CastleController>().SetSpriteCat();
            }
        }
        else if (state == (int)CHARACTER.PHONICS)
        {
            character_name = "Phonics";
            Destroy(Kaist);
            Destroy(Unist);
            Destroy(Gist);
            if (playerPosition == 1)
            {
                GameObject.Find("Castle1").GetComponent<CastleController>().SetSpriteDog();
            }
            else if (playerPosition == 2)
            {
                GameObject.Find("Castle2").GetComponent<CastleController>().SetSpriteDog();
            }
        }
        else
        {
            character_name = "Unist";
            Destroy(Kaist);
            Destroy(Gist);
            Destroy(Phonics);
            if (playerPosition == 1)
            {
                GameObject.Find("Castle1").GetComponent<CastleController>().SetSpriteDog();
            }
            else if (playerPosition == 2)
            {
                GameObject.Find("Castle2").GetComponent<CastleController>().SetSpriteDog();
            }
        }

        var targetObject = transform.Find(character_name).gameObject;
        collider = targetObject.GetComponent<Collider2D>();
    }

    public void SetPosition(int position) {
        this.playerPosition = position;
    }

    IEnumerator IEInitialLabel() {
        if(GameController.instance.AIChar == AIPlayer.CHARACTER.None) {
            // 현재 게임엔 AI 캐릭터가 존재하지 않는다
            if (gameObject.name == "Player1")
            {
                labelSprite.sprite = labelPlayer1;
            }
            else
                labelSprite.sprite = labelPlayer2;

            yield return new WaitForSecondsRealtime(1f);
            labelSprite.sprite = null;
        }
        else {
            // 게임엔 AI 캐릭터가 존재한다 
            if (name == InitAIScene.singleton.Player)
            {
                labelSprite.sprite = labelPlain;
            }
            else
                labelSprite.sprite = null;

            yield return new WaitForSecondsRealtime(1f);
            labelSprite.sprite = null;
        }
    }
}
