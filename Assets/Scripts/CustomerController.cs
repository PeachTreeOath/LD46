﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    // Inspector set
    public Rigidbody rigidBody;
    public float speedMod;
    public List<TargetPairController> targetPairs = new List<TargetPairController>();
    public bool isAerial;
    public BoxCollider boundery;
    public float bounceForce;

    public ParticleSystem explosion;
    public AudioSource audioSource;

    [HideInInspector] public float timeAlive; // This is used to help with crowd control

    private Vector3 targetPosition;
    private bool isDead;
    private float timeDead;
    public GameObject thumbsUp;


    private void Awake()
    {
    }


    private void Start()
    {
        audioSource.outputAudioMixerGroup = AudioManager.instance.sfxMixerGroup;

    }

    private void Update()
    {
        timeAlive += Time.deltaTime;

        // Prevent dead guys from hanging around if stuck
        if (isDead)
        {
            timeDead += Time.deltaTime;
            if (timeDead > 5)
                Destroy(gameObject);
        }

        //if (Input.GetKeyDown(KeyCode.J))
        //{
        //    PlayRandomCrash();
        //}
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            float speed = GetDistanceToPlayer();
            Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, speed * speedMod * Time.deltaTime);
            rigidBody.MovePosition(newPosition);
        }
        else
        {
            // Remove car when out of play
            if (transform.position.z > TrackManager.instance.cutoffPoint / 2)
            {
                Destroy(gameObject);
                // TODO: Possibly play an additional explosion here 
            }
        }
    }

    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }

    public void ReportFeeding()
    {
        PlayRandomEat();
        if (IsOrderFulfilled())
        {
            FinishOrder();
            //TODO: Play success sounds?
        }
    }

    public void PlayExplosion()
    {
        explosion.Play();
        foreach (ParticleSystem p in explosion.gameObject.GetComponentsInChildren<ParticleSystem>())
        {
            p.Play();
        }
        PlayRandomExplosion();
    }

    public void DestroyVehicle(Vector3 explosionPoint, bool isGameOver)
    {
        foreach (TargetPairController target in targetPairs)
        {
            target.ReleaseTarget();
        }

        rigidBody.constraints = RigidbodyConstraints.None;
        rigidBody.isKinematic = false;
        rigidBody.useGravity = true;
        rigidBody.AddExplosionForce(10, explosionPoint, 10, 5, ForceMode.Impulse);

        isDead = true;
        if(!isGameOver)
            GameManager.instance.RemoveCustomerFromList(this);
    }

    public void FinishOrder()
    {
        GameManager.instance.OrderFilled(this);
        // Create thumbs up
        GameObject g = Instantiate(thumbsUp, transform.position + new Vector3(0, 2.3f, 0), Quaternion.identity);
        g.GetComponentInChildren<ParticleSystem>().Play();
        DestroyVehicle(transform.position, false);
    }

    // Social distancing algorithm - doesn't work very well
    public void MoveAwayFromPoint(Vector3 point)
    {
        Vector3 oppositeDirection = transform.position - point;
        rigidBody.AddForce(oppositeDirection * Time.deltaTime * 10f);
    }

    public void AssignFoodRequirement(Bullet food)
    {
        int count = targetPairs.Where(o => o.isAssigned == true).Count();

        if (count == targetPairs.Count) // Check if all pairs are used up
        {
            // Don't assign anything if so
            return;
        }
        else
        {
            // Choose a random, unassigned window to assign to
            while (true)
            {
                int idx = UnityEngine.Random.Range(0, targetPairs.Count);

                if (!targetPairs[idx].isAssigned)
                {
                    targetPairs[idx].Init(this, food);
                    break;
                }
            }
        }
    }

    public void PlaySound(string name)
    {
        AudioClip clip = AudioManager.instance.soundMap[name];
        audioSource.PlayOneShot(clip);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Bullet"))
        {
            // currently in the order target logic but this could change
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            /*
            if (bullet.foodType == foodRequirement)
                FinishOrder();
            else
                DestroyVehicle(collision.GetContact(0));
            */
            PlayRandomScared();
            bullet.Despawn();
        }
        else if (isDead && collision.gameObject.tag.Equals("Ground"))
        {
            PlayRandomCrash();
        }
    }

    // Careful, this code is duplicated
    private int lastCrashPlayed = -1;
    public void PlayRandomCrash()
    {
        int roll = UnityEngine.Random.Range(0, 3);

        while (roll == lastCrashPlayed)
        {
            roll = UnityEngine.Random.Range(0, 3);
        }

        if (roll == 0)
            PlaySound("Just_Car_Crash");
        if (roll == 1)
            PlaySound("Just_Car_Crash-001");
        if (roll == 2)
            PlaySound("Just_Car_Crash-002");

        lastCrashPlayed = roll;
    }

    private int lastExplosionPlayed = -1;
    public void PlayRandomExplosion()
    {
        int roll = UnityEngine.Random.Range(0, 5);

        while (roll == lastExplosionPlayed)
        {
            roll = UnityEngine.Random.Range(0, 5);
        }

        if (roll == 0)
            PlaySound("General_Explosion");
        if (roll == 1)
            PlaySound("General_Explosion-001");
        if (roll == 2)
            PlaySound("General_Explosion-002");
        if (roll == 3)
            PlaySound("General_Explosion-003");
        if (roll == 4)
            PlaySound("General_Explosion-004");

        lastExplosionPlayed = roll;
    }

    private int lastEatPlayed = -1;
    public void PlayRandomEat()
    {
        int roll = UnityEngine.Random.Range(0, 3);

        while (roll == lastEatPlayed)
        {
            roll = UnityEngine.Random.Range(0, 3);
        }

        if (roll == 0)
            PlaySound("Customer_Eats_Food");
        if (roll == 1)
            PlaySound("Customer_Eats_Food-001");
        if (roll == 2)
            PlaySound("Customer_Eats_Food-002");

        lastEatPlayed = roll;
    }

    private int lastScaredPlayed = -1;
    public void PlayRandomScared()
    {
        int roll = UnityEngine.Random.Range(0, 3);

        while (roll == lastScaredPlayed)
        {
            roll = UnityEngine.Random.Range(0, 3);
        }

        if (roll == 0)
            PlaySound("Just_Customer_Scared_VO");
        if (roll == 1)
            PlaySound("Just_Customer_Scared_VO-001");
        if (roll == 2)
            PlaySound("Just_Customer_Scared_VO-002");

        lastScaredPlayed = roll;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (isDead && collision.gameObject.tag.Equals("Ground"))
        {
            rigidBody.AddForce(new Vector3(0, 0, -1000 * Time.deltaTime));
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

    private bool IsOrderFulfilled()
    {
        foreach (TargetPairController pair in targetPairs)
        {
            if (pair.isAssigned && !pair.isFed)
                return false;
        }

        return true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isDead && other.gameObject.tag.Equals("Customer"))
        {
            var dist = other.transform.position - this.transform.position;
            rigidBody.AddForce(-dist * bounceForce * Time.deltaTime);
        }
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
