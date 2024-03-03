using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFish : BaseFish
{
    [Header("Rock Fish Settings")]
    [SerializeField] private Animator anim;

    private bool isShattered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //no need to check for asteroid if already shattered
        if (isShattered)
            return;

        if (collision.gameObject.CompareTag("Asteroid"))
        {
            anim.SetTrigger("Shatter");
        }
    }
}
