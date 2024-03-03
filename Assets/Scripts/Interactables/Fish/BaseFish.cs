using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFish : MonoBehaviour
{
    protected const float MIN_ESCAPE_ANGLE = 45;
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

    [Header("Other Settings")]
    [SerializeField] protected float catchPointOffset;

    protected Vector3 escapeDirection;

    public enum FishState { swim, caught, escaped }
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

    //makes fish swim until state is changed 
    protected virtual void Swim()
    {
        swimController?.Move();
    }

    //gets called by fish line when it touches this fish
    public void Caught(Transform catchPoint)
    {
        if (state == FishState.caught)
            return;

        state = FishState.caught;

        //make the fish follow the lure with an offset 
        transform.SetParent(catchPoint, false);
        transform.localPosition = new Vector3(0, catchPointOffset, 0);
    }

    //gets called by enemy if it touches this fish
    public void Escape()
    {
        if (state == FishState.escaped)
            return;

        state = FishState.escaped;

        //remove the parent so the fish stops following the lure
        transform.SetParent(null);

        //make fish face random direction 
        float angle = Random.Range(MIN_ESCAPE_ANGLE, MAX_ESCAPE_ANGLE);
        transform.rotation = Quaternion.Euler(0, 0, angle);
        escapeDirection = new Vector3(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad), 0f);
    }

    //fish swims away towards random angle downward
    private void SwimAway()
    {
        transform.position += -escapeSpeed * Time.deltaTime * escapeDirection;
    } 
}
