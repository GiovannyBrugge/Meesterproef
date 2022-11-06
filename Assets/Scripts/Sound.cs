using UnityEngine;

[System.Serializable]
public class Sound
{
    public string audioName;
    //Insert the audio file in here
    public AudioClip audioFile;
    //Customize audio volume
    [Range(0f, 1f)]
    public float volume;
    //Loops the audio clip
    public bool loop;
    //Sets automatically from where the audio source is played
    [HideInInspector]
    public AudioSource source;
}