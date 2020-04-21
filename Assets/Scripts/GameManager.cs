using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;
using System.Linq;

public class GameManager : Singleton<GameManager>
{
    // Inspector set
    public float moveSpeed = 5f; // This moves all stationary objects. The cars themselves are still.
    public float environmentalObjectSpawnTime;
    public SpawnCanvas levelTextSpawnPosition;
    public int startingLevel = 1;
    public float startingFuelAmount;
    private float fuelAmount;
    public float fuelPerOrder;
    public float fuelPerLevelUp;
    public float maxFuel;
    private int currentLevel;
    public List<GameObject> environmentalObjectsToSpawn;
    private float environmentalObjectSpawnTimeElapsed;

    private float t;
    private LevelData level;

    private float spawnCD;
    private int maxCarOnScreen;
    private int nOrdersToFill;
    private List<GameObject> possibleCars;


    public TextMeshProUGUI orderText, levelText;
    public Slider fuelGauge;
    public float fuelSpentPerTick;



    //public float startingFuelAmount;s
    //public float fuelDecay;


    [HideInInspector] public List<CustomerController> customers = new List<CustomerController>();

    private float landmarkSpawnTimeElapsed;
    private List<GameObject> stationaryObjects = new List<GameObject>();
    private int aliveOrders;
    private int filledOrders;

