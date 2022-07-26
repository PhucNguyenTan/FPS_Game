using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Events;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;
    public PlayerInputAction pInputAction;
    public UnityAction AutoShoot;
    public UnityAction SingleShoot;
    public UnityAction HybridCharge;
    public UnityAction HybridShoot;
    public UnityAction HybridCancel;
    public UnityAction HybridChargedShoot;
    public UnityAction Weapon1;
    public UnityAction Weapon2;
    public UnityAction Weapon3;
    public UnityAction Weapon4;
    public UnityAction NextWeapon;
    public UnityAction PrevWeapon;

    public InputAction movementInput { get; private set; }
    public InputAction mouseDelta { get; private set; }
    public InputAction joystickCam { get; private set; }
    public InputAction isShootAuto { get; private set; }

   

    private void Awake()
    {
        pInputAction = new PlayerInputAction();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        GetIsShoot();
    }

    private void OnEnable()
    {
        //pInputAction.Gameplay.Movement.Enable();
        //pInputAction.Gameplay.Jump.Enable();
        //pInputAction.Gameplay.Shoot.Enable();
        //pInputAction.Gameplay.MouseDelta.Enable();
        //pInputAction.Gameplay.Dash.Enable();
        //pInputAction.Gameplay.Crouch.Enable();
        //pInputAction.Gameplay.Cam_Movement.Enable();
        pInputAction.Gameplay.Enable();

        pInputAction.Gameplay.Shoot_single.performed += SingleShootWrapper;
        pInputAction.Gameplay.Shoot_hybrid.started += HybridStartWrapper;
        pInputAction.Gameplay.Shoot_hybrid.performed += HybridShootWrapper;
        pInputAction.Gameplay.Shoot_hybrid.canceled += HybridCancelWrapper;
        pInputAction.Gameplay.Weapon_1.performed += Weapon1Wrapper;
        pInputAction.Gameplay.Weapon_2.performed += Weapon2Wrapper;
        pInputAction.Gameplay.Weapon_3.performed += Weapon3Wrapper;
        pInputAction.Gameplay.Weapon_4.performed += Weapon4Wrapper;
        pInputAction.Gameplay.Next_weapon.performed += NextWeaponWrapper;
        pInputAction.Gameplay.Prev_weapon.performed += PrevWeaponWrapper;

        isShootAuto = pInputAction.Gameplay.Shoot_auto;
        movementInput = pInputAction.Gameplay.Movement;
        mouseDelta = pInputAction.Gameplay.MouseDelta;
        joystickCam = pInputAction.Gameplay.Cam_Movement;
    }

    private void OnDisable()
    {
        //pInputAction.Gameplay.Movement.Disable();
        //pInputAction.Gameplay.Jump.Disable();
        //pInputAction.Gameplay.MousePosition.Disable();
        //pInputAction.Gameplay.Shoot_single.Disable();
        //pInputAction.Gameplay.Dash.Disable();
        //pInputAction.Gameplay.Crouch.Disable();
        //pInputAction.Gameplay.Cam_Movement.Disable();
        pInputAction.Gameplay.Disable();
    }

    private void test(InputAction.CallbackContext obj)
    {
        Debug.Log("Test");
    }

    public Vector2 GetMouseDelta()
    {
        return mouseDelta.ReadValue<Vector2>();
    }

    public Vector2 GetMoveInput()
    {
        return movementInput.ReadValue<Vector2>();
    }

    public Vector2 GetCamInput()
    {
        return joystickCam.ReadValue<Vector2>();
    }

    public void GetIsShoot()
    {
        if (isShootAuto.ReadValue<float>() > 0.5f)
        {
            AutoShoot?.Invoke();
        }
    }

    public void SingleShootWrapper(InputAction.CallbackContext obj)
    {
        SingleShoot?.Invoke();
    }

    public void HybridStartWrapper(InputAction.CallbackContext obj)
    {
        if (obj.interaction is SlowTapInteraction)
            HybridCharge?.Invoke();
    }

    public void HybridShootWrapper(InputAction.CallbackContext obj)
    {
        if (obj.interaction is SlowTapInteraction)
            HybridChargedShoot?.Invoke();
        else
            HybridShoot?.Invoke();
    }

    public void HybridCancelWrapper(InputAction.CallbackContext obj)
    {
        if (obj.interaction is SlowTapInteraction)
            HybridCancel?.Invoke();
    }

    public void Weapon1Wrapper(InputAction.CallbackContext obj)
    {
        Weapon1?.Invoke();
    }
    public void Weapon2Wrapper(InputAction.CallbackContext obj)
    {
        Weapon2?.Invoke();
    }
    public void Weapon3Wrapper(InputAction.CallbackContext obj)
    {
        Weapon3?.Invoke();
    }
    public void Weapon4Wrapper(InputAction.CallbackContext obj)
    {
        Weapon4?.Invoke();
    }
    public void NextWeaponWrapper(InputAction.CallbackContext obj)
    {
        NextWeapon?.Invoke();
    }
    public void PrevWeaponWrapper(InputAction.CallbackContext obj)
    {
        PrevWeapon?.Invoke();
    }
}
