using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimation : MonoBehaviour
{
    [SerializeField] private Image ui_Image;
    [SerializeField] private Sprite[] ui_spriteArray;
    [SerializeField] private float animationSpeed = 0.2f;

    private int spriteIndex;

    private void Start()
    {
        StartCoroutine(PlayUIAnimation());
    }

    IEnumerator PlayUIAnimation()
    {
        while (true)
        {
            //if end of animation reached, reset to start of array
            if (spriteIndex >= ui_spriteArray.Length)
                spriteIndex = 0;

            //go to next sprite in array
            ui_Image.sprite = ui_spriteArray[spriteIndex];
            spriteIndex++;

            yield return new WaitForSeconds(animationSpeed);
        }     
    }
}
