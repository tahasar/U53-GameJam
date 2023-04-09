using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerController : MonoBehaviour
{
    public UnityEngine.Animations.Rigging.Rig handIK;

    public Transform weaponParent;
    public Transform weaponLeftGrip;
    public Transform weaponRightGrip;

    public Animator rigController;

    [SerializeField] public float playerSpeed = 10f;
    Vector3 moveVector;
    Vector3 deneme;
    CharacterController characterControl;

    bool isCrouch;
    public bool isGrounded;
    public bool isJump;
    public bool isMove;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundDistance;
    Vector3 velocity;
    public float gravity;
    public float jumpHeight;

    [HideInInspector] GameManager gameManager;

    public void Start()
    {
        characterControl = GetComponent<CharacterController>();
        gameManager = GameManager.instance;
    }


    public void Update()
    {
        if (!gameManager.brokenDirection && isGrounded)
        {
            moveVector = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;

        }
        else if (gameManager.brokenDirection && isGrounded)
        {
            moveVector = Input.GetAxis("Vertical") * transform.right + Input.GetAxis("Horizontal") * transform.forward;

        }

        characterControl.Move(moveVector * (playerSpeed * Time.deltaTime));

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            isMove = true;
            rigController.SetBool("Move", true);
        }
        else
        {
            rigController.SetBool("Move", false);
            isMove = false;
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);


        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -15f;
        }
        velocity.y += gravity * Time.deltaTime;

        characterControl.Move(velocity * Time.deltaTime);

        if (isGrounded && Input.GetKeyDown(KeyCode.LeftShift) && isMove)
        {
            Debug.Log("xd");
            rigController.SetBool("Run", true);
            playerSpeed = 8;
        }
        if (isGrounded && Input.GetKeyUp(KeyCode.LeftShift) || isJump)
        {
            rigController.SetBool("Run", false);
            playerSpeed = 5;
        }

        if (Input.GetButtonDown("Jump") && isGrounded &&!isJump)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        if (!isGrounded)
        {
            isJump = true;
            rigController.SetBool("Jump", true);
        }
        else
        {
            rigController.SetBool("Jump", false);
            isJump = false;
        }

    }

    [ContextMenu("Save Weapon Pose")]
    public void SaveWeaponPose()
    {
        GameObjectRecorder recorder = new GameObjectRecorder(gameObject);
        recorder.BindComponentsOfType<Transform>(weaponParent.gameObject, false);
        recorder.BindComponentsOfType<Transform>(weaponLeftGrip.gameObject, false);
        recorder.BindComponentsOfType<Transform>(weaponRightGrip.gameObject, false);
        recorder.BindComponentsOfType<Transform>(gameObject, false);
        UnityEditor.AssetDatabase.SaveAssets();
        recorder.TakeSnapshot(0.0f);
        

    }
}
