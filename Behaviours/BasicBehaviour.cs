using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DurgeBehaviour : MonoBehaviour
{
    private GameObject playerObject;
    private Animator animatorBasic;

    [SerializeField] private NavMeshAgent agent;

    public LayerMask isGround, isPlayer;

    //Movement
    Vector3 walkPoint;
    bool walkPointSet;
    [SerializeField] private float walkPointRange;

    //Attack
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private int attackDamage;
    bool alreadyAttacked;

    //States
    [SerializeField] private float sightRange, attackRange, attackDelay;
    public bool playerInSightRange, playerInAttackRange, playerIsLocated, isAttacking, isResting, isStunned;

    [Header("AnimationStates")]
    const string Monster_Idle = "Idle";
    const string Monster_Walk = "Walk";
    const string Monster_Detect = "Detect";
    const string Monster_Attack = "Attack";
    const string Monster_Stun = "Stun";

    private string currentState;
    [Header("Sounds")]
    private AudioSource monsterSource;
    private AudioClip currentSound;
    [SerializeField] private AudioClip Monster_Idle_Sound;
    [SerializeField] private AudioClip Monster_Detect_Sound;
    [SerializeField] private AudioClip Monster_Attack_Sound;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerObject = GameObject.Find("Player Body");
        animatorBasic = GetComponent<Animator>();
        monsterSource = GetComponent<AudioSource>();

        monsterSource.Play();
    }
    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, isPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, isPlayer);

        if (!agent.isActiveAndEnabled) Stunned();

        if (!playerInSightRange && !playerInAttackRange && isResting && !isStunned) Idle();
        if (!playerInSightRange && !playerInAttackRange && !isResting && !isStunned) Patrolling();
        if (playerInSightRange && !playerInAttackRange && !playerIsLocated && !isStunned) LocatePlayer();
        if (playerInSightRange && !playerInAttackRange && playerIsLocated && !isAttacking && !isStunned) MoveTowardsPlayer();
        if (playerInSightRange && playerInAttackRange && !isAttacking && !isStunned) AttackPlayer();

        if (!playerInSightRange)
            playerIsLocated = false;
    }
    private void Stunned()
    {
        ChangeAnimation(Monster_Stun);

        isStunned = true;

        Invoke(nameof(StunReset), 2f);
    }
    private void StunReset()
    {
        isStunned = false;
    }
    private void Idle()
    {
        ChangeAnimation(Monster_Idle);
        PlaySound(Monster_Idle_Sound);

        agent.SetDestination(transform.position);
    }
    private void Patrolling()
    {
        ChangeAnimation(Monster_Walk);
        PlaySound(Monster_Idle_Sound);

        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint Reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;

        isResting = true;

        Invoke(nameof(RestReset), 1.5f);
    }
    private void RestReset()
    {
        isResting = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, isGround))
        {
            walkPointSet = true;
        }
    }
    private void LocatePlayer()
    {
        ChangeAnimation(Monster_Detect);
        PlaySound(Monster_Detect_Sound);

        agent.SetDestination(transform.position);

        Invoke(nameof(MoveTowardsPlayer), 4.5f);
    }
    private void MoveTowardsPlayer()
    {
        ChangeAnimation(Monster_Walk);
        PlaySound(Monster_Idle_Sound);

        playerIsLocated = true;

        agent.SetDestination(playerObject.transform.position);
    }
    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        ChangeAnimation(Monster_Idle);

        PlaySound(Monster_Idle_Sound);

        Vector3 targetPosition = new Vector3(playerObject.transform.position.x, this.transform.position.y, playerObject.transform.position.z);
        transform.LookAt(targetPosition);

        if (!alreadyAttacked)
        {
            isAttacking = true;
            ChangeAnimation(Monster_Attack);
            Invoke(nameof(Attack), attackDelay);
        }
    }
    private void Attack()
    {
        agent.SetDestination(transform.position);

        PlaySound(Monster_Attack_Sound);

        RaycastHit hit;

        if ((Physics.Raycast(transform.position, transform.forward, out hit, attackRange, isPlayer) && !alreadyAttacked) && !isStunned)
        {
            hit.transform.gameObject.GetComponent<HPTracker>().TakeDamage(attackDamage);
            alreadyAttacked = true;
        }

        Invoke(nameof(AttackInPlace), 1.7f);
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }
    private void AttackInPlace()
    {
        isAttacking = false;
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    void ChangeAnimation(string newState)
    {
        if (currentState == newState) return;

        animatorBasic.Play(newState);

        currentState = newState;
    }
    void PlaySound(AudioClip newSound)
    {
        if (currentSound == newSound) return;

        monsterSource.clip = newSound;

        monsterSource.Play();

        currentSound = newSound;
    }
}