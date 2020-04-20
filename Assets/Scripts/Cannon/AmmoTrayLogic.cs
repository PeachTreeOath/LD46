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
            transform.localRotation = targetRotation;
            isCoroutineRunning = false;
        }

        targetRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z + 36);
        StartCoroutine(RotateToDestination());
    }

    public void TurnRight()
    {
        // Quaternion currentLocalRotation = transform.localRotation;

        // Skip to end of rotation
        if (isCoroutineRunning)
        {
            transform.localRotation = targetRotation;
            isCoroutineRunning = false;
        }

        targetRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z - 36);
        StartCoroutine(RotateToDestination());
    }

    private IEnumerator RotateToDestination()
    {
        float timeElapsed = 0;
        Quaternion oldRotation = transform.localRotation;
        isCoroutineRunning = true;

        while (isCoroutineRunning)
        {
            timeElapsed += Time.deltaTime;

            transform.localRotation = Quaternion.Lerp(oldRotation, targetRotation, timeElapsed);

            if (timeElapsed >= 1)
                isCoroutineRunning = false;

            yield return null;
        }
    }
}
