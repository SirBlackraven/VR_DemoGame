using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectileScript : MonoBehaviour
{
    float ProjectileVelocity = 300;

    float TurnRate = 20f;

    Rigidbody homingMissile;

    GameObject ProjectileModel;

    GameObject emitter;

    ParticleSystem smokeParticlesSys;

    AudioClip fireSoundEffect;

    private GameObject target;

    private float maxLifeSpan = 10.0f;
    private float currentLifeSpan = 0.0f;

     // Start is called before the first frame update
    void Start()
    {
            
        Rigidbody body = ProjectileModel.GetComponent<Rigidbody>();
        homingMissile = body;
        
        Fire();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Fire()
    {
        //AudioSource.PlayClipAtPoint(fireSoundEffect, emitter.transform.position);

        var distance = Mathf.Infinity;

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("enemy"))
        {
            var diff = (g.transform.position = transform.position).sqrMagnitude;

            if (diff < distance)
            {
                distance = diff;

                target = g;
            }
        }
    }

    public void FixedUpdate()
    {
        if (target == null || homingMissile == null)
        { return; }

        homingMissile.velocity = transform.forward * ProjectileVelocity;

        Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);

        homingMissile.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, TurnRate));
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "enemy")
        {
            //smokeParticlesSys.emissionRate = 0.0f;
            Destroy(ProjectileModel.gameObject);
            Destroy(gameObject);
        }
    }
}
