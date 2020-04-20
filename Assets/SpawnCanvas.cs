using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCanvas : MonoBehaviour
{
    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateCanvas(int levelNumber)
    {
       var t = "LEVEL\n" + levelNumber.ToString();
       GameObject g = Instantiate(canvas, transform.position + new Vector3(0f,5f,0f), Quaternion.identity);
       g.GetComponent<SetTextTo>().SetTextToString(t);
    }
}
