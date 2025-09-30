using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgrammedDestroy : MonoBehaviour
{
    [SerializeField] private float waitTime;
    private void Update()
    {
        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }
        else
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            this.gameObject.SetActive(false);
        }
    }
}
