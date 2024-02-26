using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAnimationCompanion : MonoBehaviour
{
    public UnityEvent OnDoAttack;

    
    public void DoAttack()
    {
        OnDoAttack?.Invoke();
    }
}
