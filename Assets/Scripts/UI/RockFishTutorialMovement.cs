using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFishTutorialMovement : MonoBehaviour
{
    [Header("Movement Settings")] 
    [SerializeField] private float moveSpeed;

    [Header("Wave Settings")]
    [SerializeField] private float waveStrength;
    [SerializeField] private float waveSpeed;

    private bool moveLeft = true;
    private SpriteRenderer sr = null;

    private EntityMovement moveController;

    private void Start()
    {
        moveController = new(moveLeft, moveSpeed, waveStrength, waveSpeed, sr, transform);

    }
    private void FixedUpdate()
    {
        moveController.Move();
    }
}
