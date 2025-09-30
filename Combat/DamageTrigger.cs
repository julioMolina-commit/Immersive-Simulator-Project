using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] bool destroyAfter;
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<HPTracker>().TakeDamage(damage);
        GetComponent<AudioSource>().Play();
        GetComponent<ParticleSystem>().Emit(5);

        if (destroyAfter)
        {
            Invoke("DestroyTheObject", 0.5f);
        }
    }
    private void DestroyTheObject()
    {
        Destroy(this.gameObject);
    }
}
