using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameplayPanel : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    
    [SerializeField] private Transform healthFlagContainer;
    [SerializeField] private Transform healthFlagTemplate;
    [SerializeField] private Transform maxHealthFlagContainer;
    [SerializeField] private Transform maxHealthFlagTemplate;

    private List<Transform> healthFlags;
    private List<Transform> maxHealthFlags;

    private void Awake()
    {
        healthFlags = new List<Transform>();
        maxHealthFlags = new List<Transform>();
    }

    private void Start()
    {
        playerHealth.OnHit += PlayerHealth_OnHit;
        playerHealth.OnHeal += PlayerHealth_OnHeal;
        playerHealth.OnMaxHealthChange += PlayerHealth_OnMaxHealthChange;
        
        UpdateHealthToNewValue();
        UpdateMaxHealthToNewValue();
    }

    private void PlayerHealth_OnMaxHealthChange(object sender, EventArgs e)
    {
        UpdateMaxHealthToNewValue();
    }

    private void PlayerHealth_OnHeal(object sender, EventArgs e)
    {
        UpdateHealthToNewValue();
        Debug.Log("Heal");
    }

    private void PlayerHealth_OnHit(object sender, EventArgs e)
    {
        UpdateHealthToNewValue();
    }

    private void UpdateHealthToNewValue()
    {
        int health = playerHealth.GetHealth();

        Debug.Log("Update Health");

        while (healthFlags.Count < health)
        {
            var flagTransform = Instantiate(healthFlagTemplate, healthFlagContainer);
            flagTransform.gameObject.SetActive(true);
            healthFlags.Add(flagTransform.transform);
        }

        while (healthFlags.Count > health)
        {
            var flagTransform = healthFlags[0];
            Destroy(flagTransform.gameObject);
            healthFlags.RemoveAt(0);
        }
    }

    private void UpdateMaxHealthToNewValue()
    {
        int maxHealth = playerHealth.GetMaxHealth();

        while (maxHealthFlags.Count < maxHealth)
        {
            var flagTransform = Instantiate(maxHealthFlagTemplate, maxHealthFlagContainer);
            flagTransform.gameObject.SetActive(true);
            maxHealthFlags.Add(flagTransform.transform);
        }

        while (maxHealthFlags.Count > maxHealth)
        {
            var flagTransform = maxHealthFlags[0];
            Destroy(flagTransform.gameObject);
            maxHealthFlags.RemoveAt(0);
        }
    }
}
