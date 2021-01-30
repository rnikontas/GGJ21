using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observe : MonoBehaviour
{
    GameObject player;
    Camera cam;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        cam = player.GetComponent<Camera>();
    }

    void Update()
    {
        Vector3 screenPoint = cam.WorldToViewportPoint(transform.position);
        if (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1)
        {
            Ray ray = cam.ScreenPointToRay(screenPoint);
            RaycastHit hit;

            Physics.Raycast(ray, out hit);
            if (hit.transform.gameObject.name == "Player")
            {
                Debug.Log("isboserve");
            }

        }
        else
        {
            //currentGazeTimeInSeconds = 0;
        }
    }
}
