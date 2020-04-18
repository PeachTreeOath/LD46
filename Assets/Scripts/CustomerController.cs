using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    // Inspector set
    public Rigidbody rigidBody;
    public float speed;

    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate()
    {
        Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        rigidBody.MovePosition(newPosition);
    }

    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }

    // TODO: Change this to respect physics somewhat
    public void DestroyVehicle()
    {

    }
}
