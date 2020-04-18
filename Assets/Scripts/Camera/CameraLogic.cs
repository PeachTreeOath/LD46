using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLogic : MonoBehaviour
{
   private const int HIGH_PRIORITY_NUM = 11;
   private const int LOW_PRIORITY_NUM = 9;

   private Cinemachine.CinemachineVirtualCameraBase virtualCamera;

   // Start is called before the first frame update
   void Start()
   {
      virtualCamera = GetComponent<Cinemachine.CinemachineVirtualCameraBase>();
   }

   // Update is called once per frame
   void Update()
   {
      if (Input.GetButtonDown("Fire2"))
      {
         ChangeCameraPriority();
      }
   }

   private void ChangeCameraPriority()
   {
      if (virtualCamera)
      {
         if (virtualCamera.Priority == LOW_PRIORITY_NUM)
         {
            virtualCamera.Priority = HIGH_PRIORITY_NUM;
         }
         else if (virtualCamera.Priority == HIGH_PRIORITY_NUM)
         {
            virtualCamera.Priority = LOW_PRIORITY_NUM;
         }
      }
   }
}
