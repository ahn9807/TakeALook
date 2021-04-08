using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFade : MonoBehaviour
{
    float r, g, b;
    public RawImage img;

    // Use this for initialization
    public void Start() {
        r = img.color.r;
        g = img.color.g;
        b = img.color.b;
    }

    public void FadeIn(float time) {
        StartCoroutine(FadeImage(true, time));
    }

    public void FadeIn(float time, float a) {
        StartCoroutine(FadeImage(true, time, a));
    }

    public void FadeOut(float time) {
        StartCoroutine(FadeImage(false, time));
    }

    IEnumerator FadeImage(bool isIn, float time)
    {
        if(isIn == false)
        {
            for (float i = 0; i <= 1; i += Time.deltaTime/time)
            {
                img.color = new Color(r, g, b, i);
                yield return null; 
            }
        }
        else
        {
            for (float i = 1; i >= 0; i -= Time.fixedDeltaTime/time)
            {
                img.color = new Color(r, g, b, i);
                yield return null;
            }
        }
    }

    IEnumerator FadeImage(bool isIn, float time, float a)
    {
        if (isIn == false)
        {
            for (float i = 0; i <= a; i += Time.deltaTime / time)
            {
                img.color = new Color(r, g, b, i);
                yield return null;
            }
        }
        else
        {
            for (float i = 1; i >= a; i -= Time.fixedDeltaTime / time)
            {
                img.color = new Color(r, g, b, i);
                yield return null;
            }
        }
    }
}
