using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFish : MonoBehaviour
{
    protected const float MIN_ESCAPE_ANGLE = 45;
    protected const float MID_ESCAPE_ANGLE = 90;
    protected const float MAX_ESCAPE_ANGLE = 135;
    protected const float TARGET_ESCAPE_X = 135;

    [Header("Movement Settings")]
    [SerializeField] protected bool moveLeft;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float escapeSpeed;

    [Header("Wave Settings")]
    [SerializeField] protected float waveStrength;
    [SerializeField] protected float waveSpeed;   

    [Header("References")]
    [SerializeField] protected SpriteRenderer sr;
    [SerializeField] protected Animator anim;

    [Header("Other Settings")]
    [SerializeField] protected Vector2 hookPointOffset;

    protected Vector3 escapeDirection;

    public enum FishState { swim, hooked, escaped }
    [HideInInspector] public FishState state = FishState.swim;   

    protected EntityMovement swimController;

    protected virtual void Start()
    {
        swimController = new(moveLeft, moveSpeed, waveStrength, waveSpeed, sr, transform);
    }

    protected virtual void FixedUpdate()
    {
        if (state == FishState.swim)
            Swim();
        else if (state == FishState.escaped)
            SwimAway();
    }

    protected virtual void Swim()
    {
        swimController?.Move();
    }

    public virtual void Hooked(Transform catchPoint)
    {
        if (state == FishState.hooked)
            return;

        state = FishState.hooked;

        //play hooked animation
        anim.SetTrigger("Hooked");

        //make the fish follow the lure with an offset 
        transform.SetParent(catchPoint);
        transform.localPosition = (Vector3)hookPointOffset;
    }

    public virtual void Escape()
    {
        if (state == FishState.escaped)
            return;

        state = FishState.escaped;

        //play swimming animation
        anim.SetTrigger("Escaped");

        //remove the parent so the fish stops following the lure
        transform.SetParent(null);

        //make fish face random direction 
        float angle;
        if (moveLeft)
            angle = Random.Range(MIN_ESCAPE_ANGLE, MID_ESCAPE_ANGLE);
        else
            angle = Random.Range(MID_ESCAPE_ANGLE, MAX_ESCAPE_ANGLE);

        transform.rotation = Quaternion.Euler(0, 0, angle);
        escapeDirection = new Vector3(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad), 0f);
    }

    //fish swims away towards random angle downward
    private void SwimAway()
    {
        transform.position += -escapeSpeed * Time.deltaTime * escapeDirection;
    } 
}
