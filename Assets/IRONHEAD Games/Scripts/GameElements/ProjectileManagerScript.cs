using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManagerScript : MonoBehaviour
{
    public GameObject LeftLaserEmitter;
    public GameObject RightLaserEmitter;

    public GameObject BaseEnergyWeaponAsset;

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
