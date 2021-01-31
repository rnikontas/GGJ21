using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public GameObject pauseMenuPanel;

    public static bool isPaused;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            isPaused = true;
            pauseMenuPanel.SetActive(true);
        }
    }

    public void Resume()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        isPaused = false;
        pauseMenuPanel.SetActive(false);
    }

    public void Exit()
    {
        //TODO: Ramojus zadejo visus ismest i main menu sita paspaudus
    }
}
