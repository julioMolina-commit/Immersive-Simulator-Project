using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HPTracker : MonoBehaviour
{
    public int maxHP;
    public int HP;

    bool bloodied;

    [SerializeField] private UnityEvent bloodiedEvent;
    [SerializeField] private UnityEvent deathEvent;

    int amount;
    private void Start()
    {
        HP = maxHP;
        bloodied = false;
    }
    public void HealDamage(int heal)
    {
        if (HP < maxHP)
        {
            HP += heal;
        }
        if (HP > maxHP)
        {
            HP = maxHP;
        }
    }
    public void TakeDamage(int damage)
    {
        HP -= damage;

        if ((HP <= maxHP/2) && !bloodied)
        {
            bloodiedEvent?.Invoke();

            bloodied = true;
        }

        if (HP <= 0)
        {
            deathEvent?.Invoke();
        }
    }
}
