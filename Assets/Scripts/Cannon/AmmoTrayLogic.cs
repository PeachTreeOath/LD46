using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoTrayLogic : MonoBehaviour
{
   [SerializeField] private float rotateSpeed = 65f;
   [SerializeField] private float angleAddition = 40f;

   private GameObject ammoTray;
   private bool isRotatingRight = false;
   private bool maxNumberChecked = false;
   private float newAngle;
   private float z_rotation = 0f;

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
      //float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, angleAddition, rotateSpeed * Time.deltaTime);
      //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle);  
   }

   public IEnumerator TurnOnOffRotateRight()
   {
      isRotatingRight = true;
      yield return new WaitForSeconds(0.35f);
      isRotatingRight = false;
   }
}
