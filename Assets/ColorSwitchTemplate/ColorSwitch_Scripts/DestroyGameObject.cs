using UnityEngine;
using System.Collections;

/// <summary>
/// Simple Class that can be used to Destroy or Clean-Up gameObjects.  Anything from silently destroying some Empties you used in
/// the scene as "folders", to making this part of a prefab you instantiate on NPC/Character death - which explodes (adds force) to
/// pieces(gibs) of the destroyed object.  then plays an explosion sound, and squishy death sound.  Its a useful destroy object script.
/// </summary>
public class DestroyGameObject : MonoBehaviour
{
    public AudioClip destroySound, destroySoundTwo;	    //sound(s) to play when object is destroyed
    public float delay;                                 //delay before object is destroyed
    public bool destroyChildren;                        //should the children be detached (and kept alive) before object is destroyed?
    public float pushChildAmount;                       //push children away from centre of parent

    void Start()
    {
        //get list of children
        Transform[] children = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i);
        }

        //detach children
        if (!destroyChildren)
        {
            transform.DetachChildren();
        }

        //add force to children and some torque!
        for (int i = 0; i < children.Length; i++)
        {
            Transform child = children[i];
            if (child.GetComponent<Rigidbody>() && pushChildAmount != 0)
            {
                Rigidbody childRB = GetComponent<Rigidbody>();
                Vector3 pushDir = child.position - transform.position;
                childRB.AddForce(pushDir * pushChildAmount, ForceMode.Force);
                childRB.AddTorque(Random.insideUnitSphere, ForceMode.Force);
            }
        }
        //if destroy sound was added, then play it
        if (destroySound)
        {
            AudioSource.PlayClipAtPoint(destroySound, transform.position);
        }
        //if second destroy sound was added, then play it
        if (destroySoundTwo)
        {
            AudioSource.PlayClipAtPoint(destroySoundTwo, transform.position);
        }
        //destroy  parent
        Destroy(gameObject, delay);
    }
}