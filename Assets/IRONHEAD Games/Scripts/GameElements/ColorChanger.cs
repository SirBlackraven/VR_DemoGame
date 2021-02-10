﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ColorChanger : MonoBehaviour
{
    public Material selectMaterial = null;

    private MeshRenderer meshRenderer = null;
    public XRBaseInteractable interactable = null;
    private Material originalMaterial = null;

    public void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalMaterial = meshRenderer.material;

        interactable = GetComponent<XRBaseInteractable>();
        interactable.onHoverEntered.AddListener(SetSelectMaterial);
        interactable.onHoverExited.AddListener(SetOriginalMaterial);

    }

    public void OnDestroy()
    {
       // UnityEngine.Events.UnityAction<XRBaseInteractor>
       interactable.onHoverEntered.RemoveListener(SetSelectMaterial);
       interactable.onHoverExited.RemoveListener(SetOriginalMaterial);
    }

    private void SetSelectMaterial(XRBaseInteractor interactor)
    { 
        meshRenderer.material = selectMaterial;
    }

    private void SetOriginalMaterial(XRBaseInteractor interactor)
    {
        meshRenderer.material = originalMaterial;
    }

}
