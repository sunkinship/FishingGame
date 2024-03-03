using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private bool rotateLeft;
    [SerializeField] private float rotationSpeed;

    [Header("Wave Settings")]
    [SerializeField] private float waveStrength;
    [SerializeField] private float waveSpeed;  

    private float startingY;
    private Vector2 nextPos;

    private void Start()
    {
        nextPos = transform.position;
        startingY = transform.position.y;
    }

    private void FixedUpdate()
    {
        MoveAsteroid();
        Rotate();
    }

    private void MoveAsteroid()
    {
        nextPos.y = startingY + (Mathf.Sin(Time.time * waveSpeed) * waveStrength);
        transform.position = nextPos;
    }

    private void Rotate()
    {
        if (rotateLeft)
            transform.Rotate(rotationSpeed * Time.deltaTime * Vector3.forward);
        else
            transform.Rotate(rotationSpeed * Time.deltaTime * Vector3.back);
    } 
}
