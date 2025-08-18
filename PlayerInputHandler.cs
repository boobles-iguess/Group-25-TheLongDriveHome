using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInputHandler: MonoBehaviour 
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name Reference")]
    [SerializeField] private string actionMapName = "Player";

    [Header("Action Name References")]
    [SerializeField] private string movement = "Movement";
    [SerializeField] private string rotation = "Rotation";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string sprint = "Sprint";
    [SerializeField] private string crouch = "Crouch";

    private InputAction movementAction;
    private InputAction rotationAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction crouchAction;

    public Vector2 MovementInput { get; private set; }
    public Vector2 RotationInput { get; private set; }
    public bool JumpTriggered { get; private set; }
    public bool SprintTriggered { get; private set; }
    public bool CrouchTriggered { get; private set; }

    private void Awake()
    {
        InputActionMap mapReference = playerControls.FindActionMap (actionMapName);
        movementAction = mapReference.FindAction(movement);
        rotationAction = mapReference.FindAction(rotation);
        jumpAction = mapReference.FindAction(jump);
        sprintAction = mapReference.FindAction(sprint);
        crouchAction = mapReference.FindAction(crouch);
    }
    private void SubscribeActionValuesToInputEvents()
    {
        movementAction.performed += InputInfo => MovementInput = InputInfo.ReadValue<Vector2>();
        movementAction.canceled += InputInfo => MovementInput = Vector2.zero;
       
        rotationAction.performed += InputInfo => RotationInput = InputInfo.ReadValue<Vector2>();
        rotationAction.canceled += InputInfo => RotationInput = Vector2.zero;
        
        jumpAction.performed += inputInfo => JumpTriggered = true;
        jumpAction.canceled += inputInfo => JumpTriggered = false;
        
        sprintAction.performed += InputInfo => SprintTriggered = true;
        sprintAction.canceled += InputInfo => SprintTriggered = false;
        
        crouchAction.performed += InputInfo => CrouchTriggered = true;
        crouchAction.canceled += InputInfo => CrouchTriggered = false;
    }
    private void OnEnable()
    {
        playerControls.FindActionMap(actionMapName).Enable();
    }
    private void OnDisable()
    {
        playerControls.FindActionMap (actionMapName).Disable();
    }
}






