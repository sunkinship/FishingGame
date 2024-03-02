using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFish : MonoBehaviour
{
    protected const float MIN_ESCAPE_ANGLE = 45;
    protected const float MAX_ESCAPE_ANGLE = 135;
    protected const float TARGET_ESCAPE_X = 135;

    public bool escape;

    [Header("Movement Settings")]
    [SerializeField] protected bool moveLeft;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float waveStrength;
    [SerializeField] protected float waveSpeed;
    [SerializeField] protected float escapeSpeed;

    [Header("References")]
    [SerializeField] protected SpriteRenderer sr;

    [Header("Other Settings")]
    [SerializeField] protected float catchPointOffset;

    protected Vector3 escapeDirection; 

    protected FishState state = FishState.swim;   

    protected EntityMovement swimController;

    protected void Start()
    {
        swimController = new(moveLeft, moveSpeed, waveStrength, waveSpeed, sr, transform);
    }

    private void Update()
    {
        if (escape)
            Escaped();
    }

    protected void FixedUpdate()
    {
        if (state == FishState.swim)
            Swim();
        else if (state == FishState.escaped)
            SwimAway();
    }

    //makes fish swim until state is changed 
    protected void Swim()
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
        transform.position = new Vector3(0, catchPointOffset, 0);
    }

    //gets called by enemy if it touches this fish
    public void Escaped()
    {
        if (state == FishState.escaped)
            return;

        state = FishState.escaped;

        //make fish face random direction 
        float angle = Random.Range(MIN_ESCAPE_ANGLE, MAX_ESCAPE_ANGLE);
        transform.rotation = Quaternion.Euler(0, 0, angle);
        escapeDirection = new(0, 0, angle);
    }

    //fish swims away towards random angle downward
    private void SwimAway()
    {
        transform.position += escapeSpeed * Time.deltaTime * escapeDirection;
    }

    protected enum FishState { swim, caught, escaped }
}
