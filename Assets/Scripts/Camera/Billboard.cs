﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FiveXT.Core
{
   public class Billboard : MonoBehaviour
   {

      public new Camera camera;

      private void Start()
      {
         if (camera == null)
            camera = Camera.main;
      }

      void FixedUpdate()
      {
         transform.LookAt(transform.position + camera.transform.rotation * Vector3.back, camera.transform.rotation * Vector3.up);
      }
   }
}