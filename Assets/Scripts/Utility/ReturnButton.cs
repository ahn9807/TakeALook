using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ReturnButton : MonoBehaviour {
    private Button returnButton;
    public string sceneName;


	// Use this for initialization
	void Start () {
        returnButton = gameObject.GetComponent<Button>();
        returnButton.onClick.AddListener(Return);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Return() {
        SoundController.singleton.Stop(0);
        SceneManager.LoadScene(sceneName);
    }
}
