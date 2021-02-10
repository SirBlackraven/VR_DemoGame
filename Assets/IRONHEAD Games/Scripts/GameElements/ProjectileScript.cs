using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float ProjectileVelocity;

    float TurnRate = 0;

    public GameObject Model;
    public GameObject soundEmitter;

    public AudioClip FireSoundEffect;

    private float maxLifeSpan = 5.0f;
    private float currentLifeSpan = 0.0f;

    private Transform initialTransform;

    private Rigidbody rb;




    // Start is called before the first frame update
    void Start()
    {
        //projectile = transform.rig
        rb = GetComponent<Rigidbody>();
        Fire();

        //initialTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Fire()
    {
        //DebugManagerScript.Instance.AddMessage("Audio clip on proectile triggered");
        //AudioSource.PlayClipAtPoint(FireSoundEffect, soundEmitter.transform.position);

    }

    public void FixedUpdate()
    {

        //DebugManagerScript.Instance.AddMessage("Fixed update; projectile position now " + this.transform.position.ToString() + " lifespan-" + currentLifeSpan.ToString());

        

        //set a max life so these dont pile up toward infinity
        currentLifeSpan += Time.deltaTime;

        if (currentLifeSpan > maxLifeSpan)
        {

            Destroy(gameObject);
        }

        //transform.position = Vector3.MoveTowards(transform.position, this.EndPosition, Time.deltaTime * Speed);
        
        //rb.velocity = initialTransform.forward * ProjectileVelocity;
        rb.velocity = transform.forward * ProjectileVelocity;

        transform.parent = null;

    }

    private void OnTriggerEnter(Collider collision)
    {
        DebugManagerScript.Instance.AddMessage("Projectile collision detected:" + collision.gameObject.tag);
        if (collision.gameObject.tag == "enemy")
        {
            Collider col = gameObject.GetComponent<Collider>();
            col.enabled = false;

            Collider enemyCollider = collision.gameObject.GetComponent<Collider>();
            enemyCollider.enabled = false;

            EnemyScript script = collision.gameObject.GetComponent<EnemyScript>();

            script.Killed();

            //Destroy(collision.gameObject);
            Destroy(gameObject);
            //GameStateManagerScript.Instance.SeekerKilled();
        }
    }
}