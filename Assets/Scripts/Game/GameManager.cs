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

    private bool playingYayAnim, playingLostAnim;


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

        Destroy(currentFish.gameObject);
    }

    public void OnFishHooked(BaseFish fish)
    {
        if (fish == null) 
            return;

        currentFish = fish;
        currentFish.Hooked(catchPoint);
    }

    public void OnFishEscape()
    {
        if (IsFishCaught == false)
            return;

        currentFish.Escape();

        //only play lost animation if not being zapped and not already playing lost anim as to not override zap animation
        if (IsZapped == false && playingLostAnim == false)
            playerAnim.SetTrigger("Lost");
    }

    public void OnFishRelease()
    {
        if (IsFishCaught == false)
            return;

        currentFish.Escape();
    }

    public void OnPlayerZap()
    {
        if (IsZapped)
            return;

        playerAnim.SetTrigger("Zap");
        IsZapped = true;
        StartCoroutine(ZapTimer());
    }

    private IEnumerator ZapTimer()
    {
        yield return new WaitForSeconds(ZAP_TIME);
        IsZapped = false;
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