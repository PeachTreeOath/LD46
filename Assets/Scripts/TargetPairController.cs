using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Every target needs to be in a pair because you dont know if you'll have access to the left or right side
public class TargetPairController : MonoBehaviour
{
    // Inspector set
    public OrderTarget leftTarget;
    public OrderTarget rightTarget;

    [HideInInspector] public FoodType foodRequirement;
    [HideInInspector] public bool isFed;
    [HideInInspector] public bool isDead;

    // Update is called once per frame
    void Update()
    {
        if (!isFed && !isDead)
        {
            if (transform.position.x > 0)
            {
                leftTarget.gameObject.SetActive(true);
                rightTarget.gameObject.SetActive(false);
            }
            else
            {
                leftTarget.gameObject.SetActive(false);
                rightTarget.gameObject.SetActive(true);
            }
        }
    }

    public void Init(CustomerController parent, Bullet food)
    {
        leftTarget.Init(this, parent, food);
        rightTarget.Init(this, parent, food);
    }

    // This is done to prevent physics issues with the targets locking things into place before exploding
    public void ReleaseTarget()
    {
        isDead = true;
        leftTarget.ReleaseTarget();
        rightTarget.ReleaseTarget();
    }

    public void CompleteTarget()
    {
        isFed = true;
        leftTarget.gameObject.SetActive(false);
        rightTarget.gameObject.SetActive(false);
    }
}
