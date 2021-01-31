using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heartbeat : MonoBehaviour
{
    AudioSource audioSource;
    Player player;
    float frequency;
    float timer;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        player = GetComponentInParent<Player>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        ChangeFrequency();
        if (timer >= frequency)
        {
            PlayHeartbeat();
            timer = 0f;
        }

        timer += Time.deltaTime;
    }

    void PlayHeartbeat()
    {
        audioSource.Play();
    }

    void ChangeFrequency()
    {
        frequency = (player.health / 10) * 0.2f;
    }
}
