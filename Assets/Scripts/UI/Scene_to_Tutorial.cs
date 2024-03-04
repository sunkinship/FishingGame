using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_to_Tutorial : MonoBehaviour
{

    FadeController fade;

    private void Start()
    {
        fade = FindObjectOfType<FadeController>();
    }

    public IEnumerator ChangeScene()
    {
        fade.FadeOut();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Tutorial");

    }
    public void Switch()
    {
        StartCoroutine(ChangeScene());
    }
}