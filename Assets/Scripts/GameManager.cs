using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    // Inspector set
    public float moveSpeed = 5f; // This moves all stationary objects. The cars themselves are still.
    public float landmarkSpawnTime;

    public int startingLevel = 0;
    private int currentLevel;
    public List<ScriptableObject> vehiclePrefabs;
    public int[] orderCountPerLevel;
    public int[] maxSpawnPerLevel;
    public float[] timeBetweenSpawnPerLevel;
    public Dictionary<int,int[]> vehiclesThatCanSpawnPerLevel = new Dictionary<int, int[]>();
    private float t;


    //public float startingFuelAmount;s
    //public float fuelDecay;


    [HideInInspector] public List<CustomerController> customers = new List<CustomerController>();

    private float landmarkSpawnTimeElapsed;
    private List<GameObject> stationaryObjects = new List<GameObject>();
    private int aliveOrders;

    private const float maxDistance = 5f;

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = startingLevel;
        vehiclesThatCanSpawnPerLevel.Add(0, new int[] { 1 });
        vehiclesThatCanSpawnPerLevel.Add(1, new int[] { 1, 2 });
        vehiclesThatCanSpawnPerLevel.Add(2, new int[] { 1, 2, 3 });
        vehiclesThatCanSpawnPerLevel.Add(3, new int[] { 1, 2, 3 });
        vehiclesThatCanSpawnPerLevel.Add(4, new int[] { 1, 2, 3, 4 });
        vehiclesThatCanSpawnPerLevel.Add(5, new int[] { 1, 2, 3, 4, 5 });
    }

    // Update is called once per frame
    void Update()
    {
        if(vehiclePrefabs.Count !=0)
        {
            // Spawn logic
            if (timeBetweenSpawnPerLevel[currentLevel] > t)
            {
                if (maxSpawnPerLevel[currentLevel] < aliveOrders)
                {
                    //We can spawn something for the level.
                    var tIntArray = vehiclesThatCanSpawnPerLevel[currentLevel];
                    var r = Random.Range(0, tIntArray.Length);
                    var prefabToSpawn = vehiclePrefabs[r];
                    SpawnCustomer(prefabToSpawn);
                    aliveOrders++;
                    t = 0f;
                }
                else
                {
                    t += Time.deltaTime;
                }
            }
        
        }
        else
        {
            GameObject customerObj = Instantiate(ResourceLoader.instance.customerPrefab);
        }



        /*// Move all stationary objects
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
            SpawnCustomer(); // TODO: Separate these out
        }
        */
    }

    public void orderFilled()
    {
        aliveOrders--;
    }

    private void SpawnLandmark()
    {
        GameObject landmarkObj = Instantiate(ResourceLoader.instance.testLandmarkPrefab);
        landmarkObj.transform.position = new Vector3(UnityEngine.Random.Range(-10f, 10f), 0.5f, -50);
        //Add it to Stationary Objects.
        stationaryObjects.Add(landmarkObj);
    }

    private void SpawnCustomer(ScriptableObject newCustomer)
    {

        //GameObject customerObj = Instantiate(ResourceLoader.instance.customerPrefab);
        //GameObject customerObj = Instantiate(newCustomer);
        //Put Object At position.

        List<GameObject> possibleCars = ResourceLoader.instance.GetLevel(1).possibleCars;
        if (possibleCars.Count == 0) Debug.LogError("You forgot to assign cars to the level scriptable object");
        GameObject carPrefab = possibleCars[UnityEngine.Random.Range(0, possibleCars.Count)];

        GameObject customerObj = Instantiate(carPrefab);
        customerObj.transform.position = new Vector3(UnityEngine.Random.Range(-10f, 10f), 0.5f, -200);
        CustomerController customer = customerObj.GetComponent<CustomerController>();
        // TODO: Spawn these in intelligent quadrants
        Vector3 targetPosition = new Vector3(UnityEngine.Random.Range(-maxDistance, maxDistance), 0, UnityEngine.Random.Range(-maxDistance, maxDistance));
        customer.SetTargetPosition(targetPosition);
        //Get it's Customer Component and add it to the list.
        customers.Add(customer);
    }
}
