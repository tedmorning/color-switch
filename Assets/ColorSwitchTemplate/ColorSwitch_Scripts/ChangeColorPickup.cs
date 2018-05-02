using UnityEngine;
using System.Collections;

/// <summary>
/// Class that handles the colorChange items the player picks up while moving upward and it changes the player color
/// AND progresses the Obstacle Spawn... without picking up these items, the Obstacles would NOT continue to spawn.
/// </summary>
public class ChangeColorPickup : MonoBehaviour
{
    public AudioClip colorChangeSound;                      
    public float localSoundVolume;                          
    public PlayerController pController;                    
    public ColorColliderController cColliderController;     
    //public GameObject effectToInstantiate;

    void Start()
    {
        localSoundVolume = TemporaryGameVars.soundVolume * 2f;
        pController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        cColliderController = GameObject.FindGameObjectWithTag("ColliderController").GetComponent<ColorColliderController>();
    }

    //When the Player enters the colorChange objects trigger area Change the Player Color
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Call ChangePlayerColor Method
            ChangePlayerColor();
        }

    }

    /// <summary>
    /// This Method Accesses the playercontroller, and then switches the player color from the playercontroller, next it accesses the 
    /// ColliderController and Runs the ChangeColliderState Mehtod.  Next it plays the colorchange pickup sound and then disables the 
    /// color change pickup object so that it goes back into the pool, and can be used again.
    /// </summary>
    public void ChangePlayerColor()
    {
        //switch color from the playercontroller
        pController.SwitchPlayerColor();
        //Change color collider state from the ColliderController
        cColliderController.ChangeColorColliderState();
        //Increments the "Obstacle Progression" which spawns more obstacles about 2 obstacle lengths above the player
        cColliderController.IncrementObstacleProgression();

        //if there is a sound clip for picking up a color change object... then play it.
        if (colorChangeSound)
        {
            AudioSource.PlayClipAtPoint(colorChangeSound, transform.position, localSoundVolume * 2f);

            //THIS IS OLD CODE--  I used to get the first two obstacles on screen by leaving
            //2 pickup items right on top of the player start position.  So this if/else below
            //prevented that sound from playing before the player actually got moving

            //if (TemporaryGameVars.playerScore == 0)
            //{
            //    Debug.Log("Supressing Score Change Sound");
            //}
            //else
            //{
            //    AudioSource.PlayClipAtPoint(colorChangeSound, transform.position, localSoundVolume * 2f);
            //}

        }

        //instantiate this effect
        //Instantiate(effectToInstantiate, transform.position, transform.rotation);

        //disable the object so that it appears "destroyed" or "PickedUp", and then it will be available in the pool again.
        this.gameObject.SetActive(false);
    }

}
