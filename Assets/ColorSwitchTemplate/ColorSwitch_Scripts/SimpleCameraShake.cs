using UnityEngine;
using System.Collections;

/// <summary>
/// Method that "shakes" the camera transform to give a nice jiggly effect.
/// </summary>
public class SimpleCameraShake : MonoBehaviour {

    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;
    public Transform shakeCam;
    public string shakeCameraTag;

    //removed Find
    // Use this for initialization
    void Start () 
	{
        shakeCam = GetComponent<Camera>().transform;
    }

    public void StartShake()
    {
        StopAllCoroutines();
        StartCoroutine("Shake");
    }

    IEnumerator Shake()
    {
        float elapsedTime = 0.0f;

        //Vector3 originalShakeCameraPos = shakeCam.position;

        while (elapsedTime < shakeDuration)
        {
            //originalShakeCameraPos = transform.position;

            elapsedTime += Time.deltaTime;

            float percentComplete = elapsedTime / shakeDuration;
            float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

            // map value to [-1, 1]
            float x = Random.value * 2.0f - 1.0f;
            //use either z or y... for this game its "Y" (COLORSWITCH)
            float y = Random.value * 2.0f - 1.0f;

            x *= shakeMagnitude * damper;
            y *= shakeMagnitude * damper;
            y *= shakeMagnitude * damper;

            shakeCam.position = new Vector3(shakeCam.position.x + x, shakeCam.position.y + y, shakeCam.position.z);

            yield return null;
        }
    }
}
