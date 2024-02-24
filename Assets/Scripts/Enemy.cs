using System;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;
    
    public UnityEvent<int, DamageType> OnHurt;

    public void OnTriggerEnter(Collider other)
    {
        if (layerMask == (layerMask | (1 << other.gameObject.layer)))
        {
            // TODO: not hardcode this
            OnHurt?.Invoke(1, DamageType.Stomp);
        }
    }

    public void OnDeath()
    {
        // TODO: Animate
        Destroy(gameObject);
    }
    
}
