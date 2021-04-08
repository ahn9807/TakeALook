using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingSceneManager : MonoBehaviour {

    string rankingDes;
    string tempTxt;

    public Button buttonReturn;
    public Text rankingTxt;


	// Use this for initialization
	void Start () {
        buttonReturn.onClick.AddListener(OnReturn);
        // rankingDes 불러오기
        rankingDes = "1: 2342342424234";
        rankingTxt.text = rankingDes;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnReturn() {
        SceneManager.LoadScene("Menu_Title");
    }
}
