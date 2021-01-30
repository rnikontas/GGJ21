using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class RoomCanvasUI : MonoBehaviour
{
    public string roomCode;
    public string playerList;

    public Text roomCodeTextField;
    public Button startGameButton;

    public int noOfPlayers;
    public GameObject indicatorMovement;
    public GameObject indicatorVision;
    public GameObject indicatorSound;

    public GameObject waitingGameObject;

    void Start()
    {
        gameObject.SetActive(false);
        indicatorMovement.SetActive(false);
        indicatorVision.SetActive(false);
        indicatorSound.SetActive(false);
        waitingGameObject.SetActive(false);
    }

    void Update()
    {
        roomCodeTextField.text = roomCode;

        if (GameManager.instance.isReadyToStart &&
            PhotonNetwork.IsMasterClient)
        {
            startGameButton.gameObject.SetActive(true);
        }
            
    }
}
