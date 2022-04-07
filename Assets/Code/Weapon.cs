using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum ShootMode
{
    Auto,
    Semi
}

[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour
{
    [Header("Shooting mode")]
    [Tooltip("The shooting mode of the weapon")]
    [SerializeField] private ShootMode shootingMode;

    [Header("Ammo")]
    [Tooltip("The projectile prefab")]
    [SerializeField] private GameObject projectilePrefab;
    [Tooltip("Amount of bullets per shot")]
    [SerializeField] private int BulletsPerShot;
    [Tooltip("The amount of bullet in 1 magazine")]
    [SerializeField] private int bulletsInMag = 30;
    [Tooltip("The amount of bullets left")]
    [SerializeField] private int bulletsLeft = 200;
    [Tooltip("The current amount of bullets left in your magazine")]
    [SerializeField] private int currentBulletsInMag;

    [Header("Gun")]
    [Tooltip("Is this gun for the player or not")]
    [SerializeField] private bool playerWeapon;
    [Tooltip("How fast you fire")]
    [SerializeField] private float fireRate = .5f;
    [Tooltip("The amount of bullet spread the gun will have")]
    [SerializeField] private float spreadAmount = .1f;
    [Tooltip("How long it takes to reload")]
    [SerializeField] private float reloadTime = 2;
    [Tooltip("Tip of the weapon where the bullets are shot")]
    [SerializeField] private Transform firePoint;

    [Header("Effects")]
    [Tooltip("Prefab of the muzzle flash")]
    [SerializeField] private ParticleSystem muzzleFlash;

    [Header("Audio")]
    [Tooltip("Sound played when shooting")]
    [SerializeField] private AudioClip[] shootSound;
    [Tooltip("Sound played when reloading")]
    [SerializeField] private AudioClip reloadSound;

    private AudioSource audioSource;
    private float fireTimer;
    private bool isReloading;
    private bool shootInput;

    public bool PlayerWeapon { set { playerWeapon = value; } }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentBulletsInMag = bulletsInMag;
    }

    public void Update()
    {
        if (fireTimer < fireRate)
            fireTimer += Time.deltaTime;

        if (playerWeapon)
        {
            switch (shootingMode)
            {
                case ShootMode.Auto:
                    shootInput = Input.GetButton("Fire1");
                    break;
                case ShootMode.Semi:
                    shootInput = Input.GetButtonDown("Fire1");
                    break;
            }

            if (shootInput)
            {
                if (currentBulletsInMag > 0)
                    Fire();
                else if (bulletsLeft > 0 && !isReloading)
                    StartCoroutine(DoReload());
            }


            if (Input.GetKeyDown(KeyCode.R))
            {
                if (currentBulletsInMag < bulletsInMag && bulletsLeft > 0 && !isReloading)
                    StartCoroutine(DoReload());
            }
        }
    }

    /// <summary>
    /// Call this to shoot a bullet
    /// </summary>
    public void Fire()
    {
        if (fireTimer < fireRate || currentBulletsInMag <= 0 || isReloading)
            return;

        for (int i = 0; i < BulletsPerShot; i++)
        {
            Vector3 direction = transform.forward + new Vector3(Random.Range(-spreadAmount, spreadAmount), Random.Range(-spreadAmount, spreadAmount), Random.Range(-spreadAmount, spreadAmount));
            Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(direction));
        }

        if (muzzleFlash)
            muzzleFlash.Play();

        if (shootSound.Length > 0)
            PlayShootSound();

        currentBulletsInMag--;

        if (currentBulletsInMag < 1 && bulletsLeft > 0 && !isReloading)
            StartCoroutine(DoReload());

        fireTimer = 0;
    }

    /// <summary>
    /// This is called to reload your magazine
    /// </summary>
    private void Reload()
    {
        if (bulletsLeft <= 0)
            return;

        int bulletsToLoad = bulletsInMag - currentBulletsInMag;
        int bulletsToUse = (bulletsLeft >= bulletsToLoad) ? bulletsToLoad : bulletsLeft;

        bulletsLeft -= bulletsToUse;
        currentBulletsInMag += bulletsToUse;
    }

    /// <summary>
    /// Call this to reload your gun
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoReload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        if (reloadSound)
            audioSource.PlayOneShot(reloadSound);
        Reload();
        isReloading = false;
    }

    /// <summary>
    /// Call this to play the shoot sound
    /// </summary>
    private void PlayShootSound()
    {
        audioSource.PlayOneShot(shootSound[Random.Range(0, shootSound.Length)]);
    }
}
