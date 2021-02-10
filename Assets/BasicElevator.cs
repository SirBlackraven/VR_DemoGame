using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicElevator : MonoBehaviour
{

    public GameObject movePlatform;

    [SerializeField]
    public float Speed = 1;


    private void OnTriggerStay()
    {
        movePlatform.transform.position += movePlatform.transform.up * Time.deltaTime * Speed;
    }
}
