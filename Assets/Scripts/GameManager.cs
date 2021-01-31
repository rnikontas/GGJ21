using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int seed;
    public bool isReadyToStart;
    public GameObject Player;

    [HideInInspector]
    public int playerId = -1;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        
    }

    void Update()
    {
        CheckIfReadyToStart();
    }

    public void CheckIfReadyToStart()
    {
        if (!PhotonNetwork.IsConnectedAndReady)
            return;

        if (PhotonNetwork.IsMasterClient &&
            PhotonNetwork.CurrentRoom.PlayerCount == 3 )
        {
            isReadyToStart = true;
        }
        else
        {
            isReadyToStart = false;
        }
    }


    public void StartGame()
    {
        seed = Random.Range(0, 1000);
        Debug.Log($"Seed: {seed}");

        if (PhotonNetwork.IsMasterClient)
        {
            RoomNetworking.Instance.roomCanvasUI.BlackoutOtherPlayers();
            PhotonNetwork.LoadLevel("Level");
        }
            
    }
}
