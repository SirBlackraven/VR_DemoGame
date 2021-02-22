using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is for the enemy projectile prefab
/// </summary>
public class EnemyProjectileScript : MonoBehaviour
{
    public float ProjectileVelocity;

    float TurnRate = 0;             //for tracking projectiles. (Not in yet)

    public GameObject Model;

    public AudioSource FireSoundEffect;     //sound players hears when fired

    private float maxLifeSpan = 8.0f;       //how long the shot travels before being despawned
    private float currentLifeSpan = 0.0f;   //how long this shot has been in game

    private Rigidbody rb;       //ref to rigid body


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Fire();
    }

    /// <summary>
    /// Plays when created
    /// </summary>
    public void Fire()
    {
        FireSoundEffect.PlayOneShot(FireSoundEffect.clip);
    }

    public void FixedUpdate()
    {
        //check flight time
        currentLifeSpan += Time.deltaTime;

        if (currentLifeSpan > maxLifeSpan || GameStateManagerScript.Instance.GameActive == false)
        {
            Destroy(gameObject);
        }

        rb.velocity = transform.forward * ProjectileVelocity;

        //sever attachment so the object doesnt keep tracking the parent spawner
        //This is only needed on the first pass, but 1 pass is needed to start
        //the projectile moving, so it cant be done upon instantiate
        transform.parent = null;
    }

    /// <summary>
    /// Collision check for this projectile.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter(Collider collision)
    {
        //check to see if projectile his the player model or the 'target' at its center for aligning shots
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "AITarget")
        {
            //apply damage
            GameStateManagerScript.Instance.ApplyDamage(0, 20);

            Destroy(gameObject);

            //play player hit sound
            MechGameAudioManagerScript.Instance.PlayerHit();
        }
    }
}
