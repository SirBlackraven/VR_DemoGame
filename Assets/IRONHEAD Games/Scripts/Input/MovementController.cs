using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public enum RotationDirection
{
    LEFT,
    RIGHT
}

public class MovementController : MonoBehaviour
{
    //public List<XRController> controllers;

    public XRController leftController;
    public XRController rightController;

    public GameObject head = null;
    public float Speed = 3.0f;

    [SerializeField]
    TeleportationProvider teleportationProvider;

    public GameObject MainVRPlayer;
    public GameObject XRRigGameObject;

    public bool MechMode = false;
    public float CurrentThrottle = 0.0f;
    public float MaxThrottle = 25.0f;
    public float MaxReverseThrottle = 0.0f;

    public ProgressBarCircle ThrottleIndicator;
    public ProgressBar StatusIndicator;
    private float throttleConversionFactor = 4.0f;

    private Rigidbody rb;


    private void OnEnable()
    {
        ThrottleIndicator.Title = "Throttle";
        StatusIndicator.Title = "Status";
    }

    private void OnDisable()
    {
        
    }

    private void OnEndLocomotion(LocomotionSystem locomotionSystem)
    {
        Debug.Log("Teleportation ended");
        MainVRPlayer.transform.position = MainVRPlayer.transform.TransformPoint(XRRigGameObject.transform.localPosition);
        XRRigGameObject.transform.localPosition = Vector3.zero;
    }

    public void ResetRigState()
    {
        CurrentThrottle = 0.0f;
        MechMode = false;
        
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetMechMode()
    {
        MechMode = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {       

       if(MechMode)
        {
            MechMove();
           
        }
       else
        {
            AvatarMove();
        }       
    }

    private void AvatarMove()
    {
        if (leftController.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 positionVector))
        {
            if (positionVector.magnitude > 0.15f)
            {
                Move(positionVector);
            }
        }

        if (!MechMode) //this is really only for testing
        {
            if (rightController.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 verticalPositionVector))
            {
                if (verticalPositionVector.magnitude > 0.15f)
                {
                   // MoveVertical(verticalPositionVector);
                }
            }
        }
    }

    private void MechMove()
    {
        //check for throttle change:
        if (leftController.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 throttlePositionVector))
        {
            //DebugManagerScript.Instance.AddMessage("Old throttle setting" + CurrentThrottle.ToString()); ;
            // DebugManagerScript.Instance.AddMessage("throttle set magnitude:" + throttlePositionVector.magnitude.ToString());

            //Vector3 resolvedThrottleVector = new Vector3(0, 0, throttlePositionVector.y);
            Vector3 resolvedThrottleVector = throttlePositionVector;


            if (throttlePositionVector.magnitude > 0.15f) //allow for slight deadzone
            {
                if (throttlePositionVector.y < 0)
                {
                    CurrentThrottle -= 0.1f;
                    //DebugManagerScript.Instance.AddMessage("throttle down" + CurrentThrottle.ToString());

                    //DebugManagerScript.Instance.AddMessage("throttle vector" + throttlePositionVector.ToString());

                    if (CurrentThrottle < MaxReverseThrottle)
                    { CurrentThrottle = MaxReverseThrottle; }


                }
                else
                {
                    //CurrentThrottle += (throttlePositionVector.magnitude / 5);
                    CurrentThrottle += 0.1f;

                    //check to ensure they are not going to 'ludicrous speed'
                    if (CurrentThrottle > MaxThrottle)
                    {
                        CurrentThrottle = MaxThrottle;

                    }

                    //DebugManagerScript.Instance.AddMessage("throttle up:" + CurrentThrottle.ToString());
                    //DebugManagerScript.Instance.AddMessage("throttle vector" + throttlePositionVector.ToString());

               }



                //need to "snap to" zero in order to create a stop
                //if((CurrentThrottle <= 0.15f ) && (CurrentThrottle >= -0.15f))
                //{ CurrentThrottle = 0; }

                //DebugManagerScript.Instance.AddMessage("Throttle settting is now:" + CurrentThrottle.ToString());

                //sound effects (basic for now)
                if (CurrentThrottle > 0)
                {
                    MechGameAudioManagerScript.Instance.PlayMechWalking();
                }
                else
                {
                    MechGameAudioManagerScript.Instance.StopMechWalking();
                }
            }     
        }

