using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePickupables : MonoBehaviour
{

    public float rotateSpeed;
    public float moveSpeed;

    private float maxYPos;
    private float minYPos;

    void Start()
    {
        maxYPos = transform.position.y + 0.15f;
        minYPos = transform.position.y - 0.15f;
    }

    void Update()
    {
        transform.Rotate(rotateSpeed * Time.deltaTime, rotateSpeed * Time.deltaTime, rotateSpeed * Time.deltaTime, Space.World);
        if ((moveSpeed > 0 && transform.position.y >= maxYPos) || (moveSpeed < 0 && transform.position.y <= minYPos))
        {
            moveSpeed = -moveSpeed;
        }
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
    }
}
