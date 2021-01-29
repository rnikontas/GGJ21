using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public float lookSensitivity = 50f;

    public Transform playerBody;

    float m_xRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //float lookX = 1 * lookSensitivity * Time.deltaTime;
        //float lookY = 1 * lookSensitivity * Time.deltaTime;
        float lookX = 0f;
        float lookY = 0f;

        m_xRotation -= lookY;
        m_xRotation = Mathf.Clamp(m_xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(m_xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * lookX);

    }
}
