using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIController : MonoBehaviour
{
    public void hostGame()
    {
        SceneManager.LoadScene("Scenes/boot");
    }
    
    public void quitGame()
    {
        Application.Quit();
    }
}
