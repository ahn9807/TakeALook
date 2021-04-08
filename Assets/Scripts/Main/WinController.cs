using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinController : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetWin()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        GetComponent<AudioSource>().Play();
    }

    public void DeleteWin()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }
}
