using System.Collections;
using System.Collections.Generic;
using GAMEJAM.MyScript;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Video;
using Random = UnityEngine.Random;

public class KeyBoardController : MonoBehaviour
{
    [HideInInspector] PlayerController _playerRig;

    [HideInInspector] KeyboardRecoil _recoilCam;

    #region Shoot&Bullet
    [Header("SHOOT")]
    RaycastHit _raycastHit;
    public GameObject bulletPrefab;
    GameObject _bulletClone;
    public float shootForce = 100f;
    [SerializeField] float damage = 25f;
    [SerializeField] Transform shootPoint;
    [SerializeField] float range;
    [SerializeField] float rateOffire;
    float _nextFire = 0f;
    public Material escMat;
    public Material rMat;
    Color _currentEmission;
    //Color currentEmissionReload;
    float _newEmissionIntensity;
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
    bool _shootRay = false;
    #endregion

    #region Effect
    [Header("Effect")]
    public ParticleSystem muzzleFlash;
    public GameObject bloodEffect;
    GameObject _bloodClone;
    [SerializeField] private TrailRenderer bullettrail;
    public GameObject trailBulletPos;
    #endregion

    #region CrossHair
    [Header("CROSSHAiR")]
    public GameObject crossHair;
    public Image[] colorCrosshair;
    #endregion

    #region Keys
    [Header("KEYS")]
    Transform[] _keys;
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
    private new AudioSource audio;
    #endregion

    Vector3 _mousePosition;
    Ray _ray;

    private const bool IsPower = false;

    public float attackRadius = 2f;
    public float attackForce = 10f;
    private static readonly int Aim = Animator.StringToHash("Aim");


    public void Start()
    {
        _playerRig = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _recoilCam = GameObject.FindGameObjectWithTag("Recoil").GetComponent<KeyboardRecoil>();
        crossHair.SetActive(true);
        _keys = new Transform[parent.childCount];
        keysCount = _keys.Length;
        for (int i = 0; i < parent.childCount; i++)
        {
            _keys[i] = parent.GetChild(i);
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
            _playerRig.rigController.Play("Push");
        }

        if (Input.GetKeyDown(KeyCode.R) || currentAmmo == 0)
        {
            Reload();
        }

        #endregion

        #region AiM
        if (Input.GetButtonDown("Fire2"))
        {
            _playerRig.rigController.SetBool(Aim, true);
            isAim = true;
            crossHair.SetActive(false);
        }
        if (isAim)
        {
            if (Input.GetButtonUp("Fire2"))
            {
                _playerRig.rigController.SetBool(Aim, false);
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

        foreach (Collider col in colliders)
        {
            if (col.gameObject.tag == "Enemy")
            { 
                Rigidbody enemyRigidbody = col.gameObject.GetComponent<Rigidbody>(); 

                if (enemyRigidbody != null)
                {
                    Vector3 pushDirection = (col.transform.position - this.transform.position).normalized;

                    enemyRigidbody.AddForce(pushDirection * attackForce, ForceMode.Impulse);
                    Enemy enemy = enemyRigidbody.transform.GetComponent<Enemy>();
                    enemy.TakePushDamage();
                    
                }
            }
        }
    }

    private void Shoot()
    {
        if (Time.time > _nextFire)
        {
            if (isShoot)
            {
                audio.clip = tusSesleri[Random.Range(0, 2)];
                audio.Play();
                particleKey.Play();
                if (keysCount > 0)
                {
                    _keys[keysCount - 1].gameObject.SetActive(false);
                    keysCount--;
                    currentAmmo--;
                }
                muzzleFlash.Play();
                StartCoroutine(KeyLight(0.2f));
                _recoilCam.Recoil();
                ShootRay();

                if (_shootRay && !isAim)
                {
                    _playerRig.rigController.Play("shoot");
                  
                }
                if (IsPower)
                {
                    rateOffire = 0.01f;
                }
                else
                {
                    rateOffire = 0.1f;
                }
                _nextFire = 0;
                _nextFire = Time.time + rateOffire;
            }
        }
    }

    private void ShootRay()
    {
        _ray = Camera.main.ScreenPointToRay(_mousePosition);
        _mousePosition = Input.mousePosition;
        if (Physics.Raycast(_ray, out _raycastHit))
        {
            _bulletClone = Instantiate(bulletPrefab, trailBulletPos.transform.position, trailBulletPos.transform.rotation);

            Rigidbody rb = _bulletClone.GetComponent<Rigidbody>();
            rb.AddForce(shootPoint.forward * shootForce, ForceMode.Impulse);

            _shootRay = true;
            if (_raycastHit.transform.CompareTag("Head"))
            {
                damage = 100f;
                Enemy enemy = _raycastHit.collider.GetComponentInParent<Enemy>();
                enemy.TakeDamage(damage);
                _bloodClone = Instantiate(bloodEffect, _raycastHit.point, transform.rotation);
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

                    enemy.TriggerRagdoll(force, _raycastHit.point);
                }
                Destroy(_bulletClone, 0.1f);
            }
            if (_raycastHit.transform.CompareTag("TopBody"))
            {
                damage = 50f;
                Enemy enemy = _raycastHit.collider.GetComponentInParent<Enemy>();
                enemy.TakeDamage(damage);
                _bloodClone = Instantiate(bloodEffect, _raycastHit.point, transform.rotation);
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

                    enemy.TriggerRagdoll(force, _raycastHit.point);
                }
                Destroy(_bulletClone, 0.1f);
            }
            if (_raycastHit.transform.CompareTag("LowBody"))
            {
                damage = 25f;
                Enemy enemy = _raycastHit.collider.GetComponentInParent<Enemy>();
                enemy.TakeDamage(damage);
                _bloodClone = Instantiate(bloodEffect, _raycastHit.point, transform.rotation);
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

                    enemy.TriggerRagdoll(force, _raycastHit.point);
                }
                Destroy(_bulletClone, 0.1f);
            }
            else 
            {
                Destroy(_bulletClone, 2f);
            }
            Destroy(_bloodClone, 2f);
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
            _playerRig.rigController.Play("Reload");

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
        if (Time.time > _nextFire)
        {
            isShoot = false;
            _nextFire = 0;
            _nextFire = Time.time + rateOffire;
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
        if (_raycastHit.transform.CompareTag("Enemy"))
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
            _keys[i] = parent.GetChild(i);
            _keys[i].gameObject.SetActive(true);
        }

    }

    IEnumerator KeyLight(float time)
    {
        _currentEmission = escMat.GetColor("_EmissionColor");
        _newEmissionIntensity = _currentEmission.maxColorComponent + 20f;
        Color newEmissionColor = _currentEmission * (_newEmissionIntensity / _currentEmission.maxColorComponent);
        escMat.SetColor("_EmissionColor", newEmissionColor);

        yield return new WaitForSeconds(time);
        _currentEmission = escMat.GetColor("_EmissionColor");
        _newEmissionIntensity = _currentEmission.maxColorComponent - 20f;
        newEmissionColor = _currentEmission * (_newEmissionIntensity / _currentEmission.maxColorComponent);
        escMat.SetColor("_EmissionColor", newEmissionColor);

    }

    IEnumerator Blink(float time)
    {
        rMat.SetColor("_EmissionColor", onColor);

        yield return new WaitForSeconds(time);

        rMat.SetColor("_EmissionColor", offColor);
    }
}