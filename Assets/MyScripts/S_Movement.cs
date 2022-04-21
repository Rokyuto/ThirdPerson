using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Movement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    private void Start()
    {
        cam.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void Update()
    {
        // Value is in range [ -1 : 1 ]
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // Get Horizontal Raw Input
        float verticalInput = Input.GetAxisRaw("Vertical"); // Get Vertical Raw Input
        //                                  x:           y:     z:           || We don't want to move by Y - UP
        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput).normalized; // Normalized to don't move faster when it's pressed 2 keys

        if(direction.magnitude >= 0.1f) // If moving in any direction
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y; // Find Angle in Degrees between current Player Possition, direction and Camera Rotation
            //                                      current rotation    direction Angle       Turn Velocity      Turn Time                                 
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f) ; // Rotate Player

            //                      rotaion:                            direction:
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; // Movement Direction
            controller.Move(moveDir * speed * Time.deltaTime); // Frame Rate Independent
        }
    } 
}
