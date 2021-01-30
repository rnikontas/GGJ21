using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePickupables : MonoBehaviour
{

    public float rotateSpeed;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, rotateSpeed, 0f, Space.World);
    }
}
