using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour 
{
    public AudioClip[] _musicClips;

	// Use this for initialization
	void Start () 
    {
        if (_musicClips.Length < 4)
        {
            Debug.LogWarning("Need 4 music tracks in the inspector!");
        }

        audio.loop = true;
        audio.clip = _musicClips[0];
        audio.Play();
	}


    public void PlayMusicTrack(int trackNumber)
    {
        audio.Stop();
        audio.clip = _musicClips[trackNumber];
        audio.Play();
    }
}
