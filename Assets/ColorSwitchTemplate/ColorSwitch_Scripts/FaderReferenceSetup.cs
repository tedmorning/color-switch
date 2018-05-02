using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// This Class setups a way to reference the FaderCanvas and the RawImage which is a child of the FaderCanvas.  This Class is attached to
/// the RawImage Child.
/// </summary>
public class FaderReferenceSetup : MonoBehaviour 
{
    public RawImage faderRawImage;
    public GameObject parentCanvas;
    public static FaderReferenceSetup ourFaderReference;
    public static bool applicationIsQuiting;

    // Use this for pre-initialization
    void Awake()
    {
        //Assign the Parent GameObject to the parentCanvas var.
        parentCanvas = this.transform.parent.gameObject;
        //make sure that it persists thru scene load using DontDestroyOnLoad
        DontDestroyOnLoad(parentCanvas);

        //Make this instance the
        if (ourFaderReference == null)
        {
            ourFaderReference = this;
        }
        else if (ourFaderReference != this)
        {
            Destroy(gameObject);
        }
        //the RawImage this script is attached to is assigned to the faderRawImage var
        faderRawImage = this.GetComponent<RawImage>();

    }

    //this was setup when there was a issue of Unity destroying the RawImage/Canvas before Destroying the ScreenFaderSingleton OnApplicationQuit
    //(During editor testing). This is in place so that OnDestroy(of the Image or Canvas), I can set applicationIsQuitting to true.  
    //So that when the fader is using CrossFadeAlpha to fade the screen, it doesnt get stuck in a situation where it is trying to CrossFade the alpha of
    //this RawImage after/while its being destroyed, and before the ScreenFaderSingleton is Destroyed.  That would result in a NullReferenceException.
    void OnDestroy()
    {
        applicationIsQuiting = true;
    }

}
