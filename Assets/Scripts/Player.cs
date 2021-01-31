using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using static TimedPowerupEffect;

public class Player : MonoBehaviour
{

    public int health = 100;
    public CharacterController characterController;
    public GameObject hitEnemyScream;
    public GameObject hitEnemy;

    void Awake()
    {
        characterController = gameObject.GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other) {
        var triggerObject = other.gameObject;
        var audioSource = triggerObject.GetComponent<AudioSource>();
        var clipLength = audioSource.clip.length;

        switch (triggerObject.tag) { 
              
        case "Cheese": 
            characterController.pickupState.addTimedPowerUpEffect(PowerUpName.Speed, new TimedPowerupEffect(3 , 1));
            other.enabled = false;
            triggerObject.GetComponent<MeshRenderer>().enabled = false;
            audioSource.Play();
            Destroy(triggerObject, clipLength);
            break; 
  
        case "Carrot": 
            characterController.pickupState.addTimedPowerUpEffect(PowerUpName.Vision, new TimedPowerupEffect());
            other.enabled = false;
            triggerObject.GetComponent<MeshRenderer>().enabled = false;
            audioSource.Play();
            Destroy(triggerObject, clipLength);
            break; 

        case "Health": 
            increaseHealth(100);
            other.enabled = false;
            triggerObject.GetComponent<MeshRenderer>().enabled = false;
            audioSource.Play();
            Destroy(triggerObject, clipLength);
            break; 
  
        default:
            break; 
        } 
    }

    public int increaseHealth(int amount) {
        health += amount;
        if (health > 100){
            health = 100;
        }
        return health;
    }

    public int reduceHealth(int amount) {
        Debug.LogError($"HP: {health}");
        health -= amount;
        var audioSource = hitEnemy.GetComponent<AudioSource>();
        if (!audioSource.isPlaying)
        {
            audioSource.pitch = Random.Range(0.90f, 1.1f);
            audioSource.Play();
        }
        if (health < 0){
            health = 0;
        }

        if (health == 0) {
            PhotonNetwork.LoadLevel("GameOver");
        }

        return health;
    }

    public void PlayHitScream()
    {
        hitEnemyScream.GetComponent<AudioSource>().Play();
    }
}
