using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UNUSED
/// There might be need of this later for syncing projectiles across the network
/// </summary>
public class ProjectileManagerScript : MonoBehaviour
{
    public bool GameMode = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FireLeftEmitter()
    {
        
    }

    public void FireRightEmitter()
    {
        if (!GameMode)
        {
            return;
        }
    }

    public void SetGameAcive()
    {
        this.GameMode = true;
    }
}
