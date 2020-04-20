using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShoot : Singleton<CannonShoot>
{
   public BulletDisplay bulletDisplay;

   [SerializeField] private GameObject[] bulletPrefabs;
   [SerializeField] private Transform cannonEnd;
   [SerializeField] private float shootingCooldown = 0.25f;
   [SerializeField] private float straightBulletForce = 25f;
   [SerializeField] private float lobBulletForce = 0f;
   [SerializeField] private float lobForceIncreaseAmount = 0.5f;

   private float bulletShootForce = 5f;
   private int selectedBulletIndex = 0;
   private float shootRate = 0.0f;
   private GameObject bullet;
   private GameObject selectedBulletPrefab;
   private AmmoTrayLogic ammoTray;
   private Cinemachine.CinemachineImpulseSource impulseSource;

   // Start is called before the first frame update
   void Start()
   { 
      ammoTray = GetComponentInChildren<AmmoTrayLogic>();
      impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
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
               GenerateShake();
               bulletShootForce = straightBulletForce;
               Shoot();
            }
            break;
         case BulletType.LOB:
            if (Input.GetButton("Fire1"))
            {
               IncreaseLobBulletForce();
            }

            if (Input.GetButtonUp("Fire1"))
            {
               GenerateShake();
               bulletShootForce = lobBulletForce;
               Shoot();
            }
            break;
      }

      if (Input.GetAxis("Mouse ScrollWheel") > 0.0f || Input.GetKeyDown(KeyCode.E))
      {
         StartCoroutine(ammoTray.TurnOnOffRotateRight());
         BulletSelectionForward();
         AudioManager.instance.PlayRandomSpotInSwivel();
      }

      if (Input.GetAxis("Mouse ScrollWheel") < 0.0f || Input.GetKeyDown(KeyCode.Q))
      {
         StartCoroutine(ammoTray.TurnOnOffRotateLeft());
         BulletSelectionBackward();
         AudioManager.instance.PlayRandomSpotInSwivel();
      }
   }

   public void InitAmmo(List<GameObject> ammoTypes)
   {
      bulletPrefabs = ammoTypes.ToArray();
      selectedBulletPrefab = bulletPrefabs[selectedBulletIndex];
      if (bulletDisplay)
      {
         bulletDisplay.DisplayCurrentBullet(selectedBulletPrefab.GetComponent<Bullet>().foodType);
      }
   }

   private void Shoot()
   {
      bullet = Instantiate(selectedBulletPrefab, cannonEnd.position, Quaternion.identity);

      bullet.GetComponent<Rigidbody>().AddForce(transform.forward * bulletShootForce, ForceMode.Impulse);

      shootRate = shootingCooldown;
      lobBulletForce = 0.0f;

      AudioManager.instance.PlayRandomShot();
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

      /// AudioManager.instance.PlaySound("Food_Truck_Cannon_Turn_Revolver_Right_2D");
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

      // AudioManager.instance.PlaySound("Food_Truck_Cannon_Turn_Revolver_Left_2D");
   }

   private void AssignSelectedBullet(int bulletIndex)
   {
      selectedBulletPrefab = bulletPrefabs[bulletIndex];
      
      if (bulletDisplay)
      {
         bulletDisplay.DisplayCurrentBullet(selectedBulletPrefab.GetComponent<Bullet>().foodType);
      }
   }

   private void IncreaseLobBulletForce()
   {
      lobBulletForce += (lobForceIncreaseAmount * Time.deltaTime);
   }

   private void GenerateShake()
   {
      if (impulseSource)
      {
         impulseSource.GenerateImpulse(transform.forward);
      }
   }
}
