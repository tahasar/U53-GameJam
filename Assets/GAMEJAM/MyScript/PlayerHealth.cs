using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using DG.Tweening;
public class PlayerHealth : MonoBehaviour
{

    [Header("Health")]
    public static PlayerHealth Singleton;
    public float currentHealth;
    public float maxHealth = 400f;
    public Image healthBar;
    float _lerpspeed;

    public PostProcessProfile postPp;

    public float distance = 10f;
    public Camera cam;
    public bool isDead = false;

    void Start()
    {
        Singleton = this;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        HealthBarFiller();

        if (currentHealth > maxHealth) currentHealth = maxHealth;

        _lerpspeed = 3f * Time.deltaTime;

        //Health++
        if (!isDead) currentHealth += 1f * Time.deltaTime;


        #region POST-PROCESSİNG

        if (currentHealth <= 300f)
        {
            postPp.GetSetting<Vignette>().intensity.value = 0.1f;
            if (currentHealth <= 150f)
            {
                postPp.GetSetting<ChromaticAberration>().intensity.value = 0.5f;
                postPp.GetSetting<Vignette>().intensity.value = 0.3f;

                if (currentHealth <= 75f)
                {
                    postPp.GetSetting<ChromaticAberration>().intensity.value = 1f;
                    postPp.GetSetting<Vignette>().intensity.value = 0.4f;
                }
            }
        }
        else
        {
            postPp.GetSetting<Vignette>().intensity.value = 0f;
            postPp.GetSetting<ChromaticAberration>().intensity.value = 0;
        }
        #endregion
        
    }


    public void DamagePlayer(float damage) //DÜZENLENCEK
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            var vibrato = (int)damage / 2;
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
        GameManager.Instance.GameOver();
    }

    void HealthBarFiller()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, currentHealth / maxHealth, _lerpspeed);
    }

    public void Heal()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += 100f;
        }
    }
}
