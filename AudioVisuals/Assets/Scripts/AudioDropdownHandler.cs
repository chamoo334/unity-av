using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioDropdownHandler : MonoBehaviour
{
    AudioClip[] allSongs;

    // Start is called before the first frame update
    void Start()
    {

        var dropdown = transform.GetComponent<Dropdown>();
        dropdown.options.Clear();

        // fill dropdown with song titels from Resources
        allSongs = Resources.LoadAll<AudioClip>("Audio/GameOptions");
        foreach(var item in allSongs)
        {
            dropdown.options.Add(new Dropdown.OptionData() {text = item.name});
        }

        // add listener for change
        dropdown.onValueChanged.AddListener(delegate { UpdateGameSong(dropdown); });
    }

    void UpdateGameSong( Dropdown dropdown)
    {
        PlayerPrefs.SetInt("musicSelection", dropdown.value);
    }

}
