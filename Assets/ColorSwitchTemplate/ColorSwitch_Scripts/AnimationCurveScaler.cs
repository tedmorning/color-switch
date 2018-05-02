using UnityEngine;
using System.Collections;

/// <summary>
/// This Class Scales an object over time by evaluating a Animation Curve.  It is used to "tween" things.
/// </summary>
public class AnimationCurveScaler : MonoBehaviour
{
    [Range(0.25f, 5f)]
    public float scaleSpeed = 0.25f;
    public AnimationCurve aCurve;
    private Transform _transform;
    private float step;
    private float objScale;


    // Use this for initialization
    public void Start()
    {
        //set the starting scale/transform to whatever scale the gameobject has at gamestart
        _transform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //incrememnt the step
        step += scaleSpeed * Time.deltaTime;
        objScale = aCurve.Evaluate(step);
        _transform.localScale = new Vector3(objScale, objScale, objScale);
        if (step >= 1)
        {
            //set the step to 0 and start over
            step = 0;
        }
    }

}
