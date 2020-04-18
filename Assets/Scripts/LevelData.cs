using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/LevelData")]
public class LevelData : ScriptableObject
{
    public int levelNumber;
    public float spawnRateInSeconds;
    public int maxCarsOnScreen;
    public List<GameObject> possibleCars;
    public List<GameObject> possibleFoods;
}
