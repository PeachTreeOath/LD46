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
    [HideInInspector] public bool isFed; // Has been shot w correct food
    [HideInInspector] public bool isDead;
    [HideInInspector] public bool isAssigned; // Has be assigned a target

    private void Start()
    {
        leftTarget.gameObject.SetActive(false);
        rightTarget.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isAssigned && !isFed && !isDead)
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
        isAssigned = true;
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
