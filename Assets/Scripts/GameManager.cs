using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Inspector set
    public float landmarkSpawnTime;

    private float landmarkSpawnTimeElapsed;
    private List<GameObject> stationaryObjects = new List<GameObject>();

    private const float moveSpeed = 5; // This moves all stationary objects. The cars themselves are still.

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Move all stationary objects
        foreach (GameObject obj in stationaryObjects)
        {
            obj.transform.position += new Vector3(0, 0, moveSpeed * Time.deltaTime);
        }

        // Spawn logic
        landmarkSpawnTimeElapsed += Time.deltaTime;

        if (landmarkSpawnTimeElapsed > landmarkSpawnTime)
        {
            landmarkSpawnTimeElapsed -= landmarkSpawnTime;

            SpawnLandmark();
        }
    }

    private void SpawnLandmark()
    {
        GameObject landmarkObj = Instantiate(ResourceLoader.instance.testLandmarkPrefab);
        landmarkObj.transform.position = new Vector3(UnityEngine.Random.Range(-10f, 10f), 0.5f, -50);
        stationaryObjects.Add(landmarkObj);
    }
}
