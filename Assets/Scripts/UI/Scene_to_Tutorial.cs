using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_to_Tutorial : MonoBehaviour
{

    FadeOut fade;

    private void Start()
    {
        fade = FindObjectOfType<FadeOut>();
    }

    public IEnumerator ChangeScene()
    {
        fade.FadeeOut();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Tutorial");

    }
    public void Switch()
    {
        StartCoroutine(ChangeScene());
    }
}