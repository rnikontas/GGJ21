using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreenButtons : MonoBehaviour
{
    public void Home()
    {
        SceneManager.LoadScene("Scenes/UITestScene");
    }
}
