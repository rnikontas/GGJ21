using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomNetworking : MonoBehaviourPunCallbacks
{
    public static RoomNetworking Instance;
    public Canvas lobbyCanvas;
    public Canvas roomCanvas;
    RoomCanvasUI roomCanvasUI;

    string _inputtedRoomCode;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        DontDestroyOnLoad(this);
        roomCanvasUI = roomCanvas.GetComponent<RoomCanvasUI>();
    }

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.OfflineMode = false;
            PhotonNetwork.GameVersion = "1";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = $"Player#{Random.Range(1, 999)}";
    }

    public override void OnJoinedRoom()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            roomCanvasUI.indicatorMovement.SetActive(true);

            if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
                roomCanvasUI.indicatorVision.SetActive(true);

            if (PhotonNetwork.CurrentRoom.PlayerCount == 3)
                roomCanvasUI.indicatorSound.SetActive(true);
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.LogError($"Joined: {newPlayer.NickName}");
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
            roomCanvasUI.indicatorVision.SetActive(true);

        if (PhotonNetwork.CurrentRoom.PlayerCount == 3)
            roomCanvasUI.indicatorSound.SetActive(true);

        if (PhotonNetwork.IsMasterClient)
            GameManager.Instance.CheckIfReadyToStart();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player leftPlayer)
    {
        Debug.LogError($"Left: {leftPlayer.NickName}");
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            roomCanvasUI.indicatorVision.SetActive(false);

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            roomCanvasUI.indicatorSound.SetActive(false);

        if (PhotonNetwork.IsMasterClient)
            GameManager.Instance.CheckIfReadyToStart();
    }

    public void CreateRoom()
    {
        var roomOptions = new RoomOptions()
        {
            IsVisible = false,
            IsOpen = true,
            MaxPlayers = 3
        };

        var roomCode = Random.Range(1000, 9999).ToString();
        if (PhotonNetwork.CreateRoom(roomCode, roomOptions, TypedLobby.Default))
        {
            lobbyCanvas.gameObject.SetActive(false);
            roomCanvas.gameObject.SetActive(true);

            roomCanvasUI.roomCode = roomCode;
            roomCanvasUI.indicatorMovement.SetActive(true);
            GameManager.Instance.playerId = 0;
        }
        else
        {
            // Error handling
        }
    }

    public void Input_RoomCode(string value)
    {
        _inputtedRoomCode = value;
    }

    public void JoinRoom()
    {
        if (PhotonNetwork.JoinRoom(_inputtedRoomCode))
        {
            lobbyCanvas.gameObject.SetActive(false);
            roomCanvas.gameObject.SetActive(true);
            roomCanvasUI.roomCode = _inputtedRoomCode;
            roomCanvasUI.waitingGameObject.SetActive(true);
        }
        else
        {
            // Error handling
        }
    }
}
