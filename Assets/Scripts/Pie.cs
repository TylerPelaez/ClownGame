using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Pie : MonoBehaviour
{
    [SerializeField]
    private bool unmoving = false;
    
    [SerializeField]
    private float speed = 100f;

    [SerializeField]
    private LayerMask destroyMask;
    
    
    
    
    public UnityEvent OnHit;


    public Vector3? travelDirection = null;
    public GameObject ignoreCollisions;
    
    private void Start()
    {
        if (!unmoving)
            StartCoroutine(AwaitDeath());
    }

    private void FixedUpdate()
    {
        if (!unmoving)
            transform.position += (travelDirection ?? transform.forward) * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ( other.gameObject != ignoreCollisions && destroyMask == (destroyMask | (1 << other.gameObject.layer)))
        {
            OnHit?.Invoke();
            FindObjectOfType<AudioManager>().Play("PieHit");
            Destroy(gameObject);
        }
    }

    private IEnumerator AwaitDeath()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
