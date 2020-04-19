using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyUnit : MonoBehaviour
{
    public float lifetime;
    private float t = 0;
    void Update()
    {
        if (t > lifetime)
        {
            Destroy(this);
        }
        else
        {
            t += Time.deltaTime;
        }    
    }
}
