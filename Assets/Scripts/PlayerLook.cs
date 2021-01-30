using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
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

    public void Command(JToken command)
    {

        if ((string)command["data"]["key"] == "right")
        {
            if ((bool)command["data"]["pressed"])
            {
                _lookX = 1.0f * lookSensitivity;
            } else
            {
                _lookX = 0f;
            }


        }
        
        if ((string)command["data"]["key"] == "left")
        {
            if ((bool)command["data"]["pressed"])
            {
                _lookX = -1.0f * lookSensitivity;
            } else
            {
                _lookX = 0;
            }
        }

        if ((string)command["data"]["key"] == "up")
        {
            if ((bool)command["data"]["pressed"])
            {
                _lookY = 1.0f * lookSensitivity;
            } else
            {
                _lookY = 0;
            }


        }
        
        if ((string)command["data"]["key"] == "down")
        {
            if ((bool)command["data"]["pressed"])
            {
                _lookY = -1.0f * lookSensitivity;
            } else
            {
                _lookY = 0;
            }
        }
    }
}
