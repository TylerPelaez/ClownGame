using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform followTarget;

    [SerializeField] float distance = 5;

    [SerializeField] float sensitivity = 1.0f;

    [SerializeField] float minVerticalAngle = -25;
    [SerializeField] float maxVerticalAngle = 50;

    public Vector2 framingOffset;

    [SerializeField] bool invertX;
    [SerializeField] bool invertY;

    float rotationY;
    float rotationX;

    float invertXVal;
    float invertYVal;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        invertXVal = (invertX) ? -1 : 1;
        invertYVal = (invertY) ? -1 : 1;

        rotationY += sensitivity * invertXVal * Input.GetAxis("Camera X");
        rotationX += sensitivity * invertYVal * Input.GetAxis("Camera Y");

        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);

        var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);

        var focusPosition = followTarget.position + new Vector3(framingOffset.x, framingOffset.y);

        transform.position = focusPosition - targetRotation * new Vector3(0, 0, distance);

        transform.rotation = targetRotation;
    }

    public Quaternion PlanarRotation => Quaternion.Euler(0, rotationY, 0); // this is a property. The "=>" symbol points to the return value of the property
}
