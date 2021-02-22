using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main control script for the enemy. Its pseudo-AI and data
/// </summary>
public class EnemyScript : MonoBehaviour
{
    public GameObject Target;

    //movement rates
    public float MovementVelocity = 40;
    public float TurnRate = 20f;

    Rigidbody actorBody;

    //projectile enemy vars
    public bool IsShooter = false;              //is this a shooting AI? 
    public float FiringDelay = 5.0f;            //shot cooldown
    private float currentFiringCoolDown = 1.0f; //cooldown timer

    public float ExplosionLifespan = 1.2f;      //how long to play the death exploding effect
    private float currentExplosionTime = 0.0f;  //explosion life span

    private bool isDead = false;                //flag to immediately halt enemy collisions and movement

    public GameObject EnemyAvatar;              //enemy model
    public GameObject DeathEffect;              //enemy death explosion

    public int Points = 250;                    //score default
    public int CollisionDamage = 25;            //damage done on impact. range is 0-100
    public int ProjectileDamage = 20;           //firing damage

    public GameObject WeaponProjectile;         //holder of projectile prefab
    public EnemyShotSpawner spawner;            //projectile emission point

    // Start is called before the first frame update
    void Start()
    {

        Rigidbody body = GetComponent<Rigidbody>();
        actorBody = body;

        AcquireTarget();
    }

    // Update is called once per frame
    void Update()
    {
        //this would happen if the game is reset (all players killed, for example)
        //remove all projectiles in the game as they are updated
        if(GameStateManagerScript.Instance.GameActive == false)
        {
            Destroy(gameObject);
            return;
        }

        //either animate explosion or continue movement
        if (isDead)
        {
            UpdateDeathEffect();
        }
        else
        {
            UpdateEnemyMovement();
        }
    }

    //Set the ai's movement/firing target
    void AcquireTarget()
    {
        var distance = Mathf.Infinity;

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("AITarget"))
        {
            var diff = (g.transform.position - transform.position).sqrMagnitude;

            if (diff < distance)
            {
                distance = diff;

                Target = g;
               // DebugManagerScript.Instance.AddMessage("Target scquired:" + g.name + "," + g.tag);
            }
        }
    }

    /// <summary>
    /// Play death explosion
    /// </summary>
    private void UpdateDeathEffect()
    {
        currentExplosionTime += Time.deltaTime;

        if(currentExplosionTime >= ExplosionLifespan)
        {
            DeathEffect.SetActive(false);
            Destroy(gameObject);
        }        
    }

    /// <summary>
    /// Continue moving
    /// </summary>
    public void UpdateEnemyMovement()
    {
        if (Target == null || actorBody == null)
        {
           
            if(actorBody == null)
            {
                DebugManagerScript.Instance.AddMessage("ACTOR Is NULL");
            }
            else
            {
                DebugManagerScript.Instance.AddMessage("TARGET Is NULL");
            }

            return;
        }        
        
        actorBody.velocity = transform.forward * MovementVelocity;

        //rotate toward player if needed
        Quaternion targetRotation = Quaternion.LookRotation(Target.transform.position - transform.position);
        actorBody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, TurnRate));

        //special case for firing enemies
        if(IsShooter)
        {
            currentFiringCoolDown += Time.deltaTime;

            if(currentFiringCoolDown > FiringDelay)
            {
                currentFiringCoolDown = 0;

                try
                {
                    spawner.EnemyFire();
                }
                catch(System.Exception ex)
                {
                    DebugManagerScript.Instance.AddMessage("Error getting shot spawner:" + ex.Message);
                }
            }
        }

    }

    private void OnTriggerEnter(Collider collision)
    {
        //DebugManagerScript.Instance.AddMessage("Seeker collision detected (trigger):" + collision.gameObject.tag);
       if (collision.gameObject.tag == "Player")
        {
            Collider col = gameObject.GetComponent<Collider>();
            col.enabled = false;

            Killed();

            //apply damage
            GameStateManagerScript.Instance.ApplyDamage(0, 25);
        }
    }

    public void Killed()
    {
        EnemyAvatar.SetActive(false);

        GameStateManagerScript.Instance.SeekerKilled();

        GameStateManagerScript.Instance.AddScore(0, Points);
        
       // DebugManagerScript.Instance.AddMessage("Deactivate avatar");
        
        //Activate explosion
        DeathEffect.SetActive(true);

        isDead = true;
    }
}
