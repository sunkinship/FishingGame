using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    public static FadeController Instance;

    public const float DEAFULT_FADE_SPEED = 1f;

    private CanvasGroup canvasGroup;

    private Coroutine co_fadingIn = null;
    private Coroutine co_fadingOut = null;

    public bool FadingIn => co_fadingIn != null;
    public bool FadingOut => co_fadingOut != null;
    public bool Fading => FadingIn || FadingOut;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeIn(float speed = DEAFULT_FADE_SPEED)
    {
        if (FadingIn)
            return;

        if (FadingOut)
        {
            StopCoroutine(co_fadingOut);
            co_fadingOut = null;
        }
            
        canvasGroup.blocksRaycasts = true;
        co_fadingIn = StartCoroutine(FadingInProcess(speed));
    }

    private IEnumerator FadingInProcess(float speed)
    {
        float targetAlpha = 1;

        while (canvasGroup.alpha < targetAlpha)
        {
            //Debug.Log("Fading In " + canvasGroup.alpha);
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }
    
        co_fadingIn = null;
    }

    public void FadeOut(float speed = DEAFULT_FADE_SPEED)
    {
        if (FadingOut)
            return;

        if (FadingIn)
        {
            StopCoroutine(co_fadingIn);
            co_fadingIn = null;
        }
           
        canvasGroup.blocksRaycasts = false;
        co_fadingOut = StartCoroutine(FadingOutProcess(speed));
    }

    private IEnumerator FadingOutProcess(float speed)
    {
        float targetAlpha = 0;
        
        while (canvasGroup.alpha > targetAlpha)
        {
            //Debug.Log($"Fading Out Current {canvasGroup.alpha} Moving To {Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, speed * Time.deltaTime)} Target {targetAlpha}");
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }
     
        co_fadingOut = null;
    }

    public void InitialFade()
    {
        canvasGroup.alpha = 1;
        FadeOut();
    }
}
