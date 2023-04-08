using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float playerSpeed = 10f;
    Vector3 moveVector;
    CharacterController characterControl;

    bool isCrouch;
    bool isGrounded;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundDistance;
    Vector3 velocity;
    public float gravity;
    public float jumpHeight;

    public void Start()
    {
        characterControl = GetComponent<CharacterController>();
    }


    public void Update()
    {
        moveVector = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        characterControl.Move(moveVector * playerSpeed * Time.deltaTime);

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
     

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -15f;
        }
        velocity.y += gravity * Time.deltaTime;

        characterControl.Move(velocity * Time.deltaTime);


        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouch)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }


    }
}
