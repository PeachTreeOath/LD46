using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
   // Inspector set
   public FoodType foodType;
   public BulletType bulletType;
   public Sprite requirementIcon;

   private float destroyTime = 5f;

   // Start is called before the first frame update
   void Start()
   {
      Invoke("DestroyAfterSeconds", destroyTime);
   }

   // Update is called once per frame
   void Update()
   {

   }

   public void Despawn()
   {
      // TODO: Play sounds and animation

      Destroy(gameObject);
   }

   private void DestroyAfterSeconds()
   {
      Destroy(gameObject);
   }
}
