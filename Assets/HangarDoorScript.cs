using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangarDoorScript : MonoBehaviour
{
    public GameObject HangarDoor;
    
    public AudioSource DoorSoundSource;

    private void OnTriggerStay()
    {
        /*ElevatorScript script = HangarDoor.GetComponent<ElevatorScript>();
        script.SetBehavior(ElevatorBehavior.DESCENDING);

        DoorSoundSource.PlayOneShot(DoorSoundSource.clip);

        GameStateManagerScript.Instance.StartGame();*/
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        DebugManagerScript.Instance.AddMessage("Hangar door (trigger):" + collision.gameObject.tag);
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
