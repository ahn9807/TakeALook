using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundController : MonoBehaviour
{
    public static SoundController singleton;
    public Queue<AudioSource> audioSources = new Queue<AudioSource>();

    //songs
    public AudioClip introClip;
    public AudioClip titleClip;
    public AudioClip selectClip;
    public AudioClip gameClip;
    public AudioClip resultClip;
    public AudioClip rankingClip;

    // Use this for initialization
    void Awake()
    {
        if(singleton != null) {
            Destroy(gameObject);
        }
        else {
            singleton = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void Play(string audioName) {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSources.Enqueue(audioSource);

        switch (audioName) {
            case "introClip":
                audioSource.clip = introClip;
                break;
            case "titleClip":
                audioSource.clip = titleClip;
                break;
            case "selectClip":
                audioSource.clip = selectClip;
                break;
            case "gameClip":
                audioSource.clip = gameClip;
                break;
            case "resultClip":
                audioSource.clip = resultClip;
                break;
            case "rankingClip":
                audioSource.clip = rankingClip;
                break;
        }

        audioSource.loop = true;
        audioSource.Play();
    }

    public void Stop(float time) {
        StartCoroutine(smoothStop(time));
    }

    IEnumerator smoothStop(float time) {
        float t = time;
        while (t > 0)
        {
            yield return null;
            t -= Time.deltaTime;
            audioSources.Peek().volume = t;
        }
        audioSources.Peek().Stop();
        Destroy(audioSources.Peek());
        audioSources.Dequeue();
        yield break;
    }
}
