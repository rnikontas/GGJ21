using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class UnlockMouseOnAwake : MonoBehaviour
{
    void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
