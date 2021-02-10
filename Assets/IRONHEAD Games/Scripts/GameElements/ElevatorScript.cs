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

public class ElevatorScript : MonoBehaviour
{
    public bool ElevatorActivated = false;
    public float Speed = 2;
    public float HoldTime = 5;
    public bool SingleUse = false; //makes the elevator a one-time use operation. Use with caution to avoid entrapping a player
    private bool hasBeenTriggered;

    private float currentHoldTime = 0;    

    public Vector3 EndPosition;
    public Vector3 StartPosition;

    public bool DoNotReset = false; //Causes the elevator to stop after its first movement. Use to hold a platform in place or similar

    public ElevatorBehavior status = ElevatorBehavior.PARKED;

    public AudioSource elevatorSFX;

    // Start is called before the first frame update
    void Start()
    {
        //save initial position from gui editor
        this.StartPosition = this.transform.position;
        status = ElevatorBehavior.PARKED;
        //DebugManagerScript.Instance.AddMessage("Elevator init. Current position:" + this.StartPosition.ToString());
        //DebugManagerScript.Instance.AddMessage("set end position:" + this.EndPosition.ToString());
    }

    public void SetBehavior(ElevatorBehavior newStatus)
    {
        this.status = newStatus;
    }

    public void ElevatorActivate()
    {
        //DebugManagerScript.Instance.AddMessage("Elevator activated-BEFORE");
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
    
    public void ElevatorDeactivate()
    {
        if (this.ElevatorActivated == true)
        {
            this.ElevatorActivated = false;
            this.status = ElevatorBehavior.PARKED;
        }
    }

    // Update is called once per frame
    void Update()
    {
       // DebugManagerScript.Instance.AddMessage("Elevator listening");
        if (!ElevatorActivated)
        {

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

                // Move the object upward in world space 1 unit/second.
                //transform.Translate(Vector3.up * Time.deltaTime, Space.World);

                //DebugManagerScript.Instance.AddMessage("Elevator ascension:" + this.transform.position.ToString());
                transform.position = Vector3.MoveTowards(transform.position, this.EndPosition, Time.deltaTime * Speed);
                if (this.transform.position.y >= this.EndPosition.y)
                {
                    this.status = ElevatorBehavior.WAITING;
                   //DebugManagerScript.Instance.AddMessage("Elevator now waiting");
                }

                break;
            case ElevatorBehavior.DESCENDING:

                // Move the object downward in world space 1 unit/second.
                //transform.Translate(Vector3.down * Time.deltaTime, Space.World);

              // DebugManagerScript.Instance.AddMessage("Elevator descent:" + this.transform.position.ToString());
                transform.position = Vector3.MoveTowards(transform.position, this.StartPosition, Time.deltaTime * Speed);
                if (this.transform.position.y <= StartPosition.y)
                {
                    status = ElevatorBehavior.PARKED;
                    
                    currentHoldTime = 0;
                    this.transform.position = this.StartPosition;                    

                    this.ElevatorActivated = false;

                   //DebugManagerScript.Instance.AddMessage("Elevator stopped:" + this.StartPosition.ToString());
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
            default:
                DebugManagerScript.Instance.AddMessage("Unknown elevator state detected:" + status.ToString());

                break;
        }       

    }
}
