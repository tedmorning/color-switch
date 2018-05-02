using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class that handles Collecting, Moving, Rotating, and Spawning Obstacles for our Player
/// 
/// NOTE: Below with all of the Repetitive for Looping through lists, we should create a method that takes the list and some other paramaters so
/// that we are rewriting the same lines over and over again, but in this case I wanted a better solution than that too... I ended up leaving as
/// is for the time being.
/// </summary>
public class ColorColliderController : MonoBehaviour {

    public float verticalMin = 6f;                                              //Min distance the obstacles can be from previous Obstacle
    public float verticalMax = 10f;                                             //Max distance the obstacles can be from previous Obstacle
    public int currentlySelectedObstacle;                                       //Which Obstacle is activated/moved(always off screen 2 above the player)
    private int numOfObstacles;                                                  //How many obstacles? Entered into Inspector
    public List<GameObject> pinkColliderGameObjects = new List<GameObject>();   //Collection of all gameobjects tagged PinkCollider
    public List<GameObject> purpleColliderGameObjects = new List<GameObject>(); //Collection of all gameobjects tagged PurpleCollider
    public List<GameObject> tealColliderGameObjects = new List<GameObject>();   //Collection of all gameobjects tagged TealCollider
    public List<GameObject> yellowColliderGameObjects = new List<GameObject>(); //Collection of all gameobjects tagged YellowCollider
    private List<GameObject> obstacleObjects = new List<GameObject>();          //Collection of all gameobjects tagged Obstacle
    public List<GameObject> publicObstacleObjectList;                           //The Public Version we can monitor and use in Code.
    public Vector3 originalObstaclePosition = new Vector3(0f, 0f, 0f);          //Where Obstacles start spawning
    private Vector3 newPoolPos = new Vector3(0f, 0f, 0f);                       //A vec3 we use as placeholder/jar when moving/activating obstacles
    private GameObject starPool, colorChangePool;                               //References to the Pools(on EmptyGameObjects in Scene) of ColorChangers/Stars
    private ObjectPoolScript starPoolScript, colorChangePoolScript;             //The Pooled Items Script References(For the Generic Pool "Getter" Methods)
    private PlayerController pController;                                       //Reference to the PlayerController Script

    // Use this for initialization
    void Start () 
	{
        //Setup PlayerController Reference
        pController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        //Add all color colliders to lists
        pinkColliderGameObjects.AddRange(GameObject.FindGameObjectsWithTag("PinkCollider"));
        purpleColliderGameObjects.AddRange(GameObject.FindGameObjectsWithTag("PurpleCollider"));
        tealColliderGameObjects.AddRange(GameObject.FindGameObjectsWithTag("TealCollider"));
        yellowColliderGameObjects.AddRange(GameObject.FindGameObjectsWithTag("YellowCollider"));

        //add active offscreen obstacles to list so that we can deactivate them, and later move them into
        //position and activate them.  then keep arranging them at random as the player progresses.
        obstacleObjects.AddRange(GameObject.FindGameObjectsWithTag("ObstacleObject"));

        //get the count of the obstacles tagged in the scene.
        numOfObstacles = obstacleObjects.Count;

        //referencing actual active objects in prototype(controlled internally)INCOMPLETE
        publicObstacleObjectList = new List<GameObject>();
        //loop thru active obstacle objects and deactivate them
        for (int i = 0; i < numOfObstacles; i++)
        {
            //GO ==> oo[i] type reference
            GameObject obj = obstacleObjects[i];
            //add obstacles to the "Public List"
            publicObstacleObjectList.Add(obj);
            //Deactivate all obstacles
            obj.SetActive(false);
        }
        //this is a reference to the pool Object in the scene that creates the "star pickups"
        starPool = GameObject.FindGameObjectWithTag("StarPool");

        //this is a reference to the pool Object in the scene that creates the "color change pickups"
        colorChangePool = GameObject.FindGameObjectWithTag("ColorChangePool");

        //reference to the pool scripts on the star pool gameobject
        starPoolScript = starPool.GetComponent<ObjectPoolScript>();

        //reference to the pool scripts on the color change pool gameobject
        colorChangePoolScript = colorChangePool.GetComponent<ObjectPoolScript>();

        // sort the final public obstacle list by name.  all of the obstacles that are in the scene at game start
        // will be deactivated, and activated at the right time/place.  This is how we are going to pool our obstacles.
        // now that the final public list of obstacles is filled and ready to be accessed/used we will sort the list by name.

        //sort by name
        publicObstacleObjectList.Sort(SortByName);

        // activate the first 2 obstacles in our public obstacle list so that when the game starts, the initial obstacles are on
        // screen.  then by the time we get to the first "color change pickup" those pickups will activate and position the next
        // obstacles in this case (obstacle 3)...  When we get to the end of the list it will loop back to the beginning... but 
        // always making sure there are two obstacles above and below the player.  This will give the illusion that their are 
        // infinite numbers of obstacles spawning.  we will use the public list of named(and sorted) obstacles to manage what
        // obstacles get spawned and what rotation they are spawned at.

        //activate first obstacle
        IncrementObstacleProgression();
        //activate second obstacle
        IncrementObstacleProgression();

        //sync mesh colliders
        ChangeColorColliderState();

    }

