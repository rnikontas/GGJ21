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
    public int seed;
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

        CheckIfReadyToStart();
            
    }

    public void CheckIfReadyToStart()
    {
        if (PhotonNetwork.IsMasterClient &&
            PhotonNetwork.CurrentRoom.PlayerCount == 2 &&
            AirConsole.instance.GetControllerDeviceIds().Count == 1)
        {
            isReadyToStart = true;
        }
    }

    public void OnConnect(int deviceId)
    {
        CheckIfReadyToStart();
        FindObjectOfType<RoomCanvasUI>().indicatorMovement.SetActive(true);
    }

    public void OnDisconnect(int deviceId)
    {

    }

    public void OnMessage(int deviceId, JToken data)
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
        seed = Random.Range(0, 100);
        Debug.Log($"Seed: {seed}");

        AirConsole.instance.SetActivePlayers(1);
        if (PhotonNetwork.IsMasterClient) 
            PhotonNetwork.LoadLevel("KubolioDevScena");
    }
}
