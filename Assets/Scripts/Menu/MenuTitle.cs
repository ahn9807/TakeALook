using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuTitle : MonoBehaviour {
    string jumpSceneName;
    public GameObject SceneFadeControl;

    void Start() {
        SceneFadeControl.GetComponent<SceneFade>().FadeIn(1);
        SoundController.singleton.Play("titleClip");
    }

    void Button_Doub(MenuObjectButton button) {
        SceneFadeControl.GetComponent<SceneFade>().FadeOut(0.5f);
        jumpSceneName = "Select_Doub";
        SoundController.singleton.Stop(0.2f);
        Invoke("SceneJump", 0.5f);
    }

    void Button_Setting() {
        SceneFadeControl.GetComponent<SceneFade>().FadeOut(0.5f);
        jumpSceneName = "Select_Infinite";
        SoundController.singleton.Stop(0.2f);
        Invoke("SceneJump", 0.5f);
    }

    void Button_Single() {
        SceneFadeControl.GetComponent<SceneFade>().FadeOut(0.5f);
        jumpSceneName = "Select_Single";
        SoundController.singleton.Stop(0.2f);
        Invoke("SceneJump", 0.5f);
    }

    void Button_Ranking() {
        SceneFadeControl.GetComponent<SceneFade>().FadeOut(0.5f);
        jumpSceneName = "Ranking";
        Invoke("SceneJump", 0.5f);
    }

    void Button_Help()
    {
        SceneFadeControl.GetComponent<SceneFade>().FadeOut(0.5f);
        jumpSceneName = "Help";
        Invoke("SceneJump", 0.5f);
    }

    void Button_Status() {
        SceneFadeControl.GetComponent<SceneFade>().FadeOut(0.5f);
        jumpSceneName = "Status";
        Invoke("SceneJump", 0.5f);
    }

    void Button_End() {
        Application.Quit();
    }


    void SceneJump() {
        Debug.Log(string.Format("start Game : {0}", jumpSceneName));
        SceneManager.LoadScene(jumpSceneName);
    }
}
