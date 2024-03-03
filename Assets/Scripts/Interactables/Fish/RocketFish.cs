using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketFish : BaseFish
{
    private const float COUNTDOWN_TIME = 3;

    [Header("Rocket Fish Settings")]
    [SerializeField] private float entryTime;
    [SerializeField] private float rocketSpeed;
    [SerializeField] Transform graphic;

    private Vector3 ogOffset;

    protected override void Start()
    {
        base.Start();

        if (moveLeft == false)
            SetRocketRight();

        ogOffset = graphic.localPosition;

        StartCoroutine(EntryAndCountdown());
    }

    public override void Initialize(bool moveLeft, float moveSpeed, float waveStrength, float waveSpeed)
    {
        base.Initialize(moveLeft, moveSpeed, waveStrength, waveSpeed);

        //save the sprite's offset so the position can be restored when escaping since the position is lost when caught to account for the centered struggle animation 
        ogOffset = graphic.localPosition;

        if (moveLeft == false)
            SetRocketRight();

        StartCoroutine(EntryAndCountdown());       
    }

    public void SetRocketRight()
    {
        anim.SetTrigger("Right");
        sr.flipX = false; //undo the flipping that the move script does since the rocket fish has its own right sprite 
        graphic.localPosition = new(-0.9f, 0, 0);
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
        swimController.moveSpeed = moveLeft ? rocketSpeed : -rocketSpeed;
        swimController.waveSpeed = 0;
        swimController.waveStrength = 0;
    }

    public override void Hooked(Transform catchPoint)
    {
        base.Hooked(catchPoint);
        //set child object with sprite to parent position since the struggle animation is centered 
        graphic.localPosition = Vector3.zero;
    }

    public override void Escape()
    {
        base.Escape();
        graphic.localPosition = ogOffset;
    }
}
