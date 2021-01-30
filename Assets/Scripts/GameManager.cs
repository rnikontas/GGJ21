using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isReadyToStart;
    public GameObject Player;

    CharacterController _CharacterController;
    PlayerLook _PlayerLook;

    void Awake()
    {
        if (instance == null)
            instance = this;

        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_CharacterController == null)
        {
            _CharacterController = FindObjectOfType<CharacterController>();
            _PlayerLook = FindObjectOfType<PlayerLook>();
        }
            
    }

    void CheckIfReadyToStart()
    {
        if (PhotonNetwork.IsMasterClient &&
            PhotonNetwork.CurrentRoom.PlayerCount == 3)
        {
            isReadyToStart = true;
        }
    }

    void OnConnect(int deviceId)
    {
        if (AirConsole.instance.GetControllerDeviceIds().Count == 1)
            StartGame();
    }

    void OnDisconnect(int deviceId)
    {

    }

    void OnMessage(int deviceId, JToken data)
    {
        int player = AirConsole.instance.ConvertDeviceIdToPlayerNumber(deviceId);
        if (player < 0)
            return;

        if ((string)data["element"] == "view-0-section-3-element-0")
        {
                _CharacterController.Command(data);
        }

        if ((string)data["element"] == "view-0-section-4-element-0")
        {
                _PlayerLook.Command(data);
        }

    }


    public void StartGame()
    {
        AirConsole.instance.SetActivePlayers(1);
        if (PhotonNetwork.IsMasterClient) 
            PhotonNetwork.LoadLevel("testLevel");
    }
}
