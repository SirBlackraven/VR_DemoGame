﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float rotate = 25f * Time.deltaTime;
        gameObject.transform.Rotate(0, 0, rotate);

        if(GameStateManagerScript.Instance.GameActive == false)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        DebugManagerScript.Instance.AddMessage("healthbox collision detected:" + collision.gameObject.tag);
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Untagged" || collision.gameObject.tag == "AITarget")
        {
            Collider col = gameObject.GetComponent<Collider>();
            col.enabled = false;

            GameStateManagerScript.Instance.ApplyDamage(0, -25);
            MechGameAudioManagerScript.Instance.PlayHealed();


            //Destroy(collision.gameObject);
            Destroy(gameObject);
            //GameStateManagerScript.Instance.SeekerKilled();
        }
    }
}
