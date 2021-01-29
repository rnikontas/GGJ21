using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject Player;
    public int PlayerIDMovement = 0;
    public int PlayerIDPuzzle = 1;

    CharacterController m_CharacterController;

    // Start is called before the first frame update
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
        if (m_CharacterController == null)
            m_CharacterController = FindObjectOfType<CharacterController>();
    }

    void OnConnect(int deviceId)
    {
        if (AirConsole.instance.GetControllerDeviceIds().Count == 2)
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

        if (player == PlayerIDMovement)
        {
            m_CharacterController.Command(data);
        }

        if (player == PlayerIDPuzzle)
        {

        }

    }

    void StartGame()
    {
        AirConsole.instance.SetActivePlayers(2);
        Thread.Sleep(2000);
        SceneManager.LoadScene("testLevel");
    }
}
