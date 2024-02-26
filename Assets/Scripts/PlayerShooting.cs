using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    PieGun pieGunScript;

    [SerializeField]
    private GameObject piePrefab;

    [SerializeField]
    private float shootCooldown = 1f;
    
    private float lastFiredTime;

    private void Start()
    {
        pieGunScript = GetComponent<PieGun>();
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && (Time.time - lastFiredTime) > shootCooldown && pieGunScript.isPieGunVisible)
        {
            var instance = Instantiate(piePrefab, transform.position, Quaternion.identity);
            lastFiredTime = Time.time;
            instance.transform.forward = transform.forward;
            FindObjectOfType<AudioManager>().Play("PieThrow");
        }
    }
}
