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

    [HideInInspector] public bool HookedRockFish;

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

        if (!playingYayAnim)
        {
            playerAnim.SetTrigger("Yay");
            playingYayAnim = true;
        }

        //add score
        ScoreManager.Instance.AddScore(currentFish);

        HookedRockFish = false;
        Destroy(currentFish.gameObject);

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
}