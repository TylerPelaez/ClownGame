using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 1;

    [SerializeField]
    private int health = 1;
    
    
    public int MaxHealth => maxHealth;

    public int CurrentHealth
    {
        get => health;
        private set => health = value;
    }

    public UnityEvent OnDeath;
    
    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    public void OnHurt(int amount, DamageType type)
    {
        CurrentHealth -= amount;
        if (CurrentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }
}
