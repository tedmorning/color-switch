using UnityEngine;
using System.Collections;

/// <summary>
/// Simple Class used to Load a level, while making use of the screenFader
/// </summary>
public class LoadGameLevel : MonoBehaviour {

    /// <summary>
    /// Simple Method which communicates with the ScreenFaderSingleton and calls one
    /// of its Methods.  The ScreenFaderSingleton contains several methods that control
    /// fading and scene loading.
    /// </summary>
    public void PlayGameButton()
    {
        //call the FadeAndLoadLevelFaster Method (the immediate version). Which is best suited
        //to buttons. See that Method Summary for more Info.
        ScreenFaderSingleton.Instance.FadeAndLoadLevelFaster();
    }
}
