using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //make sure we don't already have a fish on the lures
        if (GameManager.Instance.CannotCatchFish)
            return;

        if (collision.gameObject.CompareTag("Fish"))
        {
            GameManager.Instance.OnFishHooked(collision.gameObject.GetComponent<BaseFish>());
        }
    }
}
