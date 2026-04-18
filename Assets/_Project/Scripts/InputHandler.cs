using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public Vector3 MoveInput { get; private set; }
    public bool IsMovementPressed { get; private set; }
    public bool IsJumpPressed { get; private set; }


    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        MoveInput = new Vector3(horizontal, 0f, vertical);
        IsMovementPressed = MoveInput.magnitude > 0.1f;

        IsJumpPressed = Input.GetButtonDown("Jump");
    }
}
