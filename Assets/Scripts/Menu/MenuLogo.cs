using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLogo : MonoBehaviour
{
    public int a;
    public float posX = -100;

    private GameObject sceneFade;
    public GameObject logo;
    private bool fadeOut = false;

    // Use this for initialization
    /*
    void Start()
    {
        sceneFade = GameObject.Find("SceneFadeControl");
        sceneFade.GetComponent<SceneFade>().FadeIn(6);
    }

    private void FixedUpdate()
    {
        logo.GetComponent<Transform>().position = new Vector2(0, posX);
        if (posX < 0 && fadeOut == false)
            posX += Time.fixedDeltaTime * 20;
        else
            StartCoroutine(PauseLogo(3));
    }

    IEnumerator PauseLogo(int time) {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("Menu_Ready");
    }
    */

    void Start()
    {
        sceneFade = GameObject.Find("SceneFadeControl");
        sceneFade.GetComponent<SceneFade>().FadeIn(1);
        StartCoroutine(PauseLogo(2));
    }

    IEnumerator PauseLogo(int time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("Menu_Ready");
    }
}
