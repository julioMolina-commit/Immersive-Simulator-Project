using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionObject : MonoBehaviour
{
    public bool canPickUp;
    [SerializeField] UnityEvent interactionEvent;
    public void Interact()
    {
        if (!canPickUp)
        {
            interactionEvent?.Invoke();
        }
    }
}
