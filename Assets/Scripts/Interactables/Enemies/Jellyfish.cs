using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jellyfish : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private bool moveLeft;
    [SerializeField] private float moveSpeed;

    [Header("Wave Settings")]
    [SerializeField] private float waveStrength;
    [SerializeField] private float waveSpeed;

    [Header("References")]
    [SerializeField] private SpriteRenderer sr;

    private EntityMovement moveController;

    private GameManager Manager => GameManager.Instance;

    private void Start()
    {
        moveController = new(moveLeft, moveSpeed, waveStrength, waveSpeed, sr, transform);
    }

    private void FixedUpdate()
    {
        moveController.Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Line"))
        {
            //zap player
            Manager.OnPlayerZap();

            //if there is a caught fish, free it
            if (Manager.IsFishCaught)
                Manager.OnFishEscape();
        }
    }
}
