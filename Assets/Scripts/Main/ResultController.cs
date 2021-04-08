using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ResultController : MonoBehaviour {
    public bool leftWin = false;
    public bool rightWin = false;
    public int player1score = 0;
    public int player2score = 0;
    public int infPoints = 0;
    public Text story;

    public GameController.CHARACTER leftChar;
    public GameController.CHARACTER rightChar;

    public Sprite dogBack;
    public Sprite catBack;

    bool flag = false;

    // Update is called once per frame
    void Update () {
        SetText();
    }
    private void OnDisable()
    {
        flag = false;
    }

    private void FixedUpdate()
    {
        if (leftWin)
        {
            if (GameController.instance.GetPlayer1() == 0 || GameController.instance.GetPlayer1() == 1)
            {
                gameObject.GetComponentInChildren<SpriteRenderer>().sprite = catBack;
            }
            else
            {
                gameObject.GetComponentInChildren<SpriteRenderer>().sprite = dogBack;
            }
        }
        else if (rightWin)
        {
            if (GameController.instance.GetPlayer2() == 0 || GameController.instance.GetPlayer2() == 1)
            {
                gameObject.GetComponentInChildren<SpriteRenderer>().sprite = catBack;
            }
            else
            {
                gameObject.GetComponentInChildren<SpriteRenderer>().sprite = dogBack;
            }
        }
        else
        {
            Debug.Log("Error");
        }
    }


    private void SetText() {
        infPoints = GameController.instance.points;
        int randNum = UnityEngine.Random.Range(0, 3);
        SaveData statusData = new SaveData();
        bool[] statusDatas = statusData.LoadStatus();
        if(statusDatas.Length == 1) {
            statusDatas = new bool[48];
        }


        if (SceneManager.GetActiveScene().name == "Main_Infinite" && flag == false) {
            story.text = "Your score is " + infPoints + "\n(This is Debug Message)";
            SaveData saveData = new SaveData();
            int[] data = saveData.LoadScore();
            int[] temp = new int[data.Length + 1];
            for (int i = 0; i < data.Length;i++) {
                temp[i] = data[i];
            }
            temp[data.Length] = infPoints;
            Array.Sort(temp);
            Array.Reverse(temp);

            for (int i = 0; i < data.Length;i++) {
                data[i] = temp[i];
            }
            saveData.SaveScore(data);

            flag = true;

            return;
        }

        if (GameController.instance.GetPlayer1() == 0)
        {
            leftChar = GameController.CHARACTER.GIST;
        }
        else if (GameController.instance.GetPlayer1() == 1)
        {
            leftChar = GameController.CHARACTER.KAIST;
        }
        else if (GameController.instance.GetPlayer1() == 2)
        {
            leftChar = GameController.CHARACTER.UNIST;
        }
        else if (GameController.instance.GetPlayer1() == 3)
        {
            leftChar = GameController.CHARACTER.PHONICS;
        }

        if (GameController.instance.GetPlayer2() == 0)
        {
            rightChar = GameController.CHARACTER.GIST;
        }
        else if (GameController.instance.GetPlayer2() == 1)
        {
            rightChar = GameController.CHARACTER.KAIST;
        }
        else if (GameController.instance.GetPlayer2() == 2)
        {
            rightChar = GameController.CHARACTER.UNIST;
        }
        else if (GameController.instance.GetPlayer2() == 3)
        {
            rightChar = GameController.CHARACTER.PHONICS;
        }

        Debug.Log(GameController.instance.GetPlayer1());
        Debug.Log(GameController.instance.GetPlayer2());

        if (rightWin)
        {
            var temp = leftChar;
            leftChar = rightChar;
            rightChar = temp;
        }

       
        if (leftChar == GameController.CHARACTER.GIST)
        {
            if (rightChar == GameController.CHARACTER.GIST)
            {
                story.text = "저런..껄룩이네 성 주민들이 가시에 찔려 죽었어요 :(";
            }
            else if (rightChar == GameController.CHARACTER.KAIST)
            {
                story.text = "저런..공돌이 껄룩이 성 주민들이 가시에 찔려 죽었어요 :(연애도 못해보고 죽다니 불쌍해라";
            }
            else if (rightChar == GameController.CHARACTER.PHONICS)
            {
                story.text = "저런..공돌이 댕댕이 성 주민들이 가시에 찔려 죽었어요 :(연애도 못해보고 죽다니 불쌍해라";
            }
            else if (rightChar == GameController.CHARACTER.UNIST)
            {
                story.text = "저런..댕댕이 성 주민들이 가시에 찔려 죽었어요 :(";
            }
        }
        else if (leftChar == GameController.CHARACTER.KAIST)
        {
            if (rightChar == GameController.CHARACTER.GIST)
            {
                story.text = "저런..껄룩이 성 주민들이 공돌이 껄룩이들의 한맺힌 가시 스파이크에 찔려 죽었어요 :(";
            }
            else if (rightChar == GameController.CHARACTER.KAIST)
            {
                story.text = "저런..공돌이 껄룩이 성 주민들이 공돌이 껄룩이들의 한맺힌 가시 스파이크에 찔려 죽었어요 :( 연애도 못해보고 죽다니 불쌍해라";
            }
            else if (rightChar == GameController.CHARACTER.PHONICS)
            {
                story.text = "저런..공돌이 댕댕이 성 주민들이 공돌이 껄룩이들의 한맺힌 가시 스파이크에 찔려 죽었어요 :( 연애도 못해보고 죽다니 불쌍해라";
            }
            else if (rightChar == GameController.CHARACTER.UNIST)
            {
                story.text = "저런..댕댕이 성 주민들이 공돌이 껄룩이들의 한맺힌 가시 스파이크에 찔려 죽었어요 :(";
            }
        }
        else if (leftChar == GameController.CHARACTER.PHONICS)
        {
            if (rightChar == GameController.CHARACTER.GIST)
            {
                story.text = "저런..껄룩이 성 주민들이 공돌이 댕댕이들의 한맺힌 뼈다귀 세례에 맞아 죽었어요 :(";
            }
            else if (rightChar == GameController.CHARACTER.KAIST)
            {
                story.text = "저런..공돌이 껄룩이 성 주민들이 공돌이 댕댕이들의 한맺힌 뼈다귀 세례에 맞아 죽었어요 :( 연애도 못해보고 죽다니 불쌍해라";
            }
            else if (rightChar == GameController.CHARACTER.PHONICS)
            {
                story.text = "저런..공돌이 댕댕이 성 주민들이 공돌이 댕댕이들의 한맺힌 뼈다귀 세례에 맞아 죽었어요 :( 연애도 못해보고 죽다니 불쌍해라";
            }
            else if (rightChar == GameController.CHARACTER.UNIST)
            {
                story.text = "저런..댕댕이 성 주민들이 공돌이 댕댕이들의 한맺힌 뼈다귀 세례에 맞아 죽었어요 :(";
            }
        }
        else if (leftChar == GameController.CHARACTER.UNIST)
        {
            if (rightChar == GameController.CHARACTER.GIST)
            {
                story.text = "저런..껄룩이 성 주민들이 뼈다귀에 맞아 죽었어요 :(";
            }
            else if (rightChar == GameController.CHARACTER.KAIST)
            {
                story.text = "저런..공돌이 껄룩이 성 주민들이 뼈다귀에 맞아 죽었어요 :( 연애도 못해보고 죽다니 불쌍해라";
            }
            else if (rightChar == GameController.CHARACTER.PHONICS)
            {
                story.text = "저런..공돌이 댕댕이 성 주민들이 뼈다귀에 맞아 죽었어요 :( 연애도 못해보고 죽다니 불쌍해라";
            }
            else if (rightChar == GameController.CHARACTER.UNIST)
            {
                story.text = "저런..댕댕이 성 주민들이 뼈다귀에 맞아 죽었어요 :(";
            }
        }

        /*
        if (leftChar == GameController.CHARACTER.GIST)
        {
            if (rightChar == GameController.CHARACTER.GIST)
            {
                story.text = "Kitty villagers were prinked by fishbones and killed :(";
            }
            else if (rightChar == GameController.CHARACTER.KAIST)
            {
                story.text = "Geek kitty villagers were prinked by fishbones and killed :( How sad..";
            }
            else if (rightChar == GameController.CHARACTER.PHONICS)
            {
                story.text = "Geek doggy villagers were prinked by fishbones and killed :( How sad..";
            }
            else if (rightChar == GameController.CHARACTER.UNIST)
            {
                story.text = "Doggy village villagers were prinked by fishbones and killed :( ";
            }
        }
        else if (leftChar == GameController.CHARACTER.KAIST)
        {
            if (rightChar == GameController.CHARACTER.GIST)
            {
                story.text = "Kitty villagers were hit by fishbones of sorrowful geek kitty villagers and killed :(";
            }
            else if (rightChar == GameController.CHARACTER.KAIST)
            {
                story.text = "Geek kitty villagers were hit by fishbones of sorrowful geek kitty villagers and killed :( How sad..";
            }
            else if (rightChar == GameController.CHARACTER.PHONICS)
            {
                story.text = "Geek doggy villagers were hit by fishbones of sorrowful geek kitty villagers and killed :( How sad..";
            }
            else if (rightChar == GameController.CHARACTER.UNIST)
            {
                story.text = "Doggy villagers were hit by fishbones of sorrowful geek kitty villagers and killed :(";
            }
        }
        else if (leftChar == GameController.CHARACTER.PHONICS)
        {
            if (rightChar == GameController.CHARACTER.GIST)
            {
                story.text = "Kitty villagers were hit by bones of sorrowful geek doggy villagers and killed :(";
            }
            else if (rightChar == GameController.CHARACTER.KAIST)
            {
                story.text = "Geek kitty villagers were hit by bones of sorrowful geek doggy villagers and killed :( How sad..";
            }
            else if (rightChar == GameController.CHARACTER.PHONICS)
            {
                story.text = "Geek doggy villagers were hit by bones of sorrowful geek doggy villagers :( How sad..";
            }
            else if (rightChar == GameController.CHARACTER.UNIST)
            {
                story.text = "Doggy villagers were hit by bones of sorrowful geek doggy villagers :(";
            }
        }
        else if (leftChar == GameController.CHARACTER.UNIST)
        {
            if (rightChar == GameController.CHARACTER.GIST)
            {
                story.text = "Kitty villagers were hit by bones and killed :(";
            }
            else if (rightChar == GameController.CHARACTER.KAIST)
            {
                story.text = "Geek kitty villagers were hit by bones and killed :( How sad..";
            }
            else if (rightChar == GameController.CHARACTER.PHONICS)
            {
                story.text = "Geek doggy villagers were hit by bones and killed :( How sad..";
            }
            else if (rightChar == GameController.CHARACTER.UNIST)
            {
                story.text = "Doggy villagers were hit by bones and killed :( ";
            }
        }
        */
    }
}


