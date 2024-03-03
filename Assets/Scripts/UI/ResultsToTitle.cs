using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsToTitle : MonoBehaviour
{
    FadeOut fade;

    private void Start()
    {
        fade = FindObjectOfType<FadeOut>();
    }

    private void Update()
    {
        if (Input.anyKey)
        {
            StartCoroutine(ChangeScene());
        }
    }

    public IEnumerator ChangeScene()
    {
        fade.FadeeOut();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("TitleScreen");

    }
}
