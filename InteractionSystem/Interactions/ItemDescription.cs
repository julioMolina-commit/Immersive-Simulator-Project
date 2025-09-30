using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDescription : MonoBehaviour
{
    [SerializeField] private ItemDescriptionManager descriptionManager;
    [SerializeField] private string description;
    public void Activate()
    {
        descriptionManager.DisplayDescription(description);
    }
}
