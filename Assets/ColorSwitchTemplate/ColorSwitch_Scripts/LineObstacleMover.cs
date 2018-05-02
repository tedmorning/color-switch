using UnityEngine;
using System.Collections;

/// <summary>
/// This Class controls the scrolling movement of the "line" type obstacles.
/// it moves a mesh that is about 3 screen widths wide and when it is at the end
///  it teleports it back to start.  It is supposed to give a "endless" seamless
///  movement effect.
/// </summary>
public class LineObstacleMover : MonoBehaviour
{
    public Transform startPosObj, endPosObj;
    public float moveToTime;
    public float moveBackFromTime;
    private Vector3 pointA, pointB;

    //public void OnBecameVisible()
    //{
    //    StopAllCoroutines();
    //    StartCoroutine(MoveStarter());
    //    Debug.Log("VISIBLE");
    //}

    /// <summary>
    /// On enable either initially or when reactivated from pool starts the coroutine which loops
    /// the movement coroutine
    /// </summary>
    void OnEnable()
    {
        StartCoroutine(MoveStarter());
    }

    /// <summary>
    ///  This coroutine starts the looping motion coroutine which moves obstacle from point
    ///  a to point b. the while statement is always true, so that once it returns from moving
    ///  from pointA to pointB , it starts the same coroutine but with different paramaters. 
    ///  we "moveBack" instantly and then once at the destination, it starts the slow move from 
    ///  a to b again.
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveStarter()
    {
        pointA = startPosObj.position;
        pointB = endPosObj.position;
        while (true)
        {
            yield return StartCoroutine(MoveObject(transform, pointA, pointB, moveToTime));
            yield return StartCoroutine(MoveObject(transform, pointB, pointA, moveBackFromTime));
        }
    }

    /// <summary>
    /// This is the coroutine that actually does the "moving" of the obstacle object.
    /// </summary>
    /// <param name="thisTransform"></param>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
    {
        float i = 0.0f;
        float rate = 1.0f / time;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            thisTransform.position = Vector3.Lerp(startPos, endPos, i);
            yield return null;
        }
    }

    //public void OnBecameInvisible()
    //{
    //    StopAllCoroutines();
    //    Debug.Log("INVISIBLE");

    //}

    /// <summary>
    /// we call stop all coroutine when this obstacle is deactive and not in use
    /// </summary>
    void OnDisable()
    {
        StopAllCoroutines();
    }
}
