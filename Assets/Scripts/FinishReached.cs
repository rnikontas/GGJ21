using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishReached : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Victory");
        }
    }
}