    private const float maxDistance = 6f;
    private const float minDistance = 1.5f;
    private const float minFlyerDistance = 3f;

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
        UpdateOrderText();
        fuelAmount = startingFuelAmount;
    }

    private void UpdateOrderText()
    {
        orderText.text = "ORDERS\n" + filledOrders.ToString() + "/" + nOrdersToFill.ToString();
    }

    private void UpdateLevelText()
    {
        levelText.text = "LEVEL " + currentLevel.ToString();
        filledOrders = 0;
    }

    private void UpdateFuelUI()
    {

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
        fuelAmount -= fuelSpentPerTick * Time.deltaTime;
        //Debug.Log(fuelAmount);

        float tempFuel = Mathf.Min(fuelAmount, 50);
        tempFuel = Mathf.Max(fuelAmount, 25);
        float ratio = fuelAmount - 25;
        ratio /= 25;
        AudioManager.instance.TransitionToEngine2(ratio);

        fuelGauge.value = 1 - fuelAmount / 100;

        if (fuelAmount <= 0)
        {
            currentLevel = 1;
            filledOrders = 0;
            aliveOrders = 0;
            SyncNewLevelData(currentLevel);
            fuelAmount = startingFuelAmount;

            int i = 0; // Only play 2 sounds
            foreach (CustomerController car in customers)
            {
                if (i < 2)
                    car.PlayExplosion();

                car.DestroyVehicle(car.transform.position - new Vector3(0, -3, 0), true);
                i++;
            }
            UpdateOrderText();
            UpdateLevelText();
            customers.Clear();
        }

        if (spawnCD < t)
        {
            if (nOrdersToFill > filledOrders)
            {
                if (maxCarOnScreen > aliveOrders)
                {
                    var prefabToSpawn = possibleCars[Random.Range(0, possibleCars.Count)];
                    if (customers.Count < maxCarOnScreen)
                        SpawnCustomer(prefabToSpawn);
                    t = 0;
                }
            }
        }
        else
        {
            t += Time.deltaTime;
        }

        // Spawn logic
        environmentalObjectSpawnTimeElapsed += Time.deltaTime;

        if (environmentalObjectSpawnTimeElapsed > environmentalObjectSpawnTime)
        {
            environmentalObjectSpawnTimeElapsed -= environmentalObjectSpawnTime;
            SpawnEnvObj();
        }

        // Move all env objs
        List<GameObject> objsToDelete = new List<GameObject>();
        foreach (GameObject obj in stationaryObjects)
        {
            if (obj == null)
                continue;

            obj.transform.position -= new Vector3(0, 0, moveSpeed * Time.deltaTime); // Move all objects
            if (obj.transform.position.z < -TrackManager.instance.cutoffPoint)
            {
                objsToDelete.Add(obj); // Put them into a list for deletion
            }
        }

        stationaryObjects = stationaryObjects.Except(objsToDelete).ToList();

        foreach (GameObject obj in objsToDelete)
            Destroy(obj);
    }

    public void OrderFilled(CustomerController customerController)
    {
        Debug.Log("Order Filled : " + filledOrders.ToString() + " : " + nOrdersToFill.ToString());
        aliveOrders--;
        filledOrders++;
        UpdateOrderText();
        fuelAmount += fuelPerOrder;
        fuelAmount = Mathf.Min(maxFuel, fuelAmount);
        RemoveCustomerFromList(customerController);

        if (filledOrders >= nOrdersToFill)
        {
            GotoNextLevel();
        }
    }

    public void RemoveCustomerFromList(CustomerController customerController)
    {
        customers.Remove(customerController);
    }

    public void GotoNextLevel()
    {
        currentLevel++;
        SyncNewLevelData(currentLevel);
        levelTextSpawnPosition.CreateCanvas(currentLevel);
        Debug.Log("level beaten! going to next level");
        UpdateLevelText();
        fuelAmount += fuelPerLevelUp / currentLevel;
        fuelAmount = Mathf.Min(maxFuel, fuelAmount);
        UpdateFuelUI();
        UpdateOrderText();
    }

    private void SpawnEnvObj()
    {
        GameObject landmarkObj = Instantiate(environmentalObjectsToSpawn[Random.Range(0, environmentalObjectsToSpawn.Count)]);
        landmarkObj.transform.position = new Vector3(Random.Range(-20f, 20f), 0f, 200);
        //Add it to Stationary Objects.
        stationaryObjects.Add(landmarkObj);
    }

    private void SpawnCustomer(GameObject newCustomer)
    {
        GameObject customerObj = Instantiate(newCustomer);
        CustomerController customer = customerObj.GetComponent<CustomerController>();

        // Spawn from back or front
        int roll = Random.Range(0, 2);
        bool isBackSpawn = roll == 0 ? true : false;

        // Spawn initial position
        if (isBackSpawn)
            customerObj.transform.position = new Vector3(Random.Range(-20f, 20f), 0.5f, -200);
        else
            customerObj.transform.position = new Vector3(Random.Range(-20f, 20f), 0.5f, 200);

        // Handle planes
        if (customer.isAerial)
            customerObj.transform.position = new Vector3(customerObj.transform.position.x, Random.Range(3f, 4f), customerObj.transform.position.z);

        // Initialize customer
        List<GameObject> possibleFoods = ResourceLoader.instance.GetLevel(currentLevel).possibleFoods;
        int numFoods = Random.Range(1, 4);
        for (int i = 0; i < numFoods; i++)
        {
            Bullet randomFood = possibleFoods[Random.Range(0, possibleFoods.Count)].GetComponent<Bullet>();
            customer.AssignFoodRequirement(randomFood);
        }

        // Find a horizontal distance. This is doing separately so the car doesnt just sit directly in front of the player
        int hRoll = Random.Range(0, 2);
        bool isLeftSection = roll == 0 ? true : false;
        float xPos;
        if (isLeftSection)
            xPos = Random.Range(-maxDistance, -2);
        else
            xPos = Random.Range(2, maxDistance);

        // Set position for them to move to. Make sure don't get too close or far via min and max distance
        Vector3 targetPosition = Vector3.zero;
        if (isBackSpawn)
            while (targetPosition == Vector3.zero || Vector3.Distance(targetPosition, PlayerController.instance.transform.position) < minDistance)
            {
                float currMinDistance = -minDistance;
                if (customer.isAerial)
                    currMinDistance = Mathf.Min(-minFlyerDistance, currMinDistance);

                targetPosition = new Vector3(xPos, 0, Random.Range(-maxDistance, currMinDistance));
            }
        else
            while (targetPosition == Vector3.zero || Vector3.Distance(targetPosition, PlayerController.instance.transform.position) < minDistance)
            {
                float currMinDistance = minDistance;
                if (customer.isAerial)
                    currMinDistance = Mathf.Max(minFlyerDistance, currMinDistance);

                targetPosition = new Vector3(xPos, 0, Random.Range(currMinDistance, maxDistance));
            }

        // Handle planes
        if (customer.isAerial)
            targetPosition = new Vector3(targetPosition.x, Random.Range(3f, 4f), targetPosition.z);

        customer.SetTargetPosition(targetPosition);

        // Get it's Customer Component and add it to the list.
        customers.Add(customer);
        Debug.Log("NEW CUSTOMER SPAWNED");
    }
}
