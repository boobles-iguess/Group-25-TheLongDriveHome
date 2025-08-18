using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class Flashlight : MonoBehaviour
{

    [Header("Flashlight Settings")]
    public GameObject flashlight; // assign in inspector
    private bool isFlashlightOn = false; //flashlight is set to off


    public void OnFlashlight(InputAction.CallbackContext context)
    {
        if (context.performed) //if button is pressed
        {
            isFlashlightOn = !isFlashlightOn; // Toggles between on and off
            flashlight.SetActive(true); // turns flashlight on
        }
        else if (context.canceled) // if button released
        {
            flashlight.SetActive(false); // turns flashlight off
        }
    }
}
