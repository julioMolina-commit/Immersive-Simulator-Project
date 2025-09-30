using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//TODO: Block interaction when dialogue is active and polish the moveObject
public class InteractionManager : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionRange;
    [SerializeField] private Transform interactionOrigin;

    [Header("Pickup Settings")]
    [SerializeField] Transform holdArea;
    private GameObject heldObj;
    private Rigidbody heldObjRB;
    [SerializeField] private Image interactionIcon;

    [Header("Physics Parameters")]
    [SerializeField] private float pickupForce = 150.0f;
    [SerializeField] private float throwForce = 100.0f;

    [Header("Visual Parameters")]
    [SerializeField] private float iconRevealTime;
    float iconRevealClock;

    private bool isInDialogue;
    private void Start()
    {
        iconRevealClock = 0;

        GameEvents.EnterDialogue.AddListener(EnterDialogueCheck);
        GameEvents.ExitDialogue.AddListener(ExitDialogueCheck);
    }
    private void Update()
    {
        Interaction();
        InteractionIconReveal();
    }
    //TODO: Allow interact when holding object as [void UseObject] or similar
    private void Interaction()
    {
        Ray r = new Ray(interactionOrigin.position, interactionOrigin.forward);

        if (Physics.Raycast(r, out RaycastHit hitInfo, interactionRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out InteractionObject interactObj))
            {
                if (interactObj == true)
                {
                    if (heldObj == null && isInDialogue == false)
                        iconRevealClock = iconRevealTime;

                    if (Input.GetKeyDown(KeyCode.E) && isInDialogue == false)
                    {
                        if (interactObj.GetComponent<InteractionObject>().canPickUp == false)
                            interactObj.Interact();
                        else if (interactObj.GetComponent<InteractionObject>().canPickUp == true)
                        {
                            if (heldObj == null)
                                PickupObject(hitInfo.transform.gameObject);
                            else
                                DropObject();
                        }
                    }
                }
            }
        }

        if (heldObj != null)
        {
            MoveObject();
            ThrowObject();
            UseObject();
        }
    }
    void MoveObject()
    {
        if (Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1f)
        {
            Vector3 moveDirection = (holdArea.position - heldObj.transform.position);
            heldObjRB.AddForce(moveDirection * pickupForce);
        }
    }
    void ThrowObject()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 throwDirection = (holdArea.forward);
            heldObjRB.AddForce(throwDirection * throwForce * 10);
            heldObj.AddComponent<ThrownSystem>();


            DropObject();
        }
    }
    void UseObject()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Ray r = new Ray(interactionOrigin.position, interactionOrigin.forward);

            if (Physics.Raycast(r, out RaycastHit hitInfo, interactionRange))
            {
                if (hitInfo.collider.gameObject.TryGetComponent(out InteractionObject interactObj))
                {
                    interactObj.Interact();
                }
            }
        }
    }

    void PickupObject(GameObject pickObj)
    {
        if (pickObj.GetComponent<Rigidbody>())
        {
            heldObjRB = pickObj.GetComponent<Rigidbody>();
            heldObjRB.useGravity = false;
            heldObjRB.drag = 10;
            heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;

            heldObjRB.transform.parent = holdArea;
            heldObj = pickObj;

            GameEvents.EnterPickUpMode.Invoke();
        }
    }
    void DropObject()
    {
        heldObjRB.useGravity = true;
        heldObjRB.drag = 1;
        heldObjRB.constraints = RigidbodyConstraints.None;

        heldObjRB.transform.parent = null;
        heldObj = null;

        GameEvents.ExitPickUpMode.Invoke();
    }
    private void InteractionIconReveal()
    {
        interactionIcon.color = new Color(1, 1, 1, iconRevealClock / iconRevealTime);

        if (iconRevealClock >= 0)
        {
            iconRevealClock -= Time.deltaTime;
        }
    }

    void EnterDialogueCheck()
    {
        isInDialogue = true;
    }
    void ExitDialogueCheck()
    {
        isInDialogue = false;
    }
}