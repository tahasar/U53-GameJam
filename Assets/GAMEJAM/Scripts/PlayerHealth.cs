using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using DG.Tweening;
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
    */

    public PostProcessProfile postPP;

    public float distance = 10f;
    public Camera cam;
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


        #region POST-PROCESSİNG

        if (currentHealth <= 300f)
        {
            postPP.GetSetting<Vignette>().intensity.value = 0.1f;
            if (currentHealth <= 150f)
            {
                postPP.GetSetting<ChromaticAberration>().intensity.value = 0.5f;
                postPP.GetSetting<Vignette>().intensity.value = 0.3f;

                if (currentHealth <= 75f)
                {
                    postPP.GetSetting<ChromaticAberration>().intensity.value = 1f;
                    postPP.GetSetting<Vignette>().intensity.value = 0.4f;
                }
            }
        }
        else
        {
            postPP.GetSetting<Vignette>().intensity.value = 0f;
            postPP.GetSetting<ChromaticAberration>().intensity.value = 0;
        }
        #endregion



    }


    public void DamagePlayer(float damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            var vibrato = (int)damage /2;
            cam.transform.DOPunchPosition(new Vector3(.2f, .1f), 0.5f, vibrato);
           
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
        GameManager.instance.GameOver();
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
