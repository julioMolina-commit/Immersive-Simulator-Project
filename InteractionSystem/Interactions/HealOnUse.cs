using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOnUse : MonoBehaviour
{
    [SerializeField] int healAmount;
    public void Activate()
    {
        GameObject.Find("Player Body").GetComponent<HPTracker>().HealDamage(healAmount);
    }
}
