using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Gun : MonoBehaviour
{

    [Header("Gun Properties")]
    public float damage;
    public float maxDamageToDistance;
    public float damageFalloff;
    public float maxDistance;
    public int maxAmmo;
    private int ammo;
    public int roundsPerMinute;
    public float reloadTime;
    public bool singleFire = false;

    [Space]

    [Header("Objects")]
    public Transform playerCamera;

    [Space]

    [Header("Key Binds")]
    public KeyCode shoot;
    public KeyCode reload;

    private bool canShoot;
    private bool reloading;
    private float timeBetweenShots;
    private float timeToShoot;

    void Awake()
    {
        ammo = maxAmmo;
        timeBetweenShots = 60f / roundsPerMinute;
    }

    void Update()
    {
        timeToShoot += Time.deltaTime;
        if ((singleFire ? true : timeToShoot >= timeBetweenShots) && ammo > 0) canShoot = true;
        else canShoot = false;

        if (reloading) canShoot = false;

        ProcessInputs();
    }

    void ProcessInputs()
    {
        if ((singleFire ? Input.GetKeyDown(shoot) : Input.GetKey(shoot)) && canShoot)
        {
            Shoot();
        }
        if (Input.GetKeyDown(reload))
        {
            StartCoroutine(ReloadGun());
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, maxDistance))
        {
            GameObject hitObject = hit.transform.gameObject;
            if (hitObject.GetComponent<Health>() != null)
            {
                Health health = hitObject.GetComponent<Health>();

                float rayLength = hit.distance;
                float damageDealt = damage;
                
                if (rayLength <= maxDamageToDistance)
                {
                    damageDealt = damage;
                }
                else
                {
                    float dropoff = damageFalloff * (rayLength - maxDamageToDistance);
                    damageDealt = damage - dropoff;
                }

                health.TakeDamage(damageDealt);
            }

            ammo--;
            timeToShoot = 0;
        }
    }

    IEnumerator ReloadGun()
    {
        canShoot = false;
        reloading = true;

        yield return new WaitForSeconds(reloadTime);

        ammo = maxAmmo;
        canShoot = true;
        reloading = false;
    }
}
