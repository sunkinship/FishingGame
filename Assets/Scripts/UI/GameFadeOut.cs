using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFadeOut : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public bool fadeIn = false;
    public bool fadeOut = false;

    public float timeToFade;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeOut());
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeOut == true)
        {
            if (canvasGroup.alpha >= 0)
            {
                canvasGroup.alpha -= timeToFade * Time.deltaTime;

                if (canvasGroup.alpha == 0)
                {
                    fadeOut = false;
                }
            }
        }
    }

    public IEnumerator FadeOut()
    {
        fadeOut = true;
        yield return new WaitForSeconds(2);
    }
}
