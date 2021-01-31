using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSpinner : MonoBehaviour
{
    RectTransform transform;
    SpriteRenderer sr;

    void Awake()
    {
        transform = gameObject.GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, 3f, Space.Self);
    }
}
