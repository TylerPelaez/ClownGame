using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;


[RequireComponent(typeof(NavMeshAgent))]
public class BaseEnemyAI : MonoBehaviour
{
    [SerializeField]
    private float aggroRange = 5;
    private float aggroRangeSq, dropAggroRangeSq;

    [Header("Debug")]
    
    [SerializeField]
    private State state = State.Idle;
    
    
    private NavMeshAgent navMeshAgent;
    private GameObject player;

    [SerializeField]
    private Animator animator;

    private static readonly int Walking = Animator.StringToHash("Walking");


    // Start is called before the first frame update
    void Start()
    {
        aggroRangeSq = aggroRange * aggroRange;
        dropAggroRangeSq = (2 * aggroRange) * (2 * aggroRange);
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
                Attack();
                break;
        }
    }


    private bool IsPlayerInDistanceSq(float distanceSq)
    {
        return Vector3.SqrMagnitude(player.transform.position - transform.position) < distanceSq;
    }
    
    private void Idle()
    {
        if (IsPlayerInDistanceSq(aggroRangeSq))
        {
            NavMeshPath navMeshPath = navMeshAgent.path;
            if (navMeshAgent.CalculatePath(player.transform.position, navMeshPath))
            {
                EnterChase();
            }
        }
    }

    private void EnterIdle()
    {
        state = State.Idle;
        
        navMeshAgent.SetDestination(gameObject.transform.position);
        navMeshAgent.isStopped = true;
        animator.SetBool(Walking, false);
    }

    private void EnterChase()
    {
        state = State.Chase;
        navMeshAgent.isStopped = false;
    }
    
    private void Chase()
    {
        if (!IsPlayerInDistanceSq(dropAggroRangeSq))
        {
            EnterIdle();
        }
        
        navMeshAgent.SetDestination(player.transform.position);
        
        animator.SetBool("Walking", navMeshAgent.velocity != Vector3.zero);
    }

    private void Attack()
    {
        
    }

    public void OnDeath()
    {
        Destroy(gameObject);
    }

    enum State
    {
        Idle,
        Chase,
        Attack,
    }
}
