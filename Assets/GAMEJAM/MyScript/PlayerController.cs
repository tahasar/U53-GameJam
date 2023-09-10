using UnityEditor.Animations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GAMEJAM.MyScript
{
    public class PlayerController : MonoBehaviour
    {
        [Header ("Rig")]
        public UnityEngine.Animations.Rigging.Rig handIK;
        public Transform weaponParent;
        public Transform weaponLeftGrip;
        public Transform weaponRightGrip;
        public Animator rigController;

        [Header("Move")]
        [SerializeField] public float playerSpeed = 10f;
        Vector3 _moveVector;
        CharacterController _characterControl;
        [HideInInspector] PlayerHealth _playerHealth;
        bool _isCrouch;
        public bool isMove;
        public bool isRun;

        [Header("Ground")]
        public bool isGrounded;
        public Transform groundCheck;
        public LayerMask groundMask;
        public float groundDistance;
        Vector3 velocity;
        public float gravity;

        [Header("Jump")]
        public bool isJump;
        public float jumpHeight;
        public AudioClip[] jumpSounds;
        private new AudioSource _audio;

        [HideInInspector] GameManager _gameManager;
        private static readonly int Move = Animator.StringToHash("Move");
        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int Jump = Animator.StringToHash("Jump");

        public void Start()
        {
            _characterControl = GetComponent<CharacterController>();
            _gameManager = GameManager.Instance;
            _audio = GetComponent<AudioSource>();
            _playerHealth = GetComponent<PlayerHealth>();
      
        }


        public void Update()
        {

            if (!_gameManager.brokenDirection && isGrounded)
            {
                _moveVector = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
            }
            else if (isGrounded && _gameManager.brokenDirection)
            {
                _moveVector = Input.GetAxis("Vertical") * transform.right + Input.GetAxis("Horizontal") * transform.forward;
            }
            else if (_gameManager.brokenDirection && isGrounded)
            {
                _moveVector = Input.GetAxis("Vertical") * transform.right + Input.GetAxis("Horizontal") * transform.forward;
            }

            _characterControl.Move(_moveVector * (playerSpeed * Time.deltaTime));

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                isMove = true;
                rigController.SetBool(Move, true);
            }
            else
            {
                rigController.SetBool(Move, false);
                isMove = false;
            }
       
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);


            if (isGrounded && velocity.y < 0)
            {
                velocity.y = gravity;
            }
            velocity.y += gravity * Time.deltaTime;

            _characterControl.Move(velocity * Time.deltaTime);

            if (isGrounded && Input.GetKeyDown(KeyCode.LeftShift) && isMove)
            {
                rigController.SetBool(Run, true);
                playerSpeed = 8;
                isRun = true;
            }
            if (isGrounded && Input.GetKeyUp(KeyCode.LeftShift) || isJump)
            {
                rigController.SetBool(Run, false);
                playerSpeed = 5;
                isRun = false;
            }

            if (Input.GetButtonDown("Jump") && isGrounded && !isJump)
            {
                _audio.clip = jumpSounds[Random.Range(0, 3)];
                _audio.Play();
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            
            }
            if (!isGrounded)
            {
                isJump = true;
                rigController.SetBool(Jump, true);
           
            }
            else
            {
                rigController.SetBool(Jump, false);
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
}
