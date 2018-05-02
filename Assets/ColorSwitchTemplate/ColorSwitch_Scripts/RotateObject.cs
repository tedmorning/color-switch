using UnityEngine;
using System.Collections;

/// <summary>
/// Simple Script to Rotate GameObjects Indefinitely.  I.e. a ferris wheel, or a Gem/Coin Object.
/// </summary>
public class RotateObject : MonoBehaviour {

    public string obstacleRotationName;
    public Vector3 rotation;
    public Space space;
    public Rigidbody childRB;

    //Get Rigidbody Component OnEnable that way we can rotate the gameobject in the most efficient way, but
    //if it does not have one, then we will just rotate its transform.
    void OnEnable()
    {
        childRB = GetComponentInChildren<Rigidbody>();
    }

    //This is the rotation... RigidBody.MoveRotation or transform.Rotate(if no RigidBody).
    void Update()
    {
        if (childRB)
        {
            Vector3 newRot = rotation * Time.deltaTime;
            Quaternion rot = Quaternion.Euler(newRot);
            childRB.MoveRotation(rot);
        }
        else
        {
            this.transform.Rotate(rotation * Time.deltaTime, space);
        }
    }
}