        //check for rotation change
        if (rightController.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 rotationVector))
        {
            if (rotationVector.magnitude > 0.15f)
            {
                if (rotationVector.magnitude > 0.15f)
                {
                    if (rotationVector.x < 0)
                    {
                        Rotate(RotationDirection.RIGHT);
                    }
                    else
                    {
                       Rotate(RotationDirection.LEFT);
                    }

                   // DebugManagerScript.Instance.AddMessage("Mech roatation is now:" + CurrentThrottle.ToString());
                }
            }
        }

       try
        {
            UpdateThrottleIndicator();
        }
        catch(System.Exception ex)
        {
            DebugManagerScript.Instance.AddMessage("Error updating throttle indicator:" + ex.Message);
        }
        
        ApplyMechMovement();
    }

    private void UpdateThrottleIndicator()
    {
       // ProgressBar bar = this.ThrottleIndicator.GetComponent<ProgressBar>();

        float newBarValue = this.CurrentThrottle * this.throttleConversionFactor;

        newBarValue = Mathf.Round(newBarValue);

       // DebugManagerScript.Instance.AddMessage("Throttle indicator:" + newBarValue.ToString());
        this.ThrottleIndicator.BarValue = newBarValue;
        
        //bar.BarValue = 75.0f;
    }

    private void Move(Vector2 positionVector)
    {
        //DebugManagerScript.Instance.AddMessage("apply avatar move");
        // Apply the touch position to the head's forward Vector
        Vector3 direction = new Vector3(positionVector.x, 0, positionVector.y);
        Vector3 headRotation = new Vector3(0, head.transform.eulerAngles.y, 0);

        // Rotate the input direction by the horizontal head rotation
        direction = Quaternion.Euler(headRotation) * direction;

        // Apply speed and move
        Vector3 movement;

        movement = direction * Speed;

        transform.position += (Vector3.ProjectOnPlane(Time.deltaTime * movement, Vector3.up));
    }

    private void ApplyMechMovement()
    {
       // DebugManagerScript.Instance.AddMessage("apply throttle:" + CurrentThrottle.ToString());
       
        if (CurrentThrottle == 0.0f)
        {
           // DebugManagerScript.Instance.AddMessage("bailing out of mech movement");
            return; 
       }

        //Vector3 direction = new Vector3(lastPostion.x, 0, lastPostion.y); //total guess
        //Vector3 direction = new Vector3(0, 0, lastPostion.y); //total guess

        //Vector3 headRotation = new Vector3(0, head.transform.eulerAngles.y, 0);

        // Rotate the input direction by the horizontal head rotation
        // direction = Quaternion.Euler(headRotation) * direction;

        // Apply speed and move
        /*  Vector3 movement;

          movement = direction * CurrentThrottle;
          DebugManagerScript.Instance.AddMessage("movement:" + movement.ToString());
          transform.position += (Vector3.ProjectOnPlane(Time.deltaTime * movement, Vector3.up));

          transform.position = new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;*/

        /*********************RIGID BODY SCHEME*****************/
        /* DebugManagerScript.Instance.AddMessage("Calculate Move velocity:");
         if(rb == null)
         {
             DebugManagerScript.Instance.AddMessage("Rigid body is Null!");
         }
         else
         {
             rb.velocity = transform.forward * CurrentThrottle;
             DebugManagerScript.Instance.AddMessage("Move velocity:" + rb.velocity.ToString());
         }*/
        /****************************************************************************************/

        //transform.Translate
        Vector3 movementVector = Vector3.forward * (Time.deltaTime * CurrentThrottle);
        //DebugManagerScript.Instance.AddMessage("MOVEMENT vector" + movementVector.ToString());
        transform.Translate(movementVector);
    }


    private void MoveVertical(Vector2 positionVector)
    {
        // Apply the touch position to the head's forward Vector
        Vector3 direction = new Vector3(positionVector.x, 0, positionVector.y);
        Vector3 headRotation = new Vector3(0, head.transform.eulerAngles.y, 0);

        // Rotate the input direction by the horizontal head rotation
        direction = Quaternion.Euler(headRotation) * direction;

        // Apply speed and move
        Vector3 movement = direction * Speed;

        Vector3 newPosition = transform.position;

       // DebugManagerScript.Instance.AddMessage("Vertical move position:" + positionVector.ToString());
        

        if(positionVector.y < 0)
        {
            newPosition.y -= (positionVector.magnitude / 4);
        }
        else
        {
            newPosition.y += (positionVector.magnitude / 4);
        }

        transform.position = newPosition;
    }

    private void Rotate(RotationDirection direction)
    {
        // Apply the touch position to the head's forward Vector
        /*Vector3 direction = new Vector3(rotationVector.x, 0, rotationVector.y);
        Vector3 headRotation = new Vector3(0, head.transform.eulerAngles.y, 0);

        // Rotate the input direction by the horizontal head rotation
        direction = Quaternion.Euler(headRotation) * direction;*/

        //apply rotation?
        if(direction == RotationDirection.LEFT)
        {
            transform.Rotate(0, 1, 0);
        }
        else
        {
            transform.Rotate(0, -1, 0);
        }
        

    }
}
