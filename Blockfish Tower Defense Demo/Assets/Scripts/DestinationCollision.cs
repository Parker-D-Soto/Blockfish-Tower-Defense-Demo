using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Detects when enemy collides with Destination
public class DestinationCollision : MonoBehaviour
{
    //Collision Detection
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision Detected");
        if(collision.gameObject.tag.CompareTo("enemy") == 1)
        {
            Destroy(collision.gameObject);
        }
    }
}
