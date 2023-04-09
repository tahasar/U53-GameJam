using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{

    public float Sensitivity = 80;
    public Transform playerBody;
    float xRotation = 0f;
    float rotateY;
    float rotateX;
    [HideInInspector] GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;
        // gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!gameManager.brokenRotate)
        {
            rotateY = Input.GetAxis("Mouse Y") * Sensitivity * Time.deltaTime;
            rotateX = Input.GetAxis("Mouse X") * Sensitivity * Time.deltaTime;
        }
        else
        {
            rotateY = Input.GetAxis("Mouse X") * Sensitivity * Time.deltaTime;
            rotateX = Input.GetAxis("Mouse Y") * Sensitivity * Time.deltaTime;
        }


        xRotation -= rotateY;

        xRotation = Math.Clamp(xRotation, -80f, 80f);

        playerBody.Rotate(Vector3.up * rotateX);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    public void RotateFix()
    {
        
    }
}
