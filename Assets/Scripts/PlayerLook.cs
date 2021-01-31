using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public float lookSensitivity;

    public Transform playerBody;

    float _xRotation;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GetComponent<AudioListener>().enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }


    void Update()
    {
        if (!PhotonNetwork.IsMasterClient || PauseController.isPaused)
            return;

        var lookX = Input.GetAxis("Horizontal") * lookSensitivity * 2f * Time.deltaTime;
        //var lookY = Input.GetAxis("Mouse Y") * lookSensitivity * Time.deltaTime;

        //_xRotation -= lookY;
        //_xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        playerBody.Rotate(Vector3.up * lookX);

    }
}
