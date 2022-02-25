using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoseFade : MonoBehaviour
{

    private bool fadeOut = false;
    private bool fadeIn = true;
    //private float maxA = 2f; //150
    //1.67
    //-10
    public float fadeSpeed = 0.5f;

    public float minIntensity = 0.0007314645f;
    public float maxIntensity = 2.118547f;
    private string emissionName = "_EmissionColor";


    // Start is called before the first frame update
    void Start()
    {
        Renderer renderer = this.GetComponent<Renderer>();
        Color objectColor = new Color(minIntensity, minIntensity, minIntensity);
        renderer.material.SetColor(emissionName, objectColor);
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeOut)
        {

            Renderer renderer = this.GetComponent<Renderer>();
            //Renderer tailRenderer = this.GetComponentInChildren<Renderer>();

            Color objectColor = renderer.material.GetColor(emissionName);
            float fadeAmount = objectColor.r - (fadeSpeed * Time.deltaTime);


            objectColor = new Color(fadeAmount, fadeAmount, fadeAmount);
            renderer.material.SetColor(emissionName, objectColor);
            //tailRenderer.material.SetColor(emissionName, objectColor);

            if (objectColor.r <= minIntensity)
            {
                fadeOut = false;
            }
        }

        if (fadeIn)
        {
            
            Renderer renderer = this.GetComponent<Renderer>();
            //Renderer tailRenderer = this.transform.GetChild(1).gameObject.GetComponent<Renderer>();


            Color objectColor = renderer.material.GetColor(emissionName);
            Debug.Log("fade in " + objectColor.r);
            float fadeAmount = objectColor.r + (fadeSpeed * Time.deltaTime);


            objectColor = new Color(fadeAmount, fadeAmount, fadeAmount);
            renderer.material.SetColor(emissionName, objectColor);
            //tailRenderer.material.SetColor(emissionName, objectColor);


            if (objectColor.r >= maxIntensity)
            {
                fadeIn = false;
            }
        }
    }

    public void FadeOutObject()
    {
        fadeOut = true;
    }

    public void FadeInObject()
    {
        fadeIn = true;
    }

    
}
