using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReadyScene : MonoBehaviour {
    public Button btn;

	// Use this for initialization
	void Start () {
        btn.onClick.AddListener(OnJumpScene);
        SoundController.singleton.Play("introClip");
	}

	
	// Update is called once per frame
	void Update () {
		
	}

    void OnJumpScene() {
        gameObject.GetComponent<AudioSource>().Play();
        SoundController.singleton.Stop(0.2f);
        SceneManager.LoadScene("Menu_Title");
    }
}
