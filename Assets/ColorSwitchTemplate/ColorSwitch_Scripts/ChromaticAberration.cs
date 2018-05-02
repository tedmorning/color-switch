using UnityEngine;
using System.Collections;

/// <summary>
/// This class handles the Chromatic Abberation Image Effect
/// </summary>
[ExecuteInEditMode]
public class ChromaticAberration : MonoBehaviour
{
    public float duration = 5f;
    public float maxTime;
    private float minTime = 0f;
    public float speedMulti;
    public float elapsed = 0f;
    public Shader chromeAbbShader;
    public float ChromaticAbberation = 1.0f;
    private Material curMaterial;
    Material material
    {
        get
        {
            if (curMaterial == null)
            {
                curMaterial = new Material(chromeAbbShader);
                curMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return curMaterial;
        }
    }

    // Use this for initialization
    void Start()
    {
        //if image effects are not supported disable this monobehaviour and Return
        if (!SystemInfo.supportsImageEffects)
        {
            enabled = false;
            return;
        }
    }

    void OnRenderImage(RenderTexture srcText, RenderTexture destText)
    {
        //if chromaticAbberation shader has been assigned in the inspector, it will then send the ChromaticAbberation var to _AberrationOffset propertiy on the shader.
        //We use the Coroutines to adjust the "elapsed" var over time.  we then assign the elapsed var to the ChromaticAbberation var so that when
        //StartAbberation() is called this behaviour keeps the ChromaticAbberation var the same value as the elapsed var.  So each pass thru the while loops change the value.
        if (chromeAbbShader != null)
        {
            material.SetFloat("_AberrationOffset", ChromaticAbberation);
            Graphics.Blit(srcText, destText, material);
        }
        else
        {
            Graphics.Blit(srcText, destText);
        }
    }

    /// <summary>
    /// Increases "elapsed" to spread the red/blue/yellow channels out
    /// </summary>
    /// <returns></returns>
    IEnumerator OverTime(/*float waitTime*/)
    {
        elapsed = minTime;
        duration = maxTime;
        while(elapsed < duration)
        {
            elapsed += Time.deltaTime * speedMulti;
            yield return null;
        }
        elapsed = maxTime;
        yield return StartCoroutine("UnderTime");
    }

    /// <summary>
    /// Decreases "elapsed" to shrink the red/blue/yellow channels back.
    /// </summary>
    /// <returns></returns>
    IEnumerator UnderTime(/*float waitTime*/)
    {
        elapsed = maxTime;
        duration = minTime;
        while (elapsed > duration)
        {
            elapsed -= Time.deltaTime * speedMulti;
            yield return null;
        }
        elapsed = minTime;
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        //Assigns the elapsed var to ChromaticAbberation var
        //ChromaticAbberation var is fed to the ImageEffect Shaders "_ChromAb" property
        ChromaticAbberation = elapsed;
    }

    /// <summary>
    /// This Method is Called to start the spread of the color channels effect, and when it returns it starts the coroutine to shrink the color channels back to start position.
    /// </summary>
    public void StartAbberation()
    {
        //stop coroutines for good measure
        StopAllCoroutines();
        //start the coroutine Overtime(which starts UnderTime(comes back))
        StartCoroutine("OverTime");
    }

    void OnDisable()
    {
        if (curMaterial)
        {
            DestroyImmediate(curMaterial);
        }
    }

}