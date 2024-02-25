using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StiltController : MonoBehaviour
{
    // Track time since last scaling operation
    private float lastScaledTime;

    // Define cooldown duration in seconds
    public float cooldownDuration = 0.35f;

    public GameObject clown;
    public GameObject ground;

    void Start()
    {
        // Set lastScaledTime to current time to avoid immediate scaling
        lastScaledTime = Time.time - cooldownDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastScaledTime >= cooldownDuration)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                ScaleStiltsDown();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ScaleStiltsUp();
            }
        }       
    }

    void ScaleStiltsUp()
    {
        Vector3 currentScale = transform.localScale;

        if (currentScale.y < 3f)
        {
            // Scale up the stilts by adding 1 to the y-scale
            currentScale.y = Mathf.Min(currentScale.y + 1f, 3f);

            // Apply the new scale to the stilts
            transform.localScale = currentScale;

            // Update lastScaledTime
            lastScaledTime = Time.time;

            UpdateClownPosition();
        }
        
    }

    void ScaleStiltsDown()
    {
        Vector3 currentScale = transform.localScale;

        if (currentScale.y > 0f)
        {
            // Scale down the stilts by subtracting 1 from the y-scale
            currentScale.y = Mathf.Max(currentScale.y - 1f, 0f);

            // Apply the new scale to the stilts
            transform.localScale = currentScale;

            // Update lastScaledTime
            lastScaledTime = Time.time;

            UpdateClownPosition();
        }
        
    }

    void UpdateClownPosition()
    {
        // Define the desired position offset for the clown avatar
        float desiredYOffset = -2.5f;

        // Calculate the actual position offset based on the scale of the stilts
        float actualYOffset = desiredYOffset * (3f - transform.localScale.y) / 3f;

        // Apply the position offset to the clown avatar
        clown.transform.localPosition = new Vector3(clown.transform.localPosition.x, actualYOffset, clown.transform.localPosition.z);

        // Calculate the desired position offset for the stilts to touch the ground
        float groundOffset = 0.5f * transform.localScale.y;

        // Apply the position offset to the stilts
        transform.position = new Vector3(transform.position.x, ground.transform.position.y + groundOffset, transform.position.z);
    }
}

