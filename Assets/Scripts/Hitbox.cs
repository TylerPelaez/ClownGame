using UnityEngine;
using UnityEngine.Events;

public class Hitbox : MonoBehaviour
{
    [SerializeField]
    private int damageAmount;

    [SerializeField]
    private DamageType damageType;

    public UnityEvent OnDisableRequested;


    public int DamageAmount
    {
        get => damageAmount;
    }
    
    public DamageType DamageType
    {
        get => damageType;
    }

    public void RequestDisable()
    {
        OnDisableRequested?.Invoke();
    }
}
