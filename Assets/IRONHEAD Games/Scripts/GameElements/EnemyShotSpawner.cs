using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotSpawner : MonoBehaviour
{
    public EnemyProjectileScript WeaponProjectile;

    // Start is called before the first frame update
    /*void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/

    public void EnemyFire()
    {
        DebugManagerScript.Instance.AddMessage("Enemy Fired");
        try
        {
            Instantiate(WeaponProjectile, transform);
            Rigidbody rb = WeaponProjectile.GetComponent<Rigidbody>();
            rb.velocity = transform.forward * WeaponProjectile.ProjectileVelocity;
            //WeaponProjectile.transform.parent = null;
        }
        catch (System.Exception ex)
        {
            DebugManagerScript.Instance.AddMessage("ERROR creating enemy fire:" + ex.Message);
        }
    }
}
