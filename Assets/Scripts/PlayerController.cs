using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 500f;

    [Header("Ground Check Settings")]
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;

    CameraController cameraController;
    Animator animator;
    CharacterController characterController;

    Quaternion targetRotation;

    bool isGrounded;

    float ySpeed;

    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // variable to check if character is moving. Use Mathf.Abs because even -1 counts as movement.
        // Also clamp these values from 0 to 1 since the moveAmount from the animator object only goes from 0 to 1.
        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));

        // the vector is normalized to keep its direction but its length to 1 unit (magnitude is thrown away). If removed, the player could hold two W and A or W and D and go faster.
        var moveInput = (new Vector3(h, 0, v)).normalized;

        // references the camera's property from CameraController script called PlanarRotation so the player does not move up and down based on camera rotation
        var moveDirection = cameraController.PlanarRotation * moveInput;

        GroundCheck();

        if (isGrounded)
        {
            // if player is on the ground, reset the ySpeed back to normal (which is 0). Instead of this though,
            // doing -0.5 can make sure the player is really on the ground and fall in any case
            ySpeed = -0.5f;
        }
        else
        {
            // increase speed every second of y with the gravity produced by game
            ySpeed += Physics.gravity.y * Time.deltaTime;
        }

        var velocity = moveDirection * moveSpeed;

        velocity.y = ySpeed;

        // deltaTime makes movement framerate independent
        characterController.Move(velocity * Time.deltaTime);


        if (moveAmount > 0)
        {
            // rotates the character when moving to the camera direction
            targetRotation = Quaternion.LookRotation(moveDirection);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        animator.SetFloat("moveAmount", moveAmount, 0.2f, Time.deltaTime);

    }

    // Checks if player is on the ground using physics. We could also use the built-in PlayerController.isGrounded to check, but it seems to be buggy
    void GroundCheck()
    {
        // checks if any colliders are overlapping the sphere (player feet collider)
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
    }

    // Draws a gizmo when the player object is selected so we can visualize the sphere collider
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    }
    
    public void OnDeath()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
