using System;
using UnityEngine;

public class SaveData {
    const float SaveDataVersion = 0.30f;

    public static string SaveDate = "(non)";
    // Hi Score
    public static int[] HiScore = new int[10];

    // Status
    public static bool[] Status = new bool[48];

    // 저장 데이터 검사
    public int[] DeserializeInt(string input) {
        int[] returnval;
        string[] datas = input.Split(' ');
        Debug.Log(datas.Length);

        returnval = new int[datas.Length];

        if(datas.Length != 10) {
            PlayerPrefs.DeleteKey("HiScore");
            returnval = new int[10];
            for (int i = 0; i < 10;i++) {
                returnval[i] = 0;
            }
        }

        for (int i = 0; i < datas.Length;i++) {
            try {
                returnval[i] = Int32.Parse(datas[i]);
            }
            catch {
                returnval[i] = 0;
            }
        }

        return returnval;
    }

    public bool[] DeserializeBool(string input)
    {
        bool[] returnval;
        string[] datas = input.Split(' ');
        Debug.Log(datas.Length);

        returnval = new bool[datas.Length];

        if (datas.Length != 48)
        {
            PlayerPrefs.DeleteKey("Status");
            returnval = new bool[48];
            for (int i = 0; i < 48; i++)
            {
                returnval[i] = false;
            }
        }

        for (int i = 0; i < datas.Length; i++)
        {
            try
            {
                returnval[i] = Boolean.Parse(datas[i]);
            }
            catch
            {
                returnval[i] = false;
            }
        }

        return returnval;
    }

    public string SerrializeInt(int[] input)
    {
        string datas = "";

        for (int i = 0; i < input.Length-1; i++)
        {
            datas += input[i].ToString() + " ";
        }
        datas += input[input.Length-1];

        return datas;
    }

    public string SerializeBool(bool[] input) {
        string datas = "";

        for (int i = 0; i < input.Length - 1; i++)
        {
            datas += input[i].ToString() + " ";
        }
        datas += input[input.Length - 1];

        return datas;
    }

    public void SaveScore(int[] data) {
        PlayerPrefs.SetString("HighScore", SerrializeInt(data));
        PlayerPrefs.Save();
    }

    public void SaveStatus(bool[] data) {
        PlayerPrefs.SetString("Status", SerializeBool(data));
    }

    public int[] LoadScore()
    {
        string data = PlayerPrefs.GetString("HighScore", "0 0 0 0 0 0 0 0 0 0");
        HiScore = DeserializeInt(data);
        return HiScore;
    }

    public bool[] LoadStatus() 
    {
        string data = PlayerPrefs.GetString("Status", "False");
        Status = DeserializeBool(data);
        return Status;
    }
}