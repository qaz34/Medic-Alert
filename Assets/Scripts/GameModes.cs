using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModes : ScriptableObject
{
    public GameObject currentGameMode;
    public GameObject defaultWeapon;
    public int currentRound;
    public Settings settings;
    [HideInInspector]
    public Settings currentSettings;


    // this is the designer set data for each game mode
    [System.Serializable]
    public struct Settings
    {
        [Tooltip("GameMode title")]
        public string name;
        [Tooltip("Amount of players per team")]
        public int maxTeamSize;
        public int amountOfTeams;
        public int scoreNeededToWin;
        public float deathMultiplier;
        [Tooltip("How many rounds")]
        public int roundCount;
        [Tooltip("In game countdown timer - set to -1 if no timer needed")]
        public float timer;
       
    }

    void Update()
    {
        if(settings.timer == 0)
        {
            
        }
    }


}
