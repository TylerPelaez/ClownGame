using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField]
    private int damageAmount;

    [SerializeField]
    private DamageType damageType;
    
    public int DamageAmount
    {
        get => damageAmount;
    }
    
    public DamageType DamageType
    {
        get => damageType;
    }
}
