using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public bool DebugLog = false;
    public bool DontDestoryObjectOnLoad = true;
    public string SoundFolder = "";

    const string SoundGroupNID = "SoundGroup_";

    private void Awake()
    {
        if(DontDestoryObjectOnLoad) {
            DontDestroyOnLoad(this);
        }
    }

    public bool CreatGroup(string name) {
        GameObject go = new GameObject();
        go.name = SoundGroupNID + name;
        go.transform.parent = transform;
        return false;
    }

    public GameObject GetGroup(string name)
    {
        return GameObject.Find(SoundGroupNID + name);
    }

    public AudioSource LoadResourceSound(string groupName, string fileName) {
        GameObject goSound = transform.Find(SoundGroupNID + groupName).gameObject;
        AudioSource audioSource = goSound.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        AudioClip audioClip = Resources.Load(SoundFolder + fileName, typeof(AudioClip)) as AudioClip;
        audioSource.clip = audioClip;
        return audioSource;
    }

    public AudioSource FindAudioSource(string groupName, string soundName) {
        GameObject goSound = transform.Find(SoundGroupNID + groupName).gameObject;
        AudioSource[] audioSourceList = goSound.GetComponents<AudioSource>();

        foreach(AudioSource audioSource in audioSourceList) {
            if(audioSource.clip.name == soundName) {
                return audioSource;
            }
        }
        return null;
    }

    public AudioSource[] FindAudioSource(string groupName) {
        GameObject goSound = transform.Find(SoundGroupNID + groupName).gameObject;
        return goSound.GetComponents<AudioSource>();
    }

    public void Play(AudioSource audioSource, bool loop) {
        audioSource.loop = loop;
        audioSource.Play();
    }

    public void Play(string groupName, string soundName, bool loop) {
        AudioSource audio = FindAudioSource(groupName, soundName);
        if(audio) {
            Play(audio, loop);
        }
    }

    public void PlayDontOverride(AudioSource audioSource, bool loop) {
        if(!audioSource.isPlaying) {
            audioSource.loop = loop;
            audioSource.Play();
        }
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
