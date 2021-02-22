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
        try
        {
            rb = GetComponent<Rigidbody>();
            Fire();
        }
        catch(System.Exception ex)
        {
            DebugManagerScript.Instance.AddMessage("Failed to start projectile fire: " + ex.Message + " STACK:" + ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    ///     DEPRECIATED. Stub left incase needed later
    /// </summary>
    public void Fire()
    {
        //DebugManagerScript.Instance.AddMessage("Audio clip on proectile triggered");

    }

    
    // continue on its flight path
    public void FixedUpdate()
    {
        //increment life so these dont pile up toward infinity
        currentLifeSpan += Time.deltaTime;

        if (currentLifeSpan > maxLifeSpan)
        {

            Destroy(gameObject);
        }

        rb.velocity = transform.forward * ProjectileVelocity;

        //sever connection to parent after first pass so it doesnt track the parent
        transform.parent = null;

    }

    /// <summary>
    /// Check to see if we he the target
    /// </summary>
    /// <param name="collision"></param>
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

            Destroy(gameObject);
        }
    }
}