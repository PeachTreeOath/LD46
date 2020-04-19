using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShoot : Singleton<CannonShoot>
{
   [SerializeField] private GameObject[] bulletPrefabs;
   [SerializeField] private Transform cannonEnd;
   [SerializeField] private float shootingCooldown = 0.25f;
   [SerializeField] private float straightBulletForce = 25f;
   [SerializeField] private float lobBulletForce = 0f;

   private float bulletShootForce = 5f;
   private int selectedBulletIndex = 0;
   private float shootRate = 0.0f;
   private GameObject bullet;
   private GameObject selectedBulletPrefab;
   private AmmoTrayLogic ammoTray;

   // Start is called before the first frame update
   void Start()
   {
      selectedBulletPrefab = bulletPrefabs[selectedBulletIndex];
      ammoTray = GetComponentInChildren<AmmoTrayLogic>();
   }

   // Update is called once per frame
   void Update()
   {
      if (shootRate > 0.0f)
      {
         shootRate -= Time.deltaTime;
      }

      switch (selectedBulletPrefab.GetComponent<Bullet>().bulletType)
      {
         case BulletType.STRAIGHT:
            if (Input.GetButtonDown("Fire1") && shootRate <= 0.0f)
            {
               bulletShootForce = straightBulletForce;
               Debug.Log("Straight shot selected");
               Shoot();
            }
            break;
         case BulletType.LOB:
            if (Input.GetButton("Fire1"))
            {
               Debug.Log("Pressing down");
               InvokeRepeating("IncreaseLobBulletForce", 0.3f, Time.deltaTime);
            }

            if (Input.GetButtonUp("Fire1"))
            {
               CancelInvoke();
               bulletShootForce = lobBulletForce;
               Shoot();
            }
            break;
      }

      //if (Input.GetButtonDown("Fire1") && shootRate <= 0.0f)
      //{
      //   Shoot();
      //}

      if (Input.GetAxis("Mouse ScrollWheel") > 0.0f || Input.GetKeyDown(KeyCode.E))
      {
         StartCoroutine(ammoTray.TurnOnOffRotateRight());
         BulletSelectionForward();
      }

      if (Input.GetAxis("Mouse ScrollWheel") < 0.0f || Input.GetKeyDown(KeyCode.Q))
      {
         BulletSelectionBackward();
      }
   }

   public void InitAmmo(List<GameObject> ammoTypes)
   {
      bulletPrefabs = ammoTypes.ToArray();
   }

   private void Shoot()
   {
      bullet = Instantiate(selectedBulletPrefab, cannonEnd.position, Quaternion.identity);

      bullet.GetComponent<Rigidbody>().AddForce(transform.forward * bulletShootForce, ForceMode.Impulse);

      shootRate = shootingCooldown;
      lobBulletForce = 0.0f;
   }

   private void BulletSelectionForward()
   {
      if (selectedBulletIndex >= bulletPrefabs.Length - 1)
      {
         selectedBulletIndex = 0;
      }
      else
      {
         selectedBulletIndex++;
      }

      AssignSelectedBullet(selectedBulletIndex);
   }

   private void BulletSelectionBackward()
   {
      if (selectedBulletIndex <= 0)
      {
         selectedBulletIndex = bulletPrefabs.Length - 1;
      }
      else
      {
         selectedBulletIndex--;
      }

      AssignSelectedBullet(selectedBulletIndex);
   }

   private void AssignSelectedBullet(int bulletIndex)
   {
      selectedBulletPrefab = bulletPrefabs[bulletIndex];
   }

   private void IncreaseLobBulletForce()
   {
      lobBulletForce += 0.01f;
   }
}
