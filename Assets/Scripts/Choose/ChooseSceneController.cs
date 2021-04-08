using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseSceneController : MonoBehaviour {
    public GameObject FadeImage;
	// Use this for initialization
	void Start () {
        StartCoroutine(ImageFade());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator ImageFade() {
        SpriteRenderer sprite = FadeImage.GetComponent<SpriteRenderer>();
        for (float i = 0; i <= 0.2; i += Time.deltaTime/1)
        {
            sprite.color = new Color(i, i, i, 1);
            yield return null;
        }
    }
}
