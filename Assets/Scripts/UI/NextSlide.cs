using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextSlide : MonoBehaviour
{

    public CanvasGroup canvasGroup;
    public bool fadeIn = false;
    public bool fadeOut = false;

    public float timeToFade;

    public GameObject nextSlide;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            StartCoroutine(FadeOut());
        }

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

        nextSlide.SetActive(true);
        gameObject.SetActive(false);
    }
}