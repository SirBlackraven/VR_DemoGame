using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawner for a player's projectile. Effectively a "Gun"
/// </summary>
public class ProjectileSpawnerScript : MonoBehaviour
{
    public ProjectileScript Projectile;                     //ref to the projectile
    public AudioSource SoundEffectSource;                   //sound effect gun makes
    public MultiplayerVRSynchronizationScript syncronizer;  //Syncronization code for mu games 
    
    public float FireRateDelay = 0.10f;                     //cooldown on gone fire. Not used...yet

    public void FireProjectile()
    {
        if (GameStateManagerScript.Instance.GameActive == false)
        { return; }

        if(SoundEffectSource != null)
        {
            SoundEffectSource.PlayOneShot(SoundEffectSource.clip);
            
        }

        try
        {
            Instantiate(Projectile, transform);
            Rigidbody rb = Projectile.GetComponent<Rigidbody>();
            rb.velocity = transform.forward * Projectile.ProjectileVelocity;
        }
        catch (System.Exception ex)
        {
            DebugManagerScript.Instance.AddMessage("Projectile spawn error:" + ex.Message + "|Inner message:" + ex.InnerException);
        
        }        
    }

    /// <summary>
    /// RPC to sync fire in a MU game. 
    /// </summary>
    /// <param name="shooterId"></param>
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
        
        //TODO - Finish

    }

}
