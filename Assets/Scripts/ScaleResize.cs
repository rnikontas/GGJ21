using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleResize : MonoBehaviour
{
    void Update()
    {
        var sin = Mathf.Sin(Time.time);
        Vector3 vec = new Vector3(Mathf.Clamp(sin + 1, 0, 2), Mathf.Clamp(sin + 1, 2, 2), Mathf.Clamp(sin, 0, 1));

        transform.localScale = vec;
    }

}
