using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public const float ZAP_TIME = 2.8f;

    [SerializeField] Animator playerAnim;
    [SerializeField] Transform catchPoint;

    private BaseFish currentFish = null;
    public bool IsFishCaught => currentFish != null;

    private bool zapped;
    public bool CannotCatchFish => IsFishCaught || zapped;


    private void Awake()
    {
        Instance = this;
    }

    public void OnFishCaught(BaseFish fish)
    {
        if (fish != null)
        {
            currentFish = fish;
            currentFish.Caught(catchPoint);
        }      
    }

    public void OnFishEscape()
    {
        if (IsFishCaught)
        {
            currentFish.Escape();

            //only player lost animation if not being zapped as to not override zap animation
            if (zapped == false)
                playerAnim.SetTrigger("Lost");
        }
            
    }

    public void OnPlayerZap()
    {
        playerAnim.SetTrigger("Zap");
        zapped = true;
        StartCoroutine(ZapTimer());
    }

    private IEnumerator ZapTimer()
    {
        yield return new WaitForSeconds(ZAP_TIME);
        zapped = false;
    }
}