    //make sure all colliders are enabled(protection from previous enable/disable)
    //so we loop thru each collection and enable them all.
    public void ChangeColorColliderState()
    {
        //loop thru all of the "teal" tagged colliders
        for (int i = 0; i < tealColliderGameObjects.Count; i++)
        {
            var item = tealColliderGameObjects[i];
            item.GetComponent<MeshCollider>().enabled = true;
        }

        //loop thru all of the "pink" tagged colliders
        for (int i = 0; i < pinkColliderGameObjects.Count; i++)
        {
            var item = pinkColliderGameObjects[i];
            item.GetComponent<MeshCollider>().enabled = true;
        }

        //loop thru all of the "purple" tagged colliders
        for (int i = 0; i < purpleColliderGameObjects.Count; i++)
        {
            var item = purpleColliderGameObjects[i];
            item.GetComponent<MeshCollider>().enabled = true;
        }

        //loop thru all of the "yellow" tagged colliders
        for (int i = 0; i < yellowColliderGameObjects.Count; i++)
        {
            var item = yellowColliderGameObjects[i];
            item.GetComponent<MeshCollider>().enabled = true;
        }

        //deactivate the necessary colliders... Conditional that checks the color of the
        //player and then deactivates the appropriate group of colliders

        //if player is teal deactivate all teal colliders
        if (pController.playerColor == PlayerColor.Teal)
        {
            for (int i = 0; i < tealColliderGameObjects.Count; i++)
            {
                var item = tealColliderGameObjects[i];
                item.GetComponent<MeshCollider>().enabled = false;
            }
        }

        //if player is pink deactivate all teal colliders
        if (pController.playerColor == PlayerColor.Pink)
        {
            for (int i = 0; i < pinkColliderGameObjects.Count; i++)
            {
                var item = pinkColliderGameObjects[i];
                item.GetComponent<MeshCollider>().enabled = false;
            }
        }

        //if player is purple deactivate all teal colliders
        if (pController.playerColor == PlayerColor.Purple)
        {
            for (int i = 0; i < purpleColliderGameObjects.Count; i++)
            {
                var item = purpleColliderGameObjects[i];
                item.GetComponent<MeshCollider>().enabled = false;
            }
        }

        //if player is yellow deactivate all teal colliders
        if (pController.playerColor == PlayerColor.Yellow)
        {
            for (int i = 0; i < yellowColliderGameObjects.Count; i++)
            {
                var item = yellowColliderGameObjects[i];
                item.GetComponent<MeshCollider>().enabled = false;
            }
        }
    }

