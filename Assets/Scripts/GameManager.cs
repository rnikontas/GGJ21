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

    public int playerId;

    CharacterController _CharacterController;
    PlayerLook _PlayerLook;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
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
            PhotonNetwork.LoadLevel("Level");
    }
}
