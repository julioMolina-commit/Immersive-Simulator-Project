using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headbobber : MonoBehaviour
{
    [SerializeField] private float headbobSpeed;
    [SerializeField] private float headbobAmount;
    [SerializeField] private PlayerMovement pm;
    float timer = 0;
    private void Headbob()
    {
        if (pm.grounded == true && Input.GetKey(KeyCode.LeftShift))
        {
            timer += Time.deltaTime * headbobSpeed;

            this.transform.localPosition = new Vector3(transform.localPosition.x, 0 + Mathf.Sin(timer) * headbobAmount, transform.localPosition.z);
        }
    }
}
