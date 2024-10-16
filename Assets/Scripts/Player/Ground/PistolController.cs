using UnityEngine;

public class ShotgunController : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float pistolFireRate = 0.5f;
    [SerializeField] AudioSource audioSource;
    

    private float nextFireTime = 0f;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
           
            ShootPistol();
        }
    }

    void ShootPistol()
    {
        if (Time.time >= nextFireTime)
        {
            audioSource.Play();
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            nextFireTime = Time.time + pistolFireRate;
        }
    }
}
