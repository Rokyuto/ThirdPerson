using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class S_GameController : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook _thirdPersonCamera; // ThirdPerson Camera Object
    [SerializeField] private CinemachineVirtualCamera _aimVirtualCamera;

    [SerializeField] private LayerMask _InteractionLayerMask = new LayerMask(); // On WHICH Scene Components the ShotTarget can be placed / will Interact
    [SerializeField] private Transform _shootTarget;

    [SerializeField] private Transform _BulletProjectile;
    [SerializeField] private Transform _SpawnBulletPosition;

    private S_Movement s_movement;
    private S_Inputs s_inputs;
    private S_ShootController s_shootController;

    private void Awake()
    {
        s_inputs = GetComponent<S_Inputs>();
        s_movement = GetComponent<S_Movement>();
        s_shootController = GetComponent<S_ShootController>();
    }

    private void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;

        if (s_inputs.is_Aim) // If Aiming
        {
            _thirdPersonCamera.enabled = false;
            _aimVirtualCamera.enabled = true;
            s_movement.turnSmoothTime = 0.5f;

            _shootTarget.gameObject.SetActive(true); // Show Shoot Target 

            // Update Shooting Target Position
            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()); // Create ray at Cursor Postion / Location
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, _InteractionLayerMask))
            {
                _shootTarget.position = raycastHit.point; //Update shootTarget position to raycastHit.point = cursor location

                mouseWorldPosition = raycastHit.point; // Discover Cursor Location (cursor is over shotTarget)
                Func_AimRotation(mouseWorldPosition); // Call Aim ROtation Function
            }

            if (s_inputs.is_Shoot)
            {
                _BulletProjectile.transform.localScale = new Vector3(0.1f, 0.1f, 0.5f);

                Vector3 aimDirection = (mouseWorldPosition - _SpawnBulletPosition.position).normalized;
                Instantiate(_BulletProjectile, _SpawnBulletPosition.position, Quaternion.LookRotation(aimDirection, Vector3.up));
                s_inputs.is_Shoot = false;

            }

        }
        else // If not Aiming
        {
            _shootTarget.gameObject.SetActive(false); // Hide Shoot Target 

            _thirdPersonCamera.enabled = true;
            _aimVirtualCamera.enabled = false;
            s_movement.turnSmoothTime = 0.1f;
        }

    }

    // Rotation while Aiming
    void Func_AimRotation(Vector3 mouseWorldPosition)
    {
        Vector3 worldAimTarget = mouseWorldPosition; // new Aim Rotation
        worldAimTarget.y = transform.position.y; // worldAimTarget Y is EQUAL to CURRENT Camera Y (y = up/down position)
        Vector3 aimDirection = (worldAimTarget - transform.position).normalized; // Angle between current rotation and new rotation

        //Lerp = Interpulation - ?????? ????????? ?? ???? ???????? ? ?????
        //                                last Rotation     new Rotation         step
        transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f); // Rotate Smooth
    }

}
