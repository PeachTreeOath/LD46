using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLogic : MonoBehaviour
{
   private const int HIGH_PRIORITY_NUM = 11;
   private const int LOW_PRIORITY_NUM = 9;
   private bool isCursorVisible = false;

   private Cinemachine.CinemachineVirtualCameraBase virtualCamera;

   // Start is called before the first frame update
   void Start()
   {
      //Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = isCursorVisible;
      virtualCamera = GetComponent<Cinemachine.CinemachineVirtualCameraBase>();
   }

   // Update is called once per frame
   void Update()
   {
      if (Input.GetButtonDown("Fire2"))
      {
         ChangeCameraPriority();
      }

      if (Input.GetKeyDown(KeyCode.Alpha0))
      {
         ChangeIsCursorVisible();
         //TurnCursorOnOff();
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

   private void ChangeIsCursorVisible()
   {
      if (isCursorVisible)
      {
         isCursorVisible = false;
      }
      else
      {
         isCursorVisible = true;
      }

      Cursor.visible = isCursorVisible;
   }

   //private void TurnCursorOnOff()
   //{
   //   if (isCursorVisible)
   //   {
   //      Cursor.lockState = CursorLockMode.None;
   //   }
   //   else
   //   {
   //      Cursor.lockState = CursorLockMode.Locked;
   //   }

   //   Cursor.visible = isCursorVisible;
   //}
}
