using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera theCam;

    void Start()
    {
        theCam = Camera.main;
    }

    void Update()
    {
        if (theCam == null)
        {
            theCam = Camera.main;
        }
    }

    void LateUpdate()
    {
        if (theCam == null)
            return;

        transform.LookAt(theCam.transform);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }
}
