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
    public SpawnCanvas levelTextSpawnPosition;
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
        if (startingLevel <= 0)
        {
            startingLevel = 1;
        };
        currentLevel = startingLevel;
        SyncNewLevelData(currentLevel);

        levelTextSpawnPosition.CreateCanvas(currentLevel);
    }

    private void SyncNewLevelData(int nextLevel)
    {
        level = ResourceLoader.instance.GetLevel(nextLevel);

        spawnCD = level.spawnRateInSeconds;
        maxCarOnScreen = level.maxCarsOnScreen;
        nOrdersToFill = level.nOrdersToFill;
        possibleCars = level.possibleCars;
        Debug.Log("NEW LEVEL DATA SYNC'D: " + currentLevel.ToString());
        CannonShoot.instance.InitAmmo(level.possibleFoods);
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnCD < t)
        {
            if (nOrdersToFill > filledOrders)
            {
                if (maxCarOnScreen > aliveOrders)
                {
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

    public void OrderFilled()
    {
        Debug.Log("Order Filled : " + filledOrders.ToString() + " : " + nOrdersToFill.ToString() );
        aliveOrders--;
        filledOrders++;

        if (filledOrders >= nOrdersToFill)
        {
            GotoNextLevel();
        }
    }

    public void GotoNextLevel()
    {
        currentLevel ++;
        levelTextSpawnPosition.CreateCanvas(currentLevel);
        Debug.Log("level beaten! going to next level");
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
        GameObject customerObj = Instantiate(newCustomer);

        // Spawn from back or front
        int roll = Random.Range(0, 2);
        bool isBackSpawn = roll == 0 ? true : false;

        customerObj.transform.position = new Vector3(UnityEngine.Random.Range(-10f, 10f), 0.5f, -200);
        CustomerController customer = customerObj.GetComponent<CustomerController>();
        List<GameObject> possibleFoods = ResourceLoader.instance.GetLevel(currentLevel).possibleFoods;
        Bullet randomFood = possibleFoods[UnityEngine.Random.Range(0, possibleFoods.Count)].GetComponent<Bullet>();
        customer.AssignFoodRequirement(randomFood);

        // TODO: Spawn these in intelligent quadrants
        Vector3 targetPosition = new Vector3(UnityEngine.Random.Range(-maxDistance, maxDistance), 0, UnityEngine.Random.Range(-maxDistance, maxDistance));
        customer.SetTargetPosition(targetPosition);
        //Get it's Customer Component and add it to the list.
        customers.Add(customer);
        Debug.Log("NEW CUSTOMER SPAWNED");
    }
}
