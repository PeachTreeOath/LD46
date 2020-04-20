using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    // Start is called before the first frame update
    public float speedMovement;
    public Vector3 startScale, endScale, speedScale;
    public bool dontScale;
    public bool invert;
    void Start()
    {
        transform.localScale = startScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(!dontScale)
        {
            if (transform.localScale.x < endScale.x)
            {
                transform.localScale += speedScale * Time.deltaTime;
            }
        }
        
        if(!invert)
        {
            transform.Translate(-Vector3.forward * speedMovement * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.forward * speedMovement * Time.deltaTime);
        }
        
    }
}
