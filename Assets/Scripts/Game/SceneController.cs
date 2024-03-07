using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    private Coroutine co_switchingScenes = null;
    public bool SwitchingScenes => co_switchingScenes != null;

    private FadeController fadeController => FadeController.Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Debug.Log("Initial Fade");
        fadeController.InitialFade();
    }

    public void SwitchScene(string sceneName, float speed = FadeController.DEAFULT_FADE_SPEED) => StartCoroutine(TransitionScene(sceneName, speed));

    public void SwitchScene(string sceneName) => co_switchingScenes = StartCoroutine(TransitionScene(sceneName, FadeController.DEAFULT_FADE_SPEED));

    private IEnumerator TransitionScene(string sceneName, float speed)
    {
        fadeController.FadeIn(speed);

        while (fadeController.FadingIn)
            yield return null;

        co_switchingScenes = null;
        SceneManager.LoadScene(sceneName);
    }

    public void Quit(float speed = FadeController.DEAFULT_FADE_SPEED) => StartCoroutine(Quitting(speed));

    private IEnumerator Quitting(float speed)
    {
        fadeController.FadeIn(speed);

        while (fadeController.FadingIn)
            yield return null;

        Application.Quit();
    }
}
