using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLoader : Singleton<ResourceLoader>
{

    [HideInInspector] public GameObject testLandmarkPrefab;
    [HideInInspector] public GameObject customerPrefab;

    [HideInInspector] public Dictionary<int, LevelData> levelsByLevelNumber = new Dictionary<int, LevelData>();

    protected override void Awake()
    {
        base.Awake();
        LoadResources();
    }

    private void LoadResources()
    {
        testLandmarkPrefab = Resources.Load<GameObject>("Prefabs/testLandmark");
        customerPrefab = Resources.Load<GameObject>("Prefabs/customer");

        LevelData[] tempLevels = Resources.LoadAll<LevelData>("ScriptableObjects/Levels");
        foreach (LevelData level in tempLevels)
            levelsByLevelNumber[level.levelNumber] = level;
    }

    public LevelData GetLevel(int levelNum)
    {
        return levelsByLevelNumber[levelNum];
    }
}