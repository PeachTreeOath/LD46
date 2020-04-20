using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCheck : MonoBehaviour
{
   public bool ammoTrayFull = false;

   // Start is called before the first frame update
   void Start()
   {

   }

   // Update is called once per frame
   void Update()
   {

   }

   private void OnTriggerEnter(Collider other)
   {
      ammoTrayFull = true;
   }

   private void OnTriggerExit(Collider other)
   {
      ammoTrayFull = false;
   }

}
