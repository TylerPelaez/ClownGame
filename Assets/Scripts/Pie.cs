using System.Collections;
using UnityEngine;

public class Pie : MonoBehaviour
{
    [SerializeField]
    private float speed = 100f;

    [SerializeField]
    private LayerMask destroyMask;
    

    private void Start()
    {
        StartCoroutine(AwaitDeath());
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (destroyMask == (destroyMask | (1 << other.gameObject.layer)))
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator AwaitDeath()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
