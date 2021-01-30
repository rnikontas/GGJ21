using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public float lookSensitivity = 1.15f;

    public Transform playerBody;

    float _xRotation;
    float _lookX = 0f;
    float _lookY = 0f;


    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GetComponent<AudioListener>().enabled = false;
        }
    }


    void Update()
    {
        //float lookX = 1 * lookSensitivity * Time.deltaTime;
        //float lookY = 1 * lookSensitivity * Time.deltaTime;


        _xRotation -= _lookY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * _lookX);

    }
}
