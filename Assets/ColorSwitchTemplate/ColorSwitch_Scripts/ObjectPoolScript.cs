using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This is a Basic Object Pool Script.  In the ColorSwitchClone project this class is
/// used on the ColorChangePickups and the StarPickups.  It is added to an Empty GameObject
/// in the game scene.  This is the same pooling script that Unity Technologies makes in their
/// Object Pooling Live Training.  A few quick modifications can be made to it, to make it
/// much more powerful.  To learn more about this Script visit the Unity3d.com/learn page
/// and navigate to the "Live Trainings".. youll find an object pooling video their.
/// </summary>
public class ObjectPoolScript : MonoBehaviour

{
    public static ObjectPoolScript current;     //reference to this pool script(used when getting an object from this pool)
    public GameObject pooledObject;             //the object we are pooling
    public int pooledAmount = 20;               //the number of those objects that will be instantiated at runtime
    public bool willGrow = true;                //if you need more than the pooledAmout, then it will instantiate new ones as requested.
    
    public List<GameObject> pooledObjects;

    void Awake()
    {
        current = this;
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (pooledObjects[i] == null)
            {
                GameObject obj = (GameObject)Instantiate(pooledObject);
                obj.SetActive(false);
                pooledObjects[i] = obj;
                return pooledObjects[i];
            }
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        if (willGrow)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject);
            pooledObjects.Add(obj);
            return obj;
        }
        return null;
    }
}