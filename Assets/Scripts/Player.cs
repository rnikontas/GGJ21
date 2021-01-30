using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Photon.Pun;
using UnityEngine;
using static TimedPowerupEffect;

public class Player : MonoBehaviour
{
    public bool debug;

    public PickupState pickupState;
    #region DEBUG_ONLY
    public float turnSpeed;
    public float moveSpeed;
    #endregion

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
        if (debug)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(0, 0, getMoveSpeed() * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(0, 0, -getMoveSpeed() * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(-Vector3.up * turnSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);
            }
        }
    }


    public float getMoveSpeed()
    {
        var speedBoost = pickupState.getTimedPowerUpEffect(PowerUpName.Speed);
        float speedBoostValue = speedBoost != null ? speedBoost.strength : 0f;
        return moveSpeed + speedBoostValue;
    }

    void OnTriggerEnter(Collider other) {
        var triggerObject = other.gameObject;
        switch (triggerObject.tag) { 
              
        case "Cheese": 
            pickupState.addTimedPowerUpEffect(PowerUpName.Speed, new TimedPowerupEffect(3 , 30));
            triggerObject.SetActive(false);
            break; 
  
        case "Carrot": 
            pickupState.addTimedPowerUpEffect(PowerUpName.Vision, new TimedPowerupEffect());
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