    /// <summary>
    /// This Method retrieves an obstacle and moves it into place, rotates it if needed, and 
    /// activates it, if not all ready active.
    /// </summary>
    /// <param name="obstacleNum"></param>
    /// <param name="rotateObstacle"></param>
    /// <param name="starPosDifference"></param>
    /// <param name="colorPosDifference"></param>
    public void GetObstacle(int obstacleNum, bool rotateObstacle, float starPosDifference, float colorPosDifference)
    {
        // get a random number between min/max obstacle spawn distance.
        float randDis = Random.Range(verticalMin, verticalMax);

        //disable the object
        publicObstacleObjectList[obstacleNum].SetActive(false);

        // use this value rather than the randDis above, for spawning the colorChange Objects(need their
        // location to be more predictable)
        float newDis = verticalMin * 0.5f; /*(verticalMax - verticalMin) * 0.5f; */

        //zero out the depth so everything is lined up
        originalObstaclePosition.z = 0f;

        //then go up randomly from the "first" "starting position"
        originalObstaclePosition.y += randDis;

        //set the "first" obstacles position as the starting postion...
        publicObstacleObjectList[obstacleNum].transform.position = originalObstaclePosition;

        // check to see what color the player will be when they arrive at the obstacle
        // that way we can make sure it is rotated the correct direction(the more complex 
        // obstacles require this)

        //ie a "double circle", "double square", "triple square", etc can only be traversed if you are 2 of the 4 possible colors.

        //check boolean parameter
        if (rotateObstacle)
        {

            //rotate the obstacle object 90 degrees on the z axis
            publicObstacleObjectList[obstacleNum].transform.eulerAngles = new Vector3(0f, 0f, 90f);

            //activate the obstacle
            publicObstacleObjectList[obstacleNum].SetActive(true);

            //store the new "starting pos" in a new vec3 to send to the GetObject methods for star, and colorChangerObj
            newPoolPos = originalObstaclePosition;

            //pull a star from the star pooler in the scene
            //and set its postition the the same as the obstacle(itll be inside the obstacle)
            GetStarObject(newPoolPos, starPosDifference);

            //pull a color changer from the pooler in scene
            //set it to same pos but "higher" than the obstacle (by half the randDistance from above
            GetColorChangerObject(newPoolPos, newDis + colorPosDifference);

            //DEBUG INFO FOR SETUP... Uncomment This and all Commented Debug messages to see the flow.
            //Debug.Log("Object in List: " + obstacleNum.ToString() + " WAS rotated " + publicObstacleObjectList[obstacleNum].transform.eulerAngles.z.ToString() + " around z axis" + "\n" + " rotateObstacle = " + rotateObstacle.ToString());

        }
        else
        {
            //if !RotateObstacle... Then set rotation to 0,0,0
            publicObstacleObjectList[obstacleNum].transform.eulerAngles = new Vector3(0f, 0f, 0f);

            //activate the obstacle
            publicObstacleObjectList[obstacleNum].SetActive(true);

            //store that new "starting pos" in a new vec3 to send to the GetObject methods for star, and colorChangerObj
            newPoolPos = originalObstaclePosition;

            //pull a star from the star pooler in the scene
            //and set its postition the the same as the obstacle(itll be inside the obstacle)
            GetStarObject(newPoolPos, starPosDifference);

            //pull a color changer from the pooler in scene
            //set it to same pos but "higher" than the obstacle (by half the randDistance from above
            GetColorChangerObject(newPoolPos, newDis + colorPosDifference);

            //DEBUG INFO FOR SETUP... Uncomment This and all Commented Debug messages to see the flow.
            //Debug.Log("Object in List: " + obstacleNum.ToString() + " was NOT rotated " + publicObstacleObjectList[obstacleNum].transform.eulerAngles.z.ToString() + " around z axis" + "\n" + " rotateObstacle = " + rotateObstacle.ToString());
        }

    }

    /// <summary>
    /// This method moves StarPickUp into position and activates it.
    /// </summary>
    /// <param name="newPos"></param>
    /// <param name="yOffset"></param>
    public void GetStarObject(Vector3 newPos, float yOffset)
    {
        GameObject obj = starPoolScript.GetPooledObject();

        if (obj == null) return;

        obj.transform.position = newPos + new Vector3(0, yOffset, 0);
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);
    }

    /// <summary>
    /// This method moves ColorChangeObject into position and activates it.
    /// </summary>
    /// <param name="newPos"></param>
    /// <param name="Dis"></param>
    public void GetColorChangerObject(Vector3 newPos, float Dis)
    {
        GameObject obj = colorChangePoolScript.GetPooledObject();

        if (obj == null) return;

        obj.transform.position = newPos + new Vector3( 0, Dis /** 0.5f*/, 0 );
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);
    }


