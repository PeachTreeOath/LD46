using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public GameObject player;
    public float speed;


    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(player.transform.position, Vector3.left, speed * Time.deltaTime);
    }
}
