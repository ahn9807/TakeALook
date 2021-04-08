using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultButton : MonoBehaviour
{
    public Button buttonRestart;
    public Button buttonReturn;

    private void Start()
    {
        buttonReturn.onClick.AddListener(Button_Return);
        buttonRestart.onClick.AddListener(Button_Restart);
    }

    void Button_Restart()
    {
        GameController.instance.state = GameController.State.GameIn;
    }

    void Button_Return()
    {
        GameController.instance.state = GameController.State.GameEnd;
    }

}
