using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using static TimedPowerupEffect;

public class Player : MonoBehaviour
{
    public PickupState pickupState;

    public float speed = 9;
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0, 0, 10 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, 0, -10 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.up * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * speed * Time.deltaTime);
        }

    }


    [PunRPC]
    void UpdatePositionAndRotation(Transform transform)
    {
        gameObject.transform.position = transform.position;
    }

    void OnTriggerEnter(Collider other) {
        switch (other.gameObject.tag) { 
              
        case "Cheese": 
            pickupState.addTimedPowerUpEffect(PowerUpName.Speed, new TimedPowerupEffect());
            break; 
  
        case "Carrot": 
            pickupState.addTimedPowerUpEffect(PowerUpName.Vision, new TimedPowerupEffect());
            break; 
  
        default:
            break; 
        } 
    }
}
