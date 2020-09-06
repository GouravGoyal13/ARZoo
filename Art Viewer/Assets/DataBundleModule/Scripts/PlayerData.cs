using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance { get; private set; }

    public string LastPlayedLevel
    {
        get{ return PlayerPrefs.GetString("LastLevelPlayed", "1"); }
        set{ PlayerPrefs.SetString("LastLevelPlayed", value); }
    }
    // Use this for initialization
    void Awake()
    {
        Instance = this;
    }
}

