using UnityEngine;
using System.Collections;

/// <summary>
/// Simple Script attached to the MainCamera that Calls a Method on the ScreenFaderSingleton, which "Instaniates" the ScreenFaderSingleton.  This 
/// Class has no other purpose... It did have some other logic/Jobs initially, but it has since been simplified. We could really do this on any script.
/// It just so happened that I used to use this script for other funtionaity... so in the end as functionality was moved to other classes, It was 
/// left behind only calling the method and instantiating the ScreenFaderSingleton.
/// </summary>
public class FaderCaller : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
        //Call the DebugSpawn()... that will create the ScreenFaderSingleton gameObject...
        ScreenFaderSingleton.Instance.DebugSpawn();
    }

}
