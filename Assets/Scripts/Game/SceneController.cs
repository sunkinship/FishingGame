using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    private FadeController fadeController;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        fadeController = FindObjectOfType<FadeController>();
    }

    public void NextScene(string sceneName) => StartCoroutine(TransitionScene(sceneName));

    public IEnumerator TransitionScene(string sceneName)
    {
        fadeController.FadeIn();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
    }
}
