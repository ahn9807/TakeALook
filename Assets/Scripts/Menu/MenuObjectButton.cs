using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuObjectButton : MonoBehaviour {
    public GameObject menuScript;
    public string message;

    SpriteRenderer menuButton;
    Vector3 orgLocalScale;
    Color orgColor;

    private void Awake()
    {
        menuButton = transform.Find("Menu_Button").GetComponent<SpriteRenderer>();
        orgLocalScale = transform.localScale;
        orgColor = menuButton.color;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        bool touchOn = false;
        Vector3 touchPos = Vector3.zero;

        Touch[] tcall = Input.touches;
        foreach(Touch tc in tcall) {
            if(tc.phase == TouchPhase.Began) {
                touchOn = true;
                touchPos = tc.position;
                break;
            }
        }
        if(Input.GetMouseButtonDown(0)) {
            touchOn = true;
            touchPos = Input.mousePosition;
        }

        if(touchOn) {
            touchPos = Camera.main.ScreenToWorldPoint(touchPos);
            Collider2D col2D = Physics2D.OverlapPoint(touchPos);
            if(col2D != null) {
                Debug.Log(col2D);
                Debug.Log(menuButton.gameObject);
                if(col2D.gameObject == menuButton.gameObject) {
                    transform.localScale = transform.localScale * 0.9f;
                    menuButton.color = new Color(150, 150, 150);
                    if(Time.timeScale <= 0.0f) {
                        ButtonClick();
                    }
                    else {
                        Invoke("ButtonClick", 0.2f);
                    }
                }
            }
        }
	}

    void ButtonClick() {
        GetComponent<AudioSource>().Play();
        transform.localScale = orgLocalScale;
        menuButton.color = orgColor;
        menuScript.SendMessage(message, this);
    }

    public void SetLabelText(string text) {
        transform.Find("Label").GetComponent<TextMesh>().text = text;
    }

    public static MenuObjectButton FindMessage(GameObject form, string message) {
        MenuObjectButton[] buttonList = form.transform.GetComponentsInChildren<MenuObjectButton>();
        foreach(MenuObjectButton button in buttonList) {
            if(message == button.message) {
                return button;
            }
        }
        return null;
    }
}
