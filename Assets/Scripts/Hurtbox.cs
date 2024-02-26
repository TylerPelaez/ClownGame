using System;
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

    [SerializeField]
    private bool canDoInvincibility = false;
    
    private bool healedThisFrame = false;

    
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
            if (healedThisFrame)
            {
                return;
            }
            
            OnHurt?.Invoke(hitbox.DamageAmount, hitbox.DamageType);
            Destroy(other.gameObject);
            healedThisFrame = true;
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

            if (canDoInvincibility)
            {
                StartCoroutine(WaitForInvincibility());
                invincible = true;
            }
            
            if (other.gameObject.GetComponent<Pie>() != null)
            {
                Destroy(other.gameObject);
            }
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


    private void FixedUpdate()
    {
        healedThisFrame = false;
    }
}
