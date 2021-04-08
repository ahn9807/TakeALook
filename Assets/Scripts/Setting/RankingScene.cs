using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RankingScene : MonoBehaviour
{
    SaveData saveData = new SaveData();
    public int[] highScores;
    public Text rankingTxt;
    public string text;
    public Button returnButton;

    // Use this for initialization
    void Start()
    {
        highScores = saveData.LoadScore();
        text += "Ranking\n----------------------------\n";
        for (int i = 0; i < highScores.Length;i++) {
            text += "" + string.Format("{0,2}: {1,2}\n",(i+1),highScores[i]);
        }

        returnButton.onClick.AddListener(Return);
    }

    // Update is called once per frame
    void Update()
    {
        rankingTxt.text = text;
    }

    void Return() {
        SoundController.singleton.Stop(0.2f);
        SceneManager.LoadScene("Menu_Title");
    }
}
