using UnityEngine;
using System.Collections;

/// <summary>
/// Static Class that holds some of the Game/Player Data.  At this Time we save some of these
/// values in player prefs.  Later I will setup a BinaryFormatter/FileStream to save the 
/// necessary data to a bin file.
/// </summary>
public static class TemporaryGameVars{

    public static int playerScore = 0;                                                      //the player score

    public static int highestPlayerScore = PlayerPrefs.GetInt("highestPlayerScore");        //the player highest score

    public static float soundVolume = 0.2f;                                                //global sound volume

    public static GameObject screenFaderPrefab =                                            //Reference to the location of ScreenFader Prefab(for editor use only)
        Resources.Load("Prefabs/FaderCanvas(DontDestroyOnLoad)")as GameObject;              //that way the game can be played from any scene, and the prefab will
                                                                                            //be created if its needed and missing from scene(it normally lives in scene0)
                                                                                            //and persists from that point on...

    public static int highestScoreAchieved = PlayerPrefs.GetInt("highestScore");            //Pull the HighScore from playerPrefs            
    //public static int currentRoundHighScore = PlayerPrefs.GetInt("currentScore");           
    //public static float globalSoundVolume = PlayerPrefs.GetFloat("globalSoundVolume");
    public static int mutedVolume = PlayerPrefs.GetInt("mutedAudio");                       //Saves state of the Muted Audio Boolean


}
