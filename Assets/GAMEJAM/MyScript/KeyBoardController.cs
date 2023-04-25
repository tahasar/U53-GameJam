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
    public Material rMat;
    Color currentEmission;
    //Color currentEmissionReload;
    float newEmissionIntensity;
    //float newEmissionIntensityReload;
    public Color onColor;
    public Color offColor;
    public Color baseColor;
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

    bool isPower = false;

    public float attackRadius = 2f;
    public float attackForce = 10f;
   

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

        #region Attack

        if (Input.GetButton("Fire1") && currentAmmo > 0 && isShoot)
        {
            Shoot();
        }


        if (Input.GetButtonDown("Fire1") && currentAmmo <= 0)
        {
            DryFire();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            playerRig.rigController.Play("Push");
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

        #region ReloadLight

        if (currentAmmo < 5)
        {
            StartCoroutine(Blink(2f));
        }
        else
        {
            rMat.SetColor("_EmissionColor", baseColor);
        }
        #endregion
    }

    public void AttackPush() //AnimationEvent
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRadius); 

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.tag == "Enemy")
            { 
                Rigidbody enemyRigidbody = collider.gameObject.GetComponent<Rigidbody>(); 

                if (enemyRigidbody != null)
                {
                    Vector3 pushDirection = (collider.transform.position - this.transform.position).normalized;

                    enemyRigidbody.AddForce(pushDirection * attackForce, ForceMode.Impulse);
                    Enemy enemy = enemyRigidbody.transform.GetComponent<Enemy>();
                    enemy.TakePushDamage();
                    
                }
            }
        }
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
                muzzleFlash.Play();
                StartCoroutine(KeyLight(0.2f));
                recoilCam.Recoil();
                ShootRay();

                if (shootRay && !isAim)
                {
                    playerRig.rigController.Play("shoot");
                  
                }
                if (isPower)
                {
                    rateOffire = 0.01f;
                }
                else
                {
                    rateOffire = 0.1f;
                }
                nextFire = 0;
                nextFire = Time.time + rateOffire;
            }
        }
    }

    private void ShootRay()
    {
        ray = Camera.main.ScreenPointToRay(mousePosition);
        mousePosition = Input.mousePosition;
        if (Physics.Raycast(ray, out raycastHit))
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
            else 
            {
                Destroy(bulletClone, 2f);
            }
            Destroy(bloodClone, 2f);
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

    IEnumerator Blink(float time)
    {

        rMat.SetColor("_EmissionColor", onColor);

        yield return new WaitForSeconds(time);

        rMat.SetColor("_EmissionColor", offColor);
    



    }

}