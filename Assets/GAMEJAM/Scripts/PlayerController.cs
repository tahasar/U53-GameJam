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
    bool isGrounded;
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
        if (isGrounded && !gameManager.brokenDirection)
        {
            moveVector = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        }
        else if (isGrounded && gameManager.brokenDirection)
        {
            moveVector = Input.GetAxis("Vertical") * transform.right + Input.GetAxis("Horizontal") * transform.forward;
        }
        characterControl.Move(moveVector * (playerSpeed * Time.deltaTime));

        //if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)|| Input.GetKey(KeyCode.W))
        //{
        //    Debug.Log("araba");
        //}


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
