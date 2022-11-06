using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;

    private void Awake()
    {
        //Checks duplicated audiomanagers
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        //Transfers audiomanagers to other scenes
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioFile;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
        AudioManager.instance.Play("AmbienceStatic");
    }
    //Plays audio from beginning
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.audioName == name);
        s.source.Play();
    }
    //Stops audio completely
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.audioName == name);
        s.source.Stop();
    }
    //Pauses audio untill it's being unpaused
    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.audioName == name);
        s.source.Pause();
    }
    //Resumes audio from where it was being paused
    public void Resume(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.audioName == name);
        s.source.UnPause();
    }
}
