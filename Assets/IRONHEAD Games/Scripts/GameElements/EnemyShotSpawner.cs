using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotSpawner : MonoBehaviour
{
    public EnemyProjectileScript WeaponProjectile;

    /// <summary>
    /// Basic enemy fire function. Attached to the emitter point
    /// </summary>
    public void EnemyFire()
    {
        try
        {
            Instantiate(WeaponProjectile, transform);
            Rigidbody rb = WeaponProjectile.GetComponent<Rigidbody>();
            rb.velocity = transform.forward * WeaponProjectile.ProjectileVelocity;
        }
        catch (System.Exception ex)
        {
            DebugManagerScript.Instance.AddMessage("ERROR creating enemy fire:" + ex.Message);
        }
    }
}
