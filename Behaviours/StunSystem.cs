using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StunSystem : MonoBehaviour
{
    private Rigidbody rb;
    private NavMeshAgent navAg;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        navAg = GetComponent<NavMeshAgent>();
    }
    public void OnKnockback(Vector3 knockback)
    {
        StartCoroutine(ApplyKnockback(knockback));
    }
    private IEnumerator ApplyKnockback(Vector3 knockback)
    {
        yield return null;

        navAg.enabled = false;
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.AddForce(knockback);

        yield return new WaitForFixedUpdate();
        yield return new WaitUntil(() => rb.velocity.magnitude < 0.05f);
        yield return new WaitForSeconds(0.25f);

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;
        rb.isKinematic = false;
        navAg.Warp(transform.position);
        navAg.enabled = true;

        yield return null;
    }
}
