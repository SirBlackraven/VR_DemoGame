using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to extend the Elevator script for Hangar doors, which operate in a similar way
/// </summary>
public class HangarDoorScript : MonoBehaviour
{
    public GameObject HangarDoor;           //Door model
    public AudioSource DoorSoundSource;     //open/close SFX

    /// <summary>
    /// This is the 'trip wire' to close the hangar door when the player exits
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter(Collider collision)
    {
       
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "AITarget")
        {
            ElevatorScript script = HangarDoor.GetComponent<ElevatorScript>();

            if(script.status == ElevatorBehavior.WAITING)
            {
                script.SetBehavior(ElevatorBehavior.DESCENDING);

                DoorSoundSource.PlayOneShot(DoorSoundSource.clip);
            }                       

            GameStateManagerScript.Instance.StartGame();
        }
    }
}
