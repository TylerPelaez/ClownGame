using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;


[RequireComponent(typeof(NavMeshAgent))]
public class BaseEnemyAI : MonoBehaviour
{
    [SerializeField]
    private float aggroRange = 5;
    private float aggroRangeSq, dropAggroRangeSq;

    [SerializeField]
    private float attackRange = 1;
    private float attackRangeSq;

    [SerializeField]
    private Animator animator;
    
    
    [SerializeField]
    private bool ranged;
    
    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private GameObject projectileOrigin;

    [SerializeField]
    private float attackCooldown = 1;

    
    [SerializeField]
    private float lastLeftAttackStateTime;


    [SerializeField]
    private GameObject meleeHitboxObject;

    [SerializeField]
    private float turnSpeed = 5f;
    
    
    [Header("Debug")]
    
    [SerializeField]
    private State state = State.Idle;
    
    
    private NavMeshAgent navMeshAgent;
    private GameObject player;
    
    private static readonly int Walking = Animator.StringToHash("Walking");
    private static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");
    private static readonly int Attacking = Animator.StringToHash("Attacking");


    private int attackStateFrameCounts = 0;
    private static readonly int Dead = Animator.StringToHash("Dead");

    // Start is called before the first frame update
    void Start()
    {
        aggroRangeSq = aggroRange * aggroRange;
        dropAggroRangeSq = (2 * aggroRange) * (2 * aggroRange);
        attackRangeSq = attackRange * attackRange;
        navMeshAgent = GetComponent<NavMeshAgent>();
        
        player = GameObject.FindWithTag("Player");
        Assert.IsNotNull(player, "Enemy could not find object with player tag!");

        // In case the debug preview makes it accidentally get changed 
        state = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Idle:
                Idle();
                break;
            case State.Chase:
                Chase();
                break;
            case State.Attack:
                AttackState();
                break;
            case State.Death:
                break;
        }
    }


    private bool IsPlayerInDistanceSq(float distanceSq)
    {
        var aimTarget = player.GetComponent<PlayerController>().enemyAimTarget.transform.position;
        aimTarget.y = transform.position.y;
        
        return Vector3.SqrMagnitude(aimTarget - transform.position) < distanceSq;
    }
    
    private void Idle()
     {
         if (animator.GetBool(Attacking))
         {
             animator.SetBool(Attacking, false);
         }

         
        if (IsPlayerInDistanceSq(aggroRangeSq) && !IsPlayerInDistanceSq(attackRangeSq))
        {
            NavMeshPath navMeshPath = navMeshAgent.path;
            if (navMeshAgent.CalculatePath(player.transform.position, navMeshPath))
            {
                EnterChase();
                return;
            }
        }

        if (ShouldAttack())
        {
            EnterAttack();
            return;
        }
        else if (IsPlayerInDistanceSq(attackRangeSq))
        {
            var lookTarget = player.transform.position;
            lookTarget.y = transform.position.y;

            lookTarget = Vector3.Lerp(transform.position + transform.forward, lookTarget, turnSpeed*Time.deltaTime);
            
            transform.LookAt(lookTarget);
        }

     }

    private void EnterIdle()
    {
        state = State.Idle;
        
        
        navMeshAgent.SetDestination(gameObject.transform.position);
        navMeshAgent.isStopped = true;
        animator.SetBool(Walking, false);
        animator.SetFloat(MoveSpeed, 0);
    }

    private void EnterChase()
    {
        state = State.Chase;
        navMeshAgent.isStopped = false;
    }
    
    private void Chase()
    {
        if (animator.GetBool(Attacking))
        {
            animator.SetBool(Attacking, false);
        }

        if (ShouldAttack())
        {
            EnterAttack();
            return;
        }
        else if (IsPlayerInDistanceSq(attackRangeSq))
        {
            var lookTarget = player.transform.position;
            lookTarget.y = transform.position.y;

            lookTarget = Vector3.Lerp(transform.position + transform.forward, lookTarget, turnSpeed*Time.deltaTime);
            animator.SetBool("Walking", false);
            animator.SetFloat(MoveSpeed, 0);
            navMeshAgent.SetDestination(transform.position);
            transform.LookAt(lookTarget);
            return;
        }
        
        if (!IsPlayerInDistanceSq(dropAggroRangeSq))
        {
            EnterIdle();
            return;
        }

        navMeshAgent.SetDestination(player.transform.position);
        
        animator.SetBool("Walking", navMeshAgent.velocity != Vector3.zero);
        animator.SetFloat(MoveSpeed, navMeshAgent.velocity.magnitude);
    }

    private void AttackState()
    {
        attackStateFrameCounts += 1;
        if (animator.GetBool(Attacking))
        {
            animator.SetBool(Attacking, false);
        }

        if (attackStateFrameCounts > 2 && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !animator.GetNextAnimatorStateInfo(0).IsName("Attack"))
        {
            Debug.Log("EndAttack");
            if (!IsPlayerInDistanceSq(attackRangeSq))
            {
                EnterChase();
                return;
            }
            else
            {
                EnterIdle();
                return;
            }
        }
    }

    public void OnDoAttack()
    {
        GameObject aimTarget = player.GetComponent<PlayerController>().enemyAimTarget;
        
        if (ranged)
        {
            var instance = Instantiate(projectilePrefab, projectileOrigin.transform.position, Quaternion.identity);
            var aimTargetPos = aimTarget.transform.position;
            aimTargetPos.y = projectileOrigin.transform.position.y;
            var directionToPlayer = (aimTargetPos - projectileOrigin.transform.position).normalized;
            instance.transform.forward = directionToPlayer;
            instance.transform.Rotate(Vector3.right, 90, Space.Self);
            instance.GetComponent<Pie>().travelDirection = directionToPlayer;
        }
        else
        {
            meleeHitboxObject.SetActive(true);
            StartCoroutine(DelayDisableMeleeHitbox());
        }
    }

    private IEnumerator DelayDisableMeleeHitbox()
    {
        yield return new WaitForSeconds(.1f);
        DisableMeleeHitbox();
    }
    
    private void EnterAttack()
    {
        Debug.Log("EnterAttack");
        lastLeftAttackStateTime = Time.time;
        state = State.Attack;
        navMeshAgent.SetDestination(gameObject.transform.position);
        navMeshAgent.isStopped = true;
        animator.SetBool(Attacking, true);
        animator.SetBool(Walking, false);
        animator.SetFloat(MoveSpeed, 0);
        attackStateFrameCounts = 0;
    }

    public void OnDeath()
    {
        state = State.Death;

        navMeshAgent.SetDestination(gameObject.transform.position);
        navMeshAgent.isStopped = true;
        animator.SetBool(Dead, true);
        animator.SetBool(Walking, false);
        animator.SetFloat(MoveSpeed, 0);
        animator.SetBool(Attacking, false);
        
        if (meleeHitboxObject != null)
            meleeHitboxObject.SetActive(false);
        var hurtbox = GetComponent<Hurtbox>();
        var hitbox = GetComponent<Hitbox>();
        var collider = GetComponent<Collider>();
        
        if (hurtbox == null)
            hurtbox = gameObject.GetComponentInChildren<Hurtbox>();
        if (hitbox == null)
            hitbox = gameObject.GetComponentInChildren<Hitbox>();
        if (collider == null)
            collider = gameObject.GetComponentInChildren<Collider>();
        
        
        if (hurtbox != null)
            Destroy(hurtbox);
        
        if (hitbox != null)
            Destroy(hitbox);

        if (collider != null)
        {
            collider.enabled = false;
        }
    }

    public void OnDeathComplete()
    {
        Destroy(gameObject);
    }
    

    public bool ShouldAttack()
    {
        if (Time.time - lastLeftAttackStateTime < attackCooldown || !IsPlayerInDistanceSq(attackRangeSq))
        {
            return false;
        }
        
        var playerPos = player.transform.position;
        playerPos.y = transform.position.y;
        if (!ranged && Vector3.Dot(transform.forward, (playerPos - transform.position).normalized) < 0.7f)
        {
            return false;
        }

        return true;
    }
    
    enum State
    {
        Idle,
        Chase,
        Attack,
        Death,
    }

    public void DisableMeleeHitbox()
    {
        meleeHitboxObject.SetActive(false);
    }
}
