using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_To_MainGame : MonoBehaviour
{

    FadeController fade;

    private void Start()
    {
        fade = FindObjectOfType<FadeController>();
    }

    


    public IEnumerator ChangeScene()
    {
        fade.FadeOut();
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Fishing");

    }
    public void Switch()
    {
        StartCoroutine(ChangeScene());
    }



    void Update()
    {
        if (Input.anyKey)
        {
            StartCoroutine(ChangeScene());
        }
    }
    
}
