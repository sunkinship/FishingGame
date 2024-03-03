using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFading : MonoBehaviour
{

    public CanvasGroup canvasGroup;
    public bool fadeIn = false;

    public float timeToFade;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeIn());
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeIn == true)
        {
            if (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += timeToFade * Time.deltaTime;
                if (canvasGroup.alpha >= 1)
                {
                    fadeIn = false;
                }
            }
        }
    }

    public IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(8);
        fadeIn = true;
    }
}
