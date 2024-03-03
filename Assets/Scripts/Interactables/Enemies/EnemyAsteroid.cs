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

    private bool shatteredRock;

    private EntityMovement moveController;

    private GameManager Manager => GameManager.Instance;

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
        if (Manager.IsFishCaught == false)
            return;

        if (collision.gameObject.CompareTag("Fish"))
        {
            BaseFish fish = collision.gameObject.GetComponent<BaseFish>();

            //this is a rock fish
            if (fish is RockFish rockFish)
            {
                //only allow rock fish to escape if it has been shattered and this asteroid was not the one that shattered the rock
                if (shatteredRock == false && rockFish.IsShattered)
                    Manager.OnFishEscape();
                //if rock fish is not shattered yet, shatter it
                else
                {
                    shatteredRock = true;
                    Manager.OnFishShatter();
                }                 
            }
            //this is a regular fish
            else
            {
                //allow fish to escape
                if (fish.state == BaseFish.FishState.hooked)
                    Manager.OnFishEscape();
            }         
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
