using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    // Inspector set
    public List<GameObject> trackSegments;
    public float trackSegmentLength;

    private float totalTrackLength;
    private float cutoffPoint;
    private Vector3 resetDelta;

    private void Awake()
    {
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
