using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    //[SerializeField]
    //private InputActionReference inputRef;
    public static PlayerInputAction pInputActrion;
    public static InputAction movementInput { get; private set; }
    public static InputAction mouseDelta { get; private set; }
    public static InputAction joystickCam { get; private set; }

   

    private void Awake()
    {
        pInputActrion = new PlayerInputAction();
    }

    private void OnEnable()
    {
        movementInput = pInputActrion.Gameplay.Movement;
        pInputActrion.Gameplay.Movement.Enable();
        pInputActrion.Gameplay.Jump.Enable();
        pInputActrion.Gameplay.Shoot.Enable();
        mouseDelta = pInputActrion.Gameplay.MouseDelta;
        pInputActrion.Gameplay.MouseDelta.Enable();
        pInputActrion.Gameplay.Dash.Enable();
        pInputActrion.Gameplay.Crouch.Enable();
        pInputActrion.Gameplay.Cam_Movement.Enable();
        joystickCam = pInputActrion.Gameplay.Cam_Movement;
    }

    private void OnDisable()
    {
        pInputActrion.Gameplay.Movement.Disable();
        pInputActrion.Gameplay.Jump.Disable();
        pInputActrion.Gameplay.MousePosition.Disable();
        pInputActrion.Gameplay.Shoot.Disable();
        pInputActrion.Gameplay.Dash.Disable();
        pInputActrion.Gameplay.Crouch.Disable();
        pInputActrion.Gameplay.Cam_Movement.Disable();
    }

    private void test(InputAction.CallbackContext obj)
    {
        Debug.Log("Test");
    }

    public static Vector2 GetMouseDelta()
    {
        return mouseDelta.ReadValue<Vector2>();
    }

    public static Vector2 GetMoveInput()
    {
        return movementInput.ReadValue<Vector2>();
    }

    public static Vector2 GetCamInput()
    {
        return joystickCam.ReadValue<Vector2>();
    }
}
