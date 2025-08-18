using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInputHandler))]
public class FPSController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private Transform cameraTransform;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Camera Settings")]
    [SerializeField] private float mouseSensitivity = 2f;

    [Header("Crouch Settings")]
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float standingHeight = 2f;
    [SerializeField] private float crouchCameraYOffset = -0.5f;

    private Vector3 velocity;
    private float xRotation = 0f;
    private Vector3 cameraStartLocalPos;
    private bool isCrouching = false;

    private void Start()
    {
        if (!characterController) characterController = GetComponent<CharacterController>();
        if (!inputHandler) inputHandler = GetComponent<PlayerInputHandler>();
        if (!cameraTransform) Debug.LogError("Camera Transform not assigned!");

        cameraStartLocalPos = cameraTransform.localPosition;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleCrouch();
        HandleMovement();
        HandleCameraRotation();
    }

    private void HandleMovement()
    {
        Vector2 input = inputHandler.MovementInput;
        float speed = inputHandler.SprintTriggered ? sprintSpeed : walkSpeed;

        Vector3 move = transform.right * input.x + transform.forward * input.y;
        characterController.Move(move * speed * Time.deltaTime);

        if (characterController.isGrounded && inputHandler.JumpTriggered)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void HandleCameraRotation()
    {
        Vector2 look = inputHandler.RotationInput;

        float mouseX = look.x * mouseSensitivity;
        float mouseY = look.y * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleCrouch()
    {
        if (inputHandler.CrouchTriggered && !isCrouching)
        {
            characterController.height = crouchHeight;
            cameraTransform.localPosition = cameraStartLocalPos + new Vector3(0f, crouchCameraYOffset, 0f);
            isCrouching = true;
        }
        else if (!inputHandler.CrouchTriggered && isCrouching)
        {
            characterController.height = standingHeight;
            cameraTransform.localPosition = cameraStartLocalPos;
            isCrouching = false;
        }
    }
}
