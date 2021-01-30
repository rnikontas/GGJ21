using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
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
            PhotonNetwork.CurrentRoom.PlayerCount == 2 )
        {
            isReadyToStart = true;
        }
    }


    public void StartGame()
    {
        seed = Random.Range(0, 100);
        Debug.Log($"Seed: {seed}");

        if (PhotonNetwork.IsMasterClient) 
            PhotonNetwork.LoadLevel("KubolioDevScena");
    }
}
