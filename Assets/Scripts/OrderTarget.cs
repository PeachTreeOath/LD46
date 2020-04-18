using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderTarget : MonoBehaviour
{
    // Inspector set
    public GameObject canvas; // For turning on and off requirement images
    public Image foodIcon;
    public Rigidbody rigidBody;

    [HideInInspector] public FoodType foodRequirement;
    [HideInInspector] public bool isFed;

    private CustomerController customerParent; // The vehicle that actually hold the order

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(CustomerController parent, Bullet food)
    {
        customerParent = parent;
        foodRequirement = food.foodType;
        foodIcon.sprite = food.requirementIcon;
    }

    // This is done to prevent physics issues with the targets locking things into place before exploding
    public void ReleaseTarget()
    {
        rigidBody.isKinematic = false;
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Bullet"))
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            if (bullet.foodType == foodRequirement)
                FeedTarget();
            else
                customerParent.DestroyVehicle(collision.GetContact(0));

            bullet.Despawn();
        }
    }

    private void FeedTarget()
    {
        isFed = true;
        customerParent.ReportFeeding(this);
    }
}
