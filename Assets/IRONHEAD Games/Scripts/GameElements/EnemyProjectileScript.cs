using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileScript : MonoBehaviour
{
    public float ProjectileVelocity;

    float TurnRate = 0;

    public GameObject Model;
   // public GameObject soundEmitter;

    public AudioSource FireSoundEffect;

    private float maxLifeSpan = 8.0f;
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
        FireSoundEffect.PlayOneShot(FireSoundEffect.clip);

    }

    public void FixedUpdate()
    {

        //DebugManagerScript.Instance.AddMessage("Fixed update; Enemy projectile position now " + this.transform.position.ToString() + " lifespan-" + currentLifeSpan.ToString());


        //set a max life so these dont pile up toward infinity
        currentLifeSpan += Time.deltaTime;

        if (currentLifeSpan > maxLifeSpan || GameStateManagerScript.Instance.GameActive == false)
        {

            Destroy(gameObject);
        }

        rb.velocity = transform.forward * ProjectileVelocity;

        transform.parent = null;


    }

    private void OnTriggerEnter(Collider collision)
    {
        DebugManagerScript.Instance.AddMessage("Projectile collision detected:" + collision.gameObject.tag);
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "AITarget")
        {
            GameStateManagerScript.Instance.ApplyDamage(0, 20);

            //Destroy(collision.gameObject);
            Destroy(gameObject);
            MechGameAudioManagerScript.Instance.PlayerHit();
        }
    }
}
