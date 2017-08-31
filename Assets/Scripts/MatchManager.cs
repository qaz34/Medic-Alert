using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    public GameModes _Gamemode; 
    [SerializeField]
    public static GameModes GM;
    public static MatchManager instance;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (MatchManager.instance == null)
        {
            MatchManager.instance = this;
        }
        else
        {
            Debug.LogWarning("A previously awakened GameModes MonoBehaviour exists!", gameObject);
        }
        if (MatchManager.GM == null)
        {
            MatchManager.GM = _Gamemode;
        }
    }

}