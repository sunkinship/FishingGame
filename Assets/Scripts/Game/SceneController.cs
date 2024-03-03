using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    private FadeOut fadeController;

    [SerializeField] private string nextScene;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        fadeController = FindObjectOfType<FadeOut>();
    }

    private void Update()
    {
        if (Input.anyKey)
        {
            StartCoroutine(TransitionScene(nextScene));
        }
    }

    public IEnumerator TransitionScene(string sceneName)
    {
        fadeController.FadeeOut();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
    }
}
