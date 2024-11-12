using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]

public class SC_FPSController : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    public AudioClip walkingSound;
    public AudioClip runningSound;

    private CharacterController characterController;
    private AudioSource audioSource;
    private Animator animator;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        // Handle sound effects
        HandleMovementSound(curSpeedX, curSpeedY, isRunning);

        // Update animator parameters
        float speed = new Vector3(characterController.velocity.x, 0, characterController.velocity.z).magnitude;
        animator.SetFloat("Speed", speed);
        animator.SetBool("IsRunning", isRunning);
        animator.SetBool("isIdle", speed == 0);
    }

    void HandleMovementSound(float curSpeedX, float curSpeedY, bool isRunning)
    {
        if (characterController.isGrounded && (curSpeedX != 0 || curSpeedY != 0))
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = isRunning ? runningSound : walkingSound;
                audioSource.Play();
            }
            else if (audioSource.clip == walkingSound && isRunning)
            {
                audioSource.clip = runningSound;
                audioSource.Play();
            }
            else if (audioSource.clip == runningSound && !isRunning)
            {
                audioSource.clip = walkingSound;
                audioSource.Play();
            }
        }
        else if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
