using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovement 
{
    public float moveSpeed;
    public float waveStrength;
    public float waveSpeed;

    private SpriteRenderer sr;
    private Transform transform;

    private float startingY;
    private Vector2 nextPos;

    public EntityMovement(bool moveLeft, float moveSpeed, float waveStrength, float waveSpeed, SpriteRenderer sr, Transform transform)
    {
        this.moveSpeed = moveSpeed;
        this.waveStrength = waveStrength;
        this.waveSpeed = waveSpeed;
        this.sr = sr;
        this.transform = transform;

        nextPos = transform.position;
        startingY = transform.position.y;

        if (!moveLeft)
        {
            this.sr.flipX = true;
            this.moveSpeed *= -1;
        }
    }

    public void Move()
    {
        nextPos.y = startingY + (Mathf.Sin(Time.time * waveSpeed) * waveStrength);
        nextPos.x += moveSpeed * -0.1f;
        transform.position = nextPos;
    }
}
