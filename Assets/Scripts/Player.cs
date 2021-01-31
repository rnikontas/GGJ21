using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using static TimedPowerupEffect;

public class Player : MonoBehaviour
{

    private int health = 100;
    public CharacterController characterController;

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
        switch (triggerObject.tag) { 
              
        case "Cheese": 
            characterController.pickupState.addTimedPowerUpEffect(PowerUpName.Speed, new TimedPowerupEffect(3 , 1));
            triggerObject.SetActive(false);
            break; 
  
        case "Carrot": 
            characterController.pickupState.addTimedPowerUpEffect(PowerUpName.Vision, new TimedPowerupEffect());
            triggerObject.SetActive(false);
            break; 

        case "Health": 
            increaseHealth(10);
            triggerObject.SetActive(false);
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
        health -= amount;
        if (health < 0){
            health = 0;
        }

        if (health == 0) {
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }

        return health;
    }
}
