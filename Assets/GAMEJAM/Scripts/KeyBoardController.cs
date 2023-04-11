using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Video;
using Random = UnityEngine.Random;

public class KeyBoardController : MonoBehaviour
{
    [HideInInspector] PlayerController playerRig;

    [HideInInspector] KeyboardRecoil recoilCam;

    #region Shoot&Bullet
    [Header("SHOOT")]
    RaycastHit raycastHit;
    public GameObject bulletPrefab;
    GameObject bulletClone;
    public float shootForce = 100f;
    [SerializeField] float damage = 25f;
    [SerializeField] Transform ShootPoint;
    [SerializeField] float range;
    [SerializeField] float rateOffire;
    float nextFire = 0f;
    public Material escMat;
    Color currentEmission;
    float newEmissionIntensity;
    public bool isAim = false;
    #endregion

    #region Ammo
    [Header("Ammo")]
    public TextMeshProUGUI currentAmmotext;
    public TextMeshProUGUI carriedAmmotext;
    public int currentAmmo = 12;
    public int maxAmmo = 12;
    public int carriedAmmo = 60;
    public bool isReloading;
    public bool isShoot = true;
    bool shootRay = false;
    #endregion

    #region Effect
    [Header("Effect")]
    public ParticleSystem muzzleFlash;
    public GameObject bloodEffect;
    GameObject bloodClone;
    public GameObject bulletHoleEffect;
    GameObject bulletHoleClone;
    [SerializeField] private TrailRenderer Bullettrail;
    public GameObject TrailBulletPos;
    #endregion

    #region CrossHair
    [Header("CROSSHAiR")]
    public GameObject crossHair;
    public Image[] colorCrosshair;
    #endregion

    #region Keys
    [Header("KEYS")]
    Transform[] keys;
    public int keysCount;
    public Transform parent;
    public GameObject keyParticle;
    public ParticleSystem particleKey;
    #endregion

    #region Force
    [Header("Force")]
    [SerializeField] private float maxForce;
    [SerializeField] private float maxForceTime;
    #endregion


    #region Audıo
    [Header("Audio")]
    public AudioClip[] tusSesleri;
    public AudioClip reloadSesi;
    private AudioSource audio;
    #endregion

    Vector3 mousePosition;
    Ray ray;


    public void Start()
    {
        playerRig = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        recoilCam = GameObject.FindGameObjectWithTag("Recoil").GetComponent<KeyboardRecoil>();
        crossHair.SetActive(true);
        keys = new Transform[parent.childCount];
        keysCount = keys.Length;
        for (int i = 0; i < parent.childCount; i++)
        {
            keys[i] = parent.GetChild(i);
        }

        audio = GetComponent<AudioSource>();

        
    }

