using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBoardController : MonoBehaviour
{
    [Header("SHOOT")]
    [SerializeField] float damage = 25f;
    [SerializeField] Transform ShootPoint;
    [SerializeField] float range;
    [SerializeField] float rateOffire;
    float nextFire = 0f;
    public bool isAim = false;

    [HideInInspector] PlayerController playerRig;

    [Header("CROSSHAÝR")]
    public GameObject crossHair;
    public Image[] colorCrosshair;

    #region Ammo
    [Header("Ammo")]
    public Text currentAmmotext;
    public Text carriedAmmotext;
    public int currentAmmo = 12;
    public int maxAmmo = 12;
    public int carriedAmmo = 60;
    public bool isReloading;
    public bool isShoot = true;
    bool shootRay = false;
    #endregion

    public void Start()
    {
        crossHair.SetActive(true);
    }


    public void Update()
    {
        #region SHOOT

        if (Input.GetButton("Fire1") && currentAmmo > 0 && isShoot)
        {
            Shoot();
        }
       

        if (Input.GetButtonDown("Fire1") && currentAmmo <= 0)
        {
            DryFire();

        }
        if (Input.GetKeyDown(KeyCode.R) || currentAmmo == 0)
        {

            Reload();
        }

        #endregion


        #region AÝM
        if (Input.GetButtonDown("Fire2"))
        {
            playerRig.rigController.SetBool("Aim", true);

            isAim = true;

            crossHair.SetActive(false);


        }
        if (isAim)
        {
            if (Input.GetButtonUp("Fire2"))
            {
                playerRig.rigController.SetBool("Aim", false);

                isAim = false;
                crossHair.SetActive(true);
            }
        }
        #endregion
    }

    private void Reload()
    {
        throw new NotImplementedException();
    }

    private void DryFire()
    {
        throw new NotImplementedException();
    }

    private void Shoot()
    {
        throw new NotImplementedException();
    }
}
