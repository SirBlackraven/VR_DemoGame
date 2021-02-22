using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Events;

public class InputListener : MonoBehaviour
{
    List<InputDevice> inputDevices;
    InputDeviceCharacteristics deviceCharacteristics;
    public XRNode controllerNode;

    [Tooltip("Event when the button starts being pressed")]
    public UnityEvent OnPress;

    [Tooltip("Event when the button starts being released")]
    public UnityEvent OnRelease;

    private bool isPressed = false;

    private void Awake()
    {
        inputDevices = new List<InputDevice>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        deviceCharacteristics = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left;

        InputDevices.GetDevicesAtXRNode(controllerNode, inputDevices);

        foreach(InputDevice device in inputDevices)
        {
            //DebugManagerScript.Instance.AddMessage("Device found with name: " + inputDevice.name);

            bool inputValue;
            /*if(inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out inputValue) && inputValue)
            {
                DebugManagerScript.Instance.AddMessage("Prime Left button pressed");
            }*/

            if (device.TryGetFeatureValue(CommonUsages.triggerButton, out inputValue) && inputValue)
            {
                if (!isPressed)
                {
                    isPressed = true;
                   // DebugManagerScript.Instance.AddMessage("OnPress event is called on left control, trigger");

                    OnPress.Invoke();
                }

            }
            else if (isPressed)
            {
                isPressed = false;
                OnRelease.Invoke();
                //DebugManagerScript.Instance.AddMessage ("OnRelease event is called");

            }

           /* if (device.isValid)
            {
                if (device.TryGetFeatureValue(CommonUsages.gripButton, out inputValue) && inputValue)
                {
                    if (!isPressed)
                    {
                        isPressed = true;
                        DebugManagerScript.Instance.AddMessage("Left Grip OnPress event is called");

                        OnPress.Invoke();
                    }

                }
                else if (isPressed)
                {
                    isPressed = false;
                    OnRelease.Invoke();
                    DebugManagerScript.Instance.AddMessage("Left Grip OnRelease event is called");

                }
            }*/
        }
    }
}
