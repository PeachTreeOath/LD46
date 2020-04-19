using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDisplay : MonoBehaviour
{
   //public GameObject foodHolder;
   //public GameObject[] foodModels;

   // Start is called before the first frame update
   void Start()
   {

   }

   // Update is called once per frame
   void Update()
   {

   }

   public void DisplayCurrentBullet(FoodType selectedBulletFoodType)
   {
      foreach (Transform foodModel in transform)
      {
         switch(selectedBulletFoodType)
         {
            case FoodType.BANANA:
               foodModel.gameObject.SetActive(true);
               break;
         }
      }
   }
}
