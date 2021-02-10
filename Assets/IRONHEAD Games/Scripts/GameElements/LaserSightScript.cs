using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSightScript : MonoBehaviour
{
    private LineRenderer lr;

    private bool SuppressBeam = true;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if(SuppressBeam)
        {
            return;
        }*/


        //Beam goes to max dist no matter what
        //lr.SetPosition(1, new Vector3(0, 0, 5000));

        //Optional: Trim beam when it hits something
         RaycastHit hit;

         if(Physics.Raycast(transform.position, transform.forward, out hit))
         {
             if(hit.collider)
             {
                lr.SetPosition(1, new Vector3(0, 0, hit.distance));
             }
         }
         else
         {
             lr.SetPosition(1, new Vector3(0, 0, 5000));
         }
    }

    public void SetSupressBeam()
    {
        this.SuppressBeam = true;
    }

    public void UnsetSupressBeam()
    {
        this.SuppressBeam = false;
    }
}
