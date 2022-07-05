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
    [SerializeField]
    private Player player;

   

    private void Awake()
    {
        pInputActrion = new PlayerInputAction();
    }

    private void OnEnable()
    {
        movementInput = pInputActrion.Gameplay.Movement;
        pInputActrion.Gameplay.Movement.Enable();
        //pInputActrion.Gameplay.Jump.performed += test;
        pInputActrion.Gameplay.Jump.Enable();
        pInputActrion.Gameplay.Shoot.Enable();
        mouseDelta = pInputActrion.Gameplay.MouseDelta;
        pInputActrion.Gameplay.MouseDelta.Enable();
        pInputActrion.Gameplay.Dash.Enable();
        pInputActrion.Gameplay.Crouch.Enable();
    }

    private void OnDisable()
    {
        pInputActrion.Gameplay.Movement.Disable();
        pInputActrion.Gameplay.Jump.Disable();
        pInputActrion.Gameplay.MousePosition.Disable();
        pInputActrion.Gameplay.Shoot.Disable();
        pInputActrion.Gameplay.Dash.Disable();
        pInputActrion.Gameplay.Crouch.Disable();
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
}
