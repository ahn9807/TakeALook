using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AISelect : MonoBehaviour
{
    public enum CHARACTER
    {
        GIST,
        KAIST,
        UNIST,
        PHONICS
    };

    private int currentState = 0;
    public string jumpSceneName;
    public GameObject SceneFadeControl;
    public GameController gameController;

    void Start()
    {
        SceneFadeControl.GetComponent<SceneFade>().FadeIn(1);
        OnAISelect();
    }

    void Button_Gist()
    {
        gameController.playerChar = PlayerController.CHARACTER.GIST;
        SceneJump();
    }

    void Button_Kaist()
    {
        gameController.playerChar = PlayerController.CHARACTER.KAIST;
        SceneJump();
    }

    void Button_Phonics()
    {
        gameController.playerChar = PlayerController.CHARACTER.PHONICS;
        SceneJump();
    }

    void Button_Unist()
    {
        gameController.playerChar = PlayerController.CHARACTER.UNIST;
        SceneJump();
    }

    void SceneJump()
    {
        SceneManager.LoadScene(jumpSceneName);
    }

    void OnAISelect()
    {
        int rnd_num = Random.Range(0, 4);
        switch (rnd_num) {
            case 0:
                gameController.AIChar = AIPlayer.CHARACTER.GIST;
                break;
            case 1:
                gameController.AIChar = AIPlayer.CHARACTER.KAIST;
                break;
            case 2:
                gameController.AIChar = AIPlayer.CHARACTER.PHONICS;
                break;
            case 3:
                gameController.AIChar = AIPlayer.CHARACTER.UNIST;
                break;
        }
    }
}
