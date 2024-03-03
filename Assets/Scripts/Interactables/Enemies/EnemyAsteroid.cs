using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAsteroid : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private bool moveLeft;
    [SerializeField] private float moveSpeed;

    [Header("Rotation Settings")]
    [SerializeField] private bool rotateLeft;
    [SerializeField] private float rotationSpeed;

    [Header("Wave Settings")]
    [SerializeField] private float waveStrength;
    [SerializeField] private float waveSpeed;

    [Header("References")]
    [SerializeField] private SpriteRenderer sr;

    private EntityMovement moveController;

    private void Start()
    {
        moveController = new(moveLeft, moveSpeed, waveStrength, waveSpeed, sr, transform);
    }

    private void FixedUpdate()
    {
        moveController.Move();
        Rotate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Instance.IsFishCaught == false)
            return;

        if (collision.gameObject.CompareTag("Fish"))
        {
            if (collision.gameObject.GetComponent<BaseFish>().state == BaseFish.FishState.caught)
                GameManager.Instance.OnFishEscape();
        }          
    }

    private void Rotate()
    {
        if (rotateLeft)
            transform.Rotate(rotationSpeed * Time.deltaTime * Vector3.forward);
        else
            transform.Rotate(rotationSpeed * Time.deltaTime * Vector3.back);
    }
}
