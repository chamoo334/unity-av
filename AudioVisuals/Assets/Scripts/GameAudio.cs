using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameAudio : MonoBehaviour
{
    AudioSource gameMusic;
    AudioClip[] allAudio; //TODO: DontDestroyOnLoad() allSongs from AudioDropdownHandler
    public static int _audioFrequency;

    /**/
    void Start() 
    {
        gameMusic = GetComponent<AudioSource> ();
        allAudio = Resources.LoadAll<AudioClip>("Audio/GameOptions");

        int selected;

        if(PlayerPrefs.HasKey("musicVolume"))
        {
            LoadAudioVolume();
        } 

        if(PlayerPrefs.HasKey("musicSelection"))
        {
            selected = PlayerPrefs.GetInt("musicSelection");
        } else {
            selected = 2;
        }
        LoadAudioSelection(selected);


    }

    /**/
    void LoadAudioVolume()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("musicVolume");
    }

    /**/
    void LoadAudioSelection(int songIndex)
    {
        gameMusic.clip = allAudio[songIndex];
        gameMusic.Play();
        // Debug.Log(songIndex);
        _audioFrequency = allAudio[songIndex].frequency;
    }
}
