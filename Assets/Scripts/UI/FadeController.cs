using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    FadeOut fade;

    void Start()
    {
       fade = FindObjectOfType<FadeOut>();

        fade.FadeIn();
    }
}
