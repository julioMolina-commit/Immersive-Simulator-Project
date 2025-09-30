using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PunchSystem : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private Transform originHits, mainCamera;
    [SerializeField] private bool canRightPunch, canLeftPunch, canKick, isInPickUp, isInDialogue;
    [SerializeField] private int punchDamage, kickDamage;
    [SerializeField] private float punchCooldown, kickCooldown;
    [SerializeField] private LayerMask target;
    [Header("Cosmetics")]
    [SerializeField] private AudioSource playerSource;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private ParticleSystem impactSystem;
    [SerializeField] private ParticleSystem bloodSystem;
    [Header("Animation")]
    [SerializeField] private Animator armAnimator;
    const string Arm_Idle = "Idle";
    const string Arm_RightPunch = "RightPunch";
    const string Arm_LeftPunch = "LeftPunch";
    [Header("HUD")]
    [SerializeField] private GameObject armsHolder;
    [SerializeField] private Image crosshair, indicationImage;
    [SerializeField] private float exitCombatWait;
    float exitCombatClock, indicationClock;

    private string currentState;

    bool idleDelay;
    private void Start()
    {
        canRightPunch = true;
        canLeftPunch = true;
        canKick = true;


        GameEvents.EnterDialogue.AddListener(EnterDialogueCheck);
        GameEvents.ExitDialogue.AddListener(ExitDialogueCheck);

        GameEvents.EnterPickUpMode.AddListener(EnterPickUpCheck);
        GameEvents.ExitPickUpMode.AddListener(ExitPickUpCheck);
    }
    private void Update()
    {
        CrosshairClock();
        ConfirmHitClock();

        if (Input.GetKeyDown(KeyCode.Mouse0) && canRightPunch && !isInDialogue && !isInPickUp && !idleDelay && !Input.GetKeyDown(KeyCode.Mouse1))
        {
            exitCombatClock = exitCombatWait;

            Punch();

            ChangeAnimation(Arm_RightPunch);

            idleDelay = true;

            Invoke(nameof(IdleDelayRefresh), 0.25f);

            canRightPunch = false;

            Invoke(nameof(RightPunchReload), punchCooldown);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && canLeftPunch && !isInDialogue && !isInPickUp && !idleDelay && !Input.GetKeyDown(KeyCode.Mouse0))
        {
            exitCombatClock = exitCombatWait;

            Punch();

            ChangeAnimation(Arm_LeftPunch);

            idleDelay = true;

            Invoke(nameof(IdleDelayRefresh), 0.25f);

            canLeftPunch = false;

            Invoke(nameof(LeftPunchReload), punchCooldown);
        }

        if (Input.GetKeyDown(KeyCode.F) && canKick && !isInDialogue && !isInPickUp)
        {
            Kick();

            canKick = false;

            Invoke(nameof(KickReload), kickCooldown);
        }

        if (!idleDelay)
        {
            ChangeAnimation(Arm_Idle);
        }
    }
    private void ShowArms()
    {
        armsHolder.SetActive(true);
    }
    private void HideArms()
    {
        armsHolder.SetActive(false);
    }
    private void IdleDelayRefresh()
    {
        idleDelay = false;
    }
    private void Punch()
    {
        RaycastHit hit;

        if (Physics.Raycast(originHits.position, originHits.forward, out hit, 3f, target))
        {
            playerSource.PlayOneShot(attackSound);

            if (hit.collider.TryGetComponent(out StunSystem stunS))
            {
                stunS.OnKnockback(originHits.forward * punchDamage * 10);
            }

            if (hit.collider.gameObject.TryGetComponent(out Rigidbody rb) && !hit.collider.gameObject.GetComponent<StunSystem>())
            {
                rb.AddForce(originHits.forward * 1f, ForceMode.Impulse);
                rb.AddForce(originHits.up * 1f, ForceMode.Impulse);
            }

            if (hit.collider.gameObject.GetComponent<HPTracker>())
            {
                hit.collider.gameObject.GetComponent<HPTracker>().TakeDamage(punchDamage);

                indicationClock = 1f;

                GameEvents.SlowTime.Invoke();
            }

            if (hit.collider.gameObject.GetComponent<StunSystem>())
            {
                bloodSystem.transform.position = hit.point + new Vector3(Random.Range(0, 0.1f), Random.Range(0, 0.1f), 0.05f);
                bloodSystem.transform.rotation.SetLookRotation(mainCamera.position);
                bloodSystem.Emit(100);
            }

            impactSystem.transform.position = hit.point + new Vector3(Random.Range(0, 0.1f), Random.Range(0, 0.1f), 0.05f);
            impactSystem.Emit(5);

            impactSystem.Stop();
            bloodSystem.Stop();
        }
        else if (Physics.Raycast(originHits.position, originHits.forward, out hit, 4f))
        {
            playerSource.PlayOneShot(attackSound);

            impactSystem.transform.position = hit.point + new Vector3(Random.Range(0, 0.1f), Random.Range(0, 0.1f), 0.05f);
            impactSystem.Emit(15);
        }
    }
    private void Kick()
    {
        RaycastHit hit;

        if (Physics.Raycast(originHits.position, originHits.forward, out hit, 4f, target))
        {
            playerSource.PlayOneShot(attackSound);

            if (hit.collider.TryGetComponent(out StunSystem stunS))
            {
                stunS.OnKnockback(originHits.forward * kickDamage * 10);
            }

            if (hit.collider.gameObject.TryGetComponent(out Rigidbody rb) && !hit.collider.gameObject.GetComponent<StunSystem>())
            {
                rb.AddForce(originHits.forward * 5f, ForceMode.Impulse);
                rb.AddForce(originHits.up * 5f, ForceMode.Impulse);
            }

            if (hit.collider.gameObject.GetComponent<HPTracker>())
            {
                hit.collider.gameObject.GetComponent<HPTracker>().TakeDamage(kickDamage);

                indicationClock = 1f;

                GameEvents.SlowTime.Invoke();
            }
            
            if (hit.collider.gameObject.GetComponent<StunSystem>())
            {
                bloodSystem.transform.position = hit.point + new Vector3(Random.Range(0, 0.1f), Random.Range(0, 0.1f), 0.05f);
                bloodSystem.transform.rotation.SetLookRotation(mainCamera.position);
                bloodSystem.Emit(100);
            }

            impactSystem.transform.position = hit.point + new Vector3(Random.Range(0, 0.1f), Random.Range(0, 0.1f), 0.05f);
            impactSystem.Emit(15);
        }
        else if (Physics.Raycast(originHits.position, originHits.forward, out hit, 4f))
        {
            playerSource.PlayOneShot(attackSound);

            impactSystem.transform.position = hit.point + new Vector3(Random.Range(0, 0.1f), Random.Range(0, 0.1f), 0.05f);
            impactSystem.Emit(15);
        }

        impactSystem.Stop();
        bloodSystem.Stop();
    }
    private void RightPunchReload()
    {
        canRightPunch = true;
    }
    private void LeftPunchReload()
    {
        canLeftPunch = true;
    }
    private void KickReload()
    {
        canKick = true;
    }
    private void CrosshairClock()
    {
        if (exitCombatClock >= 0)
        {
            ShowArms();

            exitCombatClock -= Time.deltaTime;
        }
        else
        {
            HideArms();
        }

        crosshair.color = new Color(1, 1, 1, exitCombatClock / 0.1f);
    }
    private void ConfirmHitClock()
    {
        if (indicationClock >= 0)
        {
            indicationClock -= Time.deltaTime;
        }

        indicationImage.color = new Color(1, 1, 1, indicationClock / 0.05f);
        indicationImage.gameObject.transform.localScale = new Vector3(indicationClock / 0.25f, indicationClock / 0.25f);
    }
    void EnterDialogueCheck()
    {
        isInDialogue = true;
        exitCombatClock = 0;
    }
    void ExitDialogueCheck()
    {
        isInDialogue = false;
    }
    void EnterPickUpCheck()
    {
        isInPickUp = true;
        exitCombatClock = 0;
    }
    void ExitPickUpCheck()
    {
        isInPickUp = false;
    }
    void ChangeAnimation(string newState)
    {
        if (currentState == newState || exitCombatClock <= 0) return;

        armAnimator.Play(newState);

        currentState = newState;
    }
}
