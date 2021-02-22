using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;

public class NetworkPlayerSetup : MonoBehaviourPunCallbacks
{
    public GameObject LocalXRRRigGameObject;

    public GameObject AvatarHeadGameObject;
    public GameObject AvatarBodyGameObject;

    // Start is called before the first frame update
    void Start()
    {
        if(photonView.IsMine)
        {
            //this is the local player
            LocalXRRRigGameObject.SetActive(true);
            gameObject.GetComponent<MovementController>().enabled = true;
            gameObject.GetComponent<AvatarInputConverter>().enabled = true;

            SetLayerRecursively(AvatarHeadGameObject, 11);
            SetLayerRecursively(AvatarBodyGameObject, 12);

            TeleportationArea[] teleportationAreas = GameObject.FindObjectsOfType<TeleportationArea>();

            if(teleportationAreas.Length > 0)
            {
                Debug.Log("Found teleportation areas");
                foreach(var item in teleportationAreas)
                {
                    item.teleportationProvider = LocalXRRRigGameObject.GetComponent<TeleportationProvider>();
                }
            }
        }
        else
        {
            //remote player
            LocalXRRRigGameObject.SetActive(false);
            gameObject.GetComponent<MovementController>().enabled = false;
            gameObject.GetComponent<AvatarInputConverter>().enabled = false;

            SetLayerRecursively(AvatarHeadGameObject, 0);
            SetLayerRecursively(AvatarBodyGameObject, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetLayerRecursively(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }
}
