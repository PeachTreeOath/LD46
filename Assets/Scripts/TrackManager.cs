using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : Singleton<TrackManager>
{
    // Inspector set
    public List<GameObject> trackSegments;
    public float trackSegmentLength;

    [HideInInspector] public float totalTrackLength;
    [HideInInspector] public float cutoffPoint; // The distance when a tile needs to loop back to itself
    private Vector3 resetDelta;

    protected override void Awake()
    {
        base.Awake();

        totalTrackLength = trackSegmentLength * trackSegments.Count;
        cutoffPoint = totalTrackLength / 2f;
        resetDelta = new Vector3(0, 0, totalTrackLength);
    }

    // Update is called once per frame
    void Update()
    {
        // Move all stationary objects
        foreach (GameObject obj in trackSegments)
        {
            obj.transform.position += new Vector3(0, 0, GameManager.instance.moveSpeed * Time.deltaTime);
            if (obj.transform.position.z > cutoffPoint)
            {
                obj.transform.position -= resetDelta;
            }
        }
    }
}
