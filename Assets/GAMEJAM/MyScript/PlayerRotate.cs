using System;
using UnityEngine;

namespace GAMEJAM.MyScript
{
    public class PlayerRotate : MonoBehaviour
    {

        public float sensitivity = 80;
        public Transform playerBody;
        float _xRotation = 0f;
        float _rotateY;
        float _rotateX;
        [HideInInspector] GameManager _gameManager;

        void Start()
        {
            _gameManager = GameManager.Instance;
            // gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            if (!_gameManager.brokenRotate)
            {
                _rotateY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
                _rotateX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            }
            else
            {
                _rotateY = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
                _rotateX = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
            }


            _xRotation -= _rotateY;

            _xRotation = Math.Clamp(_xRotation, -80f, 80f);

            playerBody.Rotate(Vector3.up * _rotateX);

            transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        }

    }
}
