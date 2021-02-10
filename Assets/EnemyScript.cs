using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public GameObject Target;

    public float MovementVelocity = 40;

    public float TurnRate = 20f;
    Rigidbody actorBody;

    public bool IsShooter = false;
    public float FiringDelay = 5.0f;
    private float currentFiringCoolDown = 1.0f;

    public float ExplosionLifespan = 1.2f;
    private float currentExplosionTime = 0.0f;
    private bool isDead = false;

    public GameObject EnemyAvatar;
    public GameObject DeathEffect;

    public int Points = 250;
    public int CollisionDamage = 25; //range is 0-100
    public int ProjectileDamage = 20;

    public GameObject WeaponProjectile;
    public EnemyShotSpawner spawner;

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
       // DebugManagerScript.Instance.AddMessage("Enemy updating.");

        //this would happen if the game is reset (all players killed, for example)
        if(GameStateManagerScript.Instance.GameActive == false)
        {
            Destroy(gameObject);
            return;
        }

        if (isDead)
        {
            UpdateDeathEffect();
        }
        else
        {
            UpdateEnemyMovement();
        }
    }

    void AcquireTarget()
    {
        //AudioSource.PlayClipAtPoint(fireSoundEffect, emitter.transform.position);
        //DebugManagerScript.Instance.AddMessage("Seeker acquiring target.");
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

    private void UpdateDeathEffect()
    {
        currentExplosionTime += Time.deltaTime;

        if(currentExplosionTime >= ExplosionLifespan)
        {
            DeathEffect.SetActive(false);
            Destroy(gameObject);
        }

        
    }

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
        //DebugManagerScript.Instance.AddMessage("actor velocity:" + actorBody.velocity.ToString());


        Quaternion targetRotation = Quaternion.LookRotation(Target.transform.position - transform.position);

        actorBody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, TurnRate));

        if(IsShooter)
        {
            currentFiringCoolDown += Time.deltaTime;

            if(currentFiringCoolDown > FiringDelay)
            {
                DebugManagerScript.Instance.AddMessage("bug firing");
                currentFiringCoolDown = 0;

                try
                {
                    // var emitterScript = gameObject.GetComponent<EnemyShotSpawner>();
                    //emitterScript.EnemyFire();
                    spawner.EnemyFire();
                }
                catch(System.Exception ex)
                {
                    DebugManagerScript.Instance.AddMessage("Error getting shot spawner:" + ex.Message);
                }
            }
        }

    }

    /*private void EnemyFire()
    {
        DebugManagerScript.Instance.AddMessage("Enemy Fired");
        try
        {
            Instantiate(WeaponProjectile, transform);
            WeaponProjectile.transform.parent = null;
        }
        catch(System.Exception ex)
        {
            DebugManagerScript.Instance.AddMessage("ERROR creating enemy fire:" + ex.Message);
        }
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        //DebugManagerScript.Instance.AddMessage("Seeker collision detected (collision):" + collision.gameObject.tag);

       /* if (collision.gameObject.tag == "Player")
        {
            //destroy yourself   
            GameStateManagerScript.Instance.SeekerKilled();
            GameStateManagerScript.Instance.AddScore(0, Points);
            Destroy(gameObject);

            //damage player
            GameStateManagerScript.Instance.ApplyDamage(0, CollisionDamage);

            EnemyAvatar.SetActive(false);
            DeathEffect.SetActive(true);

            isDead = true;

            SphereCollider collider = gameObject.GetComponent<SphereCollider>();

            collider.enabled = false;
        }*/
    }

    private void OnTriggerEnter(Collider collision)
    {
        //DebugManagerScript.Instance.AddMessage("Seeker collision detected (trigger):" + collision.gameObject.tag);
       if (collision.gameObject.tag == "Player")
        {
            Collider col = gameObject.GetComponent<Collider>();
            col.enabled = false;

            Killed();

            //DebugManagerScript.Instance.AddMessage("Apply damage");
            GameStateManagerScript.Instance.ApplyDamage(0, 25);
        }
    }

    public void Killed()
    {
        

        EnemyAvatar.SetActive(false);
        //DebugManagerScript.Instance.AddMessage("Seeker Killed");
        GameStateManagerScript.Instance.SeekerKilled();
        DebugManagerScript.Instance.AddMessage("Add score");
        GameStateManagerScript.Instance.AddScore(0, Points);
        
       // DebugManagerScript.Instance.AddMessage("Deactivate avatar");
        
        //DebugManagerScript.Instance.AddMessage("Activate explosion");
        DeathEffect.SetActive(true);

        isDead = true;
    }
}
