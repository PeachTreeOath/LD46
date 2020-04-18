﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShoot : MonoBehaviour
{
   [SerializeField] private GameObject[] bulletPrefabs;
   [SerializeField] private Transform cannonEnd;
   [SerializeField] private float shootingCooldown = 0.25f;
   [SerializeField] private float bulletShootForce = 5f;

   private int selectedBulletIndex = 0;
   private float shootRate = 0.0f;
   private GameObject bullet;
   private GameObject selectedBulletPrefab;



   // Start is called before the first frame update
   void Start()
   {
      selectedBulletPrefab = bulletPrefabs[selectedBulletIndex];
   }

   // Update is called once per frame
   void Update()
   {
      if (shootRate > 0.0f)
      {
         shootRate -= Time.deltaTime;
      }

      if (Input.GetButtonDown("Fire1") && shootRate <= 0.0f)
      {
         Shoot();
      }

      if (Input.GetAxis("Mouse ScrollWheel") > 0.0f || Input.GetKeyDown(KeyCode.E))
      {
         BulletSelectionForward();
      }

      if (Input.GetAxis("Mouse ScrollWheel") < 0.0f || Input.GetKeyDown(KeyCode.Q))
      {
         BulletSelectionBackward();
      }
   }

   private void Shoot()
   {
      bullet = Instantiate(selectedBulletPrefab, cannonEnd.position, Quaternion.identity);
 
      bullet.GetComponent<Rigidbody>().AddForce(transform.forward * bulletShootForce, ForceMode.Impulse);

      shootRate = shootingCooldown;
   }

   private void BulletSelectionForward()
   {
      if (selectedBulletIndex >= bulletPrefabs.Length - 1)
      {
         selectedBulletIndex = 0;
         Debug.Log("Selected bullet number (forward) = " + selectedBulletIndex);
      }
      else
      {
         selectedBulletIndex++;
         Debug.Log("Selected bullet number (forward) = " + selectedBulletIndex);
      }

      selectedBulletPrefab = bulletPrefabs[selectedBulletIndex];
   }

   private void BulletSelectionBackward()
   {
      if (selectedBulletIndex <= 0)
      {
         selectedBulletIndex = bulletPrefabs.Length - 1;
         Debug.Log("Selected bullet number (backward) = " + selectedBulletIndex);
      }
      else
      {
         selectedBulletIndex--;
         Debug.Log("Selected bullet number (backward) = " + selectedBulletIndex);
      }

      selectedBulletPrefab = bulletPrefabs[selectedBulletIndex];
   }
}
