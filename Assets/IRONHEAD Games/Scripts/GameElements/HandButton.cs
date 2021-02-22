using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Script to activate and move a button in VR
/// </summary>
public class HandButton : XRBaseInteractable
{
    public UnityEvent OnPress = null;

    private float previousHandHeight = 0.0f;
    private XRBaseInteractor hoverInteractor = null;

    //locator vars
    private float yMin = 0.0f;
    private float yMax = 0.0f;
    private bool previousPress = false;

    protected override void Awake()
    {
        base.Awake();

        //add event listeners
        onHoverEntered.AddListener(StartPress);
        onHoverExited.AddListener(EndPress);
    }


    private void OnDestroy()
    {
        onHoverEntered.RemoveListener(StartPress);
        onHoverExited.RemoveListener(EndPress);
    }

    // Start is called before the first frame update
    void Start()
    {
        SetMinMax();
    }

    //Start button movement
    private void StartPress(XRBaseInteractor interactor)
    {
        hoverInteractor = interactor;
        previousHandHeight = GetLocalYPosition(hoverInteractor.transform.position);
    }

    //reset button position
    private void EndPress(XRBaseInteractor interactor)
    {
        hoverInteractor = null;
        previousHandHeight = 0.0f;

        previousPress = false;
        SetYPosition(yMax);
    }

    //set the button movement limits
    private void SetMinMax()
    {
        Collider collider = GetComponent<Collider>();
        yMin = transform.localPosition.y - (collider.bounds.size.y * 0.5f); //Use the size of the object laid out in the GUI to determine motion limit
        yMax = transform.localPosition.y;
    }

    //depress button based on VR hand interaction
    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if(hoverInteractor)
        {
            float newHandHeight = GetLocalYPosition(hoverInteractor.transform.position);
            float handDifference = previousHandHeight - newHandHeight;
            previousHandHeight = newHandHeight;

            float newPosition = transform.localPosition.y - handDifference;

            SetYPosition(newPosition);

            CheckPress();
        }
    }

    /// <summary>
    /// utility function to get local Y
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private float GetLocalYPosition(Vector3 position)
    {
        Vector3 localPosition = transform.root.InverseTransformPoint(position);
        return localPosition.y;
    }

    private void SetYPosition(float position)
    {
        Vector3 newPosition = transform.localPosition;
        newPosition.y = Mathf.Clamp(position, yMin, yMax);
        transform.localPosition = newPosition;
    }

    /// <summary>
    /// Press event controller
    /// </summary>
    private void CheckPress()
    {        
        bool inPosition = InPosition();

        if(inPosition && inPosition != previousPress)
        {
            OnPress.Invoke();
        }

        previousPress = inPosition;
    }

    //check for acceptable closeness
    private bool InPosition()
    {
        float inRange = Mathf.Clamp(transform.localPosition.y, yMin, yMin + 0.01f);  
        return transform.localPosition.y == inRange;
    }
}
