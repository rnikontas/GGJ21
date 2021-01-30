using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public UnityEngine.CharacterController controller;
    public float speed = 12f;
    public int moveX;
    public int moveZ;

    void Start()
    {
        
    }

    
    void Update()
    {
        var movementDirection = transform.right * moveX + transform.forward * moveZ;
        controller.Move(movementDirection * speed * Time.deltaTime);
    }


    public void Command(JToken command)
    {
        if ((string)command["data"]["key"] == "right")
        {
            if ((bool)command["data"]["pressed"])
            {
                moveX = 1;
            } else
            {
                moveX = 0;
            }


        }
        
        if ((string)command["data"]["key"] == "left")
        {
            if ((bool)command["data"]["pressed"])
            {
                moveX = -1;
            } else
            {
                moveX = 0;
            }
        }

        if ((string)command["data"]["key"] == "up")
        {
            if ((bool)command["data"]["pressed"])
            {
                moveZ = 1;
            } else
            {
                moveZ = 0;
            }


        }
        
        if ((string)command["data"]["key"] == "down")
        {
            if ((bool)command["data"]["pressed"])
            {
                moveZ = -1;
            } else
            {
                moveZ = 0;
            }
        }
    }
}
