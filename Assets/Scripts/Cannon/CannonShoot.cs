using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShoot : Singleton<CannonShoot>
{
    [SerializeField] private GameObject[] bulletPrefabs;
    [SerializeField] private Transform cannonEnd;
    [SerializeField] private float shootingCooldown = 0.25f;
    [SerializeField] private float bulletShootForce = 5f;

    private bool isCursorVisible = false;
    private int selectedBulletIndex = 0;
    private float shootRate = 0.0f;
    private GameObject bullet;
    private GameObject selectedBulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = isCursorVisible;
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

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ChangeIsCursorVisible();
            TurnCursorOnOff();
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
    }

    private void BulletSelectionForward()
    {
        if (selectedBulletIndex >= bulletPrefabs.Length - 1)
        {
            selectedBulletIndex = 0;
            //Debug.Log("Selected bullet number (forward) = " + selectedBulletIndex);
        }
        else
        {
            selectedBulletIndex++;
            //Debug.Log("Selected bullet number (forward) = " + selectedBulletIndex);
        }

        AssignSelectedBullet(selectedBulletIndex);
    }

    private void BulletSelectionBackward()
    {
        if (selectedBulletIndex <= 0)
        {
            selectedBulletIndex = bulletPrefabs.Length - 1;
            //Debug.Log("Selected bullet number (backward) = " + selectedBulletIndex);
        }
        else
        {
            selectedBulletIndex--;
            //Debug.Log("Selected bullet number (backward) = " + selectedBulletIndex);
        }

        AssignSelectedBullet(selectedBulletIndex);
    }

    private void AssignSelectedBullet(int bulletIndex)
    {
        selectedBulletPrefab = bulletPrefabs[bulletIndex];
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
    }

    private void TurnCursorOnOff()
    {
        if (isCursorVisible)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        Cursor.visible = isCursorVisible;
    }
}
