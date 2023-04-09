using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    [Header("Health")]
    public static PlayerHealth singleton;
    public float currentHealth;
    public float maxHealth = 400f;
    public Image healthBar;
    float lerpspeed;
  /*  [Header("Medicine")]
    public float medicineCount = 0f;
    public Text medicineCountText;
    public GameObject medicineHand;

    RaycastHit hit;
    [SerializeField] Transform Cam;
    public PostProcessProfile postPP;
    public GameObject shootPoint;
  */
    public float distance = 10f;

    public bool isDead = false;

    void Start()
    {
        singleton = this;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        HealthBarFiller();

        if (currentHealth > maxHealth) currentHealth = maxHealth;

        lerpspeed = 3f * Time.deltaTime;

        //Health++
        if (!isDead) currentHealth += 1f * Time.deltaTime;

       /* #region POST-PROCESSÝNG
        if (currentHealth <= 100f)
        {
            postPP.GetSetting<ChromaticAberration>().intensity.value = 0.5f;
            postPP.GetSetting<Vignette>().intensity.value = 0.1f;
            if (currentHealth <= 70f)
            {
                postPP.GetSetting<ColorGrading>().colorFilter.overrideState = true;
                postPP.GetSetting<Vignette>().intensity.value = 0.2f;

                if (currentHealth <= 40f)
                {
                    postPP.GetSetting<ChromaticAberration>().intensity.value = 1f;
                    postPP.GetSetting<Vignette>().intensity.value = 0.3f;
                    postPP.GetSetting<Vignette>().smoothness.value = 1f;
                    postPP.GetSetting<Vignette>().roundness.value = 1f;
                }
            }
        }
        else
        {
            postPP.GetSetting<Vignette>().intensity.value = 0f;
            postPP.GetSetting<ChromaticAberration>().intensity.value = 0;
            postPP.GetSetting<ColorGrading>().colorFilter.overrideState = false;
        }
        #endregion

        #region MEDÝCÝNE

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Medicine"))
        {
            medicineHand.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {

                Destroy(hit.collider.gameObject);
                medicineCount++;
            }

        }
        else medicineHand.SetActive(false);


        if (medicineCount != 0 && currentHealth < maxHealth)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                Heal();
                medicineCount--;
            }
        }
        medicineCountText.text = medicineCount.ToString();
        if (medicineCount < 0f) medicineCount = 0f;
        #endregion*/
    }


    public void DamagePlayer(float damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            //Cam.transform.DOPunchPosition(new Vector3(.5f, 0), 1, 10);
        }
        else
        {
            PlayerDead();
        }
    }

    public void PlayerDead()
    {
        currentHealth = 0;
        isDead = true;
    }

    void HealthBarFiller()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, currentHealth / maxHealth, lerpspeed);
    }

    public void Heal()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += 100f;
        }
    }
}
