using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsToTitle : MonoBehaviour
{
    [SerializeField] private FadeController fade;

    private void Update()
    {
        if (Input.anyKey)
        {
            StartCoroutine(ChangeScene());
        }
    }

    private IEnumerator ChangeScene()
    {
        fade.FadeOut();
        AudioManager.Instance.StopMusic();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("TitleScreen");

    }
}
