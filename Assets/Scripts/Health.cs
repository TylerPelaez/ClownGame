using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 1;
    private int health;

    public UnityEvent OnDeath;
    
    private void Start()
    {
        health = maxHealth;
    }

    public void OnHurt(int amount, DamageType type)
    {
        health -= amount;
        if (health < 0)
        {
            OnDeath?.Invoke();
        }
    }
}
