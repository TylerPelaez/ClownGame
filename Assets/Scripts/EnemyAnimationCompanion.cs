using UnityEngine;
using UnityEngine.Events;

public class EnemyAnimationCompanion : MonoBehaviour
{
    public UnityEvent OnDoAttack;
    public UnityEvent OnDeathComplete;

    
    public void DoAttack()
    {
        OnDoAttack?.Invoke();
    }

    public void OnDeath()
    {
        OnDeathComplete?.Invoke();
    }
}
