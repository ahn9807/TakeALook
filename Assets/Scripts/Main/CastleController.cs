using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class CastleController : MonoBehaviour {
    private enum Type
    {
        Dog,
        Cat,
    };

    private Type castleType;

    private float bombInterval = 0.5f;
    public GameObject bombSpawner;
    public Sprite catSprite;
    public Sprite dogSprite;
    public InitAIScene aiInfo;
    private float health;
    private int owner;
    private DateTime ticker;
    private TimeSpan timeSpan;
    private int ballCount = 0;
    private GameObject lastBall;

    public GameObject catSpwaner;
    public GameObject dogSpwaner;

    public Sprite[] catCollapse;
    public Sprite[] dogCollapse;

	// Use this for initialization
	void Awake () {
        ticker = DateTime.Now;
        if(this.name == "Castle1") 
            owner = 1;
        else if(this.name == "Castle2") 
            owner = 2;
        Debug.Log(owner);
	}
	
	// Update is called once per frame
    void Update () {

	}

    private void FixedUpdate()
    {
        timeSpan = DateTime.Now - ticker;
        if(timeSpan.Seconds > bombInterval) {
            ticker = DateTime.Now;
            if (GameController.instance.state != GameController.State.GameOut)
                if (gameObject.name.Contains("1") && ballCount % 2 == 0)
                    lastBall = bombSpawner.GetComponent<BombSpawner>().Fire();
            else if (gameObject.name.Contains("2") && ballCount % 2 == 1)
                    lastBall = bombSpawner.GetComponent<BombSpawner>().Fire();

            ballCount++;
        }
    }

    public void SetOwner(int id) {
        owner = id;
    }

    public int GetOwner() {
        return owner;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bomb" && collision.gameObject != lastBall)
        {
            if(SceneManager.GetActiveScene().name == "Main_Infinite") {
                if(owner == 1 && aiInfo.Player == "Player1") {
                    GameController.instance.health--;
                }
                else if(owner == 1 && aiInfo.AIPlayer == "Player1") {
                    GameController.instance.points++;
                }

                if (owner == 2 && aiInfo.Player == "Player2")
                {
                    GameController.instance.health--;
                }
                else if (owner == 2 && aiInfo.AIPlayer == "Player2")
                {
                    GameController.instance.points++;
                }
            }
            else 
            {
                if (owner == 1)
                    GameController.instance.castle1Health--;
                else if (owner == 2)
                    GameController.instance.castle2Health--;
            }

            Destroy(collision.gameObject);
        }
    }

    public void SetSpriteCat() {
        GetComponent<SpriteRenderer>().sprite = catSprite;
        castleType = Type.Cat;
        var temp = Instantiate(catSpwaner, gameObject.transform);
        temp.transform.localPosition = new Vector3(-40, 190, 0);
        temp.GetComponent<BombSpawner>().fireDirection = new Vector2(owner == 1 ? 100 : -100, 100);
        bombSpawner = temp;
    }

    public void SetSpriteDog() {
        GetComponent<SpriteRenderer>().sprite = dogSprite;
        castleType = Type.Dog;
        var temp = Instantiate(dogSpwaner, gameObject.transform);
        temp.transform.localPosition = new Vector3(-40, 190, 0);
        temp.GetComponent<BombSpawner>().fireDirection = new Vector2(owner == 1 ? 100 : -100, 100);
        bombSpawner = temp;
    }

    bool isCollapse = false;
    public void Collapse() {
        Debug.Log("Collapse");
        if (castleType == Type.Cat && isCollapse == false)
            StartCoroutine(Animation(Type.Cat));
        else if(isCollapse == false)
            StartCoroutine(Animation(Type.Dog));

        isCollapse = true;
    }

    public void Reset()
    {
        if (castleType == Type.Cat)
        {
            GetComponent<SpriteRenderer>().sprite = catSprite;
        }
        else
            GetComponent<SpriteRenderer>().sprite = dogSprite;

        isCollapse = false;
    }

    IEnumerator Animation(Type type) {
        int index = 0;
        // 스프라이트 설정이 잘못되어 고양이와 개를 스왑시킨다 
        if (type == Type.Cat) {
            while (index < catCollapse.Length)
            {
                GetComponent<SpriteRenderer>().sprite = dogCollapse[index++];
                Debug.Log(catCollapse.Length);
                yield return new WaitForSecondsRealtime(0.15f);
            }
        }
        else if(type == Type.Dog) {
            while (index < dogCollapse.Length) {
                GetComponent<SpriteRenderer>().sprite = catCollapse[index++];
                Debug.Log(dogCollapse.Length);
                yield return new WaitForSecondsRealtime(0.15f);
            }
        }

    }
}
