using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardRecoil : MonoBehaviour
{
    Vector3 _currentRotation, _targetRotation, _targetPosition, _currentPosition, _initialGunPosition;
    public Transform cam;

    [SerializeField] float recoilX;
    [SerializeField] float recoilY;
    [SerializeField] float recoilZ;
    [SerializeField] float aimRecoilX;
    [SerializeField] float aimRecoilY;
    [SerializeField] float aimRecoilZ;


    public float snappiness, returnAmount;

    public void Start()
    {
        _initialGunPosition = transform.localPosition;
    }

    public void Update()
    {
        

        _targetRotation = Vector3.Lerp(_targetRotation, Vector3.zero, returnAmount * Time.deltaTime);
        _currentRotation = Vector3.Slerp(_currentRotation, _targetRotation, snappiness * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(_currentRotation);
    }

    public void Recoil()
    {

       _targetRotation += new Vector3(recoilX, UnityEngine.Random.Range(-recoilY, recoilY), UnityEngine.Random.Range(-recoilZ, recoilZ));


    }


}
