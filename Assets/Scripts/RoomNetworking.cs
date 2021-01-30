using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomNetworking : MonoBehaviourPunCallbacks
{
    public Canvas lobbyCanvas;
    public Canvas roomCanvas;

    string _inputtedRoomCode;

    private void Start()
    {
        DontDestroyOnLoad(this);
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
        var players = PhotonNetwork.CurrentRoom.Players;
        string playerList = "";
        foreach (var player in players)
        {
            playerList += $"{player.Value.NickName}\n";
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        var canvasControl = roomCanvas.GetComponent<RoomCanvasUI>();
        var players = canvasControl.playerList;
        players += $"{newPlayer.NickName} \n";
        Debug.Log($"{newPlayer.NickName}");
        canvasControl.playerList = players;
    }

    public void CreateRoom()
    {
        var roomOptions = new RoomOptions()
        {
            IsVisible = false,
            IsOpen = true,
            MaxPlayers = 2
        };

        var roomCode = Random.Range(1000, 9999).ToString();
        if (PhotonNetwork.CreateRoom(roomCode, roomOptions, TypedLobby.Default))
        {
            lobbyCanvas.gameObject.SetActive(false);
            roomCanvas.gameObject.SetActive(true);
            var roomCanvasUi = roomCanvas.GetComponent<RoomCanvasUI>();
            roomCanvasUi.roomCode = roomCode;
            roomCanvasUi.noOfPlayers++;
            //roomCanvasUi.playerMoveUiIndicator.SetActive(true);
            roomCanvasUi.UpdatePlayers();
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
            roomCanvas.GetComponent<RoomCanvasUI>().roomCode = _inputtedRoomCode;
        }
        else
        {
            // Error handling
        }
    }
}
