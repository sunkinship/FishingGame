using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketFish : BaseFish
{
    private const float COUNTDOWN_TIME = 3;

    [Header("Rocket Fish Settings")]
    [SerializeField] private float entryTime;
    [SerializeField] private float rocketSpeed;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(EntryAndCountdown());
    }

    private IEnumerator EntryAndCountdown()
    {
        yield return new WaitForSeconds(entryTime);
        SetToCountdown();
        yield return new WaitForSeconds(COUNTDOWN_TIME);
        SetToRocket();
    }

    //freezes movement so fish only moves up and down while counting down
    private void SetToCountdown()
    {
        anim.SetTrigger("Countdown");
        swimController.moveSpeed = 0;
    }

    //set speed to rocket speed when firing  
    private void SetToRocket()
    {
        swimController.moveSpeed = rocketSpeed;
        swimController.waveSpeed = 0;
        swimController.waveStrength = 0;
    }
}
