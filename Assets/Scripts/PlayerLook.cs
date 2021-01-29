using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public float lookSensitivity = 50f;

    public Transform playerBody;

    float m_xRotation;
    float lookX = 0f;
    float lookY = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //float lookX = 1 * lookSensitivity * Time.deltaTime;
        //float lookY = 1 * lookSensitivity * Time.deltaTime;


        m_xRotation -= lookY;
        m_xRotation = Mathf.Clamp(m_xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(m_xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * lookX);

    }

    public void Command(JToken command)
    {

        if ((string)command["data"]["key"] == "right")
        {
            if ((bool)command["data"]["pressed"])
            {
                lookX = 1.0f;
            } else
            {
                lookX = 0f;
            }


        }
        
        if ((string)command["data"]["key"] == "left")
        {
            if ((bool)command["data"]["pressed"])
            {
                lookX = -1.0f;
            } else
            {
                lookX = 0;
            }
        }

        if ((string)command["data"]["key"] == "up")
        {
            if ((bool)command["data"]["pressed"])
            {
                lookY = 1.0f;
            } else
            {
                lookY = 0;
            }


        }
        
        if ((string)command["data"]["key"] == "down")
        {
            if ((bool)command["data"]["pressed"])
            {
                lookY = -1.0f;
            } else
            {
                lookY = 0;
            }
        }
    }
}
