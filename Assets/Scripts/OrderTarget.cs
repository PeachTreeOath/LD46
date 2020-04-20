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

    private CustomerController customerParent; // The vehicle that actually holds the order
    private TargetPairController targetPairParent; // Pair that tracks both left and right orders

    public void Init(TargetPairController targetPairController, CustomerController parent, Bullet food)
    {
        customerParent = parent;
        targetPairParent = targetPairController;
        foodRequirement = food.foodType;
        foodIcon.sprite = food.requirementIcon;
        foodIcon.rectTransform.sizeDelta = new Vector2(food.requirementIcon.bounds.size.x * 100, food.requirementIcon.bounds.size.y * 100);
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
            {
                FeedTarget();
            }   
            else
            {
                customerParent.DestroyVehicle(collision.GetContact(0).point, false);
                customerParent.PlayExplosion();
            }
                

            bullet.Despawn();
        }
    }

    private void FeedTarget()
    {
        targetPairParent.CompleteTarget();
        customerParent.ReportFeeding();
    }
}
