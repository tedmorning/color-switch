using UnityEngine;
using UnityEngine.UI;
using System.Collections;


/// <summary>
/// This Enum contains the color values
/// </summary>
public enum PlayerColor
{
    Pink, Purple, Teal, Yellow
};

/// <summary>
/// The Player Controller Class which Controls Player Color, Position, State, Instatiates the Death Effects and Enables the EndGameCanvas.
/// </summary>
public class PlayerController : MonoBehaviour
{
    public bool testingMode;                            //Select this to be Invincible
    public Rigidbody playerRB;                          //Ref to Rigidbody
    public ForceMode upwardForceType;                   //ForceMode to allow easy hand of force type in inspector)
    public float upwardForce;                           //amount of upward force(currently force of 6.75f,Impulse,Physics Gravity -15(not -9.81ms)
    public bool playerActivated;                        //used initially when enabling/disabling gravity on impact and level start
    public bool isPlayerDead;                           //bool that answers... is the player dead? lol
    public PlayerColor playerColor;                     //the pub reference to the playerColor
    public Color[] playerColorArray;                    //Array of colors the player can be.  I used the color dropper on my obstacle assets
    public Color originalColor;                         //player starting color(Get from Renderer.material)
    private Renderer playerRenderer;                    //Player's Sphere Mesh Renderer(originally had diff mesh shapes, and the player rotated)
    public int numOfColors;                             //easy way to keep track of the possible color variations
    public AudioClip deathSound;                        //sound clip to play when dead
    private float localSoundVolume;                     //local sound volume was multiplied by the TemporaryGameVars.soundVolume(done to tweak fx/music bal without mixer)
    public GameObject deathParticles, flashObject;      //the particles systems that are instantiated when player collides/dies
    private Camera mainCam;                             //reference to main scene cam(used to GetComponet the Camera Shake and Chromativ Abberation Scripts)
    private ChromaticAberration chromo;                 //chromatic abberation script(used in conjunction with chromatic abb shader)
    private SimpleCameraShake shake;                    //camera shake script
    private float buttonCount = 1.5f;                   //length to hold the U and I keys to turn player Invinceable(for testing out in the real world) ;-)
    private Animator pAnimator;                         //Ref to player animator.  We play a wiggly anim when player jumps upward.
    private Canvas endGameCanvas;                       //Ref to EndGameCanvas that brings up scores and restart/home buttons

    // Use this for pre-initialization
    void Awake()
    {
        //Get MainCamera
        mainCam = Camera.main;
        //Get EndGameCanvas
        endGameCanvas = GameObject.FindGameObjectWithTag("EndGameCanvas").GetComponent<Canvas>();
        //Disable initially.. we didnt lose yet people!
        endGameCanvas.enabled = false;
        //Get Animator
        pAnimator = gameObject.GetComponentInChildren<Animator>();
        //Get Volume
        localSoundVolume = TemporaryGameVars.soundVolume * 5f;
        //Get renderer from player mesh
        playerRenderer = gameObject.GetComponentInChildren<Renderer>();
        //Set renderer color to one of the playerColor arrays colors using the integer rep of the color enum
        //currently... 0=Pink , 1=Purple , 2=Teal , 3=Yellow ;
        playerRenderer.material.color = playerColorArray[(int)playerColor];

        //Get RigidBody
        playerRB = GetComponent<Rigidbody>();
        //Disable gravity at start
        playerRB.useGravity = false;
    }

    // Use this for initialization
    void Start()
    {
        //use mainCamera reference to get the Chromatic Abberation and Camera Shake References setup;
        chromo = mainCam.GetComponent<ChromaticAberration>();

        shake = mainCam.GetComponent<SimpleCameraShake>();

        //get a count of the colors and store in numOfColors(another integer variable from the enum that will helps us later)
        numOfColors = System.Enum.GetValues(typeof(PlayerColor)).Length;
    }

    // Update is called once per frame
    void Update()
    {
        //this will be the input for Mobile,Web, and Standalone seeing as mobile will count a mouse click as emulated tap
        //this saves us time because for a simple game i feel its overkill to use Input.Touches
        if (Input.GetMouseButtonDown(0))
        {
            //if we havent clicked anywhere yet, when we do the player will be activated
            if (!playerActivated)
            {
                playerActivated = true;
            }
            if (playerActivated && !endGameCanvas.enabled)
            {
                //if we are alive and jumpin turn the gravity back on
                playerRB.useGravity = true;
                //play jump animation when "Jumping"
                pAnimator.SetTrigger("Jump");
                //add our force to the player rigidbody... again... these vars were set in inspector
                playerRB.AddForce(Vector3.up * upwardForce, upwardForceType);
            }
        }

        //if we hold u and i in any version with a keyboard we wont die!  We can cheat! lol
        if (Input.GetKey(KeyCode.U) && Input.GetKey(KeyCode.I))
        {
            //buttonCount decrements from initial val of 1.5
            buttonCount -= Time.deltaTime;
            //if it falls below zero we will run our cheating method
            if (buttonCount < 0)
            {
                //cheat method
                DebugMode();
                //set count back to 1.5 so we can turn it off.
                buttonCount = 1.5f;
            }
        }
        else
        {
            //else if the player did not get to the full count, then reset the time. 
            buttonCount = 1.5f;
        }

    }

