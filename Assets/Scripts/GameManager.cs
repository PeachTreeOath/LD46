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

    public int startingLevel = 1;
    private int currentLevel;

    private float t;
    private LevelData level;

    private float spawnCD;
    private int maxCarOnScreen;
    private int nOrdersToFill;
    private List<GameObject> possibleCars;
  
  

    //public float startingFuelAmount;s
    //public float fuelDecay;


    [HideInInspector] public List<CustomerController> customers = new List<CustomerController>();

    private float landmarkSpawnTimeElapsed;
    private List<GameObject> stationaryObjects = new List<GameObject>();
    private int aliveOrders;
    private int filledOrders;

    private const float maxDistance = 5f;

    // Start is called before the first frame update
    void Start()
    {
        t = 0;
        if(startingLevel <= 0) 
        {
            startingLevel = 1; 
        };
        currentLevel = startingLevel;
        SyncNewLevelData(currentLevel);
    }

    private void SyncNewLevelData(int nextLevel)
    {
        level = ResourceLoader.instance.GetLevel(nextLevel);

        spawnCD = level.spawnRateInSeconds;
        maxCarOnScreen = level.maxCarsOnScreen;
        nOrdersToFill = level.nOrdersToFill;
        possibleCars = level.possibleCars;
        Debug.Log("NEW LEVEL DATA SYNC'D");
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnCD < t)
        {
            Debug.Log("CAN SPAWN");
            if (nOrdersToFill > filledOrders)
            {
                Debug.Log("CAN ADD NEW ORDER");
                if (maxCarOnScreen > aliveOrders)
                {
                    Debug.Log("SPAWNING PREFAB");
                    var prefabToSpawn = possibleCars[Random.Range(0, possibleCars.Count)];
                    SpawnCustomer(prefabToSpawn);
                    t = 0;
                }
            }
            
        }
        else
        {
            t += Time.deltaTime;
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

    private void SpawnCustomer(GameObject newCustomer)
    {
        //List<GameObject> possibleCars = ResourceLoader.instance.GetLevel(1).possibleCars;
        //GameObject carPrefab = possibleCars[UnityEngine.Random.Range(0, possibleCars.Count)];


        if (possibleCars.Count == 0) Debug.LogError("You forgot to assign cars to the level scriptable object");
        GameObject customerObj = Instantiate(newCustomer);
        customerObj.transform.position = new Vector3(UnityEngine.Random.Range(-10f, 10f), 0.5f, -200);
        CustomerController customer = customerObj.GetComponent<CustomerController>();
        // TODO: Spawn these in intelligent quadrants
        Vector3 targetPosition = new Vector3(UnityEngine.Random.Range(-maxDistance, maxDistance), 0, UnityEngine.Random.Range(-maxDistance, maxDistance));
        customer.SetTargetPosition(targetPosition);
        //Get it's Customer Component and add it to the list.
        customers.Add(customer);
        Debug.Log("NEW CUSTOMER SPAWNED");
    }
}
