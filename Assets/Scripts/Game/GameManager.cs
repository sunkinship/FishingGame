using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public const float ZAP_TIME = 2.8f;

    [SerializeField] Animator playerAnim;
    [SerializeField] Transform catchPoint;

    [HideInInspector] public BaseFish currentFish = null;
    public bool IsFishCaught => currentFish != null;

    public bool IsZapped {  get; private set; }
    public bool CannotCatchFish => IsFishCaught || IsZapped;

    public bool HookedRockFish {  get; private set; }   

    private bool playingYayAnim, playingLostAnim;

    AudioManager audioManger => AudioManager.Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void OnCatchConfirm()
    {
        if (IsFishCaught == false)
            return;

        HookedRockFish = false;

        //untag fish so it can't be caught again while point anim is playing
        currentFish.tag = "Untagged";

        if (!playingYayAnim)
        {
            playingYayAnim = true;
            playerAnim.SetTrigger("Yay");           
        }

        FishPointAnim();

        currentFish = null;

        //add score
        ScoreManager.Instance.AddScore(currentFish);

        audioManger.PlaySFX("Yay_SFX");     
    }

    public void OnFishHooked(BaseFish fish)
    {
        if (fish == null) 
            return;

        if (fish.escaped)
            return;

        currentFish = fish;
        currentFish.Hooked(catchPoint);

        audioManger.PlaySFX("Hooked_SFX", 0.7f);
    }

    public void OnFishEscape()
    {
        if (IsFishCaught == false)
            return;
      
        currentFish.Escape();
        currentFish = null;

        //only play lost animation if not being zapped and not already playing lost anim as to not override zap animation
        if (IsZapped == false && playingLostAnim == false)
            playerAnim.SetTrigger("Lost");

        audioManger.PlaySFX("Hit_SFX");
    }

    public void OnFishRelease()
    {
        if (IsFishCaught == false)
            return;
        
        currentFish.Escape();
        currentFish = null;
    }

    public void OnFishShatter()
    {
        if (IsFishCaught == false)
            return;

        if (currentFish is RockFish rockFish)
        {
            rockFish.Shatter();
            audioManger.PlaySFX("Rock_Break_SFX");
        }
    }

    public void OnPlayerZap()
    {
        if (IsZapped)
            return;
        
        playerAnim.SetTrigger("Zap");
        IsZapped = true;

        audioManger.PlaySFX("Zap_SFX");
    }

    public void SetHookedRockFish(bool hookedRockFish)
    {
        if (hookedRockFish)
        {
            HookedRockFish = true;
            playerAnim.SetBool("Idle", false);
        }
        else
        {
            HookedRockFish = false;
            playerAnim.SetBool("Idle", true);
        }       
    }

    #region ANIM TRIGGERS
    public void EndZapAnim()
    {
        IsZapped = false;
    }

    public void EndLostAnim()
    {
        playingLostAnim = false;
    }

    public void EndYayAnim()
    {
        playingYayAnim = false;
    }
    #endregion

    //play point animation for fish
    private void FishPointAnim()
    {
        //make sure fish stops following line
        currentFish.transform.SetParent(null);

        //bring sprite to foreground
        if (currentFish is RocketFish rocketFish)
            rocketFish.gameObject.GetComponentInChildren<SpriteRenderer>().sortingLayerName = "Foreground";
        else
        {
            currentFish.sr.sortingLayerName = "Foreground";
        }

        //flip back so the point ui is not flipped too unless it is the rocket fish since those have a flipped sprite
        if (currentFish.moveLeft == false && currentFish is not RocketFish)
            currentFish.sr.flipX = false;

        //play anim trigger for point anim
        if (currentFish is RockFish rockFish)
        {
            if (rockFish.IsShattered)
                currentFish.anim.SetTrigger("Diamond Capture");
            else
                currentFish.anim.SetTrigger("Rock Capture");
        }
        else
        {
            currentFish.anim.SetTrigger("Capture");         
        }
    }
}