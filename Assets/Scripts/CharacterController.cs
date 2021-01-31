using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public UnityEngine.CharacterController controller;
    public float speed;
    public PickupState pickupState;

    void Awake()
    {
        Debug.LogError(GameManager.Instance.playerId);
        pickupState = gameObject.GetComponent<PickupState>();
        if (GameManager.Instance.playerId == 0 || GameManager.Instance.playerId == 2)
        {
            gameObject.GetComponentInChildren<Camera>().enabled = false;
        }

        if (GameManager.Instance.playerId < 2)
        {
            gameObject.GetComponentInChildren<AudioListener>().enabled = false;
        }
    }

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("UpdatePositionAndRotation", RpcTarget.Others, transform.position, transform.rotation);
        }
    }

    [PunRPC]
    void UpdatePositionAndRotation(Vector3 position, Quaternion rotation)
    {
        Debug.LogError($"Position from server: {position.x} {position.y} {position.z}");
        Debug.LogError($"Position from server: {rotation.x} {rotation.y} {rotation.z} {rotation.w}");

        Debug.Log($"Position here: {gameObject.transform.position.x} {gameObject.transform.position.y} {gameObject.transform.position.z}");
        Debug.Log($"Rotation here: {gameObject.transform.rotation.x} {gameObject.transform.rotation.y} {gameObject.transform.rotation.z} {gameObject.transform.rotation.w}");
        gameObject.transform.SetPositionAndRotation(position, rotation);
    }
    
    void Update()
    {
        

        if (!PhotonNetwork.IsMasterClient)
            return;

        var speed = GetMoveSpeed();
        var moveX = Input.GetAxis("Horizontal");
        var moveZ = Input.GetAxis("Vertical");

        var movementDirection = transform.right * moveX + transform.forward * moveZ;
        controller.Move(movementDirection * speed * Time.deltaTime);
    }

    public float GetMoveSpeed()
    {
        if (pickupState == null)
            return speed;

        var speedBoost = pickupState.getTimedPowerUpEffect(TimedPowerupEffect.PowerUpName.Speed);
        float speedBoostValue = speedBoost != null ? speedBoost.strength : 0f;
        return speed + speedBoostValue;
    }

}
