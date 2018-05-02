using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Class that handles the stars the player picks up/earns while moving upward and Increments score/plays effects.
/// </summary>
public class ChangeScorePickup : MonoBehaviour
{
    public AudioClip scoreChangeSound;                                  //Sound Clip to play when a Star is Picked Up.
    public float localSoundVolume;                                      //The localSoundVol Var (Assigned to the Value of the TemporaryGameVars.soundVol).
    public Text scoreText;                                              //The score text that is on the GameCanvas during play.
    public GameObject effectToInstantiate, effectToInstantiate2;        //The 2 particle systems to Instantiate on player death.

    // Use this for initialization
    void Start()
    {
        localSoundVolume = TemporaryGameVars.soundVolume * 3f;
        scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();
    }

    //If the player collides with this trigger object, then we increase the score.
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IncreaseScore();
        }
    }

    /// <summary>
    /// Method that Handles Incrementing the score and playing the "pickup" effects.
    /// </summary>
    public void IncreaseScore()
    {
        //increment static var
        TemporaryGameVars.playerScore++;
        
        //change the canvas "score" to the new incremented "playerScore"
        scoreText.text = TemporaryGameVars.playerScore.ToString();

        //if there is a score change Audio Clip, then we can play it.
        if (scoreChangeSound)
        {
            AudioSource.PlayClipAtPoint(scoreChangeSound, transform.position, localSoundVolume * 2f);
        }

        //Instantiate the 2 particle systems.  One is the "stars spawn and fall effect", and the other is the "rising +1"
        Instantiate(effectToInstantiate, transform.position, transform.rotation);
        Instantiate(effectToInstantiate2, transform.position, transform.rotation);

        //disable the StarPickUp Object(ScoreChangePickup) rendering it invisible, and ready for use by the pooler again.
        gameObject.SetActive(false);
    }

}
