using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Laser sights on the player's arm cannons (on the VR grip button by default)
/// </summary>
public class LaserSightScript : MonoBehaviour
{
    private LineRenderer lr;
    private bool SuppressBeam = false;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            lr = GetComponent<LineRenderer>();
        }
        catch(System.Exception ex)
        {
            DebugManagerScript.Instance.AddMessage("Failed to get laser sight line renderer: " + ex.Message + " Stack:" + ex.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(SuppressBeam)
        {
            return;
        }

        //Option:
        //Beam goes to max dist no matter what
        //lr.SetPosition(1, new Vector3(0, 0, 5000));

        //Option: Trim beam when it hits something
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

    /// <summary>
    /// Utiltiy functions to disable/enable the beam from an external source (if needed)
    /// </summary>
    public void SetSupressBeam()
    {
        this.SuppressBeam = true;
    }

    public void UnsetSupressBeam()
    {
        this.SuppressBeam = false;
    }
}
