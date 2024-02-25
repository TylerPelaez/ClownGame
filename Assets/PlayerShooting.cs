using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField]
    private GameObject piePrefab;

    [SerializeField]
    private float shootCooldown = 1f;
    
    private float lastFiredTime;
    
    private void Update()
    {
        if (Input.GetButton("Fire1") && (Time.time - lastFiredTime) > shootCooldown)
        {
            var instance = Instantiate(piePrefab, transform.position, Quaternion.identity);
            lastFiredTime = Time.time;
            instance.transform.forward = transform.forward;
        }
    }
}
