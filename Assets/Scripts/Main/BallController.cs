using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour {

    private int touchCount;
    private int owner;

    private void Start()
    {
        var middleFence = GameObject.Find("MiddleFence").GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), middleFence);
    }
    // Use this for initialization
	
	// Update is called once per frame
	void Update () {
        if(GameController.instance.state == GameController.State.GameOut) {
            Destroy(gameObject);
        }
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.name == "Fence") {
            touchCount++;
        }
        if(touchCount == 4) {
            Destroy(this.gameObject);
        }
        GetComponent<AudioSource>().Play();
    }

    public void SetOwner(int id) {
        owner = id;
    }

    public int GetOwner() {
        return owner;
    }
}
