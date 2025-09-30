using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDeactivateObjects : MonoBehaviour
{
    [SerializeField] private GameObject[] objectList;
    public void ActivateList()
    {
        for (int i = 0; i < objectList.Length; i++)
        {
            objectList[i].SetActive(true);
        }
    }
    public void DeactivateList()
    {
        for (int i = 0; i < objectList.Length; i++)
        {
            objectList[i].SetActive(false);
        }
    }
}
