using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private bool moveLeft;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float waveStrength;
    [SerializeField] private float waveSpeed;

    [Header("References")]
    [SerializeField] private SpriteRenderer sr;

    private float startingY;
    private Vector2 nextPos;

    private void Start()
    {
        nextPos = transform.position;
        startingY = transform.position.y;

        if (!moveLeft)
        {
            sr.flipX = true;
            moveSpeed *= -1;
        }    
    }

    private void FixedUpdate()
    {
        MoveFish();
    }

    private void MoveFish()
    {
        CalcualteY();
        CalculateX();
        transform.position = nextPos;
    }

    private void CalcualteY()
    {     
        nextPos.y = startingY + (Mathf.Sin(Time.time * waveSpeed) * waveStrength);
    }

    private void CalculateX()
    {
        nextPos.x += moveSpeed * -0.1f;
    }
}
