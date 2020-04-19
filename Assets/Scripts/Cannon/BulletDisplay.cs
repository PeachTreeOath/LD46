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

         if (foodModel.GetComponent<FoodDisplayItem>().foodType == selectedBulletFoodType)
         {
            foodModel.gameObject.SetActive(true);
         }
         else
         {
            foodModel.gameObject.SetActive(false);
         }
      }
   }
}
