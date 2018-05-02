using UnityEngine;
using System.Collections;

/// <summary>
/// Simple Method that Disables a GameObject after delay time.
/// </summary>
public class DisableGameObject : MonoBehaviour {

    public float delay;                 //The amount of time to wait for disabling the GameObject

	// Use this for initialization
	void Start () 
	{
        Invoke("DisableObj",delay);
	}

    void DisableObj()
    {
        this.gameObject.SetActive(false);
    }
}
