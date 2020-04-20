using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public bool self;


    // Update is called once per frame
    void Update()
    {
        if(!self)
        {
            transform.RotateAround(player.transform.position, Vector3.left, speed * Time.deltaTime);
        }
        else
        {
            transform.Rotate( Vector3.up* speed * Time.deltaTime);
        }
        
    }
}
