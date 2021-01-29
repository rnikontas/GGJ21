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
        Debug.Log("Command");
        if (command["moveX"] != null)
        {
            moveX = (int)command["moveX"];
            Debug.Log($"moveX: {moveX}");
        }

        if (command["moveZ"] != null)
        {
            moveZ = (int)command["moveZ"];
            Debug.Log($"moveZ: {moveZ}");
        }
    }
}
