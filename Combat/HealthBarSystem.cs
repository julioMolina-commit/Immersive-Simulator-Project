using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarSystem : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    private HPTracker hpTrack;
    private float maximumHP;
    private void Start()
    {
        hpTrack = GetComponent<HPTracker>();

        maximumHP = hpTrack.maxHP;
    }
    void Update()
    {
        healthBar.transform.LookAt(GameObject.Find("Main Camera").transform.position);

        float currentHP = hpTrack.HP;

        healthBar.fillAmount = currentHP / maximumHP;
    }
}
