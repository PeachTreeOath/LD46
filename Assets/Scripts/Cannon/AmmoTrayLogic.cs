using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoTrayLogic : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 20f;
    [SerializeField] private Transform targetTransform;

    private bool isCoroutineRunning;
    private Quaternion targetRotation;
    private Coroutine rotateCoroutine;
    private bool hasRotatedYet;

    //private bool isRotatingRight = false;
    //private bool isRotatingLeft = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (isRotatingRight)
        {
            RotateRight();
        }

        if (isRotatingLeft)
        {
            RotateLeft();
        }
        */
    }
    /*
    private void RotateRight()
    {
        transform.RotateAround(this.transform.position, this.transform.forward, rotateSpeed * Time.deltaTime);
    }

    private void RotateLeft()
    {
        Quaternion newEulerAngle = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + 36);
        StartCoroutine(RotateToDestination(newEulerAngle));
        //transform.RotateAround(this.transform.position, -this.transform.forward, rotateSpeed * Time.deltaTime);
    }

    public IEnumerator TurnOnOffRotateRight()
    {
        isRotatingRight = true;
        yield return new WaitForSeconds(1f);
        isRotatingRight = false;
    }

    public IEnumerator TurnOnOffRotateLeft()
    {
        isRotatingLeft = true;
        yield return new WaitForSeconds(1f);
        isRotatingLeft = false;
    }
    */

    public void TurnLeft()
    {
        // Skip to end of rotation
        if (isCoroutineRunning)
        {
            StopCoroutine(rotateCoroutine);
        }

        if (!hasRotatedYet)
            targetRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z + 36);
        else
            targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z + 36);

        hasRotatedYet = true;
        rotateCoroutine = StartCoroutine(RotateToDestination());
    }

    public void TurnRight()
    {
        // Skip to end of rotation
        if (isCoroutineRunning)
        {
            StopCoroutine(rotateCoroutine);
        }

        if (!hasRotatedYet)
            targetRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z - 36);
        else
            targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z - 36);

        hasRotatedYet = true;
        rotateCoroutine = StartCoroutine(RotateToDestination());
    }

    private IEnumerator RotateToDestination()
    {
        float timeElapsed = 0;
        Quaternion oldRotation = transform.localRotation;
        isCoroutineRunning = true;

        while (isCoroutineRunning)
        {
            timeElapsed += Time.deltaTime;

            transform.localRotation = Quaternion.Lerp(oldRotation, targetRotation, timeElapsed * 4);

            if (timeElapsed >= .25f)
                isCoroutineRunning = false;

            yield return null;
        }
    }
}
