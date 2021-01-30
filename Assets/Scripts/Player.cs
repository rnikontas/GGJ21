using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Photon.Pun;
using UnityEngine;
using static TimedPowerupEffect;

public class Player : MonoBehaviour
{

    public int health = 100;
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
            health += 10;
            triggerObject.SetActive(false);
            break; 
  
        default:
            break; 
        } 
    }
}
