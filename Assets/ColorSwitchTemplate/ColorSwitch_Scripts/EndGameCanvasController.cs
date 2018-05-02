using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GoogleMobileAds;
using GoogleMobileAds.Api;

/// <summary>
/// This Class Updates the player scores on the EndGameCanvas, and Holds the MainMenu & Replay Button Methods that
/// are fired when those buttons are clicked on the EndGameCanvas.
/// </summary>
public class EndGameCanvasController : MonoBehaviour {

    private Canvas endGameCan;                  //Reference to EndGameCanvas
    public Text currentScore, highestScore;     //Reference to the EndGameCanvas' "Text" objects.(highScore&currentScore)

	private InterstitialAd interstitial;

    // Use this for initialization
	
	private void RequestInterstitial()
    {
        string adUnitId = "ca-app-pub-9738690958049285/3835313056";
        // Create an interstitial.
        interstitial = new InterstitialAd(adUnitId);
        // Load an interstitial ad.
		AdRequest request = new AdRequest.Builder().Build();
        interstitial.LoadAd(request);
    }
	
	private void ShowInterstitial()
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }
    }
	
    // Use this for pre-initialization
    void Awake()
    {
        //Find our Text gameobjects
        currentScore = GameObject.Find("CurrentScoreField").GetComponent<Text>();
        highestScore = GameObject.Find("HighestScoreField").GetComponent<Text>();
    }

    // Use this for initialization
    void Start () 
	{
        //Setup reference to EndGameCanvas
        endGameCan = GetComponent<Canvas>();
		RequestInterstitial();
    }

    // Update is called once per frame
    void Update () 
	{
        //if the EndGameCanvas is enabled(because the player controller activated it on player death)
        if(endGameCan.enabled == true)
        {
            //Update the player scores
            highestScore.text = TemporaryGameVars.highestPlayerScore.ToString();
            currentScore.text = TemporaryGameVars.playerScore.ToString();
        }
    }

    /// <summary>
    /// Method that the Replay Button calls when clicked.(It then calls FadeAndReloadLevel() on our ScreenFaderSingleton)
    /// </summary>
    public void ReplayButtonPress()
    {
        ScreenFaderSingleton.Instance.FadeAndReloadLevel();
		ShowInterstitial();
		
    }

    /// <summary>
    /// Method that the Main Menu Button calls when clicked.(It then calls FadeAndLoadPreviousLevel() on our ScreenFaderSingleton)
    /// </summary>
    public void MainMenuButtonPress()
    {
        ScreenFaderSingleton.Instance.FadeAndLoadPreviousLevel();
    }
}
