using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] int numBullets = 4;
    [SerializeField] float spreadAngle = 30f;
    [SerializeField] AudioSource audioSource;


    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        audioSource.Play();
        for (int i = 0; i < numBullets; i++)
        {
            float randomAngle = Random.Range(-spreadAngle, spreadAngle);
            Quaternion bulletRotation = Quaternion.Euler(0f, 0f, firePoint.rotation.eulerAngles.z + randomAngle);
            Instantiate(bulletPrefab, firePoint.position, bulletRotation);
        }
    }
}
