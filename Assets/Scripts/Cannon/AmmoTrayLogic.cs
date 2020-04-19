using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoTrayLogic : MonoBehaviour
{
   private GameObject ammoTray;
   private bool isRotatingRight = false;

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
      float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, transform.eulerAngles.z + 35f, 45f * Time.deltaTime);
      transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle);
   }

   public IEnumerator TurnOnOffRotateRight()
   {
      isRotatingRight = true;
      yield return new WaitForSeconds(0.5f);
      isRotatingRight = false;
   }
}
