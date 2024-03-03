using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIAnimation : MonoBehaviour
{

    public Image ui_Image;

    public Sprite[] ui_spriteArray;

    public float speed = 0.2f;

    private int indexSprite;
    Coroutine coroutineAnim;
    bool hasRun = false;
    bool isDone = false;
    private void Update()
    {
        if(hasRun == false)
        {
            StartCoroutine(PlayAnim());
        }
        
    }

    IEnumerator PlayAnim()
    {

        yield return new WaitForSeconds(speed);


        hasRun = true;

        if (indexSprite >= ui_spriteArray.Length)
        {
            indexSprite = 0;
        }
        ui_Image.sprite = ui_spriteArray[indexSprite];
        indexSprite += 1;
        if (isDone == false)
        {
            coroutineAnim = StartCoroutine(PlayAnim());
        }

    }
}
