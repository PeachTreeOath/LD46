using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShoot : Singleton<CannonShoot>
{
    public BulletDisplay bulletDisplay;
    public List<BulletDisplay> ammoTrayDisplays;
    public Animator anim;

    public GameObject[] bulletPrefabs;
    [SerializeField] private Transform cannonEnd;
    [SerializeField] private float shootingCooldown = 0.25f;
    [SerializeField] private float straightBulletForce = 25f;
    [SerializeField] private float lobBulletForce = 0f;
    [SerializeField] private float lobForceIncreaseAmount = 0.5f;

    private float bulletShootForce = 5f;
    private int selectedBulletIndex = 0;
    private int selectedAmmoTrayIndex = 0;
    private float shootRate = 0.0f;
    private GameObject bullet;
    private GameObject selectedBulletPrefab;
    private AmmoTrayLogic ammoTray;
    private Cinemachine.CinemachineImpulseSource impulseSource;
    private bool isShotQueuedUp;

    // Start is called before the first frame update
    void Start()
    {
        ammoTray = GetComponentInChildren<AmmoTrayLogic>();
        impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
        anim = GetComponent<Animator>();
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
            isShotQueuedUp = true;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f || Input.GetKeyDown(KeyCode.E))
        {
            ammoTray.TurnRight();
            //StartCoroutine(ammoTray.TurnOnOffRotateRight());
            BulletSelectionForward();
            RefreshAmmoTrayDisplays();
            AudioManager.instance.PlayRandomSpotInSwivel();
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0.0f || Input.GetKeyDown(KeyCode.Q))
        {
            ammoTray.TurnLeft();
            //StartCoroutine(ammoTray.TurnOnOffRotateLeft());
            BulletSelectionBackward();
            RefreshAmmoTrayDisplays();
            AudioManager.instance.PlayRandomSpotInSwivel();
        }
    }

    private void RefreshAmmoTrayDisplays()
    {
        int ammoTrayIdx = selectedAmmoTrayIndex;
        int bulletIdx = selectedBulletIndex;

        // Fill out 1 direction
        for (int i = 0; i < 3; i++)
        {
            GameObject prefab = bulletPrefabs[bulletIdx];
            ammoTrayDisplays[ammoTrayIdx].DisplayCurrentBullet(prefab.GetComponent<Bullet>().foodType);

            ammoTrayIdx++;
            if (ammoTrayIdx >= ammoTrayDisplays.Count)
            {
                ammoTrayIdx = 0;
            }

            bulletIdx++;
            if (bulletIdx >= bulletPrefabs.Length)
            {
                bulletIdx = 0;
            }
        }

        // Fill out the other direction
        ammoTrayIdx = selectedAmmoTrayIndex;
        bulletIdx = selectedBulletIndex;
        for (int i = 0; i < 3; i++)
        {
            GameObject prefab = bulletPrefabs[bulletIdx];
            ammoTrayDisplays[ammoTrayIdx].DisplayCurrentBullet(prefab.GetComponent<Bullet>().foodType);

            ammoTrayIdx--;
            if (ammoTrayIdx < 0)
            {
                ammoTrayIdx = ammoTrayDisplays.Count - 1;
            }

            bulletIdx--;
            if (bulletIdx < 0)
            {
                bulletIdx = bulletPrefabs.Length - 1;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isShotQueuedUp)
        {
            if (anim)
            {
                anim.SetTrigger("Shot");
            }
            GenerateShake();
            bulletShootForce = straightBulletForce;
            Shoot();
            isShotQueuedUp = false;
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

        RefreshAmmoTrayDisplays();
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

        if (selectedAmmoTrayIndex >= ammoTrayDisplays.Count - 1)
        {
            selectedAmmoTrayIndex = 0;
        }
        else
        {
            selectedAmmoTrayIndex++;
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

        if (selectedAmmoTrayIndex <= 0)
        {
            selectedAmmoTrayIndex = ammoTrayDisplays.Count - 1;
        }
        else
        {
            selectedAmmoTrayIndex--;
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
