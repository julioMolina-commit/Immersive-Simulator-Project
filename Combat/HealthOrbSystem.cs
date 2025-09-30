using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthOrbSystem : MonoBehaviour
{
    [SerializeField] private int healAmount;
    [SerializeField] private float distance;
    bool isInRange;
    Transform orbTarget;
    private void Start()
    {
        isInRange = false;

        orbTarget = GameObject.Find("Player Body").transform;
    }
    private void Update()
    {
        if (isInRange)
        {
            distance -= Time.deltaTime;

            transform.position = Vector3.Lerp(orbTarget.position, transform.position, distance);

            if (Vector3.Distance(orbTarget.position, transform.position) <= 0.5f)
            {
                orbTarget.gameObject.GetComponent<HPTracker>().HealDamage(healAmount);

                Destroy(this.gameObject);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;

            distance = 1;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
        }
    }
}
