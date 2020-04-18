using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLoader : Singleton<ResourceLoader>
{

    [HideInInspector] public GameObject testLandmarkPrefab;
    [HideInInspector] public GameObject customerPrefab;

    protected override void Awake()
    {
        base.Awake();
        LoadResources();
    }

    private void LoadResources()
    {
        testLandmarkPrefab = Resources.Load<GameObject>("Prefabs/testLandmark");
        customerPrefab = Resources.Load<GameObject>("Prefabs/customer");
    }
}