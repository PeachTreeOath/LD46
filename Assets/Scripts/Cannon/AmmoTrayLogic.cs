using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoTrayLogic : MonoBehaviour
{
   [SerializeField] private float rotateSpeed = 20f;
   [SerializeField] private float angleAddition = 40f;
   [SerializeField] private float zRotationMax = 40f;

   private GameObject ammoTray;
   private bool isRotatingRight = false;
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
   }

   private void RotateRight()
   {
      if (!maxNumberChecked)
      {
         zRotation = zRotation + zRotationMax;
         maxNumberChecked = true;
      }

      transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-90f, 0f, zRotation), rotateSpeed * Time.deltaTime);
   }

   public IEnumerator TurnOnOffRotateRight()
   {
      isRotatingRight = true;
      yield return new WaitForSeconds(0.35f);
      isRotatingRight = false;
      maxNumberChecked = false;
   }
}
