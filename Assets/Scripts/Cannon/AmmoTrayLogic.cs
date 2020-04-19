using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoTrayLogic : MonoBehaviour
{
   [SerializeField] private float rotateSpeed = 20f;
   //[SerializeField] private float angleAddition = 40f;
   //[SerializeField] private float zRotationMax = 40f;
   [SerializeField] private Transform targetTransform;

   private bool isRotatingRight = false;
   private bool isRotatingLeft = false;
   private bool maxNumberChecked = false;
   private float newAngle;
   private float zRotation = 6f;

   // Start is called before the first frame update
   void Start()
   {

   }

   // Update is called once per frame
   void Update()
   {
      if (isRotatingRight)
      {
         RotateRight();
      }

      if (isRotatingLeft)
      {
         RotateLeft();
      }
   }

   private void RotateRight()
   {
      transform.RotateAround(this.transform.position, this.transform.forward, rotateSpeed * Time.deltaTime);
   }

   private void RotateLeft()
   {
      transform.RotateAround(this.transform.position, -this.transform.forward, rotateSpeed * Time.deltaTime);
   }

   public IEnumerator TurnOnOffRotateRight()
   {
      isRotatingRight = true;
      yield return new WaitForSeconds(1f);
      isRotatingRight = false;
      maxNumberChecked = false;
   }

   public IEnumerator TurnOnOffRotateLeft()
   {
      isRotatingLeft = true;
      yield return new WaitForSeconds(1f);
      isRotatingLeft = false;
      maxNumberChecked = false;
   }
}
