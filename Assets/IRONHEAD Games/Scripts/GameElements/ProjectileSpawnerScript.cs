using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawnerScript : MonoBehaviour
{

    public ProjectileScript Projectile;
    public AudioSource SoundEffectSource;
    public MultiplayerVRSynchronizationScript syncronizer;
    
    public float FireRateDelay = 0.10f;

    public void FireProjectile()
    {
        //DebugManagerScript.Instance.AddMessage("fire projectile called. Game active=" + GameStateManagerScript.Instance.GameActive.ToString());
        if (GameStateManagerScript.Instance.GameActive == false)
        { return; }

        //DebugManagerScript.Instance.AddMessage("fire projectile called on left controller, trying to Instantiate:");
        /* if (!GameStateManagerScript.Instance.GameActive)
         {
             return;
         }*/

        if(SoundEffectSource != null)
        {
            SoundEffectSource.PlayOneShot(SoundEffectSource.clip);
           // DebugManagerScript.Instance.AddMessage("Fire laser sound");
            
        }

        try
        {
            Instantiate(Projectile, transform);
            Rigidbody rb = Projectile.GetComponent<Rigidbody>();
            rb.velocity = transform.forward * Projectile.ProjectileVelocity;
            

           // DebugManagerScript.Instance.AddMessage("Object spawned at " + transform.position.ToString() + " velocity:" + rb.velocity.ToString());
        }
        catch (System.Exception ex)
        {
            DebugManagerScript.Instance.AddMessage("Projectile spawn error:" + ex.Message + "|Inner message:" + ex.InnerException);
        
        }

        
    }

    [PunRPC]
    public void RemoteFireProjectile(int shooterId)
    {
        if(syncronizer == null)
        {
            return;
        }

        if(syncronizer.PlayerNumber == shooterId)
        {
            return; //do nothing as it is myself
        }
        
        //foreach()

    }

}
