using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class StiltController : MonoBehaviour
{
    Transform leftStilt;
    Transform rightStilt;
    CameraController cameraController;

    private float lastScaledTime;
    public float cooldownDuration = 0.28f;

    public GameObject clown;
    public GameObject ground;


    [SerializeField]
    private float clownMeshYOffset = 2.9f;
    
    [SerializeField]
    private BoxCollider stompHitbox;


    [SerializeField]
    private float tweenTime;

    public UnityEvent<bool> ShowHidePieGun;


    private Tweener tweener;
    
    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    void Start()
    {
        // Set lastScaledTime to current time to avoid immediate scaling
        lastScaledTime = Time.time - cooldownDuration;

        leftStilt = transform.Find("LeftStilt");
        rightStilt = transform.Find("RightStilt");
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
        var currentScale = transform.localScale;

        // Enable the mesh renderers of both stilts
        leftStilt.GetComponent<MeshRenderer>().enabled = true;
        rightStilt.GetComponent<MeshRenderer>().enabled = true;
        stompHitbox.enabled = true;
        
        if (currentScale.y < 3f)
        {
            // Scale up the stilts by adding 1 to the y-scale
            currentScale.y = Mathf.Min(currentScale.y + 1f, 3f);

            DoScale(currentScale);

            FindObjectOfType<AudioManager>().Play("StiltUp");
            FindObjectOfType<AudioManager>().Play("StiltStretch");
        }
    }

    public void ScaleStiltsDown()
    {
        var currentScale = transform.localScale;

        if (currentScale.y > 0f)
        {
            // Scale down the stilts by subtracting 1 from the y-scale
            currentScale.y = Mathf.Max(currentScale.y - 1f, 0f);

            DoScale(currentScale);

            FindObjectOfType<AudioManager>().Play("StiltDown");
            FindObjectOfType<AudioManager>().Play("StiltStretch");
        }
    }

    private void DoScale(Vector3 newScale)
    {
        if (tweener != null && tweener.active)
        {
            tweener.Complete();
        }
        
        tweener = DOTween.To(() => transform.localScale, StiltTween, newScale, tweenTime);
        tweener.SetEase(Ease.OutExpo);
        tweener.onComplete += OnTweenComplete;
    }

    private void StiltTween(Vector3 newScale)
    {
        // Apply the new scale to the stilts
        transform.localScale = newScale;
        UpdateClownPosition();
    }

    private void OnTweenComplete()
    {
        // Update lastScaledTime
        lastScaledTime = Time.time;
        if (transform.localScale.y <= 0)
        {
            // Disable the mesh renderers of both stilts
            leftStilt.GetComponent<MeshRenderer>().enabled = false;
            rightStilt.GetComponent<MeshRenderer>().enabled = false;
            stompHitbox.enabled = false;
        }
    }
    
    
    void UpdateClownPosition()
    {
        // Define the desired position offset for the clown avatar
        float desiredYOffset = -3f;

        // Calculate the actual position offset based on the scale of the stilts
        float actualYOffset = desiredYOffset * (3f - transform.localScale.y) / 3f;

        // Apply the position offset to the clown avatar
        clown.transform.localPosition = new Vector3(clown.transform.localPosition.x, actualYOffset + clownMeshYOffset, clown.transform.localPosition.z);
    }
}

