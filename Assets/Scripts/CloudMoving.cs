using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMoving : MonoBehaviour
{
    public float moveSpeed;
    public Transform startPos;
    public Transform endPos;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(-moveSpeed * Time.deltaTime, 0, 0));
        if (transform.position.x <= endPos.position.x)
        {
            transform.position = new Vector3(startPos.position.x, transform.position.y, transform.position.z);
        }
    }
}
