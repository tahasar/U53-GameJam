using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardSway : MonoBehaviour
{
    [SerializeField] float kickBackZ;
    Vector3 targetPosition, currentPosition, initialGunPosition, currentRotation;
    public Transform cam;

   
    public float snappiness, returnAmount;
    public void Start()
    {
        initialGunPosition = transform.localPosition;
       
    }

    void Update()
    {
        KickBack();

    }
    public void KickBack()
    {
        targetPosition = Vector3.Lerp(targetPosition, initialGunPosition, Time.deltaTime * returnAmount);
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, Time.fixedDeltaTime * snappiness);
        transform.localPosition = currentPosition;
    }

    public void Back()
    {

        targetPosition -= new Vector3(0, 0, kickBackZ);
    }

}
