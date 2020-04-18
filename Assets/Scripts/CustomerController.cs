using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    // Inspector set
    public Rigidbody rigidBody;
    public float speedMod;

    [HideInInspector] public float timeAlive; // This is used to help with crowd control

    private Vector3 targetPosition;
    private bool isDead;

    private void Update()
    {
        timeAlive += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            float speed = GetDistanceToPlayer();
            Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, speed * speedMod * Time.deltaTime);
            rigidBody.MovePosition(newPosition);
        }
    }

    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }

    public void DestroyVehicle(ContactPoint contactPoint)
    {
        rigidBody.constraints = RigidbodyConstraints.None;
        rigidBody.useGravity = true;
        rigidBody.AddExplosionForce(10, contactPoint.point, 10, 5, ForceMode.Impulse);

        isDead = true;
    }

    // Social distancing algorithm - doesn't work very well
    public void MoveAwayFromPoint(Vector3 point)
    {
        Vector3 oppositeDirection = transform.position - point;
        rigidBody.AddForce(oppositeDirection * Time.deltaTime * 10f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Bullet"))
        {
            DestroyVehicle(collision.GetContact(0));
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (isDead && collision.gameObject.tag.Equals("Ground"))
        {
            rigidBody.AddForce(new Vector3(0, 0, 1000 * Time.deltaTime));
        }
    }

    private float GetDistanceToPlayer()
    {
        return Vector3.Distance(transform.position, PlayerController.instance.transform.position);
    }

    // Currently not used in favor of the simpler alg, get min distance to player
    private float GetMinDistanceToOthers()
    {
        float minDistance = 50f;

        foreach (CustomerController customer in GameManager.instance.customers)
        {
            float distance = Vector3.Distance(transform.position, customer.transform.position);
            if (distance < minDistance)
            {
                distance = minDistance;
            }
        }

        return minDistance;
    }

    /*
    private void OnTriggerStay(Collider other)
    {
        if (!isDead && other.gameObject.tag.Equals("Customer"))
        {
            CustomerController otherCar = other.GetComponent<CustomerController>();

            // Make the newer car push things out of its way so it can get to the right spot
            if (timeAlive < otherCar.timeAlive)
            {
                otherCar.MoveAwayFromPoint(transform.position);
            }
        }
    }
    */
}
