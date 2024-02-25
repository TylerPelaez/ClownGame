using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerHurtbox : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;
    
    public UnityEvent<int, DamageType> OnHurt;

    [SerializeField]
    private float invincibilityTime = 0.5f;
    private bool invincible;

    public void OnTriggerEnter(Collider other)
    {
        HandleTrigger(other);
    }

    public void OnTriggerStay(Collider other)
    {
        HandleTrigger(other);
    }

    private void HandleTrigger(Collider other)
    {
        if (invincible)
            return;
            
        if (layerMask == (layerMask | (1 << other.gameObject.layer)))
        {
            Debug.Log($"Playerinvoke {other.gameObject.name}");

            // TODO: not hardcode this
            OnHurt?.Invoke(1, DamageType.Stomp);
            invincible = true;
            StartCoroutine(WaitForInvincibility());
        }
    }

    private IEnumerator WaitForInvincibility()
    {
        yield return new WaitForSeconds(invincibilityTime);
        invincible = false;
    }

    public void OnDeath()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