    /// <summary>
    /// Simple Method that makes us invincible(re-enables testing mode boolean)
    /// </summary>
    public void DebugMode()
    {
        //change the value of the boolean
        testingMode = !testingMode;

    }


    /// <summary>
    /// This Method changes the color of the player, and the active colliders in the scene.
    /// </summary>
    public void SwitchPlayerColor()
    {
        //integer player color gets increased by 1
        playerColor += 1;
        //we create an int and give it the same value as the int casted Enum state (ie 0,1,2,3,pink,purple,teal,yellow)
        int playerColorsInt = (int)playerColor;
        //if the player color int is the same as the num of colors we set it back to 0(pink).  because this method always increments that color
        //we can go from 0 thru 3 and then back again.  we check this first in the method to get our state(Color) straight.
        if(playerColorsInt == numOfColors)
        {
            playerColor = 0;
        }

        //when the playersColor enum is a 0 or 1 or 2 or 3 these case statements are executed and the folling occurs
        switch (playerColor)
        {
            case PlayerColor.Pink:
                //Debug.Log("Color is Pink");
                playerRenderer.material.color = playerColorArray[0];

                break;
            case PlayerColor.Purple:
                //Debug.Log("Color is Purple");
                playerRenderer.material.color = playerColorArray[1];

                break;
            case PlayerColor.Teal:
                //Debug.Log("Color is Teal");
                playerRenderer.material.color = playerColorArray[2];

                break;
            case PlayerColor.Yellow:
                //Debug.Log("Color is Yellow");
                playerRenderer.material.color = playerColorArray[3];

                break;
            default:
                //Debug.Break();
                //Debug.Log("Color is Broken");

                break;
        }

    }


    /// <summary>
    /// If the player collides with anything it dies.  we Call the PlayerDeathEffectsOnly() Method and then 1.75Seconds later
    ///  the endgamecanvas gets enabled.
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionEnter(Collision col)
    {
        //isPlayerDead = true;
        if (!testingMode)
        {
            PlayerDeathEffectsOnly();
            Invoke("ShowEndGameCanvasAndScore", 1.75f);
        }
        else
        {
            //Debug.Log("Player Should Die " + "\n" + "Player Collided with " + col.collider.name + "\n" + col.collider.name + "is Tagged: " + col.collider.tag);
        }

    }


    /// <summary>
    /// This is the Method that is Invoked in the previous collision event.  It is called after 1.75Seconds so that the particle flash, and particle ball
    /// explosion have time to play out.  It checks to see if you have achieved a new record score by comparing your score to your highest score from previous
    /// calls to this method.( When your new score is higher it calls the UpdateHighScore() Method) After this it turns gravity off, zeros out any player velocity, 
    /// and enables the endGameCanvas 
    /// </summary>
    void ShowEndGameCanvasAndScore()
    {

        if(TemporaryGameVars.highestPlayerScore < TemporaryGameVars.playerScore)
        {
            UpdateHighestScore(TemporaryGameVars.playerScore);
        }

        playerRB.useGravity = false;
        playerRB.velocity = new Vector3(0, 0, 0);
        endGameCanvas.enabled = true;

    }

    /// <summary>
    /// Simple Method that takes One parameter (an int - amt), which is then set as your HighScore in the TemporaryGameVars static class, and then calls PlayerPrefs.SetInt
    /// and saves your high score under the key provided, and the amt passes.
    /// </summary>
    /// <param name="amt"></param>
    public void UpdateHighestScore(int amt)
    {
        TemporaryGameVars.highestPlayerScore = amt;
        PlayerPrefs.SetInt("highestPlayerScore", TemporaryGameVars.playerScore);
    }

    /// <summary>
    /// This Method is very similiar to PlayerDeath, except that it doesnt reload the level.  In earlier designs the level just reloaded when you died, and then 
    /// later the endGameCanvas was added.
    /// </summary>
    public void PlayerDeathEffectsOnly()
    {
        //Call chromatic abberation Method on ChromAbb script on MainCam
        chromo.StartAbberation();

        //Call camera shake Method on camera shake script on MainCam
        shake.StartShake();

        //if there is a deathclip then play sound
        if (deathSound)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position, localSoundVolume * 2f);
        }
        //Spawn the exploding balls particle system
        Instantiate(deathParticles, transform.position, transform.rotation);
        //spawn the bright white flash plane
        Instantiate(flashObject, transform.position + new Vector3(0, 7f, 0), transform.rotation);
        //disable gameobject
        gameObject.SetActive(false);
        //player dead equals true
        isPlayerDead = true;

    }

    /// <summary>
    /// This Method is very similiar to PlayerDeathEffectsOnly, except that it reloads the level.  In earlier designs the level just reloaded when you died, and then 
    /// later the endGameCanvas was added.
    /// </summary>
    public void PlayerDeath()
    {
        chromo.StartAbberation();

        shake.StartShake();

        if (deathSound)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position, localSoundVolume * 2f);
        }

        Instantiate(deathParticles, transform.position, transform.rotation);

        Instantiate(flashObject, transform.position + new Vector3(0, 7f, 0), transform.rotation);

        gameObject.SetActive(false);
        isPlayerDead = true;

        ScreenFaderSingleton.Instance.FadeAndReloadLevel();
    }

    //when this object is disabled cancel any invoke calls that may be running
    void OnDisable()
    {
        CancelInvoke();
    }
}
