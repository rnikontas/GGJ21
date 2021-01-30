using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public UnityEngine.CharacterController controller;
    public float speed = 12f;
    public int moveX;
    public int moveZ;

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
        var movementDirection = transform.right * moveX + transform.forward * moveZ;
        controller.Move(movementDirection * speed * Time.deltaTime);
    }

}
