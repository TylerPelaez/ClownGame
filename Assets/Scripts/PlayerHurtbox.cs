using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerHurtbox : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;
    
    public UnityEvent<int, DamageType> OnHurt;

    public void OnTriggerEnter(Collider other)
    {
        if (layerMask == (layerMask | (1 << other.gameObject.layer)))
        {
            Debug.Log($"Playerinvoke {other.gameObject.name}");

            // TODO: not hardcode this
            OnHurt?.Invoke(1, DamageType.Stomp);
        }
    }

    public void OnDeath()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
