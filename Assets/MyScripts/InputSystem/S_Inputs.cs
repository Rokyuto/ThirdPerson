using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_Inputs : MonoBehaviour
{
    public bool is_Aim;
    public bool is_Shoot;

    public void OnAim(InputValue value)
    {
        AimInput(value.isPressed); // is Right Mouse Button Pressed
    }

    public void AimInput(bool newAimState)
    {
        is_Aim = newAimState; // Set Is Aiming
    }

    public void OnShoot(InputValue value)
    {
        ShootInput(value.isPressed); // Is Left Mouse Button Pressed
    }

    public void ShootInput(bool newShootState)
    {
        is_Shoot = newShootState; // Set Is Shooting
    }


}
