using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    [SerializeField] private Image healthOrb;
    [SerializeField] private Image staminaOrb;
    private HPTracker playerHP;
    private PlayerMovement pm;
    private float maximumHP, maximumStamina;
    //private StaminaTracker playerStamina;
    private void Start()
    {
        playerHP = GetComponent<HPTracker>();
        pm = GameObject.Find("Player Body").GetComponent<PlayerMovement>();

        maximumHP = playerHP.maxHP;
        maximumStamina = pm.staminaMax;
    }
    private void Update()
    {
        float currentHP = playerHP.HP;

        float currentStamina = pm.stamina;

        healthOrb.fillAmount = currentHP / maximumHP;
        staminaOrb.fillAmount = currentStamina / maximumStamina;
    }
}
