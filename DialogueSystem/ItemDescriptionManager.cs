using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemDescriptionManager : MonoBehaviour
{
    [SerializeField] private GameObject descriptionParent;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private float displayTime;
    private void Start()
    {
        descriptionParent.SetActive(false);
    }
    public void DisplayDescription(string description)
    {
        StartCoroutine(DescriptionDisplay(description));
    }
    IEnumerator DescriptionDisplay(string description)
    {
        descriptionParent.SetActive(true);
        descriptionText.text = description;

        yield return new WaitForSeconds(displayTime);

        descriptionParent.SetActive(false);

        yield return null;
    }
}
