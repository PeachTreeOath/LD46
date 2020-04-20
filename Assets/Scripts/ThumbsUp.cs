using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThumbsUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroySelf", 3);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, 100 * Time.deltaTime);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
