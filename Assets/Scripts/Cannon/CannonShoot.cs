using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShoot : MonoBehaviour
{
   [SerializeField] private GameObject bulletPrefab;
   [SerializeField] private Transform cannonEnd;
   [SerializeField] private float shootingCooldown = 0.25f;
   [SerializeField] private float bulletShootForce = 5f;
   private float shootRate = 0.0f;
   private GameObject bullet;



   // Start is called before the first frame update
   void Start()
   {

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
   }

   private void Shoot()
   {
      bullet = Instantiate(bulletPrefab, cannonEnd.position, Quaternion.identity);
      bullet.GetComponent<Rigidbody>().AddForce(transform.forward * bulletShootForce, ForceMode.Impulse);

      shootRate = shootingCooldown;
   }
}
