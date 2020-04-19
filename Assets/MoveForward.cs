using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    // Start is called before the first frame update
    public float speedMovement;
    public Vector3 startScale, endScale, speedScale;
    void Start()
    {
        transform.localScale = startScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localScale.x < endScale.x)
        {
            transform.localScale += speedScale * Time.deltaTime;
        }

        transform.Translate(Vector3.forward * speedMovement * Time.deltaTime);
    }
}
