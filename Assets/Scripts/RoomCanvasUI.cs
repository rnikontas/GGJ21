using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomCanvasUI : MonoBehaviour
{
    public string roomCode;
    public string playerList;

    public Text roomCodeTextField;
    public Text roomPlayerListField;
    public Button startGameButton;

    public int noOfPlayers;
    public List<GameObject> playerUiIndicators;

    void Start()
    {
        gameObject.SetActive(false);
        for (var i = 0; i < playerUiIndicators.Count; i++)
        {
            playerUiIndicators[i].SetActive(false);
        }
    }

    void Update()
    {
        roomCodeTextField.text = roomCode;
        roomPlayerListField.text = playerList;

        if (GameManager.instance.isReadyToStart)
            startGameButton.gameObject.SetActive(true);
    }

    public void UpdatePlayers()
    {

    }
}
