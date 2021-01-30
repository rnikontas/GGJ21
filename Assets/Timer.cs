using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public double time {get; private set;}

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Timer");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }
}
