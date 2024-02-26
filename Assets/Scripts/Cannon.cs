using UnityEngine;


[RequireComponent(typeof(Animator))]
public class Cannon : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private Animator idleloop;

    [SerializeField]
    private bool floatLoop = true;
    
    [SerializeField]
    private float fireDelay = 3.0f;

    private float lastFireTime;
    private static readonly int Fire1 = Animator.StringToHash("Fire");

    [SerializeField]
    private GameObject piePrefab;

    [SerializeField]
    private GameObject pieOrigin;
    
    void Fire()
    {
        lastFireTime = Time.time;
        
        animator.SetBool(Fire1, true);
        idleloop.speed = 0;
    }

    void LaunchPie()
    {
        var instance = Instantiate(piePrefab, pieOrigin.transform.position, Quaternion.identity);
        instance.transform.forward = pieOrigin.transform.forward;
    }

    void FireComplete()
    {
        idleloop.speed = 1;
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if (!floatLoop)
        {
            idleloop.SetFloat("FloatSpeed", 0f);            
        }
    }

    private void Update()
    {
        if (animator.GetBool(Fire1))
        {
            animator.SetBool(Fire1, false);
        }
        
        if (Time.time - lastFireTime > fireDelay)
        {
            Fire();
        }
    }
}
