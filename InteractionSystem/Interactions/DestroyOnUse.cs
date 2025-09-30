using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnUse : MonoBehaviour
{
    [SerializeField] GameObject[] objectsToDetroy;
    public void Activate()
    {
        for (int i = 0; i <= objectsToDetroy.Length; i++)
        {
            Destroy(objectsToDetroy[i]);
        }
    }
}
