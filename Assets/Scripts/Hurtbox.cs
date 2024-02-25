using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Hurtbox : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private List<DamageType> damageTypeImmunities;

    public UnityEvent<int, DamageType> OnHurt;

    [SerializeField]
    private MeshRenderer meshRenderer;
    
    [SerializeField]
    private float invincibilityTime = 0.2f;
    private bool invincible;
    private const float InvincibilityBlinkStep = 0.1f;

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
            var hitbox = other.gameObject.GetComponent<Hitbox>();
            if (damageTypeImmunities.Contains(hitbox.DamageType))
            {
                return;
            }
            
            OnHurt?.Invoke(hitbox.DamageAmount, hitbox.DamageType);
            invincible = true;
            StartCoroutine(WaitForInvincibility());
        }
    }

    private IEnumerator WaitForInvincibility()
    {
        float totalTime = 0;
        while (true)
        {
            meshRenderer.enabled = !meshRenderer.enabled;
            totalTime += InvincibilityBlinkStep;
            if (totalTime >= invincibilityTime)
            {
                break;
            }

            yield return new WaitForSeconds(InvincibilityBlinkStep);
        }
        meshRenderer.enabled = true;
        invincible = false;
    }
    
    public void OnPlayerDeath()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
