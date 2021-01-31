using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomNotFoundText : MonoBehaviour
{
    public float alpha = 0f;
    int r = 255;
    int g = 23;
    int b = 23;

    Text text;

    void Awake()
    {
        text = GetComponent<Text>();
        text.color = new Color(r / 255f, g / 255f, b / 255f, alpha);
    }

    void Start()
    {
        
    }

    void Update()
    {
        alpha = Mathf.Clamp(alpha - Time.deltaTime, 0f, 1f);
        text.color = new Color(r / 255f, g / 255f, b / 255f, alpha);
    }
}
