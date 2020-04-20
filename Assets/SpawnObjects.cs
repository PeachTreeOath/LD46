using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    public float locOffset;
    public float scaleOffset;
    public List<GameObject> thingsToSpawn = new List<GameObject>();
    public float cd;
    private float t;
    private int alive;
    public int maxSpawn;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(maxSpawn > alive)
        {
            if (t > cd)
            {
                var objectToSpawn = thingsToSpawn[Random.Range(0, thingsToSpawn.Count)];
                GameObject g = Instantiate(objectToSpawn, new Vector3(transform.position.x + Random.Range(-locOffset, locOffset), transform.position.y , transform.position.z + Random.Range(-locOffset, locOffset)), Quaternion.identity);
                g.transform.localScale *= Random.Range(-scaleOffset, scaleOffset);
                alive++;
                t = 0;
            }
            else
            {
                t += Time.deltaTime;
            }
        }
    }


   public void death()
    {
        alive--;
    }
}
