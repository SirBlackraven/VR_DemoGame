using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    GameObject GenericVRPlayerPrefab;

    public Vector3 spawnPosition;
    

    // Start is called before the first frame update
    void Start()
    {
        DebugManagerScript.Instance.AddMessage("Attempting to spawn player prefab...");
        /*if(PhotonNetwork.IsConnectedAndReady)
        {
           PhotonNetwork.Instantiate(GenericVRPlayerPrefab.name, spawnPosition, Quaternion.identity);
        }
        DebugManagerScript.Instance.AddMessage("...task complete");*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
