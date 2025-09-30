using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownSystem : MonoBehaviour
{
    int thrownDamage = 45;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform != this.transform)
        {
            if (collision.gameObject.layer == 6)
            {
                if (collision.gameObject.TryGetComponent(out StunSystem stunS))
                {
                    stunS.OnKnockback(-collision.transform.forward * thrownDamage * 10);
                }

                collision.gameObject.GetComponent<HPTracker>().TakeDamage(thrownDamage);

                GameEvents.SlowTime.Invoke();
            }

            if (GetComponent<HPTracker>())
            {
                GetComponent<HPTracker>().TakeDamage(thrownDamage);
            }
        }

        Destroy(this.GetComponent<ThrownSystem>());
    }
}