/// <summary>
/// This Method controls when/what obstacle is spawned
/// 1.)Initially we spawn 2 obstacles. The goal is that @ any given time there will be at least 2 obstacles ahead, and behind
/// the player.
/// 2.)Once all of the obstacles in the level(publicObstaclePoolList) are re-activated from their initial "deactivation", we will
/// start removing the first obstacles we completed/traversed.  So... if we only had 6 obstacles initially and we were about 
/// to cycle thru the list there may be 4 obstacles behind us, and 2 obstacles ahead of us.  I hope this was exaplined well 
/// enough.  If for some reason it was not, change the editor layout to 2by3, and Do NOT maximize on play, and watch how the
/// obstacles are spawned and moved as your height increases.
/// 3.) This is not the best way to do this, but it will get the job done for now.
/// </summary>
    public void IncrementObstacleProgression()
    {
        //boolean we will send to GetObsacle as a Paramater
        bool shouldWeRotateObstacle;

        //Determine if the more complex obstacles need to be rotated based on the players color(pr eventual color).
        if(pController.playerColor == PlayerColor.Teal || pController.playerColor == PlayerColor.Pink)
        {
            shouldWeRotateObstacle = false;
        }
        else
        {
            shouldWeRotateObstacle = true;
        }

        //if we reach the end of the obstacle pool, then start back at zero.
        if (currentlySelectedObstacle >= publicObstacleObjectList.Count)
        {
            currentlySelectedObstacle = 0;
        }

        // When Calling GetObstacle, I just decided after some tinkering to pass parameters that changed the orientation
        // of the obstacle, and the height of the "stars" and "color change objects"...  Since I numbered all the names of the 
        // obstacles and then sorted them by name I just figured id keep it simple, and tweak the values after a couple play
        // throughs.. This is definetley NOT the way this should be done, especially because the obstacles are not random/dynamic
        // but for the sake of a simple, understandable implementation that just about anyone could improve upon... I decided not
        // to spend any real time trying to do something elogant.

        // switch case for the obstacles.. It calls the GetObstacle(); with the appropriate paramaters for each of 
        // the "currentlySelectedObstacle".. (Only a few of the more complicated obstacles need extra stuff done)
        switch (currentlySelectedObstacle)
        {
            //GetObstacle( NumOfObstacle, doRotate? (bool) , raise star height___f, raise color change object height____f)//
            case 0:
                //
                GetObstacle (currentlySelectedObstacle , false, 0f , 0f );
                break;
            case 1:
                //
                GetObstacle(currentlySelectedObstacle, false, 0f, 0f);
                break;
            case 2:
                //
                GetObstacle(currentlySelectedObstacle, false, 2f, 0f);
                break;
            case 3:
                //Double Circle.. these can only be traversed if you are one of two colors(without being rotated.)
                //when we get to this Double Circle Obstacle we wont be the right color... so we send 
                //      GetObstacle(currentlySelectedObstacle,TRUE(needs rotate),0f,0f);
                GetObstacle(currentlySelectedObstacle, shouldWeRotateObstacle, 0f, 0f);
                break;
            case 4:
                //
                GetObstacle(currentlySelectedObstacle, false, 0f, 0f);
                break;
            case 5:
                //
                GetObstacle(currentlySelectedObstacle, false, 0f, 0f);
                break;
            case 6:
                //
                GetObstacle(currentlySelectedObstacle, false, 1.5f, 0f);
                break;
            case 7:
                //Double Square(same as above)
                GetObstacle(currentlySelectedObstacle, shouldWeRotateObstacle, 0f, 0f);
                break;
            case 8:
                //
                GetObstacle(currentlySelectedObstacle, false, 3f, 0f);
                break;
            case 9:
                //
                GetObstacle(currentlySelectedObstacle, false, 0f, 0f);
                break;

            case 10:
                //
                GetObstacle(currentlySelectedObstacle, false, 0f, 0f);
                break;

            case 11:
                //
                GetObstacle(currentlySelectedObstacle, false, 0f, 2.75f);
                break;

            case 12:
                //
                GetObstacle(currentlySelectedObstacle, false, 0f, 0f);
                break;

            case 13:
                //
                GetObstacle(currentlySelectedObstacle, false, 2f, 0f);
                break;

            case 14:
                //
                GetObstacle(currentlySelectedObstacle, false, 3f, 0f);
                break;

            case 15:
                //
                GetObstacle(currentlySelectedObstacle, false, 2f, 0f);
                break;

            case 16:
                //TrippleSquare... yep we will be the wrong color when we arrive, send true and rotate it.
                // etc, etc... this is how we make sure there arent any issues hanging the player up
                GetObstacle(currentlySelectedObstacle, shouldWeRotateObstacle, 0f, 0f);
                break;

            case 17:
                //
                GetObstacle(currentlySelectedObstacle, false, 0f, 0f);
                break;

            case 18:
                //
                GetObstacle(currentlySelectedObstacle, false, 2f, -0.5f);
                break;

            case 19:
                //
                GetObstacle(currentlySelectedObstacle, false, 1f, 3.25f);
                break;


            case 20:
                //
                GetObstacle(currentlySelectedObstacle, false, 0f, 0f);
                break;

            case 21:
                //
                GetObstacle(currentlySelectedObstacle, false, 1.25f, 0f);
                //goto case 0;
                break;

            default:
                break;
        }

        //Increment the obstacle to continue progressing thru the list
        currentlySelectedObstacle++;


    }

    /// <summary>
    /// Simple Method to Sort the Public Obstacle List By Name (because we started all the obstacles with numbers 
    /// 00,01,02,etc.. )
    /// </summary>
    /// <param name="obstacle1"></param>
    /// <param name="obstacle2"></param>
    /// <returns></returns>
    private static int SortByName(GameObject obstacle1, GameObject obstacle2)
    {
        return obstacle1.name.CompareTo(obstacle2.name);
    }
}
