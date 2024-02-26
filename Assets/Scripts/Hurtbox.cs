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
    private Renderer meshRenderer, secondaryMeshRenderer;
    
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
        var hitbox = other.gameObject.GetComponent<Hitbox>();
        if (hitbox == null)
        {
            return;
        }
        
        if (damageTypeImmunities.Contains(hitbox.DamageType))
        {
            return;
        }

        // Love hurts!
        if (hitbox.DamageType == DamageType.Healing)
        {
            OnHurt?.Invoke(hitbox.DamageAmount, hitbox.DamageType);
            Destroy(other.gameObject);
            return;
        }
        
        
        if (invincible)
            return;
            
        if (layerMask == (layerMask | (1 << other.gameObject.layer)))
        {
            OnHurt?.Invoke(hitbox.DamageAmount, hitbox.DamageType);
            if (gameObject.name == "StiltsHurtbox" && hitbox.DamageType == DamageType.EnemyDropStilts)
            {
                hitbox.RequestDisable();
            }

            invincible = true;
            if (other.gameObject.GetComponent<Pie>() != null)
            {
                Destroy(other.gameObject);
            }
            
            
            StartCoroutine(WaitForInvincibility());
        }
    }

    private IEnumerator WaitForInvincibility()
    {
        float totalTime = 0;
        while (true)
        {
            if (meshRenderer != null)
                meshRenderer.enabled = !meshRenderer.enabled;
            if (secondaryMeshRenderer != null)
                secondaryMeshRenderer.enabled = !secondaryMeshRenderer.enabled;
            totalTime += InvincibilityBlinkStep;
            if (totalTime >= invincibilityTime)
            {
                break;
            }

            yield return new WaitForSeconds(InvincibilityBlinkStep);
        }
        if (meshRenderer != null)
            meshRenderer.enabled = true;
        if (secondaryMeshRenderer != null)
            secondaryMeshRenderer.enabled = true;
        invincible = false;
    }
}
