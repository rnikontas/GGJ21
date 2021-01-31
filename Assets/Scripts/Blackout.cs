using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackout : MonoBehaviour
{
    public Camera cam;
    void Awake()
    {
        if (GameManager.Instance.playerId == 0 || GameManager.Instance.playerId == 2)
        {
            gameObject.SetActive(true);
        }

        cam = Camera.main;

    }


    void Start()
    {
        
    }


    void Update()
    {
        if (cam == null)
        {
            Debug.LogError("Camera null");
            cam = Camera.main;
            var canvas = GetComponentInParent<Canvas>();
            canvas.worldCamera = cam;
            
            if (GameManager.Instance.playerId == 0 || GameManager.Instance.playerId == 2)
            {
                Camera.main.enabled = false;
            }
        }
            
    }
}