    public void Update()
    {
        UpdateAmmoUI();

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

        #region AiM
        if (Input.GetButtonDown("Fire2"))
        {
            playerRig.rigController.SetBool("Aim", true);
            isAim = true;
            crossHair.SetActive(false);
            Debug.Log("xd");
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

    private void Shoot()
    {
        if (Time.time > nextFire)
        {
            if (isShoot)
            {
                audio.clip = tusSesleri[Random.Range(0, 2)];
                audio.Play();
                particleKey.Play();
                if (keysCount > 0)
                {
                    keys[keysCount - 1].gameObject.SetActive(false);
                    keysCount--;
                    currentAmmo--;
                }
                nextFire = Time.time + rateOffire;
                nextFire = 0;

                muzzleFlash.Play();
                StartCoroutine(KeyLight(0.2f));
                recoilCam.Recoil();
                ShootRay();

                if (isAim && shootRay)
                {

                    playerRig.rigController.SetBool("AimShoot", true);
                }
                else if (!isAim && shootRay)
                {

                    playerRig.rigController.Play("shoot");
                }
                nextFire = Time.time + rateOffire;
            }

        }
    }

    private void ShootRay()
    {
        ray = Camera.main.ScreenPointToRay(mousePosition);
        mousePosition = Input.mousePosition;
        if (Physics.Raycast(ray , out raycastHit))
        {
            bulletClone = Instantiate(bulletPrefab, TrailBulletPos.transform.position, TrailBulletPos.transform.rotation);

            Rigidbody rb = bulletClone.GetComponent<Rigidbody>();
            rb.AddForce(ShootPoint.forward * shootForce, ForceMode.Impulse);

            shootRay = true;
            if (raycastHit.transform.CompareTag("Head"))
            {
                damage = 100f;
                Enemy enemy = raycastHit.collider.GetComponentInParent<Enemy>();
                enemy.TakeDamage(damage);
                bloodClone = Instantiate(bloodEffect, raycastHit.point, transform.rotation);
                if (enemy.health <= 0)
                {
                    #region RagdollForce
                    float mouseButtonDown = Time.time - 1;
                    float forcePercentage = mouseButtonDown / maxForceTime;
                    float forceMagnitude = Mathf.Lerp(1, maxForce, forcePercentage);

                    Vector3 forceDirection = enemy.transform.position - transform.position;
                    forceDirection.y = 1;
                    forceDirection.Normalize();
                    Vector3 force = forceMagnitude * forceDirection;
                    #endregion

                    enemy.TriggerRagdoll(force, raycastHit.point);
                }
                Destroy(bulletClone, 0.1f);
            }
            if (raycastHit.transform.CompareTag("TopBody"))
            {
                damage = 50f;
                Enemy enemy = raycastHit.collider.GetComponentInParent<Enemy>();
                enemy.TakeDamage(damage);
                bloodClone = Instantiate(bloodEffect, raycastHit.point, transform.rotation);
                if (enemy.health <= 0)
                {
                    #region RagdollForce
                    float mouseButtonDown = Time.time - 1;
                    float forcePercentage = mouseButtonDown / maxForceTime;
                    float forceMagnitude = Mathf.Lerp(1, maxForce, forcePercentage);

                    Vector3 forceDirection = enemy.transform.position - transform.position;
                    forceDirection.y = 1;
                    forceDirection.Normalize();
                    Vector3 force = forceMagnitude * forceDirection;
                    #endregion

                    enemy.TriggerRagdoll(force, raycastHit.point);
                }
                Destroy(bulletClone, 0.1f); 
            }
            if (raycastHit.transform.CompareTag("LowBody"))
            {
                damage = 25f;
                Enemy enemy = raycastHit.collider.GetComponentInParent<Enemy>();
                enemy.TakeDamage(damage);
                bloodClone = Instantiate(bloodEffect, raycastHit.point, transform.rotation);
                if (enemy.health <= 0)
                {
                    #region RagdollForce
                    float mouseButtonDown = Time.time - 1;
                    float forcePercentage = mouseButtonDown / maxForceTime;
                    float forceMagnitude = Mathf.Lerp(1, maxForce, forcePercentage);

                    Vector3 forceDirection = enemy.transform.position - transform.position;
                    forceDirection.y = 1;
                    forceDirection.Normalize();
                    Vector3 force = forceMagnitude * forceDirection;
                    #endregion

                    enemy.TriggerRagdoll(force, raycastHit.point);
                }
                Destroy(bulletClone, 0.1f);
            }
            else if(raycastHit.transform.CompareTag("Untagged"))
            {

                bulletHoleClone = Instantiate(bulletHoleEffect, raycastHit.point, transform.rotation);
                Destroy(bulletClone, 2f);
            }
            Destroy(bloodClone, 2f); Destroy(bulletHoleClone, 2f);
        }
    }

    public void Reload()
    {
        if (!isReloading && currentAmmo != maxAmmo && carriedAmmo != 0)
        {
            audio.clip = reloadSesi;
            audio.Play();
            isShoot = false;

            if (!isAim)
            {
                crossHair.SetActive(true);
            }
            playerRig.rigController.Play("Reload");


            //reloadAS.PlayOneShot(reloadAC);
            if (carriedAmmo <= 0) return;

            StartCoroutine(ReloadCountdown(1.5f));
        }
    }

    IEnumerator ReloadCountdown(float timer)
    {
        while (timer > 0f)
        {
            isReloading = true;
            timer -= Time.deltaTime;
            yield return null;

        }
        if (timer <= 0f)
        {
            int bulletsNeededToFillMag = maxAmmo - currentAmmo;
            int bulletsToDeduct = (carriedAmmo >= bulletsNeededToFillMag) ? bulletsNeededToFillMag : carriedAmmo;

            carriedAmmo -= bulletsToDeduct;
            currentAmmo += bulletsToDeduct;

            isReloading = false;
            UpdateAmmoUI();
            isShoot = true;

        }
    }

    public void UpdateAmmoUI()
    {
        currentAmmotext.text = currentAmmo.ToString();
        carriedAmmotext.text = carriedAmmo.ToString();

    }

    private void DryFire()
    {
        if (Time.time > nextFire)
        {
            isShoot = false;
            nextFire = 0;
            nextFire = Time.time + rateOffire;
        }
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, RaycastHit Hit)
    {
        float time = 0f;
        Vector3 startPosition = Trail.transform.position;
        while (time < 1)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, Hit.point, time);
            time += Time.deltaTime / Trail.time;
            yield return null;
        }
        Trail.transform.position = Hit.point;
    }

    private void CrossHairColor()
    {
        if (raycastHit.transform.CompareTag("Enemy"))
        {
            for (int i = 0; i < colorCrosshair.Length; i++)
            {
                Image ui = colorCrosshair[i];
                ui.GetComponent<Image>().color = Color.white;
            }
        }
        else
        {
            for (int i = 0; i < colorCrosshair.Length; i++)
            {
                Image ui = colorCrosshair[i];
                ui.GetComponent<Image>().color = Color.red;
            }
        }


    }

    public void KeyBoardFix()
    {

        for (int i = 0; i < parent.childCount; i++)
        {
            keys[i] = parent.GetChild(i);
            keys[i].gameObject.SetActive(true);
        }

    }

    IEnumerator KeyLight(float time)
    {
        currentEmission = escMat.GetColor("_EmissionColor");
        newEmissionIntensity = currentEmission.maxColorComponent + 20f;
        Color newEmissionColor = currentEmission * (newEmissionIntensity / currentEmission.maxColorComponent);
        escMat.SetColor("_EmissionColor", newEmissionColor);

        yield return new WaitForSeconds(time);
        currentEmission = escMat.GetColor("_EmissionColor");
        newEmissionIntensity = currentEmission.maxColorComponent - 20f;
        newEmissionColor = currentEmission * (newEmissionIntensity / currentEmission.maxColorComponent);
        escMat.SetColor("_EmissionColor", newEmissionColor);

    }
  
}