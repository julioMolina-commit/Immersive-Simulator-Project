using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgrammedInstantiate : MonoBehaviour
{
    [SerializeField] private GameObject[] instantiateArray;
    public void OnInstantiate()
    {
        for (int i = 0; i < instantiateArray.Length; i++)
        {
            Instantiate(instantiateArray[i], transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        }
    }
}
