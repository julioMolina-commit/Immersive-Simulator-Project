using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    [SerializeField] private int explosionDamage;
    [SerializeField] private float explosionRadius;
    [SerializeField] private LayerMask target;
    public List<Collider> TriggerList = new List<Collider>();

    bool readyToDestroy;
    private void Start()
    {
        GetComponent<SphereCollider>().radius = explosionRadius;
    }
    public void OnExplosion()
    {
        for (int i = 0; i < TriggerList.Count; i++)
        {
            if (TriggerList[i].gameObject.GetComponent<HPTracker>().HP <= 0)
            {
                TriggerList.RemoveAt(i);
                break;
            }

            if (TriggerList[i].gameObject != this.gameObject && TriggerList[i].GetComponent<ExplosionDamage>())
            {
                TriggerList[i].gameObject.GetComponent<HPTracker>().TakeDamage(explosionDamage);
                if (TriggerList[i].gameObject.GetComponent<StunSystem>())
                {
                    Vector3 dir = (this.transform.position - TriggerList[i].transform.position).normalized;

                    TriggerList[i].gameObject.GetComponent<StunSystem>().OnKnockback(dir * explosionDamage * 10);
                }

                TriggerList.RemoveAt(i);

                if (i == TriggerList.Count)
                    readyToDestroy = true;

                break;
            }
        }
        if (readyToDestroy)
            Destroy(this.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!TriggerList.Contains(other) && (other.gameObject.layer == 6 || other.gameObject.layer == 7) && !(other.gameObject.GetComponent<HPTracker>().HP <= 0))
        {
            TriggerList.Add(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (TriggerList.Contains(other))
        {
            TriggerList.Remove(other);
        }
    }
}