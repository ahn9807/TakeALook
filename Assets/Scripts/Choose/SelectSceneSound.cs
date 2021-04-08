using UnityEngine;
using System.Collections;

public class SelectSceneSound : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        SoundController.singleton.Play("selectClip");
    }

    // Update is called once per frame
    private void OnDestroy()
    {

    }
}
