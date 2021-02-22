using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElevatorBehavior
{
    PARKED,
    ASCENDING,
    DESCENDING,
    WAITING //has reached its apex and is waiting to desecend
}


/// <summary>
/// This script is creating and controlling more complex elevators than a basic trigger type.
/// It is also used to open/close hangar doors (and could be used in other places)
/// </summary>
public class ElevatorScript : MonoBehaviour
{
    public bool ElevatorActivated = false;
    public float Speed = 2;

    public float HoldTime = 5;          //time the elevators stays in a position before returning to its home position
    private float currentHoldTime = 0;  //how long the elevator has been waiting (compared to HoldTime)

    public bool SingleUse = false; //makes the elevator a one-time use operation. Use with caution to avoid entrapping a player
    private bool hasBeenTriggered;  //special flag for use with the SingleUse indicator    

    public Vector3 EndPosition;     //Where the e elevator is to go to
    public Vector3 StartPosition;   //initial position

    public bool DoNotReset = false; //Causes the elevator to stop after its first movement. Use to hold a platform in place or similar

    public ElevatorBehavior status = ElevatorBehavior.PARKED;  //start state

    public AudioSource elevatorSFX; //sound effect for the moving elevator

    // Start is called before the first frame update
    void Start()
    {
        //save initial position from gui editor
        this.StartPosition = this.transform.position;
        status = ElevatorBehavior.PARKED;
    }

    /// <summary>
    /// Gateway to set the elevator status from an external source
    /// </summary>
    /// <param name="newStatus"></param>
    public void SetBehavior(ElevatorBehavior newStatus)
    {
        this.status = newStatus;
    }

    public void ElevatorActivate()
    {
        if (this.ElevatorActivated == false)
        {
            DebugManagerScript.Instance.AddMessage("Elevator activated. Current position:" + this.StartPosition.ToString());
            this.ElevatorActivated = true;
            this.status = ElevatorBehavior.ASCENDING;
            this.hasBeenTriggered = true; //for single use logic

            if (elevatorSFX != null)
            {
                elevatorSFX.PlayOneShot(elevatorSFX.clip);
            }

            //set triggered flag. Only used in 1 shot elevators
            hasBeenTriggered = true;
        }
    }
    
    /// <summary>
    /// Utility function to freeze an elevator immediately. 
    /// </summary>
    public void ElevatorDeactivate()
    {
        if (this.ElevatorActivated == true)
        {
            this.ElevatorActivated = false;
            this.status = ElevatorBehavior.PARKED;
        }
    }

    void Update()
    {
        if (!ElevatorActivated)
        {
            //not in use. bail out.
            return;
        }

        if(hasBeenTriggered && SingleUse) //single use elevator
        {
            if(status == ElevatorBehavior.PARKED)
            {
                //cycle complete
                return;
            }
        }

        switch(this.status)
        {
            case ElevatorBehavior.ASCENDING:

                transform.position = Vector3.MoveTowards(transform.position, this.EndPosition, Time.deltaTime * Speed);
                if (this.transform.position.y >= this.EndPosition.y)
                {
                    this.status = ElevatorBehavior.WAITING;
                }

                break;
            case ElevatorBehavior.DESCENDING:

                transform.position = Vector3.MoveTowards(transform.position, this.StartPosition, Time.deltaTime * Speed);
                if (this.transform.position.y <= StartPosition.y)
                {
                    status = ElevatorBehavior.PARKED;
                    
                    //reset elevator
                    currentHoldTime = 0;
                    this.transform.position = this.StartPosition;    
                    this.ElevatorActivated = false;
                }

                break;
            case ElevatorBehavior.WAITING:

                if(DoNotReset)
                {
                    return;
                }

                currentHoldTime += Time.deltaTime;
                               
                if(currentHoldTime >= HoldTime)
                {
                    status = ElevatorBehavior.DESCENDING;
                 }

                break;
            default:  //this should be impossible
                DebugManagerScript.Instance.AddMessage("Error: Unknown elevator state detected:" + status.ToString());

                break;
        }       

    }
}
