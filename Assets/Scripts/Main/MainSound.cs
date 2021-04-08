using UnityEngine;
using System.Collections;

public class MainSound : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        SoundController.singleton.Stop(0.2f);
    }
}
