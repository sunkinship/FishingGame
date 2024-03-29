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
    [SerializeField] private Animator anim;

    private Vector3 rotationVector;

    private bool shatteredRock;

    private EntityMovement moveController;

    private GameManager Manager => GameManager.Instance;

    private bool initialized;


    private void FixedUpdate()
    {
        if (!initialized)
            return;

        moveController.Move();
        Rotate();
    }

    public virtual void InitializeDirection(bool moveLeft)
    {
        this.moveLeft = moveLeft;
        rotateLeft = moveLeft;
        rotationVector = rotateLeft ? Vector3.forward : Vector3.back;

        moveController = new(this.moveLeft, moveSpeed, waveStrength, waveSpeed, sr, transform);

        initialized = true;
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
                    HitFish();
                //if rock fish is not shattered yet, shatter it
                else
                {
                    shatteredRock = true;
                    anim.SetTrigger("Shatter");
                    Manager.OnFishShatter();
                }                 
            }
            //this is a regular fish
            else
            {
                //allow fish to escape
                if (fish.state == BaseFish.FishState.hooked)
                    HitFish();
            }         
        }     
    }

    private void HitFish()
    {
        AudioManager.Instance.PlaySFX("Hit_SFX");
        Manager.OnFishEscape();
    }

    private void Rotate()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime * rotationVector);
    }

    public void DestorySelf()
    {
        Destroy(gameObject);
    }
}
