using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseButtonControl : MonoBehaviour
{
    public enum CHARACTER
    {
        GIST,
        KAIST,
        UNIST,
        PHONICS
    };

    private int currentState = 0;
    string jumpSceneName;
    public GameObject SceneFadeControl;
    public GameController gameController;

    public HUD_Choose_Controller HUD_Gist;
    public HUD_Choose_Controller HUD_Kaist;
    public HUD_Choose_Controller HUD_Phonics;
    public HUD_Choose_Controller HUD_Unist;

    void Start()
    {
        gameController = GameController.instance;
        SceneFadeControl.GetComponent<SceneFade>().FadeIn(1);
    }

    private void Update()
    {
        if(currentState == 2) {
            jumpSceneName = "Main_Doub";
            Invoke("SceneJump", 2);
        }
    }

    void Button_Gist()
    {
        gameController.SetCharacter((int)CHARACTER.GIST);
        currentState++;
        HUD_Gist.SetState(currentState);
    }

    void Button_Kaist()
    {
        gameController.SetCharacter((int)CHARACTER.KAIST);
        currentState++;
        HUD_Kaist.SetState(currentState);
    }

    void Button_Phonics()
    {
        gameController.SetCharacter((int)CHARACTER.PHONICS);
        currentState++;
        HUD_Phonics.SetState(currentState);
    }

    void Button_Unist() {
        gameController.SetCharacter((int)CHARACTER.UNIST);
        currentState++;
        HUD_Unist.SetState(currentState);
    }

    void SceneJump()
    {
        Debug.Log(string.Format("start Game : {0}", jumpSceneName));
        SceneManager.LoadScene(jumpSceneName);
    }
}
