using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager:IListener,IValueSender{

    static AudioManager instance;

    List<AudioClip> audioList = new List<AudioClip>();
    AudioSource bgAudioSource;
    AudioSource actionAudioSource;

    public static AudioManager GetInstance(){
        if (instance == null)
            instance = new AudioManager();
        return instance;
    }
        
    public void Init(){
        audioList = new List<AudioClip>(Resources.LoadAll<AudioClip>("Audio/"));
        if (GameObject.Find("[BgAudioSource]") == null)
        {
            var obj = new GameObject("[BgAudioSource]");
            Object.DontDestroyOnLoad(obj);
            bgAudioSource = obj.AddComponent<AudioSource>();
        }
        if (GameObject.Find("[ActionAudioSource]") == null)
        {
            var obj = new GameObject("[ActionAudioSource]");
            Object.DontDestroyOnLoad(obj);
            actionAudioSource = obj.AddComponent<AudioSource>();
        }
        InitAudioSource();
        //PlayBgAudio();

        Mediator.AddListener(this, "playAudio","soundOn","soundOff","playBgAudio");
        Mediator.AddValue(this, "soundOn");
    }

    public void OnNotify(string notify,object args){
        switch(notify){
            case "playAudio":
                PlayAudio(args.ToString());
                break;
            case "soundOn":
                bgAudioSource.volume = 1f;
                actionAudioSource.volume = 1f;
                break;
            case "soundOff":
                bgAudioSource.volume = 0f;
                actionAudioSource.volume = 0f;
                break;
            case "playBgAudio":
                PlayBgAudio();
                break;
        }
    }

    public object OnGetValue(string valueType){
        switch(valueType){
            case "soundOn":
                if (bgAudioSource.volume == 1.0f)
                    return true;
                else
                    return false;
            default:
                return null;
        }
    }

    void InitAudioSource(){
        if (bgAudioSource == null || actionAudioSource == null)
            return;
        bgAudioSource.playOnAwake = true;
        bgAudioSource.loop = true;

        actionAudioSource.playOnAwake = false;
        actionAudioSource.loop = false;
    }

    void PlayBgAudio(){
        if (bgAudioSource == null)
            return;
        bgAudioSource.clip = GetAudio("BGM");
        bgAudioSource.Play();
    }

    AudioClip GetAudio(string audioName){
        foreach (var audio in audioList)
        {
            if (audio.name == audioName)
                return audio;
        }
        Debug.LogWarning("dont have this audio");
        return null;
    }

    public void PlayAudio(string audioName){
        if (GetAudio(audioName) == null)
            return;
        actionAudioSource.clip = GetAudio(audioName);
        actionAudioSource.Play();
    }

    public void PlayAudio(AudioClip audioClip){
        if (audioClip == null)
            return;
        actionAudioSource.clip = audioClip;
        actionAudioSource.Play();
    }
}
